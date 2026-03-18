Imports System.Globalization.CultureInfo

Public Class PageBase
    Inherits Page

#Region "Variables compartidas"

    Public Shared STICKET As String = "Ticket"
    Public Shared SCULTURA As String = "MiCultura"
    Public Shared RECURSO_TELEFONIA As String = "RecursoTelefonia"
    Public Shared PAG_PERMISODENEGADO As String = "~/PermisoDenegado.aspx"
    Public Shared PAG_REFRESCARSESSION As String = "~/RefrescarSession.aspx"
    Public Shared PAG_INICIO As String = "~/Default.aspx"
    Public Shared PAG_ADMINISTRACION As String = "~/Mantenimientos/DefaultAdm.aspx"
    Public Shared PAG_INTRANET As String = "http://intranet2.batz.es"
    Public Shared HOME As String = "home"
    Public Shared itzultzaileWeb As New TraduccionesLib.itzultzaile
    Public Shared log As log4net.ILog = log4net.LogManager.GetLogger("root.Telefonia")

#End Region

#Region "Ticket"

    ''' <summary>
    ''' Obtiene el ticket de la session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Private Property Ticket() As TelefoniaLib.ELL.TicketTlfno
        Get
            If (Session(STICKET) Is Nothing) Then
                Return Nothing
            Else
                Return CType(Session(STICKET), TelefoniaLib.ELL.TicketTlfno)
            End If
        End Get
        Set(ByVal value As TelefoniaLib.ELL.TicketTlfno)
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
            Response.Redirect(PAG_REFRESCARSESSION)
        Else
            'hay que comprobar que siempre que se cargue una pagina que este en la carpeta mantenimiento, solo puedan acceder administradores, adm de plantas y gestores
            'y luego entre estos, capar algunas paginas            
            Dim segmentos As String() = Page.Request.Url.Segments
            Dim pag As String = segmentos(Page.Request.Url.Segments.GetUpperBound(0))
            'Si proviene del portal del empleado, solo podra acceder a la pagina de FacturacionPersona.aspx
            If (Ticket.ProvienePortalEmpleado) Then
                If (Not pag = "FacturacionPersona.aspx") Then
                    Response.Redirect(PAG_INTRANET)
                End If
            Else
                If (segmentos.Count > 2) Then
                    If (segmentos(1) = "Mantenimientos/") Then
                        If Not (Ticket.EsAdministrador OrElse Ticket.EsAdministradorPlanta) Then
                            Response.Redirect(PAG_PERMISODENEGADO)
                        ElseIf (pag = "Administradores.aspx" AndAlso Not Ticket.EsAdministrador) Then   'A la pagina de administradores, solo podran entrar los administradores de la aplicacion
                            Response.Redirect(PAG_PERMISODENEGADO)
                        End If
                    End If
                End If
            End If
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


    Protected Overrides Sub InitializeCulture()
        Try
            If Session(SCULTURA) Is Nothing Then
                Dim cultureInfo As Globalization.CultureInfo = Globalization.CultureInfo.CreateSpecificCulture("es-ES")
                Threading.Thread.CurrentThread.CurrentCulture = cultureInfo
                Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo
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

#Region "Decimales"

    ''' <summary>
    ''' Dada una cultura y el formato de los decimales, devuelve el decimal con la coma o punto
    ''' </summary>
    ''' <param name="sDec">Numero a convertir</param>
    ''' <returns></returns>	
    Public Shared Function DecimalValue(ByVal sDec As String) As Decimal
        Dim myDec As String = String.Empty
        If (Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ",") Then
            myDec = sDec.Trim.Replace(".", ",")
        ElseIf (Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".") Then
            myDec = sDec.Trim.Replace(",", ".")
        End If
        Return Convert.ToDecimal(myDec, Threading.Thread.CurrentThread.CurrentCulture.NumberFormat)
    End Function

#End Region

End Class
