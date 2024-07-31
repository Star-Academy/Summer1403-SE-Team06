using Mohaymen.FullTextSearch.DocumentManagement.Interfaces;
using Mohaymen.FullTextSearch.DocumentManagement.Models;

namespace Mohaymen.FullTextSearch.DocumentManagement.Services.InvertedIndexService;

public class InvertedIndexSearcher(IInvertedIndex invertedIndex) : ISearcher<string>
{
    public ICollection<string> Search(List<SearchQuery> queries)
    {
        var filteredDocuments = new HashSet<string>(invertedIndex.AllDocuments);
        
        foreach (var (searchStrategy, keywords) in queries)
        {
            searchStrategy.FilterDocuments(filteredDocuments, keywords, invertedIndex);
        }

        return filteredDocuments;
    }
    
}