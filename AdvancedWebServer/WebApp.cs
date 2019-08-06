using System.IO;
using System.Net;

namespace AdvancedWebServer
{
    public class WebApp
    {
        private IFileSystem _fileSystem;

        public WebApp(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public void RenderWebPage(HttpListenerContext context, string filePath)
        {
            using (Stream stream = context.Response.OutputStream)
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(_fileSystem.GetFileContents(filePath));
                }
            }
        }
    }
}
