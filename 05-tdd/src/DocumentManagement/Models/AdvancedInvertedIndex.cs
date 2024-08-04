using Mohaymen.FullTextSearch.DocumentManagement.Interfaces;

namespace Mohaymen.FullTextSearch.DocumentManagement.Models;

public class AdvancedInvertedIndex : IInvertedIndex
{
    private readonly Dictionary<Keyword, HashSet<KeywordInfo>> _invertedIndexMap = [];
    public HashSet<string> AllDocuments { get; } = [];
    private ITokenizer _tokenizer;

    public AdvancedInvertedIndex(ITokenizer tokenizer)
    {
        _tokenizer = tokenizer;
    }
    
    public void AddDocumentToKeyword(Keyword keyword, KeywordInfo keywordInfo)
    {
        AllDocuments.Add(keywordInfo.Document);

        if (!_invertedIndexMap.ContainsKey(keyword))
            _invertedIndexMap.Add(keyword, []);

        _invertedIndexMap[keyword].Add(keywordInfo);
    }
    public HashSet<string> GetDocumentsByKeyword(Keyword phrase)
    {
        var phraseWords = _tokenizer.ExtractKeywords(phrase.Word);

        if (!phraseWords.Any())
        {
            return [];
        }
        
        var keywordInfos = new HashSet<KeywordInfo>(_invertedIndexMap[phraseWords[0]]);
        
        for (int i=1; i<phraseWords.Count; i++)
        {
            var currentKeywordInfos = new HashSet<KeywordInfo>(_invertedIndexMap[phraseWords[i]]);
            currentKeywordInfos.RemoveWhere(keywordInfo =>
                !keywordInfos.Contains(new KeywordInfo(keywordInfo.Document, keywordInfo.Position - 1))
            );

            keywordInfos = currentKeywordInfos;
        }

        return keywordInfos.Select(keywordInfo => keywordInfo.Document).ToHashSet();
    }

    private bool Equals(AdvancedInvertedIndex other)
    {
        var areMapsEqual = _invertedIndexMap.Equals(other._invertedIndexMap);
        var areDocumentsEqual = AllDocuments.Equals(other.AllDocuments);
        return  areMapsEqual && areDocumentsEqual;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((AdvancedInvertedIndex)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_invertedIndexMap, AllDocuments);
    }
}