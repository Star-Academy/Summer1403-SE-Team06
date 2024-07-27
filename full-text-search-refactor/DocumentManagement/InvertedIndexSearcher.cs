using Mohaymen.FullTextSearch.DocumentManagement;

namespace DocumentManagement;

public class InvertedIndexSearcher(InvertedIndex invertedIndex) : ISearcher<string>
{
    public ICollection<string> Search(SearchQuery query)
    {
        var (mandatories, optionals, excludeds) = query;

        var result = new HashSet<string>(invertedIndex.AllDocuments);
        foreach (var mandatory in mandatories)
        {
            HashSet<string> currentFiles = invertedIndex.GetDocumentsByKeyword(mandatory);
            result.IntersectWith(currentFiles);
        }

        var optionalsSet = new HashSet<string>();
        foreach (var optional in optionals)
        {
            HashSet<string> currentFiles = invertedIndex.GetDocumentsByKeyword(optional);
            optionalsSet.UnionWith(currentFiles);
        }

        if (optionals.Count > 0) result.IntersectWith(optionalsSet);

        foreach (var excluded in excludeds)
        {
            HashSet<string> currentFiles = invertedIndex.GetDocumentsByKeyword(excluded);
            result.ExceptWith(currentFiles);
        }

        return result;
    }
}