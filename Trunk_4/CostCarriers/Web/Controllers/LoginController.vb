Imports System.Web.Mvc

Public Class LoginController
    Inherits BaseController

#Region "Métodos"

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function AccesoDenegado() As ActionResult
        Return View()
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function Salir() As ActionResult
        Dim url As String = "/homeintranet"

        Session("Ticket") = Nothing
        Session.Abandon()
        Session.Clear()
        Return Redirect(url)
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function CambiarUsuario() As ActionResult
        Dim directorioActivo As String = User.Identity.Name.ToLower()
        Dim usuariosCambio As List(Of String) = ConfigurationManager.AppSettings("UsuariosCambio").ToLower().Replace(" ", String.Empty).Split(",").ToList()

        If (Not usuariosCambio.Exists(Function(f) f = directorioActivo)) Then
            Return RedirectToAction("Index", "CostCarriers")
        End If

        Return View()
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <HttpPost>
    Function CambiarUsuario(ByVal hfUsuario As String) As ActionResult
        If (String.IsNullOrEmpty(hfUsuario)) Then
            MensajeError(Utils.Traducir("Debe seleccionar un usuario"))
            Return View()
        End If

        Dim myTicket As SabLib.ELL.Ticket
        Dim recurso As String = System.Configuration.ConfigurationManager.AppSettings.Get("RecursoWeb")
        Dim lg As New SabLib.BLL.LoginComponent

        Dim usuariosBLL As New SabLib.BLL.UsuariosComponent()
        Dim usuario As SabLib.ELL.Usuario = usuariosBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = CInt(hfUsuario)}, False)

        If (usuario Is Nothing) Then
            Return View()
        End If

        myTicket = lg.Login(usuario.IdDirectorioActivo, False)

        Session("Ticket") = Nothing
        If myTicket IsNot Nothing Then
            If lg.AccesoRecursoValido(myTicket, recurso) Then
                'Como de primeras coge la cultura del servidor le forzamos a ingés
                Threading.Thread.CurrentThread.CurrentCulture = Globalization.CultureInfo.GetCultureInfo("en-GB")
                Threading.Thread.CurrentThread.CurrentUICulture = Globalization.CultureInfo.GetCultureInfo("en-GB")

                Session("Ticket") = myTicket

                Dim usuariosRoles As List(Of ELL.UsuarioRol) = BLL.UsuariosRolBLL.CargarListado(myTicket.IdUser)

                Session("RolesUsuario") = usuariosRoles
            Else
                Response.Redirect("~/Login/AccesoDenegado")
            End If
        Else
            Response.Redirect("~/Login/AccesoDenegado")
        End If

        Return RedirectToAction("Index", "CostCarriers")
    End Function

#End Region

End Class