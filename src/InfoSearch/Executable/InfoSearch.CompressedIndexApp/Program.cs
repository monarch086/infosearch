using InfoSearch.ConsoleUtils;
using InfoSearch.Core;
using InfoSearch.Core.Extensions;
using System.Collections;
using static InfoSearch.ConsoleUtils.Constants;

namespace InfoSearch.CompressedIndexApp;

internal class Program
{
    private static StopWatch _watch = new StopWatch();
    private const string INDEX_PATH = "C:\\Users\\Oleksandr_Barsuk\\Downloads\\spimi_index";

    static void Main(string[] args)
    {
        Console.WriteLine("InfoSearch Compressed Index");

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

        //
        var code = compressedIndex.CompressPosting(5);
        BitArray bitArray = new BitArray(code);
        compressedIndex.PrintValues(bitArray, 8);

        var ids = compressedIndex.DecompressPostings(code);
        Console.WriteLine($"Decompressed ids for 5: {string.Join(", ", ids)}.");
        //
        var codes = new List<byte>();
        codes.AddRange(code);
        codes.AddRange(code);
        ids = compressedIndex.DecompressPostings(codes.ToArray());
        Console.WriteLine($"Decompressed ids for 5,5: {string.Join(", ", ids)}.");
        //
        code = compressedIndex.CompressPosting(500);
        bitArray = new BitArray(code);
        compressedIndex.PrintValues(bitArray, 8);

        ids = compressedIndex.DecompressPostings(code);
        Console.WriteLine($"Decompressed ids for 500: {string.Join(", ", ids)}.");
        //
        code = compressedIndex.CompressPosting(15000);
        bitArray = new BitArray(code);
        compressedIndex.PrintValues(bitArray, 8);

        ids = compressedIndex.DecompressPostings(code);
        Console.WriteLine($"Decompressed ids for 15000: {string.Join(", ", ids)}.");
        //
        codes = new List<byte>();
        codes.AddRange(compressedIndex.CompressPosting(855));
        codes.AddRange(compressedIndex.CompressPosting(825));
        codes.AddRange(compressedIndex.CompressPosting(661));
        codes.AddRange(compressedIndex.CompressPosting(593));
        codes.AddRange(compressedIndex.CompressPosting(567));
        ids = compressedIndex.DecompressPostings(codes.ToArray());
        Console.WriteLine($"Decompressed ids for 855,825,661,593,567: {string.Join(", ", ids)}.");
        //

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