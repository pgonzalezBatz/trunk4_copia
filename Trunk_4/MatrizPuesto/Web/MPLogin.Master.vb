Public Class MPLogin
    Inherits System.Web.UI.MasterPage

    '  Inherits MasterPage

#Region "Visualizacion de mensajes"

    Private m_Mensaje As String
    Private m_tipo As Integer

    Private Enum TipoMensaje As Integer
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
    ''' Obtiene el servidor en el que se esta ejecutando
    ''' </summary>
    ''' <returns></returns>
    Private ReadOnly Property Servidor As String
        Get
            Return If(ConfigurationManager.AppSettings("CurrentStatus").ToLower = "debug", "intranet-test", "intranet2")
        End Get
    End Property


    Private itzultzaileWeb As New LocalizationLib.Itzultzaile

#End Region

    ''' <summary>
    ''' Inicializacion de la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub page_init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init
        linkCSS.Href = "https://" & Servidor & ".batz.es/BaliabideOrokorrak/estiloIntranet.css"
        If Not Session("miCultura") Is Nothing Then Page.Culture = Session("miCultura")
        imgLogo.ImageUrl = "https://" & Servidor & ".batz.es/BaliabideOrokorrak/logo_batz_menu.png"
    End Sub

    ''' <summary>
    ''' Carga de la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            imgLogo.ToolTip = Server.MachineName & If(ConfigurationManager.AppSettings("CurrentStatus").ToLower = "debug", "-Debug", String.Empty)
            imgLogo.AlternateText = itzultzaileWeb.Itzuli("Portal del empleado")
            liSessionOff.Visible = If(Regex.IsMatch(Request.Path, "/Default.aspx", RegexOptions.IgnoreCase), False, True)
            liHome.Visible = Not (liSessionOff.Visible)
            aHome.HRef = "https://" & Servidor & ".batz.es/Homeintranet"
            If (Request.Path IsNot Nothing AndAlso Request.Path.ToLower.Contains("default.aspx")) Then
                Dim cultura As String
                If (Session("Ticket") Is Nothing) Then
                    Dim genComp As New SabLib.BLL.LoginComponent
                    Dim ticket As SabLib.ELL.Ticket = genComp.Login(HttpContext.Current.User.Identity.Name.ToLower)
                    Session("Ticket") = ticket
                    cultura = ticket.Culture
                Else
                    cultura = CType(Session("Ticket"), SabLib.ELL.Ticket).Culture
                End If
                Dim cultureInfo As Globalization.CultureInfo = Globalization.CultureInfo.CreateSpecificCulture(cultura)
                Threading.Thread.CurrentThread.CurrentCulture = cultureInfo
                Session("miCultura") = cultura
            End If
        End If
        Dim nombre As String = String.Empty
        If (Session("Ticket") IsNot Nothing) Then nombre = CType(Session("Ticket"), SabLib.ELL.Ticket).NombreCompleto
        If (Session("Ticket") IsNot Nothing) Then PageBase.plantaAdmin = CInt(CType(Session("Ticket"), SabLib.ELL.Ticket).IdPlanta)
        lblUserCon.Text = nombre
        liUserCon.Visible = (nombre <> String.Empty) 'Cuando te conectas desde fuera, hasta que no se logea, el nombre es blanco
        If liUserCon.Visible Then
            li2.Visible = True
            li3.Visible = True
        Else
            li2.Visible = False
            li3.Visible = False
        End If
        'If (Session("Ticket") IsNot Nothing) Then
        '    '    li2.Visible = True
        '    '    li3.Visible = True
        '    'Else
        '    li2.Visible = False
        '    li3.Visible = False
        'End If


    End Sub

    ''' <summary>
    ''' Se cierra sesion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub aSessionOff_ServerClick(sender As Object, e As EventArgs) Handles aSessionOff.ServerClick
        Session.Clear()
        Session.Abandon()
        Response.Redirect("https://" & Servidor & ".batz.es/Homeintranet")
    End Sub

    ''' <summary>
    ''' Renderizado de la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        lblMensaje.Text = m_Mensaje
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
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(lblSalir) : itzultzaileWeb.Itzuli(lblSessionOff)
        End If
    End Sub


End Class