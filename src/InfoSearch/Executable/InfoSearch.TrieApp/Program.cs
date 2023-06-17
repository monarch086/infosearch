using InfoSearch.ConsoleUtils;
using InfoSearch.Core;
using InfoSearch.Core.Model;
using InfoSearch.Parsing;
using InfoSearch.QueryProcessing.QueryRunners;
using static InfoSearch.ConsoleUtils.Constants;

namespace InfoSearch.TrieApp;

internal class Program
{
    private static ParserResolver _parserResolver = new ParserResolver();
    private static StopWatch _watch = new StopWatch();

    static void Main(string[] args)
    {
        Console.WriteLine("InfoSearch Trie");

        var options = new ConsoleOptions();
        var optionSet = new OptionSetBuilder().Build(options);
        options = ArgsParser.Parse(args, optionSet, options);

        var parser = _parserResolver.Resolve(options.Type);

        var documents = FileScanner.Scan(options.WorkingDirectory, parser.SearchPattern);
        Console.WriteLine($"Documents: \n - {string.Join("\n - ", documents)}.");

        var documentList = new List<Document>();

        _watch.Start();

        foreach (var document in documents)
        {
            var parsedText = parser.Parse(document);
            documentList.Add(new Document(document, parsedText));
        }

        _watch.Stop();
        _watch.Print("Parsing documents");

        _watch.Start();
        var queryRunner = new TrieQueryRunner(documentList);
        _watch.Stop();
        _watch.Print("Creating tries");

        Console.WriteLine("Please enter your query:");
        string queryString = Console.ReadLine() ?? string.Empty;

        while (!EXIT_COMMANDS.Contains(queryString))
        {
            if (string.IsNullOrEmpty(queryString))
                continue;

            _watch.Start();
            var searchResults = queryRunner.Run(queryString);
            _watch.Stop();
            _watch.Print("Searching terms");

            Console.WriteLine($"Search results for query \"{queryString}\":\n - {string.Join("\n - ", searchResults)}.");

            Console.WriteLine("Please enter your query:");
            queryString = Console.ReadLine() ?? string.Empty;
        }

        Console.WriteLine("Finished...");
    }
}