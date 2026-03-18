Imports System.Web.Optimization
Imports log4net

Imports System
Imports System.Globalization
Imports System.Web.Mvc
Imports System.Security.Principal
Imports System.Threading
Imports System.Security
Imports Microsoft.AspNet.Identity
Imports System.Reflection

Public Class MvcApplication
    Inherits System.Web.HttpApplication

    Dim db As New Entities_BezerreSis
    Public Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Protected Sub Application_Start()
        log4net.Config.XmlConfigurator.Configure()

        AreaRegistration.RegisterAllAreas()
        FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters)
        RouteConfig.RegisterRoutes(RouteTable.Routes)
        BundleConfig.RegisterBundles(BundleTable.Bundles)

        'ModelBinders.Binders.Add(GetType(Decimal), New DecimalModelBinder())
    End Sub
    Protected Sub Session_Start()
        Session("FiltroGTK") = New gtkFiltro
        Try
            'log.Info(String.Format("PC IP: {0} -> {1}", If(String.IsNullOrWhiteSpace(Request.ServerVariables("HTTP_X_FORWARDED_FOR")), Request.UserHostAddress, Request.ServerVariables("HTTP_X_FORWARDED_FOR")), If(String.IsNullOrWhiteSpace(Request.ServerVariables("HTTP_X_FORWARDED_FOR")), System.Net.Dns.GetHostEntry(Request.UserHostAddress).HostName, System.Net.Dns.GetHostEntry(Request.ServerVariables("HTTP_X_FORWARDED_FOR")).HostName)))
            log.Info(String.Format("PC IP: {0}", If(String.IsNullOrWhiteSpace(Request.ServerVariables("HTTP_X_FORWARDED_FOR")), Request.UserHostAddress, Request.ServerVariables("HTTP_X_FORWARDED_FOR"))))
        Catch ex As Net.Sockets.SocketException
            If ex.ErrorCode = 11001 Then
                'Si la funcion "GetHostEntry" no encuentra el "UserHostAddress" levanta una excepcion.
                Dim msg As String = String.Format(ex.Message & "({0})", Request.UserHostAddress)
                log.Error(msg)
            Else
                log.Error(ex)
            End If
        End Try

        'Comprobamos que el usuario este logado en la intranet
        If User.Identity.IsAuthenticated Then
            log.Info(String.Format(User.Identity.Name & "{0}", " authenticated"))

            Dim userName = User.Identity.Name.ToLower
#If DEBUG Then
                        userName = "batznt\iciarabad"
#End If
            Dim FECHABAJA_Min As Nullable(Of Date) = CType(Now.Date, Nullable(Of Date))
            Dim RecursoWeb As Integer = Configuration.ConfigurationManager.AppSettings("RecursoWeb")
            Dim IdUsr As Nullable(Of Decimal) = (From Usr In db.USUARIOS From Rec In db.RECURSOS, gpr In Usr.GRUPOS, Recurso In gpr.RECURSOS
                                                 Where (Usr.FECHABAJA Is Nothing Or Usr.FECHABAJA >= FECHABAJA_Min) And (Usr.IDDIRECTORIOACTIVO IsNot Nothing AndAlso Usr.IDDIRECTORIOACTIVO.ToLower = userName) And Recurso.ID = RecursoWeb
                                                 Select Usr.ID Distinct).SingleOrDefault
            Try
                If IdUsr Is Nothing Then
                    Throw New Security.SecurityException("Acceso Denegado", New Exception("No tiene acceso al recurso en la intranet. <BR>Solicita acceso a la aplicacion"))
                Else

                    Dim Usr_SAB = db.USUARIOS.Find(IdUsr)
                    'Dim Usr_SAB = db.USUARIOS.Find(74781)
                    Dim cookie As HttpCookie = FormsAuthentication.GetAuthCookie(IdUsr, False)
                    Dim ticket = FormsAuthentication.Decrypt(cookie.Value)
                    Dim newTicket = New FormsAuthenticationTicket(ticket.Version, ticket.Name, ticket.IssueDate, ticket.Expiration, ticket.IsPersistent, String.Format("{0} {1} {2}", Usr_SAB.NOMBRE, Usr_SAB.APELLIDO1, Usr_SAB.APELLIDO2), ticket.CookiePath)
                    Dim encTicket = FormsAuthentication.Encrypt(newTicket)
                    cookie.Value = encTicket
                    HttpContext.Current.Response.Cookies.Add(cookie)
                    Dim Usr = (From Reg In db.USUARIOS Where Reg.ID = IdUsr Select Reg Distinct).SingleOrDefault
                    Dim aCookie As New HttpCookie("BezerreSis_Filtro")
                    aCookie.Values("Cultura") = Usr.IDCULTURAS
