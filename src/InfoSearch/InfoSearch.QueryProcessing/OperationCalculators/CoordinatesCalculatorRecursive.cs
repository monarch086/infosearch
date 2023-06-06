namespace InfoSearch.QueryProcessing.OperationCalculators;

internal class CoordinatesCalculatorRecursive
{
    public static IEnumerable<int> Intersection(IDictionary<int, IList<int>> coordinatesA, IDictionary<int, IList<int>> coordinatesB, int distance = 1)
    {
        var documents = coordinatesA.Keys.Intersect(coordinatesB.Keys);

        var result = new HashSet<int>();

        foreach (var document in documents)
        {
            var list1 = coordinatesA[document];
            var list2 = coordinatesB[document];

            Console.WriteLine($"list1.Count(): {list1.Count()}, list2.Count(): {list2.Count()}");

            Scan(list1, list2, 0, 0, distance, document, result);
        }

        return result;
    }

    private static void Scan(IList<int> list1, IList<int> list2, int ind1, int ind2, int distance, int document, ISet<int> result)
    {
        if (ind1 == list1.Count() || ind2 == list2.Count())
            return;

        if (list1[ind1] < list2[ind2])
        {
            if (list2[ind2] - list1[ind1] <= distance)
            {
                result.Add(document);
                Scan(list1, list2, ind1 + 1, ind2 + 1, distance, document, result);
            }
            else Scan(list1, list2, ind1 + 1, ind2, distance, document, result);
        }
        else
        {
            Scan(list2, list1, ind2, ind1, distance, document, result);
        }
    }
}
