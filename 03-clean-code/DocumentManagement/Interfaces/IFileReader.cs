using Mohaymen.FullTextSearch.DocumentManagement.Models;

namespace Mohaymen.FullTextSearch.DocumentManagement.Interfaces;

public interface IFileReader
{
    FileCollection ReadAllFiles(string folderPath);
}