using InfoSearch.Core;
using iTextSharp.text.pdf;
using System.Text;

namespace InfoSearch.Parsing;

public class PdfParser : IParser
{
    public ParserType Type => ParserType.PDF;

    public string SearchPattern => "*.pdf";

    public string Parse(string filename)
    {
        var strings = new StringBuilder();

        try
        {
            var reader = new PdfReader(filename);

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

            reader.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error parsing PDF {filename}: {e}.");
        }

        return strings.ToString();
    }
}