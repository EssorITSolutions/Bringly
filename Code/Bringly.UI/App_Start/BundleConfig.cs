using System.Web;
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

                 ,  "~/Templates/Scripts/Plugins/ckeditor/ckeditor.js"
                  //,"~/Templates/Scripts/Plugins/ckeditor/plugins/maxlengthplugin.js"
                ));
            #endregion
        }
    }
}
