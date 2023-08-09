// ***********************************************************************
// Assembly         : EFMVC.Web
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="BootstrapBundleConfig.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Web.Optimization;
using EFMVC.Web.App_Start;
using WebActivatorEx;

/// <summary>
/// The App_Start namespace.
/// </summary>

[assembly: PostApplicationStartMethod(typeof (BootstrapBundleConfig), "RegisterBundles")]

namespace EFMVC.Web.App_Start
{
    /// <summary>
    /// Class BootstrapBundleConfig.
    /// </summary>
    public class BootstrapBundleConfig
    {
        /// <summary>
        /// Registers the bundles.
        /// </summary>
        public static void RegisterBundles()
        {
            // Add @Styles.Render("~/Content/bootstrap/base") in the <head/> of your _Layout.cshtml view
            // For Bootstrap theme add @Styles.Render("~/Content/bootstrap/theme") in the <head/> of your _Layout.cshtml view
            // Add @Scripts.Render("~/bundles/bootstrap") after jQuery in your _Layout.cshtml view
            // When <compilation debug="true" />, MVC4 will render the full readable version. When set to <compilation debug="false" />, the minified version will be rendered automatically
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.js"));
            BundleTable.Bundles.Add(
                new StyleBundle("~/Content/bootstrap/base").Include("~/Content/bootstrap/bootstrap.css"));
            BundleTable.Bundles.Add(
                new StyleBundle("~/Content/bootstrap/theme").Include("~/Content/bootstrap/bootstrap-theme.css"));

            #region Foundation Bundles

            //If your project requires jQuery, you may remove the zepto bundle
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/zepto").Include(
                "~/Scripts/zepto.js"));

        
            BundleTable.Bundles.Add(new StyleBundle("~/Content/foundation/css").Include(
                "~/Content/foundation/foundation.css",
                "~/Content/foundation/foundation.mvc.css",
                "~/Content/foundation/app.css"));

            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/foundation").Include(
                "~/Scripts/foundation/foundation.js",
                "~/Scripts/foundation/foundation.*",
                "~/Scripts/foundation/app.js"));

            #endregion


            // CSS style (bootstrap/inspinia)
            BundleTable.Bundles.Add(new StyleBundle("~/Content/css").Include(
                       "~/Content/bootstrap.min.css",
                       "~/Content/animate.css",
                       "~/Content/style.css",
                       "~/Content/video-css/reset.css",
                       "~/Content/video-css/style.css"));
            // CSS style (bootstrap/inspinia)
            BundleTable.Bundles.Add(new StyleBundle("~/Content/campaigncss").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/animate.css",
                       "~/Content/style_campaign.css"));


            // Font Awesome icons
            BundleTable.Bundles.Add(new StyleBundle("~/font-awesome/css").Include(
                      "~/fonts/font-awesome/css/font-awesome.min.css", new CssRewriteUrlTransform()));

            // jQuery
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
			//"~/Scripts/jquery.min.js",   // datepicker and dropdown new changes - 17-05-2019
            "~/Scripts/jquery-1.10.2.js",
            //"~/Scripts/jquery-2.1.1.min.js",
            "~/Scripts/modernizr-2.7.2.js",
            "~/Scripts/video-js/main.js"));
            //juqery migrate
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/jquery-migrate").Include(
                      "~/Scripts/plugins/jQuerymigrate/jquery-migrate-1.2.1.min.js"));
            //juqery wow
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/jquery-wow").Include(
                    "~/Scripts/plugins/wow/wow.min.js"));

            // jQueryUI CSS
            BundleTable.Bundles.Add(new ScriptBundle("~/Scripts/plugins/jquery-ui/jqueryuiStyles").Include(
                        "~/Scripts/plugins/jquery-ui/jquery-ui.css"));

            // jQueryUI CSS
            BundleTable.Bundles.Add(new ScriptBundle("~/Scripts/plugins/jquery-ui/fancyboxStyles").Include(
                        "~/Scripts/plugins/fancybox/source/jquery.fancybox.css"));

            // jQueryUI 
            BundleTable.Bundles.Add(new StyleBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/plugins/jquery-ui/jquery-ui.min.js"));

            BundleTable.Bundles.Add(new StyleBundle("~/bundles/fancyboxjs").Include(
                        "~/Scripts/plugins/fancybox/source/jquery.fancybox.pack.js"));

            // Bootstrap
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js"));

            // Inspinia script
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/inspinia").Include(
                      "~/Scripts/plugins/metisMenu/metisMenu.min.js",
                      "~/Scripts/plugins/pace/pace.min.js" //,
                      //"~/Scripts/app/inspinia.js" // datepicker and dropdown new changes - 17-05-2019
                      ));

            // Common script
            BundleTable.Bundles.Add(new ScriptBundle("~/Scripts/Common").Include(
                      "~/Scripts/Common.js"));

            // CommonAdmin script
            BundleTable.Bundles.Add(new ScriptBundle("~/Scripts/Commonadmin").Include(
                      "~/Scripts/Commonadmin.js"));

            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/skinConfig").Include(
                  "~/Scripts/app/skin.config.min.js"));

            // SlimScroll
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/slimScroll").Include(
                      "~/Scripts/plugins/slimscroll/jquery.slimscroll.min.js"));

            // Peity
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/peity").Include(
                      "~/Scripts/plugins/peity/jquery.peity.min.js"));

            // Video responsible
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/videoResponsible").Include(
                      "~/Scripts/plugins/video/responsible-video.js"));

            // Lightbox gallery css styles
            BundleTable.Bundles.Add(new StyleBundle("~/Content/plugins/blueimp/css/").Include(
                      "~/Content/plugins/blueimp/css/blueimp-gallery.min.css"));

            // Lightbox gallery
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/lightboxGallery").Include(
                      "~/Scripts/plugins/blueimp/jquery.blueimp-gallery.min.js"));

            // Sparkline
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/sparkline").Include(
                      "~/Scripts/plugins/sparkline/jquery.sparkline.min.js"));

            // Morriss chart css styles
            BundleTable.Bundles.Add(new StyleBundle("~/plugins/morrisStyles").Include(
                      "~/Content/plugins/morris/morris-0.4.3.min.css"));

            // Morriss chart
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/morris").Include(
                      "~/Scripts/plugins/morris/raphael-2.1.0.min.js",
                      "~/Scripts/plugins/morris/morris.js"));

            // Flot chart
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/flot").Include(
                      "~/Scripts/plugins/flot/jquery.flot.js",
                      "~/Scripts/plugins/flot/jquery.flot.tooltip.min.js",
                      "~/Scripts/plugins/flot/jquery.flot.resize.js",
                      "~/Scripts/plugins/flot/jquery.flot.pie.js",
                      "~/Scripts/plugins/flot/jquery.flot.time.js",
                      "~/Scripts/plugins/flot/jquery.flot.spline.js"));

            // Rickshaw chart
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/rickshaw").Include(
                      "~/Scripts/plugins/rickshaw/vendor/d3.v3.js",
                      "~/Scripts/plugins/rickshaw/rickshaw.min.js"));

            // ChartJS chart
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/chartJs").Include(
                      "~/Scripts/plugins/chartjs/Chart.min.js"));

            // iCheck css styles
            BundleTable.Bundles.Add(new StyleBundle("~/Content/plugins/iCheck/custom").Include(
                      "~/Content/plugins/iCheck/custom.css"));

            // iCheck
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/iCheck").Include(
                      "~/Scripts/plugins/iCheck/icheck.min.js"));

            // dataTables css styles
            BundleTable.Bundles.Add(new StyleBundle("~/Content/plugins/dataTables/dataTablesStyles").Include(
                      "~/Content/plugins/dataTables/datatables.min.css"));
            // accordion css styles
            BundleTable.Bundles.Add(new StyleBundle("~/Content/accordion").Include(
                      "~/Content/accordion.css"));
            // dataTables 
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/dataTables").Include(
                      //"~/Scripts/plugins/dataTables/datatables.js",
                      //"~/Scripts/jquery.js",
                      "~/Scripts/plugins/dataTables/datatables.min.js"));

            // jeditable 
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/jeditable").Include(
                      "~/Scripts/plugins/jeditable/jquery.jeditable.js"));

            // jqGrid styles
            BundleTable.Bundles.Add(new StyleBundle("~/Content/plugins/jqGrid/jqGridStyles").Include(
                      "~/Content/plugins/jqGrid/ui.jqgrid.css"));

            // jqGrid 
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/jqGrid").Include(
                      "~/Scripts/plugins/jqGrid/i18n/grid.locale-en.js",
                      "~/Scripts/plugins/jqGrid/jquery.jqGrid.min.js"));

            // codeEditor styles
            BundleTable.Bundles.Add(new StyleBundle("~/plugins/codeEditorStyles").Include(
                      "~/Content/plugins/codemirror/codemirror.css",
                      "~/Content/plugins/codemirror/ambiance.css"));

            // codeEditor 
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/codeEditor").Include(
                      "~/Scripts/plugins/codemirror/codemirror.js",
                      "~/Scripts/plugins/codemirror/mode/javascript/javascript.js"));

            // codeEditor 
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/nestable").Include(
                      "~/Scripts/plugins/nestable/jquery.nestable.js"));

            // validate 
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/validate").Include(
                      "~/Scripts/plugins/validate/jquery.validate.min.js"));

            // validate 
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/unobtrusive").Include(
                      "~/Scripts/jquery.unobtrusive-ajax.min.js"));
            //validate unobtrusive
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/validateunobtrusive").Include("~/Scripts/jquery.validate.unobtrusive.min.js"));

            // fullCalendar styles
            BundleTable.Bundles.Add(new StyleBundle("~/plugins/fullCalendarStyles").Include(
                      "~/Content/plugins/fullcalendar/fullcalendar.css"));

            // fullCalendar 
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/fullCalendar").Include(
                      "~/Scripts/plugins/fullcalendar/moment.min.js",
                      "~/Scripts/plugins/fullcalendar/fullcalendar.min.js"));

            // vectorMap 
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/vectorMap").Include(
                      "~/Scripts/plugins/jvectormap/jquery-jvectormap-1.2.2.min.js",
                      "~/Scripts/plugins/jvectormap/jquery-jvectormap-world-mill-en.js"));

            // ionRange styles
            BundleTable.Bundles.Add(new StyleBundle("~/Content/plugins/ionRangeSlider/ionRangeStyles").Include(
                      "~/Content/plugins/ionRangeSlider/ion.rangeSlider.css",
                      "~/Content/plugins/ionRangeSlider/ion.rangeSlider.skinFlat.css"));

            // ionRange 
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/ionRange").Include(
                      "~/Scripts/plugins/ionRangeSlider/ion.rangeSlider.min.js"));

            // dataPicker styles
            BundleTable.Bundles.Add(new StyleBundle("~/plugins/dataPickerStyles").Include(
                      "~/Content/plugins/datapicker/datepicker3.css"));

            // dataPicker 
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/dataPicker").Include(
                      "~/Scripts/plugins/datapicker/bootstrap-datepicker.js"));

            // nouiSlider styles
            BundleTable.Bundles.Add(new StyleBundle("~/plugins/nouiSliderStyles").Include(
                      "~/Content/plugins/nouslider/jquery.nouislider.css"));

            // nouiSlider 
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/nouiSlider").Include(
                      "~/Scripts/plugins/nouslider/jquery.nouislider.min.js"));

            // jasnyBootstrap styles
            BundleTable.Bundles.Add(new StyleBundle("~/plugins/jasnyBootstrapStyles").Include(
                      "~/Content/plugins/jasny/jasny-bootstrap.min.css"));

            // jasnyBootstrap 
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/jasnyBootstrap").Include(
                      "~/Scripts/plugins/jasny/jasny-bootstrap.min.js"));

            // switchery styles
            BundleTable.Bundles.Add(new StyleBundle("~/plugins/switcheryStyles").Include(
                      "~/Content/plugins/switchery/switchery.css"));

            // switchery 
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/switchery").Include(
                      "~/Scripts/plugins/switchery/switchery.js"));

            // chosen styles
            BundleTable.Bundles.Add(new StyleBundle("~/Content/plugins/chosen/chosenStyles").Include(
                      "~/Content/plugins/chosen/chosen.css"));

            // chosen 
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/chosen").Include(
                      "~/Scripts/plugins/chosen/chosen.jquery.js"));

            // knob 
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/knob").Include(
                      "~/Scripts/plugins/jsKnob/jquery.knob.js"));

            // wizardSteps styles
            BundleTable.Bundles.Add(new StyleBundle("~/plugins/wizardStepsStyles").Include(
                      "~/Content/plugins/steps/jquery.steps.css"));

            // wizardSteps 
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/wizardSteps").Include(
                      "~/Scripts/plugins/staps/jquery.steps.min.js"));

            // dropZone styles
            BundleTable.Bundles.Add(new StyleBundle("~/Content/plugins/dropzone/dropZoneStyles").Include(
                      "~/Content/plugins/dropzone/basic.css",
                      "~/Content/plugins/dropzone/dropzone.css"));

            // dropZone 
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/dropZone").Include(
                      "~/Scripts/plugins/dropzone/dropzone.js"));

            // summernote styles
            BundleTable.Bundles.Add(new StyleBundle("~/plugins/summernoteStyles").Include(
                      "~/Content/plugins/summernote/summernote.css",
                      "~/Content/plugins/summernote/summernote-bs3.css"));

            // summernote 
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/summernote").Include(
                      "~/Scripts/plugins/summernote/summernote.min.js"));

            // toastr notification 
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/toastr").Include(
                      "~/Scripts/plugins/toastr/toastr.min.js"));

            // toastr notification styles
            BundleTable.Bundles.Add(new StyleBundle("~/plugins/toastrStyles").Include(
                      "~/Content/plugins/toastr/toastr.min.css"));

            // color picker
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/colorpicker").Include(
                      "~/Scripts/plugins/colorpicker/bootstrap-colorpicker.min.js"));

            // color picker styles
            BundleTable.Bundles.Add(new StyleBundle("~/Content/plugins/colorpicker/colorpickerStyles").Include(
                      "~/Content/plugins/colorpicker/bootstrap-colorpicker.min.css"));

            // image cropper
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/imagecropper").Include(
                      "~/Scripts/plugins/cropper/cropper.min.js"));

            // image cropper styles
            BundleTable.Bundles.Add(new StyleBundle("~/plugins/imagecropperStyles").Include(
                      "~/Content/plugins/cropper/cropper.min.css"));

            // jsTree
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/jsTree").Include(
                      "~/Scripts/plugins/jsTree/jstree.min.js"));

            // jsTree styles
            BundleTable.Bundles.Add(new StyleBundle("~/Content/plugins/jsTree").Include(
                      "~/Content/plugins/jsTree/style.css"));

            // Diff
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/diff").Include(
                      "~/Scripts/plugins/diff_match_patch/javascript/diff_match_patch.js",
                      "~/Scripts/plugins/preetyTextDiff/jquery.pretty-text-diff.min.js"));

            // Idle timer
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/idletimer").Include(
                      "~/Scripts/plugins/idle-timer/idle-timer.min.js"));

            // Tinycon
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/tinycon").Include(
                      "~/Scripts/plugins/tinycon/tinycon.min.js"));

            // Chartist
            BundleTable.Bundles.Add(new StyleBundle("~/plugins/chartistStyles").Include(
                      "~/Content/plugins/chartist/chartist.min.css"));

            // jsTree styles
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/chartist").Include(
                      "~/Scripts/plugins/chartist/chartist.min.js"));

            // Awesome bootstrap checkbox
            BundleTable.Bundles.Add(new StyleBundle("~/plugins/awesomeCheckboxStyles").Include(
                      "~/Content/plugins/awesome-bootstrap-checkbox/awesome-bootstrap-checkbox.css"));

            // Clockpicker styles
            BundleTable.Bundles.Add(new StyleBundle("~/plugins/clockpickerStyles").Include(
                      "~/Content/plugins/clockpicker/clockpicker.css"));

            // Clockpicker
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/clockpicker").Include(
                      "~/Scripts/plugins/clockpicker/clockpicker.js"));

            // Date range picker Styless
            BundleTable.Bundles.Add(new StyleBundle("~/plugins/dateRangeStyles").Include(
                      "~/Content/plugins/daterangepicker/daterangepicker-bs3.css"));

            // Date range picker
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/dateRange").Include(
                      // Date range use moment.js same as full calendar plugin 
                      "~/Scripts/plugins/fullcalendar/moment.min.js",
                      "~/Scripts/plugins/daterangepicker/daterangepicker.js"));


            // Sweet alert Styless
            BundleTable.Bundles.Add(new StyleBundle("~/plugins/sweetAlertStyles").Include(
                      "~/Content/plugins/sweetalert/sweetalert.css"));

            // Sweet alert
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/sweetAlert").Include(
                      "~/Scripts/plugins/sweetalert/sweetalert.min.js"));

            // Footable Styless
            BundleTable.Bundles.Add(new StyleBundle("~/plugins/footableStyles").Include(
                      "~/Content/plugins/footable/footable.core.css", new CssRewriteUrlTransform()));

            // Footable alert
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/footable").Include(
                      "~/Scripts/plugins/footable/footable.all.min.js"));

            // Select2 Styless
            BundleTable.Bundles.Add(new StyleBundle("~/plugins/select2Styles").Include(
                      "~/Content/plugins/select2/select2.min.css"));

            // Select2
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/select2").Include(
                      "~/Scripts/plugins/select2/select2.full.min.js"));

            // Masonry
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/masonry").Include(
                      "~/Scripts/plugins/masonary/masonry.pkgd.min.js"));

            // Slick carousel Styless
            BundleTable.Bundles.Add(new StyleBundle("~/plugins/slickStyles").Include(
                      "~/Content/plugins/slick/slick.css", new CssRewriteUrlTransform()));

            // Slick carousel theme Styless
            BundleTable.Bundles.Add(new StyleBundle("~/plugins/slickThemeStyles").Include(
                      "~/Content/plugins/slick/slick-theme.css", new CssRewriteUrlTransform()));

            // Slick carousel
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/slick").Include(
                      "~/Scripts/plugins/slick/slick.min.js"));

            // Ladda buttons Styless
            BundleTable.Bundles.Add(new StyleBundle("~/plugins/laddaStyles").Include(
                      "~/Content/plugins/ladda/ladda-themeless.min.css"));

            // Ladda buttons
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/ladda").Include(
                      "~/Scripts/plugins/ladda/spin.min.js",
                      "~/Scripts/plugins/ladda/ladda.min.js",
                      "~/Scripts/plugins/ladda/ladda.jquery.min.js"));

            // Dotdotdot buttons
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/truncate").Include(
                      "~/Scripts/plugins/dotdotdot/jquery.dotdotdot.min.js"));

            // Touch Spin Styless
            BundleTable.Bundles.Add(new StyleBundle("~/plugins/touchSpinStyles").Include(
                      "~/Content/plugins/touchspin/jquery.bootstrap-touchspin.min.css"));

            // Touch Spin
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/touchSpin").Include(
                      "~/Scripts/plugins/touchspin/jquery.bootstrap-touchspin.min.js"));

            // Tour Styless
            BundleTable.Bundles.Add(new StyleBundle("~/plugins/tourStyles").Include(
                      "~/Content/plugins/bootstrapTour/bootstrap-tour.min.css"));

            // Tour Spin
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/tour").Include(
                      "~/Scripts/plugins/bootstrapTour/bootstrap-tour.min.js"));

            // i18next Spin
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/i18next").Include(
                      "~/Scripts/plugins/i18next/i18next.min.js"));

            // Clipboard Spin
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/clipboard").Include(
                      "~/Scripts/plugins/clipboard/clipboard.min.js"));

            // c3 Styless
            BundleTable.Bundles.Add(new StyleBundle("~/plugins/c3Styles").Include(
                      "~/Content/plugins/c3/c3.min.css"));

            // c3 Spin
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/c3").Include(
                      "~/Scripts/plugins/c3/c3.min.js"));

            // d3 Spin
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/d3").Include(
                      "~/Scripts/plugins/d3/d3.min.js"));

            // Markdown Styless
            BundleTable.Bundles.Add(new StyleBundle("~/plugins/markdownStyles").Include(
                      "~/Content/plugins/bootstrap-markdown/bootstrap-markdown.min.css"));

            // Markdown Spin
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/markdown").Include(
                      "~/Scripts/plugins/bootstrap-markdown/bootstrap-markdown.js",
                      "~/Scripts/plugins/bootstrap-markdown/markdown.js"));

            BundleTable.Bundles.Add(new ScriptBundle("~/Scripts/tinymce/tinymcejs").Include(
                            "~/Scripts/tinymce/tinymce.min.js"));

            // Date range picker New
            //BundleTable.Bundles.Add(new StyleBundle("~/plugins/daterangepickerNew").Include(
            //  "~/Content/plugins/daterangepickerNew/bootstrap.min.css",
            //  "~/Content/plugins/daterangepickerNew/bootstrap-datetimepicker.min.css"));

            BundleTable.Bundles.Add(new StyleBundle("~/plugins/daterangepickerNewStylesforusers").Include(
              "~/Content/plugins/daterangepickerNewStyles/bootstrap-datetimepicker.min.css"));

            BundleTable.Bundles.Add(new StyleBundle("~/plugins/daterangepickerNewStyles").Include(
               "~/Content/plugins/daterangepickerNewStyles/bootstrap.min.css",
              "~/Content/plugins/daterangepickerNewStyles/bootstrap-datetimepicker.min.css"));

            // Date range picker New
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/daterangepickerNew").Include(
            "~/Scripts/plugins/daterangepickerNew/bootstrap.min.js",
            "~/Scripts/plugins/daterangepickerNew/moment.min.js",
            // datepicker and dropdown new changes - 17-05-2019
            "~/Scripts/plugins/daterangepickerNew/bootstrap-datetimepicker.min.js"
             ));

            // Data table responsive css
            BundleTable.Bundles.Add(new StyleBundle("~/plugins/dataTablesNew").Include(
            "~/Content/plugins/dataTablesNew/datatables.min.css"));

            // Data table responsive JS.
            BundleTable.Bundles.Add(new ScriptBundle("~/plugins/datatableNew").Include(
            "~/Scripts/plugins/datatableNew/datatables.min.js"));

            BundleTable.EnableOptimizations = false;
        }
    }
}