namespace Mohaymen.FullTextSearch.DocumentManagement;

public class FileReader
{
    public FileCollection ReadAllFiles(string folderPath)
    {
        var fileCollection = Directory
            .GetFiles(folderPath)
            .Aggregate(new FileCollection(), (collection, filePath) =>
            {
                if (!collection.ContainsFile(filePath))
                    collection.AddFile(filePath,
                    File.ReadAllText(filePath));

                return collection;
            });

        return fileCollection;
    }
}