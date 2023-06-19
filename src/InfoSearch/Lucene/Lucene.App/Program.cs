using InfoSearch.ConsoleUtils;
using InfoSearch.Core;
using InfoSearch.Core.Model;
using InfoSearch.Parsing;
using Lucene.Core;
using static InfoSearch.ConsoleUtils.Constants;

namespace Lucene.App;

internal class Program
{
    private static StopWatch _watch = new StopWatch();
    private static ParserResolver _parserResolver = new ParserResolver();

    static void Main(string[] args)
    {
        Console.WriteLine("Lucene.NET example");

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

        using (var searchService = new LuceneService())
        {
            //_watch.Start();
            //searchService.AddDocuments(documentList);
            //_watch.Stop();
            //_watch.Print("Building index");

            Console.WriteLine("Please enter your query:");
            string queryString = Console.ReadLine() ?? string.Empty;

            while (!EXIT_COMMANDS.Contains(queryString))
            {
                if (string.IsNullOrEmpty(queryString))
                    continue;

                _watch.Start();
                var searchResults = searchService.Fetch(queryString);
                _watch.Stop();
                _watch.Print("Searching documents");

                Console.WriteLine($"Search results for query \"{queryString}\":\n - {string.Join("\n - ", searchResults)}.");

                Console.WriteLine("Please enter your query:");
                queryString = Console.ReadLine() ?? string.Empty;
            }

            Console.WriteLine("Finished...");
        }
    }
}