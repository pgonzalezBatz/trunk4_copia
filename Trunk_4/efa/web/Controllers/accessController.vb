Namespace efa
    Public Class accessController
        Inherits System.Web.Mvc.Controller

        Private strCn As String = ConfigurationManager.ConnectionStrings("SAB").ConnectionString
        Private Grupo As Integer = ConfigurationManager.AppSettings.Get("IDGRUPO")
        Private GrupoAdmin As Integer = ConfigurationManager.AppSettings.Get("GrupoAdmin")

        <SimpleRoleProvider(EfaRole.admin, EfaRole.jefegrupo, EfaRole.touch, EfaRole.usuarioTemporal)>
        Function Index() As ActionResult
            Select Case SimpleRoleProvider.GetRole()
                Case EfaRole.jefegrupo
                    Return RedirectToAction("Index", "reasign", Nothing)
                Case EfaRole.admin
                    Return RedirectToAction("Index", "admin", Nothing)
                Case EfaRole.touch
                    Return View()
                Case Else
                    Return RedirectToAction("accion", "recurso", Nothing)
            End Select
        End Function

        <SimpleRoleProvider(EfaRole.touch)>
        <AcceptVerbs(HttpVerbs.Post)> _
        Function Index(ByVal formValues As FormCollection) As ActionResult
            If formValues("numerotrabajador") Is Nothing OrElse formValues("numerotrabajador").Length = 0 Then
                ModelState.AddModelError("numerotrabajador", "Introduzca su numero de trabajador para entrar en la aplicación")
            End If
            If formValues("pwd") Is Nothing OrElse formValues("pwd").Length = 0 Then
                ModelState.AddModelError("pwd", "Introduzca su clave para entrar en la aplicación")
            End If
            If ModelState.IsValid Then
                'Asegurar que el usuario existe en SAB y tiene el recurso 
                Dim lst = DB.GetLoginUsuario(formValues("numerotrabajador"), formValues("pwd"), Grupo, strCn)
                If lst.Count = 0 Then
                    ModelState.AddModelError("pwd", "Clave incorrecta")
                    ViewData("numerotrabajador") = formValues("numerotrabajador")
                    Return View()
                End If
                Dim idSab = lst(0)
                Dim log As log4net.ILog = log4net.LogManager.GetLogger("root")
                log.Info("login id sab " + idSab.ToString + " , numero trabajador " + formValues("numerotrabajador") + " , " + Server.MachineName)

                h.SetCulture(idSab, ConfigurationManager.ConnectionStrings("SAB").ConnectionString)
                'FormsAuthentication.SetAuthCookie("38", False)
                SimpleRoleProvider.setAuthCookieWithRole(idSab, Function() EfaRole.usuarioTemporal)
                Return RedirectToAction("accion", "recurso", Nothing)
            End If
            ViewData("numerotrabajador") = formValues("numerotrabajador")
            Return View()
        End Function
    End Class
End Namespace