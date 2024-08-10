namespace Mohaymen.FullTextSearch.MyWebApplication.Interfaces;

public interface IApplicationService
{
    IEnumerable<string> Search(List<string> mandatoryWords, List<string> optionalWords, List<string> excludedWords);
}