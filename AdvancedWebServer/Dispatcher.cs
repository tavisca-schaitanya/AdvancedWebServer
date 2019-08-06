using System.Threading;

namespace AdvancedWebServer
{
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
}
