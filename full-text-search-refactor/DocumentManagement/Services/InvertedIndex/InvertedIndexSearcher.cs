using Mohaymen.FullTextSearch.DocumentManagement.Interfaces;
using Mohaymen.FullTextSearch.DocumentManagement.Models;

namespace Mohaymen.FullTextSearch.DocumentManagement.Services.InvertedIndexService;

public class InvertedIndexSearcher(IInvertedIndex invertedIndex) : ISearcher<string>
{
    public ICollection<string> Search(SearchQuery query)
    {
        var (mandatories, optionals, excludeds) = query;

        var result = new HashSet<string>(invertedIndex.AllDocuments);
        
        ProcessMandatories(result, mandatories);
        ProcessOptionals(result, optionals);
        ProcessExcludeds(result, excludeds);

        return result;
    }

    private void ProcessMandatories(HashSet<string> result, List<Keyword> mandatories)
    {
        foreach (var mandatory in mandatories)
        {
            HashSet<string> currentFiles = invertedIndex.GetDocumentsByKeyword(mandatory);
            result.IntersectWith(currentFiles);
        }
    }

    private void ProcessOptionals(HashSet<string> result, List<Keyword> optionals)
    {
        var optionalsSet = new HashSet<string>();
        foreach (var optional in optionals)
        {
            HashSet<string> currentFiles = invertedIndex.GetDocumentsByKeyword(optional);
            optionalsSet.UnionWith(currentFiles);
        }

        if (optionals.Count > 0)
        {
            result.IntersectWith(optionalsSet);
        }
    }

    private void ProcessExcludeds(HashSet<string> result, List<Keyword> excludeds)
    {
        foreach (var excluded in excludeds)
        {
            HashSet<string> currentFiles = invertedIndex.GetDocumentsByKeyword(excluded);
            result.ExceptWith(currentFiles);
        }
    }
}