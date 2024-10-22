﻿using Microsoft.Extensions.Logging;
using Mohaymen.FullTextSearch.App.Utilities;
using Mohaymen.FullTextSearch.DocumentManagement.Models;
using Mohaymen.FullTextSearch.DocumentManagement.Services.InvertedIndexService;
using Mohaymen.FullTextSearch.App.Services;
using Mohaymen.FullTextSearch.App.UI;
using Mohaymen.FullTextSearch.Assets;
using Mohaymen.FullTextSearch.DocumentManagement.Interfaces;
using Mohaymen.FullTextSearch.DocumentManagement.Services.FilesService;
using Mohaymen.FullTextSearch.DocumentManagement.Utilities;

namespace Mohaymen.FullTextSearch.App;

internal static class Program
{
    public static void Main()
    {
        var documentsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Resources.DocumentsPath);
        var fileLoader = new FileLoader(new FileReader());
        var fileCollection = fileLoader.LoadFiles(documentsPath);
        var invertedIndex = IndexFiles(fileCollection);
        var invertedIndexSearcher = new InvertedIndexSearcher(invertedIndex);
        var parser = new InputParser();
        var userInterface = new UserInterface(invertedIndexSearcher, parser);
        userInterface.StartProgramLoop();
    }
    
    private static IInvertedIndex IndexFiles(FileCollection fileCollection)
    {
        Logging.Logger.LogInformation("Processing files...");
        var tokenizer = new Tokenizer();
        var invertedIndexBuilder = new FilesInvertedIndexBuilder(tokenizer);
        var invertedIndex = invertedIndexBuilder.IndexFilesWords(fileCollection).Build();
        Logging.Logger.LogInformation("{fileCount} files loaded.", fileCollection.FilesCount());
        return invertedIndex;
    }
}   