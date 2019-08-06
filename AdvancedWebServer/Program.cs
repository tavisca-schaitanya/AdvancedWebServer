using System.Collections;

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
}
