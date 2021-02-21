using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Text.RegularExpressions;

namespace Globals
{
    public sealed class Utilities
    {
        public static bool IsInArray(Array ar, int x)
        {
            for (int i = 0; i < ar.Length; i++)
                if (int.Parse(ar.GetValue(x).ToString()) == x)
                    return true;

            return false;
        }

        public static List<int> ConvertToList(string strIds, char separator)
        {
            List<int> lst = new List<int>();
            string[] arr = strIds.Split(separator);
            if (arr == null || arr.Length == 0) return null;
            //
            int iitem = 0;
            foreach (string item in arr)
            {
                bool isInt = int.TryParse(item, out iitem);
                if (isInt)
                    lst.Add(iitem);
            }
            //
            return lst.Count == 0 ? null : lst;
        }

        public static int[] ConvertToList(string strIds)
        {
            List<int> lst = ConvertToList(strIds, ',');
            return lst == null ? null : lst.ToArray();
        }

        public static string CombineListOfStrings(List<string> strs)
        {
            if (null == strs || strs.Count == 0)
                return "";
            //
            string result = "";
            for (int i = 0; i < strs.Count; i++)
            {
                if (i == strs.Count - 1)
                {
                    if (string.IsNullOrEmpty(strs[i]))
                        result += strs[i].Trim();
                }
                else
                {
                    //check for next character
                    if (!string.IsNullOrEmpty(strs[i + 1]))
                        result += strs[i].Trim() + " ";
                    else
                        result += strs[i].Trim();
                }
            }
            return result;
        }

        public static string CombineListOfStrings(List<string> strs, char separator)
        {
            if (null == strs || strs.Count == 0)
                return "";
            //
            string result = "";
            foreach (string str in strs)
                result += string.IsNullOrEmpty(str) ? "" : (str + separator);
            // remove the last separators
            result = result.TrimEnd(new char[] { separator });
            return result;
        }

        public static string CombineListOfStrings(string[] strs, char separator)
        {
            if (strs == null || strs.Length == 0)
                return "";
            //
            string result = "";
            foreach (string str in strs)
                result += string.IsNullOrEmpty(str) ? "" : (str + separator);
            // remove the last separators
            result = result.TrimEnd(new char[] { separator });
            return result;
        }

        public static string CombineListOfStrings(string[] strs, string separator)
        {
            if (strs == null || strs.Length == 0)
                return "";
            //
            string result = "";
            foreach (string str in strs)
                result += string.IsNullOrEmpty(str) ? "" : (str + separator);
            // remove the last separators
            if (result.EndsWith(separator))
                result = result.Substring(0, result.Length - 3);

            //result = result.Substring(separator.Length-3,3)( TrimEnd(new string[] { separator });
            return result;
        }

        public static string CombineListOfStrings(List<int> lst, char separator)
        {
            if (lst == null || lst.Count == 0)
                return "";
            //
            string result = "";
            foreach (int num in lst)
                result += num.ToString() + separator;
            // remove the last separators
            result = result.TrimEnd(new char[] { separator });
            return result;

        }

        public static string CombineListOfStrings(int[] lst, char separator)
        {
            if (lst == null || lst.Length == 0)
                return "";
            //
            string result = "";
            foreach (int num in lst)
                result += num.ToString() + separator;
            // remove the last separators
            result = result.TrimEnd(new char[] { separator });
            return result;

        }

        public static string CombineListOfStrings(string lst, char separator)
        {
            if (lst == null || lst.Length == 0)
                return "";

            string[] split = lst.Split(new char[] { separator });
            string result = "";
            foreach (string str in split)
                result += str;

            return result;
        }

        public static string CreatePassword(int length)
        {
            string valid = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string res = "";
            Random rnd = new Random();
            while (0 < length--)
                res += valid[rnd.Next(valid.Length)];
            return res;
        }

        public static string CreateRandomNumbers(int length)
        {
            string valid = "123456789";
            string res = "";
            Random rnd = new Random();
            while (0 < length--)
                res += valid[rnd.Next(valid.Length)];
            return res;
        }

