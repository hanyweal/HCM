using HCM.DI;
using MvcSiteMapProvider.Loader;
using MvcSiteMapProvider.Web.Mvc;
using MvcSiteMapProvider.Xml;
using System.Web.Hosting;
using System.Web.Routing;

internal class MvcSiteMapProviderConfig
{
    public static void Register(IDependencyInjectionContainer container)
    {
        // Setup global sitemap loader
        MvcSiteMapProvider.SiteMaps.Loader = container.GetInstance<ISiteMapLoader>();

        // Check all configured .sitemap files to ensure they follow the XSD for MvcSiteMapProvider
        var validator = container.GetInstance<ISiteMapXmlValidator>();
        validator.ValidateXml(HostingEnvironment.MapPath("~/Mvc.sitemap"));

        // Register the Sitemaps routes for search engines
        XmlSiteMapController.RegisterRoutes(RouteTable.Routes);
    }
}