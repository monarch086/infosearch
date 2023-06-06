namespace InfoSearch.Core;

public interface IQuery<T>
{
    IList<T> Components { get; set; }
}
