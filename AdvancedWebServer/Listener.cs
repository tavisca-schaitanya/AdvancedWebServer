using System;
using System.Net;
using System.Threading;

namespace AdvancedWebServer
{
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
}
