using InfoSearch.Core.Model;

namespace InfoSearch.Core.Indexes;

public class InvertedListIndex
{
    private IDictionary<string, IList<int>> _index;
    private IList<string> _documentNames;

    public InvertedListIndex(IEnumerable<Document> documents)
    {
        _index = new Dictionary<string, IList<int>>();
        _documentNames = new List<string>();

        foreach (var document in documents)
        {
            _documentNames.Add(document.Name);
            var documentIndex = _documentNames.Count() - 1;

            var terms = document.Text.SplitTerms();
            var termsSet = new HashSet<string>(terms);

            foreach (var term in termsSet)
            {
                if (!_index.ContainsKey(term))
                    _index.Add(term, new List<int>());

                _index[term].Add(documentIndex);
            }
        }
    }

    public int[] GetDocumentList(string term) =>
        _index.ContainsKey(term) ? _index[term].ToArray() : Array.Empty<int>();

    public string[] GetDocumentNames(int[] list) =>
        list.Select(e => _documentNames[e]).ToArray();
}
