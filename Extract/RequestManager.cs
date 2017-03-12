using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FilterPolish.Extract
{
    public class RequestManager
    {
        public List<string> urlSuffix = new List<string>();
        public RequestManager(List<string> urlSuffix)
        {
            this.urlSuffix = urlSuffix;
        }

        public void ExecuteAll()
        {
            var urlbase = Util.getConfigValue("Ninja Request URL");

            List<NinjaRequest> Requests = new List<NinjaRequest>();

            foreach (string s in urlSuffix)
            {
                NinjaRequest n = new NinjaRequest(s, "http://poeninja.azureedge.net/api/Data/" + s + "?league=" + Util.getConfigValue("Ninja League"));
                JsonConvert.PopulateObject(n.responseContent, n.DSO);
                Requests.Add(n);

            }
        }
    }

}
