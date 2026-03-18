Public Class Site
	Inherits System.Web.UI.MasterPage

#Region "Propiedades"
	Public PageBase As New PageBase
    Public WithEvents ascx_Mensajes As Global.GTK_Troqueleria.Mensajes

    ''' <summary>
    ''' Recupera el ticket del usuario
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public ReadOnly Property Ticket() As SabLib.ELL.Ticket
        Get
            If (Session("Ticket") IsNot Nothing) Then
                Return CType(Session("Ticket"), SabLib.ELL.Ticket)
            Else
                Return Nothing
            End If
        End Get
    End Property

#End Region
#Region "Eventos de Pagina"
    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
		Try
            'Establece la url de la hoja de estilo depediendo si se entra por http o https
            If Not (Page.IsPostBack) Then
                'linkCSS.Href = If(String.Compare(My.Computer.Name, "ltdiglesias", True) = 0 _
                '    , "http://localhost/BaliabideOrokorrak/estiloIntranet.css" _
                '    , "https://intranet2.batz.es/BaliabideOrokorrak/estiloIntranet.css")
                linkCSS.Href = "https://intranet2.batz.es/BaliabideOrokorrak/estiloIntranet.css"
            End If

            'lblVersion.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString
            'lblVersion.ToolTip = System.Diagnostics.FileVersionInfo.GetVersionInfo(Reflection.Assembly.GetExecutingAssembly.Location).ProductVersion
            '--------------------------------------------------------------------------------------------------------------------------------------
            'Indicamos si la aplicacion tiene una nueva version.
            '--------------------------------------------------------------------------------------------------------------------------------------
            Dim Nuevo_Cookies As Boolean = (Request.Cookies("AssemblyVersion") Is Nothing)
			Dim aCookie_AssemblyVersion As HttpCookie = If(Nuevo_Cookies = True, New HttpCookie("AssemblyVersion"), Request.Cookies("AssemblyVersion"))
			If Nuevo_Cookies Or String.Compare(aCookie_AssemblyVersion.Values("AssemblyVersion"), System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString) <> 0 Then _
				aCookie_AssemblyVersion.Values("FechaDeteccion") = Now.Date.ToString("yyyy/MM/dd")
			aCookie_AssemblyVersion.Expires = DateTime.Now.AddYears(10)
			aCookie_AssemblyVersion.Values("AssemblyVersion") = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString
			Response.Cookies.Add(aCookie_AssemblyVersion)

            'If DateDiff(DateInterval.Weekday, CType(Server.HtmlEncode(aCookie_AssemblyVersion.Values("FechaDeteccion")), Date), Now.Date) < 1 Then
            '	btnInfo.ImageUrl = "~/App_Themes/Batz/IconosAjaxControl/VentanaMensajes/warning-icon16.png"
            'End If
            '--------------------------------------------------------------------------------------------------------------------------------------

            If PageBase.EsExtranet(Request) = True Then
                lnkIntranet.NavigateUrl = "/extranetlogin"
                lnkIntranet.ToolTip = "Ir a la página principal de la Extranet"
            End If
        Catch ex As Exception
			Request.Cookies.Clear()
			Global_asax.log.Debug("Cookies borradas")
			Global_asax.log.Error(ex)
		End Try
	End Sub

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ComprobacionPerfil()
    End Sub
    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    'End Sub
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Try
            Dim FuncBBDD As New BatzBBDD.Funciones
            Dim tick As SabLib.ELL.Ticket = PageBase.Ticket
            If (tick Is Nothing) Then
                lblUsuario.Text = String.Empty
            Else
                lblUsuario.Text = HttpUtility.HtmlDecode("<b>") & If(String.IsNullOrWhiteSpace(tick.NombreCompleto), tick.NombreUsuario, tick.NombreCompleto) & HttpUtility.HtmlDecode("</b>")
            End If
            ltlFechaActual.Text = Now.ToLongDateString
            ddlIdioma.SelectedValue = FuncBBDD.SeleccionarItem(ddlIdioma, If(Ticket Is Nothing OrElse Ticket.Culture Is Nothing, System.Threading.Thread.CurrentThread.CurrentCulture.Name, Ticket.Culture))
        Catch ex As Exception
            Dim msg As String = String.Format(vbCrLf & "ddlIdioma: {0}" _
                                              & vbCrLf & "Ticket: {1}" _
                                              & vbCrLf & "Ticket.Culture: {2}" _
                                              , If(ddlIdioma Is Nothing, "NOTHING", ddlIdioma.SelectedValue) _
                                              , If(Ticket Is Nothing, "NOTHING", "EXISTE") _
                                              , If(Ticket Is Nothing OrElse Ticket.Culture Is Nothing, "NOTHING", Ticket.Culture))
            Global_asax.log.Error(msg, ex)
        End Try
    End Sub
#End Region

#Region "Funciones y Procesos"
    ''' <summary>
    ''' Ocultamos o mostramos los elementos correspondientes al perfil
    ''' </summary>
    ''' <remarks></remarks>
    Sub ComprobacionPerfil()
        '---------------------------------------------------------------------------------------
        'Mostramos las opciones del menu segun el perfil y la pagina de inicio.
        '---------------------------------------------------------------------------------------
        If String.Compare(Page.Request.Url.Segments.LastOrDefault, "Index.aspx", True) = 0 _
            Or PageBase.PerfilUsuario Is Nothing Then
            mnOpciones.Visible = False
        ElseIf PageBase.PerfilUsuario Is Nothing OrElse Not PageBase.PerfilUsuario.Equals(PageBase.Perfil.Administrador) Then
            If Not IsPostBack Then
				'mnOpciones.Items.Item(0).ChildItems.Remove(mnOpciones.Items.Item(0).ChildItems.Item(0))
				mnOpciones.Items.Remove(mnOpciones.Items.Item(1))
			End If
		End If
		'---------------------------------------------------------------------------------------
	End Sub

    Private Sub ddlIdioma_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlIdioma.SelectedIndexChanged
        Try
            Dim StrIdioma As String = ddlIdioma.SelectedValue
            Dim cultureInfo As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture(StrIdioma)
            System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo
            ltlFechaActual.Text = Now.ToLongDateString

            If Ticket Is Nothing Then
                Session.Clear()
                Session.Abandon()
            Else
                Ticket.Culture = cultureInfo.Name
                'Se guarda en base de datos el nuevo idioma
                Dim user As New SabLib.ELL.Usuario
                Dim userComp As New SabLib.BLL.UsuariosComponent
                user.Id = Ticket.IdUser
                user = userComp.GetUsuario(user)
                user.Cultura = cultureInfo.Name
                userComp.Save(user)
            End If

            Response.Redirect("~/Index.aspx", True)
        Catch ex As Threading.ThreadAbortException
            'Evitamos el error "ThreadAbortException" para las redirecciones que fuerzan la finalización de la página (Response.Redirect("~", True)) y así agilizar el tiempo de carga. (ExpReg: (\W|^) Response.Redirect\("(\W|$) )
        Catch ex As Exception
            Global_asax.log.Error(ex)
            ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub


    'Public Sub cambiarUsuario(sender As Object, e As EventArgs) Handles cambioUsuario.Click
    '    Response.Redirect("~/CambiarUsuario.aspx", True)
    'End Sub

#End Region
End Class