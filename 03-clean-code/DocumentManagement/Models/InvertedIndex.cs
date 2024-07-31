using Mohaymen.FullTextSearch.DocumentManagement.Interfaces;

namespace Mohaymen.FullTextSearch.DocumentManagement.Models;

public class InvertedIndex : IInvertedIndex
{
    private readonly Dictionary<Keyword, HashSet<string>> _invertedIndexMap = [];
    public HashSet<string> AllDocuments { get; } = [];

    public void AddDocumentToKeyword(Keyword keyword, string document)
    {
        AllDocuments.Add(document);

        if (!_invertedIndexMap.ContainsKey(keyword))
            _invertedIndexMap.Add(keyword, []);

        _invertedIndexMap[keyword].Add(document);
    }

    public HashSet<string> GetDocumentsByKeyword(Keyword keyword)
    {
        _invertedIndexMap.TryGetValue(keyword, out HashSet<string>? documents);

        return documents ?? [];
    }
}