Imports System.Web.Mvc
Imports System.Web.Routing

Public Class MvcApplication
    Inherits System.Web.HttpApplication

    ''' <summary>
    ''' 
    ''' </summary>
    Protected Sub Application_Start()
        AreaRegistration.RegisterAllAreas()
        RouteConfig.RegisterRoutes(RouteTable.Routes)

        log4net.Config.XmlConfigurator.Configure(New IO.FileInfo(Server.MapPath("~") & "\App_Data\log4netConfig.config"))
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        Dim log As log4net.ILog = log4net.LogManager.GetLogger("root")

        ' Eliminamos los elementos de la caché
        For Each element In Runtime.Caching.MemoryCache.Default
            Runtime.Caching.MemoryCache.Default.Remove(element.Key)
        Next

        Dim myTicket As SabLib.ELL.Ticket
        Dim recurso As String = System.Configuration.ConfigurationManager.AppSettings.Get("RecursoWeb")
        Dim lg As New SabLib.BLL.LoginComponent

        ' Si aquí devuelve el identity a vacio pueden ser dos cosas
        ' - En el web config tiene que estar  <authentication mode="Windows"/>
        ' - En propiedades del proyecto en la pestaña web hay que utilizar el servidor de desarrollo del visual studio
#If DEBUG Then
        'myTicket = lg.Login(User.Identity.Name.ToLower)
        myTicket = lg.Login("czech\mriestra")
#Else
        myTicket = lg.Login(User.Identity.Name.ToLower)
#End If
        Session("Ticket") = Nothing
        If myTicket IsNot Nothing Then
            If lg.AccesoRecursoValido(myTicket, recurso) Then
                Response.Cookies("DOBCulture").Value = myTicket.Culture
                Response.Cookies("DOBCulture").Expires = DateTime.MaxValue

                'Como de primeras coge la cultura del servidor le forzamos a la cultura del ticket
                Threading.Thread.CurrentThread.CurrentCulture = Globalization.CultureInfo.GetCultureInfo(myTicket.Culture)
                Threading.Thread.CurrentThread.CurrentUICulture = Globalization.CultureInfo.GetCultureInfo(myTicket.Culture)

                Session("Ticket") = myTicket

                Dim usuariosRoles As List(Of ELL.UsuarioRol) = BLL.UsuariosRolBLL.CargarListado(myTicket.IdUser).Where(Function(f) (f.FechaBaja = DateTime.MinValue OrElse f.FechaBaja >= DateTime.Today) AndAlso f.FechaBajaDOB = DateTime.MinValue AndAlso f.FechaBajaDOB = DateTime.MinValue).ToList()
                If (usuariosRoles Is Nothing OrElse usuariosRoles.Count = 0) Then
                    Response.Redirect("~/Login/UsuarioSinRol")
                End If

                'Session("RolesUsuario") = usuariosRoles

                ' Activar esto para mostrar el selector de roles
                Dim numRoles As Integer = usuariosRoles.Where(Function(f) f.IdRol <> ELL.Rol.RolUsuario.Administrador).Count
                If (numRoles = 0) Then
                    ' Es sólo administrador
                    Session("RolActual") = usuariosRoles.First
                ElseIf (numRoles = 1) Then
                    ' Es sólo administrador o es administrador con un rol más
                    Session("RolActual") = usuariosRoles.Where(Function(f) f.IdRol <> ELL.Rol.RolUsuario.Administrador).First
                Else
                    ' Mostramos la pantalla para que seleccione un rol
                    Response.Redirect("~/Login/Index?seleccionarRol=true&cargaInicial=true")
                End If
            Else
                log.Info("No tiene acceso al recurso el usuario " & User.Identity.Name.ToLower)
                Response.Redirect("~/Login/AccesoDenegado")
            End If
        Else
            log.Info("El ticket es nothing para el usuario " & User.Identity.Name.ToLower)
            Response.Redirect("~/Login/AccesoDenegado")
        End If
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Sub Application_BeginRequest()
        Dim culture As String = "es-ES"
        If (Request.Cookies("DOBCulture") IsNot Nothing) Then
            culture = CStr(Request.Cookies("DOBCulture").Value)
        End If

        Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(culture)
        Threading.Thread.CurrentThread.CurrentUICulture = New System.Globalization.CultureInfo(culture)
    End Sub

End Class
