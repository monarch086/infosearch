using InfoSearch.Core;

namespace InfoSearch.QueryProcessing.Model.BooleanQuery;

public class BoolQuery : IQuery<BoolQueryComponent>
{
    public IList<BoolQueryComponent> Components { get; set; }
}