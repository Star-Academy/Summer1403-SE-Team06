using System.Text.RegularExpressions;
using Mohaymen.FullTextSearch.DocumentManagement.Interfaces;
using Mohaymen.FullTextSearch.DocumentManagement.Models;

namespace Mohaymen.FullTextSearch.DocumentManagement.Utilities;

public class Tokenizer : ITokenizer
{
    private readonly string SplitRegex = @"[^\w']+";
    
    public List<Keyword> ExtractKeywords(string text)
    {
        return Regex.Split(text, SplitRegex)
            .Where(word => !string.IsNullOrWhiteSpace(word))
            .Select(word => new Keyword(word))
            .ToList();
    }
}