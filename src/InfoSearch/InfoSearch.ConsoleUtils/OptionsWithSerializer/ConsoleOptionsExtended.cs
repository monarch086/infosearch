using InfoSearch.Core;

namespace InfoSearch.ConsoleUtils.OptionsWithSerializer;

public class ConsoleOptionsExtended : ConsoleOptions
{
    public SerializerType SerializerType { get; set; } = SerializerType.Text;
}
