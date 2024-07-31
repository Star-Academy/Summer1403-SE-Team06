using Mohaymen.FullTextSearch.DocumentManagement.Models;
using Mohaymen.FullTextSearch.DocumentManagement.Utilities;

namespace Mohaymen.FullTextSearch.DocumentManagement.Test.DocumentManagement.Utilities;

public class TokenizersTests
{
    public static IEnumerable<object[]> GetTestData()
    {
        yield return ["sta'r", new List<Keyword> { new ("sta'r") }];
        yield return ["'star", new List<Keyword> { new ("'star") }];
        yield return ["st 'ar", new List<Keyword> { new ("st"), new ("'ar") }];
        yield return ["st'  ar", new List<Keyword> { new("st'"), new("ar") }];
    }

    [Theory]
    [MemberData(nameof(GetTestData))]
    public void ExtractKeywords_ShouldTokenizeText_WhenStringContainsSingleQuotation(string text, List<Keyword> expectedKeywords)
    {
        //Arrange
        var tokenizer = new Tokenizer();
        
        //Act
        var keywords = tokenizer.ExtractKeywords(text);
      
        //Assert
        Assert.Equal(expectedKeywords, keywords);
    }
}