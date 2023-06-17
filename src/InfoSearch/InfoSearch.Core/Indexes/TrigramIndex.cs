using InfoSearch.Core.Extensions;
using InfoSearch.Core.Model;

namespace InfoSearch.Core.Indexes;

public class TrigramIndex
{
    private readonly IDictionary<string, ISet<int>> _index;
    private readonly IList<string> _termsList;

    private const char JOKER_SIGN = '*';

    public TrigramIndex(IEnumerable<Document> documents)
    {
        _index = new Dictionary<string, ISet<int>>();
        _termsList = new List<string>();
        var termsSet = new HashSet<string>();

        foreach (var document in documents)
        {
            var terms = document.Text.SplitTerms();

            foreach (var term in terms)
            {
                if (termsSet.Contains(term)) continue;

                termsSet.Add(term);
                _termsList.Add(term);

                var termIndex = _termsList.Count() - 1;

                var trigrams = GetTrigrams($"${term}$");

                foreach (var trigram in trigrams)
                {
                    if (_index.ContainsKey(trigram)) _index[trigram].Add(termIndex);
                    else _index.Add(trigram, new HashSet<int>() { termIndex });
                }
            }
        }
    }

    public IEnumerable<string> GetTerms(string query)
    {
        if (query.Count(c => c == JOKER_SIGN) != 1)
            throw new ArgumentException($"Input query should contain one joker sign ({JOKER_SIGN}).");

        var queryParts = query.Split(JOKER_SIGN);

        var terms = new List<int>();
        var initializedList = false;

        foreach (var part in queryParts)
        {
            var trigrams = part == queryParts[0] ? GetTrigrams($"${part}") : GetTrigrams($"{part}$");

            foreach (var trigram in trigrams)
            {
                Console.WriteLine("trigram: " + trigram);

                var trigramTerms = _index.ContainsKey(trigram) ? _index[trigram] : new HashSet<int>();
                
                if (!initializedList)
                {
                    terms.AddRange(trigramTerms);
                    initializedList = true;
                }
                else
                {
                    terms = terms.Intersect(trigramTerms).ToList();
                }
            }
        }

        return terms.Select(e => _termsList[e]).ToArray();
    }

    private IList<string> GetTrigrams(string term)
    {
        var trigrams = new List<string>();

        if (term == "$")
        {
            return trigrams;
        }

        if (term.Length <= 2)
        {
            trigrams.Add($"{term}");
            return trigrams;
        }

        for (int i = 1; i < term.Length - 1; i++)
        {
            trigrams.Add($"{term[i - 1]}{term[i]}{term[i + 1]}");
        }

        return trigrams;
    }
}
