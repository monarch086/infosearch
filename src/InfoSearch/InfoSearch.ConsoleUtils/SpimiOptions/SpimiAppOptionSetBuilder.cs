using Mono.Options;

namespace InfoSearch.ConsoleUtils.SpimiOptions;

public class SpimiAppOptionSetBuilder : OptionSetBuilder
{
    public OptionSet Build(SpimiAppOptions options)
    {
        return base.Build(options)
            .Add("l|loading=", "loading index options: build index or load already built one.", (LoadingOption o) => options.Loading = o);
    }
}
