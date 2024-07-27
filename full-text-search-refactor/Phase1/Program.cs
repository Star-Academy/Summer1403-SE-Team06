using System.Reflection;
using System.Resources;
using DocumentManagement;
using Microsoft.Extensions.Logging;
using Mohaymen.FullTextSearch.DocumentManagement;

namespace Mohaymen.FullTextSearch.Phase1;

internal class Program
{
    private static readonly ResourceManager ResourceManager =
        new("full_text_search_phase1.assets.Resources", Assembly.GetExecutingAssembly());

    private static readonly string FolderPath = ResourceManager.GetString("DocumentsPath") ?? "";
    private static ILogger? _logger;

    public static void Main()
    {
        InitializeLogger();
        try
        {
            var invertedIndex = LoadFilesAndIndexThem();
            StartProgram(invertedIndex);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private static InvertedIndex LoadFilesAndIndexThem()
    {
        var fileReader = new FileReader();

        FileCollection fileCollection;
        try
        {
            fileCollection = fileReader.ReadAllFiles(FolderPath);
        }
        catch (DirectoryNotFoundException exception)
        {
            _logger?.LogError(exception, "Wrong Folder Path: {path}", FolderPath);
            throw;
        }
        catch (Exception exception)
        {
            _logger?.LogError(exception, "An error occurred while reading files.");
            throw;
        }

        _logger?.LogInformation("Processing files...");
        var invertedIndexBuilder = new FilesInvertedIndexBuilder();
        var invertedIndex = invertedIndexBuilder.IndexFilesWords(fileCollection).Build();
        _logger?.LogInformation("{fileCount} files loaded.", fileCollection.FilesCount());
        return invertedIndex;
    }

    private static void InitializeLogger()
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        _logger = loggerFactory.CreateLogger("Program");
    }

    private static void StartProgram(InvertedIndex invertedIndex)
    {
        var searcher = new InvertedIndexSearcher(invertedIndex);

        while (true)
        {
            Console.Write("Enter your word (Enter !q to exit): ");
            var input = Console.ReadLine()?.Trim() ?? "";

            if (input == "!q") break;

            var searchQuery = new SearchQuery([new Keyword(input)], [], []);

            ICollection<string> containingDocuments = searcher.Search(searchQuery);

            if (containingDocuments.Count == 0)
            {
                Console.WriteLine("Word doesn't exist in any document");
                continue;
            }

            var count = containingDocuments.Count;
            Console.WriteLine($"Word found in {count} file{(count > 1 ? "s" : "")}");
            Console.WriteLine("----------------------");
            foreach (var fileName in containingDocuments) Console.WriteLine($"File '{fileName}'");
            Console.WriteLine("----------------------");
        }
    }
}