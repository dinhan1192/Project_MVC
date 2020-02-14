using System.Web;
using System.Web.Optimization;

namespace Project_MVC
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                      "~/Scripts/Customs/deleteNotify.js",
                      "~/Scripts/Customs/autocomplete.js",
                      "~/Scripts/Customs/checkFileSize.js",
                      "~/Scripts/Customs/checkboxselectallWithPopup.js",
                      "~/Scripts/Customs/twoDropDownListEvent.js",
                      "~/Scripts/Customs/createPopup.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/video").Include(
                      "~/Scripts/Customs/customerMustWatchVideo.js",
                      "~/Scripts/Customs/preventSeekingVideo.js",
                      "~/Scripts/Customs/setStyleDefault.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/typeahead").Include(
                      "~/Scripts/bootstrap3-typeahead.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/Js").Include(
                     "~/Scripts/vendor/bootstrap/js/bootstrap.bundle.min.js",
                     "~/Scripts/vendor/jquery-easing/jquery.easing.min.js",
                     //"~/Scripts/vendor/chart.js/Chart.min.js",
                     "~/Scripts/vendor/datatables/jquery.dataTables.js",
                     "~/Scripts/vendor/datatables/dataTables.bootstrap4.js",
                     "~/Scripts/js/sb-admin.min.js",
                     "~/Scripts/js/demo/datatables-demo.js",
                     "~/Scripts/Customs/jquery-confirmPopup.js"
                     //"~/Scripts/js/demo/chart-area-demo.js"
                     ));

            bundles.Add(new StyleBundle("~/Css").Include(
                      "~/Content/vendor/fontawesome-free/css/all.min.css",
                      //"~/Content/vendor/datatables/dataTables.bootstrap4.css",
                      "~/Content/css/sb-admin.css",
                      "~/Content/css/jquery-confirmPopup.css"
                      ));

            bundles.Add(new StyleBundle("~/customs").Include(
                      "~/Content/Customs/completeAndInComplete.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/layoutAdminPage").Include(
                     "~/Content/LayoutAdminPage/bootstrap.min.css",
                     "~/Content/LayoutAdminPage/mdb.min.css",
                     "~/Content/LayoutAdminPage/style.min.css",
                     "~/Content/LayoutAdminPage/customAdmin.css"
                     ));

            bundles.Add(new StyleBundle("~/Content/fonts").Include(
                    "~/Content/LayoutAdminPage/fontawesome.css"
                    ));


            bundles.Add(new ScriptBundle("~/bundles/layoutAdminPage").Include(
                      "~/Scripts/LayoutAdminPage/popper.min.js",
                      "~/Scripts/LayoutAdminPage/mdb.min.js"
                      ));

            bundles.Add(new StyleBundle("~/bundles/customCustomerPageCSS").Include(
               "~/Content/CustomCustomerPage/Phong.css",
               "~/Content/CustomCustomerPage/bootstrap.css",
               "~/Content/CustomCustomerPage/animate.css",
               "~/Content/CustomCustomerPage/owl.carousel.min.css",
               "~/Content/CustomCustomerPage/style.css",
               "~/Content/CustomCustomerPage/bootstrap.css.map",
               "~/Content/css/jquery-confirmPopup.css"
               ));

            bundles.Add(new ScriptBundle("~/bundles/customCustomerPageJS").Include(
                     //"~/Scripts/CustomCustomerPage/jquery-3.2.1.min.js",
                     "~/Scripts/CustomCustomerPage/jquery-migrate-3.0.0.js",
                     "~/Scripts/CustomCustomerPage/popper.min.js",
                     "~/Scripts/CustomCustomerPage/bootstrap.min.js",
                     "~/Scripts/CustomCustomerPage/owl.carousel.min.js",
                     "~/Scripts/CustomCustomerPage/jquery.waypoints.min.js",
                     "~/Scripts/CustomCustomerPage/jquery.stellar.min.js",
                     "~/Scripts/CustomCustomerPage/main.js",
                     "~/Scripts/Customs/jquery-confirmPopup.js"
                     ));

            // BundleTable.EnableOptimizations = true;
        }
    }
}
