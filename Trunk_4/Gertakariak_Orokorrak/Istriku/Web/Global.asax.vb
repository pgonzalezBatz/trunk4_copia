Imports System.Web.SessionState
Imports System.IO
Imports System.Globalization.CultureInfo
Imports System.Xml
Imports System.Web.Routing
Imports System.Web.Optimization
Imports Microsoft.AspNet.FriendlyUrls
Imports System.Net
Imports Microsoft.SqlServer.Types

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Public Shared log As log4net.ILog = log4net.LogManager.GetLogger("root.Istriku")

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena al iniciar la aplicación
        log4net.Config.XmlConfigurator.Configure()

        'SqlServerTypes.LoadNativeAssemblies(Server.MapPath("~/bin"))

        ' Se desencadena al iniciar la aplicación
        'RouteConfig.RegisterRoutes(RouteTable.Routes)
        'BundleConfig.RegisterBundles(BundleTable.Bundles)

        'Dim log4netFile As String = System.Configuration.ConfigurationManager.AppSettings.Get("log4netFile")
        'Configuramos el log4net
        'log4net.Config.XmlConfigurator.Configure(New IO.FileInfo(log4netFile))
        'log4net.Config.XmlConfigurator.Configure(New IO.StreamReader(My.Resources.log4netConfig.ToString))

        'log4net.Config.XmlConfigurator.Configure(New IO.FileInfo(Server.MapPath("~") & "\App_Code\log4netConfig.xml"))

        '--------------------------------------------------------------------------------
        'FROGA: Cambia el "Log" para que se guarde en local.
        '--------------------------------------------------------------------------------
        '#If DEBUG Then
        '        'Dim Repositorio As log4net.Repository.ILoggerRepository = log4net.LogManager.GetRepository
        '        'Dim Anexos As log4net.Appender.IAppender() = Repositorio.GetAppenders()
        '        'Dim ArchivoAnexo As log4net.Appender.FileAppender = Anexos(0)
        '        'ArchivoAnexo.File = ".\Logs\Log"
        '        'ArchivoAnexo.ActivateOptions()

        '        'System.Configuration.ConfigurationManager.AppSettings.Item("RecursoWeb") = "334" '(Istriku)
        '        'System.Configuration.ConfigurationManager.AppSettings.Item("CurrentStatus") = "Debug"
        '#Else
        '        'PageBase.log.Debug("KAIXO - 2")
        '#End If
        '--------------------------------------------------------------------------------

    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena al iniciar la sesión
        ServicePointManager.Expect100Continue = False
        ServicePointManager.SecurityProtocol = CType(3072, SecurityProtocolType)
        Dim myTicket As New SabLib.ELL.Ticket
        Dim lg As New SabLib.BLL.LoginComponent
        Dim Recurso As String = System.Configuration.ConfigurationManager.AppSettings.Get("RecursoWeb")

        '------------------------------------------------------
        'Inicializacion del Pooling debido a errores de Oracle.
        '------------------------------------------------------
        'ConsultaPooling()
        '------------------------------------------------------

        '------------------------------------------------------
        'Codigo para el acceso a la aplicacion Sin Login.
        '------------------------------------------------------
        myTicket = lg.Login(User.Identity.Name)
#If DEBUG Then
        'myTicket = lg.Login("batznt\aortuzar")
        'Session("FiltroGTK") = New gtkFiltro With {.TipoIncidencia = My.Settings.IdTipoIncidencia} '5
        'Session("PerfilUsuario") = PageBase.Perfil.Administrador
        'Session("Propiedades_gvSucesos") = New gtkGridView With {.IdSeleccionado = 42185} 'Identificador del registro con el que vamos a hacer pruebas.
        'myTicket.Culture = "eu-ES"
