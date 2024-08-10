using Microsoft.Extensions.Logging;
using Mohaymen.FullTextSearch.DocumentManagement.Models;
using Mohaymen.FullTextSearch.App.Utilities;
using Mohaymen.FullTextSearch.DocumentManagement.Interfaces;

namespace Mohaymen.FullTextSearch.App.Services;

public class FileLoader
{
    private IFileReader _fileReader;

    public FileLoader(IFileReader fileReader)
    {
        _fileReader = fileReader;
    }
    
    public FileCollection LoadFiles(string documentsPath)
    {
        try
        {
            var fileCollection = _fileReader.ReadAllFiles(documentsPath);
            return fileCollection;
        }
        catch (DirectoryNotFoundException exception)
        {
            Logging<FileLoader>.Logger.LogError(exception, "Wrong Folder Path: {path}", documentsPath);
            throw;
        }
    }
}