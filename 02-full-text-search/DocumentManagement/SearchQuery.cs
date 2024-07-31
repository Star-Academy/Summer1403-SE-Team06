namespace DocumentManagement;

public record SearchQuery(List<string> Mandatories, List<string> Optionals, List<string> Excludeds);