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
        // var phraseWords = _tokenizer.ExtractKeywords(phrase.Word);
        // HashSet<KeywordInfo> keywordInfos = _invertedIndexMap[phraseWords[0]];
        // for (int i=1; i<phraseWords.Count; i++)
        // {
        //     HashSet<KeywordInfo> currentKeywordInfos = _invertedIndexMap[phraseWords[i]];
        //     foreach (var VARIABLE in COLLECTION)
        //     {
        //         
        //     }
        //     
        // }
        //
        //     
        // // for (int i = 0; i < keywords.Count; i++)
        // // {
        // //     
        // // }
        // var keywordInfo = _invertedIndexMap[keywords[0]];
    }
}