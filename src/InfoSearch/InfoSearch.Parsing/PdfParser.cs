using InfoSearch.Core;
using iTextSharp.text.pdf;
using System.Text;

namespace InfoSearch.Parsing;

public class PdfParser : IParser
{
    public string Parse(string filename)
    {
        PdfReader reader = new PdfReader(filename);
        var strings = new StringBuilder();

        try
        {
            for (var pageNum = 1; pageNum <= reader.NumberOfPages; pageNum++)
            {
                var contentBytes = reader.GetPageContent(pageNum);
                var tokenizer = new PrTokeniser(new RandomAccessFileOrArray(contentBytes));

                while (tokenizer.NextToken())
                {
                    if (tokenizer.TokenType == PrTokeniser.TK_STRING)
                    {
                        strings.AppendLine(tokenizer.StringValue);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"ERROR parsing {filename}: {e}.");
        }
        finally { reader.Close(); }

        return strings.ToString();
    }
}