namespace Mohaymen.FullTextSearch.DocumentManagement;

public record SearchQuery(List<Keyword> Mandatories, List<Keyword> Optionals, List<Keyword> Excludeds);