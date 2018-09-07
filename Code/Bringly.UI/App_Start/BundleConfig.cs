using System.Web.Optimization;

namespace Bringly.UI
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region css
            bundles.Add(new StyleBundle("~/css").Include(

                 //dropdown to auto help
                 "~/Templates/scripts/Plugin/drpAUtoHelp/chosen.css"

                //dropdown to auto help
                , "~/Templates/scripts/Plugins/drpAUtoHelp/chosen.css"

                //css
                , "~/Templates/css/*.css"


                //alert message css
                , "~/Templates/scripts/Plugins/AlertMessage/sweetalert.css"

                 //datetime picker
                 , "~/Templates/Scripts/Plugins/DateTimePicker/jquery.datetimepicker.css"
               //"~/Templates/Scripts/Plugins/ckeditor/contents.css"
               ));

            bundles.Add(new StyleBundle("~/Ngcss").Include("~/Templates/scripts/libs/styles.c362bfb2c56b5b71a27e.css"));

            #endregion

            #region javascript Bundel
            bundles.Add(new ScriptBundle("~/Ljs").Include(
                  "~/Templates/scripts/jquery/jquery-3.2.1.js"
                  , "~/Templates/scripts/jquery/jquery-ui.js"

                       //Validation Js Starts
                       , "~/Templates/scripts/validation/jquery.validate.js"
                       , "~/Templates/scripts/validation/jquery.validate.unobtrusive.js"
                       , "~/Templates/scripts/validation/jquery-migrate-1.1.0.js"
                //Validation Js Starts

                //"~/Templates/resources/js/commonJs.js",
                //"~/Templates/resources/js/secureJs.js",


                //jquery UI.block
                , "~/Templates/scripts/Plugins/jquery.blockUI.js"

                 //dropdown to auto help
                 , "~/Templates/scripts/Plugins/drpAUtoHelp/chosen.jquery.js"
                 , "~/Templates/scripts/Plugins/drpAUtoHelp/prism.js"

                 //bootstrap js
                 , "~/Templates/scripts/bootstrap.js"

                 //alert message js
                 , "~/Templates/scripts/Plugins/AlertMessage/sweetalert-dev.js"

                , "~/Templates/scripts/commonJs.js"

                , "~/Templates/scripts/SecureJs.js"


                 //datetime picker
                 , "~/Templates/Scripts/Plugins/DateTimePicker/jquery.datetimepicker.js"
                 //ck editor js

                 , "~/Templates/Scripts/Plugins/ckeditor/ckeditor.js"
                //,"~/Templates/Scripts/Plugins/ckeditor/plugins/maxlengthplugin.js"
                ));


            bundles.Add(new ScriptBundle("~/NgLjs").Include(
                "~/Templates/Scripts/libs/polyfills.7a0e6866a34e280f48e7.js",
                "~/Templates/Scripts/libs/runtime.a66f828dca56eeb90e02.js",
                "~/Templates/Scripts/libs/scripts.f2e2c97149783497f7de.js",
                "~/Templates/Scripts/libs/main.48acce8a674ea1769784.js"
                ));
            #endregion
        }
    }
}
