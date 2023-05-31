namespace InfoSearch.Core.Indexes;

public interface IIncidenceMatrix
{
    bool[] GetDocumentIncidence(string term);
    string[] GetDocumentNames(bool[] incidence);
}
