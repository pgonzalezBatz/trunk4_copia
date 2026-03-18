Imports System.Xml

Partial Public Class MPWeb
    Inherits MasterPage

#Region "Visualizacion de mensajes"

    Private m_Mensaje As String
    Private m_tipo As Integer
    Private m_limpiar As Boolean
    Public itzultzaileWeb As New LocalizationLib.Itzultzaile

    Public Enum TipoMensaje As Integer
        _Info = 1
        _Advertencia = 2
        _Error = 3
    End Enum

    Public WriteOnly Property MensajeInfo() As String
        Set(ByVal value As String)
            m_Mensaje = value
            m_tipo = TipoMensaje._Info
        End Set
    End Property

    Public WriteOnly Property MensajeAdvertencia() As String
        Set(ByVal value As String)
            m_Mensaje = value
            m_tipo = TipoMensaje._Advertencia
        End Set
    End Property

    Public WriteOnly Property MensajeError() As String
        Set(ByVal value As String)
            m_Mensaje = value
            m_tipo = TipoMensaje._Error
        End Set
    End Property

    ''' <summary>
    ''' Ticket
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
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

    ''' <summary>
    ''' Id de la empresa de Epsilon del usuario
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public Property IdEmpresaEpsilon() As String
        Get
            If (Session("IdEpsilon") Is Nothing) Then
                Return String.Empty
            Else
                Return Session("IdEpsilon").ToString
            End If
        End Get
        Set(ByVal value As String)
            Session("IdEpsilon") = value
        End Set
    End Property

    ''' <summary>
    ''' Rol del usuario para esa planta
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Private Property Rol() As Integer
        Get
            If (Session("Rol") Is Nothing) Then
                Return Integer.MinValue
            Else
                Return CInt(Session("Rol"))
            End If
        End Get
        Set(ByVal value As Integer)
            Session("Rol") = value
        End Set
    End Property

    ''' <summary>
    ''' Obtiene el servidor en el que se esta ejecutando
    ''' </summary>
    ''' <returns></returns>
    Private ReadOnly Property Servidor As String
        Get
            Return If(ConfigurationManager.AppSettings("CurrentStatus").ToLower = "debug", "intranet-test", "intranet2")
        End Get
    End Property

    ''' <summary>
    ''' Escribe el titulo
    ''' </summary>
    Public WriteOnly Property SetTitle As String
        Set(ByVal value As String)
            lblTitulo.Text = itzultzaileWeb.Itzuli(value)
        End Set
    End Property

    ''' <summary>
    ''' No muestra la cabecera
    ''' </summary>
    Public Sub NotShowHeader()
        divMenu.Visible = False
        btnMenu.Visible = False
    End Sub

#End Region

