using Mohaymen.FullTextSearch.DocumentManagement;
using System.Resources;
using System.Reflection;

namespace Mohaymen.FullTextSearch.Phase2;
class Program
{
    private static readonly ResourceManager ResourceManager = new ResourceManager("full_text_search_phase2.assets.Resources", Assembly.GetExecutingAssembly());
    private static readonly string FolderPath = ResourceManager.GetString("DocumentsPath") ??"";

    public static void Main()
    {
        var fileReader = new FileReader();
        
        Dictionary<string, string> filesContent;
        try
        {
            filesContent = fileReader.ReadAllFiles(FolderPath);
        }
        catch(Exception exception)
        {
            Console.WriteLine(exception.Message);
            return;
        }
        
        Console.WriteLine("Processing files...");
        
        var advancedInvertedIndex = new AdvancedInvertedIndex();
        advancedInvertedIndex.ProcessFilesWords(filesContent);
        
        while(true)
        {
            Console.Write("Enter your statement: ");
            var input = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(input)) break;

            var (mandatoryWords, optionalWords, excludedWords) = SplitInput(input);
            
            HashSet<string> containingFiles = advancedInvertedIndex.AdvancedSearch(mandatoryWords, optionalWords, excludedWords);
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

    private static (List<string> mandatories, List<string> optionals, List<string> excludeds) SplitInput(string input)
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
        
        return (mandatoryWords, optionalWords, excludedWords);
    }
}