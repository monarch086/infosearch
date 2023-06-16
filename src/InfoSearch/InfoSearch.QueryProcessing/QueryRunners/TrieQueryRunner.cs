using InfoSearch.Core;
using InfoSearch.Core.Extensions;
using InfoSearch.Core.Model;

namespace InfoSearch.QueryProcessing.QueryRunners;

public class TrieQueryRunner
{
    private readonly Trie directTrie = new Trie();
    private readonly Trie reverseTrie = new Trie();

    private const char JOKER_SIGN = '*';

    public TrieQueryRunner(IEnumerable<Document> documents)
    {
        foreach (var document in documents)
        {
            var terms = document.Text.SplitTerms();
            foreach (var term in terms)
            {
                directTrie.Insert(term);
                reverseTrie.Insert(RotateWord(term));
            }
        }
    }

    public IEnumerable<string> Run(string query)
    {
        if (query.Count(c => c == JOKER_SIGN) != 1)
            throw new ArgumentException($"Input query should contain one joker sign ({JOKER_SIGN}).");

        var queryParts = query.Split(JOKER_SIGN);

        var beforeJokerWords = directTrie.StartsWith(queryParts[0]);
        var afterJokerWords = reverseTrie.StartsWith(RotateWord(queryParts[1]))
            .Select(w => RotateWord(w));

        if (string.IsNullOrEmpty(queryParts[0]))
            return afterJokerWords;
        else if (string.IsNullOrEmpty(queryParts[1]))
            return beforeJokerWords;
        else return beforeJokerWords.Intersect(afterJokerWords);
    }

    private string RotateWord(string word)
    {
        return string.Join(null, word.Reverse());
    }
}
