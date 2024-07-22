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
}