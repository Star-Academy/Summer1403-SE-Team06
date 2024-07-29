using Mohaymen.FullTextSearch.DocumentManagement.Interfaces;

namespace Mohaymen.FullTextSearch.DocumentManagement.Models;

public record SearchQuery(ISearchStrategy SearchStrategy, List<Keyword> Keywords);