Imports TelefoniaLib

Partial Public Class MPTelefoniaAdm
    Inherits MasterPage

#Region "Visualizacion de mensajes"

    Private m_Mensaje As String
    Private m_tipo As Integer
    Private m_traducido As Boolean

    Public Enum TipoMensaje As Integer
        _Info = 1
        _Advertencia = 2
        _Error = 3
    End Enum

    ''' <summary>
    ''' Mensaje de informacion a mostrar
    ''' </summary>
    ''' <value></value>	
    Public WriteOnly Property MensajeInfo() As String
        Set(ByVal value As String)
            ShowSuccessNotif(value)
        End Set
    End Property

    ''' <summary>
    ''' Mensaje de informacion a mostrar
    ''' </summary>
    ''' <value></value>	
    Public WriteOnly Property MensajeInfoKey() As String
        Set(ByVal value As String)
            ShowSuccessNotif(itzultzaileWeb.Itzuli(value))
        End Set
    End Property

    ''' <summary>
    ''' Mensaje de advertencia a mostrar
    ''' </summary>
    ''' <value></value>	
    Public WriteOnly Property MensajeAdvertencia() As String
        Set(ByVal value As String)
            ShowWarningNotif(value)
        End Set
    End Property

    ''' <summary>
    ''' Mensaje de advertencia a mostrar
    ''' </summary>
    ''' <value></value>	
    Public WriteOnly Property MensajeAdvertenciaKey() As String
        Set(ByVal value As String)
            ShowWarningNotif(itzultzaileWeb.Itzuli(value))
        End Set
    End Property

    ''' <summary>
    ''' Mensaje de error a mostrar
    ''' </summary>
    ''' <value></value>	
    Public WriteOnly Property MensajeError() As String
        Set(ByVal value As String)
            ShowErrorNotif(value)
        End Set
    End Property

    ''' <summary>
    ''' Mensaje de error a mostrar
    ''' </summary>
    ''' <value></value>	
    Public WriteOnly Property MensajeErrorKey() As String
        Set(ByVal value As String)
            ShowErrorNotif(itzultzaileWeb.Itzuli(value))
        End Set
    End Property

    ''' <summary>
    ''' Muestra una notificacion de tipo informacion
    ''' </summary>
    ''' <param name="mensa">Mensaje</param>
    ''' <param name="bStick">True si se queda visible hasta que se cierre manualmente</param>
    Public Sub ShowInfoNotif(ByVal mensa As String, Optional ByVal bStick As Boolean = True)
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "info", "showToast('" & mensa & "','notice'," & bStick.ToString.ToLower & ");", True)
    End Sub

    ''' <summary>
    ''' Muestra una notificacion de tipo accion realizada con exito
    ''' </summary>
    ''' <param name="mensa">Mensaje</param>
    ''' <param name="bStick">True si se queda visible hasta que se cierre manualmente</param>
    Public Sub ShowSuccessNotif(ByVal mensa As String, Optional ByVal bStick As Boolean = False)
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "info", "showToast('" & mensa & "','success'," & bStick.ToString.ToLower & ");", True)
    End Sub

    ''' <summary>
    ''' Muestra una notificacion de tipo advertencia
    ''' </summary>
    ''' <param name="mensa">Mensaje</param>
    ''' <param name="bStick">True si se queda visible hasta que se cierre manualmente</param>
    Public Sub ShowWarningNotif(ByVal mensa As String, Optional ByVal bStick As Boolean = True)
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "info", "showToast('" & mensa & "','warning'," & bStick.ToString.ToLower & ");", True)
    End Sub

    ''' <summary>
    ''' Muestra una notificacion de tipo error
    ''' </summary>
    ''' <param name="mensa">Mensaje</param>
    ''' <param name="bStick">True si se queda visible hasta que se cierre manualmente</param>
    Public Sub ShowErrorNotif(ByVal mensa As String, Optional ByVal bStick As Boolean = True)
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "info", "showToast('" & mensa & "','error'," & bStick.ToString.ToLower & ");", True)
    End Sub

    Public itzultzaileWeb As New TraduccionesLib.itzultzaile

#End Region

#Region "Properties"

    ''' <summary>
    ''' Visualiza u oculta el menu
    ''' </summary>
    ''' <value></value>
    Public WriteOnly Property VisualizarMenu() As Boolean
        Set(ByVal value As Boolean)
            pnlMenu.Visible = value
        End Set
    End Property

    ''' <summary>
    ''' Visualiza u oculta el boton de cerrar session
    ''' </summary>
    ''' <value></value>
    Public WriteOnly Property VisualizarCerrarSession() As Boolean
        Set(ByVal value As Boolean)
            pnlCerrarSession.Visible = value
        End Set
    End Property

    ''' <summary>
    ''' Visualiza u oculta la informacion sobre la planta de administracion
    ''' </summary>
    ''' <value></value>
    Public WriteOnly Property VisualizarPlantaAdm() As Boolean
        Set(ByVal value As Boolean)
            pnlPlantaAdm.Visible = value
        End Set
    End Property

#End Region

#Region "Datos del Ticket"

    ''' <summary>
    ''' Recupera el ticket del usuario
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public ReadOnly Property Ticket() As ELL.TicketTlfno
        Get
            Return CType(Session("Ticket"), ELL.TicketTlfno)
        End Get
    End Property

#End Region

