using Mohaymen.FullTextSearch.DocumentManagement.Models;

namespace Mohaymen.FullTextSearch.DocumentManagement.Interfaces;

public interface IInvertedIndex
{
    HashSet<string> AllDocuments { get; }
    void AddDocumentToKeyword(Keyword keyword, string document);
    HashSet<string> GetDocumentsByKeyword(Keyword keyword);
}