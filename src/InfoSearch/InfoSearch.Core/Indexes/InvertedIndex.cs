using InfoSearch.Core.Model;

namespace InfoSearch.Core.Indexes;

public class InvertedIndex
{
    private IDictionary<string, IList<int>> _matrix;
    private IList<string> _documentNames;

    public InvertedIndex(IEnumerable<Document> documents)
    {
        _matrix = new Dictionary<string, IList<int>>();
        _documentNames = new List<string>();

        foreach (var document in documents)
        {
            _documentNames.Add(document.Name);

            var terms = document.Text.SplitTerms();
            var termsSet = new HashSet<string>(terms);

           // _matrix.ContainsKey()
        }
    }
}
