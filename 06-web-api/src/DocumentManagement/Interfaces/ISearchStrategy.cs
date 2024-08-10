using Mohaymen.FullTextSearch.DocumentManagement.Models;

namespace Mohaymen.FullTextSearch.DocumentManagement.Interfaces;

public interface ISearchStrategy
{
    void FilterDocuments(HashSet<string> documents, List<Keyword> keywords, IInvertedIndex invertedIndex);
}