using InfoSearch.Core.Extensions;
using InfoSearch.Core.Indexes;

namespace InfoSearch.QueryProcessing.QueryRunners;

public class TwoWordIndexQueryRunner
{
    private readonly TwoWordIndex _index;

    public TwoWordIndexQueryRunner(TwoWordIndex index)
    {
            _index = index;
    }

    public IEnumerable<string> Run(string query)
    {
        var terms = query.SplitTerms();
        if (terms.Length == 1)
        {
            throw new ArgumentException("There should be at least two terms in query.");
        }

        var pairs = terms.ToPairs();

        Console.WriteLine($"Word pairs: {string.Join(", ", pairs)}");

        var documentResultSets = new List<IEnumerable<int>>(terms.Length - 1);

        foreach (var pair in pairs)
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
