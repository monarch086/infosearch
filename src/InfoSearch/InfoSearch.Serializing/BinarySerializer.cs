using InfoSearch.Core;
using MessagePack;

namespace InfoSearch.Serializing;

public class BinarySerializer : ISerializer
{
    public SerializerType Type => SerializerType.Binary;

    public string FileExtension => "bin";

    public void Serialize(ISet<string> set, string filename)
    {
        using (var stream = File.OpenWrite(filename))
        {
            MessagePackSerializer.Serialize(stream, set);
            stream.Flush();
        }
    }
}

