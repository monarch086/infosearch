using InfoSearch.QueryProcessing.Model.BooleanQuery;

namespace InfoSearch.QueryProcessing.OperationCalculators;

internal static class BitwiseCalculator
{
    public static bool[] Calculate(BoolOperator operation, bool[] incidenceA, bool[] incidenceB)
    {
        if (incidenceA.Length != incidenceB.Length)
            throw new ArgumentException("Provided incidence arrays should have the same length.");

        var result = new bool[incidenceA.Length];

        switch (operation)
        {
            case BoolOperator.AND:
                for (int i = 0; i < incidenceA.Count(); i++)
                {
                    result[i] = incidenceA[i] & incidenceB[i];
                }
                return result;

            case BoolOperator.OR:
                for (int i = 0; i < incidenceA.Count(); i++)
                {
                    result[i] = incidenceA[i] | incidenceB[i];
                }
                return result;

            case BoolOperator.NOT:
                for (int i = 0; i < incidenceA.Count(); i++)
                {
                    result[i] = incidenceA[i] & !incidenceB[i];
                }
                return result;

            default: throw new NotImplementedException("Operator not implemented");
        }
    }
}
