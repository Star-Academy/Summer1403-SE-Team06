namespace Mohaymen.FullTextSearch.DocumentManagement;

public class FileReader
{
    public Dictionary<string, string> ReadAllFiles(string folderPath)
    {
        var filesContent = Directory
            .GetFiles(folderPath)
            .ToDictionary(file => file, file => File.ReadAllText(file));
            
        return filesContent;
    }
}