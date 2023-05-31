using InfoSearch.Core.Model.Query;

namespace InfoSearch.QueryProcessing;

public static class QueryParser
{
    public static Query Parse(string query)
    {
        var separators = new char[] { ' ', ',' };
        var operators = new string[] { "AND", "OR", "NOT" };
        var parsedQuery = new Query();

        try
        {
            var elements = query.Split(separators);
            var queryComponent = new QueryComponent();

            for (int i = 0; i < elements.Length; i++)
            {
                if (operators.Contains(elements[i]))
                    queryComponent.Operator = Enum.Parse<Operator>(elements[i]);
                else
                    queryComponent.Term = elements[i].ToLower().Trim();

                var firstComponentIsReady = i == 0 && !string.IsNullOrEmpty(queryComponent.Term);
                var otherComponentsAreReady = queryComponent.Operator.HasValue && !string.IsNullOrEmpty(queryComponent.Term);

                if (firstComponentIsReady || otherComponentsAreReady)
                {
                    parsedQuery.Components.Add(queryComponent);
                    queryComponent = new QueryComponent();
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
