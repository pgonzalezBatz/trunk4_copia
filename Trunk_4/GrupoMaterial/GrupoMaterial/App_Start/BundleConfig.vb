Imports System.Web.Optimization

Public Module BundleConfig
    Public Sub RegisterBundles(ByVal bundles As BundleCollection)
        BundleTable.EnableOptimizations = True ''''ASÍ NOS DAREMOS CUENTA SI HACE OK EL BUNDLING AL DEBUGUEAR!
        bundles.Add(New ScriptBundle("~/bundles/jquery").Include(
                    "~/Scripts/jquery-{version}.js",
                    "~/Scripts/jquery-ui-1.11.4.min.js",
                    "~/Scripts/jquery.validate.min.js",
                    "~/Scripts/jquery.tablesorter.js"))
        bundles.Add(New ScriptBundle("~/bundles/jqueryval").Include(
                    "~/Scripts/jquery.validate*"))
        bundles.Add(New ScriptBundle("~/bundles/modernizr").Include(
                    "~/Scripts/modernizr-*"))
        bundles.Add(New ScriptBundle("~/bundles/bootstrap").Include(
                  "~/Scripts/bootstrap.min.js",
                  "~/Scripts/respond.js"))
        'bundles.Add(New StyleBundle("~/Content/myCss").Include(
        '          "~/Content/bootstrap/3.3.7/css/bootstrap.min.css",
        '          "~/Content/themes/base/all.css",
        '          "~/Content/site.css",
        '          "~/Content/style.css"))
        'bundles.Add(New StyleBundle("~/Content/myCss").Include(
        '          "~/Content/bootstrap/3.3.7/css/bootstrap.min.css",
        '          "~/Content/themes/base/base.css",
        '          "~/Content/themes/base/theme.css",
        '          "~/Content/site.css",
        '          "~/Content/style.css"))

        bundles.Add(New StyleBundle("~/Content/myCss").Include(
                    "~/Content/bootstrap/3.3.7/css/bootstrap.min.css",
                    "~/Content/themes/base/core.css",
                    "~/Content/themes/base/accordion.css",
                    "~/Content/themes/base/autocomplete.css",
                    "~/Content/themes/base/button.css",
                    "~/Content/themes/base/datepicker.css",
                    "~/Content/themes/base/dialog.css",
                    "~/Content/themes/base/draggable.css",
                    "~/Content/themes/base/menu.css",
                    "~/Content/themes/base/progressbar.css",
                    "~/Content/themes/base/resizable.css",
                    "~/Content/themes/base/selectable.css",
                    "~/Content/themes/base/selectmenu.css",
                    "~/Content/themes/base/sortable.css",
                    "~/Content/themes/base/slider.css",
                    "~/Content/themes/base/spinner.css",
                    "~/Content/themes/base/tabs.css",
                    "~/Content/themes/base/tooltip.css",
                    "~/Content/themes/base/theme.css",
                    "~/Content/site.css",
                    "~/Content/style.css"))

    End Sub
End Module

