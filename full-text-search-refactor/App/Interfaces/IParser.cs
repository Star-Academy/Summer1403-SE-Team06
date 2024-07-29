using Mohaymen.FullTextSearch.DocumentManagement.Models;

namespace Mohaymen.FullTextSearch.App.Interfaces;

public interface IInputParser
{
    SearchQuery ParseToSearchQuery(string input);
}