using Microsoft.Extensions.Logging;
using Mohaymen.FullTextSearch.App.Interfaces;
using Mohaymen.FullTextSearch.App.Services;
using Mohaymen.FullTextSearch.App.Utilities;
using Mohaymen.FullTextSearch.Assets;
using Mohaymen.FullTextSearch.DocumentManagement.Interfaces;
using Mohaymen.FullTextSearch.DocumentManagement.Models;
using Mohaymen.FullTextSearch.DocumentManagement.Services.InvertedIndexService;
using Mohaymen.FullTextSearch.DocumentManagement.Services.InvertedIndexService.SearchStrategies;

namespace Mohaymen.FullTextSearch.App;

public class ApplicationService : IApplicationService
{
    private readonly IInvertedIndexBuilder _invertedIndexBuilder;
    private readonly ISearcher<string> _invertedIndexSearcher;
    public ApplicationService(IFileReader fileReader, IInvertedIndexBuilder invertedIndexBuilder)
    {
        var documentsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Resources.DocumentsPath);
        var fileLoader = new FileLoader(fileReader);
        var fileCollection = fileLoader.LoadFiles(documentsPath);
        _invertedIndexBuilder = invertedIndexBuilder;
        var invertedIndex = IndexFiles(fileCollection);
        _invertedIndexSearcher = new InvertedIndexSearcher(invertedIndex);
    }
    
    private IInvertedIndex IndexFiles(FileCollection fileCollection)
    {
        Logging<Program>.Logger.LogInformation("Processing files...");
        var invertedIndex = _invertedIndexBuilder.IndexFilesWords(fileCollection).Build();
        Logging<Program>.Logger.LogInformation("{fileCount} files loaded.", fileCollection.FilesCount());
        return invertedIndex;
    }

    public IEnumerable<string> Search(List<string> mandatoryWords, List<string> optionalWords, List<string> excludedWords)
    {
        var query = new List<SearchQuery>
        {
            new SearchQuery(
                new MandatorySearchStrategy(),
                mandatoryWords.Select(word => new Keyword(word)).ToList()
                ),
            new SearchQuery(
                new OptionalSearchStrategy(),
                optionalWords.Select(word => new Keyword(word)).ToList()
                ),
            new SearchQuery(
                new ExcludedSearchStrategy(),
                excludedWords.Select(word => new Keyword(word)).ToList()
                )
        };
        return _invertedIndexSearcher
            .Search(query)
            .Select(fullPath => Path.GetFileName(fullPath));
    }
}