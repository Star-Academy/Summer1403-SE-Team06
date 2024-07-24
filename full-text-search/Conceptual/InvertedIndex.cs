using System.Text.RegularExpressions;

namespace Mohaymen.FullTextSearch.Conceptual;
public class InvertedIndex
{
    private Dictionary<string, HashSet<string>> _invertedIndexMap;
    public InvertedIndex()
    {
        _invertedIndexMap = new Dictionary<string, HashSet<string>>();
    }

    public void ProcessFilesWords(Dictionary<string, string> filesContent)
    {
        foreach(var (filePath, fileText) in filesContent)
        {
            var words = Regex.Split(fileText, @"[^\w']+");

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
}