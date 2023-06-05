namespace InfoSearch.Core.Indexes;

public class WordPair : IEquatable<WordPair>
{
    private string WordA { get; init; }

    private string WordB { get; init; }

    public string Key => WordA + WordB;

    public WordPair(string wordA, string wordB)
    {
        WordA = wordA;
        WordB = wordB;
    }

    public bool Equals(WordPair? other)
    {
        if (other == null)
            return false;

        else if (Key == other.Key)
            return true;

        else
            return false;
    }

    public override string ToString() => Key;

    public override int GetHashCode() => Key.GetHashCode();
}
