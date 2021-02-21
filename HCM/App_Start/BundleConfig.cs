using System.Web.Optimization;

namespace HCM
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-1.12.1.min.js",
                        "~/Scripts/Jquery.extend.js",
                        "~/Scripts/DataTables/jquery.dataTables.min.js",
                        "~/Scripts/DataTables/Custom/DataTablesRendering.js",
                        "~/Scripts/DataTables/dataTables.buttons.min.js",
                        "~/Scripts/DataTables/buttons.flash.min.js",
                        "~/Scripts/DataTables/jszip.min.js",
                        //"~/Scripts/DataTables/pdfmake.min.js",
                        "~/Scripts/DataTables/vfs_fonts.js",
                        "~/Scripts/DataTables/buttons.html5.min.js",
                        "~/Scripts/DataTables/buttons.print.min.js",

                        "~/Scripts/DataTables/dataTables.buttons.min.js",
                        "~/Scripts/DataTables/buttons.flash.min.js",
                        "~/Scripts/DataTables/buttons.html5.js",
                        "~/Scripts/DataTables/buttons.html5.min.js",

                        "~/Scripts/DataTables/input.js",

                        "~/Scripts/Toast/jquery.toast.min.js",
                        "~/Scripts/Confirm/jquery-confirm.min.js",
                        "~/Scripts/bootstrap-treeview.js",
                        "~/Scripts/bootstrap-toggle.min.js",
                        //"~/Scripts/moment.js",
                        "~/Scripts/moment-with-locales.js",
                        "~/Scripts/moment-hijri.js",
                        "~/Scripts/DatePick/jquery.plugin.js",
                        "~/Scripts/DatePick/jquery.calendars.js",
                        "~/Scripts/DatePick/jquery.calendars.plus.js",
                        "~/Scripts/DatePick/jquery.calendars.picker.js",
                        "~/Scripts/DatePick/jquery.calendars.ummalqura.js",
                        "~/Scripts/DatePick/jquery.calendars.ummalqura-ar.js",
                        "~/Scripts/DatePick/jquery.calendars.picker-ar.js",
                        "~/Scripts/Chart.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*",
                        //"~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.validate.unobtrusive.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/orgChart").Include(
                        //"~/Scripts/getorgchart.js"
                        "~/Scripts/orgchart.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/JavaScript").Include(
                        "~/Scripts/JScript.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap-rtl.css",
                      "~/Content/Global.css",
                      "~/Content/DataTables/css/jquery.dataTables.min.css",
                      "~/Content/themes/base/jquery-ui.min.css",
                      "~/Content/Toast/jquery.toast.min.css",
                      "~/Content/jquery-confirm.min.css",
                      "~/Content/bootstrap-treeview.css",
                      "~/Scripts/DatePick/jquery.calendars.picker.css",
                      "~/Content/font-awesome-5.7/css/all.min.css",
                      "~/Content/bootstrap-toggle.min.css",
                      "~/Content/getorgchart.css"

                      //"~/Content/fonts/font-awesome/css/font-awesome.css" bootstrap-treeview
                      ));
        }
    }
}