#End If
        'If String.Compare(Request.Url.Segments.LastOrDefault, "IndiceFecuencia.aspx", True) <> 0 Then
        If Not myTicket Is Nothing Then
            If lg.AccesoRecursoValido(myTicket, Recurso) Then
                Session("Ticket") = myTicket
                '#If DEBUG Then
                '				Response.Redirect("~/Default.aspx", False)
                '#End If
            Else
                Response.Redirect(PageBase.PAG_PERMISODENEGADO, False)
            End If
        Else
            Response.Redirect(PageBase.PAG_PERMISODENEGADO, False)
        End If
        'End If
        '------------------------------------------------------
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena al comienzo de cada solicitud
#If DEBUG And Trace Then
		'Habilitamos el seguimiento para la aplicacion
		Context.Trace.IsEnabled = True
#End If
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena al intentar autenticar el uso
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena cuando se produce un error
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena cuando finaliza la sesión
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena cuando finaliza la aplicación
    End Sub

    Private Sub Page_AcquireRequestState(sender As Object, e As EventArgs) Handles Me.AcquireRequestState
        '---------------------------------------------------------------------------
        'Detectamos si la sesion a caducado para indicarselo al usuario.
        '---------------------------------------------------------------------------
        Try
            If HttpContext.Current IsNot Nothing _
            AndAlso HttpContext.Current.Session IsNot Nothing Then
                If String.Compare(Request.Url.Segments.LastOrDefault, "Login.aspx", True) <> 0 _
                    And String.Compare(Request.Url.Segments.LastOrDefault, "IndiceFecuencia.aspx", True) <> 0 Then
                    Dim FiltroGTK As gtkFiltro = Session("FiltroGTK")
                    If FiltroGTK Is Nothing OrElse FiltroGTK.TipoIncidencia Is Nothing Then
                        Session.Clear()
                        Session.Abandon()
                        Response.Redirect("~/Login.aspx", False)
                    End If
                End If
            End If
        Catch ex As Exception
            log.Error(ex)
        End Try
    End Sub

    Private Sub Page_PostRequestHandlerExecute(sender As Object, e As EventArgs) Handles Me.PostRequestHandlerExecute
        Try
            If HttpContext.Current IsNot Nothing AndAlso HttpContext.Current.Session IsNot Nothing Then
                If String.Compare(Request.Url.Segments.LastOrDefault, "Login.aspx", True) <> 0 _
                    And String.Compare(Request.Url.Segments.LastOrDefault, "PermisoDenegado.aspx", True) <> 0 _
                    And String.Compare(Request.Url.Segments.LastOrDefault, "IndiceFecuencia.aspx", True) <> 0 Then
                    Dim FiltroGTK As gtkFiltro = Session("FiltroGTK")
                    If FiltroGTK Is Nothing Then
                        Throw New ApplicationException(String.Format("{0} / {1}", "Sesion Caducada".Itzuli, "Regrese a la pagina de inicio.".Itzuli, True))
                    ElseIf FiltroGTK.TipoIncidencia Is Nothing Then
                        Throw New ApplicationException("FiltroGTK.TipoIncidencia: 'ES NULO'")
                    End If
                End If
            End If
        Catch ex As ApplicationException
            log.Debug(ex)
            Response.Redirect(PageBase.PAG_PERMISODENEGADO & "?msg=" & ex.Message, True)
        Catch ex As Exception
            log.Error(ex)
            Response.Redirect("~/Login.aspx", True)
        End Try
    End Sub
End Class

Public Class PageBase
    Inherits Page

#Region "Variables compartidas"
    Public ItzultzaileWeb As New LocalizationLib.Itzultzaile

    Public Shared PAG_PERMISODENEGADO As String = "~/PermisoDenegado.aspx"
    Public Shared Log As log4net.ILog = Global_asax.log

    Public Property FiltroGTK() As gtkFiltro
        Get
            If (Session("FiltroGTK") Is Nothing) Then Session("FiltroGTK") = New gtkFiltro
            Return CType(Session("FiltroGTK"), gtkFiltro)
        End Get
        Set(value As gtkFiltro)
            Session("FiltroGTK") = value
        End Set
    End Property
    Public Property PerfilUsuario() As Nullable(Of Perfil)
        Get
            If (Session("PerfilUsuario") Is Nothing) Then
                Return Nothing
            Else
                Return CType(Session("PerfilUsuario"), Perfil)
            End If
        End Get
        Set(ByVal value As Nullable(Of Perfil))
            Session("PerfilUsuario") = value
        End Set
    End Property
    ''' <summary>
    ''' Identificador del perfil en la base de datos para la estructura de perfiles
    ''' </summary>
    ''' <remarks></remarks>
    Enum Perfil
        Administrador = 0
        Usuario = 1
        Consultor = 2
        UsuarioAcceso = 3
        AdministradorPlanta = 4
    End Enum
