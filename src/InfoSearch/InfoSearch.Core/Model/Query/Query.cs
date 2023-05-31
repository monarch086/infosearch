namespace InfoSearch.Core.Model.Query;

public class Query
{
    public IList<QueryComponent> Components { get; set; } = new List<QueryComponent>();
}