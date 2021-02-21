using System.Web.Mvc;
using System.Web.Routing;

namespace HCM
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.aspx/{*pathInfo}");
            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
            name: "Default",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
              );


            routes.MapRoute(
                name: "Modal",
                url: "{controller}/{action}/{ModalType}/{Message}/{CallBackFnName}",
                defaults: new { controller = "Modal", action = "Index", ModalType = UrlParameter.Optional, Message = UrlParameter.Optional, CallBackFnName = UrlParameter.Optional }
            );


            //routes.MapRoute(
            //    name: "RenderingPromotionToolbar",
            //    url: "{controller}/{action}/{PromotionRecordStatus}/",
            //    defaults: new { controller = "PromotionsRecords", action = "RenderToolBar", PromotionRecordStatus = UrlParameter.Optional }
            //);






            //routes.MapMvcAttributeRoutes();

            // Show a 404 error page for anything else.
            //routes.MapRoute(
            //    "Error", 
            //    "{*url}",
            //    new { controller = "Error", action = "404" }
            //);
        }
    }
}
