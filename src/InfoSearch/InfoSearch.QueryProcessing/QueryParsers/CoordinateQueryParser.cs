using InfoSearch.Core;
using InfoSearch.Core.Extensions;
using InfoSearch.QueryProcessing.Model;

namespace InfoSearch.QueryProcessing.QueryParsers;

public class CoordinateQueryParser : IQueryParser<CoordinateQueryComponent>
{
    private const string DISTANCE_SIGN = "/d=";

    public IQuery<CoordinateQueryComponent> Parse(string queryString)
    {
        if (!queryString.Contains(DISTANCE_SIGN))
        {
            throw new ArgumentException($"No distance sign was found in the query. It should be in the format: {DISTANCE_SIGN}5");
        }

        var terms = queryString.SplitTerms();

        if (terms.Length == 1)
        {
            throw new ArgumentException("There should be at least two terms in query.");
        }

        var query = new CoordinateQuery();
        var component = new CoordinateQueryComponent();

        foreach (var item in terms)
        {
            if (item.StartsWith(DISTANCE_SIGN))
                component.Distance = GetDistance(item);
            else
            {
                component.Term = item;
                query.Components.Add(component);
                component = new CoordinateQueryComponent();
            }
        }

        return query;
    }

    private int GetDistance(string queryPart)
    {
        var distPart = queryPart.Split(DISTANCE_SIGN);

        int.TryParse(distPart[1], out var distance);

        return distance;
    }
}