#Region "Eventos de pagina"

    ''' <summary>
    ''' Establece la url de la hoja de estilo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Private Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init
        Dim server As String = "intranet2"
#If DEBUG Then
        server = "intranet-test"
#End If
        linkCSS.Href = "https://" & server & ".batz.es/BaliabideOrokorrak/estiloIntranet.css"
        If (Request.UserAgent.IndexOf("AppleWebKit") > 0) Then Request.Browser.Adapters.Clear()
    End Sub

    ''' <summary>
    ''' Carga la pagina maestra
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not (Page.IsPostBack) Then
            Dim cultureInfo As Globalization.CultureInfo = Globalization.CultureInfo.CreateSpecificCulture(Ticket.Culture)
            Threading.Thread.CurrentThread.CurrentCulture = cultureInfo
            Session(PageBase.SCULTURA) = Ticket.Culture
            Session("Status") = ConfigurationManager.AppSettings("CurrentStatus").ToLower
            lblPlantaAdm.Text = Ticket.Planta
        End If
        imgLogo.ToolTip = Server.MachineName & If(ConfigurationManager.AppSettings("CurrentStatus").ToLower = "debug", "-Debug", String.Empty)
    End Sub

    ''' <summary>
    ''' Configura el mensaje de error, advertencia o informacion a mostrar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Render(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        Dim tick As ELL.TicketTlfno = Ticket
        lblUsuario.Text = If(tick Is Nothing, String.Empty, HttpUtility.HtmlDecode("<b>") & tick.NombreCompleto & HttpUtility.HtmlDecode("</b>"))
        ltlFechaActual.Text = Now.ToLongDateString
        MenuDataBind()
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(ltlUsuario) : itzultzaileWeb.Itzuli(imgCerrarSession)
            itzultzaileWeb.Itzuli(labelPie) : itzultzaileWeb.Itzuli(labelPlanta)
        End If
    End Sub

    ''' <summary>
    ''' Funcion para traducir
    ''' </summary>
    ''' <param name="termino">Termino</param>
    ''' <returns></returns>    
    Protected Function Traducir(ByVal termino As String)
        Return itzultzaileWeb.Itzuli(termino)
    End Function

#End Region

#Region "Menu"

    ''' <summary>
    ''' Enlaza el menu y sino es administrador, no muestra la pestaña de administracion
    ''' </summary>
    Private Sub MenuDataBind()        
        Try
            XmlDataSource.DataFile = "~/App_Data/menuAdm.xml"
            XmlDataSource.XPath = "Root/Item"
            menu.DataBind()
        Catch 'puede fallar porque no exista el xml del idioma de la cultura
        End Try
    End Sub

    ''' <summary>
    ''' Evento que salta al pintar el menu
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub menu_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles menu.DataBound
        Dim mnItem As MenuItem
        Dim mn As Menu = CType(sender, Menu)
        If (mn IsNot Nothing AndAlso mn.Items.Count > 0) Then
            Dim i As Integer
            For i = mn.Items.Count - 1 To 0 Step -1
                mnItem = mn.Items.Item(i)
                mnItem.Text = itzultzaileWeb.Itzuli(mnItem.Text)
                ConstruirMenu(mnItem)
                If (mnItem.Value = "admPl") Then
                    If (Ticket.EsAdministradorPlanta) Then
                        menu.Items.Remove(mnItem)   'Si es administrador de plantas, no vera la opcion de administradores (hijo 3 del primer nodo)
                    End If
                ElseIf (mnItem.Value = "igorre") Then 'solo sera visible en la administracion de igorre
                    If Not (Ticket.IdPlantaActual = 1 And (Ticket.EsAdministrador Or Ticket.EsAdministradorPlanta)) Then menu.Items.Remove(mnItem)
                End If
            Next
        End If
    End Sub

    ''' <summary>
    ''' Construye el menu recursivamente
    ''' </summary>
    ''' <param name="mnItem"></param>
    Private Sub ConstruirMenu(ByVal mnItem As MenuItem)
        Dim mnItem1 As MenuItem
        Dim i As Integer
        For i = mnItem.ChildItems.Count - 1 To 0 Step -1
            mnItem1 = mnItem.ChildItems.Item(i)
            mnItem1.Text = itzultzaileWeb.Itzuli(mnItem1.Text)
            ConstruirMenu(mnItem1)
            If (mnItem1.Value = "admPl") Then
                If Not (Ticket.EsAdministrador) Then mnItem.ChildItems.Remove(mnItem1) 'Se quita el item de administracion
            ElseIf (mnItem1.Value = "igorre") Then 'solo sera visible en la administracion de igorre
                If Not (Ticket.IdPlantaActual = 1 And (Ticket.EsAdministrador Or Ticket.EsAdministradorPlanta)) Then mnItem.ChildItems.Remove(mnItem1)
            End If
        Next
    End Sub

#End Region

#Region "Cerrar Session"

    ''' <summary>
    ''' Vuelve a la pagina del portal del empleado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub imgCerrarSession_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles imgCerrarSession.Click
        Try
            Dim generi As New SabLib.BLL.LoginComponent
            generi.SetTicketEnBD(New SabLib.ELL.Ticket With {.IdUser = Ticket.IdUser, .IdSession = Ticket.IdSession})
            Session(PageBase.STICKET) = Nothing
            Response.Redirect(Session(PageBase.HOME))
        Catch ex As Exception
            MensajeError = "Error al cerrar session"
        End Try
    End Sub

#End Region

End Class