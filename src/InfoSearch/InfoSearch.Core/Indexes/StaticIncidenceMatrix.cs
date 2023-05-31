using InfoSearch.Core.Model;

namespace InfoSearch.Core.Indexes;

public class StaticIncidenceMatrix : IIncidenceMatrix
{
    private IDictionary<string, bool[]> _matrix;
    private IList<string> _documentNames;

    public StaticIncidenceMatrix(IEnumerable<Document> documents)
    {
        _matrix = new Dictionary<string, bool[]>();
        _documentNames = new List<string>();

        var totalSet = new HashSet<string>();
        var documentsMap = new Dictionary<string, ISet<string>>();

        foreach (var document in documents)
        {
            var terms = document.Text.SplitTerms();
            var documentSet = new HashSet<string>();

            foreach (string term in terms)
            {
                totalSet.Add(term);
                documentSet.Add(term);
            }

            documentsMap.Add(document.Name, documentSet);
            _documentNames.Add(document.Name);
        }

        foreach (var term in totalSet)
        {
            _matrix.Add(term, new bool[_documentNames.Count()]);

            for (int i = 0; i < _documentNames.Count(); i++)
            {
                var doumentName = _documentNames[i];
                _matrix[term][i] = documentsMap[doumentName].Contains(term);
            }
        }
    }

    public bool[] GetDocumentIncidence(string term) =>
        _matrix.ContainsKey(term) ? _matrix[term] : GetEmptyRow();

    public string[] GetDocumentNames(bool[] incidence)
    {
        var resultDocs = new List<string>();

        for (int i = 0; i < incidence.Count(); i++)
            if (incidence[i])
                resultDocs.Add(_documentNames[i]);

        return resultDocs.ToArray();
    }

    private bool[] GetEmptyRow() =>
        _documentNames.Select(_ => false).ToArray();
}
