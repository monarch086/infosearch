using InfoSearch.ConsoleUtils;
using InfoSearch.PgSearchCore;

namespace InfoSearch.PgSearch.App;

internal class Program
{
    private static string[] _exitCommands = { "exit", "quit", "e", "q" };
    private static StopWatch _watch = new StopWatch();
    private static Repository _repository = new Repository();

    static void Main(string[] args)
    {
        Console.WriteLine("InfoSearch PostgreSQL Extensions");

        Console.WriteLine("Please enter your query:");
        string queryString = Console.ReadLine() ?? string.Empty;

        while (!_exitCommands.Contains(queryString))
        {
            if (string.IsNullOrEmpty(queryString))
                continue;

            _watch.Start();
            var searchResults = _repository.SearchByTitle(queryString);
            _watch.Stop();
            _watch.Print("Searching documents");

            Console.WriteLine($"Search results for query \"{queryString}\":\n - {string.Join("\n - ", searchResults)}.");

            Console.WriteLine("Please enter your query:");
            queryString = Console.ReadLine() ?? string.Empty;
        }

        Console.WriteLine("Finished...");
    }
}