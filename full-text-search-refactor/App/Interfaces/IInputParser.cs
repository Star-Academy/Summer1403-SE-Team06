using Mohaymen.FullTextSearch.DocumentManagement.Models;

namespace Mohaymen.FullTextSearch.App.Interfaces;

public interface IInputParser
{
    List<SearchQuery> ParseToSearchQuery(string input);
}