using System.IO;
using System.Text.Json;

namespace Mohaymen.FullTextSearch;
class Program
{
    private const string FolderPath = "EnglishData";
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

        var invertedIndex = new InvertedIndex();
        invertedIndex.ProcessFilesWords(filesContent);

        Console.Write("Enter your word: ");
        string input = Console.ReadLine();
        while(!string.IsNullOrEmpty(input))
        {
            HashSet<string> containingFiles = invertedIndex.SearchWord(input);
            if(containingFiles.Count == 0)
            {
                Console.WriteLine("Word doesn't exist in any document");
            }

            foreach(var file in containingFiles)
            {
                Console.WriteLine(file);
            }

            Console.Write("\nEnter your word: ");
            input = Console.ReadLine();
        }
    }
}
