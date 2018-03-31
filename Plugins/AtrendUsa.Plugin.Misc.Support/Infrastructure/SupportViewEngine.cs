using Nop.Web.Framework.Themes;

namespace AtrendUsa.Plugin.Misc.Support.Infrastructure
{
    public class SupportViewEngine : ThemeableRazorViewEngine
    {
        public SupportViewEngine()
        {
            ViewLocationFormats = new[] { "~/Plugins/AtrendUsa.Plugin.Misc.Support/Views/{0}.cshtml" };
            PartialViewLocationFormats = new[] { "~/Plugins/AtrendUsa.Plugin.Misc.Support/Views/{0}.cshtml" };
        }
    }
}
