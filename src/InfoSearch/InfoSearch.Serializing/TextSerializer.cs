using InfoSearch.Core;

namespace InfoSearch.Serializing;

public class TextSerializer : ISerializer
{
    public void Serialize(ISet<string> set, string filename)
    {
        using (var outputFile = new StreamWriter(filename))
        {
            foreach (string item in set)
                outputFile.WriteLine(item);
        }
    }
}