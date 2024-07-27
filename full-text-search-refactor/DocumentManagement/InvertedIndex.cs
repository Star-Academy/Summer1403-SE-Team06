using System.Text.RegularExpressions;

namespace Mohaymen.FullTextSearch.DocumentManagement;
public class InvertedIndex
{
    private Dictionary<Keyword, HashSet<string>> _invertedIndexMap;
    public InvertedIndex()
    {
        _invertedIndexMap = new Dictionary<Keyword, HashSet<string>>();
    }

    // public void ProcessFilesWords(Dictionary<string, string> filesContent)
    // {
    //     foreach(var (filePath, fileText) in filesContent)
    //     {
    //         var words = Regex.Split(fileText, @"[^\w']+");

    //         _invertedIndexMap = words
    //             .Where(word => !string.IsNullOrWhiteSpace(word))
    //             .Select(word => word.ToUpper())
    //             .Aggregate(_invertedIndexMap, (map, searchWord) =>
    //             {
    //                 if (!map.ContainsKey(searchWord))
    //                 {
    //                     map[searchWord] = new HashSet<string>();
    //                 }
    //                 map[searchWord].Add(filePath);
    //                 return map;
    //             });
    //     }
    // }

    public void ProcessFilesWords(IEnumerable<FileData> filesData)
    {
        foreach(var fileData in filesData)
        {
            var words = Regex.Split(fileData.FileContent, @"[^\w']+");

            _invertedIndexMap = words
                .Where(word => !string.IsNullOrWhiteSpace(word))
                .Select(word => new Keyword(word))
                .Aggregate(_invertedIndexMap, (map, searchWord) =>
                {
                    if (!map.ContainsKey(searchWord))
                    {
                        map[searchWord] = new HashSet<string>();
                    }
                    map[searchWord].Add(fileData.FilePath);
                    return map;
                });
        }
    }

    public HashSet<string> SearchWord(string word)
    {
        var searchWord = new Keyword(word);
        _invertedIndexMap.TryGetValue(searchWord, out HashSet<string>? result);
        return result ?? new HashSet<string>();
    }
}

public class Keyword
{
    public string Value{get;}
    public Keyword(string value)
    {
        Value = value.ToUpper();
    }
}