using InfoSearch.Core.Model.Query;

namespace InfoSearch.Core;

public interface IQueryRunner
{
    IEnumerable<string> Run(Query query);
}
