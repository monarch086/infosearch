using InfoSearch.ConsoleUtils;
using InfoSearch.ConsoleUtils.PostgresOptions;
using InfoSearch.PgSearch.Model;
using InfoSearch.PgSearchCore;
using static InfoSearch.ConsoleUtils.Constants;

namespace InfoSearch.PgSearch.App;

internal class Program
{
    private static StopWatch _watch = new StopWatch();
    private static Repository _repository = new Repository();

    static void Main(string[] args)
    {
        Console.WriteLine("InfoSearch PostgreSQL Extensions");

        var options = new PgOptions();
        var optionSet = new PgOptionSetBuilder().Build(options);
        options = ArgsParser.Parse(args, optionSet, options);

        var allBooks = _repository.GetAllBooks();
        Console.WriteLine($"All available books:\n - {string.Join("\n - ", allBooks)}.");

        Console.WriteLine("Please enter your query:");
        string queryString = Console.ReadLine() ?? string.Empty;

        while (!EXIT_COMMANDS.Contains(queryString))
        {
            if (string.IsNullOrEmpty(queryString))
                continue;

            _watch.Start();
            IEnumerable<Book> searchResults;

            switch (options.SearchType)
            {
                case SearchType.ByAuthor:
                    searchResults = _repository.SearchByAuthor(queryString);
                    break;
                case SearchType.ByTitle:
                    searchResults = _repository.SearchByTitle(queryString);
                    break;
                default:
                    searchResults = new List<Book>();
                    break;
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