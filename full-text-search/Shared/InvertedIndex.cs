using System.Text.RegularExpressions;

namespace Mohaymen.FullTextSearch.Shared;
public class InvertedIndex
{
    private Dictionary<string, HashSet<string>> InvertedIndexMap;
    private HashSet<string> AllFiles;
    public InvertedIndex()
    {
        InvertedIndexMap = new Dictionary<string, HashSet<string>>();
        AllFiles = new HashSet<string>();
    }

    public void ProcessFilesWords(Dictionary<string, string> filesContent)
    {
        foreach(var file in filesContent)
        {
            string filePath = file.Key;
            string fileText = file.Value;
            var words = Regex.Split(fileText, @"[^\w']+");
            AllFiles.Add(filePath);

            foreach(var word in words)
            {
                if (string.IsNullOrWhiteSpace(word)) continue;
                string upperWord = word.ToUpper();
                if(!InvertedIndexMap.ContainsKey(upperWord))
                {
                    InvertedIndexMap.Add(upperWord, new HashSet<string>());
                }

                InvertedIndexMap[upperWord].Add(filePath);
            }
        }
    }

    public HashSet<string> SearchWord(string word)
    {
        string upperWord = word.ToUpper();
        InvertedIndexMap.TryGetValue(upperWord, out HashSet<string>? result);
        return result ?? new HashSet<string>();
    }

    public HashSet<string> AdvancedSearch(List<string> mandatories, List<string> optionals, List<string> excludeds)
    {
        var result = new HashSet<string>(AllFiles);
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