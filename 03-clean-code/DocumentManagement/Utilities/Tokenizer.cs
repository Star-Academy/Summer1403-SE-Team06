using System.Text.RegularExpressions;
using Mohaymen.FullTextSearch.DocumentManagement.Models;

namespace Mohaymen.FullTextSearch.DocumentManagement.Utilities;

public static class Tokenizer
{
    private static readonly string SplitRegex = @"[^\w']+";
    
    public static List<Keyword> ExtractKeywords(string text)
    {
        return Regex.Split(text, SplitRegex)
            .Where(word => !string.IsNullOrWhiteSpace(word))
            .Select(word => new Keyword(word))
            .ToList();
    }
}