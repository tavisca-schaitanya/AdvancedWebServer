using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

namespace AdvancedWebServer
{
    class Program
    {
        static void Main(string[] args)
        {
            DomainLookUp domainLookUp = new DomainLookUp();
            ContextBuffer contextBuffer = new ContextBuffer();
            Listener listener = new Listener(contextBuffer, domainLookUp);
            Dispatcher dispatcher = new Dispatcher(contextBuffer, domainLookUp);
            WebApp gfg = new WebApp(new GfgFileSystem());
            listener.AddDomain("http://localhost:8888/", gfg);
            WebApp oogle = new WebApp(new OogleFileSystem());
            listener.AddDomain("http://localhost:3333/", oogle);
            listener.Listen();
            dispatcher.DispatchRequests();
        }
    }

    public class ContextBuffer
    {
        private List<HttpListenerContext> _contextList = new List<HttpListenerContext>();


        public bool IsEmpty()
        {
            return _contextList.Count == 0;
        }

        public void Add(HttpListenerContext context)
        {
            _contextList.Add(context);
        }

        public HttpListenerContext Deque()
        {
            if (_contextList.Count == 0)
                return null;

            var context = _contextList[0];
            _contextList.RemoveAt(0);

            return context;
        }
    }

    public class DomainLookUp
    {
        private Dictionary<string, WebApp> domainDict = new Dictionary<string, WebApp>();

        public void Add(string domainName, WebApp webApp)
        {
            domainDict.Add(domainName, webApp);
        }

        public WebApp GetWebApp(string domainName)
        {
            return domainDict[domainName];
        }
    }

    public class Listener
    {
        HttpListener httpListener = new HttpListener();

        ContextBuffer contextBuffer;

        DomainLookUp domainLookUp;

        public Listener(ContextBuffer contextBuffer, DomainLookUp domainLookUp)
        {
            this.contextBuffer = contextBuffer;
            this.domainLookUp = domainLookUp;
        }

        public void AddDomain(string prefix, WebApp webApp)
        {
            httpListener.Prefixes.Add(prefix);
            Uri uri = new Uri(prefix);
            domainLookUp.Add(uri.Authority, webApp);
        }

        private void TakeRequests()
        {
            Console.WriteLine("Listening...");
            while (true)
            {
                var context = httpListener.GetContext();
                Console.WriteLine("Received " + context.Request.RawUrl);
                contextBuffer.Add(context);
            }
        }
        public void Listen()
        {
            httpListener.Start();
            Thread listenThread = new Thread(() => TakeRequests());
            listenThread.Start();
        }
    }

    
    public class Dispatcher
    {
        ContextBuffer contextBuffer;

        DomainLookUp domainLookUp;

        public Dispatcher(ContextBuffer contextBuffer, DomainLookUp domainLookUp)
        {
            this.contextBuffer = contextBuffer;
            this.domainLookUp = domainLookUp;
        }


        private void RespondToRequests()
        {
            while(true)
            {
                if(!contextBuffer.IsEmpty())
                {
                    var context = contextBuffer.Deque();
                    if(context != null)
                    {
                        var domainName = context.Request.Url.Authority;
                        var filePath = context.Request.Url.AbsolutePath;
                        var webApp = domainLookUp.GetWebApp(domainName);
                        webApp.RenderWebPage(context, filePath);
                    }
                }
            }
        }

        public void DispatchRequests()
        {
            Thread dispatchThread = new Thread(() => RespondToRequests());
            dispatchThread.Start();
        }

    }

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

    public interface IFileSystem
    {
        string FileLocation { get; }

        string GetFileContents(string filePath);
    }


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
