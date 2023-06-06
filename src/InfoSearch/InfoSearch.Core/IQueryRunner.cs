namespace InfoSearch.Core;

public interface IQueryRunner<T>
{
    IEnumerable<string> Run(IQuery<T> query);
}
