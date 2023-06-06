using InfoSearch.Core.Extensions;
using InfoSearch.Core.Model;

namespace InfoSearch.Core.Indexes;

public class TwoWordIndex
{
    private IDictionary<WordPair, IList<int>> _index;
    private IList<string> _documentNames;

    public TwoWordIndex(IEnumerable<Document> documents)
    {
        _index = new Dictionary<WordPair, IList<int>>();
        _documentNames = new List<string>();

        foreach (var document in documents)
        {
            _documentNames.Add(document.Name);
            var documentIndex = _documentNames.Count() - 1;

            var terms = document.Text.SplitTerms();
            var previousTerm = string.Empty;

            foreach (var term in terms)
            {
                if (string.IsNullOrEmpty(previousTerm))
                {
                    previousTerm = term;
                    continue;
                }

                var key = new WordPair(previousTerm, term);

                if (!_index.ContainsKey(key))
                    _index.Add(key, new List<int>());

                _index[key].Add(documentIndex);

                previousTerm = term;
            }
        }
    }

    public IEnumerable<int> GetDocumentList(WordPair pair) =>
        _index.ContainsKey(pair) ? _index[pair] : Array.Empty<int>();

    public string[] GetDocumentNames(IList<int> list) =>
        list.Select(e => _documentNames[e]).ToArray();
}
