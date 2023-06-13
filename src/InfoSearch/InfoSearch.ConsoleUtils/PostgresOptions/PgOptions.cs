using InfoSearch.PgSearchCore;

namespace InfoSearch.ConsoleUtils.PostgresOptions;

public class PgOptions
{
    public SearchType SearchType { get; set; } = SearchType.ByTitle;
}