#Region "Eventos de pagina"

    ''' <summary>
    ''' Establece la url de la hoja de estilo depediendo si se entra por http o https
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init
        linkCSS.Href = "https://" & Servidor & ".batz.es/BaliabideOrokorrak/estiloIntranet.css"
        imgLogo.ImageUrl = "https://" & Servidor & ".batz.es/BaliabideOrokorrak/logo_batz_menu.png"
    End Sub

    ''' <summary>
    ''' Carga la pagina maestra
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not (Page.IsPostBack) Then
            imgLogo.ToolTip = Server.MachineName & If(ConfigurationManager.AppSettings("CurrentStatus").ToLower = "debug", "-Debug", String.Empty)
            Dim myCulture As String = If(Ticket IsNot Nothing, Ticket.Culture, "es-ES")
            Dim cultureInfo As Globalization.CultureInfo = Globalization.CultureInfo.CreateSpecificCulture(myCulture)
            Threading.Thread.CurrentThread.CurrentCulture = cultureInfo
        End If
    End Sub

    ''' <summary>
    ''' Traduce un termino
    ''' </summary>
    ''' <param name="termino"></param>
    ''' <returns></returns>
    Protected Function Traducir(ByVal termino As String) As String
        Return itzultzaileWeb.Itzuli(termino)
    End Function

    ''' <summary>
    ''' Configura el mensaje de error, advertencia o informacion a mostrar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Protected Sub Page_Render(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        MenuDataBind()
        lblMensaje.Text = m_Mensaje
        If (m_limpiar) Then
            lblMensaje.Text = String.Empty
            pnlMensaje.CssClass = "SinMensaje"
            pnlMensaje.Visible = False
            upMensaje.Update()
        Else
            pnlMensaje.Visible = True
            If (m_tipo = TipoMensaje._Info) Then
                pnlMensaje.CssClass = "alert alert-success"
                upMensaje.Update()
            ElseIf (m_tipo = TipoMensaje._Advertencia) Then
                pnlMensaje.CssClass = "alert alert-warning"
                upMensaje.Update()
            ElseIf (m_tipo = TipoMensaje._Error) Then
                pnlMensaje.CssClass = "alert alert-danger"
                upMensaje.Update()
            Else
                pnlMensaje.Visible = False
            End If
        End If
    End Sub

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
                PaintMenu(node, sMenu)
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
    Private Sub PaintMenu(ByVal node As XmlNode, ByRef sMenu As StringBuilder)
        Dim tipoNodo As String = node.Attributes("id").Value
        If (tipoNodo <> String.Empty AndAlso Rol <> 0) Then 'Si tiene algo y no es el administrador, se chequea si se tiene que pintar
            If ((tipoNodo = "encrip" AndAlso (Rol And NominasLib.Nomina.Roles.Encriptar) <> NominasLib.Nomina.Roles.Encriptar) OrElse
                (tipoNodo = "docInt" AndAlso (Rol And NominasLib.Nomina.Roles.DocIntereses) <> NominasLib.Nomina.Roles.DocIntereses) OrElse
                (tipoNodo = "doc10T" AndAlso (Rol And NominasLib.Nomina.Roles.Doc10T) <> NominasLib.Nomina.Roles.Doc10T) OrElse
                (tipoNodo = "encripIgorre" AndAlso ((Rol And NominasLib.Nomina.Roles.Encriptar) <> NominasLib.Nomina.Roles.Encriptar OrElse Ticket.IdPlanta <> 1)) OrElse
                (tipoNodo = "adm")) Then
                Exit Sub
            End If
        End If
        sMenu.Append("<li>")
        If (Not node.HasChildNodes) Then
            sMenu.Append("<a href='" & Request.Url.GetLeftPart(UriPartial.Authority) & VirtualPathUtility.ToAbsolute("~/") & (node.Attributes("url").Value) & "'>" & itzultzaileWeb.Itzuli(node.Attributes("title").Value) & "</a>")
        Else
            sMenu.Append("<a href='#' data-toggle='dropdown'>" & node.Attributes("title").Value & "<span class='caret'></span></a>")
            sMenu.Append("<ul class='dropdown-menu'>")
            For Each subNode In node.ChildNodes
                PaintMenu(subNode, sMenu)
            Next
            sMenu.Append("</ul>")
        End If
        sMenu.Append("</li>")
    End Sub

#End Region

#Region "Cerrar session"

    ''' <summary>
    ''' Cierra la sesion del usuario activo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub btnLogOff_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLogOff.Click
        Try
            If (Ticket Is Nothing) Then 'Vuelve al login del portal del empleado
                Response.Redirect("\LangileenTxokoa")
            Else 'Vuelve a los recursos del portal del empleado
                Dim generi As New SabLib.BLL.LoginComponent
                generi.SetTicketEnBD(New SabLib.ELL.Ticket With {.IdSession = Ticket.IdSession, .IdUser = Ticket.IdUser})
                Session.Abandon()
                Response.Redirect(Session("home"))
            End If
        Catch
        End Try
    End Sub

#End Region

End Class