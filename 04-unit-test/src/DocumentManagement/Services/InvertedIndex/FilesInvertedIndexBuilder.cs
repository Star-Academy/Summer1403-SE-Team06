using Mohaymen.FullTextSearch.DocumentManagement.Interfaces;
using Mohaymen.FullTextSearch.DocumentManagement.Models;
using Mohaymen.FullTextSearch.DocumentManagement.Utilities;

namespace Mohaymen.FullTextSearch.DocumentManagement.Services.InvertedIndexService;

public class FilesInvertedIndexBuilder : IInvertedIndexBuilder
{
    private readonly InvertedIndex _invertedIndex = new();
    private readonly ITokenizer _tokenizer;

    public FilesInvertedIndexBuilder(ITokenizer tokenizer)
    {
        _tokenizer = tokenizer;
    }

    public FilesInvertedIndexBuilder IndexFilesWords(FileCollection fileCollection)
    {
        foreach (var filePath in fileCollection.GetFilesPath())
        {
            var keywords = _tokenizer.ExtractKeywords(fileCollection.GetFileContent(filePath));
            UpdateInvertedIndexMap(keywords, filePath);
        }

        return this;
    }

    private void UpdateInvertedIndexMap(List<Keyword> keywords, string filePath)
    {
        foreach (var keyword in keywords)
        {
            _invertedIndex.AddDocumentToKeyword(keyword, filePath);
        }
    }

    public IInvertedIndex Build()
    {
        return _invertedIndex;
    }
}