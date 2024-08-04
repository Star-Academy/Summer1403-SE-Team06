using Mohaymen.FullTextSearch.DocumentManagement.Models;

namespace Mohaymen.FullTextSearch.DocumentManagement.Interfaces;

public interface IInvertedIndex
{
    HashSet<string> AllDocuments { get; }
    HashSet<string> GetDocumentsByKeyword(Keyword keyword);
}