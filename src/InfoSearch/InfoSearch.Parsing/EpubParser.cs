using HtmlAgilityPack;
using InfoSearch.Core;
using System.Text;
using VersOne.Epub;

namespace InfoSearch.Parsing;

public class EpubParser : IParser
{
    public ParserType Type => ParserType.EPUB;

    public string SearchPattern => "*.epub";

    public string Parse(string filename)
    {
        EpubBook book = EpubReader.ReadBook(filename);
        StringBuilder sb = new();

        foreach (EpubLocalTextContentFile textContentFile in book.ReadingOrder)
        {
            sb.AppendLine(ReadTextContentFile(textContentFile));
        }

        return sb.ToString();
    }

    private static string ReadTextContentFile(EpubLocalTextContentFile textContentFile)
    {
        HtmlDocument htmlDocument = new();
        htmlDocument.LoadHtml(textContentFile.Content);
        StringBuilder sb = new();

        foreach (HtmlNode node in htmlDocument.DocumentNode.SelectNodes("//text()"))
        {
            sb.AppendLine(node.InnerText.Trim());
        }

        return sb.ToString();
    }
}
