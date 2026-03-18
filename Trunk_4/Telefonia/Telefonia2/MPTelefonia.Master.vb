Imports System.Xml

Partial Public Class MPTelefonia
    Inherits MasterPage

#Region "Visualizacion de mensajes"

    Private itzultzaileWeb As New itzultzaile
    Private log As log4net.ILog = log4net.LogManager.GetLogger("root.Bidaiak")
    Private pg As New PageBase

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
    ''' Mensaje de advertencia a mostrar
    ''' </summary>
    ''' <value></value>	
    Public WriteOnly Property MensajeAdvertencia() As String
        Set(ByVal value As String)
            ShowWarningNotif(value)
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
    ''' Muestra una notificacion de tipo informacion
    ''' </summary>
    ''' <param name="mensa">Mensaje</param>
    ''' <param name="bStick">True si se queda visible hasta que se cierre manualmente</param>
    Public Sub ShowInfoNotif(ByVal mensa As String, Optional ByVal bStick As Boolean = True)
        mensa = mensa.Replace("'", "\'")
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "info", "showToast('" & mensa & "','notice'," & bStick.ToString.ToLower & ");", True)
    End Sub

    ''' <summary>
    ''' Muestra una notificacion de tipo accion realizada con exito
    ''' </summary>
    ''' <param name="mensa">Mensaje</param>
    ''' <param name="bStick">True si se queda visible hasta que se cierre manualmente</param>
    Public Sub ShowSuccessNotif(ByVal mensa As String, Optional ByVal bStick As Boolean = False)
        mensa = mensa.Replace("'", "\'")
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "info", "showToast('" & mensa & "','success'," & bStick.ToString.ToLower & ");", True)
    End Sub

    ''' <summary>
    ''' Muestra una notificacion de tipo advertencia
    ''' </summary>
    ''' <param name="mensa">Mensaje</param>
    ''' <param name="bStick">True si se queda visible hasta que se cierre manualmente</param>
    Public Sub ShowWarningNotif(ByVal mensa As String, Optional ByVal bStick As Boolean = True)
        mensa = mensa.Replace("'", "\'")
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "info", "showToast('" & mensa & "','warning'," & bStick.ToString.ToLower & ");", True)
    End Sub

    ''' <summary>
    ''' Muestra una notificacion de tipo error
    ''' </summary>
    ''' <param name="mensa">Mensaje</param>
    ''' <param name="bStick">True si se queda visible hasta que se cierre manualmente</param>
    Public Sub ShowErrorNotif(ByVal mensa As String, Optional ByVal bStick As Boolean = True)
        mensa = mensa.Replace("'", "\'")
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "info", "showToast('" & mensa & "','error'," & bStick.ToString.ToLower & ");", True)
    End Sub

#End Region

#Region "Properties"

    ''' <summary>
    ''' Escribe el titulo
    ''' </summary>
    Public WriteOnly Property SetTitle As String
        Set(ByVal value As String)
            lblTitulo.Text = itzultzaileWeb.Itzuli(value)
        End Set
    End Property

    ''' <summary>
    ''' Obtiene el servidor en el que se esta ejecutando
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Servidor As String
        Get
            Return If(ConfigurationManager.AppSettings("CurrentStatus").ToLower = "debug", "intranet-test", "intranet2")
        End Get
    End Property

    ''' <summary>
    ''' No muestra la cabecera
    ''' </summary>
    Public Sub NotShowHeader()
        divMenu.Visible = False
        btnMenu.Visible = False
    End Sub

    '''' <summary>
    '''' Visualiza u oculta el boton de cerrar session
    '''' </summary>
    '''' <value></value>
    'Public WriteOnly Property VisualizarCerrarSession() As Boolean
    '    Set(ByVal value As Boolean)
    '        pnlCerrarSession.Visible = value
    '    End Set
    'End Property

#End Region

#Region "Datos del Ticket"

    ''' <summary>
    ''' Recupera el ticket del usuario
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public Property Ticket() As ELL.TicketTlfno
        Get
            Return CType(Session("Ticket"), ELL.TicketTlfno)
        End Get
        Set(ByVal value As ELL.TicketTlfno)
            Session("Ticket") = value
        End Set
    End Property

