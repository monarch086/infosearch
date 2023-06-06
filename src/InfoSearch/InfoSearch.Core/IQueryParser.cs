namespace InfoSearch.Core;

public interface IQueryParser<T>
{
    IQuery<T> Parse(string query);
}
