using PSADTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL
{
    public class ErrorsLogsBLL
    {
        public int AddErrorLog(ErrorsLogs ErrorLog)
        {
            try
            {
                HttpClient client = CreateHTTPClient(ConfigurationManager.AppSettings["SecurityServiceBaseURL"]);

                var postTask = client.PostAsJsonAsync("api/ClientsExceptionsLogging/", ErrorLog).Result;
                if (postTask.IsSuccessStatusCode)
                    return 1;
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static HttpClient CreateHTTPClient(string ServiceBaseURL)
        {
            //if (!string.IsNullOrEmpty(ServiceBaseURL))
            //    ServiceBaseURL = "http://mic-app-test:8038/";

            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri(ServiceBaseURL)
            };

            // Add an accept header for json format
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            //string LocalUserName = ConfigurationManager.AppSettings["UserName"];
            //string LocalPassword = ConfigurationManager.AppSettings["Password"];

            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic",
            //                    Convert.ToBase64String(Encoding.UTF8.GetBytes(LocalUserName + ":" + LocalPassword)));

            return client;
        }
    }
}
