using System.Collections;
using System.Text;

namespace InfoSearch.Core.Spimi;

public class IndexCompressed
{
    private string _terms = string.Empty;
    private IDictionary<int, byte[]> _index = new Dictionary<int, byte[]>();
    private IList<string> _documentNames = new List<string>();

    public IndexCompressed(Index index)
    {
        var sb = new StringBuilder();
        var pointer = 0;

        foreach (var term in index.Terms)
        {
            _index.Add(pointer, Array.Empty<byte>());
            sb.Append(term);
            AddPostings(pointer, index.GetPostings(term));

            pointer += term.Length;
        }

        _terms = sb.ToString();
        _documentNames = index.DocumentNames;
    }

    public void AddTerms(IEnumerable<string> terms)
    {
        var sb = new StringBuilder();
        var pointer = 0;

        foreach (var term in terms)
        {
            _index.Add(pointer, Array.Empty<byte>());
            sb.Append(term);
            pointer += term.Length;
        }

        _terms = sb.ToString();
    }

    private void AddPostings(int pointer, IEnumerable<int> postings)
    {
        var variableByteCodes = new List<byte>();

        var previousValue = 0;

        foreach (var posting in postings)
        {
            var difference = posting - previousValue;
            var vbCode = CompressPosting(difference);
            variableByteCodes.AddRange(vbCode);
            previousValue = posting;
        }

        _index[pointer] = variableByteCodes.ToArray();
    }

    public byte[] CompressPosting(int value)
    {
        var bitArray = new BitArray(BitConverter.GetBytes(value));

        var latestSetBit = bitArray.GetLastSetBit();

        for (int i = 0; i < bitArray.Length; i++)
        {
            var isByteSet = i - latestSetBit < 0;
            if (i.IsLastBitInByte() && isByteSet) bitArray.InsertBit(i);
        }

        return bitArray.Trim().ToBytes();
    }

    public IList<int> DecompressPostings(byte[] vbCodes)
    {
        var postings = new List<int>();
        var currentPostingBytes = new List<byte>();

        foreach (var bytePart in vbCodes)
        {
            if (bytePart < 128)
            {
                currentPostingBytes.Add(bytePart);
                var bitArray = new BitArray(currentPostingBytes.ToArray());
                var normalized = bitArray.RemoveVariableBits();
                var posting = normalized.ToInt();
                postings.Add(posting);
                currentPostingBytes.Clear();
            }
            else
            {
                currentPostingBytes.Add(bytePart);
            }
        }

        if (currentPostingBytes.Any())
        {
            var bitArray = new BitArray(currentPostingBytes.ToArray());
            var normalized = bitArray.RemoveVariableBits();
            var posting = normalized.ToInt();
            postings.Add(posting);
            currentPostingBytes.Clear();
        }

        return postings;
    }

    public string[] GetDocuments(string term)
    {
        var termPointer = FindTermPointer(term);
        if (termPointer == -1)
        {
            return new string[0];
        }

        var vbCodes = _index.ContainsKey(termPointer) ? _index[termPointer] : Array.Empty<byte>();

        BitArray bits = new BitArray(vbCodes);

        var documentIds = DecompressPostings(vbCodes);
        Console.WriteLine($"DocIds: {string.Join(", ", documentIds)}");

        return documentIds.Select(e => _documentNames[e]).ToArray();
    }

    public void PrintValues(IEnumerable myList, int myWidth)
    {
        int i = myWidth;
        foreach (var obj in myList)
        {
            if (i <= 0)
            {
                i = myWidth;
                Console.WriteLine();
            }
            i--;
            var val = (bool)obj == true ? 1 : 0;
            Console.Write("{0,8}", val);
        }
        Console.WriteLine();
    }

    public int FindTermPointer(string term)
    {
        var keys = _index.Keys.ToArray();
        var high = keys.Length;
        var low = 0;
        var pointer = high / 2;
        var start = keys[pointer];
        var end = keys[pointer + 1];

        var currentTerm = GetTerm(start, end);

        while (currentTerm != term)
        {
           // Console.WriteLine(currentTerm);

            if (currentTerm.CompareTo(term) < 0)
            {
                if (pointer == high|| pointer == low) return -1;

                low = pointer;
                pointer += (high - low) / 2;
                start = keys[pointer];
                end = keys[pointer + 1];
                currentTerm = GetTerm(start, end);
            }
            else if (currentTerm.CompareTo(term) > 0)
            {
                if (pointer == high || pointer == low) return -1;

                high = pointer;
                pointer -= (high - low) / 2;
                start = keys[pointer];
                end = keys[pointer + 1];
                currentTerm = GetTerm(start, end);
            }
        }

        return keys[pointer];
    }

    private string GetTerm(int start, int end)
    {
        return _terms.Substring(start, end - start);
    }
}
