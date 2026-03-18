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
    ''' </summary>    
    Function Salir() As RedirectResult
        Dim usuariosBLL As New SabLib.BLL.UsuariosComponent()
        Dim usuario As SabLib.ELL.Usuario = usuariosBLL.GetUsuario(New SabLib.ELL.Usuario With {.IdDirectorioActivo = User.Identity.Name.ToLower()})
        CambiarUsuario(usuario.Id)
        Dim url As String = String.Format("/langileentxokoa/menuEmpleado.aspx?id={0}", Ticket.IdSession)
        Try
            Dim loginCompBLL As New SabLib.BLL.LoginComponent
            loginCompBLL.SetTicketEnBD(New SabLib.ELL.Ticket With {.IdSession = Ticket.IdSession, .IdUser = Ticket.IdUser})
        Catch ex As Exception
        End Try

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
            Return RedirectToAction("Index", "Solicitud")
        End If

        Return View()
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function BuscarUsuarios(ByVal q As String) As JsonResult
        Dim usuariosBLL As New SabLib.BLL.UsuariosComponent
        Dim plantasBLL As New SabLib.BLL.PlantasComponent
        Dim usuarios = (From users In usuariosBLL.GetUsuariosBusquedaSAB_Optimizado(q) Where ((users.FechaBaja = DateTime.MinValue OrElse users.FechaBaja > DateTime.Today) AndAlso Not String.IsNullOrEmpty(users.IdDirectorioActivo)) Select New With {.Id = users.Id, .NombreCompleto = users.NombreCompleto, .NombreCompletoYPlanta = users.NombreCompletoYPlanta, .IdPlanta = users.IdPlanta, .Email = users.Email}).ToList()

        Return Json(usuarios, JsonRequestBehavior.AllowGet)
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
        Dim usuario As SabLib.ELL.Usuario = usuariosBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = CInt(hfUsuario)})

        If (usuario Is Nothing) Then
            Return View()
        End If

        myTicket = lg.Login(usuario.IdDirectorioActivo)

        Session("Ticket") = Nothing
        If myTicket IsNot Nothing Then
            If lg.AccesoRecursoValido(myTicket, recurso) Then
                Response.Cookies("TarjetaVisCulture").Value = myTicket.Culture
                Response.Cookies("TarjetaVisCulture").Expires = DateTime.MaxValue

                'Como de primeras coge la cultura del servidor le forzamos a la cultura del ticket
                Threading.Thread.CurrentThread.CurrentCulture = Globalization.CultureInfo.GetCultureInfo(myTicket.Culture)
                Threading.Thread.CurrentThread.CurrentUICulture = Globalization.CultureInfo.GetCultureInfo(myTicket.Culture)

                Session("Ticket") = myTicket

                Dim usuariosRoles As List(Of TarjetasVisitaLib.ELL.UsuarioRol) = TarjetasVisitaLib.BLL.UsuariosRolBLL.CargarListado(myTicket.IdUser)

                Session("RolesUsuario") = usuariosRoles
            Else
                Response.Redirect("~/Login/AccesoDenegado")
            End If
        Else
            Response.Redirect("~/Login/AccesoDenegado")
        End If

        Return RedirectToAction("MisSolicitudes", "Solicitud")
    End Function

#End Region

End Class