#End Region

#Region "Eventos de pagina"

    ''' <summary>
    ''' Establece la url de la hoja de estilo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Private Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init
        imgLogo.ImageUrl = "https://" & Servidor & ".batz.es/BaliabideOrokorrak/logo_batz_menu.png"
        linkCSS.Href = "https://" & Servidor & ".batz.es/BaliabideOrokorrak/estiloIntranet.css"
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
            Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo
            Session(PageBase.SCULTURA) = Ticket.Culture
            lblPlanta.Text = Ticket.Planta
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
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(lblUserCon) : itzultzaileWeb.Itzuli(labelFooter)
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
        Dim sMenu As New StringBuilder
        If (Session("menu") Is Nothing) Then
            Dim xmlFile As New XmlDocument()
            xmlFile.Load(Server.MapPath("~") & "\App_Data\menu.xml")
            sMenu.Append("<ul class='nav navbar-nav navbar-left'>")
            Dim rootNode As XmlNode = xmlFile.SelectSingleNode("Root")
            For Each node As XmlNode In rootNode.ChildNodes
                PaintMenu(node, sMenu, 0)
            Next
            sMenu.Append("</ul>")
        Else
            sMenu.Append(Session("menu"))
        End If
        Session("menu") = sMenu.ToString
        labelMenu.Text = sMenu.ToString
        lblUserCon.Text = If(Ticket IsNot Nothing, Ticket.NombreCompleto, String.Empty)
    End Sub

    ''' <summary>
    ''' Funcion recursiva que pinta el menu
    ''' </summary>
    ''' <param name="node">Nodo</param>
    ''' <param name="sMenu">String donde se escribe el html</param>
    Private Sub PaintMenu(ByVal node As XmlNode, ByRef sMenu As StringBuilder, ByVal level As Integer)
        Dim tipoNodo As String = node.Attributes("id").Value
        If (tipoNodo = "invisible") Then
            Exit Sub
        ElseIf (tipoNodo = "igorre" AndAlso Not (Ticket.IdPlantaActual = 1 And Ticket.EsAdministradorPlanta)) Then
            Exit Sub
        End If
        If (level = 0) Then
            sMenu.Append("<li>")
        Else
            sMenu.Append("<li" & If(node.HasChildNodes, " class='dropdown-submenu'", String.Empty) & "> ")
        End If
        Dim target As String = If(tipoNodo = "manual", " target='_blank'", String.Empty)
        If (Not node.HasChildNodes) Then
            sMenu.Append(" <a href='" & Request.Url.GetLeftPart(UriPartial.Authority) & VirtualPathUtility.ToAbsolute("~/") & (node.Attributes("url").Value) & "'" & target & ">" & itzultzaileWeb.Itzuli(node.Attributes("title").Value) & "</a>")
        Else
            sMenu.Append("<a href='#' data-toggle='dropdown'" & target & "> " & itzultzaileWeb.Itzuli(node.Attributes("title").Value) & If(level = 0, "<span class='caret'></span>", "<span class='caret visible-xs'></span>") & "</a>")
            sMenu.Append("<ul class='dropdown-menu'>")
            For Each subNode In node.ChildNodes
                PaintMenu(subNode, sMenu, level + 1)
            Next
            sMenu.Append("</ul>")
        End If
        sMenu.Append("</li>")
    End Sub

#End Region

#Region "Cerrar Session"

    '''' <summary>
    '''' Vuelve a la pagina del portal del empleado
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    'Private Sub imgCerrarSession_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles imgCerrarSession.Click
    '    Try
    '        Dim generi As New SabLib.BLL.LoginComponent
    '        generi.SetTicketEnBD(New SabLib.ELL.Ticket With {.IdUser = Ticket.IdUser, .IdSession = Ticket.IdSession})
    '        Session(PageBase.STICKET) = Nothing
    '        Response.Redirect(Session(PageBase.HOME))
    '    Catch ex As Exception
    '        MensajeError = "Error al cerrar session"
    '    End Try
    'End Sub

#End Region

End Class