using InfoSearch.Core;
using InfoSearch.Core.Indexes;
using InfoSearch.QueryProcessing.Model;
using InfoSearch.QueryProcessing.OperationCalculators;

namespace InfoSearch.QueryProcessing.QueryRunners;

public class CoordinateIndexQueryRunner : IQueryRunner<CoordinateQueryComponent>
{
    private readonly CoordinateIndex _index;

    public CoordinateIndexQueryRunner(CoordinateIndex index)
    {
        _index = index;
    }

    public IEnumerable<string> Run(IQuery<CoordinateQueryComponent> query)
    {
        Console.WriteLine($"Query components: {string.Join(", ", query.Components)}");

        var documentIndexes = new List<int>();
        IDictionary<int, IList<int>> prevCoordinates = new Dictionary<int, IList<int>>();

        foreach (var component in query.Components)
        {
            var coordinates = _index.GetTermCoordinates(component.Term);
            Console.WriteLine($"Documents for term \'{component.Term}\': {string.Join(", ", coordinates.Keys)}");

            if (component == query.Components.FirstOrDefault())
            {
                documentIndexes.AddRange(coordinates.Keys);
                prevCoordinates = coordinates;
                continue;
            }

            // Here CoordinatesCalculatorRecursive can be used
            var intersectDocs = CoordinatesCalculator.Intersection(prevCoordinates, coordinates, component.Distance);

            documentIndexes = documentIndexes.Intersect(intersectDocs).ToList();
            Console.WriteLine($"Intersected docs: {string.Join(", ", documentIndexes)}");

            prevCoordinates = coordinates;
        }

        return _index.GetDocumentNames(documentIndexes);
    }
}
