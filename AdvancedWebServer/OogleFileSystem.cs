using System.IO;

namespace AdvancedWebServer
{
    public class OogleFileSystem : IFileSystem
    {
        public string FileLocation => @"C:\Users\achaitanya\source\repos\AdvancedWebServer\WebApp2";

        public string GetFileContents(string filePath)
        {
            if (File.Exists(FileLocation + filePath))
                return File.ReadAllText(FileLocation + filePath);
            return null;
        }
    }
}
