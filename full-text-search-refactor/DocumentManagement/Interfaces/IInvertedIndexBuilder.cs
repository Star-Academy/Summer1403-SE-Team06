using Mohaymen.FullTextSearch.DocumentManagement.Models;
namespace Mohaymen.FullTextSearch.DocumentManagement.Interfaces;

public interface IInvertedIndexBuilder
{
    public InvertedIndex Build();
}