Imports System.Web.Optimization

Public Module BundleConfig
    ' Para obtener más información sobre Bundles, visite http://go.microsoft.com/fwlink/?LinkId=301862
    Public Sub RegisterBundles(ByVal bundles As BundleCollection)
        bundles.Add(New ScriptBundle("~/bundles/js").Include("~/Scripts/jquery-3.4.1.min.js",
                                                             "~/Scripts/jquery-ui-1.12.1.min.js",
                                                             "~/Scripts/bootstrap.min.js",
                                                             "~/Scripts/moment-with-locales.js",
                                                             "~/Scripts/bootstrap-datetimepicker.js",
                                                             "~/Scripts/jquery.validate.js"))
        bundles.Add(New StyleBundle("~/bundles/css").Include("~/Content/style.css",
                                                             "~/Content/bootstrap.css",
                                                             "~/Content/bootstrap-datetimepicker.css"))
    End Sub
End Module