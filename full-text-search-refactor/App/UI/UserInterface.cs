using Mohaymen.FullTextSearch.App.Utilities;
using Mohaymen.FullTextSearch.Assets;
using Mohaymen.FullTextSearch.DocumentManagement.Interfaces;
using Mohaymen.FullTextSearch.DocumentManagement.Models;
using Mohaymen.FullTextSearch.DocumentManagement.Services.InvertedIndexService;

namespace Mohaymen.FullTextSearch.App.UI;

public static class UserInterface
{
    public static void StartProgramLoop(IInvertedIndex invertedIndex)
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
            foreach (var filePath in containingFiles)
            {
                var documentsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Resources.DocumentsPath);
                var relativePath = Path.GetRelativePath(documentsPath, filePath);
                Console.WriteLine($"File '{relativePath}'");
            };
            Console.WriteLine("----------------------");
        }
    }
}