using System.Web;
using System.Web.Optimization;

namespace GrassrootsFloodCtrl
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //~/Bundles/vendor/css
            bundles.Add(
               new StyleBundle("~/Bundles/vendor/css")
                   .Include(
                       //"~/Content/themes/base/all.css",
                       "~/Content/bootstrap.css",
                       "~/Content/toastr.min.css",
                       "~/scripts/sweetalert/sweet-alert.css",
                       "~/Content/font-awesome.min.css",
                       "~/Content/select2/css/select2.css",
                       "~/Content/bootstrap-treeview/bootstrap-treeview.min.css",
                       "~/Content/simplePage/simplePage.css",
                       "~/css/style.css",
                       "~/css/style2.css",
                       "~/css/main.css",
                       "~/css/zzzrCss.css"
                   )
               );

            bundles.Add(new StyleBundle("~/css/leaflet").Include(
                 "~/Content/leaflet/leaflet.css",
                 "~/Content/bootstrapSwitch.css",
                 "~/Content/leaflet.draw/leaflet.draw.css",
                 //"~/Content/leaflet.draw/mapbox.css",
                 //"~/Content/leaflet.draw/leaflet.draw.2.3.css",
                 "~/Content/leaflet.label/leaflet.label.css",
                 "~/Content/MapBar/map.utilbar.css"
            ));

            //~/Bundles/vendor/js/top (These scripts should be included in the head of the page)
            bundles.Add(
                new ScriptBundle("~/Bundles/vendor/js/top")
                    .Include(
                        "~/Abp/Framework/scripts/utils/ie10fix.js",
                        "~/scripts/jquery-1.11.3.min.js",
                        "~/scripts/jquery.cookie.js"
                    //"~/scripts/modernizr-2.8.3.js"
                    )
                );

            //~/Bundles/vendor/bottom (Included in the bottom for fast page load)
            bundles.Add(
                new ScriptBundle("~/Bundles/vendor/js/bottom")
                    .Include(
                        "~/scripts/json2.min.js",

                        "~/scripts/bootstrap.min.js",
                        //"~/scripts/jquery.cookie.js",
                        //"~/scripts/jquery.dcjqaccordion.2.7.js",
                        "~/Content/select2/js/select2.js",
                        "~/Content/select2/js/i18n/zh-CN.js",
                        "~/Content/simplePage/simplePage.js",
                        "~/Content/jsrender/jsrender.js",
                        "~/scripts/moment-with-locales.min.js",
                        "~/scripts/jquery.validate.min.js",
                        "~/scripts/jquery.blockUI.js",
                        "~/scripts/toastr.min.js",
                        "~/scripts/sweetalert/sweet-alert.min.js",
                        "~/scripts/others/spinjs/spin.js",
                        "~/scripts/others/spinjs/jquery.spin.js",
                       "~/Content/bootstrap-treeview/bootstrap-treeview.min.js",

                        "~/Abp/Framework/scripts/abp.js",
                        "~/Abp/Framework/scripts/libs/abp.jquery.js",
                        "~/Abp/Framework/scripts/libs/abp.toastr.js",
                        "~/Abp/Framework/scripts/libs/abp.blockUI.js",
                        "~/Abp/Framework/scripts/libs/abp.spin.js",
                        "~/Abp/Framework/scripts/libs/abp.sweet-alert.js"
                    )
                );

            bundles.Add(new ScriptBundle("~/js/plupload").Include(
                "~/Content/plupload/moxie.js",
                "~/Content/plupload/plupload.dev.js",
                "~/Content/plupload/zh_CN.js"
                ));

            bundles.Add(new ScriptBundle("~/js/leaflet").Include(
                 "~/Content/leaflet/leaflet-src.js",
                 "~/Content/leaflet/leaflet-extend.js",
                 "~/Content/bootstrapSwitch.js",
                 //"~/Content/leaflet.draw/leaflet.draw.2.3.js",
                 //"~/Content/leaflet.draw/mapbox.js",
                 "~/Content/leaflet.draw/leaflet.draw-src.js",
                 "~/Content/leaflet/leaflet.draw.custom.js",
                 "~/Content/leaflet/leaflet.draw.local_cn.js",
                 "~/Content/leaflet.label/leaflet.label-src.js",
                 "~/Content/leaflet/L.Map.Sync.js",
                 "~/Content/MapBar/map.utilbar.js",
                 "~/js/map-loader.js"
            ));

            bundles.Add(new ScriptBundle("~/js/mapEdit").Include(
                "~/js/map-edit.js"
                ));

            bundles.Add(new ScriptBundle("~/js/echarts").Include(
                    "~/Content/echarts/echarts.common.min.js"
                ));

            bundles.Add(new ScriptBundle("~/js/bootstrap-plugin").Include(
                "~/Content/bootstrap-table/bootstrap-table.js",
                "~/Content/bootstrap-table/locale/bootstrap-table-zh-CN.js",
                "~/Content/bootstrap-datetimepicker/js/bootstrap-datetimepicker.js",
                "~/Content/bootstrap-datetimepicker/js/locales/bootstrap-datetimepicker.zh-CN.js"
                ));

            bundles.Add(new ScriptBundle("~/js/ztree").Include(
                 "~/Content/zTree/js/jquery.ztree.all-3.5.js"));

            bundles.Add(new StyleBundle("~/css/bootstrap-plugin").Include(
                 "~/Content/bootstrap-table/bootstrap-table.css",
                 "~/Content/bootstrap-datetimepicker/css/bootstrap-datetimepicker.css"
             ));

            bundles.Add(new StyleBundle("~/css/ztree").Include(
                 "~/Content/zTree/css/zTreeStyle/zTreeStyle.css"));

            //APPLICATION RESOURCES

            //~/Bundles/css
            bundles.Add(
                new StyleBundle("~/Bundles/css")
                    .Include("~/css/main.css")
                );

            //~/Bundles/js
            bundles.Add(
                new ScriptBundle("~/Bundles/js")
                    .Include("~/js/main.js")
                );

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
