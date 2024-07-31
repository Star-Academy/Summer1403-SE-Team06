using Mohaymen.FullTextSearch.DocumentManagement;
using System.Resources;
using System.Reflection;
using DocumentManagement;
using Microsoft.Extensions.Logging;

namespace Mohaymen.FullTextSearch.Phase2;
class Program
{
    private static readonly ResourceManager ResourceManager =
        new ("full_text_search_phase2.assets.Resources",
        Assembly.GetExecutingAssembly());
    private static readonly string FolderPath = ResourceManager.GetString("DocumentsPath") ??"";
    private static bool _isProgramRunning = true;
    private static ILogger? _logger;
    private static readonly AdvancedInvertedIndex AdvancedInvertedIndex = new();

    public static void Main()
    {
        InitializeLogger();

        if (!TryToLoadFilesAndIndexThem()) return;
        
        while(_isProgramRunning)
        {
            HandleUserInput();
        }
    }

    private static bool TryToLoadFilesAndIndexThem()
    {
        var fileReader = new FileReader();
        
        Dictionary<string, string> filesContent;
        try
        {
            filesContent = fileReader.ReadAllFiles(FolderPath);
        }
        catch (DirectoryNotFoundException exception)
        {
            _logger?.LogError(exception, "Wrong Folder Path: {path}", FolderPath);
            return false;
        }
        catch(Exception exception)
        {
            _logger?.LogError(exception, "An error occurred while reading files.");
            return false;
        }
        
        _logger?.LogInformation("Processing files...");
        AdvancedInvertedIndex.ProcessFilesWords(filesContent);
        _logger?.LogInformation("{fileCount} files loaded.", filesContent.Count);
        return true;
    }

    private static void InitializeLogger()
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        _logger = loggerFactory.CreateLogger("Program");
    }

    private static void HandleUserInput()
    {
        Console.Write("Enter your statement (Enter !q to exit): ");
        var input = Console.ReadLine()?.Trim() ?? "";

        if (input == "!q")
        {
            _isProgramRunning = false;
            return;
        };

        var searchQuery = ParseToSearchQuery(input);
            
        HashSet<string> containingFiles = AdvancedInvertedIndex.AdvancedSearch(searchQuery);
        if(containingFiles.Count == 0)
        {
            Console.WriteLine("No result for your statement");
            return;
        }
            
        var count = containingFiles.Count;
        Console.WriteLine($"Word found in {count} file{(count > 1 ? "s" : "")}");
        Console.WriteLine("----------------------");
        foreach(var fileName in containingFiles)
        {
            Console.WriteLine($"File '{fileName}'");
        }
        Console.WriteLine("----------------------");
    }

    private static SearchQuery ParseToSearchQuery(string input)
    {
        var mandatoryWords = new List<string>();
        var optionalWords = new List<string>();
        var excludedWords = new List<string>();
        
        var words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        foreach (var word in words)
        {
            if (word.StartsWith("+"))
            {
                optionalWords.Add(word.Substring(1));
            }
            else if (word.StartsWith("-"))
            {
                excludedWords.Add(word.Substring(1));
            }
            else
            {
                mandatoryWords.Add(word);
            }
        }
        
        return new SearchQuery(mandatoryWords, optionalWords, excludedWords);
    }
}