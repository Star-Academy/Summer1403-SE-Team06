namespace Mohaymen.FullTextSearch.DocumentManagement;

public class FileCollection
{
    private readonly Dictionary<string, string> _filesDataDictionary = [];

    public void AddFile(string filePath, string fileContent)
    {
        _filesDataDictionary.Add(filePath, fileContent);
    }

    public List<string> GetFilesPath()
    {
        return _filesDataDictionary.Keys.ToList();
    }

    public string GetFileContent(string filePath)
    {
        return _filesDataDictionary[filePath];
    }

    public bool ContainsFile(string filePath)
    {
        return _filesDataDictionary.ContainsKey(filePath);
    }

    public int FilesCount()
    {
        return _filesDataDictionary.Count;
    }
}