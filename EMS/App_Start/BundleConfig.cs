using System.Web;
using System.Web.Optimization;

namespace EMS
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bootstrap/css").Include(
                "~/Content/bootstrap/dist/css/bootstrap.min.css"));
            bundles.Add(new StyleBundle("~/jquery-ui/css").Include(
                "~/Content/jquery-ui/jquery-ui.css"));
            bundles.Add(new StyleBundle("~/font-awesome/css").Include(
                "~/Content/font-awesome/css/font-awesome.min.css"));
            bundles.Add(new StyleBundle("~/ionicons/css").Include(
                "~/Content/Ionicons/css/ionicons.min.css"));
            bundles.Add(new StyleBundle("~/iCheck/css").Include(
                "~/Content/iCheck/all.css"));
            bundles.Add(new StyleBundle("~/select2/css").Include(
                "~/Content/select2/dist/css/select2.min.css"));
            bundles.Add(new StyleBundle("~/theme/css").Include(
                "~/Content/dist/css/AdminLTE.min.css"));
            bundles.Add(new StyleBundle("~/adminSkin/css").Include(
                "~/Content/dist/css/skins/_all-skins.min.css"));
            bundles.Add(new StyleBundle("~/dashboardSkin/css").Include(
                "~/Content/dist/css/skins/_all-skins.min.css"));
            bundles.Add(new StyleBundle("~/datatable/css").Include(
                "~/Content/datatables.net/css/dataTables.bootstrap.min.css"));
            bundles.Add(new StyleBundle("~/custom/css").Include(
                "~/Content/custom.css"));
            bundles.Add(new StyleBundle("~/login/css").Include(
                "~/Content/login.css"));


            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Content/jquery/dist/jquery.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
                "~/Content/jquery-ui/jquery-ui.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Content/bootstrap/dist/js/bootstrap.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/slimscroll").Include(
                "~/Content/jquery-slimscroll/jquery.slimscroll.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/iCheck").Include(
                "~/Content/iCheck/icheck.js"));
            bundles.Add(new ScriptBundle("~/bundles/select2").Include(
                "~/Content/select2/dist/js/select2.full.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/fastclick").Include(
                "~/Content/fastclick/lib/fastclick.js"));
            bundles.Add(new ScriptBundle("~/bundles/theme").Include(
                "~/Content/dist/js/adminlte.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                "~/Scripts/angular.js",
                "~/Scripts/angular-datatables.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatable").Include(
                "~/Content/datatables.net/js/jquery.dataTables.min.js",
                "~/Content/datatables.net/js/dataTables.bootstrap.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/login").Include(
                "~/Scripts/login.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));


            //               bundles.Add(new StyleBundle("~/bootstrap/css").Include(
            //          "~/Content/bower_components/bootstrap/dist/css/bootstrap.min.css"));
            //bundles.Add(new StyleBundle("~/jquery-ui/css").Include(
            //          "~/Content/bower_components/jquery-ui-1.12.1/jquery-ui.css"));
            //bundles.Add(new StyleBundle("~/font-awesome/css").Include(
            //          "~/Content/bower_components/font-awesome/css/font-awesome.min.css"));
            //bundles.Add(new StyleBundle("~/ionicons/css").Include(
            //          "~/Content/bower_components/Ionicons/css/ionicons.min.css"));
            //bundles.Add(new StyleBundle("~/iCheck/css").Include(
            //          "~/plugins/iCheck/all.css"));
            //bundles.Add(new StyleBundle("~/theme/css").Include(
            //          "~/Content/dist/css/AdminLTE.min.css"));
            //bundles.Add(new StyleBundle("~/adminTheme/css").Include(
            //          "~/Content/dist/css/skins/skin-red-light.min.css"));
            //bundles.Add(new StyleBundle("~/dashboardTheme/css").Include(
            //          "~/Content/dist/css/skins/skin-reskin-blue-light.min.css"));
            //bundles.Add(new StyleBundle("~/chart/css").Include(
            //          "~/Content/bower_components/morris.js/morris.css"));
            //bundles.Add(new StyleBundle("~/jvectormap/css").Include(
            //          "~/Content/bower_components/jvectormap/jquery-jvectormap.css"));
            //bundles.Add(new StyleBundle("~/datepicker/css").Include(
            //          "~/Content/bower_components/bootstrap-datepicker/dist/css/bootstrap-datepicker.min.css"));
            //bundles.Add(new StyleBundle("~/daterangepicker/css").Include(
            //          "~/Content/bower_components/bootstrap-daterangepicker/daterangepicker.css"));
            //bundles.Add(new StyleBundle("~/wysihtml5/css").Include(
            //          "~/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.min.css"));
            //bundles.Add(new StyleBundle("~/datatable/css").Include(
            //          "~/Content/bower_components/datatables.net/css/dataTables.bootstrap.min.css"));
            //bundles.Add(new StyleBundle("~/custom/css").Include(
            //          "~/Content/custom.css"));

            ////bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            ////            "~/Content/bower_components/jquery/dist/jquery.min.js",
            ////            "~/Content/bower_components/jquery-ui/jquery-ui.min.js"));
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Content/bower_components/jquery-ui-1.12.1/external/jquery/jquery.js",
            //            "~/Content/bower_components/jquery-ui-1.12.1/jquery-ui.js"));
            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Content/bower_components/bootstrap/dist/js/bootstrap.min.js"));
            //bundles.Add(new ScriptBundle("~/bundles/morris").Include(
            //         "~/Content/bower_components/raphael/raphael.min.js",
            //         "~/Content/bower_components/morris.js/morris.min.js"));
            //bundles.Add(new ScriptBundle("~/bundles/sparkline").Include(
            //         "~/Content/bower_components/jquery-sparkline/dist/jquery.sparkline.min.js"));
            //bundles.Add(new ScriptBundle("~/bundles/jvectormap").Include(
            //         "~/plugins/jvectormap/jquery-jvectormap-1.2.2.min.js",
            //         "~/plugins/jvectormap/jquery-jvectormap-world-mill-en.js"));

            //bundles.Add(new ScriptBundle("~/bundles/knob").Include(
            //         "~/Content/bower_components/jquery-knob/dist/jquery.knob.min.js"));
            //bundles.Add(new ScriptBundle("~/bundles/daterangepicker").Include(
            //         "~/Content/bower_components/moment/min/moment.min.js",
            //         "~/Content/bower_components/bootstrap-daterangepicker/daterangepicker.js"));
            //bundles.Add(new ScriptBundle("~/bundles/datepicker").Include(
            //         "~/Content/bower_components/bootstrap-datepicker/dist/js/bootstrap-datepicker.min.js"));
            //bundles.Add(new ScriptBundle("~/bundles/wysihtml5").Include(
            //         "~/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.all.min.js"));
            //bundles.Add(new ScriptBundle("~/bundles/slimscroll").Include(
            //         "~/Content/bower_components/jquery-slimscroll/jquery.slimscroll.min.js"));
            //bundles.Add(new ScriptBundle("~/bundles/iCheck").Include(
            //         "~/plugins/iCheck/icheck.js"));
            //bundles.Add(new ScriptBundle("~/bundles/fastclick").Include(
            //         "~/Content/bower_components/fastclick/lib/fastclick.js"));
            //bundles.Add(new ScriptBundle("~/bundles/theme").Include(
            //         "~/Content/dist/js/adminlte.min.js"));

            //bundles.Add(new ScriptBundle("~/bundles/angular").Include(
            //            "~/Scripts/angular.js",
            //            "~/Scripts/angular-datatables.js"));
            //bundles.Add(new ScriptBundle("~/bundles/datatable").Include(
            //            "~/Content/bower_components/datatables.net/js/jquery.dataTables.min.js",
            //            "~/Content/bower_components/datatables.net/js/dataTables.bootstrap.min.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

        }
    }
}
