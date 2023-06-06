namespace InfoSearch.QueryProcessing.OperationCalculators;

internal class CoordinatesCalculator
{
    public static IEnumerable<int> Intersection(IDictionary<int, IList<int>> coordinatesA, IDictionary<int, IList<int>> coordinatesB, int distance = 1)
    {
        var documents = coordinatesA.Keys.Intersect(coordinatesB.Keys);

        var result = new HashSet<int>();

        foreach ( var document in documents )
        {
            var list1 = coordinatesA[document];
            var list2 = coordinatesB[document];

            for (int i = 0, j = 0; i < list1.Count() && j < list2.Count();)
            {
                if (list1[i] < list2[j])
                    if (i == list1.Count() - 1 || list1[i + 1] > list2[j])
                    {
                        if (list2[j] - list1[i] <= distance || i < list1.Count() - 1 && list1[i + 1] - list2[j] <= distance)
                        {
                            result.Add(document);
                        }
                        i++;
                        j++;
                    }
                    else
                    {
                        i++;
                    }
                else
                {
                    if (j == list2.Count() - 1 || list2[j + 1] > list1[i])
                    {
                        if (list1[i] - list2[j] <= distance || j < list2.Count() - 1 && list2[j + 1] - list1[i] <= distance)
                        {
                            result.Add(document);
                        }
                        i++;
                        j++;
                    }
                    else
                    {
                        j++;
                    }
                }
            }
        }

        return result;
    }
}
