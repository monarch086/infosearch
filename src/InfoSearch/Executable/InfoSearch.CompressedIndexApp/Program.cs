using InfoSearch.ConsoleUtils;
using InfoSearch.Core;
using InfoSearch.Core.Extensions;
using static InfoSearch.ConsoleUtils.Constants;

namespace InfoSearch.CompressedIndexApp;

internal class Program
{
    private static StopWatch _watch = new StopWatch();
    private const string INDEX_PATH = "C:\\Users\\Oleksandr_Barsuk\\Downloads\\spimi_index";

    static void Main(string[] args)
    {
        Console.WriteLine("InfoSearch SPIMI Index");

        var index = new Core.Spimi.Index();

        _watch.Start();
        var indexFileName = FileScanner.Scan(INDEX_PATH, "*.txt")
                .OrderBy(s => s)
                .LastOrDefault();

        Console.WriteLine($"Latest index file found: {indexFileName}");

        index.LoadIndex(indexFileName);
        index.LoadDocumentNames(INDEX_PATH);
        _watch.Stop();
        _watch.Print("Loading index");

        var compressedIndex = new Core.Spimi.IndexCompressed(index);

        Console.WriteLine("Please enter your query:");
        string queryString = Console.ReadLine() ?? string.Empty;

        while (!EXIT_COMMANDS.Contains(queryString))
        {
            if (string.IsNullOrEmpty(queryString))
                continue;

            _watch.Start();
            var terms = queryString.SplitTerms();
            var searchResults = new List<string>();
            var resultsInitialized = false;

            foreach (var term in terms)
            {
                var termPointer = compressedIndex.FindTermPointer(queryString);
                Console.WriteLine($"termPointer \"{term}\": {termPointer}.");

                var docs = compressedIndex.GetDocuments(term);

                if (resultsInitialized)
                    searchResults = searchResults.Intersect(docs).ToList();
                else
                {
                    searchResults.AddRange(docs);
                    resultsInitialized = true;
                }
            }

            _watch.Stop();
            _watch.Print("Searching documents");

            Console.WriteLine($"Search results for query \"{queryString}\":\n - {string.Join("\n - ", searchResults)}.");

            Console.WriteLine("Please enter your query:");
            queryString = Console.ReadLine() ?? string.Empty;
        }

        Console.WriteLine("Finished...");
    }
}