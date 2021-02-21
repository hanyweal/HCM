using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TakafulDTO;

namespace HCMBLL
{
    public class TakafulDeductionsBLL
    {
        public List<Deduction> GetActiveTakafulDeductions(int EmployeeID)
        {
            try
            {
                HttpClient client = new HttpClient()
                {
                    BaseAddress = new Uri(ConfigurationManager.AppSettings["TakafulServiceBaseURL"])
                };
                // Add an accept header for json format
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync(string.Format("api/Deductions/GetEmployeeDeductions/{0}", EmployeeID)).Result;
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<List<Deduction>>(response.Content.ReadAsStringAsync().Result);
                }
                else return null;
            }
            catch
            {
                throw;
            }
        }
    }
}
