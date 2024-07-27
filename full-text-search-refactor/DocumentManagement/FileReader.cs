namespace Mohaymen.FullTextSearch.DocumentManagement;

public class FileReader
{
    public IEnumerable<FileData> ReadAllFiles(string folderPath)
    {
        var filesData = Directory
            .GetFiles(folderPath)
            .Select(file => new FileData(
                file,
                File.ReadAllText(file)
            ));
            
        return filesData;
    }
}

public record FileData(string FilePath, string FileContent);

// public class FileData
// {
//     public FileData(string filePath, string fileContent)
//     {
//         FilePath = filePath;
//         FileContent = fileContent;
//     }

//     public string FilePath{get; set;}
//     public string FileContent{get; set;}
// }