using System.Web.Mvc;
using Nop.Web.Framework.Mvc.Routes;

namespace AtrendUsa.Plugin.Misc.Support.Infrastructure
{
    public class RouteProvider : IRouteProvider
    {
        public int Priority
        {
            get { return 0; }
        }

        public void RegisterRoutes(System.Web.Routing.RouteCollection routes)
        {
            //ViewEngines.Engines.Insert(0, new SupportViewEngine());

            routes.MapRoute("AtrendUsa.Plugin.Misc.Support",
                 "Support",
                 new { controller = "Support", action = "Index" },
                 new[] { "AtrendUsa.Plugin.Misc.Support.Controllers" }
            );
        }
    }
}
