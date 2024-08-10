using Mohaymen.FullTextSearch.DocumentManagement.Models;

namespace Mohaymen.FullTextSearch.DocumentManagement.Interfaces;

public interface IInvertedIndexBuilder
{ 
    IInvertedIndex Build();
    IInvertedIndexBuilder IndexFilesWords(FileCollection fileCollection);
}