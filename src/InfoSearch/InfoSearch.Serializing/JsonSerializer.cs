using InfoSearch.Core;

namespace InfoSearch.Serializing
{
    public class JsonSerializer : ISerializer
    {
        public SerializerType Type => SerializerType.Json;

        public string FileExtension => "json";

        public void Serialize(ISet<string> set, string filename)
        {
            using (var outputFile = new StreamWriter(filename))
            {
                var result = System.Text.Json.JsonSerializer.Serialize(set);
                outputFile.WriteLine(result);
            }
        }
    }
}
