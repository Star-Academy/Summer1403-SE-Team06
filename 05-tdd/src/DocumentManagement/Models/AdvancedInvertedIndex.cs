using Mohaymen.FullTextSearch.DocumentManagement.Interfaces;

namespace Mohaymen.FullTextSearch.DocumentManagement.Models;

public class AdvancedInvertedIndex : IInvertedIndex, IEquatable<AdvancedInvertedIndex>
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
        
        foreach (var phraseWord in phraseWords)
        {
            if (!_invertedIndexMap.ContainsKey(phraseWord))
            {
                return [];
            }
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
    

    public bool Equals(AdvancedInvertedIndex? other)
    {
        if (other is null) return false;
        
        if (_invertedIndexMap.Count != other._invertedIndexMap.Count)
            return false;
    
        foreach (var kvp in _invertedIndexMap)
        {
            if (!other._invertedIndexMap.TryGetValue(kvp.Key, out var otherSet))
                return false;
    
            if (!kvp.Value.SetEquals(otherSet))
                return false;
        }
    
        var areDocumentsEqual = AllDocuments.SetEquals(other.AllDocuments);
        return areDocumentsEqual;
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