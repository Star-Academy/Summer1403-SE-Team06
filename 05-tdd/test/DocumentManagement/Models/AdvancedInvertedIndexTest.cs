using Mohaymen.FullTextSearch.DocumentManagement.Models;
using Mohaymen.FullTextSearch.DocumentManagement.Utilities;

namespace Mohaymen.FullTextSearch.Test.DocumentManagement.Models;

public class AdvancedInvertedIndexTest
{
    private readonly AdvancedInvertedIndex _advancedInvertedIndex;
    public AdvancedInvertedIndexTest()
    {
        _advancedInvertedIndex = new AdvancedInvertedIndex(new Tokenizer());
        _advancedInvertedIndex.AddDocumentToKeyword(
            new Keyword("key1"),
            new KeywordInfo("doc1", 1)
        );
        _advancedInvertedIndex.AddDocumentToKeyword(
            new Keyword("key1"),
            new KeywordInfo ("doc2", 3)
        );
        _advancedInvertedIndex.AddDocumentToKeyword(
            new Keyword("key2"),
            new KeywordInfo ("doc1", 2)
        );
        _advancedInvertedIndex.AddDocumentToKeyword(
            new Keyword("key2"),
            new KeywordInfo ("doc2", 2)
        );
    }
    
    [Fact]
    public void GetDocGetDocumentsByKeyword_ShouldWorkCorrectlyForPhrases()
    {
        // Act
        var documents1 = _advancedInvertedIndex.GetDocumentsByKeyword(new Keyword("key1 key2"));
        var documents2 = _advancedInvertedIndex.GetDocumentsByKeyword(new Keyword("key2 key1"));
        // Assert
        Assert.Equal(["doc1"], documents1);
        Assert.Equal(["doc2"], documents2);
    }

    [Fact]
    public void GetDocGetDocumentsByKeyword_ShouldWorkCorrectlyForWords()
    {
        var documents1 = _advancedInvertedIndex.GetDocumentsByKeyword(new Keyword("key1"));
        Assert.Equal(documents1, ["doc1", "doc2"]);
    }
}