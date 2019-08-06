using System.Collections.Generic;

namespace AdvancedWebServer
{
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
}
