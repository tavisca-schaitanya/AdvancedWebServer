namespace AdvancedWebServer
{
    public interface IFileSystem
    {
        string FileLocation { get; }

        string GetFileContents(string filePath);
    }
}
