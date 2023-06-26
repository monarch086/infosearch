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
            AddPostings(pointer, index.GetPostings(term).ToHashSet());

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

    private void AddPostings(int pointer, ISet<int> postings)
    {
        var variableByteCodes = new List<byte>();

        var previousValue = 0;

        foreach (var posting in postings)
        {
            var difference = posting - previousValue;
            var vbCode = CalculateVbCode(difference);
            variableByteCodes.AddRange(vbCode);
            previousValue = posting;
        }

        _index[pointer] = variableByteCodes.ToArray();
    }

    private byte[] CalculateVbCode(int value)
    {
        var bytes = new List<byte>();

        while (value > 0)
        {
            value = value << 1;

            var bytePart = value % 128;
            var shifted = bytePart >> 1;

            if (value / 128 != 0) shifted |= 128;
            else shifted &= 127;

            bytes.AddRange(BitConverter.GetBytes(shifted));

            value /= 128;
        }

        bytes.Reverse();

        return bytes.Where(x => x != 0).ToArray();
    }

    private IList<int> DecompressPostings(byte[] vbCodes)
    {
        var postings = new List<int>();
        var currentPostingBytes = new List<byte>();

        foreach (var bytePart in vbCodes)
        {
            if (bytePart < 128)
            {
                currentPostingBytes.Add(bytePart);
                //var shiftedNumber = BitConverter.ToInt32(currentPostingBytes.ToArray());
                var shiftedNumber = bytePart;
                var posting = 0;

                while (shiftedNumber > 0)
                {
                    posting = posting << 1;
                    posting += shiftedNumber % 128;
                    posting = posting >> 1;
                    shiftedNumber /= 128;
                }

                postings.Add(posting);
                currentPostingBytes.Clear();
            }
            else
            {
                currentPostingBytes.Add(bytePart);
            }
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
        PrintValues(vbCodes, 8);

        var documentIds = DecompressPostings(vbCodes);
        Console.WriteLine($"DocIds: {string.Join(", ", documentIds)}");

        return documentIds.Select(e => _documentNames[e]).ToArray();
    }

    public static void PrintValues(IEnumerable myList, int myWidth)
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
            Console.Write("{0,8}", obj);
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
            Console.WriteLine(currentTerm);

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
