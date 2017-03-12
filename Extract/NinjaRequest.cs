using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FilterPolish.Extract
{
    public class NinjaRequest
    {
        public string name = "";
        public string url = @"";
        public string responseContent = "";
        public object DSO = new object();

        public NinjaRequest(string name, string url)
        {
            this.name = name;
            this.url = url;
            this.Execute();
        }

        public void Execute()
        {
            this.responseContent = "";
            // Create a request for the URL. 
            WebRequest request = WebRequest.Create(this.url);
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            // Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            this.responseContent = reader.ReadToEnd();

            reader.Close();
            response.Close();
        }
    }
}
