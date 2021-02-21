using HCMBLL.Enums;
using Newtonsoft.Json;
using PSADTO;
using PSADTO.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace HCMBLL
{
    public class UsersBLL : CommonEntity, IEntity
    {
        private readonly int ProjectID;

        private readonly string URL;

        private HttpClient client;

        public UsersBLL()
        {
            ProjectID = int.Parse(ConfigurationManager.AppSettings["ProjectID"]);
            URL = ConfigurationManager.AppSettings["SecurityServiceBaseURL"];
            client = new HttpClient()
            {
                BaseAddress = new Uri(URL)
            };
            // Add an accept header for json format
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public AuthenticationResult AuthenticateUserByName(string UserName)
        {
            HttpResponseMessage response = client.GetAsync(string.Format("api/Authentications/UserAuthenticatedByOnlyProject/{0}/{1}", UserName, ProjectID)).Result;
            AuthenticationResult result = null;
            if (response.IsSuccessStatusCode)
                result = JsonConvert.DeserializeObject<AuthenticationResult>(response.Content.ReadAsStringAsync().Result);

            return result;
        }

        public AuthenticationResult AuthenticateUser(string UserName, string Password)
        {
            HttpResponseMessage response = client.GetAsync(string.Format("api/Authentications/UserProjectAuthenticated/{0}/{1}/{2}", UserName, Password, ProjectID)).Result;
            AuthenticationResult result = null;
            if (response.IsSuccessStatusCode)
                result = JsonConvert.DeserializeObject<AuthenticationResult>(response.Content.ReadAsStringAsync().Result);
           
            return result;
        }

        public AuthenticationResult GetUserInfo(string UserName, int ProjectID)
        {
            HttpResponseMessage response = client.GetAsync(string.Format("api/Authentications/GetUserInfoByProject/{0}/{1}", UserName, ProjectID)).Result;
            AuthenticationResult result = null;
            if (response.IsSuccessStatusCode)
                result = JsonConvert.DeserializeObject<AuthenticationResult>(response.Content.ReadAsStringAsync().Result);

            return result;
        }

        public ChangePasswordResultEnum ChangePassword(int UserID, string OldPassword, string NewPassword, string ConfirmNewPassword)
        {
            HttpResponseMessage response = client.PostAsJsonAsync(string.Format("api/Authentications/ChangePassword/{0}/{1}/{2}/{3}", UserID, OldPassword, NewPassword, ConfirmNewPassword), "").Result;
            if (response.IsSuccessStatusCode)
                return ChangePasswordResultEnum.Done;
            else
            {
                var result = JsonConvert.DeserializeObject<ChangePasswordResultEnum>(response.Content.ReadAsStringAsync().Result);
                return result;
            }
        }

        public ChangePasswordResultEnum ResetPassword(int UserID, string NewPassword, string ConfirmNewPassword)
        {
            HttpResponseMessage response = client.PostAsJsonAsync(string.Format("api/Authentications/ResetPassword/{0}/{1}/{2}", UserID, NewPassword, ConfirmNewPassword), "").Result;
            if (response.IsSuccessStatusCode)
                return ChangePasswordResultEnum.Done;
            else
            {
                var result = JsonConvert.DeserializeObject<ChangePasswordResultEnum>(response.Content.ReadAsStringAsync().Result);
                return result;
            }
        }

        public bool SendMessage(BusinessSubCategoriesEnum BusinessSubCategory, string Message, string EmployeeMobileNo)
        {
            SMSLogsBLL SmsLogBLL = new SMSLogsBLL()
            {
                BusinssSubCategory = BusinessSubCategory,
                MobileNo = EmployeeMobileNo,
                DetailID = 0,
                Message = Message,
                CreatedBy = this.LoginIdentity,
                CreatedDate = DateTime.Now,
            };

            SMSBLL SmsLog = new SMSBLL();
            bool IsSent = SmsLog.SendSMS(SmsLogBLL);
            return IsSent;
        }
    }
}
