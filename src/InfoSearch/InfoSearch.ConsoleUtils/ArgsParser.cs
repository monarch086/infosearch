using InfoSearch.Core;
using Mono.Options;

namespace InfoSearch.ConsoleUtils;

public static class ArgsParser
{
    public static ConsoleOptions Parse(string[] args)
    {
        var options = new ConsoleOptions();

        var p = new OptionSet() {
                { "t|type=", "the {TYPE} of parser.",
                   (ParserType v) => options.Type = v },
                { "d|directory=",
                   "the directory to scan for the documents.\n" +
                      "this must be an integer.",
                    (v) => options.WorkingDirectory = v },
                { "s|serializer=", "the type of serializer.",
                   (SerializerType v) => options.SerializerType = v },
            };

        List<string> extra;
        try
        {
            extra = p.Parse(args);
            return options;
        }
        catch (OptionException e)
        {
            Console.WriteLine(e.Message);
            return options;
        }
    }
}
