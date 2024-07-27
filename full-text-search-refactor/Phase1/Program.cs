using Mohaymen.FullTextSearch.DocumentManagement;
using System.Resources;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Mohaymen.FullTextSearch.Phase1;
class Program
{
    private static readonly ResourceManager ResourceManager = 
        new ("full_text_search_phase1.assets.Resources", Assembly.GetExecutingAssembly());
    private static readonly string FolderPath = ResourceManager.GetString("DocumentsPath") ??"";

    private static bool _isProgramRunning = true;
    private static ILogger? _logger;
    private static readonly InvertedIndex InvertedIndex = new();
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
        
        IEnumerable<FileData> filesContent;
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
        InvertedIndex.ProcessFilesWords(filesContent);
        _logger?.LogInformation("{fileCount} files loaded.", filesContent.Count());
        return true;
    }

    private static void InitializeLogger()
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        _logger = loggerFactory.CreateLogger("Program");
    }

    private static void HandleUserInput()
    {
        Console.Write("Enter your word (Enter !q to exit): ");
        var input = Console.ReadLine()?.Trim() ?? "";

        if (input == "!q")
        {
            _isProgramRunning = false;
            return;
        };
            
        HashSet<string> containingFiles = InvertedIndex.SearchWord(input);
        if(containingFiles.Count == 0)
        {
            Console.WriteLine("Word doesn't exist in any document");
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
}