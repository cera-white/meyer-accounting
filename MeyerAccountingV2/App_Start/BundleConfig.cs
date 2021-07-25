using System.Web.Optimization;

namespace IdentitySample
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/jquery.validate.unobtrusive.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/vendor").Include(
                        "~/Scripts/jquery.appear.min.js",
                        "~/Scripts/jquery.easing.min.js",
                        "~/Scripts/jquery-cookie.min.js",
                        "~/Scripts/bootstrap.min.js",
                        "~/Scripts/common.min.js",
                        "~/Scripts/jquery.validation.min.js",
                        "~/Scripts/jquery.easy-pie-chart.min.js",
                        "~/Scripts/jquery.gmap.min.js",
                        "~/Scripts/jquery.lazyload.min.js",
                        "~/Scripts/jquery.isotope.min.js",
                        "~/Scripts/owl.carousel.min.js",
                        "~/Scripts/jquery.magnific-popup.min.js",
                        "~/Scripts/vide.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/theme").Include(
                        "~/Scripts/theme.min.js",
                        "~/Scripts/jquery.themepunch.tools.min.js",
                        "~/Scripts/jquery.themepunch.revolution.min.js",
                        "~/Scripts/demo-law-firm.min.js",
                        "~/Scripts/custom.js",
                        "~/Scripts/theme.init.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizer").Include(
                        "~/Scripts/modernizr.min.js"));

            bundles.Add(new StyleBundle("~/css/vendor").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/animate.min.css",
                      "~/Content/simple-line-icons.min.css",
                      "~/Content/owl.carousel.min.css",
                      "~/Content/owl.theme.default.min.css",
                      "~/Content/magnific-popup.min.css"));

            bundles.Add(new StyleBundle("~/css/theme").Include(
                      "~/Content/theme.min.css",
                      "~/Content/theme-elements.min.css",
                      "~/Content/theme-blog.min.css",
                      "~/Content/theme-shop.min.css",
                      "~/Content/settings.min.css",
                      "~/Content/layers.min.css",
                      "~/Content/navigation.min.css"));

            bundles.Add(new StyleBundle("~/css/skin").Include(
                      "~/Content/skin-law-firm.min.css",
                      "~/Content/demo-law-firm.min.css",
                      "~/Content/custom.css",
                      "~/Content/branding.css"));
        }
    }
}
