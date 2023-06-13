using InfoSearch.Core;
using Mono.Options;

namespace InfoSearch.ConsoleUtils.OptionsWithSerializer;

public class OptionSetBuilderExtended : OptionSetBuilder
{
    public OptionSet Build(ConsoleOptionsExtended options)
    {
        return base.Build(options)
            .Add("s|serializer=", "the type of serializer.", (SerializerType v) => options.SerializerType = v);
    }
}
