using InfoSearch.Core;

namespace InfoSearch.QueryProcessing.Model;

public class CoordinateQuery : IQuery<CoordinateQueryComponent>
{
    public IList<CoordinateQueryComponent> Components { get; set; } = new List<CoordinateQueryComponent>();
}
