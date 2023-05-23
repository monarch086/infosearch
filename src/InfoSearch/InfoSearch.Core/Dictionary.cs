namespace InfoSearch.Core;

public class Dictionary
{
    private readonly ISet<string> _set = new SortedSet<string>();
    // HashSet gives O(1) complexity,
    // where SortedSet gives O(log n)

    public void Add(string term)
    {
        _set.Add(term);
    }

    public void AddRange(IEnumerable<string> terms)
    {
        foreach (string term in terms)
            _set.Add(term);
    }

    public bool Exists(string term)
    {
        return _set.Contains(term);
    }

    public override string ToString()
    {
        var limit = 100;

        return $"Dictionary top {limit} words: " + string.Join(", ", _set.Take(limit));
    }

    public ISet<string> Set => _set;

    public void Save(ISerializer serializer, string path)
    {
        serializer.Serialize(_set, path);
    }
}