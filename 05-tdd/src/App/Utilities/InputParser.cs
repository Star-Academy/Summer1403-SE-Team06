using System.Text.RegularExpressions;
using Mohaymen.FullTextSearch.App.Interfaces;
using Mohaymen.FullTextSearch.DocumentManagement.Models;
using Mohaymen.FullTextSearch.DocumentManagement.Services.InvertedIndexService.SearchStrategies;

namespace Mohaymen.FullTextSearch.App.Utilities;

public class InputParser : IInputParser
{
    // SplitRegexPattern matches single words and phrases
    private const string SplitRegexPattern = @"[+-]?\b\w+\b|[+-]?""[^""]+""";
    public List<SearchQuery> ParseToSearchQuery(string input)
    {
        var mandatoryWords = new List<Keyword>();
        var optionalWords = new List<Keyword>();
        var excludedWords = new List<Keyword>();

        var regex = new Regex(SplitRegexPattern);
        
        var matches = regex.Matches(input);

        foreach (Match match in matches)
        {
            var word = match.Value;

            if (word.StartsWith('+'))
                optionalWords.Add(new Keyword(word.Substring(1).Trim('"')));
            else if (word.StartsWith('-'))
                excludedWords.Add(new Keyword(word.Substring(1).Trim('"')));
            else
                mandatoryWords.Add(new Keyword(word.Trim('"')));
        }

        return [
            new SearchQuery(new MandatorySearchStrategy(), mandatoryWords),
            new SearchQuery(new OptionalSearchStrategy(), optionalWords),
            new SearchQuery(new ExcludedSearchStrategy(), excludedWords)
        ];
    }
}
