namespace InfoSearch.Core;

public class Trie
{
    private TrieNode root;

    public Trie()
    {
        root = new TrieNode();
    }

    public void Insert(string word)
    {
        var current = root;

        foreach (char c in word)
        {
            if (!current.Children.ContainsKey(c))
            {
                current.Children[c] = new TrieNode();
            }

            current = current.Children[c];
        }

        current.IsWordEnd = true;
    }

    public bool Contains(string word)
    {
        TrieNode current = root;

        foreach (char c in word)
        {
            if (!current.Children.ContainsKey(c))
            {
                return false;
            }

            current = current.Children[c];
        }

        return current.IsWordEnd;
    }

    public IList<string> StartsWith(string prefix)
    {
        var results = new List<string>();
        var current = root;

        foreach (char c in prefix)
        {
            if (!current.Children.ContainsKey(c))
            {
                return results;
            }

            current = current.Children[c];
        }

        CollectWords(current, prefix, results);

        return results;
    }

    private void CollectWords(TrieNode node, string prefix, List<string> results)
    {
        if (node.IsWordEnd)
        {
            results.Add(prefix);
        }

        foreach (char c in node.Children.Keys)
        {
            CollectWords(node.Children[c], prefix + c, results);
        }
    }
}

internal class TrieNode
{
    public IDictionary<char, TrieNode> Children { get; set; }
    public bool IsWordEnd { get; set; }

    public TrieNode()
    {
        Children = new Dictionary<char, TrieNode>();
        IsWordEnd = false;
    }
}