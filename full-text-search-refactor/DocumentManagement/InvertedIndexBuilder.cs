using System.Text.RegularExpressions;
using Mohaymen.FullTextSearch.DocumentManagement;

namespace DocumentManagement;

public class InvertedIndexBuilder
{
    private readonly InvertedIndex _invertedIndex = new();

    public InvertedIndexBuilder ProcessFilesWords(FileCollection fileCollection)
    {
        foreach (var filePath in fileCollection.GetFilesPath())
        {
            var words = Regex.Split(fileCollection.GetFileContent(filePath), @"[^\w']+");

            words
                .Where(word => !string.IsNullOrWhiteSpace(word))
                .Select(word => new Keyword(word))
                .ToList()
                .ForEach(keyword => _invertedIndex.AddKeyword(keyword, filePath));
        }

        return this;
    }

    public InvertedIndex Build()
    {
        return _invertedIndex;
    }
}