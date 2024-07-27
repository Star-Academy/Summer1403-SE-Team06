namespace Mohaymen.FullTextSearch.DocumentManagement;

public record Keyword
{
    public Keyword(string word)
    {
        Word = word.ToUpper();
    }

    public string Word { get; init; }
}