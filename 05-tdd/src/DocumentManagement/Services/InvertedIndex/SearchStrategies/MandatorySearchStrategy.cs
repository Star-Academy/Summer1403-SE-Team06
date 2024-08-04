using Mohaymen.FullTextSearch.DocumentManagement.Interfaces;
using Mohaymen.FullTextSearch.DocumentManagement.Models;

namespace Mohaymen.FullTextSearch.DocumentManagement.Services.InvertedIndexService.SearchStrategies;

public class MandatorySearchStrategy : ISearchStrategy
{
    public void FilterDocuments(HashSet<string> documents, List<Keyword> keywords, IInvertedIndex invertedIndex)
    {
        foreach (var keyword in keywords)
        {
            HashSet<string> currentFiles = invertedIndex.GetDocumentsByKeyword(keyword);
            documents.IntersectWith(currentFiles);
        }
    }
}