Imports System.Security.Permissions
Namespace ExtranetMVC
    Public Class accessController
        Inherits System.Web.Mvc.Controller


        Private strCn As String = ConfigurationManager.ConnectionStrings("inventario").ConnectionString
        Private idRecurso As Integer = ConfigurationManager.AppSettings("idrecurso")

        Function Index() As ActionResult
            Return RedirectToAction("index", "inventario", New With {.idEstado = 1})
        End Function
       
    End Class
End Namespace