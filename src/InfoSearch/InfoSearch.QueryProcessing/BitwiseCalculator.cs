using InfoSearch.Core.Model.Query;

namespace InfoSearch.QueryProcessing;

public static class BitwiseCalculator
{
    public static bool[] Calculate(Operator operation, bool[] incidenceA, bool[] incidenceB)
    {
        if (incidenceA.Length != incidenceB.Length)
            throw new ArgumentException("Provided incidence arrays should have the same length.");

        var result = new bool[incidenceA.Length];

        switch (operation)
        {
            case Operator.AND:
                for (int i = 0; i < incidenceA.Count(); i++)
                {
                    result[i] = incidenceA[i] & incidenceB[i];
                }
                return result;

            case Operator.OR:
                for (int i = 0; i < incidenceA.Count(); i++)
                {
                    result[i] = incidenceA[i] | incidenceB[i];
                }
                return result;

            case Operator.NOT:
                for (int i = 0; i < incidenceA.Count(); i++)
                {
                    result[i] = incidenceA[i] & !incidenceB[i];
                }
                return result;

            default: throw new NotImplementedException("Operator not implemented");
        }
    }
}
