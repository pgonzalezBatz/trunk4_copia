Imports System.Security.Permissions
Namespace ExtranetMVC
    Public Class accessController
        Inherits System.Web.Mvc.Controller

        Private strCn As String = ConfigurationManager.ConnectionStrings("sab").ConnectionString
        Private idGrupo As String = ConfigurationManager.AppSettings("segipe")
        Function Index() As ActionResult
            Return RedirectToAction("listcabecera", "pedido", New With {.idestado = 1})
            Dim log As log4net.ILog = log4net.LogManager.GetLogger("root")
            'Tiene cookie de autenticacion?
            If User.Identity.IsAuthenticated Then
                Return RedirectToAction("listcabecera", "pedido", New With {.idestado = 1})
            End If
            'Asegurar que el usuario existe en SAB y tiene el recurso segipe
            Dim lst = db.GetUsuario(Request.LogonUserIdentity.Name.ToLower, idGrupo, strCn)
            If lst.Count = 0 Then
                log.Info("login fallido")
                Return Redirect("~/accesodenegado.htm")
            End If
            Dim idSab = lst(0)(0)
            'Session("idsab") = 56955 'Naia
            'Session("idsab") = 57941 'Josetxu
            'Session("idsab") = 57066 'Robeto Alonso
            'Session("idsab") = 57172 'Sonia Lanbarri

            log.Info("login")
            FormsAuthentication.SetAuthCookie(idSab, True)
            Return RedirectToAction("listcabecera", "pedido", New With {.idestado = 1})
        End Function
    End Class
End Namespace