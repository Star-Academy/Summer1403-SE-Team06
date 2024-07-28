using System.Reflection;
using System.Resources;
using Microsoft.Extensions.Logging;
using Mohaymen.FullTextSearch.DocumentManagement.Models;
using Mohaymen.FullTextSearch.DocumentManagement.Services.FilesService;
using Mohaymen.FullTextSearch.App.Utilities;

namespace Mohaymen.FullTextSearch.App.Services;

public class FileLoader
{
    private static readonly ResourceManager ResourceManager =
        new("full_text_search.assets.Resources", Assembly.GetExecutingAssembly());

    private static readonly string FolderPath = ResourceManager.GetString("DocumentsPath") ?? "";

    public static FileCollection LoadFiles()
    {
        var fileReader = new FileReader();

        FileCollection fileCollection;
        try
        {
            fileCollection = fileReader.ReadAllFiles(FolderPath);
            return fileCollection;
        }
        catch (DirectoryNotFoundException exception)
        {
            Logging.Logger.LogError(exception, "Wrong Folder Path: {path}", FolderPath);
            throw;
        }
        catch (Exception exception)
        {
            Logging.Logger.LogError(exception, "An error occurred while reading files.");
            throw;
        }
    }
}