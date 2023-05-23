using InfoSearch.Core;

namespace InfoSearch.Serializing
{
    public class SerializerResolver
    {
        private readonly IList<ISerializer> serializers;

        public SerializerResolver()
        {
            serializers = new List<ISerializer>
            {
                new BinarySerializer(),
                new JsonSerializer(),
                new TextSerializer()
            };
        }

        public ISerializer Resolve(SerializerType type)
        {
            foreach (var serializer in serializers)
            {
                if (serializer.Type == type) { return serializer; }
            }

            throw new NotImplementedException($"Serializer for {type} not found.");
        }
    }
}
