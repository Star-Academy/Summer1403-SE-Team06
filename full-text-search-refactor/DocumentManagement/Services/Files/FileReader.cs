using Mohaymen.FullTextSearch.DocumentManagement.Models;
namespace Mohaymen.FullTextSearch.DocumentManagement.Services.FilesService;

public class FileReader
{
    public FileCollection ReadAllFiles(string folderPath)
    {
        var files = Directory.GetFiles(folderPath);
        var fileCollection = files.Aggregate(new FileCollection(), (collection, filePath) =>
            {
                if (!collection.ContainsFile(filePath))
                    collection.AddFile(filePath,
                    File.ReadAllText(filePath));

                return collection;
            });

        return fileCollection;
    }
}