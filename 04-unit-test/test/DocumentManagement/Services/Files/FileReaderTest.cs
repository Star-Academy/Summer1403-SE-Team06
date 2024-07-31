using Mohaymen.FullTextSearch.DocumentManagement.Services.FilesService;

namespace Mohaymen.FullTextSearch.Test.DocumentManagement.Services.Files;

public class FileReaderTest : IDisposable
{
    private readonly FileReader _fileReader;
    private readonly string _testDirectory;

    public FileReaderTest()
    {
        _fileReader = new FileReader();
        _testDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testDirectory);
    }

    [Fact]
    public void ReadAllFiles_ShouldReadAllFilesInADirectory()
    {
        //Arrange
        var filePath1 = Path.Combine(_testDirectory, "file1.txt");
        var filePath2 = Path.Combine(_testDirectory, "file2.txt");
        File.WriteAllText(filePath1, "Content of file1");
        File.WriteAllText(filePath2, "Content of file2");
        
        // Act
        var result = _fileReader.ReadAllFiles(_testDirectory);

        // Assert
        Assert.Equal(2, result.FilesCount());
        Assert.Contains(Path.Combine(_testDirectory, "file1.txt"), result.GetFilesPath());
        Assert.Contains(Path.Combine(_testDirectory, "file2.txt"), result.GetFilesPath());
        Assert.Equal("Content of file1", result.GetFileContent(Path.Combine(_testDirectory, "file1.txt")));
        Assert.Equal("Content of file2", result.GetFileContent(Path.Combine(_testDirectory, "file2.txt")));
    }

    [Fact]
    public void ReadAllFiles_ShouldHandleEmptyDirectory()
    {
        // Act
        var result = _fileReader.ReadAllFiles(_testDirectory);

        // Assert
        Assert.Equal(0, result.FilesCount());
    }

    public void Dispose()
    {
        if (Directory.Exists(_testDirectory))
            Directory.Delete(_testDirectory, true);
    }
}