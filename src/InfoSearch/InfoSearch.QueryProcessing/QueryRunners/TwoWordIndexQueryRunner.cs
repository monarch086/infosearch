using InfoSearch.Core;
using InfoSearch.Core.Indexes;

namespace InfoSearch.QueryProcessing.QueryRunners;

public class TwoWordIndexQueryRunner : IQueryRunner<WordPair>
{
    private readonly TwoWordIndex _index;

    public TwoWordIndexQueryRunner(TwoWordIndex index)
    {
        _index = index;
    }

    public IEnumerable<string> Run(IQuery<WordPair> query)
    {
        Console.WriteLine($"Word pairs: {string.Join(", ", query.Components)}");

        var documentResultSets = new List<IEnumerable<int>>();

        foreach (var pair in query.Components)
        {
            var result = _index.GetDocumentList(pair);

            Console.WriteLine($"Found document list for pair {pair}: {string.Join(", ", result)}");

            documentResultSets.Add(result);
        }

        var mergedSet = new List<int>();
        foreach (var set in documentResultSets)
        {
            if (mergedSet.Count() == 0)
                mergedSet.AddRange(set);
            else mergedSet = mergedSet.Intersect(set).ToList();
        }

        return _index.GetDocumentNames(mergedSet);
    }
}
