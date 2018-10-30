using Nop.Core.Plugins;
using Nop.Services.Common;
using Nop.Services.Localization;
using System.Web.Routing;

namespace AtrendUsa.Plugin.Misc.BuildYourBox
{
    /// <summary>
    /// PLugin
    /// </summary>
    public class BuildYourBoxPlugin : BasePlugin, IMiscPlugin
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
            controllerName = "BuildYourBox";
            routeValues = new RouteValueDictionary { { "Namespaces", "AtrendUsa.Plugin.Misc.BuildYourBox.Controllers" }, { "area", null } };
        }

        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.BoxSpecifications", "Box Specifications");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.Button", "Submit");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.CarpetColor", "Carpet Color");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.Depth", "Depth");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.FieldRequired", "Required");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.Height", "Height");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.ImageOfVehicleTrunkArea", "Image of Vehicle Trunk Area (stand 1 foot back and take image)");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.MDFThicknessRequested", "MDF Thickness Requested");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.NumberOfSubwoofers", "Number of Subwoofers Used");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.SealedOrVented", "Sealed or Vented");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.SquareOrAngledBox", "Square or Angled Box");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.SubwooferDataSection", "Subwoofer Data Section");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.ContactAndSubInfo", "Contact and Sub Info");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.SubwooferManufacturer", "Subwoofer Manufacturer");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.SubwooferModel", "Subwoofer Model");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.TerminalLocation", "Terminal Location (sides/back)");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.TerminalType", "Terminal Type");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.VehicleDataSection", "Vehicle Data Section");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.VehicleManufacturerOrModel", "Vehicle Manufacturer/Model");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.VehicleYear", "Vehicle Year");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.Width", "Width");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.SelectCarpetColor", "Select Carpet Color");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.SelectTerminalType", "Select Terminal Type");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.SelectMDFThickness", "Select MDF Thickness");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.SelectMDFThickness", "Select MDF Thickness");

            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.Name", "Name");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.Address", "Address");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.PhoneNumber", "Phone Number");
            this.AddOrUpdatePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.Email", "Email");

            base.Install();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.BoxSpecifications");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.Button");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.CarpetColor");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.Depth");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.FieldRequired");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.Height");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.ImageOfVehicleTrunkArea");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.MDFThicknessRequested");
            this.DeletePluginLocaleResource("atrendusa.plugin.misc.buildyourbox.numberofsubwoofers");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.SealedOrVented");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.SquareOrAngledBox");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.SubwooferDataSection");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.ContactAndSubInfo");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.SubwooferManufacturer");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.SubwooferModel");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.TerminalLocation");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.TerminalType");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.VehicleDataSection");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.VehicleManufacturerOrModel");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.VehicleYear");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.Width");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.SelectCarpetColor");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.SelectTerminalType");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.SelectMDFThickness");

            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.Name");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.Address");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.PhoneNumber");
            this.DeletePluginLocaleResource("AtrendUsa.Plugin.Misc.BuildYourBox.Email");

            base.Uninstall();
        }
    }
}
