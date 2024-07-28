using Mohaymen.FullTextSearch.DocumentManagement.Models;

namespace Mohaymen.FullTextSearch.App.Interfaces;

public interface IParser
{
    SearchQuery ParseInputToSearchQuery(string input);
}