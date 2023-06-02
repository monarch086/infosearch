using InfoSearch.QueryProcessing.Model;

namespace InfoSearch.Core;

public interface IQueryRunner
{
    IEnumerable<string> Run(Query query);
}
