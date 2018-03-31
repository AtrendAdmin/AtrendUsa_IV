using Nop.Core.Plugins;
using Nop.Services.Cms;
using Nop.Services.Localization;
using System.Collections.Generic;
using System.Web.Routing;
using Nop.Web.Framework.Menu;
using Nop.Web.Framework.Web;

namespace AtrendUsa.Plugin.Widgets.News
{
    /// <summary>
    /// PLugin
    /// </summary>
    public class NewsPlugin : BasePlugin, IAdminMenuPlugin
    {

        /// <summary>
        /// Gets a route for provider configuration
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Upload";
            controllerName = "Upload";
            routeValues = new RouteValueDictionary { { "Namespaces", "AtrendUsa.Plugin.Admin.Uploader.Controllers" }, { "area", "admin" } };
        }

        ///// <summary>
        ///// Gets a route for displaying widget
        ///// </summary>
        ///// <param name="widgetZone">Widget zone where it's displayed</param>
        ///// <param name="actionName">Action name</param>
        ///// <param name="controllerName">Controller name</param>
        ///// <param name="routeValues">Route values</param>
        //public void GetDisplayWidgetRoute(string widgetZone, out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        //{
        //    actionName = "PublicInfo";
        //    controllerName = "WidgetsNews";
        //    routeValues = new RouteValueDictionary
        //    {
        //        {"Namespaces", "AtrendUsa.Plugin.Admin.Uploader.Controllers"},
        //        {"area", null},
        //        {"widgetZone", widgetZone}
        //    };
        //}

        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Admin.Uploader", "Product Uploader");

            base.Install();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            //locales
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Admin.Uploader");

            base.Uninstall();
        }

        #region Implementation of IAdminMenuPlugin

        public bool Authenticate()
        {
            return true;
        }

        public SiteMapNode BuildMenuItem()
        {
            return new SiteMapNode
            {
                Title = "Product Uploader",
                ControllerName = "Upload",
                ActionName = "Upload",
                Visible = true,
                RouteValues = new RouteValueDictionary { { "area", null } }
            };
        }

        #endregion
    }
}
