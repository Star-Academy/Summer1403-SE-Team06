using Mohaymen.FullTextSearch.DocumentManagement.Interfaces;
using Mohaymen.FullTextSearch.DocumentManagement.Models;
using Mohaymen.FullTextSearch.DocumentManagement.Services.InvertedIndexService;
using Mohaymen.FullTextSearch.DocumentManagement.Services.InvertedIndexService.SearchStrategies;
using NSubstitute;

namespace Mohaymen.FullTextSearch.DocumentManagement.Test;

public class InvertedIndexSearcherTests
{
    private IInvertedIndex _invertedIndex;
    private InvertedIndexSearcher _searcher;

    public InvertedIndexSearcherTests()
    {
        _invertedIndex = CreateTestIndex();
        _searcher = new InvertedIndexSearcher(_invertedIndex);
    }

    private IInvertedIndex CreateTestIndex()
    {
        var invertedIndex = Substitute.For<IInvertedIndex>();
        
        var keywordDocumentMapping = new Dictionary<Keyword, HashSet<string>>
        {
            { new Keyword("star"), ["doc1.txt", "doc2.txt"] },
            { new Keyword("academy"), ["doc1.txt", "doc3.txt"] },
            { new Keyword("coder"), ["doc2.txt", "doc3.txt"] },
            { new Keyword("summer"), ["doc4.txt"] }
        };

        foreach (var entry in keywordDocumentMapping)
        {
            invertedIndex.GetDocumentsByKeyword(entry.Key)
                .Returns(entry.Value);
        }

        invertedIndex.AllDocuments.Returns(["doc1.txt", "doc2.txt", "doc3.txt", "doc4.txt"]);

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