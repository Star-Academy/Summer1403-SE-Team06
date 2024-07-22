using Mohaymen.FullTextSearch.Shared;
using System.Resources;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Mohaymen.FullTextSearch.Phase1;
class Program
{
    private static readonly ResourceManager ResourceManager = new ResourceManager("full_text_search_phase1.assets.Resources", Assembly.GetExecutingAssembly());
    private static readonly string FolderPath = ResourceManager.GetString("DocumentsPath") ??"";
    public static void Main()
    {
        using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
        ILogger logger = factory.CreateLogger("Program");
        
        var fileReader = new FileReader();
        
        Dictionary<string, string> filesContent;
        try
        {
            filesContent = fileReader.ReadAllFiles(FolderPath);
        }
        catch (DirectoryNotFoundException)
        {
            logger.LogError("Wrong Folder Path: {path}", FolderPath);
            return;
        }
        catch(Exception exception)
        {
            Console.WriteLine(exception.Message);
            return;
        }
        
        logger.LogInformation("Processing files...");
        
        var invertedIndex = new InvertedIndex();
        invertedIndex.ProcessFilesWords(filesContent);
        
        while(true)
        {
            Console.Write("Enter your word: ");
            var input = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(input)) break;
            
            HashSet<string> containingFiles = invertedIndex.SearchWord(input);
            if(containingFiles.Count == 0)
            {
                Console.WriteLine("Word doesn't exist in any document");
                continue;
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
}