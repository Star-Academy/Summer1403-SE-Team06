using System.Text.RegularExpressions;

namespace Mohaymen.FullTextSearch.Shared;
public class InvertedIndex
{
    private Dictionary<string, HashSet<string>> InvertedIndexMap;
    public InvertedIndex()
    {
        InvertedIndexMap = new Dictionary<string, HashSet<string>>();
    }

    public void ProcessFilesWords(Dictionary<string, string> filesContent)
    {
        foreach(var file in filesContent)
        {
            string filePath = file.Key;
            string fileText = file.Value;
            var words = Regex.Split(fileText, @"[^\w']+");

            InvertedIndexMap = words
                .Where(word => !string.IsNullOrWhiteSpace(word))
                .Select(word => word.ToUpper())
                .Aggregate(InvertedIndexMap, (map, upperWord) =>
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
        InvertedIndexMap.TryGetValue(upperWord, out HashSet<string>? result);
        return result ?? new HashSet<string>();
    }
}