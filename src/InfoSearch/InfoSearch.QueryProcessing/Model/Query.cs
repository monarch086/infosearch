namespace InfoSearch.QueryProcessing.Model;

public class Query
{
    public IList<QueryComponent> Components { get; set; } = new List<QueryComponent>();
}