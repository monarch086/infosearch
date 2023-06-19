using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using Directory = Lucene.Net.Store.Directory;
using InfoSearchDocument = InfoSearch.Core.Model.Document;

namespace Lucene.Core;

public class LuceneService : IDisposable
{
    private const string _indexDirName = "index";
    private string _basePath => Environment.GetFolderPath(
            Environment.SpecialFolder.CommonApplicationData);
    private string _indexPath => Path.Combine(_basePath, _indexDirName);

    const LuceneVersion LUCENE_VERSION = LuceneVersion.LUCENE_48;

    private readonly Directory indexDirectory;
    private readonly IndexWriter writer;

    public LuceneService()
    {
        Console.WriteLine("Index path: " + _indexPath);
        indexDirectory = FSDirectory.Open(_indexPath);

        var analyzer = new StandardAnalyzer(LUCENE_VERSION);

        var indexConfig = new IndexWriterConfig(LUCENE_VERSION, analyzer);
        writer = new IndexWriter(indexDirectory, indexConfig);
    }

    public void AddDocuments(IEnumerable<InfoSearchDocument> documents)
    {
        foreach (var document in documents)
        {
            var doc = new Document
                {
                    new TextField("name",
                        document.Name,
                        Field.Store.YES),
                    new TextField("text",
                        document.Text,
                        Field.Store.YES)
                };

            writer.AddDocument(doc);
        }

        writer.Flush(triggerMerge: true, applyAllDeletes: true);
    }

    public IEnumerable<DocumentSearchResult> Fetch(string queryString)
    {
        var analyzer = new StandardAnalyzer(LUCENE_VERSION);
        //QueryParser parser = new QueryParser(LUCENE_VERSION, "text", analyzer);
        QueryParser parser = new MultiFieldQueryParser(LUCENE_VERSION, new string[] { "name", "text" }, analyzer);
        Query query = parser.Parse(queryString);

        using var reader = writer.GetReader(applyAllDeletes: true);
        var searcher = new IndexSearcher(reader);
        var hits = searcher.Search(query, 3).ScoreDocs;

        var resultDocs = new List<DocumentSearchResult>();

        foreach (var hit in hits)
        {
            var foundDoc = searcher.Doc(hit.Doc);

            resultDocs.Add(new DocumentSearchResult()
            {
                Name = foundDoc.Get("name"),
                Text = foundDoc.Get("text"),
                Score = hit.Score
            });
        }

        return resultDocs;
    }

    public void Dispose()
    {
        writer.Dispose();
        indexDirectory.Dispose();
    }
}
