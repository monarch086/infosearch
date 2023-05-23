namespace InfoSearch.Core;

public interface IParser
{
    ParserType Type { get; }
    string SearchPattern { get; }
    string Parse(string filename);
}
