using InfoSearch.Core;
using InfoSearch.Core.Extensions;
using InfoSearch.Core.Indexes;
using InfoSearch.QueryProcessing.Model;

namespace InfoSearch.QueryProcessing.QueryParsers;

public class PairQueryParser : IQueryParser<WordPair>
{
    public IQuery<WordPair> Parse(string queryString)
    {
        var terms = queryString.SplitTerms();
        if (terms.Length == 1)
        {
            throw new ArgumentException("There should be at least two terms in query.");
        }

        var query = new TwoWordQuery();

        query.Components = terms.ToPairs();

        return query;
    }
}
