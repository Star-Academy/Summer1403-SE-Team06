using Mohaymen.FullTextSearch.DocumentManagement.Interfaces;
using Mohaymen.FullTextSearch.DocumentManagement.Models;
using Mohaymen.FullTextSearch.DocumentManagement.Services.InvertedIndexService;
using NSubstitute;

namespace Mohaymen.FullTextSearch.Test.DocumentManagement.Services.InvertedIndex;

public class FilesInvertedIndexBuilderTest
{
    private readonly ITokenizer _tokenizer;
    private readonly FilesInvertedIndexBuilder _filesInvertedIndexBuilder;
    
    public FilesInvertedIndexBuilderTest()
    {
        _tokenizer = Substitute.For<ITokenizer>();
        _filesInvertedIndexBuilder = new FilesInvertedIndexBuilder(_tokenizer);
    }

    [Fact]
    public void IndexFilesWords_ValidFileCollection_ShouldIndexAllWords()
    {
        // Arrange
        var fileCollection = new FileCollection();
        fileCollection.AddFile("doc1.txt", "star academy star");
        fileCollection.AddFile("doc2.txt", "star coder");
        fileCollection.AddFile("doc3.txt", "academy coder");
        fileCollection.AddFile("doc4.txt", "summer");
        
        _tokenizer.ExtractKeywords("star academy star")
            .Returns([new Keyword("star"), new Keyword("academy"), new Keyword("star")]);
        _tokenizer.ExtractKeywords("star coder")
            .Returns([new Keyword("star"), new Keyword("coder")]);
        _tokenizer.ExtractKeywords("academy coder")
            .Returns([new Keyword("academy"), new Keyword("coder")]);
        _tokenizer.ExtractKeywords("summer")
            .Returns([new Keyword("summer")]);
        
        // Act
        IInvertedIndex invertedIndex = _filesInvertedIndexBuilder.IndexFilesWords(fileCollection).Build();
        
        // Assert
        Assert.Equal(["doc1.txt", "doc2.txt"], invertedIndex.GetDocumentsByKeyword(new Keyword("star")));
        Assert.Equal(["doc1.txt", "doc3.txt"], invertedIndex.GetDocumentsByKeyword(new Keyword("academy")));
        Assert.Equal(["doc2.txt", "doc3.txt"], invertedIndex.GetDocumentsByKeyword(new Keyword("coder")));
        Assert.Equal(["doc4.txt"], invertedIndex.GetDocumentsByKeyword(new Keyword("summer")));
    }
}