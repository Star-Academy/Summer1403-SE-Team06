using Mohaymen.FullTextSearch.DocumentManagement.Models;

namespace Mohaymen.FullTextSearch.App.Utilities;

public class Parser
{
    public static SearchQuery ParseInputToSearchQuery(string input)
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

        return new SearchQuery(mandatoryWords, optionalWords, excludedWords);
    }
}
