using InfoSearch.Core.Extensions;
using InfoSearch.Core.Model;

namespace InfoSearch.Core.Indexes;

public class CoordinateIndex
{
    private IDictionary<string, IDictionary<int, IList<int>>> _index;
    private IList<string> _documentNames;

    public CoordinateIndex(IEnumerable<Document> documents)
    {
        _index = new Dictionary<string, IDictionary<int, IList<int>>>();
        _documentNames = new List<string>();

        foreach (var document in documents)
        {
            _documentNames.Add(document.Name);
            var documentIndex = _documentNames.Count() - 1;

            var terms = document.Text.SplitTerms();

            for (int i = 0; i < terms.Length; i++)
            {
                if (_index.ContainsKey(terms[i]))
                {
                    if (_index[terms[i]].ContainsKey(documentIndex))
                    {
                        // Add coordinate to doc
                        _index[terms[i]][documentIndex].Add(i);
                    }
                    else
                    {
                        // Add doc with coordinate
                        _index[terms[i]].Add(documentIndex, new List<int>{ i });
                    }
                }
                else
                {
                    // Add term with doc with coord
                    _index.Add(terms[i], new Dictionary<int, IList<int>>
                    {
                        { documentIndex, new List<int> { i } }
                    });
                }
            }
        }
    }

    public IDictionary<int, IList<int>> GetTermCoordinates(string term)
    {
        return _index.ContainsKey(term) ? _index[term] : new Dictionary<int, IList<int>>();
    }

    public string[] GetDocumentNames(IEnumerable<int> docIndexes) =>
        docIndexes.Select(i => _documentNames[i]).ToArray();
}
