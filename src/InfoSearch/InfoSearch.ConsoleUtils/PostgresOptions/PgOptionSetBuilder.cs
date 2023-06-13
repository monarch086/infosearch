using InfoSearch.PgSearchCore;
using Mono.Options;

namespace InfoSearch.ConsoleUtils.PostgresOptions;

public class PgOptionSetBuilder
{
    public OptionSet Build(PgOptions options)
    {
        return new OptionSet() {
            { "t|type=", "the {TYPE} of search.",
               (SearchType v) => options.SearchType = v }
        };
    }
}
