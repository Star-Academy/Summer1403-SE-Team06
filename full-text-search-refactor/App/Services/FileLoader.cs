using Microsoft.Extensions.Logging;
using Mohaymen.FullTextSearch.DocumentManagement.Models;
using Mohaymen.FullTextSearch.DocumentManagement.Services.FilesService;
using Mohaymen.FullTextSearch.App.Utilities;
using Mohaymen.FullTextSearch.DocumentManagement.Interfaces;

namespace Mohaymen.FullTextSearch.App.Services;

public class FileLoader
{
    public FileCollection LoadFiles(string documentsPath)
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
    }
}