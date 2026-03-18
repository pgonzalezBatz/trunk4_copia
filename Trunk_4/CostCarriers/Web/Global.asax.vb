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

        RouteConfig.RegisterRoutes(RouteTable.Routes)
        BundleConfig.RegisterBundles(BundleTable.Bundles)
        BundleTable.EnableOptimizations = True

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

        ' Si aquí devuelve el identity a vacio pueden ser dos cosas
        ' - En el web config tiene que estar  <authentication mode="Windows"/>
        ' - En propiedades del proyecto en la pestaña web hay que utilizar el servidor de desarrollo del visual studio
#If DEBUG Then
        myTicket = lg.Login(User.Identity.Name.ToLower)
        'myTicket = lg.Login("batznt\ibeascoechea")
#Else
        myTicket = lg.Login(User.Identity.Name.ToLower)
#End If
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
    End Sub

End Class
