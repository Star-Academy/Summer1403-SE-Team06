﻿using Mohaymen.FullTextSearch.DocumentManagement.Interfaces;
using Mohaymen.FullTextSearch.DocumentManagement.Services.InvertedIndexService.SearchStrategies;
using Mohaymen.FullTextSearch.DocumentManagement.Models;
using NSubstitute;

namespace Mohaymen.FullTextSearch.Test.DocumentManagement.Services.InvertedIndex.SearchStrategies;

public class ExcludedSearchStrategyTest
{
    private ISearchStrategy _searchStrategy;
    private IInvertedIndex _invertedIndex;

    public ExcludedSearchStrategyTest()
    {
        _searchStrategy = new ExcludedSearchStrategy();
        _invertedIndex = CreateTestIndex();
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
    public void FilterDocuments_ShouldExcludeDocumentsWithSpecificKeywords()
    {
        // Arrange
        var documents = new HashSet<string>(_invertedIndex.AllDocuments);
        var keywords = new List<Keyword> { new Keyword("star") };
        var expected = new HashSet<string> { "doc3.txt", "doc4.txt" };

        // Act
        _searchStrategy.FilterDocuments(documents, keywords, _invertedIndex);

        // Assert
        Assert.True(expected.SetEquals(documents), $"Expected: {string.Join(", ", expected)}, Actual: {string.Join(", ", documents)}");
    }
}