#End Region

#Region "Ticket"
    ''' <summary>
    ''' Obtiene el ticket de la session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Ticket() As SabLib.ELL.Ticket
        Get
            If (Session("Ticket") Is Nothing) Then
                Return Nothing
            Else
                Return CType(Session("Ticket"), SabLib.ELL.Ticket)
            End If

        End Get
        Set(ByVal value As SabLib.ELL.Ticket)
            Session("Ticket") = value
        End Set
    End Property
#End Region

#Region "Page Load"
    Private Sub Page_Error(sender As Object, e As EventArgs) Handles Me.Error
        'If Server.GetLastError IsNot Nothing Then log.Error(Server.GetLastError.ToString, Server.GetLastError)
        '-------------------------------------------------------------------------------------------
        If Server.GetLastError IsNot Nothing Then
            Dim ex As Exception = Server.GetLastError
            Log.Error(ex.ToString, ex)
            'If TypeOf ex Is HttpException Then Response.Redirect(PAG_PERMISODENEGADO & "?msg=" & ex.Message.Substring(0, ex.Message.IndexOf("(")))
            If TypeOf ex Is HttpException Then Response.Redirect(PAG_PERMISODENEGADO & "?msg=" & If(ex.Message.IndexOf("(") < 0, ex.Message, ex.Message.Substring(0, ex.Message.IndexOf("("))))
        End If
        '-------------------------------------------------------------------------------------------
    End Sub
    ''' <summary>
    ''' <para>Si no tiene ticket, se vuelve a logear</para>    
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Ticket") Is Nothing Then Response.Redirect(PageBase.PAG_PERMISODENEGADO, False)
    End Sub
#End Region

#Region "Cultura"

    ' ''' <summary>
    ' ''' Inicializa la cultura
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Public Sub inicializarCultura()
    '	MyBase.InitializeCulture()
    'End Sub

    Protected Overrides Sub InitializeCulture()
        'Try
        If Ticket Is Nothing Then
            Dim cultureInfo As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture("es-ES")
            System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture
            Culture = cultureInfo.Name
            Ticket = New SabLib.ELL.Ticket With {.Culture = cultureInfo.Name}
        Else
            Culture = Ticket.Culture
        End If
        'Ticket.Culture = CurrentCulture.Name
        MyBase.InitializeCulture()
        'Catch

        'End Try
    End Sub

#End Region

#Region "Traducciones"
    ''' <summary>
    ''' En este evento se encuentran cargados todos los controles de la página y podemos usar el traductor "Itzultzaile".
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_PreRenderComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRenderComplete
        ItzultzaileWeb.TraducirWebControls(Page.Controls)
    End Sub
#End Region

#Region "log4net"

    ''' <summary>
    ''' Escribe en el log un error
    ''' </summary>
    ''' <param name="key"></param>
    ''' <param name="ex"></param>
    ''' <remarks></remarks>
    <Obsolete("NO USAR", False)>
    Public Shared Function LogError(ByVal key As String, ByVal ex As Exception) As String
        Try
            Log.Error(key, ex)
            Return key
        Catch
            Return String.Empty
        End Try
    End Function

#End Region

#Region "Informes Cognos"
    ''' <summary>
    ''' Obtiene la ruta del informe de la key proporcionada
    ''' </summary>
    ''' <param name="key">Nombre del informe</param>
    ''' <returns>Ruta del mismo</returns>
    ''' <remarks></remarks>
    Public Function consultarInformeCognos(ByVal key As String, ByVal scheme As String) As String
        Try
            Dim ruta As String = String.Empty
            Dim xmlDoc As New XmlDocument
            Dim m_nodelist As XmlNodeList
            xmlDoc.Load(Server.MapPath("~") & "\App_Code\InformesCognos.xml")
            m_nodelist = xmlDoc.SelectNodes("Informes/Informe")
            For Each m_node As XmlNode In m_nodelist
                If (m_node.Attributes.GetNamedItem("name").Value = key) Then
                    'ruta = Request.Url.Scheme & "://" & m_node.InnerText
                    ruta = scheme & "://" & m_node.InnerText
                    Exit For
                End If
            Next
            Return ruta
        Catch ex As Exception
            Log.Error(Server.MapPath("~") & "\App_Code\InformesCognos.xml")
            Log.Error("Error al leer la ruta del informe de cognos con la key=" & key)
            Return String.Empty
        End Try
    End Function
