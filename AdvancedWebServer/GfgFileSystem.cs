using System.IO;

namespace AdvancedWebServer
{
    public class GfgFileSystem : IFileSystem
    {
        public string FileLocation => @"C:\Users\achaitanya\source\repos\AdvancedWebServer\GFG";

        public string GetFileContents(string filePath)
        {
            if (File.Exists(FileLocation + filePath))
                return File.ReadAllText(FileLocation + filePath);
            return null;
        }
    }
}
