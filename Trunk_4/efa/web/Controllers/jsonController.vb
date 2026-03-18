Namespace efa
    Public Class jsonController
        Inherits System.Web.Mvc.Controller
        Private strCnSab As String = ConfigurationManager.ConnectionStrings("SAB").ConnectionString
        Function BuscarTrabajador(ByVal q As String) As JsonResult
            Return Json(DB.BuscarTrabajador(q, strCnSab), JsonRequestBehavior.AllowGet)
        End Function
    End Class
End Namespace