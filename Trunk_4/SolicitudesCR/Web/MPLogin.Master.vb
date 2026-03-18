Partial Public Class MPLogin
    Inherits MasterPage

#Region "Visualizacion de mensajes"
    Public elementos As String
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




    Private Property Ticket() As SabLib.ELL.Ticket
        Get
            If (Session("Ticket") IsNot Nothing) Then
                Return CType(Session("Ticket"), SabLib.ELL.Ticket)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As SabLib.ELL.Ticket)
            Session("Ticket") = value
        End Set
    End Property

    ''' <summary>
    ''' Carga de la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        '''''''Dim genComp As New Sablib.BLL.LoginComponent
        '''''''Dim ticket As Sablib.ELL.Ticket = genComp.Login(HttpContext.Current.User.Identity.Name.ToLower)

        '''''''If Not Page.IsPostBack Then
        '''''''    imgLogo.ToolTip = Server.MachineName & If(ConfigurationManager.AppSettings("CurrentStatus").ToLower = "debug", "-Debug", String.Empty)
        '''''''    imgLogo.AlternateText = itzultzaileWeb.Itzuli("Portal del empleado")
        '''''''    liSessionOff.Visible = If(Regex.IsMatch(Request.Path, "/Default.aspx", RegexOptions.IgnoreCase), False, True)
        '''''''    liHome.Visible = Not (liSessionOff.Visible)
        '''''''    aHome.HRef = "https://" & Servidor & ".batz.es/Homeintranet"
        '''''''    If (Request.Path IsNot Nothing AndAlso Request.Path.ToLower.Contains("default.aspx")) Then
        '''''''        Dim cultura As String
        '''''''        If (Session("Ticket") Is Nothing) Then


        '''''''            '''''''''''          Dim ticket As Sablib.ELL.Ticket = genComp.Login("batznt\igorrotxategi")
        '''''''            Session("Ticket") = ticket
        '''''''            cultura = ticket.Culture
        '''''''        Else
        '''''''            cultura = CType(Session("Ticket"), Sablib.ELL.Ticket).Culture
        '''''''        End If
        '''''''        Dim cultureInfo As Globalization.CultureInfo = Globalization.CultureInfo.CreateSpecificCulture(cultura)
        '''''''        Threading.Thread.CurrentThread.CurrentCulture = cultureInfo
        '''''''        Session("miCultura") = cultura
        '''''''    End If
        '''''''End If
        '''''''Dim nombre As String = String.Empty
        '''''''If (Session("Ticket") IsNot Nothing) Then nombre = CType(Session("Ticket"), Sablib.ELL.Ticket).NombreCompleto
        '''''''lblUserCon.Text = nombre
        '''''''liUserCon.Visible = (nombre <> String.Empty) 'Cuando te conectas desde fuera, hasta que no se logea, el nombre es blanco




        Dim recBLL As New SabLib.BLL.RecursosComponent

        If Not IsPostBack Then
            If (Request.QueryString("id") IsNot Nothing And Session("F5") Is Nothing) Then 'Si viene un idSession en la url, hay que cargar el ticket
                Dim loginComp As New SabLib.BLL.LoginComponent
                Ticket = loginComp.getTicket(Request.QueryString("id"))
                Session("Ticket") = Ticket
                If (Session("PlantaSel") Is Nothing) Then Session("PlantaSel") = Ticket.IdPlanta
            End If
        End If
        Session("F5") = "menu"
        ''''''If Session("Ticket") Is Nothing Then

        ''''''    Response.Redirect("default.aspx")
        ''''''End If
        Dim myTicket As SabLib.ELL.Ticket = Ticket
        If myTicket IsNot Nothing Then
            Dim qRecursos As List(Of RRHHLib.ELL.recursoPE)
            Dim claseMenu As New RRHHLib.BLL.RRHHComponent
            Dim MachineUserName As String = String.Empty
            ''''''''''Dim nombreUsuario As String() = User.Identity.Name.ToLower.Split("\")
            ''''''''''If (nombreUsuario IsNot Nothing AndAlso nombreUsuario.Length = 2) Then

            ''''''''''    Dim lRecursos As List(Of String()) = recBLL.GetListadoLimitacionRecUser(nombreUsuario(1))
            ''''''''''    If (lRecursos.Count > 0) Then MachineUserName = nombreUsuario(1)
            ''''''''''End If
            Dim selPlanta As String = Session("PlantaSel")
            'Recursos
            '------------------------------------------           
            Dim numControl As Integer = 0
            Dim idRecursoPadre As Integer = CInt(ConfigurationManager.AppSettings("RecursoSolicitudesCR"))
            qRecursos = claseMenu.dameRecursosHijo(idRecursoPadre, myTicket.IdUser, Globalization.CultureInfo.CurrentCulture.Name, MachineUserName)
            If Not qRecursos Is Nothing AndAlso qRecursos.Count > 0 Then 'comprobamos que al menos tenga algún recurso
                qRecursos = qRecursos.FindAll(Function(o As RRHHLib.ELL.recursoPE) o.Visible)
                'rptRec.DataSource = qRecursos
                'rptRec.DataBind()
            Else
                'pnlRecursos.Controls.Add(New Label With {.Text = itzultzaileWeb.Itzuli("Sin recursos"), .CssClass = "negrita"})
            End If

            'sacar los datos


            'If ticket.IdUser = 63690 Then

            'If System.Configuration.ConfigurationManager.AppSettings("administrar").ToString = Ticket.IdUser.ToString Then
            elementos = "<li><a href = SolicitudesCR.aspx >" & itzultzaileWeb.Itzuli("Crear Solicitudes") & "</a></li> "
            'End If

            'If System.Configuration.ConfigurationManager.AppSettings("comite").ToString = Ticket.IdUser.ToString Then 'And Mid(HttpContext.Current.Request.Url.AbsoluteUri, 24, 35) = "SolicitudesCR.aspx"
            elementos = elementos & "<li><a href = ConsultaSolicitudes.aspx >" & itzultzaileWeb.Itzuli("Consulta de Solicitudes") & "</a></li> "
            'End If

        End If



    End Sub

    ''' <summary>
    ''' Se cierra sesion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub aSessionOff_ServerClick(sender As Object, e As EventArgs) Handles aSessionOff.ServerClick
        Session.Clear()
        Session.Abandon()
        Response.Redirect("https://" & Servidor & ".batz.es/SolicitudesCR")
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
        itzultzaileWeb.Itzuli(lblSessionOff)
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(lblSalir) : itzultzaileWeb.Itzuli(Label1)
        End If
    End Sub

End Class