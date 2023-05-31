using InfoSearch.Core.Model;

namespace InfoSearch.Core.Indexes;

internal class DynamicIncidenceMatrix : IIncidenceMatrix
{
    private IDictionary<string, IList<bool>> _matrix;
    private IList<string> _documentNames;

    public DynamicIncidenceMatrix()
    {
        _matrix = new Dictionary<string, IList<bool>>();
        _documentNames = new List<string>();
    }

    public void AddDocument(Document document)
    {
        var terms = document.Text.SplitTerms();

        foreach (var term in terms)
        {
            if (!_matrix.ContainsKey(term))
            {
                _matrix.Add(term, GetEmptyRow());
            }

            _matrix[term].Add(true);
        }

        foreach (var key in _matrix.Keys)
        {
            if (_matrix[key].Count() == _documentNames.Count())
            {
                _matrix[key].Add(false);
            }
        }

        _documentNames.Add(document.Name);
    }

    public bool[] GetDocumentIncidence(string term)
    {
        return _matrix.ContainsKey(term) ? _matrix[term].ToArray()
                                         : GetEmptyRow().ToArray();
    }

    public string[] GetDocumentNames(bool[] incidence)
    {
        var resultDocs = new List<string>();

        for (int i = 0; i < incidence.Count(); i++)
            resultDocs.Add(_documentNames[i]);

        return resultDocs.ToArray();
    }

    private IList<bool> GetEmptyRow()
    {
        return _documentNames.Select(_ => false).ToList();
    }
}
