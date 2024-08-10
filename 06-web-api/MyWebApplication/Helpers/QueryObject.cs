namespace MyWebApplication.Helpers;

public class QueryObject
{
    public List<string> MandatoryWords { get; set; } = new();
    public List<string> ExcludedWords { get; set; } = new();
    public List<string> OptionalWords { get; set; } = new();
}