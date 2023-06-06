using InfoSearch.ConsoleUtils;
using InfoSearch.Core;
using InfoSearch.Core.Extensions;
using InfoSearch.Parsing;
using InfoSearch.Serializing;

public class Program
{
    private static ParserResolver parserResolver = new ParserResolver();
    private static SerializerResolver serializerResolver = new SerializerResolver();

    public static void Main(string[] args)
    {
        Console.WriteLine("InfoSearch Dictionary");

        var options = ArgsParser.Parse(args);
        var parser = parserResolver.Resolve(options.Type);

        var documents = FileScanner.Scan(options.WorkingDirectory, parser.SearchPattern);
        Console.WriteLine($"Documents: \n - {string.Join("\n - ", documents)}.");

        var dictionary = new Dictionary();

        foreach (var document in documents)
        {
            var doc = parser.Parse(document);
            var terms = doc.SplitTerms();
            dictionary.AddRange(terms);
        }

        Console.WriteLine($"Result set: {dictionary}");

        Console.WriteLine($"Dictionary length: {dictionary.Set.Count()}");

        var currentTime = DateTime.Now;
        var serializationTypeName = options.SerializerType.ToString().ToLower();
        var serializer = serializerResolver.Resolve(options.SerializerType);
        var filename = $"{options.WorkingDirectory}\\{currentTime.ToString("yyyy-MM-dd-HH-mm-ss")}" +
            $"_dictionary_{serializationTypeName}.{serializer.FileExtension}";

        dictionary.Save(serializer, filename);

        Console.WriteLine("Done...");
    }
}
