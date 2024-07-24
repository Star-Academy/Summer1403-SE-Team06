using System.Text.RegularExpressions;

namespace Mohaymen.FullTextSearch.DocumentManagement;
public class InvertedIndex
{
    private Dictionary<string, HashSet<string>> _invertedIndexMap;
    private HashSet<string> _allFiles;
    public InvertedIndex()
    {
        _invertedIndexMap = new Dictionary<string, HashSet<string>>();
        _allFiles = new HashSet<string>();
    }

    public void ProcessFilesWords(Dictionary<string, string> filesContent)
    {
        foreach(var (filePath, fileText) in filesContent)
        {
            var words = Regex.Split(fileText, @"[^\w']+");
            _allFiles.Add(filePath);

            _invertedIndexMap = words
                .Where(word => !string.IsNullOrWhiteSpace(word))
                .Select(word => word.ToUpper())
                .Aggregate(_invertedIndexMap, (map, upperWord) =>
                {
                    if (!map.ContainsKey(upperWord))
                    {
                        map[upperWord] = new HashSet<string>();
                    }
                    map[upperWord].Add(filePath);
                    return map;
                });
        }
    }

    public HashSet<string> SearchWord(string word)
    {
        string upperWord = word.ToUpper();
        _invertedIndexMap.TryGetValue(upperWord, out HashSet<string>? result);
        return result ?? new HashSet<string>();
    }

    public HashSet<string> AdvancedSearch(List<string> mandatories, List<string> optionals, List<string> excludeds)
    {
        var result = new HashSet<string>(_allFiles);
        foreach(var mandatory in mandatories)
        {
            HashSet<string> currentFiles = SearchWord(mandatory);
            result.IntersectWith(currentFiles);
        }

        var optionalsSet = new HashSet<string>();
        foreach (var optional in optionals)
        { 
            HashSet<string> currentFiles = SearchWord(optional);
            optionalsSet.UnionWith(currentFiles); 
        }

        if (optionals.Count > 0)
        {
            result.IntersectWith(optionalsSet);
        }
        
        foreach (var excluded in excludeds)
        {
            HashSet<string> currentFiles = SearchWord(excluded);
            result.ExceptWith(currentFiles);
        }

        return result;
    }
}