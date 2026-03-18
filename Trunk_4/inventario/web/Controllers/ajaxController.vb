Namespace web
    Public Class AjaxController
        Inherits System.Web.Mvc.Controller

        Private ReadOnly strCnEpsilon As String = ConfigurationManager.ConnectionStrings("epsilon").ConnectionString
        Private ReadOnly strCn As String = ConfigurationManager.ConnectionStrings("inventario").ConnectionString
        Function Help() As JsonResult
            Dim lst As New List(Of String)
            For Each e In Me.GetType.GetMethods(Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public).ToList.FindAll(Function(mi As Reflection.MethodInfo) mi.DeclaringType.Name = "ajaxController")
                lst.Add(e.Name)
            Next
            Return Json(lst, JsonRequestBehavior.AllowGet)
        End Function

        Function Buscar(term As String) As JsonResult
            Return Json(DB.SearchUsuario(term, strCn), JsonRequestBehavior.AllowGet)
        End Function
        Function BuscarBaja(term As String) As JsonResult
            Return Json(DB.SearchUsuarioBaja(term, strCn), JsonRequestBehavior.AllowGet)
        End Function
        Function GetUser(idsab As Integer) As JsonResult
            Return Json(DB.GetUsuario(idsab, strCn, strCnEpsilon), JsonRequestBehavior.AllowGet)
        End Function
        Function Elementosactivos(idsab As Integer) As JsonResult
            Return Json(DB.GetListOfEtiquetasUsuario(idsab, strCn), JsonRequestBehavior.AllowGet)
        End Function
    End Class
End Namespace