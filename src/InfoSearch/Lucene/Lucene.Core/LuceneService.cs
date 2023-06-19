using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using Directory = Lucene.Net.Store.Directory;

namespace Lucene.Core;

public class LuceneService : IDisposable
{
    //private Analyzer analyzer = new WhitespaceAnalyzer();
    private Directory indexDirectory;
    private IndexWriter writer;
    //private string indexPath = @"c:\temp\LuceneIndex";

    public LuceneService()
    {
        InitialiseLucene();
    }

    private void InitialiseLucene()
    {
        // Ensures index backward compatibility
        const LuceneVersion AppLuceneVersion = LuceneVersion.LUCENE_48;

        // Construct a machine-independent path for the index
        var basePath = Environment.GetFolderPath(
            Environment.SpecialFolder.CommonApplicationData);
        var indexPath = Path.Combine(basePath, "index");

        indexDirectory = FSDirectory.Open(indexPath);

        //using (var dir = FSDirectory.Open(indexPath))
        {
            // Create an analyzer to process the text
            var analyzer = new StandardAnalyzer(AppLuceneVersion);

            // Create an index writer
            var indexConfig = new IndexWriterConfig(AppLuceneVersion, analyzer);
            writer = new IndexWriter(indexDirectory, indexConfig);

            var source = new
            {
                Name = "Kermit the Frog",
                FavoritePhrase = "The quick brown fox jumps over the lazy dog"
            };
            var doc = new Document
            {
                new StringField("name",
                    source.Name,
                    Field.Store.YES),
                new TextField("favoritePhrase",
                    source.FavoritePhrase,
                    Field.Store.YES)
            };

            writer.AddDocument(doc);
            writer.Flush(triggerMerge: false, applyAllDeletes: false);
        }
    }

    public void Fetch()
    {
        // Search with a phrase
        var phrase = new MultiPhraseQuery
        {
            new Term("favoritePhrase", "brown"),
            new Term("favoritePhrase", "fox"),
        };

        // Re-use the writer to get real-time updates
        using var reader = writer.GetReader(applyAllDeletes: true);
        var searcher = new IndexSearcher(reader);
        var hits = searcher.Search(phrase, 20 /* top 20 */).ScoreDocs;

        // Display the output in a table
        Console.WriteLine($"{"Score",10}" +
            $" {"Name",-15}" +
            $" {"Favorite Phrase",-40}");
        foreach (var hit in hits)
        {
            var foundDoc = searcher.Doc(hit.Doc);
            Console.WriteLine($"{hit.Score:f8}" +
                $" {foundDoc.Get("name"),-15}" +
                $" {foundDoc.Get("favoritePhrase"),-40}");
        }
    }

    public void Dispose()
    {
        writer.Dispose();
        indexDirectory.Dispose();
    }
}
