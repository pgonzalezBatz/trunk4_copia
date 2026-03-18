Imports System.ComponentModel
Imports System.Net
Imports System.Web.SessionState

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Public Shared log As log4net.ILog = log4net.LogManager.GetLogger("root.GTK_Troqueleria")
    Dim lg As New SabLib.BLL.LoginComponent

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena al iniciar la aplicación
        log4net.Config.XmlConfigurator.Configure()
        '#If DEBUG Then
        '        System.Configuration.ConfigurationManager.AppSettings.Set("CurrentStatus", "DEBUG")
        '        System.Configuration.ConfigurationManager.AppSettings.Set("RecursoWeb", "339")  '339-Gertakariak Sistemas de Automocion, 112-Gertakariak, 331-Batztracker
        '        System.Configuration.ConfigurationManager.AppSettings.Set("GrupoRecursoWeb_Extranet", "112") '112-Gertakariak Troqueleria (solo para hacer pruebas)
        '#End If
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ServicePointManager.Expect100Continue = False
        ServicePointManager.SecurityProtocol = CType(3072, SecurityProtocolType)
        '-----------------------------------------------------------------------------------------
        'log.Info("Session_Start")
        '-----------------------------------------------------------------------------------------
        'Log de Inicio de sesion
        '-----------------------------------------------------------------------------------------
        Dim msg As String = String.Format("Session_Start" _
                                          & vbCrLf & New String("=", 30) _
                                          & vbCrLf & "- User.Identity.Name: {0}" _
                                          & vbCrLf & "- RawUrl: " & Request.RawUrl _
                                          & vbCrLf & "- Url.OriginalString: " & Request.Url.OriginalString _
                                          & vbCrLf & "- UrlReferrer.OriginalString: " & If(Request.UrlReferrer Is Nothing, "Acceso Directo", Request.UrlReferrer.OriginalString) _
                                          & vbCrLf & "- Url.DnsSafeHost: " & Request.Url.DnsSafeHost _
                                          & vbCrLf & New String("=", 30),
                                          If(User Is Nothing OrElse User.Identity Is Nothing OrElse String.IsNullOrWhiteSpace(User.Identity.Name), "?", User.Identity.Name))
        log.Info(msg)
        '-----------------------------------------------------------------------------------------

        ' Se desencadena al iniciar la sesión
        Dim Recurso As String = System.Configuration.ConfigurationManager.AppSettings.Get("RecursoWeb")
        Dim myTicket As New SabLib.ELL.Ticket
        Dim pb As New PageBase
        Dim IdSession As String = Request("IdSession")

        If String.Compare(Request.Url.Segments.LastOrDefault, "PlantillaAviso.aspx", True) <> 0 _
            And String.Compare(Request.Url.Segments.LastOrDefault, "Responsables.aspx", True) <> 0 _
            And String.Compare(Request.Url.Segments.LastOrDefault, "ResponsablesRechazo.aspx", True) <> 0 _
            And String.Compare(Request.Url.Segments.LastOrDefault, "DocumentoBBDD.aspx", True) <> 0 Then
            '------------------------------------------------------
            'Codigo para el acceso a la aplicacion Sin Login.
            '------------------------------------------------------
            If PageBase.EsExtranet(Request) Then Recurso = System.Configuration.ConfigurationManager.AppSettings.Get("RecursoWeb_Extranet")

            'log.Info("Request.UrlReferrer:" & If(Request Is Nothing OrElse Request.UrlReferrer Is Nothing, String.Empty, Request.UrlReferrer.ToString))
            'log.Info("Request.Url:" & If(Request Is Nothing OrElse Request.Url Is Nothing, String.Empty, Request.Url.ToString))

            If String.IsNullOrWhiteSpace(IdSession) Then
                myTicket = lg.Login(User.Identity.Name)
            Else
                myTicket = lg.getTicket(IdSession)
            End If

            'myTicket = lg.Login("batznt\pablofernandez")

            If Not ConfigurationManager.AppSettings.Get("CurrentStatus").ToLower.Equals("live") Then
                myTicket = lg.Login(User.Identity.Name) : Session("PerfilUsuario") = PageBase.Perfil.Administrador
                'myTicket.Culture = "zh-CN"
                'myTicket = lg.Login("batznt\operez")
                'myTicket = lg.Login("araluce\jmanon")
                'myTicket = lg.Login("batznt\pablofernandez")
                'myTicket = lg.Login("batznt\jgainza")

                Dim FiltroGTK As New gtkFiltro
                FiltroGTK.TipoIncidencia = 1
                FiltroGTK.IdPlantaSAB = (From idB As String In My.Settings.IdTipoIncidencia_IdPlantaSAB
                                         Select IdBT = New Tuple(Of String, String)(idB.Split(";")(0), idB.Split(";")(1))
                                         Where IdBT.Item1 = FiltroGTK.TipoIncidencia.Value Select IdBT.Item2).SingleOrDefault
                Session("FiltroGTK") = FiltroGTK
            End If

            If Not myTicket Is Nothing Then
                Session("Ticket") = myTicket
                If lg.AccesoRecursoValido(myTicket, Recurso) Then
                    'Login correcto
                Else
                    log.Info(String.Format("Permiso denegado. Sin acceso al recurso ({0})." _
                                            & vbNewLine & "Usuario: {1}" _
                                            , Recurso, myTicket.IdUser))
                    Response.Redirect(PageBase.PAG_PERMISODENEGADO)
                End If
            Else
                log.Info("Permiso denegado. Sin Login.")
                Response.Redirect(PageBase.PAG_PERMISODENEGADO)
            End If
            '------------------------------------------------------
        End If
    End Sub

    'Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
    '	' Se desencadena al comienzo de cada solicitud		
    'End Sub

    'Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
    '	' Se desencadena al intentar autenticar el uso
    'End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena cuando se produce un error
        If Server.GetLastError IsNot Nothing Then log.Error(DirectCast(sender, System.Web.HttpApplication).Request.Url.ToString, Server.GetLastError)
    End Sub

    Private Sub Global_asax_AcquireRequestState(sender As Object, e As EventArgs) Handles Me.AcquireRequestState
        '----------------------------------------------------------------------------------------------------
        'Acceso Extranet
        'Corregimos el error de una posible sesion abierta para que vuelva a pasar por el Global.asax
        '----------------------------------------------------------------------------------------------------
        Dim IdSession As String = Request("IdSession")
        If Not String.IsNullOrWhiteSpace(IdSession) Then
            Dim myTicket As SabLib.ELL.Ticket = lg.getTicket(IdSession)
            If Session("Ticket") IsNot Nothing And myTicket IsNot Nothing Then
                Dim sTicket As SabLib.ELL.Ticket = Session("Ticket")
                If myTicket.IdUser <> sTicket.IdUser Then
                    lg.SetTicketEnBD(New SabLib.ELL.Ticket With {.IdUser = myTicket.IdUser, .IdSession = Session.SessionID})
                    Dim Url As String = String.Empty
                    If Request.QueryString.HasKeys Then
                        Dim qs As String = String.Empty
                        For Each item In Request.QueryString
                            If Not String.IsNullOrWhiteSpace(qs) Then qs &= "&"
                            If item = "IdSession" Then
                                qs &= "IdSession" & "=" & Session.SessionID
                            Else
                                qs &= item & "=" & Request.QueryString(item)
                            End If
                        Next
                        Url = Request.Url.LocalPath & "?" & qs
                    End If

                    Session.Clear()
                    Session.Abandon()

                    Response.Redirect(Url)
                End If
            End If
        End If
        '----------------------------------------------------------------------------------------------------
    End Sub

    Public Shared Function LNumeroPedidoOrigen(ByRef BBDD As BatzBBDD.Entities_Gertakariak, ByVal IDPROVEEDOR As Integer, ByRef lOF As List(Of BatzBBDD.OFMARCA)) As IEnumerable(Of ListItem)
        '------------------------------------------------------------------------------------------------
        ''--------------------------------------------------------------------
        ''Aligera la carga de entidades.
        ''--------------------------------------------------------------------
        'BBDD.GCLINPED.MergeOption = Objects.MergeOption.NoTracking
        'BBDD.SCPEDLIN.MergeOption = Objects.MergeOption.NoTracking
        'BBDD.OFMARCA.MergeOption = Objects.MergeOption.NoTracking
        ''--------------------------------------------------------------------
        'Dim lNumPed_Materiales = From GCLINPED As BatzBBDD.GCLINPED In BBDD.GCLINPED.AsEnumerable
        '                         Join OFM As BatzBBDD.OFMARCA In lOF On GCLINPED.NUMORDF Equals OFM.NUMOF And GCLINPED.NUMOPE Equals OFM.OP
        '                         Where CInt(GCLINPED.CODPROLIN) = IDPROVEEDOR And If(String.IsNullOrWhiteSpace(OFM.MARCA), True = True, OFM.MARCA = GCLINPED.NUMMAR)
        '                         Select GCLINPED.NUMPEDLIN Distinct
        'Dim lNumPed_SubContratacion = From SCPEDLIN As BatzBBDD.SCPEDLIN In BBDD.SCPEDLIN.AsEnumerable
        '                              Join OFM As BatzBBDD.OFMARCA In lOF On SCPEDLIN.NUMORDLIN Equals OFM.NUMOF And SCPEDLIN.NUMOPELIN Equals OFM.OP
        '                              Where CInt(SCPEDLIN.SCPEDCAB.CODPROEXT) = IDPROVEEDOR
        '                              Select SCPEDLIN.NUMPEDLIN Distinct
        'Return (From Reg_NUMPEDLIN In lNumPed_Materiales Select New ListItem(Reg_NUMPEDLIN & " (M)", Reg_NUMPEDLIN)).Union(From Reg_NUMPEDLIN In lNumPed_SubContratacion Select New ListItem(Reg_NUMPEDLIN & " (S)", Reg_NUMPEDLIN)).Distinct
        '------------------------------------------------------------------------------------------------
        '------------------------------------------------------------------------------------------------
        'Dividimos las consultas para agilizar la carga
        '------------------------------------------------------------------------------------------------
        Dim lGCLINPED = From GCLINPED As BatzBBDD.GCLINPED In BBDD.GCLINPED
                        Where CInt(GCLINPED.CODPROLIN) = IDPROVEEDOR
                        Select GCLINPED.NUMPEDLIN, GCLINPED.NUMORDF, GCLINPED.NUMOPE, GCLINPED.NUMMAR Distinct
        Dim lNumPed_Materiales = From GCLINPED In lGCLINPED.AsEnumerable
                                 Join OFM As BatzBBDD.OFMARCA In lOF On GCLINPED.NUMORDF Equals OFM.NUMOF And GCLINPED.NUMOPE Equals OFM.OP
                                 Where If(String.IsNullOrWhiteSpace(OFM.MARCA), True = True, OFM.MARCA = GCLINPED.NUMMAR)
                                 Select GCLINPED.NUMPEDLIN Distinct

        Dim lSCPEDLIN = From SCPEDLIN As BatzBBDD.SCPEDLIN In BBDD.SCPEDLIN Where CInt(SCPEDLIN.SCPEDCAB.CODPROEXT) = IDPROVEEDOR Select SCPEDLIN.NUMPEDLIN, SCPEDLIN.NUMORDLIN, SCPEDLIN.NUMOPELIN Distinct
        Dim lNumPed_SubContratacion = From SCPEDLIN In lSCPEDLIN.AsEnumerable
                                      Join OFM As BatzBBDD.OFMARCA In lOF On SCPEDLIN.NUMORDLIN Equals OFM.NUMOF And SCPEDLIN.NUMOPELIN Equals OFM.OP
                                      Select SCPEDLIN.NUMPEDLIN Distinct
        '------------------------------------------------------------------------------------------------
        Return (From Reg_NUMPEDLIN In lNumPed_Materiales Select New ListItem(Reg_NUMPEDLIN & " (M)", Reg_NUMPEDLIN)).Union(From Reg_NUMPEDLIN In lNumPed_SubContratacion Select New ListItem(Reg_NUMPEDLIN & " (S)", Reg_NUMPEDLIN)).Distinct
    End Function

    'TODO:Revisar esta caracteristica.
    'https://docs.microsoft.com/es-es/dotnet/api/system.flagsattribute?view=netframework-4.6.2
    '<Flags()> Public Enum Permisos As Integer
    ''' <summary>
    ''' Permisos de la aplicacion    
    ''' </summary>
    <Flags()> Public Enum Permisos As Integer
        <Description("Gestionar Roles y Usuaios")> Gestionar_Roles_Usuaios = 1 'Bin: 1
        <Description("Permitir tramitar No Conformidad")> TramitarNC = 2 'Bin: 10
        '<Description("Asignar y editar objetivos PPM por proveedor")> ObjetivoPPM = 4 'Bin: 100
        '<Description("Cargar y verificar certificados")> CargarCertificados = 8 'Bin: 1000
        '<Description("Cargar auditorias")> CargarAuditorias = 16 'Bin: 10000
        '<Description("Acceso a 'Evaluacion de Calidad'")> Evaluacio_Calidad = 32 'Bin: 100000
        '<Description("Acceso a 'Evaluacion de Servicio'")> Evaluacio_Servicio = 64 'Bin: 1000000
        '<Description("Acceso a 'Evaluacion de coste'")> Evaluacio_Coste = 128 'Bin: 10000000
        '<Description("Gestionar Roles y Usuaios")> Gestionar_Roles_Usuaios = 256 'Bin: 100000000
    End Enum
End Class

''' <summary>
''' Clase de la que deben heradar todas las paginas ASPX.
''' </summary>
''' <remarks></remarks>
Public Class PageBase
    Inherits Page

#Region "Variables compartidas"
    Public ItzultzaileWeb As New TraduccionesLib.itzultzaile
    Public Shared PAG_PERMISODENEGADO As String = "~/PermisoDenegado.aspx"
    Public Shared Log As log4net.ILog = Global_asax.log

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

    Public Property FiltroGTK() As gtkFiltro
        Get
            If (Session("FiltroGTK") Is Nothing) Then Session("FiltroGTK") = New gtkFiltro
            Return CType(Session("FiltroGTK"), gtkFiltro)
        End Get
        Set(value As gtkFiltro)
            Session("FiltroGTK") = value
        End Set
    End Property
    ''' <summary>
    ''' Usar Equals() para comparar con una enumeracion porque 0 = Nothing
    '''Ejm.: PerfilUsuario.Equals(Perfil.Administrador)
    ''' </summary>
    ''' <returns></returns>
    Public Property PerfilUsuario() As Perfil?
        Get
            If (Session("PerfilUsuario") Is Nothing) Then
                Return New Nullable(Of Perfil)
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
        Gestor = 1
        Consultor = 2
    End Enum

    ''' <summary>
    ''' Null = En Proceso, 0 = Pendiente de Aprobación, 1 = Aprobado, -1 = Rechazado
    ''' </summary>
    Enum EstadoEtapa
        EnProceso = Nothing
        PendienteAprobacion = 0
        Aprobado = 1
        Rechazado = -1
    End Enum

    Enum AsistenteReunionPreliminar
        Coordinador_Fabricacion = 0
        Calidad_Fabricacion = 1
        Calidad_proveedores = 2
        Calidad_Cliente = 3
        Almacen = 4
        Ingenieria_Fabricacion = 5
        Otros = 6
        Gestor = 7
        Ajuste = 8
        Seguimiento = 9
        Medicion = 10
        Homologacion = 11
    End Enum

    ''' <summary>
    ''' Identificador del grupo al que pertenece el recurso dependiendo de la planta.
    ''' </summary>
    ''' <returns></returns>
    ReadOnly Property IdGrupoRecurso As Integer
        Get
            Try
                Return (From Reg As String In My.Settings.IdTipoIncidencia_IdPlantaSAB Where Split(Reg, ";")(0) = FiltroGTK.TipoIncidencia Select Split(Reg, ";")(3)).SingleOrDefault
            Catch ex As ApplicationException
                Log.Debug(ex)
                Throw
            Catch ex As Exception
                Log.Error(ex)
                Throw
            End Try
        End Get
    End Property

    ''' <summary>
    ''' Perseguidor de la NC
    ''' </summary>
    Private _PerseguidorNC As Nullable(Of Boolean)
    ''' <summary>
    ''' Perseguidor de la NC
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property PerseguidorNC(ByVal Incidencia As BatzBBDD.GERTAKARIAK) As Boolean
        Get
            If _PerseguidorNC Is Nothing Then fRolNC(Incidencia, Ticket)
            Return _PerseguidorNC
        End Get
    End Property
    Private _CreadorNC As Nullable(Of Boolean)
    ''' <summary>
    ''' Perseguidor de la NC
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property CreadorNC(ByVal Incidencia As BatzBBDD.GERTAKARIAK) As Boolean
        Get
            If _CreadorNC Is Nothing Then fRolNC(Incidencia, Ticket)
            Return _CreadorNC
        End Get
    End Property

    ''' <summary>
    ''' Rol del usuario en curso
    ''' </summary>
    ''' <returns></returns>
    Public Property ROL_Usuario() As Integer
        Get
            If Session("ROL_Usuario") Is Nothing Then Session("ROL_Usuario") = 0
            Return Session("ROL_Usuario")
        End Get
        Set(value As Integer)
            Session("ROL_Usuario") = value
        End Set
    End Property
    ''' <summary>
    ''' Rol que acumula todos los permisos.
    ''' Se considera el administrador.
    ''' </summary>
    ''' <returns></returns>
    Public Property ROL_Admin() As Integer
        Get
            If Session("ROL_Admin") Is Nothing Then Session("ROL_Admin") = 0
            Return Session("ROL_Admin")
        End Get
        Set(value As Integer)
            Session("ROL_Admin") = value
        End Set
    End Property
#End Region

#Region "Eventos de Pagina"
    '#If DEBUG Then
    '	Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
    '		Log.Info("MapPath('~'): " & MapPath("~"))
    '		'Log.Info("MapPath('.'): " & MapPath("."))
    '		'Log.Info("MapPath('..'):" & MapPath(".."))
    '		'Log.Info("MapPath('/SBatz'): " & MapPath("/SBatz"))
    '		'Log.Info("MapPath('\SBatz'): " & MapPath("\SBatz"))
    '		Log.Info("PhysicalApplicationPath: " & Request.PhysicalApplicationPath)

    '		Dim aRutaAplicaciones As String() = MapPath(".").Split("\")

    '		Dim KaPlan_WebConfig As String = "\KaPlan\Web\Web.config"
    '		If String.Compare(Server.MachineName, "staarrondo", True) = 0 Then KaPlan_WebConfig = "\KaPlan\KaPlanWeb\Web.config"

    '		Log.Info(String.Join("\", aRutaAplicaciones.Take(aRutaAplicaciones.Length - 3)) & If(String.Compare(Server.MachineName, "staarrondo", True) = 0, "\KaPlan\KaPlanWeb\Web.config", "\KaPlan\Web\Web.config"))



    '		'-------------------------------------------------------------------------------------
    '		'Recuperamos las conexiones del KAPLAN.
    '		'-------------------------------------------------------------------------------------
    '		'Dim xmlDoc As New System.Xml.XmlDocument
    '		'Dim m_nodelist As System.Xml.XmlNodeList
    '		'Dim aRutaAplicaciones As String() = MapPath("~").Split("\")
    '		'xmlDoc.Load(String.Join("\", aRutaAplicaciones.Take(aRutaAplicaciones.Length - 3)) & "\KaPlan\KaPlanWeb\Web.config")
    '		'm_nodelist = xmlDoc.SelectNodes("configuration/connectionStrings/add")
    '		'For Each m_node As System.Xml.XmlNode In m_nodelist
    '		'	m_node = m_node
    '		'	If String.Compare(m_node.Attributes("name").Value, "KAPLAN_TEST") = 0 Then
    '		'		Dim cn As String = m_node.Attributes("connectionString").Value
    '		'		cn = cn
    '		'	End If
    '		'Next
    '		'-------------------------------------------------------------------------------------
    '	End Sub
    '#End If
    Private Sub Page_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Try
            '----------------------------------------------------------------------------------------------
            'Comprobamos que la sesion siga activa
            '----------------------------------------------------------------------------------------------
            If HttpContext.Current IsNot Nothing AndAlso HttpContext.Current.Session IsNot Nothing Then
                If (String.Compare(Request.Url.Segments.LastOrDefault, "Index.aspx", True) <> 0) _
                    And String.Compare(Request.Url.Segments.LastOrDefault, "PermisoDenegado.aspx", True) <> 0 _
                    And String.Compare(Request.Url.Segments.LastOrDefault, "PlantillaAviso.aspx", True) <> 0 _
                    And String.Compare(Request.Url.Segments.LastOrDefault, "Responsables.aspx", True) <> 0 _
                    And String.Compare(Request.Url.Segments.LastOrDefault, "ResponsablesRechazo.aspx", True) <> 0 _
                    And String.Compare(Request.Url.Segments.LastOrDefault, "DocumentoBBDD.aspx", True) <> 0 Then
                    Dim FiltroGTK As gtkFiltro = Session("FiltroGTK")
                    If FiltroGTK Is Nothing Then
                        Throw New ApplicationException("Sesion Caducada")
                    ElseIf FiltroGTK.TipoIncidencia Is Nothing Then
                        Throw New ApplicationException("FiltroGTK.TipoIncidencia: 'ES NULO'")
                    End If
                End If
            End If
            '----------------------------------------------------------------------------------------------
        Catch ex As ApplicationException
            Log.Debug(ex)
            Response.Redirect(PageBase.PAG_PERMISODENEGADO & "?msg=" & ex.Message, True)
        Catch ex As Exception
            Log.Error(ex)
            Response.Redirect("~/Index.aspx", True)
        End Try
    End Sub
    Private Sub Page_Error(sender As Object, e As EventArgs) Handles Me.Error
        'If Server.GetLastError IsNot Nothing Then Log.Error(Server.GetLastError.ToString, Server.GetLastError)
        '-------------------------------------------------------------------------------------------
        If Server.GetLastError IsNot Nothing Then
            Dim ex As Exception = Server.GetLastError
            Log.Error(ex.ToString, ex)
            If TypeOf ex Is HttpException Then Response.Redirect(PAG_PERMISODENEGADO & "?msg=" & If(ex.Message.IndexOf("(") < 0, ex.Message, ex.Message.Substring(0, ex.Message.IndexOf("("))))
        End If
        '-------------------------------------------------------------------------------------------
    End Sub
    Private Sub Page_PreRenderComplete(sender As Object, e As EventArgs) Handles Me.PreRenderComplete
        ItzultzaileWeb.TraducirWebControls(Page.Controls)
    End Sub
#End Region

#Region "Funciones y Procesos"
    ''' <summary>
    ''' Sobrecarga de "InitializeCulture" para cambiar la cultura.
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overrides Sub InitializeCulture()
        If Ticket Is Nothing Then
            Dim cultureInfo As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture("es-ES")
            System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo
            Culture = cultureInfo.Name
        Else
            Culture = Ticket.Culture
        End If
        MyBase.InitializeCulture()
    End Sub

    ''' <summary>
    ''' Devuelve la ruta (ImageUrl) de la imagen para el tipo de archivo correspondiente.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function ImagenDocumento(CONTENT_TYPE As String) As String
        If String.Compare("image/pjpeg", CONTENT_TYPE, True) = 0 _
            Or String.Compare("image/jpeg", CONTENT_TYPE, True) = 0 Then
            ImagenDocumento = "~/App_Themes/Batz/Imagenes/TipoArchivos/jpeg-icon32.png"
        ElseIf String.Compare("image/x-png", CONTENT_TYPE, True) = 0 Or String.Compare("image/png", CONTENT_TYPE, True) = 0 Then
            ImagenDocumento = "~/App_Themes/Batz/Imagenes/TipoArchivos/png-icon32.png"
        ElseIf String.Compare("image/bmp", CONTENT_TYPE, True) = 0 Then
            ImagenDocumento = "~/App_Themes/Batz/Imagenes/TipoArchivos/bmp-icon32.png"
        ElseIf String.Compare("text/plain", CONTENT_TYPE, True) = 0 _
            Or String.Compare("text/richtext", CONTENT_TYPE, True) = 0 Then
            ImagenDocumento = "~/App_Themes/Batz/Imagenes/TipoArchivos/text-icon32.png"
        ElseIf String.Compare("application/msword", CONTENT_TYPE, True) = 0 _
            Or String.Compare("application/vnd.openxmlformats-officedocument.wordprocessingml.document", CONTENT_TYPE, True) = 0 Then
            ImagenDocumento = "~/App_Themes/Batz/Imagenes/TipoArchivos/docx-win-icon32.png"
        ElseIf String.Compare("application/vnd.sealed-xls", CONTENT_TYPE, True) = 0 _
            Or String.Compare("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", CONTENT_TYPE, True) = 0 _
            Or String.Compare("application/vnd.ms-excel", CONTENT_TYPE, True) = 0 Then
            ImagenDocumento = "~/App_Themes/Batz/Imagenes/TipoArchivos/xlsx-win-icon32.png"
        ElseIf String.Compare("application/pdf", CONTENT_TYPE, True) = 0 Then
            ImagenDocumento = "~/App_Themes/Batz/Imagenes/TipoArchivos/pdf-icon32.png"
        ElseIf String.Compare("application/octet-stream", CONTENT_TYPE, True) = 0 Then
            ImagenDocumento = "~/App_Themes/Batz/Imagenes/TipoArchivos/Mail-icon32.png"
        ElseIf String.Compare("application/vnd.openxmlformats-officedocument.presentationml.presentation", CONTENT_TYPE, True) = 0 Then
            ImagenDocumento = "~/App_Themes/Batz/Imagenes/TipoArchivos/pptx-win-icon32.png"
        Else
            ImagenDocumento = "~/App_Themes/Batz/Imagenes/TipoArchivos/Document-Blank-icon32.png"
            Log.Debug("Falta icono para el tipo de archivo: " & CONTENT_TYPE)
        End If
        Return ImagenDocumento
    End Function

    ''' <summary>
    ''' Abre los informes para la ruta marcada.
    ''' Si el informe se abre por 2ª vez, sin que se cierre, coge el foco.
    ''' </summary>
    ''' <param name="url"></param>
    ''' <remarks></remarks>
    Public Sub AbrirInformeCognos(ByVal url As String)
        Dim Script As String = "var InformeCognos; "
        Script &= "function abrirVentana(){InformeCognos = window.open('" & url & "','Informe');}; "
        Script &= "function cerrarVentana(){if (InformeCognos != undefined){InformeCognos.close();}; }; "
        Script &= "cerrarVentana();abrirVentana();"
        '---------------------------------------------------------------------------------------------------
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "informe", Script, True)
    End Sub

    ''' <summary>
    ''' Funcion que incluye el tipo de NC (NCI, NCP, NCPP) concatenado al identificador.
    ''' </summary>
    ''' <param name="Incidencia">Incidencia</param>
    ''' <returns>NCI-1234, NCP-1234, NCPP-1234</returns>
    ''' <remarks></remarks>
    Public Function CodigoNC(ByRef Incidencia As BatzBBDD.GERTAKARIAK) As String
        CodigoNC = "???"
        If Incidencia.PROCEDENCIANC IsNot Nothing Then
            If Incidencia.PROCEDENCIANC = 1 Then
                CodigoNC = "NCIT" '"Interna (Torrea)"
            ElseIf Incidencia.PROCEDENCIANC = 2 Then
                CodigoNC = "NCP" '"A proveedor"
            ElseIf Incidencia.PROCEDENCIANC = 3 Then
                CodigoNC = "NCPP" '"A planta Batz"
            ElseIf Incidencia.PROCEDENCIANC = 4 Then
                CodigoNC = "NCIA" '"Interna (Araluce)"
            End If
        End If
        Return CodigoNC & "-" & Incidencia.ID
    End Function

    Sub fRolNC(ByRef Incidencia As BatzBBDD.GERTAKARIAK, ByVal Ticket As SabLib.ELL.Ticket)
        If Incidencia Is Nothing Then
            _CreadorNC = False
            _PerseguidorNC = False
        Else
            Dim lResponsables As IEnumerable(Of BatzBBDD.RESPONSABLES_GERTAKARIAK) = From Resp As BatzBBDD.RESPONSABLES_GERTAKARIAK In Incidencia.RESPONSABLES_GERTAKARIAK Select Resp
            _PerseguidorNC = lResponsables.Where(Function(Reg) Reg.IDUSUARIO = Ticket.IdUser).Any
            _CreadorNC = (Incidencia.IDCREADOR IsNot Nothing AndAlso Incidencia.IDCREADOR.Value.Equals(Ticket.IdUser))
        End If
    End Sub
    ''' <summary>
    ''' Funcion para comprobar si la aplicacion se esta ejecutando en la Extranet.
    ''' </summary>
    ''' <param name="HttpRequest"></param>
    ''' <returns></returns>
    Public Shared Function EsExtranet(ByRef HttpRequest As HttpRequest) As Boolean
        EsExtranet = (HttpRequest.Url.Host.IndexOf("Extranet", StringComparison.CurrentCultureIgnoreCase) >= 0)
        Return EsExtranet
    End Function

    '''' <summary>
    '''' Identificamos si el usuario esta en la Extranet.
    '''' </summary>
    '''' <returns></returns>
    'Function EsExtranet() As Boolean
    '    Return New Regex(SabLib.BLL.Utils.TextoLike("Extranet"), RegexOptions.IgnoreCase).IsMatch(Request.ServerVariables("HTTP_HOST"))
    'End Function

    ''' <summary>
    ''' Funcion para comprobar que la suma de las lineas de coste sea igual que el total acordado de la NC. 
    ''' TODO: no le veo sentido, quitar? Si lo quitamos y editamos NCs viejas, se cambiará el coste, igual es por eso?
    ''' </summary>
    ''' <param name="Incidencia"></param>
    ''' <returns></returns>
    Function TotalAcordado_LineasCoste(ByRef Incidencia As BatzBBDD.GERTAKARIAK) As Boolean
        Dim Total_LC As Decimal = 0
        Dim Total_NC As Decimal = If(Incidencia.TOTALACORDADO Is Nothing, 0, Incidencia.TOTALACORDADO)
        If Incidencia.LINEASCOSTE.Any Then Total_LC = Incidencia.LINEASCOSTE.Sum(Function(o) o.IMPORTE)
        Return (Total_NC = Total_LC)
    End Function

