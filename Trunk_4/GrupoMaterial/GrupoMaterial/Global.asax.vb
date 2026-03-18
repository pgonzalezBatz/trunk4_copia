Imports System.Web.Optimization
Imports System.Security

Public Class MvcApplication
    Inherits System.Web.HttpApplication

    Private idRecurso = ConfigurationManager.AppSettings.Get("resourceId")
    Private strCn = ConfigurationManager.ConnectionStrings("ORACLECONNECTION").ConnectionString
    Private strCn2 = ConfigurationManager.ConnectionStrings("GRUPOLIVE").ConnectionString

    Public Shared log As log4net.ILog = log4net.LogManager.GetLogger("root.grupomaterial")

    Public Enum TipoLog As Integer
        Info = 0
        Err = 1
        Warn = 2
    End Enum
    Public Shared Sub WriteLog(ByVal texto As String, ByVal tipo As TipoLog, Optional ByVal ex As Exception = Nothing)
        Try
            Dim myTicket As SabLib.ELL.Ticket = Nothing
            If (System.Web.HttpContext.Current.Session("Ticket") IsNot Nothing) Then
                myTicket = CType(System.Web.HttpContext.Current.Session("Ticket"), SabLib.ELL.Ticket)
            End If
            If (myTicket IsNot Nothing) Then
                texto &= " [user]:" & myTicket.NombreUsuario
            End If
            If (tipo = TipoLog.Info) Then
                Log.Info(texto)
            ElseIf (tipo = TipoLog.Err) Then
                Log.Error(texto, ex)
            ElseIf (tipo = TipoLog.Warn) Then
                Log.Warn(texto, ex)
            End If
        Catch
        End Try
    End Sub

    Protected Sub Application_Start()
        AreaRegistration.RegisterAllAreas()
        FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters)
        RouteConfig.RegisterRoutes(RouteTable.Routes)
        BundleConfig.RegisterBundles(BundleTable.Bundles)
    End Sub

    Protected Sub Session_Start()
        log4net.Config.XmlConfigurator.Configure(New IO.FileInfo(Server.MapPath("~") & "\App_Data\log4netConfig.config"))
        Dim log As log4net.ILog = log4net.LogManager.GetLogger("grupomaterial")
        WriteLog(" iniciado session", TipoLog.Info)


        '    Dim usr = HttpContext.Current.User.Identity

        '    If User.Identity.IsAuthenticated Then
        '        'Dim IdUsr As Nullable(Of Decimal) = (From Usr In db.USUARIOS From Rec In db.RECURSOS, gpr In Usr.GRUPOS, Recurso In gpr.RECURSOS
        '        '                                     Where (Usr.FECHABAJA Is Nothing Or Usr.FECHABAJA >= FECHABAJA_Min) And (Usr.IDDIRECTORIOACTIVO IsNot Nothing AndAlso Usr.IDDIRECTORIOACTIVO.ToLower = User.Identity.Name.ToLower) And Recurso.ID = RecursoWeb
        '        '                                     Select Usr.ID Distinct).SingleOrDefault
        '        '-----------------------------------------------------------------------------------------

        '        Dim result As New List(Of String)
        '        result.Add("batznt\jzarraga")
        '        result.Add("batznt\aoti")
        '        result.Add("batznt\ayuste")
        '        result.Add("batznt\diglesias")
        '        result.Add("batznt\joseba.bilbao")

        '        Try
        '            If Not result.Contains(User.Identity.Name.ToLower()) Then
        '                Throw New Security.SecurityException("Acceso Denegado", New Exception("No tiene acceso al recurso en la intranet. <BR>Solicita acceso a la aplicacion"))
        '            End If
        '        Catch ex As Security.SecurityException
        '            Response.Redirect("/Home/Mensaje")
        '        End Try
        '    End If
    End Sub


    Sub Application_BeginRequest()
        'Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(h.GetCulture())
        'Threading.Thread.CurrentThread.CurrentUICulture = New System.Globalization.CultureInfo(h.GetCulture())
    End Sub

    Private Sub MvcApplication_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Error
        Dim err = Server.GetLastError()
        If err.GetType().Name = "SecurityException" Then
            FormsAuthentication.SignOut()
            Session.Abandon()
            Response.Redirect("~/accesodenegado.html")
            Dim log2 As log4net.ILog = log4net.LogManager.GetLogger("root.grupomaterial")
            log2.Error(err.Message, err)
            Exit Sub
        End If
        'File the error
        Dim log As log4net.ILog = log4net.LogManager.GetLogger("root.grupomaterial")
        log.Error(err.Message, err)
    End Sub

    Private Sub MvcApplication_AuthorizeRequest(sender As Object, e As EventArgs) Handles Me.AuthorizeRequest
        If User.Identity.IsAuthenticated AndAlso Request.Cookies(FormsAuthentication.FormsCookieName) Is Nothing Then
            Dim log As log4net.ILog = log4net.LogManager.GetLogger("root")
            If User.Identity.IsAuthenticated AndAlso Request.Cookies(FormsAuthentication.FormsCookieName) Is Nothing Then
                log.Info("Session starting. We have windows loggin for " + User.Identity.Name.ToLower + " recurso " + idRecurso.ToString)
                Dim idSab = db.GetLogin(User.Identity.Name.ToLower, idRecurso, strCn)
                'Dim idSab = db.GetLogin("batznt\lpacin", idRecurso, strCn)
                If Not idSab.HasValue Then
                    Exit Sub
                End If

                If User.Identity.Name.ToLower = "batznt\jzarraga" Then
                    'lst(0) = 63417 'Mariola
                    'idSab = 57561 'Igor bustinza
                    'lst(0) = 62920 'Daniel Alonso
                    ''''''''''''''idSab = 63485 'Enara
                    'idSab = 61487 'Elena
                    'lst(0) = 58814
                    'lst(0) = 57561
                    'idSab = 67378 'usuario maquina
                    'idSab = 59312
                    'idSab = 58732 'Prototipos de sistemas
                    'idSab = 68699 ' Maite Barandarain
                    'idSab = 66656
                    'idSab = 57944
                    'idSab = 57247 ' Egoitz Anzola
                    'idSab = 60831
                    'idSab = 68595
                    'idSab = 66873 'Christian Teresa
                    'idSab = 57525
                    'idSab = 68595 'Edith guerrero                  
                End If
                'h.SetCulture(idSab.Value, ConfigurationManager.ConnectionStrings("oracle").ConnectionString)
                'Dim DbEntity As New Entities()  AQUI METER CODIGO
                Dim nivel As Integer = db.GetNivel(idSab.Value, strCn2)


                SimpleRoleProvider.SetAuthCookieWithRole(idSab.Value, Function() nivel)
                Dim p = db.GetUsuarioSAB(idSab.Value, strCn)
                Response.Cookies.Add(New HttpCookie("username", p.NOMBRE + " " + p.APELLIDO1))
            End If
        End If
    End Sub

End Class
