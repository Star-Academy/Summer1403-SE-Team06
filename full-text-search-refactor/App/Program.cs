using Microsoft.Extensions.Logging;
using Mohaymen.FullTextSearch.App.Utilities;
using Mohaymen.FullTextSearch.DocumentManagement.Models;
using Mohaymen.FullTextSearch.DocumentManagement.Services.InvertedIndexService;
using Mohaymen.FullTextSearch.App.Services;
using Mohaymen.FullTextSearch.App.UI;
using Mohaymen.FullTextSearch.Assets;
using Mohaymen.FullTextSearch.DocumentManagement.Interfaces;

namespace Mohaymen.FullTextSearch.App;

internal static class Program
{
    public static void Main()
    {
        var documentsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Resources.DocumentsPath);
        FileCollection fileCollection;
        try
        {
            fileCollection = FileLoader.LoadFiles(documentsPath);
        }
        catch (Exception)
        {
            return;
        }

        IInvertedIndex invertedIndex = IndexFiles(fileCollection);
        UserInterface.StartProgramLoop(invertedIndex);
    }
    
    private static IInvertedIndex IndexFiles(FileCollection fileCollection)
    {
        Logging.Logger.LogInformation("Processing files...");
        var invertedIndexBuilder = new FilesInvertedIndexBuilder();
        var invertedIndex = invertedIndexBuilder.IndexFilesWords(fileCollection).Build();
        Logging.Logger.LogInformation("{fileCount} files loaded.", fileCollection.FilesCount());
        return invertedIndex;
    }
}