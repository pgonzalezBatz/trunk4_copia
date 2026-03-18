Public Class PageBase
    Inherits Page

#Region "Variables compartidas"

    Public Shared PAG_PERMISO_DENEGADO As String = "~/PermisoDenegado.aspx"
    Public itzultzaileWeb As New LocalizationLib.Itzultzaile
    Private Shared log As log4net.ILog = log4net.LogManager.GetLogger("root.Nominas")

#End Region

#Region "Ticket"

    ''' <summary>
    ''' Obtiene el ticket de la session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Private Property Ticket() As Sablib.ELL.Ticket
        Get
            If (Session("Ticket") Is Nothing) Then
                Return Nothing
            Else
                Return CType(Session("Ticket"), Sablib.ELL.Ticket)
            End If
        End Get
        Set(ByVal value As Sablib.ELL.Ticket)
            Session("Ticket") = value
        End Set
    End Property

#End Region

#Region "Page Load"

    ''' <summary>
    ''' <para>Si no tiene ticket, se vuelve a logear</para>    
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
#If DEBUG Then
        Dim lg As New SabLib.BLL.LoginComponent
        Dim userName As String = User.Identity.Name.ToLower
        Session("Ticket") = lg.Login(userName)
        Session("Ticket") = lg.Login("batznt\afernandez")
        Session("Rol") = "1"
#End If
        If (Ticket Is Nothing) Then
            WriteLog("El ticket es nothing, asi que se redirige a la pagina de permiso denegado", TipoLog.Warn)
            Response.Redirect(PAG_PERMISO_DENEGADO & "?mensa=El usuario no tiene acceso", True)
        Else
            Dim bSinAcceso As Boolean = False
            Dim segmentos As String() = Page.Request.Url.Segments
            Dim pag As String = segmentos(Page.Request.Url.Segments.GetUpperBound(0))
            If (segmentos.Count > 2) Then  'Se ha metido en una pagina de alguna carpeta
                Dim nameCarpeta As String = segmentos(1).Substring(0, segmentos(1).Length - 1).ToLower
                If (nameCarpeta = "admin" And (Session("Rol") Is Nothing OrElse CInt(Session("Rol")) <> NominasLib.Nomina.Roles.Admin)) Then bSinAcceso = True
            End If
#If DEBUG Then
            bSinAcceso = False
#End If
            If (bSinAcceso) Then Response.Redirect("~/PermisoDenegado.aspx", True)
        End If
    End Sub

#End Region

#Region "Cultura"

    ''' <summary>
    ''' Inicializa la cultura
    ''' </summary>
    Public Sub inicializarCultura()
        MyBase.InitializeCulture()
    End Sub

    ''' <summary>
    ''' Evento para asignar la cultura al ticket
    ''' </summary>	
    Protected Overrides Sub InitializeCulture()
        Try
            If Ticket Is Nothing Then
                Dim cultureInfo As Globalization.CultureInfo = Globalization.CultureInfo.CreateSpecificCulture("es-ES")
                Threading.Thread.CurrentThread.CurrentCulture = cultureInfo
                Culture = cultureInfo.Name
            Else
                Culture = Ticket.Culture
            End If
            MyBase.InitializeCulture()
        Catch ex As Exception
            WriteLog("Ha ocurrido un error al inicializar la cultura. Se redirige a la pagina de permiso denegado", TipoLog.Err, ex)
            Response.Redirect(PAG_PERMISO_DENEGADO & "?mensa=Ha ocurrido un error al inicializar la cultura. Contacte con el administrador", False)
        End Try
    End Sub

#End Region

#Region "log4net"

    ''' <summary>
    ''' Enumeracion con los valores posibles del log
    ''' </summary>
    Public Enum TipoLog As Integer
        Info = 0
        Err = 1
        Warn = 2
    End Enum

    ''' <summary>
    ''' Escribe en el log un texto
    ''' </summary>
    ''' <param name="texto">Texto a escribir</param>
    ''' <param name="ex">Excepcion lanzada en su caso</param>	
    Public Shared Sub WriteLog(ByVal texto As String, ByVal tipo As TipoLog, Optional ByVal ex As Exception = Nothing)
        Try
            Dim myTicket As Sablib.ELL.Ticket = Nothing
            If (HttpContext.Current.Session("Ticket") IsNot Nothing) Then
                myTicket = CType(HttpContext.Current.Session("Ticket"), Sablib.ELL.Ticket)
            End If
            If (myTicket IsNot Nothing) Then
                texto &= " [user]:" & myTicket.NombreUsuario
            End If
            If (tipo = TipoLog.Info) Then
                log.Info(texto)
            ElseIf (tipo = TipoLog.Err) Then
                log.Error(texto, ex)
            ElseIf (tipo = TipoLog.Warn) Then
                log.Warn(texto, ex)
            End If
        Catch
        End Try
    End Sub

#End Region

#Region "Directorio Virtual"

    ''' <summary>
    ''' Calcula el directorio virtual de la aplicacion. El problema es que en local, no funciona con IIS y en real si, entonces las rutas de las imagenes asignadas a controles html no funcionan
    ''' </summary>
    ''' <returns></returns>	
    Public Function CalcularDirectorioVirtual() As String
        Dim APPL_MD_PATH As String = Request.ServerVariables("APPL_MD_PATH").Split("/")(Request.ServerVariables("APPL_MD_PATH").Split("/").Length - 1).ToString()
        'Calculamos el nombre del directorio virtual.
        If APPL_MD_PATH <> String.Empty Then
            APPL_MD_PATH = "/" & APPL_MD_PATH & "/"
        Else
            APPL_MD_PATH = "/"   'Si no tiene directorio virtual, se le añade la barra para que acceda desde la raiz del servidor web
        End If
        Return APPL_MD_PATH
    End Function

#End Region

End Class
