using DocumentManagement;

namespace Mohaymen.FullTextSearch.DocumentManagement;

public class AdvancedInvertedIndex
{
    private InvertedIndex _invertedIndex;
    private HashSet<string> _allFiles;

    public AdvancedInvertedIndex()
    {
        _invertedIndex = new InvertedIndex();
        _allFiles = new HashSet<string>();
    }

    // public void ProcessFilesWords(Dictionary<string, string> filesContent)
    // {
    //     _allFiles.UnionWith(filesContent.Keys);
    //     _invertedIndex.ProcessFilesWords(filesContent);
    // }

    public void ProcessFilesWords(IEnumerable<FileData> filesData)
    {
        _allFiles.UnionWith(filesData.Select(file => file.FilePath));
        _invertedIndex.ProcessFilesWords(filesData);
    }

    public HashSet<string> AdvancedSearch(SearchQuery searchQuery)
    {
        var (mandatories, optionals, excludeds) = searchQuery;
        
        var result = new HashSet<string>(_allFiles);
        foreach(var mandatory in mandatories)
        {
            HashSet<string> currentFiles = _invertedIndex.SearchWord(mandatory);
            result.IntersectWith(currentFiles);
        }

        var optionalsSet = new HashSet<string>();
        foreach (var optional in optionals)
        { 
            HashSet<string> currentFiles = _invertedIndex.SearchWord(optional);
            optionalsSet.UnionWith(currentFiles); 
        }

        if (optionals.Count > 0)
        {
            result.IntersectWith(optionalsSet);
        }
        
        foreach (var excluded in excludeds)
        {
            HashSet<string> currentFiles = _invertedIndex.SearchWord(excluded);
            result.ExceptWith(currentFiles);
        }

        return result;
    }
}