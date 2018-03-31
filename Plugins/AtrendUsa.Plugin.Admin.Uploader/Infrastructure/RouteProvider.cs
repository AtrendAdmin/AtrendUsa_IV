using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Mvc.Routes;

namespace AtrendUsa.Plugin.Admin.Uploader.Infrastructure
{
    public class RouteProvider : IRouteProvider
    {
        public int Priority
        {
            get
            {
                return 100;
            }
        }

        public void RegisterRoutes(RouteCollection routes)
        {
           var route =  routes.MapRoute("AtrendUsa.Plugin.Admin.Uploader",
                "Plugins/Upload/{action}",
                new {controller = "Upload", action = "Upload"},
                new[] {"AtrendUsa.Plugin.Admin.Uploader.Controllers"});
            //routes.MapRoute("AtrendUsa.Plugin.Admin.Uploader.Error", "Plugins/AnkoPluginAdminUploader/Upload", new
            //{
            //    controller = "Upload",
            //    action = "Upload"
            //}, new string[1]
            //{
            //    "AtrendUsa.Plugin.Admin.Uploader.Controllers"
            //});
        }
    }
}
