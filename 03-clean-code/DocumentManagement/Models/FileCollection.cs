namespace Mohaymen.FullTextSearch.DocumentManagement.Models;

public class FileCollection
{
    private readonly Dictionary<string, string> _filesDataDictionary = [];

    public void AddFile(string filePath, string fileContent) =>
        _filesDataDictionary.Add(filePath, fileContent);

    public List<string> GetFilesPath() =>
        _filesDataDictionary.Keys.ToList();

    public string GetFileContent(string filePath) =>
        _filesDataDictionary[filePath];

    public bool ContainsFile(string filePath) =>
        _filesDataDictionary.ContainsKey(filePath);

    public int FilesCount() =>
        _filesDataDictionary.Count;
}