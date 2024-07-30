using Mohaymen.FullTextSearch.DocumentManagement.Interfaces;
using Mohaymen.FullTextSearch.DocumentManagement.Models;
using Mohaymen.FullTextSearch.DocumentManagement.Services.InvertedIndexService;
using Mohaymen.FullTextSearch.DocumentManagement.Services.InvertedIndexService.SearchStrategies;
using Mohaymen.FullTextSearch.DocumentManagement.Utilities;
using NSubstitute;

namespace Mohaymen.FullTextSearch.DocumentManagement.Test;

public class TokenizersTests
{
    public static IEnumerable<object[]> GetTestData()
    {
        yield return ["sta'r", new List<Keyword> { new ("sta'r") }];
        yield return ["'star", new List<Keyword> { new ("'star") }];
        yield return ["st 'ar", new List<Keyword> { new ("st"), new ("'ar") }];
        yield return ["st ' ar", new List<Keyword> { new ("st"), new ("ar") }];
    }

    [Theory]
    [MemberData(nameof(GetTestData))]
    public void ExtractKeywords_ShouldTokenizeText_WhenStringContainsSingleQuotation(string text, List<Keyword> expectedKeywords)
    {
        //Act
        var keywords = Tokenizer.ExtractKeywords(text);
      
        //Assert
        Assert.Equal(expectedKeywords, keywords);
    }
}

public class InvertedIndexSearcherTests
{
    private InvertedIndex _invertedIndex;
    private InvertedIndexSearcher _searcher;

    public InvertedIndexSearcherTests()
    {
        _invertedIndex = CreateTestIndex();
        _searcher = new InvertedIndexSearcher(_invertedIndex);
    }

    private InvertedIndex CreateTestIndex()
    {
        var invertedIndex = new InvertedIndex();

        invertedIndex.AddDocumentToKeyword(new ("star"), "doc1.txt");
        invertedIndex.AddDocumentToKeyword(new ("academy"), "doc1.txt");
        invertedIndex.AddDocumentToKeyword(new ("star"), "doc2.txt");
        invertedIndex.AddDocumentToKeyword(new ("coder"), "doc2.txt");
        invertedIndex.AddDocumentToKeyword(new ("academy"), "doc3.txt");
        invertedIndex.AddDocumentToKeyword(new ("coder"), "doc3.txt");
        invertedIndex.AddDocumentToKeyword(new ("summer"), "doc4.txt");

        return invertedIndex;
    }

    [Fact]
    public void Search_ExcludedStrategy_ShouldExcludeDocumentsWithSpecificKeywords()
    {
        // Arrange
        var searchStrategy = new ExcludedSearchStrategy();
        var keywords = new List<Keyword> { new Keyword("star") };
        
        var queries = new List<SearchQuery>
        {
            new SearchQuery(searchStrategy,  keywords)
        };

        // Act
        var results = _searcher.Search(queries);

        // Assert
        var expected = new HashSet<string> { "doc3.txt", "doc4.txt" };
        Assert.True(expected.SetEquals(results), $"Expected: {string.Join(", ", expected)}, Actual: {string.Join(", ", results)}");
    }

    [Fact]
    public void Search_MandatoryStrategy_ShouldIncludeOnlyDocumentsContainingAllKeywords()
    {
        // Arrange
        var searchStrategy = new MandatorySearchStrategy();
        var keywords = new List<Keyword> { new Keyword("academy"), new Keyword("coder") };
        
        var queries = new List<SearchQuery>
        {
            new SearchQuery(searchStrategy, keywords)
        };

        // Act
        var results = _searcher.Search(queries);

        // Assert
        var expected = new HashSet<string> { "doc3.txt" };
        Assert.True(expected.SetEquals(results), $"Expected: {string.Join(", ", expected)}, Actual: {string.Join(", ", results)}");
    }

    [Fact]
    public void Search_OptionalStrategy_ShouldIncludeDocumentsContainingAnyKeywords()
    {
        // Arrange
        var searchStrategy = new OptionalSearchStrategy();
        var keywords = new List<Keyword> { new Keyword("academy"), new Keyword("summer") };
        
        var queries = new List<SearchQuery>
        {
            new SearchQuery(searchStrategy, keywords)
        };

        // Act
        var results = _searcher.Search(queries);

        // Assert
        var expected = new HashSet<string> { "doc1.txt", "doc3.txt", "doc4.txt" };
        Assert.True(expected.SetEquals(results), $"Expected: {string.Join(", ", expected)}, Actual: {string.Join(", ", results)}");
    }
}