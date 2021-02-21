using Globals;
using HCMBLL.SMSService;
using HCMDAL;
using IMMDTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL
{
    public class SMSBLL
    {
        static string UserName = ConfigurationManager.AppSettings["SMSServiceUser"];
        static string Password = ConfigurationManager.AppSettings["SMSServicePassword"];
        private readonly string URL;
        private HttpClient client;

        public SMSBLL()
        {
            URL = ConfigurationManager.AppSettings["SMSServiceBaseURL"];
            client = new HttpClient()
            {
                BaseAddress = new Uri(URL)
            };
            // Add an accept header for json format
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public bool SendSMS(SMSLogsBLL SMSLog, bool IsLog = true)
        {
            try
            {
                //string resourceAddress = "SMS/SendSingleSMS";
                string MobileNo = FormatMobileNum(SMSLog.MobileNo);

                if (!string.IsNullOrEmpty(MobileNo))
                {
                    if (Utilities.IsProEnv() || Utilities.IsTestEnv())
                    {
                        if (Utilities.IsTestEnv())
                            SMSLog.Message = SMSLog.Message + " - " + Globalization.TestTrialText;

                        SMS PostSendingDTO = new SMS()
                        {
                            Message = SMSLog.Message.Replace("<br/>", Environment.NewLine),
                            MobileNo = MobileNo,
                            Password = Password,
                            UserName = UserName
                        };

                        var PostTask = client.PostAsJsonAsync("SMS/SendSingleSMS", PostSendingDTO).Result;

                        if (PostTask.IsSuccessStatusCode)
                        {
                            if (IsLog)
                                SMSLog.Add();

                            return true;
                        }
                        else
                            return false;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public bool SendSMSWithOutLog(string Message, string MobileNum)
        //{
        //    try
        //    {
        //        Service1SoapClient Bulk_SMSClass = new Service1SoapClient();
        //        string Content = Message;

        //        string MobileNo = FormatMobileNum(MobileNum);

        //        if (!string.IsNullOrEmpty(MobileNo))
        //        {
        //            /*
        //          // for testing during Dev 
        //          if (Utilities.IsDevEnv())
        //              MobileNum = "966581656117";                                                 // Mr. Fahad 966590064719
        //          else if (Utilities.IsTestEnv())
        //              MobileNum = ConfigurationSettings.AppSettings["DefaultMobileNumber"]; 
        //          */

        //            int result = 0;
        //            if (Utilities.IsProEnv())
        //                result = Bulk_SMSClass.SendSingleSMS(UserName, Password, MobileNo, Content.Replace("<br/>", Environment.NewLine));

        //            if (result == 0)
        //                return true;
        //            else
        //                return false;
        //        }
        //        else
        //            return false;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        public string FormatMobileNum(string MobileNo)
        {
            if (ValidateMobileNum(MobileNo))
            {
                if (MobileNo.Substring(0, 1) == "0")
                    MobileNo = "966" + MobileNo.Substring(1, MobileNo.Length - 1);
                else
                    MobileNo = "966" + MobileNo;
            }
            else
                MobileNo = string.Empty;

            return MobileNo;
        }

        public bool ValidateMobileNum(string MobileNo)
        {
            // expected input: 0598830505 or 598830505
            if ((MobileNo.Length == 9 && MobileNo.StartsWith("5")) || (MobileNo.Length == 10 && MobileNo.StartsWith("05")))
                return true;
            else
                return false;
        }
    }
}