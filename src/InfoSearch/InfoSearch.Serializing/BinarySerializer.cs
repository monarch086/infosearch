using Binaron.Serializer;
using InfoSearch.Core;

namespace InfoSearch.Serializing;

public class BinarySerializer : ISerializer
{
    public SerializerType Type => SerializerType.Binary;

    public string FileExtension => "bin";

    public void Serialize(ISet<string> set, string filename)
    {
        using (var stream = File.OpenWrite(filename))
        {
            BinaronConvert.Serialize(set, stream);
            stream.Flush();
        }
    }
}

