using System.IO;
using System.Text.RegularExpressions;
using System.Text.Json;
class FileReader
{
    public Dictionary<string, string> ReadAllFiles(string folderPath)
    {
        var files = Directory.GetFiles(folderPath);
        Dictionary<string, string> filesContent = new Dictionary<string, string>();
        foreach(var file in files)
        {
            var text = File.ReadAllText(file);
            filesContent.Add(file, text);
        }

        return filesContent;
    }
}

class InvertedIndex
{
    public Dictionary<string, HashSet<string>> InvertedIndexMap {get; set;}
    public InvertedIndex()
    {
        InvertedIndexMap = new Dictionary<string, HashSet<string>>();
    }

    public void ProcessFilesWords(Dictionary<string, string> filesContent)
    {
        foreach(var file in filesContent)
        {
            string filePath = file.Key;
            string fileText = file.Value;
            var words = Regex.Split(fileText, @"[^\w']+");

            foreach(var word in words)
            {
                string upperWord = word.ToUpper();
                if(!InvertedIndexMap.ContainsKey(upperWord))
                {
                    InvertedIndexMap.Add(upperWord, new HashSet<string>());
                }

                InvertedIndexMap[upperWord].Add(filePath);
            }
        }

        if(InvertedIndexMap.ContainsKey(""))
        {
            InvertedIndexMap.Remove("");
        }
    }

    public HashSet<string> SearchWord(string word)
    {
        string upperWord = word.ToUpper();
        try
        {
            return InvertedIndexMap[upperWord];
        }
        catch(KeyNotFoundException)
        {
            return new HashSet<string>();
        }
    }
}

class Program
{
    public static void Main()
    {
        string folderPath = "EnglishData";
        FileReader fileReader = new FileReader();
        var filesContent = fileReader.ReadAllFiles(folderPath);

        InvertedIndex invertedIdx = new InvertedIndex();
        invertedIdx.ProcessFilesWords(filesContent);

        var options = new JsonSerializerOptions{ WriteIndented = true };

        string input = Console.ReadLine();
        while(!string.IsNullOrEmpty(input))
        {
            var containingFiles = invertedIdx.SearchWord(input);
    
            Console.WriteLine(JsonSerializer.Serialize(containingFiles, options));
            input = Console.ReadLine();
        }
    }
}
