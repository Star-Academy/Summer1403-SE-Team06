namespace Mohaymen.FullTextSearch.DocumentManagement.Models;

public record SearchQuery(List<Keyword> Mandatories, List<Keyword> Optionals, List<Keyword> Excludeds);