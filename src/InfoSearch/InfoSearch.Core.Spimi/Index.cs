using InfoSearch.Core.Extensions;
using InfoSearch.Core.Model;
using System.Collections.Concurrent;
using System.Text;

namespace InfoSearch.Core.Spimi;

public class Index
{
    private IDictionary<string, IList<int>> _index = new Dictionary<string, IList<int>>();
    private IList<string> _documentNames = new List<string>();

    private const int BLOCK_SIZE = 10000;
    private const int CONCURRENCY_LEVEL = 4;

    private int _blockCounter = 0;
    private List<string> _indexBlocks = new List<string>();

    private const string DOCUMENT_LIST_FILE = "documents.txt";

    public void AddDocuments(IEnumerable<Document> documents, string indexPath)
    {
        var index = new SortedDictionary<string, IList<int>>();

        foreach (var document in documents)
        {
            _documentNames.Add(document.Name);
            var documentIndex = _documentNames.Count() - 1;

            var terms = document.Text.SplitTerms();
            var termsSet = new HashSet<string>(terms);

            foreach (var term in termsSet)
            {
                if (index.Keys.Count() == BLOCK_SIZE)
                {
                    var blockName = $"{indexPath}\\index_block_{_blockCounter++:d8}.txt";
                    WriteIndexBlock(index, blockName);
                    _indexBlocks.Add(blockName);
                    index.Clear();
                }

                if (!index.ContainsKey(term))
                    index.Add(term, new List<int>());

                index[term].Add(documentIndex);
            }
        }

        if (index.Keys.Count() > 0)
        {
            var blockName = $"{indexPath}\\index_block_{_blockCounter++:d8}.txt";
            WriteIndexBlock(index, blockName);
            _indexBlocks.Add(blockName);
            index.Clear();
        }

        WriteDocumentNames(_documentNames, $"{indexPath}\\{DOCUMENT_LIST_FILE}");
    }

    public void BuildIndex(string indexPath)
    {
        var mergedBlockCounter = 0;
        var mergedIndexBlocks = new ConcurrentBag<string>();

        _indexBlocks.ForEach(mergedIndexBlocks.Add);

        while (mergedIndexBlocks.Count() > 1)
        {
            var tasks = new List<Task<string>>();
            
            for (int i = 0; i < CONCURRENCY_LEVEL; i++)
            {
                if (mergedIndexBlocks.Count() > 1)
                {
                    var mergedFileName = $"{indexPath}\\index_block_merged_{mergedBlockCounter++:d8}.txt";

                    mergedIndexBlocks.TryTake(out string blockA);
                    mergedIndexBlocks.TryTake(out string blockB);

                    var task = MergeIndexBlocks(blockA, blockB, mergedFileName);
                    tasks.Add(task);
                }
            }

            Task.WaitAll(tasks.ToArray());

            tasks.ForEach(t => mergedIndexBlocks.Add(t.Result));
        }

        mergedIndexBlocks.TryTake(out string finalIndex);
        LoadIndex(finalIndex);
    }

    public void LoadIndex(string fileName)
    {
        var lines = File.ReadLines(fileName);
        foreach (var line in lines)
        {
            var parsedLine = ParseLine(line);
            _index[parsedLine.Key] = parsedLine.DocIds;
        }

        Console.WriteLine($"Loaded {lines.Count()} terms to index.");
    }

    public void LoadDocumentNames(string indexPath)
    {
        var lines = File.ReadLines($"{indexPath}\\{DOCUMENT_LIST_FILE}");
        foreach (var line in lines)
            _documentNames.Add(line);
    }

    public string[] GetDocuments(string term)
    {
        var documentIds = _index.ContainsKey(term) ? _index[term] : Array.Empty<int>();

        return documentIds.Select(e => _documentNames[e]).ToArray();
    }

    private Task<string> MergeIndexBlocks(string blockA, string blockB, string fileName)
    {
        return Task<string>.Factory.StartNew(() =>
        {
            const int BufferSize = 128;

            var mergedIndex = new SortedDictionary<string, IList<int>>();

            using var fileStreamA = File.Exists(blockA) ? File.OpenRead(blockA) : File.Create(blockA);
            using var fileStreamB = File.Exists(blockB) ? File.OpenRead(blockB) : File.Create(blockB);

            using var streamReaderA = new StreamReader(fileStreamA, Encoding.UTF8, true, BufferSize);
            using var streamReaderB = new StreamReader(fileStreamB, Encoding.UTF8, true, BufferSize);

            MergeLines(streamReaderA, streamReaderB, mergedIndex);
            WriteIndexBlock(mergedIndex, fileName);

            return fileName;
        });
    }

    private void MergeLines(StreamReader readerA, StreamReader readerB, IDictionary<string, IList<int>> index)
    {
        var lineA = readerA.ReadLine();
        var lineB = readerB.ReadLine();

        while (!string.IsNullOrEmpty(lineA) || !string.IsNullOrEmpty(lineB))
        {
            if (string.IsNullOrEmpty(lineA))
            {
                var parsedLineB = ParseLine(lineB);
                AddLineToIndex(parsedLineB, index);
                lineB = readerB.ReadLine();
                continue;
            }
            else if (string.IsNullOrEmpty(lineB))
            {
                var parsedLineA = ParseLine(lineA);
                AddLineToIndex(parsedLineA, index);
                lineA = readerA.ReadLine();
                continue;
            }
            else
            {
                var parsedLineA = ParseLine(lineA);
                var parsedLineB = ParseLine(lineB);

                if (parsedLineA.Key == parsedLineB.Key)
                {
                    var mergedLine = new InvertedListLine
                    {
                        Key = parsedLineA.Key,
                        DocIds = parsedLineA.DocIds.Union(parsedLineB.DocIds).ToList(),
                    };
                    AddLineToIndex(mergedLine, index);
                    lineA = readerA.ReadLine();
                    lineB = readerB.ReadLine();
                    continue;
                }
                else
                {
                    if (parsedLineA.Key.CompareTo(parsedLineB.Key) < 0)
                    {
                        AddLineToIndex(parsedLineA, index);
                        lineA = readerA.ReadLine();
                        continue;
                    }
                    else
                    {
                        AddLineToIndex(parsedLineB, index);
                        lineB = readerB.ReadLine();
                        continue;
                    }
                }
            }
        }
    }

    private void AddLineToIndex(InvertedListLine line, IDictionary<string, IList<int>> index)
    {
        if (index.ContainsKey(line.Key))
            index[line.Key] = index[line.Key].Union(line.DocIds).ToList();
        else
            index.Add(line.Key, line.DocIds);
    }

    private void WriteIndexBlock(IDictionary<string, IList<int>> index, string fileName)
    {
        var isAppend = false;

        using (var outputFile = new StreamWriter(fileName, isAppend))
        {
            foreach (var key in index.Keys)
            {
                var line = $"{key}:" + string.Join(",", index[key]);

                outputFile.WriteLine(line);
            }

            outputFile.Flush();
        }
    }

    private void WriteDocumentNames(IEnumerable<string> documentNames, string fileName)
    {
        var isAppend = false;

        using (var outputFile = new StreamWriter(fileName, isAppend))
        {
            foreach (var name in documentNames)
                outputFile.WriteLine(name);

            outputFile.Flush();
        }
    }

    private InvertedListLine ParseLine(string lineStr)
    {
        var parts = lineStr.Split(':');

        return new InvertedListLine()
        {
            Key = parts[0],
            DocIds = parts[1].Split(',').Select(id => int.Parse(id)).ToList()
        };
    }
}
