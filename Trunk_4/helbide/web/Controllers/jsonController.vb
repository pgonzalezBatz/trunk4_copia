Namespace web
    Public Class jsonController
        Inherits System.Web.Mvc.Controller

        Private strCn As String = ConfigurationManager.ConnectionStrings("XBAT").ConnectionString

        Function Index() As ActionResult
            Return View()
        End Function

        Function Buscar(ByVal q As String) As JsonResult
            Return Json(DBAccess.Buscar(q, strCn), JsonRequestBehavior.AllowGet)
        End Function
        Function GetHelbide(ByVal id As Integer) As JsonResult
            Return Json(DBAccess.GetHelbide(id, strCn), JsonRequestBehavior.AllowGet)
        End Function
    End Class
End Namespace