#If DEBUG Then
                    If String.IsNullOrWhiteSpace(aCookie.Values("IdPlanta")) Then
                        aCookie.Values("IdPlanta") = 1 '1-Igorre 2-MBTooling
                    End If
#End If
                    Response.Cookies.Add(aCookie)
                End If
            Catch ex As Security.SecurityException
                Response.Redirect("/Home/Mensaje")
            End Try
            Session("idUser") = IdUsr
        Else
            log.Info("User not authenticated")
            Session("idUser") = 0
            Response.Redirect("~/Home/Unauthorized")
        End If
        'Dim aCookie As New HttpCookie("BezerreSis_Filtro")
        'aCookie.Values("Cultura") = "es-ES"
        'Response.Cookies.Add(aCookie)
    End Sub

    Protected Sub Application_EndRequest()
    End Sub

    Private Sub MvcApplication_Error(sender As Object, e As EventArgs) Handles Me.[Error]
        'Dim ex As Exception = Server.GetLastError.GetBaseException
        Dim ex As Exception = Server.GetLastError
        'Dim exFrames = New StackTrace(Server.GetLastError).GetFrames()
        'Dim extemp As Exception = Server.GetLastError
        log.Error(ex)
        Response.Redirect("~/Home/MyError")
    End Sub

    Private Sub MvcApplication_AcquireRequestState(sender As Object, e As EventArgs) Handles Me.AcquireRequestState
        '#If DEBUG Then
        '        HttpContext.Current.User = New System.Security.Principal.GenericPrincipal(HttpContext.Current.User.Identity, {"Usuario"})
        '#End If
        Dim cookie As HttpCookie = Request.Cookies(FormsAuthentication.FormsCookieName)
        'log.Info("formsCookieName: " & FormsAuthentication.FormsCookieName)
        If cookie IsNot Nothing Then
            'Asignamos Roles al usuario en curso.
            HttpContext.Current.User = New System.Security.Principal.GenericPrincipal(HttpContext.Current.User.Identity, {"Usuario"})
            'Asignamos la cultura del usuario a la aplicacion
            Dim aCookie As HttpCookie = Request.Cookies("BezerreSis_Filtro")
            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(aCookie.Values("Cultura"))
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture
        End If
    End Sub

#Region "Funciones y Procesos"
    ''' <summary>
    ''' Comprobamos que este seleccionada una planta
    ''' </summary>
    Shared Function PlantaSeleccionada(aCookie As HttpCookie) As Boolean
        Try
            If aCookie Is Nothing OrElse String.IsNullOrWhiteSpace(aCookie.Values("IdPlanta")) OrElse String.IsNullOrWhiteSpace(aCookie.Values("Planta_Nombre")) Then Throw New ApplicationException("Sin Planta")
        Catch ex As ApplicationException
            MvcApplication.log.Debug(ex)
            Return False
        Catch ex As Exception
            MvcApplication.log.Error(ex)
            Return False
        End Try
        Return True
    End Function

    Shared Sub Loguear(tipoString As String, funcion As String, user As String, ParamArray params As String())
        CallByName(log, tipoString, CallType.Method, "================================================")
        CallByName(log, tipoString, CallType.Method, " Function: " & funcion)
        CallByName(log, tipoString, CallType.Method, "================================================")
        CallByName(log, tipoString, CallType.Method, "   User: " & user)
        For Each param In params
            CallByName(log, tipoString, CallType.Method, param)
        Next
        CallByName(log, tipoString, CallType.Method, "================================================")
    End Sub
#End Region
End Class

Public Class gtkFiltro
    ''' <summary>
    ''' Identificador de la planta en SAB.PLANTAS.ID
    ''' </summary>
    Private _IdPlanta As Integer?

    ''' <summary>
    ''' Indicamos si se debe cargar la Cookie del Filtro.
    ''' </summary>
    ''' <remarks></remarks>
    Private _CargarCookie As Boolean = True

    ''' <summary>
    ''' Identificador del codigo de planta en el BRAIN.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdPlanta As Integer?
        Get
            Return _IdPlanta
        End Get
        Set(value As Integer?)
            _IdPlanta = value
        End Set
    End Property

    ''' <summary>
    ''' Indicamos si se debe cargar la Cookie del Filtro.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CargarCookie As Boolean
        Get
            Return _CargarCookie
        End Get
        Set(value As Boolean)
            _CargarCookie = value
        End Set
    End Property
End Class