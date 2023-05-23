using System.Text.RegularExpressions;

namespace InfoSearch.Core;

public static class TermSplitting
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
}
