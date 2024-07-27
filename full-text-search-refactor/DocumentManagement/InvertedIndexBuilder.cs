using System.Text.RegularExpressions;
using Mohaymen.FullTextSearch.DocumentManagement;

namespace DocumentManagement;

public class InvertedIndexBuilder
{
    private readonly InvertedIndex _invertedIndex = new();

    public InvertedIndexBuilder IndexFilesWords(FileCollection fileCollection)
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
        foreach(var keyword in keywords)
        {
            _invertedIndex.AssociateKeywordWithDocument(keyword, filePath);
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