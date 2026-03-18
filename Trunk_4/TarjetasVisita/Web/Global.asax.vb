Imports System.Web.Mvc
Imports System.Web.Routing
Imports System.Web.Optimization

Public Class MvcApplication
    Inherits System.Web.HttpApplication

    ''' <summary>
    ''' 
    ''' </summary>
    Protected Sub Application_Start()
        AreaRegistration.RegisterAllAreas()
        BundleConfig.RegisterBundles(BundleTable.Bundles)
        RouteConfig.RegisterRoutes(RouteTable.Routes)

        log4net.Config.XmlConfigurator.Configure(New IO.FileInfo(Server.MapPath("~") & "\App_Data\log4netConfig.config"))
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        System.Net.ServicePointManager.Expect100Continue = False
        System.Net.ServicePointManager.SecurityProtocol = CType(3072, System.Net.SecurityProtocolType)
        Dim log As log4net.ILog = log4net.LogManager.GetLogger("root")

        ' Eliminamos los elementos de la caché
        For Each element In Runtime.Caching.MemoryCache.Default
            Runtime.Caching.MemoryCache.Default.Remove(element.Key)
        Next

        Dim myTicket As SabLib.ELL.Ticket
        Dim recurso As String = System.Configuration.ConfigurationManager.AppSettings.Get("RecursoWeb")
        Dim lg As New SabLib.BLL.LoginComponent

        ' Para probar como si fuera un login desde el portal del empleado hay que meter un ticket a mano en la tabla 
        ' TICKETS de SAB
#If Not DEBUG Then
        Dim idsession As String = Request.QueryString("id")
        SaveTicket(idsession)
        myTicket = CType(Session("Ticket"), SabLib.ELL.Ticket)
#Else
        myTicket = lg.Login("batznt\retxebarrena")
#End If
        Session("Ticket") = Nothing
        If myTicket IsNot Nothing Then
#If Not DEBUG Then
            If lg.AccesoRecursoValido(myTicket, recurso) Then
#End If
            Response.Cookies("TarjetaVisCulture").Value = myTicket.Culture
            Response.Cookies("TarjetaVisCulture").Expires = DateTime.MaxValue

            'Como de primeras coge la cultura del servidor le forzamos a la cultura del ticket
            Threading.Thread.CurrentThread.CurrentCulture = Globalization.CultureInfo.GetCultureInfo(myTicket.Culture)
            Threading.Thread.CurrentThread.CurrentUICulture = Globalization.CultureInfo.GetCultureInfo(myTicket.Culture)

            Session("Ticket") = myTicket

            Dim usuariosRoles As List(Of TarjetasVisitaLib.ELL.UsuarioRol) = TarjetasVisitaLib.BLL.UsuariosRolBLL.CargarListado(myTicket.IdUser)

            Session("RolesUsuario") = usuariosRoles
#If Not DEBUG Then
            Else
                Response.Redirect("~/Login/AccesoDenegado")
            End If
#End If
        Else
            Response.Redirect("~/Login/AccesoDenegado")
        End If
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Sub Application_BeginRequest()
        Dim culture As String = "es-ES"
        If (Request.Cookies("TarjetaVisCulture") IsNot Nothing) Then
            culture = CStr(Request.Cookies("TarjetaVisCulture").Value)
        End If

        Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(culture)
        Threading.Thread.CurrentThread.CurrentUICulture = New System.Globalization.CultureInfo(culture)
    End Sub

#Region "Métodos"

    ''' <summary>
    ''' Dado el idSession, obtiene el ticket
    ''' </summary>
    ''' <param name="idSession"></param>    
    Private Sub SaveTicket(ByVal idSession As String)
        Dim loginComp As New SabLib.BLL.LoginComponent
        Dim ticket As SabLib.ELL.Ticket
#If DEBUG Then
        ticket = loginComp.getTicket(idSession, False)
#Else
        ticket = loginComp.getTicket(idSession, true)
#End If
        Session("Ticket") = ticket
    End Sub

#End Region

End Class
