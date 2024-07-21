namespace Mohaymen.FullTextSearch;
class FileReader
{
    public Dictionary<string, string> ReadAllFiles(string folderPath)
    {
        var filesContent = new Dictionary<string, string>();
        string[] files = Directory.GetFiles(folderPath);
        foreach(var file in files)
        {
            string text = File.ReadAllText(file);
            filesContent.Add(file, text);
        }

        return filesContent;
    }
}