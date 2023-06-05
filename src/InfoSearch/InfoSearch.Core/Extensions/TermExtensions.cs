using InfoSearch.Core.Indexes;
using System.Text.RegularExpressions;

namespace InfoSearch.Core.Extensions;

public static class TermExtensions
{
    private static char[] delimiters = new char[] { ',', ';', ':', ' ', '.', '-', '—', '!', '?', '\n', '\r' };

    private static char[] trimChars = new char[] { ' ', '.', ',', ';', ':', '-', '!', '?', '"', '”', '“', '‘', '’', '(', ')', '—', '*', '\'', '\"', '[', ']', '_', '…', '#', '$', '&' };

    public static string[] SplitTerms(this string document)
    {
        return document.Split(delimiters)
            .Select(s => s.Trim(trimChars))
            .Select(s => s.Trim())
            .Where(s => !Regex.Match(s, @"[^\p{L}]").Success) // Exclude terms with non-letter chars
            .Select(s => s.ToLower())
            .Where(s => s.Length > 1)
            .ToArray();
    }

    public static IEnumerable<WordPair> ToPairs(this IEnumerable<string> terms)
    {
        if (terms.Count() == 1)
        {
            throw new ArgumentException("There should be at least two terms in query.");
        }

        var result = new List<WordPair>(terms.Count() - 1);
        var previousTerm = string.Empty;

        foreach (var term in terms)
        {
            if (string.IsNullOrEmpty(previousTerm))
            {
                previousTerm = term;
                continue;
            }

            result.Add(new WordPair(previousTerm, term));
            previousTerm = term;
        }

        return result;
    }
}