#End Region

#Region "Funciones y Procesos"
    ''' <summary>
    ''' Comprobamos que la Session no este caducada.
    ''' No incluir dentro de un Try Catch.
    ''' </summary>
    ''' <remarks></remarks>
    Sub ComprobarSesion()
        If Page.IsPostBack AndAlso Session("FiltroGTK") Is Nothing Then
            Response.Redirect(PAG_PERMISODENEGADO & "?msg=" & String.Format("{0} / {1}",
                        ItzultzaileWeb.Itzuli("Sesion Caducada"),
                        ItzultzaileWeb.Itzuli("Regrese a la pagina de inicio.")), True)
        End If
    End Sub

    ''' <summary>
    ''' Proceso de notificacion por correo 
    ''' </summary>
    ''' <param name="lTO">Listado de identificadores de usuarios de SAB a notificar</param>
    Sub EnviarEmail(ByVal lTO As List(Of Integer), ByVal idIncidencia As Integer)
        Dim mail As New MailMessage()
        Dim UsuariosComponent As New SabLib.BLL.UsuariosComponent
        Try
            'Dim lIdUsuarios As List(Of String) = If(Request("hd_IdUsuarios") Is Nothing, New List(Of String), Request("hd_IdUsuarios").Split(",").ToList)

            If lTO.Any Then
                Dim href As String = Request.Url.Scheme & Uri.SchemeDelimiter & Request.Url.Authority & Request.ApplicationPath & "/Login.aspx?idIncidencia=" & idIncidencia

                'Definir dereccion (FROM)
                Dim UsrFrom As SabLib.ELL.Usuario = UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = Ticket.IdUser})
                If Not String.IsNullOrWhiteSpace(UsrFrom.Email) Then mail.From = New MailAddress(UsrFrom.Email)

                'Para (TO)
                For Each Id As Integer In lTO
                    Dim UsrTO As SabLib.ELL.Usuario = UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = Id})
                    If UsrTO IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(UsrTO.Email) Then mail.To.Add(New MailAddress(UsrTO.Email))
                Next
#If DEBUG Then
                mail.To.Clear()
                mail.To.Add("diglesias@batz.es")
#End If
                'Asunto (SUBJECT)
                mail.Subject = ItzultzaileWeb.Itzuli("AVISO de Accidente/Incidente")
                'Cuerpo del mensaje (BODY)
                mail.Body &= "<a href=""" & href & """ target=""_blank"" type=""text/html"">" & ItzultzaileWeb.Itzuli("Accidente/Incidente") & " " & ItzultzaileWeb.Itzuli("Nº:") & idIncidencia & "</a>"

                mail.IsBodyHtml = True
                mail.BodyEncoding = System.Text.Encoding.UTF8
                mail.SubjectEncoding = System.Text.Encoding.UTF8

                'Enviar el Mensaje
                Dim smtp As New SmtpClient("POSTA.BATZ.COM") 'Nombre del servidor de Exchange (IP => 172.17.200.3).
                smtp.Send(mail)
                Log.Info("Mail sent")
                smtp.Dispose()
            Else
                Throw New ApplicationException("Falta los email de destino.")
            End If
        Catch ex As ApplicationException
            Throw
        Catch ex As Exception
            Log.Error(ex)
            Throw
        End Try
    End Sub
#End Region
End Class
Public Class UserControlBase
    Inherits System.Web.UI.UserControl

    Public ItzultzaileWeb As New LocalizationLib.Itzultzaile

End Class

Public Class gtkFiltro
    ''' <summary>
    ''' Texto que se va a buscar en diferentes tablas.
    ''' </summary>
    Private _descripcion As String
    ''' <summary>
    ''' Campo para realizar busquedas por fecha.
    ''' </summary>
    Private _FechaAperturaInicio As Nullable(Of Date)
    ''' <summary>
    ''' Campo para realizar busquedas por fecha.
    ''' </summary>
    Private _FechaAperturaFin As Nullable(Of Date)
    ''' <summary>
    ''' Campo para realizar busquedas por fecha.
    ''' </summary>
    Private _FechaCierreInicio As Nullable(Of Date)
    ''' <summary>
    ''' Campo para realizar busquedas por fecha.
    ''' </summary>
    Private _FechaCierreFin As Nullable(Of Date)

    ''' <summary>
    ''' Estado de las Incidencias.
    ''' </summary>
    Private _Estado As Nullable(Of EstadoIncidencia)
    ''' <summary>
    ''' Identificadores de los responsables de las Incidencias y Acciones.
    ''' </summary>
    Private _Responsables As List(Of Integer)
    ''' <summary>
    ''' Identificadores de los responsables de las Incidencias y Acciones.
    ''' </summary>
    Private _Caracteristicas As List(Of Integer)
    ''' <summary>
    ''' Identificadores de los responsables de las Incidencias y Acciones.
    ''' </summary>
    Private _Activos As List(Of String)
    ''' <summary>
    ''' Identificador del Tipo de Incidencia y Tipo de Documento:
    ''' 1-Troqueleria (gtkTroqueleria),
    ''' 2-Servicios Generales (gtkSerciviosGenerales),
    ''' 3-Txokos (gtkTxokos),
    ''' 4-Sugerencias (gtkSugerencias),
    ''' 5-Seguridad Laboral-Sucesos (gtkIstriku),
    ''' 6-Mantenimiento de Sistemas (gtkMantSistemas),
    ''' 7-Open Issues.
    ''' 8-Open Issues MB
    ''' 9-Sistemas de Automocion
    ''' </summary>
    ''' <remarks>Gertakariak.IdTipoIncidencia</remarks>
    Private _TipoIncidencia As Nullable(Of Integer)
    ''' <summary>
    ''' Identificador del proyecto
    ''' </summary>
    ''' <remarks></remarks>
    Private _Proyecto As Nullable(Of Integer)
    ''' <summary>
    ''' Identificadores de los proyectos.
    ''' </summary>
    ''' <remarks></remarks>
    Private _Proyectos As List(Of Nullable(Of Integer))

    ''' <summary>
    ''' Texto que se va a buscar en diferentes tablas.
    ''' </summary>
    Public Property Descripcion As String
        Get
            Return _descripcion
        End Get
        Set(ByVal value As String)
            _descripcion = value
        End Set
    End Property

    ''' <summary>
    ''' Campo para realizar busquedas por fecha.
    ''' </summary>
    Public Property FechaAperturaInicio As Nullable(Of Date)
        Get
            Return _FechaAperturaInicio
        End Get
        Set(value As Nullable(Of Date))
            _FechaAperturaInicio = value
        End Set
    End Property
    ''' <summary>
    ''' Campo para realizar busquedas por fecha.
    ''' </summary>
    Public Property FechaAperturaFin As Nullable(Of Date)
        Get
            Return _FechaAperturaFin
        End Get
        Set(value As Nullable(Of Date))
            _FechaAperturaFin = value
        End Set
    End Property
    Public Property FechaCierreInicio As Nullable(Of Date)
        Get
            Return _FechaCierreInicio
        End Get
        Set(value As Nullable(Of Date))
            _FechaCierreInicio = value
        End Set
    End Property
    ''' <summary>
    ''' Campo para realizar busquedas por fecha.
    ''' </summary>
    Public Property FechaCierreFin As Nullable(Of Date)
        Get
            Return _FechaCierreFin
        End Get
        Set(value As Nullable(Of Date))
            _FechaCierreFin = value
        End Set
    End Property

    ''' <summary>
    ''' Estado de las Incidencias.
    ''' </summary>
    ''' <value></value>
    Public Property Estado As Nullable(Of EstadoIncidencia)
        Get
            Return _Estado
        End Get
        Set(value As Nullable(Of EstadoIncidencia))
            _Estado = value
        End Set
    End Property

    ''' <summary>
    ''' Identificadores de los responsables de las Incidencias y Acciones.
    ''' </summary>
    Public Property Responsables As List(Of Integer)
        Get
            If _Responsables Is Nothing Then _Responsables = New List(Of Integer)
            Return _Responsables
        End Get
        Set(value As List(Of Integer))
            _Responsables = value
        End Set
    End Property

    ''' <summary>
    ''' Identificadores de caracteristicas o propiedades (gtkEstructuras) que tiene asignada una Incidencia.
    ''' </summary>
    Public Property Caracteristicas As List(Of Integer)
        Get
            If _Caracteristicas Is Nothing Then
                _Caracteristicas = New List(Of Integer)
            End If
            Return _Caracteristicas
        End Get
        Set(value As List(Of Integer))
            _Caracteristicas = value
        End Set
    End Property

    ''' <summary>
    ''' Identificadores de los responsables de las Incidencias y Acciones.
    ''' </summary>
    Public Property Activos As List(Of String)
        Get
            If _Activos Is Nothing Then
                _Activos = New List(Of String)
            End If
            Return _Activos
        End Get
        Set(value As List(Of String))
            _Activos = value
        End Set
    End Property

    ''' <summary>
    ''' Identificador del Tipo de Incidencia y Tipo de Documento:
    ''' 1-Troqueleria (gtkTroqueleria),
    ''' 2-Servicios Generales (gtkSerciviosGenerales),
    ''' 3-Txokos (gtkTxokos),
    ''' 4-Sugerencias (gtkSugerencias),
    ''' 5-Seguridad Laboral-Sucesos (gtkIstriku),
    ''' 6-Mantenimiento de Sistemas (gtkMantSistemas),
    ''' 7-Open Issues.
    ''' 8-Open Issues MB
    ''' 9-Sistemas de Automocion
    ''' </summary>
    ''' <remarks>Gertakariak.IdTipoIncidencia</remarks>
    Public Property TipoIncidencia As Nullable(Of Integer)
        Get
            Return _TipoIncidencia
        End Get
        Set(value As Nullable(Of Integer))
            _TipoIncidencia = value
        End Set
    End Property

    ''' <summary>
    ''' Identificador del proyecto
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Obsolete("Cambiar por 'Proyectos'")> Public Property Proyecto As Nullable(Of Integer)
        Get
            Return _Proyecto
        End Get
        Set(value As Nullable(Of Integer))
            _Proyecto = value
        End Set
    End Property
    ''' <summary>
    ''' Identificadores de los proyectos.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Proyectos As List(Of Nullable(Of Integer))
        Get
            If _Proyectos Is Nothing Then _Proyectos = New List(Of Nullable(Of Integer))
            Return _Proyectos
        End Get
        Set(value As List(Of Nullable(Of Integer)))
            _Proyectos = value
        End Set
    End Property

    ''' <summary>
    ''' Estado de la Incidencia. 
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum EstadoIncidencia
        ''' <summary>
        ''' Incidencia SIN "Fecha de Cierre" (GERTAKARIA.FECHACIERRE = NULL)
        ''' </summary>
        ''' <remarks></remarks>
        Abierta
        ''' <summary>
        ''' Incidencia CON "Fecha de Cierre" (GERTAKARIA.FECHACIERRE != NULL)
        ''' </summary>
        ''' <remarks></remarks>
        Cerrada
    End Enum

    ''' <summary>
    ''' Indicamos la procedencia del Incidenia.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum ProcedenciaNC
        Interna = 1
        Externa = 2
        Cliente = 3
    End Enum
    ''' <summary>
    ''' Indicamos la procedencia de la incidencia.
    ''' </summary>
    ''' <remarks></remarks>
    Private _Procedencia As List(Of Integer)
    ''' <summary>
    ''' Indicamos la procedencia de la incidencia.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Procedencia As List(Of Integer)
        Get
            If _Procedencia Is Nothing Then
                _Procedencia = New List(Of Integer)
            End If
            Return _Procedencia
        End Get
        Set(value As List(Of Integer))
            _Procedencia = value
        End Set
    End Property

    Private _InformeFinal As List(Of Integer)
    Public Property lInformeFinal As List(Of Integer)
        Get
            If _InformeFinal Is Nothing Then _InformeFinal = New List(Of Integer)
            Return _InformeFinal
        End Get
        Set(value As List(Of Integer))
            _InformeFinal = value
        End Set
    End Property
    ''''Private _ModificarEvaluacion As List(Of Integer)
    ''''Public Property lModificarEvaluacion As List(Of Integer)
    ''''    Get
    ''''        If _ModificarEvaluacion Is Nothing Then _ModificarEvaluacion = New List(Of Integer)
    ''''        Return _ModificarEvaluacion
    ''''    End Get
    ''''    Set(value As List(Of Integer))
    ''''        _ModificarEvaluacion = value
    ''''    End Set
    ''''End Property

    ''''Public Enum InformeFinal
    ''''    Si = 1
    ''''    No = 2
    ''''    No_Aplica = 3
    ''''End Enum
    ''''Public Enum ModificarEvaluacion
    ''''    Si = 1
    ''''    No = 2
    ''''    No_Procede = 3
    ''''End Enum

    Public Enum Riesgo
        Caída_de_personas_a_distinto_nivel = 1
        Caída_de_personas_al_mismo_nivel
        Caída_de_objetos_por_desplome_o_derrumbamiento
        Caída_de_objetos_en_manipulación
        Caída_por_objetos_desprendidos
        Pisadas_sobre_objetos
        Choques_contra_objetos_inmóviles
        Choques_contra_objetos_móviles
        Cortes_o_golpes_por_objetos_o_herramientas
        Proyección_de_fragmentos_o_partículas
        Atrapamiento_por_o_entre_objetos
        Atrapamiento_por_vuelco_de_maquinas
        Sobreesfuerzos
        Exposición_a_temperaturas_ambientales_extremas
        Contactos_térmicos
        Exposición_a_contactos_eléctricos
        Exposición_a_sustancias_nocivas
        Contactos_a_sustancias_cáusticas_y_o_corrosivas
        Exposición_a_radiaciones
        Explosiones
        Incendios
        Accidentes_causados_por_seres_vivos
        Atropellos_o_golpes_con_vehículos
        Espacios_confinados
        Agresiones
    End Enum


    Private _Riesgo As List(Of Integer)
    Public Property lRiesgo As List(Of Integer)
        Get
            If _Riesgo Is Nothing Then _Riesgo = New List(Of Integer)
            Return _Riesgo
        End Get
        Set(value As List(Of Integer))
            _Riesgo = value
        End Set
    End Property


    Public Enum NivelDeRiesgo
        Trivial = 1
        Tolerable
        Moderado
        Importante
        Intolerable
    End Enum

    Private _NivelDeRiesgo As List(Of Integer)
    Public Property lNivelDeRiesgo As List(Of Integer)
        Get
            If _NivelDeRiesgo Is Nothing Then _NivelDeRiesgo = New List(Of Integer)
            Return _NivelDeRiesgo
        End Get
        Set(value As List(Of Integer))
            _NivelDeRiesgo = value
        End Set
    End Property
End Class

Public Class gtkGridView
    ''' <summary>
    ''' Identificador del Registro Seleccionado.
    ''' </summary>
    Private _IdSeleccionadoIstriku As Nullable(Of Integer)
    ''' <summary>
    ''' Nombre de la PROPIEDAD por la que se quiere ordenar los objetos.
    ''' </summary>
    Private _CampoOrdenacion As String
    ''' <summary>
    ''' Direccion de Ordenacion para el Campo de Ordenacion (Nombre de la Propiedad).
    ''' </summary>
    Private _DireccionOrdenacion As Nullable(Of System.ComponentModel.ListSortDirection)
    ''' <summary>
    ''' Indice de la página en curso.
    ''' </summary>
    Private _Pagina As Nullable(Of Integer)

    ''' <summary>
    ''' Identificador del Registro Seleccionado.
    ''' </summary>
    Public Property IdSeleccionadoIstriku() As Nullable(Of Integer)
        Get
            Return _IdSeleccionadoIstriku
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _IdSeleccionadoIstriku = value
        End Set
    End Property

    ''' <summary>
    ''' Nombre de la PROPIEDAD por la que se quiere ordenar los objetos.
    ''' </summary>
    Public Property CampoOrdenacion() As String
        Get
            Return _CampoOrdenacion
        End Get
        Set(ByVal value As String)
            _CampoOrdenacion = value
        End Set
    End Property
    ''' <summary>
    ''' Direccion de Ordenacion para el Campo de Ordenacion (Nombre de la Propiedad).
    ''' </summary>
    Public Property DireccionOrdenacion() As Nullable(Of System.ComponentModel.ListSortDirection)
        Get
            Return _DireccionOrdenacion
        End Get
        Set(ByVal value As Nullable(Of System.ComponentModel.ListSortDirection))
            _DireccionOrdenacion = value
        End Set
    End Property

    ''' <summary>
    ''' Indice de la página en curso.
    ''' </summary>
    Public Property Pagina As Nullable(Of Integer)
        Get
            Return _Pagina
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _Pagina = value
        End Set
    End Property

End Class

''' <summary>
''' Procedencia del Suceso (Accidente/Incidente).
''' </summary>
Public Enum ProcedenciaNC
    ''' <summary>
    ''' Procedencia del Suceso (Con lesion).
    ''' </summary>
    Accidente = 4
    ''' <summary>
    ''' Procedencia del Suceso (Sin lesion).
    ''' </summary>
    Incidente = 5
