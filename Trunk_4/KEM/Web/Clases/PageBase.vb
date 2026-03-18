Imports System.Io
Imports System.Globalization.CultureInfo

Public Class PageBase
    Inherits Page

#Region "Variables compartidas"

    Public Shared STICKET As String = "Ticket"
    Public Shared SCULTURA As String = "MiCultura"
    Public Shared PLANTA As String = "Planta"
    Public Shared PLANTA_IGORRE As String = "PlantaIgorre"
    Public Shared RECURSO_KEM As String = "RecursoKEM"
    Public Shared PAG_PERMISODENEGADO As String = "~/PermisoDenegado.aspx"
    Public Shared PAG_REFRESCARSESSION As String = "~/RefrescarSession.aspx"
    Public Shared PAG_INICIO As String = "~/Usuarios/Usuarios.aspx"
    Public itzultzaileWeb As New itzultzaile
    Private Shared log As log4net.ILog = log4net.LogManager.GetLogger("root.Kem")

#End Region

#Region "Ticket"

    ''' <summary>
    ''' Obtiene el ticket de la session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Private Property Ticket() As SABLib.ELL.Ticket
        Get
            If (Session(STICKET) Is Nothing) Then
                Return Nothing
            Else
                Return CType(Session(STICKET), SABLib.ELL.Ticket)
            End If
        End Get
        Set(ByVal value As SABLib.ELL.Ticket)
            Session(STICKET) = value
        End Set
    End Property

#End Region

#Region "Page Load"

    ''' <summary>
    ''' <para>Si no tiene ticket, se redirecciona a una pagina de permiso denegado</para>
    ''' <para>Comprueba que esa pagina es accesible por el perfil</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (Session(STICKET) Is Nothing) Then
            Response.Redirect(PAG_REFRESCARSESSION, True)
        End If
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
            If (System.Web.HttpContext.Current.Session("Ticket") IsNot Nothing) Then
                myTicket = CType(System.Web.HttpContext.Current.Session("Ticket"), Sablib.ELL.Ticket)
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

#Region "Cultura"

    ''' <summary>
    ''' Inicializa la cultura
    ''' </summary>
    Public Sub inicializarCultura()
        MyBase.InitializeCulture()
    End Sub

    ''' <summary>
    ''' Inicializa la cultura
    ''' </summary>    
    Protected Overrides Sub InitializeCulture()
        Try
            If Session(SCULTURA) Is Nothing Then
                Dim cultureInfo As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture("es-ES")
                System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo
                Session(SCULTURA) = cultureInfo.Name
                Culture = cultureInfo.Name
            Else
                Culture = Session(SCULTURA)
            End If
            Ticket.Culture = CurrentCulture.Name
            MyBase.InitializeCulture()
        Catch
            Response.Redirect(PAG_PERMISODENEGADO, False)
        End Try
    End Sub

#End Region

End Class
