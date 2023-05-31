using InfoSearch.ConsoleUtils;
using InfoSearch.Core;
using InfoSearch.Core.Indexes;
using InfoSearch.Core.Model;
using InfoSearch.Parsing;
using InfoSearch.QueryProcessing;
using InfoSearch.QueryProcessing.QueryRunners;

namespace InfoSearch.IndexApp;

internal class Program
{
    private static ParserResolver parserResolver = new ParserResolver();
    private static string[] exitCommands = { "exit", "quit", "e", "q" };

    static void Main(string[] args)
    {
        Console.WriteLine("InfoSearch Indexes");

        var options = ArgsParser.Parse(args);
        var parser = parserResolver.Resolve(options.Type);

        var documents = FileScanner.Scan(options.WorkingDirectory, parser.SearchPattern);
        Console.WriteLine($"Documents: \n - {string.Join("\n - ", documents)}.");

        var documentList = new List<Document>();

        foreach (var document in documents)
        {
            var parsedText = parser.Parse(document);
            documentList.Add(new Document(document, parsedText));
        }

        var index = new StaticIncidenceMatrix(documentList);
        var queryRunner = new IncidenceMatrixQueryRunner(index);

        string queryString = string.Empty;

        Console.WriteLine("Please enter your query:");
        queryString = Console.ReadLine() ?? string.Empty;

        while (!exitCommands.Contains(queryString))
        {
            if (string.IsNullOrEmpty(queryString))
                continue;

            var query = QueryParser.Parse(queryString);
            var searchResults = queryRunner.Run(query);

            Console.WriteLine($"Search results for query \"{queryString}\":\n - {string.Join("\n - ", searchResults)}.");

            Console.WriteLine("Please enter your query:");
            queryString = Console.ReadLine() ?? string.Empty;
        }

        Console.WriteLine("Finished...");
    }
}