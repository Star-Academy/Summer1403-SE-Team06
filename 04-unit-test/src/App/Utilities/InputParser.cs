using Mohaymen.FullTextSearch.App.Interfaces;
using Mohaymen.FullTextSearch.DocumentManagement.Models;
using Mohaymen.FullTextSearch.DocumentManagement.Services.InvertedIndexService.SearchStrategies;

namespace Mohaymen.FullTextSearch.App.Utilities;

public class InputParser : IInputParser
{
    public List<SearchQuery> ParseToSearchQuery(string input)
    {
        var mandatoryWords = new List<Keyword>();
        var optionalWords = new List<Keyword>();
        var excludedWords = new List<Keyword>();

        var words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        foreach (var word in words)
            if (word.StartsWith('+'))
                optionalWords.Add(new Keyword(word.Substring(1)));
            else if (word.StartsWith('-'))
                excludedWords.Add(new Keyword(word.Substring(1)));
            else
                mandatoryWords.Add(new Keyword(word));

        return [
            new SearchQuery(new MandatorySearchStrategy(), mandatoryWords),
            new SearchQuery(new OptionalSearchStrategy(), optionalWords),
            new SearchQuery(new ExcludedSearchStrategy(), excludedWords)
        ];
    }
}
