namespace Mohaymen.FullTextSearch.DocumentManagement;

public interface ISearcher<T>
{
    ICollection<T> Search(SearchQuery query);
}