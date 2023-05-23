using InfoSearch.Core;

namespace InfoSearch.Serializing;

public class TextSerializer : ISerializer
{
    public SerializerType Type => SerializerType.Text;

    public string FileExtension => "txt";

    public void Serialize(ISet<string> set, string filename)
    {
        using (var outputFile = new StreamWriter(filename))
        {
            foreach (string item in set)
                outputFile.WriteLine(item);
        }
    }
}