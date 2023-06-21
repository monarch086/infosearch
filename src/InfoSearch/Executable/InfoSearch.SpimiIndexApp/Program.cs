namespace InfoSearch.SpimiIndexApp;

using InfoSearch.ConsoleUtils;
using InfoSearch.Core;
using InfoSearch.Parsing;
using static InfoSearch.ConsoleUtils.Constants;
using InfoSearch.Core.Model;
using InfoSearch.ConsoleUtils.SpimiOptions;

internal class Program
{
    private static ParserResolver _parserResolver = new ParserResolver();
    private static StopWatch _watch = new StopWatch();
    private const string INDEX_PATH = "C:\\Users\\Oleksandr_Barsuk\\Downloads\\spimi_index";

    static void Main(string[] args)
    {
        Console.WriteLine("InfoSearch SPIMI Index");

        var options = new SpimiAppOptions();
        var optionSet = new SpimiAppOptionSetBuilder().Build(options);
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

        var index = new Core.Spimi.Index();

        if (options.Loading == LoadingOption.BuildIndex)
        {
            _watch.Start();
            index.BuildIndex(documentList, INDEX_PATH);
            _watch.Stop();
            _watch.Print("Creating index");
        } else
        {
            _watch.Start();
            var indexFileName = FileScanner.Scan(INDEX_PATH, "*.txt")
                .OrderBy(s => s)
                .LastOrDefault();

            Console.WriteLine($"Latest index file found: {indexFileName}");

            index.LoadIndex(indexFileName);
            index.LoadDocumentNames(INDEX_PATH);
            _watch.Stop();
            _watch.Print("Loading index");
        }

        Console.WriteLine("Please enter your query:");
        string queryString = Console.ReadLine() ?? string.Empty;

        while (!EXIT_COMMANDS.Contains(queryString))
        {
            if (string.IsNullOrEmpty(queryString))
                continue;

            _watch.Start();
            var searchResults = index.GetDocuments(queryString);
            _watch.Stop();
            _watch.Print("Searching documents");

            Console.WriteLine($"Search results for query \"{queryString}\":\n - {string.Join("\n - ", searchResults)}.");

            Console.WriteLine("Please enter your query:");
            queryString = Console.ReadLine() ?? string.Empty;
        }

        Console.WriteLine("Finished...");
    }
}