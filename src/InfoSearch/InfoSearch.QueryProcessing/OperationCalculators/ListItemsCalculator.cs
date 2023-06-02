using InfoSearch.QueryProcessing.Model;

namespace InfoSearch.QueryProcessing.OperationCalculators;

internal static class ListItemsCalculator
{
    public static IEnumerable<int> Calculate(Operator operation, IList<int> listA, IList<int> listB)
    {
        switch (operation)
        {
            case Operator.AND:
                return listA.Intersect(listB);

            case Operator.OR:
                var resultList = new List<int>();
                resultList.AddRange(listA);
                resultList.AddRange(listB);
                return resultList;

            case Operator.NOT:
                return listA.Except(listB);

            default: throw new NotImplementedException("Operator not implemented");
        }
    }
}
