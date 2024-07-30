using Mohaymen.FullTextSearch.DocumentManagement.Interfaces;
using Mohaymen.FullTextSearch.DocumentManagement.Models;

namespace Mohaymen.FullTextSearch.DocumentManagement.Services.InvertedIndexService.SearchStrategies;

public class OptionalSearchStrategy : ISearchStrategy
{
    public void FilterDocuments(HashSet<string> documents, List<Keyword> keywords, IInvertedIndex invertedIndex)
    {
        var optionalsSet = new HashSet<string>();
        foreach (var keyword in keywords)
        {
            HashSet<string> currentFiles = invertedIndex.GetDocumentsByKeyword(keyword);
            optionalsSet.UnionWith(currentFiles);
        }

        if (keywords.Count > 0)
        {
            documents.IntersectWith(optionalsSet);
        }
    }
}