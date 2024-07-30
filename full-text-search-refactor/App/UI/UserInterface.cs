using Mohaymen.FullTextSearch.App.Interfaces;
using Mohaymen.FullTextSearch.Assets;
using Mohaymen.FullTextSearch.DocumentManagement.Interfaces;


namespace Mohaymen.FullTextSearch.App.UI;

public class UserInterface
{
    private readonly ISearcher<string> _searcher;
    private readonly IInputParser _parser;
    private const string QuitCommand = "!q";

    public UserInterface(ISearcher<string> searcher, IInputParser parser)
    {
        ArgumentNullException.ThrowIfNull(searcher);
        ArgumentNullException.ThrowIfNull(parser);
        _searcher = searcher;
        _parser = parser;
    }
    
    public void StartProgramLoop()
    {
        while (true)
        {
            var input = GetInput();
            
            if (input == QuitCommand)
                break;

            var containingFiles = GetContainingFiles(input);

            DisplayResult(containingFiles);
        }
    }

    private ICollection<string> GetContainingFiles(string input)
    {
        var searchQueries = _parser.ParseToSearchQuery(input);

        ICollection<string> containingFiles = _searcher.Search(searchQueries);
        return containingFiles;
    }

    private static void DisplayResult(ICollection<string> containingFiles)
    {
        if (containingFiles.Count == 0)
        {
            Console.WriteLine("No result for your statement");
            return;
        }

        var count = containingFiles.Count;
        Console.WriteLine($"Word found in {count} file{(count > 1 ? "s" : "")}");
        Console.WriteLine("----------------------");
        foreach (var filePath in containingFiles)
        {
            var documentsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Resources.DocumentsPath);
            var relativePath = Path.GetRelativePath(documentsPath, filePath);
            Console.WriteLine($"File '{relativePath}'");
        };
        Console.WriteLine("----------------------");
    }

    private static string GetInput()
    {
        Console.Write("Enter your statement (Enter !q to exit): ");
        var input = Console.ReadLine()?.Trim() ?? "";
        return input;
    }
}