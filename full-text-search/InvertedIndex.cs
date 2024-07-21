using System.Text.RegularExpressions;

namespace Mohaymen.FullTextSearch;
class InvertedIndex
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
                string upperWord = word.ToUpper();
                if(!InvertedIndexMap.ContainsKey(upperWord))
                {
                    InvertedIndexMap.Add(upperWord, new HashSet<string>());
                }

                InvertedIndexMap[upperWord].Add(filePath);
            }
        }

        // Removes the empty string caused by regex split
        if(InvertedIndexMap.ContainsKey(""))
        {
            InvertedIndexMap.Remove("");
        }
    }

    public HashSet<string> SearchWord(string word)
    {
        string upperWord = word.ToUpper();
        try
        {
            return InvertedIndexMap[upperWord];
        }
        catch
        {
            return new HashSet<string>();
        }
    }
}