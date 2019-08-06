using System.Collections.Generic;
using System.Net;

namespace AdvancedWebServer
{
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
}
