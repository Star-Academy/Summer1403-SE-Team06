using Microsoft.Extensions.Logging;
using Mohaymen.FullTextSearch.DocumentManagement.Models;
using Mohaymen.FullTextSearch.DocumentManagement.Services.FilesService;
using Mohaymen.FullTextSearch.App.Utilities;

namespace Mohaymen.FullTextSearch.App.Services;

public static class FileLoader
{
    public static FileCollection LoadFiles(string documentsPath)
    {
        var fileReader = new FileReader();
        
        try
        {
            var fileCollection = fileReader.ReadAllFiles(documentsPath);
            return fileCollection;
        }
        catch (DirectoryNotFoundException exception)
        {
            Logging.Logger.LogError(exception, "Wrong Folder Path: {path}", documentsPath);
            throw;
        }
        catch (Exception exception)
        {
            Logging.Logger.LogError(exception, "An error occurred while reading files.");
            throw;
        }
    }
}