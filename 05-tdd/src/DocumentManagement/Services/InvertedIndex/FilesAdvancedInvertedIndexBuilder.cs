using Mohaymen.FullTextSearch.DocumentManagement.Interfaces;
using Mohaymen.FullTextSearch.DocumentManagement.Models;

namespace Mohaymen.FullTextSearch.DocumentManagement.Services.InvertedIndexService;

public class FilesAdvancedInvertedIndexBuilder : IInvertedIndexBuilder
{
    private AdvancedInvertedIndex _advancedInvertedIndex;
    private ITokenizer _tokenizer;

    public FilesAdvancedInvertedIndexBuilder(ITokenizer tokenizer)
    {
        _advancedInvertedIndex = new(tokenizer);
        _tokenizer = tokenizer;
    }
    
    public FilesAdvancedInvertedIndexBuilder IndexFilesWords(FileCollection fileCollection)
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
        for (var i = 0; i < keywords.Count; i++)
        {
            _advancedInvertedIndex.AddDocumentToKeyword(keywords[i], new KeywordInfo(filePath, i));    
        }
    }


    public IInvertedIndex Build()
    {
        return _advancedInvertedIndex;
    }
}