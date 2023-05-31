using InfoSearch.Core;
using InfoSearch.Core.Indexes;
using InfoSearch.QueryProcessing.Model;
using InfoSearch.QueryProcessing.OperationCalculators;

namespace InfoSearch.QueryProcessing.QueryRunners;

public class InvertedIndexQueryRunner : IQueryRunner
{
    private readonly InvertedListIndex _index;

    public InvertedIndexQueryRunner(InvertedListIndex index)
    {
        _index = index;
    }

    public IEnumerable<string> Run(Query query)
    {
        var firstTerm = query.Components[0].Term;
        var resultDocumentList = _index.GetDocumentList(firstTerm);

        if (query.Components.Count() == 1)
            return _index.GetDocumentNames(resultDocumentList);

        for (int i = 1; i < query.Components.Count(); i++)
        {
            var operation = query.Components[i].Operator
                ?? throw new ArgumentNullException($"Operator for term {query.Components[i].Term} is not provided.");
            var currentDocumentList = _index.GetDocumentList(query.Components[i].Term);

            resultDocumentList = ListItemsCalculator.Calculate(operation, resultDocumentList, currentDocumentList).ToList();
        }

        return _index.GetDocumentNames(resultDocumentList);
    }
}
