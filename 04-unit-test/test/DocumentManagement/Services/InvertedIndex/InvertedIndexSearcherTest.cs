using Mohaymen.FullTextSearch.DocumentManagement.Interfaces;
using Mohaymen.FullTextSearch.DocumentManagement.Models;
using Mohaymen.FullTextSearch.DocumentManagement.Services.InvertedIndexService;
using Mohaymen.FullTextSearch.DocumentManagement.Services.InvertedIndexService.SearchStrategies;
using NSubstitute;

namespace Mohaymen.FullTextSearch.Test.DocumentManagement.Services.InvertedIndex;

public class InvertedIndexSearcherTest
{
    private IInvertedIndex _invertedIndex;
    private InvertedIndexSearcher _searcher;

    public InvertedIndexSearcherTest()
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
    public void Search_WithMultipleStrategies_CombinesFiltersCorrectly()
    {
        // Arrange
        var mandatorySearchStrategy = new MandatorySearchStrategy();
        var optionalSearchStrategy = new OptionalSearchStrategy();
        var excludedSearchStrategy = new ExcludedSearchStrategy();
        var mandatoryKeywords = new List<Keyword> { new Keyword("academy") };
        var optionalKeywords = new List<Keyword> { new Keyword("coder") };
        var excludedKeywords = new List<Keyword> { new Keyword("star") };
        
        var queries = new List<SearchQuery>
        {
            new SearchQuery(mandatorySearchStrategy, mandatoryKeywords),
            new SearchQuery(optionalSearchStrategy, optionalKeywords),
            new SearchQuery(excludedSearchStrategy,  excludedKeywords)
        };

        // Act
        var results = _searcher.Search(queries);

        // Assert
        var expected = new HashSet<string> { "doc3.txt" };
        Assert.True(expected.SetEquals(results), $"Expected: {string.Join(", ", expected)}, Actual: {string.Join(", ", results)}");
    }
}