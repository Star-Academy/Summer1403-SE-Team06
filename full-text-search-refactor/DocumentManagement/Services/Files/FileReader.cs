using Mohaymen.FullTextSearch.DocumentManagement.Interfaces;
using Mohaymen.FullTextSearch.DocumentManagement.Models;
namespace Mohaymen.FullTextSearch.DocumentManagement.Services.FilesService;

public class FileReader : IFileReader
{
    public FileCollection ReadAllFiles(string folderPath)
    {
        var files = Directory.GetFiles(folderPath);
        var fileCollection = files.Aggregate(new FileCollection(), AddFileToCollection);

        return fileCollection;
    }

    private FileCollection AddFileToCollection(FileCollection collection, string filePath)
    {
        if (!collection.ContainsFile(filePath))
            collection.AddFile(
                filePath,
                File.ReadAllText(filePath)
            );

        return collection;
    }
}