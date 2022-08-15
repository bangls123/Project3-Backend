using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Vnext.Intern.Web
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            var getMethodConstraints = new HttpMethodConstraint(new string[] { "GET" });
            var postMethodConstraints = new HttpMethodConstraint(new string[] { "POST" });

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //ASP.NET Web API Route Config
            routes.MapHttpRoute(
                name: "DownloadApi",
                routeTemplate: "api/{controller}/file",
                defaults: new { action = "Download" },
                constraints: new RouteValueDictionary { { "httpMethod", getMethodConstraints } }
            );
            routes.MapHttpRoute(
                name: "UploadApi",
                routeTemplate: "api/{controller}/file",
                defaults: new { action = "Upload" },
                constraints: new RouteValueDictionary { { "httpMethod", postMethodConstraints } }
            );

            routes.MapHttpRoute(
                name: "GetListApi",
                routeTemplate: "api/{controller}",
                defaults: new { action = "GetList" },
                constraints: new RouteValueDictionary { { "httpMethod", getMethodConstraints } }
            );
            routes.MapHttpRoute(
                name: "CreateApi",
                routeTemplate: "api/{controller}",
                defaults: new { action = "Create" },
                constraints: new RouteValueDictionary { { "httpMethod", postMethodConstraints } }
            );

            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

