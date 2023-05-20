namespace InfoSearch.Core;

public interface ISerializer
{
    void Serialize(ISet<string> set, string filename);
}

