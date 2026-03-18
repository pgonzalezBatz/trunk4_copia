Public Class GuiaController
    Inherits System.Web.Mvc.Controller



    <SimpleRoleProvider(Role.responsable, Role.rrhh)>
    Function Index() As ActionResult
        Return View()
    End Function

End Class