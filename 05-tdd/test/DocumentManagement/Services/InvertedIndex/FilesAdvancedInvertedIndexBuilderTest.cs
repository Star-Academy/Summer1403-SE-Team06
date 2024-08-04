using Mohaymen.FullTextSearch.DocumentManagement.Interfaces;
using Mohaymen.FullTextSearch.DocumentManagement.Models;
using Mohaymen.FullTextSearch.DocumentManagement.Services.InvertedIndexService;
using Mohaymen.FullTextSearch.DocumentManagement.Utilities;
using NSubstitute;

namespace Mohaymen.FullTextSearch.Test.DocumentManagement.Services.InvertedIndex;

public class FilesAdvancedInvertedIndexBuilderTest
{
    private readonly ITokenizer _tokenizer;
    private readonly FilesAdvancedInvertedIndexBuilder _filesAdvancedInvertedIndexBuilder;
    
    public FilesAdvancedInvertedIndexBuilderTest()
    {
        _tokenizer = new Tokenizer();
        _filesAdvancedInvertedIndexBuilder = new FilesAdvancedInvertedIndexBuilder(_tokenizer);
    }
    
    [Fact]
    public void IndexFilesWords_ValidFileCollection_ShouldIndexAllWords()
    {
        // Arrange
        var fileCollection = new FileCollection();
        fileCollection.AddFile("doc1.txt", "star academy star");
        fileCollection.AddFile("doc2.txt", "star coder academy");
        fileCollection.AddFile("doc3.txt", "academy coder");
        fileCollection.AddFile("doc4.txt", "summer");
        
        // _tokenizer.ExtractKeywords("star academy star")
        //     .Returns([new Keyword("star"), new Keyword("academy"), new Keyword("star")]);
        // _tokenizer.ExtractKeywords("star coder")
        //     .Returns([new Keyword("star"), new Keyword("coder")]);
        // _tokenizer.ExtractKeywords("academy coder")
        //     .Returns([new Keyword("academy"), new Keyword("coder")]);
        // _tokenizer.ExtractKeywords("summer")
        //     .Returns([new Keyword("summer")]);

        var expectedInvertedIndex = new AdvancedInvertedIndex(_tokenizer);
        expectedInvertedIndex.AddDocumentToKeyword(new Keyword("star"), new KeywordInfo("doc1.txt", 0));
        expectedInvertedIndex.AddDocumentToKeyword(new Keyword("academy"), new KeywordInfo("doc1.txt", 1));
        expectedInvertedIndex.AddDocumentToKeyword(new Keyword("star"), new KeywordInfo("doc1.txt", 1));
        expectedInvertedIndex.AddDocumentToKeyword(new Keyword("star"), new KeywordInfo("doc2.txt", 0));
        expectedInvertedIndex.AddDocumentToKeyword(new Keyword("coder"), new KeywordInfo("doc2.txt", 1));
        expectedInvertedIndex.AddDocumentToKeyword(new Keyword("academy"), new KeywordInfo("doc2.txt", 1));
        expectedInvertedIndex.AddDocumentToKeyword(new Keyword("academy"), new KeywordInfo("doc3.txt", 0));
        expectedInvertedIndex.AddDocumentToKeyword(new Keyword("coder"), new KeywordInfo("doc3.txt", 1));
        expectedInvertedIndex.AddDocumentToKeyword(new Keyword("summer"), new KeywordInfo("doc4.txt", 0));
        
        // Act
        IInvertedIndex invertedIndex = _filesAdvancedInvertedIndexBuilder.IndexFilesWords(fileCollection).Build();
        
        // Assert
        Assert.Equal(expectedInvertedIndex, invertedIndex);
    }
}