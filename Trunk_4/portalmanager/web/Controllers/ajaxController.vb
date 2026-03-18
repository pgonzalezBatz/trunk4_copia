Namespace web
    Public Class ajaxController
        Inherits System.Web.Mvc.Controller

        Private strCnMicrosof As String = ConfigurationManager.ConnectionStrings("microsoft").ConnectionString
        Private strCnOracle As String = ConfigurationManager.ConnectionStrings("oracle").ConnectionString

        Function help() As JsonResult
            Dim lst As New List(Of String)
            For Each e In Me.GetType.GetMethods(Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public).ToList.FindAll(Function(mi As Reflection.MethodInfo) mi.DeclaringType.Name = "ajaxController")
                lst.Add(e.Name)
            Next
            Return Json(lst, JsonRequestBehavior.AllowGet)
        End Function

        Function GetListOfDepartamento(idNegocio As String) As JsonResult
            Return Json(db.GetListOfDepartamento(idNegocio, strCnMicrosof), JsonRequestBehavior.AllowGet)
        End Function

        Function GetListOfUsuario(term As String) As JsonResult
            Return Json(db.searchUsuario(term, strCnOracle), JsonRequestBehavior.AllowGet)
        End Function
        
    End Class
End Namespace