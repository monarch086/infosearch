using InfoSearch.Core;
using InfoSearch.QueryProcessing.Model.BooleanQuery;

namespace InfoSearch.QueryProcessing.QueryParsers;

public class BoolQueryParser : IQueryParser<BoolQueryComponent>
{
    public IQuery<BoolQueryComponent> Parse(string query)
    {
        var separators = new char[] { ' ', ',' };
        var operators = new string[] { "AND", "OR", "NOT" };
        var parsedQuery = new BoolQuery();

        try
        {
            var elements = query.Split(separators);
            var queryComponent = new BoolQueryComponent();

            for (int i = 0; i < elements.Length; i++)
            {
                if (operators.Contains(elements[i]))
                    queryComponent.Operator = Enum.Parse<BoolOperator>(elements[i]);
                else
                    queryComponent.Term = elements[i].ToLower().Trim();

                var firstComponentIsReady = i == 0 && !string.IsNullOrEmpty(queryComponent.Term);
                var otherComponentsAreReady = queryComponent.Operator.HasValue && !string.IsNullOrEmpty(queryComponent.Term);

                if (firstComponentIsReady || otherComponentsAreReady)
                {
                    parsedQuery.Components.Add(queryComponent);
                    queryComponent = new BoolQueryComponent();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Query parsing error: " + ex.ToString());
        }

        return parsedQuery;
    }
}
