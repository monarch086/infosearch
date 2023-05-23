namespace InfoSearch.Core;

public interface ISerializer
{
    SerializerType Type { get; }

    string FileExtension { get; }

    void Serialize(ISet<string> set, string filename);
}

