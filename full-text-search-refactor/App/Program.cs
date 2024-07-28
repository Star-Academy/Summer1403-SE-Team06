using Microsoft.Extensions.Logging;
using Mohaymen.FullTextSearch.DocumentManagement;
using Mohaymen.FullTextSearch.App.Utilities;
using Mohaymen.FullTextSearch.DocumentManagement.Models;
using Mohaymen.FullTextSearch.DocumentManagement.Services.InvertedIndexService;
using Mohaymen.FullTextSearch.App.Services;

namespace Mohaymen.FullTextSearch.App;

internal class Program
{
    public static void Main()
    {
        FileCollection fileCollection;
        try
        {
            fileCollection = FileLoader.LoadFiles();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return;
        }

        InvertedIndex invertedIndex = IndexFiles(fileCollection);
        StartProgram(invertedIndex);
    }
    
    public static InvertedIndex IndexFiles(FileCollection fileCollection)
    {
        Logging.Logger.LogInformation("Processing files...");
        var invertedIndexBuilder = new FilesInvertedIndexBuilder();
        var invertedIndex = invertedIndexBuilder.IndexFilesWords(fileCollection).Build();
        Logging.Logger.LogInformation("{fileCount} files loaded.", fileCollection.FilesCount());
        return invertedIndex;
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

            var searchQuery = Parser.ParseInputToSearchQuery(input);
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
}