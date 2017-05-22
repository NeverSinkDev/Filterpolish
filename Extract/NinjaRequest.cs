using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FilterPolish.Extract
{
    /// <summary>
    /// Information about a single ninja request
    /// </summary>
    public class NinjaRequest
    {
        public string name = "";
        public string url = @"";
        public string responseContent = "";
        public dynamic structuredResponse;
        public object DSO = new object();

        public NinjaRequest(string name, string url, bool LoadFromFile)
        {
            this.name = name;
            this.url = url;

            if (LoadFromFile)
            {
                this.responseContent = System.IO.File.ReadAllText(Util.GetOutputPath()
                                   + "/EcoData/" + Util.GetTodayDateTimeExtension() + "/" + name + ".json");
            }
            else
            {
                this.Execute();
            }
        }

        public void Execute()
        {
            this.responseContent = "";
            WebRequest request = WebRequest.Create(this.url);
            request.Credentials = CredentialCache.DefaultCredentials;
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();

            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            this.responseContent = reader.ReadToEnd();

            reader.Close();
            response.Close();
        }

        public void SaveToFile(string s)
        {
            System.IO.FileInfo file = new FileInfo(Util.GetOutputPath()
                                   + "/EcoData/" + Util.GetTodayDateTimeExtension() + "/" + s + ".json");
            file.Directory.Create();
            System.IO.File.WriteAllText(file.FullName, this.responseContent);
        }
    }
}
