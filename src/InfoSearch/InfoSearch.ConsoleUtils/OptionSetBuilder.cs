using InfoSearch.Core;
using Mono.Options;

namespace InfoSearch.ConsoleUtils;

public class OptionSetBuilder
{
    public OptionSet Build(ConsoleOptions options)
    {
        return new OptionSet() {
            { "t|type=", "the {TYPE} of parser.",
               (ParserType v) => options.Type = v },
            { "d|directory=",
               "the directory to scan for the documents.",
                (v) => options.WorkingDirectory = v }
        };
    }
}
