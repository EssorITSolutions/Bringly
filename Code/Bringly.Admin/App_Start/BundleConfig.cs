using System.Web;
using System.Web.Optimization;

namespace Bringly.Admin
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            /*Bundel For Login User*/
            bundles.Add(new ScriptBundle("~/Ljs").Include(
                      "~/Templates/resources/js/jquery/jquery-1.11.1.js",
                       //Validation Js Starts
                       "~/Templates/resources/js/validation/jquery.validate.js",
                       "~/Templates/resources/js/validation/jquery.validate.unobtrusive.js",
                       "~/Templates/resources/js/validation/jquery-migrate-1.1.0.js",
                      //Validation Js Starts
                      "~/Templates/resources/js/commonJs.js",
                      "~/Templates/resources/js/secureJs.js",
                        "~/Templates/resources/js/jquery.blockUI.js",

                          "~/Templates/resources/js/Plugin/drpAUtoHelp/chosen.jquery.js",
                         "~/Templates/resources/js/Plugin/drpAUtoHelp/prism.js",

                         "~/Templates/resources/js/Plugin/AlertMessage/sweetalert-dev.js",
                 //Smooth scroll js
                 "~/Templates/resources/js/Plugin/smoothscroll.js"

                        ));


            /*Bundel For Not Logged In User*/
            bundles.Add(new ScriptBundle("~/js").Include(
                      "~/Templates/resources/js/jquery/jquery-1.11.1.js",
                       //Validation Js Starts
                       "~/Templates/resources/js/validation/jquery.validate.js",
                       "~/Templates/resources/js/validation/jquery.validate.unobtrusive.js",
                       "~/Templates/resources/js/validation/jquery-migrate-1.1.0.js",
                      //Validation Js Starts
                      "~/Templates/resources/js/commonJs.js",

                      "~/Templates/resources/js/Plugin/AlertMessage/sweetalert-dev.js",
                 //Smooth scroll js
                 "~/Templates/resources/js/Plugin/smoothscroll.js"

                        ));


            bundles.Add(new StyleBundle("~/css").Include(
                    "~/Templates/resources/bootstrap/css/bootstrap.css",
                    //"~/Templates/resources/css/fonts/ptsans/stylesheet.css",

                    "~/Templates/resources/css/fonts/icomoon/style.css",
                    "~/Templates/resources/css/mws-style.css",
                    "~/Templates/resources/css/mws-theme.css",
                    "~/Templates/resources/css/themer.css",
                    "~/Templates/resources/css/wizard/wizard.css",
                    "~/Templates/resources/js/Plugin/drpAUtoHelp/chosen.css",
                      "~/Templates/resources/js/Plugin/AlertMessage/sweetalert.css"

                    ));
        }
    }
}