        public static string CreatePassword(string validString, int length)
        {
            string valid = validString;
            string res = "";
            Random rnd = new Random();
            while (0 < length--)
                res += valid[rnd.Next(valid.Length)];
            return res;
        }

        public static bool IsNumber(string str)
        {
            if (str == null || str == "")
                return false;

            foreach (char chr in str.ToCharArray())
            {
                if (Char.IsNumber(chr) == false)
                    return false;
            }
            return true;
        }

        public static DataTable ConvertToDataTable<T>(IList<T> list, DataColumn[] columns)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            foreach (T item in list)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    if (prop.GetValue(item) != null)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }

                table.Rows.Add(row);
            }

            DataTable newtable = new DataTable();
            newtable.Columns.AddRange(columns);

            foreach (DataRow dr in table.Rows)
            {
                DataRow ddr = newtable.NewRow();
                foreach (DataColumn cc in columns)
                    ddr[cc.ColumnName] = dr[cc.ColumnName].ToString();

                newtable.Rows.Add(ddr);
            }

            return newtable;
        }

        public static string RemoveSpace(string Text)
        {
            return HttpContext.Current.Server.UrlEncode(Text).Trim().Replace("+", "%20");
        }

        public static string NewLineInHTML(string Text)
        {
            return Text.Trim().Replace("\r\n", "<br/>");
        }


        public static List<string> GetNumAsList(int firstNum, int SecondNum)
        {
            List<string> lst = new List<string>();
            var f = string.Empty;
            for (int i = firstNum; i <= SecondNum; i++)
            {
                f = i.ToString();
                if (i.ToString().Length == 1)
                    f = "0" + i.ToString();

                lst.Add(f);
            }
            return lst;
        }

        /// <summary>
        /// This function to convert provided days into Years Months and Days
        /// e.g.
        /// </summary>
        /// <param name="TotalDays"></param>
        /// <param name="DaysCountInUmAlquraYear"></param>
        /// <returns></returns>
        public static string GetYearMonthDaysFromTotalDays(int TotalDays, int DaysCountInUmAlquraYear, decimal DaysCountInUmAlquraMonth)
        {
            int years = TotalDays / DaysCountInUmAlquraYear;
            int months = Convert.ToInt32(Math.Floor((TotalDays % DaysCountInUmAlquraYear) / DaysCountInUmAlquraMonth));
            int days = Convert.ToInt32(Math.Floor(((TotalDays % DaysCountInUmAlquraYear) % DaysCountInUmAlquraMonth)));
            return string.Format("{0} ({1}) {2} ({3}) {4} ({5})", years, "سنه", months, "شهر", days, "يوم");
        }

        public static bool IsDevEnv()
        {
            return ConfigurationManager.AppSettings["Environment"].ToLower().Equals("dev");
        }

        public static bool IsTestEnv()
        {
            return ConfigurationManager.AppSettings["Environment"].ToLower().Equals("test");
        }

        public static bool IsProEnv()
        {
            return ConfigurationManager.AppSettings["Environment"].ToLower().Equals("pro");
        }

        public static string Encrypt(string strEncrypted)
        {
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);
            string encryptedPassword = Convert.ToBase64String(b);
            return encryptedPassword;
        }

        public static string DecryptString(string encrString)
        {
            byte[] b = Convert.FromBase64String(encrString);
            string decryptedPassword = System.Text.ASCIIEncoding.ASCII.GetString(b);
            return decryptedPassword;
        }
    }

    public sealed class Calendar
    {
        private HttpContext cur;
        private const int startGreg = 1900;
        private const int endGreg = 2100;
        private const int StartHijYear = 1370;
        private const int StartGregYear = 1950;
        private static CultureInfo arCul;
        private static CultureInfo enCul;
        private static UmAlQuraCalendar UmAlQura;
        private static GregorianCalendar Greg;
        private static string[] allFormats = { "yyyy/MM/dd", "MM/dd/yy", "MM/dd/yyyy", "yyyy/M/d", "dd/MM/yy", "d/M/yy", "dd/M/yy", "d/MM/yy", "dd/MM/yyyy", "d/M/yyyy", "dd/M/yyyy", "d/MM/yyyy", "yyyy-MM-dd", "yyyy-M-d", "dd-MM-yy", "d-M-yy", "dd-M-yy", "d-MM-yy", "dd-MM-yyyy", "d-M-yyyy", "dd-M-yyyy", "d-MM-yyyy", "yyyy MM dd", "yyyy M d", "dd MM yyyy", "d M yyyy", "dd M yyyy", "d MM yyyy" };

        public Calendar()
        {
            //arCul = new CultureInfo("ar-SA");
            //enCul = new CultureInfo("en-US");

            //UmAlQura = new UmAlQuraCalendar();
            //Greg = new GregorianCalendar(GregorianCalendarTypes.USEnglish);

            //arCul.DateTimeFormat.Calendar = UmAlQura;
            //enCul.DateTimeFormat.Calendar = Greg;
            cur = HttpContext.Current;

            arCul = new CultureInfo("ar-SA");
            enCul = new CultureInfo("en-US");

            UmAlQura = new UmAlQuraCalendar();
            Greg = new GregorianCalendar(GregorianCalendarTypes.USEnglish);

            arCul.DateTimeFormat.Calendar = UmAlQura;
        }

        public bool IsHijri(string hijri)
        {
            if (hijri.Length <= 0)
            {
                cur.Trace.Warn("IsHijri Error: Date String is Empty");
                return false;
            }
            try
            {
                DateTime tempDate = DateTime.ParseExact(hijri, allFormats, arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
                if (tempDate.Year >= startGreg && tempDate.Year <= endGreg)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                cur.Trace.Warn("IsHijri Error :" + hijri.ToString() + "\n" + ex.Message);
                return false;
            }

        }

        public bool IsHijri(DateTime hijri)
        {
            try
            {
                DateTime tempDate = hijri;
                if (tempDate.Year >= startGreg && tempDate.Year <= endGreg)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                cur.Trace.Warn("IsHijri Error :" + hijri.ToString() + "\n" + ex.Message);
                return false;
            }

        }

        /// <summary>
        /// Check if string is Gregorian date and then return true 
        /// </summary>
        /// <param name="greg"></param>
        /// <returns></returns>
        public bool IsGreg(string greg)
        {
            if (greg.Length <= 0)
            {
                cur.Trace.Warn("IsGreg :Date String is Empty");
                return false;
            }
            try
            {
                DateTime tempDate = DateTime.ParseExact(greg, allFormats, enCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
                if (tempDate.Year >= startGreg && tempDate.Year <= endGreg)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                cur.Trace.Warn("IsGreg Error :" + greg.ToString() + "\n" + ex.Message);
                return false;
            }

        }

        public static int GetUmAlQuraYear(DateTime TheDate)
        {
            UmAlQuraCalendar cal = new UmAlQuraCalendar();
            int year = cal.GetYear(TheDate);
            return year;
        }

        public static int GetUmAlQuraMonth(DateTime TheDate)
        {
            UmAlQuraCalendar cal = new UmAlQuraCalendar();
            int month = cal.GetMonth(TheDate);
            return month;
        }

        public static int GetUmAlQuraDay(DateTime TheDate)
        {
            UmAlQuraCalendar cal = new UmAlQuraCalendar();
            int day = cal.GetDayOfMonth(TheDate);
            return day;
        }

        public static string GetUmAlQuraDate(DateTime TheDate)
        {
            UmAlQura = new UmAlQuraCalendar();
            string day = Convert.ToString(UmAlQura.GetDayOfMonth(TheDate)).Length == 1 ? "0" + Convert.ToString(UmAlQura.GetDayOfMonth(TheDate)) : Convert.ToString(UmAlQura.GetDayOfMonth(TheDate));
            string month = Convert.ToString(UmAlQura.GetMonth(TheDate)).Length == 1 ? "0" + Convert.ToString(UmAlQura.GetMonth(TheDate)) : Convert.ToString(UmAlQura.GetMonth(TheDate));
            string year = UmAlQura.GetYear(TheDate).ToString();
            string UmAlQuraDate = year.ToString() + "-" + month.ToString() + "-" + day.ToString();
            return UmAlQuraDate;
        }

        public static int GetGregYear(DateTime TheDate)
        {
            GregorianCalendar cal = new GregorianCalendar();
            int year = cal.GetYear(TheDate);
            return year;
        }

        public static int GetGregMonth(DateTime TheDate)
        {
            GregorianCalendar cal = new GregorianCalendar();
            int month = cal.GetMonth(TheDate);
            return month;
        }

        public static int GetGregDay(DateTime TheDate)
        {
            GregorianCalendar cal = new GregorianCalendar();
            int day = cal.GetDayOfMonth(TheDate);
            return day;
        }

        public static string GregToUmAlQura(DateTime GregDate)
        {
            arCul = new CultureInfo("ar-SA");
            enCul = new CultureInfo("en-US");

            UmAlQura = new UmAlQuraCalendar();
            Greg = new GregorianCalendar(GregorianCalendarTypes.USEnglish);

            arCul.DateTimeFormat.Calendar = UmAlQura;
            enCul.DateTimeFormat.Calendar = Greg;

            DateTime date = DateTime.ParseExact(GregDate.ToShortDateString(), allFormats, enCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
            return date.Date.ToString("yyyy/MM/dd", arCul.DateTimeFormat);
        }

        public static string UmAlquraToGreg(string d)
        {
            //string[] allFormats = { "yyyy/MM/dd", "yyyy/M/d", "dd/MM/yyyy", "d/M/yyyy", "dd/M/yyyy", "dd/MM/yy", "d/MM/yyyy", "yyyy-MM-dd", "yyyy-M-d", "dd-MM-yyyy", "d-M-yyyy", "dd-M-yyyy", "d-MM-yyyy", "yyyy MM dd", "yyyy M d", "dd MM yyyy", "d M yyyy", "dd M yyyy", "d MM yyyy" };
            if (d != null)
            {
                try
                {
                    CultureInfo arCul = new CultureInfo("ar-SA");
                    CultureInfo enCul = new CultureInfo("en-US");

                    arCul.DateTimeFormat.Calendar = new UmAlQuraCalendar();
                    enCul.DateTimeFormat.Calendar = new GregorianCalendar(GregorianCalendarTypes.USEnglish);

                    DateTime date = DateTime.ParseExact(d, allFormats, arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
                    // return Convert.ToDateTime(date.Date.ToString("yyyy/MM/dd", enCul.DateTimeFormat)).Date;
                    return date.Date.ToString("yyyy-MM-dd", enCul.DateTimeFormat);
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TheDate">TheDate is normal Gregorian date</param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static DateTime AddYearMonthDayInUmAlQuraDate(DateTime TheDate, int year, int month, int day)
        {
            DateTime date1 = new DateTime(TheDate.Year, TheDate.Month, TheDate.Day, new GregorianCalendar());
            UmAlQuraCalendar cal = new UmAlQuraCalendar();

            date1 = cal.AddYears(date1, year);
            date1 = cal.AddMonths(date1, month);
            date1 = cal.AddDays(date1, day);

            return date1;
        }

        public static double GetUmAlquraDateDiff(string StartDate, string EndDate)
        {
            DateTime StartDat = new DateTime(Convert.ToInt32(StartDate.Substring(0, 4)), Convert.ToInt32(StartDate.Substring(5, 2)), Convert.ToInt32(StartDate.Substring(8, 2)), new UmAlQuraCalendar());
            DateTime EndDat = new DateTime(Convert.ToInt32(EndDate.Substring(0, 4)), Convert.ToInt32(EndDate.Substring(5, 2)), Convert.ToInt32(EndDate.Substring(8, 2)), new UmAlQuraCalendar());
            TimeSpan ts = EndDat - StartDat;

            return ts.TotalDays + 1;
        }
    }

    public sealed class Password
    {
        public static bool ValidatePassword(string password)
        {
            string patternPassword = @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$";
            if (!string.IsNullOrEmpty(password))
            {
                if (!Regex.IsMatch(password, patternPassword))
                {
                    return false;
                }
            }
            return true;
        }

    }
}