Public Class listadoController
    Inherits System.Web.Mvc.Controller

    Private strCnMicrosof As String = ConfigurationManager.ConnectionStrings("microsoft").ConnectionString
    Private strCnOracle As String = ConfigurationManager.ConnectionStrings("oracle").ConnectionString

    <SimpleRoleProvider(Role.responsable, Role.rrhh)> _
    Function Index(ejercicio As Nullable(Of Integer)) As ActionResult
        If ejercicio.HasValue Then
            ViewData("ejercicioactual") = ejercicio.Value
        Else
            ViewData("ejercicioactual") = Now.Year
        End If
        If SimpleRoleProvider.IsUserAuthorised(Role.rrhh) AndAlso Request("idsab") IsNot Nothing Then
            ViewData("usuario") = db.GetUsuarioSab(Request("idsab"), strCnOracle)
        Else
            ViewData("usuario") = db.GetUsuarioSab(SimpleRoleProvider.GetId(), strCnOracle)
        End If

        Return View()
    End Function

End Class