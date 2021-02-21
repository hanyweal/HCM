using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace HCM
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_Start()
        {
            //Session["EmployeeCodeNo"] = "90012454";
            //Session["UserID"] = "6";
        }

        public override string GetVaryByCustomString(HttpContext context, string custom)
        {
            if (custom.ToLower() == "sessionid")
            {
                HttpCookie cookie = context.Request.Cookies["ASP.NET_SessionId"];
                if (cookie != null)
                    return cookie.Value;
            }
            return base.GetVaryByCustomString(context, custom);
        }
    }
}