#End Region
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

    Private _OF As String

    Private _OP As String

    ''' <summary>
    ''' Estado de las No Conformidades.
    ''' </summary>
    Private _Estado As Nullable(Of EstadoIncidencia)
    ''' <summary>
    ''' Identificadores de los responsables de las No Conformidades y Acciones.
    ''' </summary>
    Private _Responsables As List(Of Integer)
    ''' <summary>
    ''' Identificadores de los responsables de las No Conformidades y Acciones.
    ''' </summary>
    Private _Caracteristicas As List(Of Integer)
    ''' <summary>
    ''' Identificadores de los responsables de las No Conformidades y Acciones.
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
    Private _IdPlantaSAB As Nullable(Of Integer)
    Private _Capacidades As List(Of String)

    ''' <summary>
    ''' Identificador del Cliente
    ''' </summary>
    ''' <remarks></remarks>
    Private _Cliente As Nullable(Of Integer)
    ''' <summary>
    ''' Identificador del proyecto
    ''' </summary>
    ''' <remarks></remarks>
    Private _Proyecto As Nullable(Of Integer)
    ''' <summary>
    ''' Indicamos si se debe cargar la Cookie del Filtro.
    ''' </summary>
    ''' <remarks></remarks>
    Private _CargarCookie As Boolean = True
    ''' <summary>
    ''' Identificador de la "Unidad de Negicio" (UG - Lantegi)
    ''' </summary>
    Private _UG As Nullable(Of Integer)
    ''' <summary>
    ''' Identificadores de proveedor
    ''' </summary>
    Private _Proveedores As List(Of Integer)


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


    Public Property OFstring As String
        Get
            Return _OF
        End Get
        Set(ByVal value As String)
            _OF = value
        End Set
    End Property

    Public Property OPstring As String
        Get
            Return _OP
        End Get
        Set(ByVal value As String)
            _OP = value
        End Set
    End Property

    ''' <summary>
    ''' Estado de las No Conformidades.
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
    ''' Identificadores de los responsables de las No Conformidades y Acciones.
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
    ''' Identificadores de los responsables de las No Conformidades y Acciones.
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
    ''' 9-Sistemas de Automocion (Igorre)
    ''' 11-Sistemas de Automocion (Otra Planta)
    ''' 15-Aero Space
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
    ''' Identificador del codigo de planta en SAB.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdPlantaSAB As Nullable(Of Integer)
        Get
            Return _IdPlantaSAB
        End Get
        Set(value As Nullable(Of Integer))
            _IdPlantaSAB = value
        End Set
    End Property

    ''' <summary>
    ''' Identificador del cliente.
    ''' </summary>
    ''' <returns></returns>
    Public Property Cliente As Nullable(Of Integer)
        Get
            Return _Cliente
        End Get
        Set(value As Nullable(Of Integer))
            _Cliente = value
        End Set
    End Property
    ''' <summary>
    ''' Identificador del proyecto
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Proyecto As Nullable(Of Integer)
        Get
            Return _Proyecto
        End Get
        Set(value As Nullable(Of Integer))
            _Proyecto = value
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

    '---------------------------------------------------------------------
    'TODO: Usar una enumeracion para el tipo de Procedencia en toda la aplicacion.
    'https://docs.microsoft.com/es-es/dotnet/visual-basic/language-reference/statements/enum-statement
    'https://docs.microsoft.com/es-es/dotnet/api/system.flagsattribute?view=netframework-4.6.2
    '---------------------------------------------------------------------
    '''' <summary>
    '''' Indicamos la procedencia del Incidenia.
    '''' </summary>
    '''' <remarks></remarks>
    'Public Enum ProcedenciaNC
    '    Interna = 1
    '    Externa = 2
    '    Cliente = 3
    'End Enum
    'Public Enum ProcedenciaNC As Integer
    '    <Description("Interna (Torrea)"), Category(""), DesignerSerializationVisibility(True)> Interna_Torrea = 1
    '    <Description("Externa")> Externa = 2
    '    <Description("Cliente")> Cliente = 3
    '    <Description("Interna (Araluce)")> Interna_Araluce = 4
    'End Enum
    '---------------------------------------------------------------------

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
    ''' <summary>
    ''' Identificador de la "Unidad de Negicio" (UG - Lantegi)
    ''' </summary>
    ''' <returns></returns>
    Public Property UG As Nullable(Of Integer)
        Get
            Return _UG
        End Get
        Set(value As Nullable(Of Integer))
            _UG = value
        End Set
    End Property
    ''' <summary>
    ''' Identificadores de proveedor
    ''' </summary>
    ''' <returns></returns>
    Public Property Proveedores As List(Of Integer)
        Get
            If _Proveedores Is Nothing Then
                _Proveedores = New List(Of Integer)
            End If
            Return _Proveedores
        End Get
        Set(ByVal value As List(Of Integer))
            _Proveedores = value
        End Set
    End Property
    ''' <summary>
    ''' Identificadores de capacidades
    ''' </summary>
    ''' <returns></returns>
    Public Property Capacidades As List(Of String)
        Get
            If _Capacidades Is Nothing Then
                _Capacidades = New List(Of String)
            End If
            Return _Capacidades
        End Get
        Set(value As List(Of String))
            _Capacidades = value
        End Set
    End Property
End Class

Public Class gtkGridView
    ''' <summary>
    ''' Identificador del Registro Seleccionado.
    ''' </summary>
    Private _IdSeleccionado As Nullable(Of Integer)
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
    Public Property IdSeleccionado() As Nullable(Of Integer)
        Get
            Return _IdSeleccionado
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _IdSeleccionado = value
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