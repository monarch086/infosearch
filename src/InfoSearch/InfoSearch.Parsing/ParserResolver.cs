using InfoSearch.Core;

namespace InfoSearch.Parsing;

public class ParserResolver
{
    private readonly IList<IParser> parsers;

    public ParserResolver()
    {
        parsers = new List<IParser>
        {
            new PdfParser(),
            new EpubParser()
        };
    }

    public IParser Resolve(ParserType type)
    {
        foreach (IParser parser in parsers)
        {
            if (parser.Type == type) { return parser; }
        }

        throw new NotImplementedException($"Parser for {type} not found.");
    }
}
