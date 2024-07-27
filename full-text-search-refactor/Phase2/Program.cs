using System.Reflection;
using System.Resources;
using Microsoft.Extensions.Logging;
using Mohaymen.FullTextSearch.DocumentManagement;

namespace Mohaymen.FullTextSearch.Phase2;

internal class Program
{
    private static readonly ResourceManager ResourceManager =
        new("full_text_search_phase2.assets.Resources", Assembly.GetExecutingAssembly());

    private static readonly string FolderPath = ResourceManager.GetString("DocumentsPath") ?? "";
    private static ILogger? _logger;

    public static void Main()
    {
        InitializeLogger();

        FileCollection fileCollection;
        try
        {
            fileCollection = LoadFiles();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        InvertedIndex invertedIndex = IndexFiles(fileCollection);
        StartProgram(invertedIndex);
    }

    private static FileCollection LoadFiles()
    {
        var fileReader = new FileReader();

        FileCollection fileCollection;
        try
        {
            fileCollection = fileReader.ReadAllFiles(FolderPath);
            return fileCollection;
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
    }

    private static InvertedIndex IndexFiles(FileCollection fileCollection)
    {
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
            Console.Write("Enter your statement (Enter !q to exit): ");
            var input = Console.ReadLine()?.Trim() ?? "";

            if (input == "!q")
                break;

            var searchQuery = ParseInputToSearchQuery(input);
            ICollection<string> containingFiles = searcher.Search(searchQuery);

            if (containingFiles.Count == 0)
            {
                Console.WriteLine("No result for your statement");
                continue;
            }

            var count = containingFiles.Count;
            Console.WriteLine($"Word found in {count} file{(count > 1 ? "s" : "")}");
            Console.WriteLine("----------------------");
            foreach (var fileName in containingFiles) Console.WriteLine($"File '{fileName}'");
            Console.WriteLine("----------------------");
        }
    }

    private static SearchQuery ParseInputToSearchQuery(string input)
    {
        var mandatoryWords = new List<Keyword>();
        var optionalWords = new List<Keyword>();
        var excludedWords = new List<Keyword>();

        var words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        foreach (var word in words)
            if (word.StartsWith('+'))
                optionalWords.Add(new Keyword(word.Substring(1)));
            else if (word.StartsWith('-'))
                excludedWords.Add(new Keyword(word.Substring(1)));
            else
                mandatoryWords.Add(new Keyword(word));

        return new SearchQuery(mandatoryWords, optionalWords, excludedWords);
    }
}