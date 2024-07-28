using Mohaymen.FullTextSearch.DocumentManagement.Models;
namespace Mohaymen.FullTextSearch.DocumentManagement.Interfaces;

public interface ISearcher<T>
{
    ICollection<T> Search(SearchQuery query);
}