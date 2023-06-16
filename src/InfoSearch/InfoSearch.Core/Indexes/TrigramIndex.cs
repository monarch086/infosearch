using InfoSearch.Core.Extensions;
using InfoSearch.Core.Model;
using System.Reflection.Metadata;

namespace InfoSearch.Core.Indexes;

public class TrigramIndex
{
    private readonly IDictionary<string, ISet<int>> _index;
    private readonly IList<string> _documentNames;

    public TrigramIndex(IEnumerable<Model.Document> documents)
    {
        _index = new Dictionary<string, ISet<int>>();
        _documentNames = new List<string>();

        foreach (var document in documents)
        {
            _documentNames.Add(document.Name);
            var documentIndex = _documentNames.Count() - 1;
            var terms = document.Text.SplitTerms();

            foreach (var term in terms)
            {
                var trigrams = GetTrigrams(term);

                foreach (var trigram in trigrams)
                {
                    if (_index.ContainsKey(trigram)) _index[trigram].Add(documentIndex);
                    else _index.Add(trigram, new HashSet<int>() { documentIndex });
                }
            }
        }
    }

    public IEnumerable<int> GetDocumentList(string query)
    {
        var terms = query.SplitTerms();
        var documents = new List<int>();
        var initializedList = false;

        foreach (var term in terms)
        {
            var trigrams = GetTrigrams(term);

            foreach (var trigram in trigrams)
            {
                var trigramDocs = _index.ContainsKey(trigram) ? _index[trigram] : new HashSet<int>();
                
                if (!initializedList)
                {
                    documents.AddRange(trigramDocs);
                    initializedList = true;
                }
                else
                {
                    documents = documents.Intersect(trigramDocs).ToList();
                }
            }
        }

        return documents;
    }

    public string[] GetDocumentNames(IEnumerable<int> list) =>
        list.Select(e => _documentNames[e]).ToArray();

    private IList<string> GetTrigrams(string term)
    {
        var trigrams = new List<string>();

        if (term.Length == 1)
        {
            trigrams.Add($"  {term}");
            trigrams.Add($"{term}  ");
            return trigrams;
        }

        if (term.Length == 2)
        {
            trigrams.Add($" {term}");
            trigrams.Add($"{term} ");
            return trigrams;
        }

        for (int i = 0; i < term.Length; i++)
        {
            if (i == 0)
            {
                trigrams.Add($" {term[i]}{term[i + 1]}");
            }
            else if (i == term.Length - 1)
            {
                trigrams.Add($"{term[i - 1]}{term[i]} ");
            }
            else
            {
                trigrams.Add($"{term[i - 1]}{term[i]}{term[i + 1]}");
            }
        }

        return trigrams;
    }
}
