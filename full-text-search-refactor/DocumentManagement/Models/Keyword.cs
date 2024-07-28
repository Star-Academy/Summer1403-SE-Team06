namespace Mohaymen.FullTextSearch.DocumentManagement.Models;

public record Keyword
{
    public Keyword(string word)
    {
        Word = word.ToUpper();
    }

    public string Word { get; init; }
}