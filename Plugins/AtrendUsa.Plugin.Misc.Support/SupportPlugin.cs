using Nop.Core.Plugins;
using Nop.Services.Common;
using Nop.Services.Localization;
using System.Web.Routing;

namespace AtrendUsa.Plugin.Misc.Support
{
    /// <summary>
    /// PLugin
    /// </summary>
    public class SupportPlugin : BasePlugin, IMiscPlugin
    {
        /// <summary>
        /// Gets a route for provider configuration
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "Support";
            routeValues = new RouteValueDictionary { { "Namespaces", "AtrendUsa.Plugin.Misc.Support.Controllers" }, { "area", null } };
        }

        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.PageTitle", "Support");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.Address", "Address");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.Details", "Details");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.Button", "Submit");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.SelectCountry", "Select country");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.SelectCategory", "Select category");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.SelectManufacturer", "Select brand");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.FieldRequired", "Required");

            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.EmailSubject", "Support request");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.RequestSuccessfulSent", "Your request has been successfully sent to the store owner.");

            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.FirstName", "First Name");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.LastName", "Last Name");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.Email", "Email");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.AddressLine1", "Address Line 1");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.AddressLine2", "Address Line 2");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.City", "City");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.StateProvince", "State / Province / Region");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.PostalCode", "Postal / Zip Code");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.Phone", "Phone Number");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.Country", "Country");

            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.ModelNumber", "Model #");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.UnderWarranty", "Is product under Warranty");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.DatePurchased", "Date purchased if Warranty");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.SerialNumber", "Serial #");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.Manufacturer", "Brand");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.Category", "Category");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.Problem", "Describe Problem");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.Dealer", "Dealer (required if Warranty)");

            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.SelectModelNumber", "Select Model Numbers");    //Added by IV Santosh 
            base.Install();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.PageTitle");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.Address");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.Details");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.Button");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.SelectCountry");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.SelectCategory");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.SelectManufacturer");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.FieldRequired");

            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.EmailSubject");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.RequestSuccessfulSent");

            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.FirstName");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.LastName");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.Email");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.AddressLine1");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.AddressLine2");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.City");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.StateProvince");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.PostalCode");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.Phone");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.Country");

            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.ModelNumber");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.UnderWarranty");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.DatePurchased");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.SerialNumber");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.Manufacturer");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.Category");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.Problem");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.Dealer");

            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.Support.SelectModelNumber"); //Added by IV Santosh 
            base.Uninstall();
        }
    }
}
