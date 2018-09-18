using System.Web;
using System.Web.Optimization;

namespace Cfs.Web.Incidents
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/scripts/app").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery-ui.min.js",
                "~/Scripts/modernizr.js",
                "~/Scripts/knockout-{version}.js",
                "~/Scripts/knockout.map.js",
                "~/Scripts/knockout.validation.js",
                "~/Scripts/notifications.js",
                "~/Scripts/moment.min.js",
                "~/Scripts/jquery.fancybox.js",
                "~/Scripts/jquery.hoverIntent.js",
                "~/Scripts/app-engine.js"
                ));


            bundles.Add(new StyleBundle("~/content/theme").Include(
                "~/Content/css/reset.css",
                "~/Content/css/fonts.css",
                "~/Content/css/jquery-ui.theme.css",
                "~/Content/css/jquery-ui.structure.css",
                "~/Content/css/notifications.css",
                "~/Content/css/jquery.fancybox.css",
                "~/Content/css/default.css"
                ));
        }
    }
}