using Mono.Options;

namespace InfoSearch.ConsoleUtils;

public static class ArgsParser
{
    public static T Parse<T>(string[] args, OptionSet optionSet, T options) where T : class, new()
    {
        List<string> extra;
        try
        {
            extra = optionSet.Parse(args);
            return options;
        }
        catch (OptionException e)
        {
            Console.WriteLine(e.Message);
            return options;
        }
    }
}
