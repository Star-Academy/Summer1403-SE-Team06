using Mohaymen.FullTextSearch.DocumentManagement.Models;

namespace Mohaymen.FullTextSearch.DocumentManagement.Interfaces;

public interface ITokenizer
{
    List<Keyword> ExtractKeywords(string text);
}