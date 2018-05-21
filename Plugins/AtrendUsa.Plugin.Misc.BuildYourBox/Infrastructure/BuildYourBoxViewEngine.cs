using Nop.Web.Framework.Themes;

namespace AtrendUsa.Plugin.Misc.BuildYourBox.Infrastructure
{
    public class BuildYourBoxViewEngine : ThemeableRazorViewEngine
    {
        public BuildYourBoxViewEngine()
        {
            ViewLocationFormats = new[] { "~/Plugins/AtrendUsa.Plugin.Misc.BuildYourBox/Views/{0}.cshtml" };
            PartialViewLocationFormats = new[] { "~/Plugins/AtrendUsa.Plugin.Misc.BuildYourBox/Views/{0}.cshtml" };
        }
    }
}
