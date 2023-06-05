using InfoSearch.ConsoleUtils;
using InfoSearch.Core.Indexes;
using InfoSearch.Core;
using InfoSearch.Parsing;
using InfoSearch.QueryProcessing.QueryRunners;
using InfoSearch.Core.Model;

namespace InfoSearch.TwoWordIndexApp
{
    internal class Program
    {
        private static ParserResolver _parserResolver = new ParserResolver();
        private static string[] _exitCommands = { "exit", "quit", "e", "q" };
        private static StopWatch _watch = new StopWatch();

        static void Main(string[] args)
        {
            Console.WriteLine("InfoSearch Indexes");

            var options = ArgsParser.Parse(args);
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
            var index = new TwoWordIndex(documentList);
            var queryRunner = new TwoWordIndexQueryRunner(index);

            _watch.Stop();
            _watch.Print("Creating index");

            Console.WriteLine("Please enter your query:");
            string queryString = Console.ReadLine() ?? string.Empty;

            while (!_exitCommands.Contains(queryString))
            {
                if (string.IsNullOrEmpty(queryString))
                    continue;

                _watch.Start();
                var searchResults = queryRunner.Run(queryString);
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