using System.Web;

namespace HCM.Classes
{
    public sealed class UIUtilities
    {
        public static string GetGlobalResource(string Member)
        {
            return HttpContext.GetGlobalResourceObject("Globalization", Member).ToString();
        }

        public static string GetControllerName(string URL)
        {
            return URL.Split('/')[3];
        }

        public static string GetActionName(string URL)
        {
            return URL.Split('/')[4];
        }
    }
}