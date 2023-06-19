using Lucene.Core;

namespace Lucene.App;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Lucene.NET example");

        using var searchService = new LuceneService();
        searchService.Fetch();
    }
}