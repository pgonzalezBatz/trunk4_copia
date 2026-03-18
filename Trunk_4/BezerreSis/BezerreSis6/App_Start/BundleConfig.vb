Imports System.Web.Optimization

Public Module BundleConfig
    ' Para obtener más información sobre Bundles, visite http://go.microsoft.com/fwlink/?LinkId=301862
    Public Sub RegisterBundles(ByVal bundles As BundleCollection)
        bundles.Add(New ScriptBundle("~/bundles/jquery").Include(
                    "~/Scripts/jquery-{version}.js",
                    "~/Scripts/jquery-ui-{version}.js",
                    "~/Scripts/jquery-ui-i18n.min.js",
                    "~/Scripts/gridmvc.min.js",
                    "~/Scripts/Batz.js"))
        bundles.Add(New ScriptBundle("~/bundles/jqueryval").Include(
                    "~/Scripts/jquery.validate*"))

        bundles.Add(New ScriptBundle("~/bundles/filters").Include(
                    "~/Scripts/multiSelect-dropdown.js"))
        ' Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
        ' preparado para la producción y podrá utilizar la herramienta de compilación disponible en http://modernizr.com para seleccionar solo las pruebas que necesite.
        bundles.Add(New ScriptBundle("~/bundles/modernizr").Include(
                    "~/Scripts/modernizr-*"))
        bundles.Add(New ScriptBundle("~/bundles/bootstrap").Include(
                  "~/Scripts/bootstrap.js",
                  "~/Scripts/bootstrap-multiselect.js",
                  "~/Scripts/respond.js"))
        '"~/Scripts/bootstrap.min.js",
        bundles.Add(New StyleBundle("~/Content/css").Include( ' no se usan los .less aunque estén, se usa directamente el archivo bootstrap.css que no deriva de los less * el bootstrap breakpoint se ha modificado en el site.css
                  "~/Content/bootstrap.css",
                  "~/Content/bootstrap-multiselect.css",
                  "~/Content/themes/base/all.css",
                  "~/Content/Site.css",
                  "~/Content/Gridmvc.css",
                  "~/Content/Hobekuntza.css"))
        '--------------------------------------------------------------------------
        bundles.Add(New ScriptBundle("~/bundles/bootstrap-datetimepicker").Include(
                    "~/Scripts/moment-with-locales.min.js",
                    "~/Scripts/bootstrap-datetimepicker.min.js"))

        bundles.Add(New StyleBundle("~/Content/bootstrap-datetimepicker").Include(
                    "~/Content/bootstrap-datetimepicker.css"))
    End Sub
End Module