End Enum

Public Enum EstadoParte
    Pendiente = 0
    Aceptado = 1
    Denegado = -1
End Enum

Public Class BundleConfig
    ' Para obtener más información sobre la unión, visite http://go.microsoft.com/fwlink/?LinkID=303951
    Public Shared Sub RegisterBundles(ByVal bundles As BundleCollection)
        'bundles.Add(New ScriptBundle("~/bundles/WebFormsJs").Include(
        '                "~/Scripts/WebForms/WebForms.js",
        '                "~/Scripts/WebForms/WebUIValidation.js",
        '                "~/Scripts/WebForms/MenuStandards.js",
        '                "~/Scripts/WebForms/Focus.js",
        '                "~/Scripts/WebForms/GridView.js",
        '                "~/Scripts/WebForms/DetailsView.js",
        '                "~/Scripts/WebForms/TreeView.js",
        '                "~/Scripts/WebForms/WebParts.js"))

        '' El orden es muy importante para el funcionamiento de estos archivos ya que tienen dependencias explícitas
        'bundles.Add(New ScriptBundle("~/bundles/MsAjaxJs").Include(
        '        "~/Scripts/WebForms/MsAjax/MicrosoftAjax.js",
        '        "~/Scripts/WebForms/MsAjax/MicrosoftAjaxApplicationServices.js",
        '        "~/Scripts/WebForms/MsAjax/MicrosoftAjaxTimer.js",
        '        "~/Scripts/WebForms/MsAjax/MicrosoftAjaxWebForms.js"))

        '' Use la versión de desarrollo de Modernizr para desarrollar y aprender. Luego, cuando esté listo
        '' para la producción, use la herramienta de creación en http://modernizr.com para elegir solo las pruebas que necesite
        'bundles.Add(New ScriptBundle("~/bundles/modernizr").Include(
        '                "~/Scripts/modernizr-*"))

        'ScriptManager.ScriptResourceMapping.AddDefinition("respond", New ScriptResourceDefinition() With {
        '        .Path = "~/Scripts/respond.min.js",
        '        .DebugPath = "~/Scripts/respond.js"})

    End Sub
End Class

'Public Module RouteConfig
'    Sub RegisterRoutes(ByVal routes As RouteCollection)
'        Dim settings As FriendlyUrlSettings = New FriendlyUrlSettings()
'        settings.AutoRedirectMode = RedirectMode.Permanent
'        routes.EnableFriendlyUrls(settings)
'    End Sub
'End Module