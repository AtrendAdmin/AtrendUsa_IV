using System.Web.Mvc;
using Nop.Web.Framework.Mvc.Routes;


namespace AtrendUsa.Plugin.Misc.BuildYourBox.Infrastructure
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

            routes.MapRoute("AtrendUsa.Plugin.Misc.BuildYourBox",
                 "BuildYourBox",
                 new { controller = "BuildYourBox", action = "Index" },
                 new[] { "AtrendUsa.Plugin.Misc.BuildYourBox.Controllers" }
            );
        }
    }
}
