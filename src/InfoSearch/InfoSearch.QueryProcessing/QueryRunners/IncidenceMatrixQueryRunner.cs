using InfoSearch.Core;
using InfoSearch.Core.Indexes;
using InfoSearch.QueryProcessing.Model;

namespace InfoSearch.QueryProcessing.QueryRunners;

public class IncidenceMatrixQueryRunner : IQueryRunner
{
    private readonly IIncidenceMatrix _index;

    public IncidenceMatrixQueryRunner(IIncidenceMatrix index)
    {
        _index = index;
    }

    public IEnumerable<string> Run(Query query)
    {
        var firstTerm = query.Components[0].Term;
        var resultDocumentIncidence = _index.GetDocumentIncidence(firstTerm);

        if (query.Components.Count() == 1)
            return _index.GetDocumentNames(resultDocumentIncidence);

        for (int i = 1; i < query.Components.Count(); i++)
        {
            var operation = query.Components[i].Operator
                ?? throw new ArgumentNullException($"Operator for term {query.Components[i].Term} is not provided.");
            var currentDocumentIncidence = _index.GetDocumentIncidence(query.Components[i].Term);

            resultDocumentIncidence = BitwiseCalculator.Calculate(operation, resultDocumentIncidence, currentDocumentIncidence);
        }

        return _index.GetDocumentNames(resultDocumentIncidence);
    }
}
