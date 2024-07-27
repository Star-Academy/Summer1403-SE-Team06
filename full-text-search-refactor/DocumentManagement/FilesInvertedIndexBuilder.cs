using System.Text.RegularExpressions;

namespace Mohaymen.FullTextSearch.DocumentManagement;

public class FilesInvertedIndexBuilder : IInvertedIndexBuilder
{
    private readonly InvertedIndex _invertedIndex = new();

    public FilesInvertedIndexBuilder IndexFilesWords(FileCollection fileCollection)
    {
        foreach (var filePath in fileCollection.GetFilesPath())
        {
            var keywords = ExtractKeywords(fileCollection.GetFileContent(filePath));
            UpdateInvertedIndexMap(keywords, filePath);
        }

        return this;
    }

    public void UpdateInvertedIndexMap(List<Keyword> keywords, string filePath)
    {
        foreach (var keyword in keywords)
        {
            _invertedIndex.AddDocumentToKeyword(keyword, filePath);
        }
    }

    public List<Keyword> ExtractKeywords(string fileContent)
    {
        return Regex.Split(fileContent, @"[^\w']+")
                    .Where(word => !string.IsNullOrWhiteSpace(word))
                    .Select(word => new Keyword(word))
                    .ToList();
    }

    public InvertedIndex Build()
    {
        return _invertedIndex;
    }
}