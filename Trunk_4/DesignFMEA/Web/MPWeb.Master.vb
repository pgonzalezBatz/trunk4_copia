Public Partial Class MPWeb
    Inherits MasterPage

#Region "Visualizacion de mensajes"

    Private m_Mensaje As String
	Private m_tipo As Integer
	Private m_traducido As Boolean
    Private m_limpiar As Boolean    

	''' <summary>
	''' Tipo del mensaje
	''' </summary>	
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
			m_Mensaje = value
			m_tipo = TipoMensaje._Info
			m_traducido = True
		End Set
	End Property

	''' <summary>
	''' Mensaje de advertencia a mostrar
	''' </summary>
	''' <value></value>	
	Public WriteOnly Property MensajeAdvertencia() As String
		Set(ByVal value As String)
			m_Mensaje = value
			m_tipo = TipoMensaje._Advertencia
			m_traducido = True
		End Set
	End Property

	''' <summary>
	''' Mensaje de error a mostrar
	''' </summary>
	''' <value></value>	
	Public WriteOnly Property MensajeError() As String
		Set(ByVal value As String)
			m_Mensaje = value
			m_tipo = TipoMensaje._Error
			m_traducido = True
		End Set
	End Property

	''' <summary>
	''' Limpia el mensaje
	''' </summary>	
	Public Sub LimpiarMensajes()
		m_limpiar = True
	End Sub

	''' <summary>
	''' Visualiza u oculta el panel de la cabecera: Menu y link
	''' </summary>
	''' <value></value>	
	Public WriteOnly Property VisualizarCabecera() As Boolean
		Set(ByVal value As Boolean)
			menu2.Visible = value
		End Set
	End Property

	''' <summary>
	''' Obtiene el ticket de la session
	''' </summary>
	''' <value></value>
	''' <returns></returns>	
	Public Property Ticket() As Sablib.ELL.Ticket
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

    ''' <summary>
    ''' Guarda el id de la planta con el que se accede a la aplicacion. Todas las gestiones, se haran con este idPlanta
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>	
    Public Property IdPlantaGestion() As Integer
        Get
            If (Session("IdPlanta") Is Nothing) Then
                Return Integer.MinValue
            Else
                Return CInt(Session("IdPlanta"))
            End If

        End Get
        Set(ByVal value As Integer)
            Session("IdPlanta") = value
        End Set
    End Property

    'Public itzultzaileWeb As New LocalizationLib.Itzultzaile
    Public itzultzaileWeb As New TraduccionesLib.itzultzaile

#End Region

#Region "Eventos de pagina"
    ''' <summary>
    ''' Establece la url de la hoja de estilo depediendo si se entra por http o https
    ''' Arregla el problema del menu de google chrome'' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Private Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init
        linkCSS.Href = Request.Url.Scheme & "://intranet2.batz.es/BaliabideOrokorrak/estiloIntranet.css"
        If (Request.UserAgent.IndexOf("AppleWebKit") > 0) Then Request.Browser.Adapters.Clear()
    End Sub

    ''' <summary>
    ''' Carga la pagina maestra
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If (Not Page.IsPostBack And Ticket IsNot Nothing) Then
            Dim cultureInfo As Globalization.CultureInfo = Globalization.CultureInfo.CreateSpecificCulture(Ticket.Culture)
            Threading.Thread.CurrentThread.CurrentCulture = cultureInfo
            Session("Cultura") = Ticket.Culture
            MenuDataBind()
        End If
        imgLogo.ToolTip = Server.MachineName & If(ConfigurationManager.AppSettings("CurrentStatus").ToLower = "debug", "-Debug", String.Empty)
    End Sub

    ''' <summary>
    ''' Configura el mensaje de error, advertencia o informacion a mostrar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Protected Sub Page_Render(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        Dim tick As Sablib.ELL.Ticket = Ticket
        lblUsuario.Text = If(tick Is Nothing, String.Empty, HttpUtility.HtmlDecode("<b>") & tick.NombreCompleto & HttpUtility.HtmlDecode("</b>"))
        ltlFechaActual.Text = Now.ToLongDateString
        lblMensaje.Text = m_Mensaje
        pnlMensaje.Visible = True
        Page.MaintainScrollPositionOnPostBack = True
        If (m_limpiar) Then
            pnlMensaje.CssClass = "SinMensaje"
            imgMensaje.Visible = False
            upMensaje.Update()
        Else
            imgMensaje.Visible = True
            If (m_tipo = TipoMensaje._Info) Then
                pnlMensaje.CssClass = "MensajeInfoAdvertencia"
                imgMensaje.ImageUrl = "~\App_Themes\Tema1\Images\info.gif"
                Page.MaintainScrollPositionOnPostBack = False
                upMensaje.Update()
            ElseIf (m_tipo = TipoMensaje._Advertencia) Then
                pnlMensaje.CssClass = "MensajeInfoAdvertencia"
                imgMensaje.ImageUrl = "~\App_Themes\Tema1\Images\advertencia.gif"
                Page.MaintainScrollPositionOnPostBack = False
                upMensaje.Update()
            ElseIf (m_tipo = TipoMensaje._Error) Then
                pnlMensaje.CssClass = "MensajeError"
                imgMensaje.ImageUrl = "~\App_Themes\Tema1\Images\error.gif"
                Page.MaintainScrollPositionOnPostBack = False
                upMensaje.Update()
            Else
                pnlMensaje.Visible = False
            End If
        End If
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(ltlUsuario) : itzultzaileWeb.Itzuli(labelPie1)
        End If
    End Sub

    ''' <summary>
    ''' Traduce un termino
    ''' </summary>
    ''' <param name="termino">Termino</param>    
    Protected Function Traducir(ByVal termino As String)
        Return itzultzaileWeb.Itzuli(termino)
    End Function

#End Region

#Region "Menu"

    ''' <summary>
    ''' A las paginas de listado, se les asigna la url. Se borran los items que por el perfil, no se puedan ver
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub menu_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles menu.DataBound
        Dim mnItem As MenuItem
        Dim mn As Menu = CType(sender, Menu)
        If (mn IsNot Nothing AndAlso mn.Items.Count > 0) Then
            Dim i As Integer
            Dim bConstruirMenu As Boolean
            For i = mn.Items.Count - 1 To 0 Step -1
                bConstruirMenu = True
                mnItem = mn.Items.Item(i)
                mnItem.Text = itzultzaileWeb.Itzuli(mnItem.Text)
                If (Session("Admin") Is Nothing) Then
                    If (mnItem.Value = "admin") Then
                        mn.Items.Remove(mnItem)
                        bConstruirMenu = False
                    End If
                End If
                If (bConstruirMenu) Then ConstruirMenu(mnItem)
            Next
        End If
    End Sub

    ''' <summary>
    ''' Construye el menu recursivamente
    ''' </summary>
    ''' <param name="mnItem"></param>	
    Private Sub ConstruirMenu(ByVal mnItem As MenuItem)
		Dim mnItem1 As MenuItem		
        Dim bConstruirMenu As Boolean
        For i As Integer = mnItem.ChildItems.Count - 1 To 0 Step -1
            bConstruirMenu = True
            mnItem1 = mnItem.ChildItems.Item(i)
            mnItem1.Text = itzultzaileWeb.Itzuli(mnItem1.Text)
            If (bConstruirMenu) Then ConstruirMenu(mnItem1)
        Next
	End Sub

	''' <summary>
	''' Enlaza el menu y sino es administrador, no muestra la pestaña de administracion
	''' </summary>	
	Private Sub MenuDataBind()
        Try            
            XmlDataSource.DataFile = "~/App_Data/menu.xml"
            XmlDataSource.XPath = "Root/Item"
            menu.DataBind()
        Catch
        End Try
	End Sub

#End Region

End Class