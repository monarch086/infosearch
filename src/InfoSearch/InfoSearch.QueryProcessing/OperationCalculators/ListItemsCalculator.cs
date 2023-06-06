using InfoSearch.QueryProcessing.Model.BooleanQuery;

namespace InfoSearch.QueryProcessing.OperationCalculators;

internal static class ListItemsCalculator
{
    public static IEnumerable<int> Calculate(BoolOperator operation, IList<int> listA, IList<int> listB)
    {
        switch (operation)
        {
            case BoolOperator.AND:
                return listA.Intersect(listB);

            case BoolOperator.OR:
                var resultList = new List<int>();
                resultList.AddRange(listA);
                resultList.AddRange(listB);
                return resultList;

            case BoolOperator.NOT:
                return listA.Except(listB);

            default: throw new NotImplementedException("Operator not implemented");
        }
    }
}
