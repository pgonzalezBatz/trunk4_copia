'Imports System.Globalization.CultureInfo
Partial Public Class MPWeb
	Inherits System.Web.UI.MasterPage

#Region "Propiedades"
	''' <summary>
	''' Estructura donde se almacenamos las propiedades que queremos del GridView.
	''' </summary>
	''' <remarks></remarks>
	Public Propiedades_gvGertakariak As New GertakariakLib2.gtkGridView
	Public WithEvents ascx_Mensajes As Mensajes
	''' <summary>
	''' Perfil del usuario en curso.
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Property PerfilUsuario As Perfil
		Get
			If (Session("PerfilUsuario") Is Nothing) Then
				Return Nothing
			Else
				Return CType(Session("PerfilUsuario"), Perfil)
			End If
		End Get
		Set(value As Perfil)
			Session("PerfilUsuario") = value
		End Set
	End Property
	''' <summary>
	''' Roles que puede tener los usuarios.
	''' Administrador: Tiene acceso a todo.
	''' Usurio: No tiene acceso al menu de "Mantenimiento".
	''' Consultor: Solo puede modificar lo que tenga asignado y el resto solo consultarlo.
	''' </summary>
	''' <remarks></remarks>
	Enum Perfil As Integer
		Administrador
		Usuario
		Consultor
	End Enum

	''' <summary>
	''' Estructura donde se almacenamos las propiedades que queremos del GridView para realiza la seleccion, paginacion y ordenacion.
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Property gvGertakariak_Propiedades() As gtkGridView
		Get
			If (Session("gvGertakariak_Propiedades") Is Nothing) Then Session("gvGertakariak_Propiedades") = New gtkGridView
			Return CType(Session("gvGertakariak_Propiedades"), gtkGridView)
		End Get
		Set(value As gtkGridView)
			Session("gvGertakariak_Propiedades") = value
		End Set
	End Property
#End Region
#Region "Ticket"
    ''' <summary>
    ''' Obtiene el ticket de la session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
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
#Region "Visualizacion de mensajes"
    Private m_Mensaje As String
    Private m_tipo As Integer
    Private m_traducido As Boolean
    Private m_limpiar As Boolean

    Public Enum TipoMensaje As Integer
        _Info = 1
        _Advertencia = 2
        _Error = 3
    End Enum

    'Public WriteOnly Property MensajeInfoKey() As String
    '	Set(ByVal value As String)
    '		m_Mensaje = value
    '		m_tipo = TipoMensaje._Info
    '		m_traducido = False
    '	End Set
    'End Property

    Public WriteOnly Property MensajeInfo() As String
        Set(ByVal value As String)
            m_Mensaje = value
            m_tipo = TipoMensaje._Info
            m_traducido = True

            m_limpiar = False
        End Set
    End Property

    'Public WriteOnly Property MensajeAdvertenciaKey() As String
    '	Set(ByVal value As String)
    '		m_Mensaje = value
    '		m_tipo = TipoMensaje._Advertencia
    '		m_traducido = False
    '	End Set
    'End Property

    Public WriteOnly Property MensajeAdvertencia() As String
        Set(ByVal value As String)
            m_Mensaje = value
            m_tipo = TipoMensaje._Advertencia
            m_traducido = True

            m_limpiar = False
        End Set
    End Property

    'Public WriteOnly Property MensajeErrorKey() As String
    '	Set(ByVal value As String)
    '		m_Mensaje = value
    '		m_tipo = TipoMensaje._Error
    '		m_traducido = False
    '	End Set
    'End Property

    Public WriteOnly Property MensajeError() As String
        Set(ByVal value As String)
            m_Mensaje = value
            m_tipo = TipoMensaje._Error
            m_traducido = True

            m_limpiar = False
        End Set
    End Property

    Public Sub LimpiarMensajes()
        m_limpiar = True
    End Sub

    ''' <summary>
    ''' Visualiza u oculta el panel de la cabecera: Menu y link
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public WriteOnly Property VisualizarCabecera() As Boolean
        Set(ByVal value As Boolean)
			MenuContenedor.Visible = value
        End Set
    End Property

#End Region
#Region "Eventos de pagina"
    ''' <summary>
    ''' Establece la url de la hoja de estilo depediendo si se entra por http o https
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
	Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Not (Page.IsPostBack) Then linkCSS.Href = Request.Url.Scheme & "://intranet2.batz.es/BaliabideOrokorrak/estiloIntranet.css"
        '------------------------------------------------------------------------------------
        'Recogemos los valores de "Propiedades_gvGertakariak".
        '------------------------------------------------------------------------------------
        If Session("Propiedades_gvGertakariak") IsNot Nothing Then
			Propiedades_gvGertakariak = Session("Propiedades_gvGertakariak")
		Else
			'------------------------------------------------------------------
			'Parametros iniciales del GridView principal.
			'------------------------------------------------------------------
			Propiedades_gvGertakariak.CampoOrdenacion = "ID"
			Propiedades_gvGertakariak.DireccionOrdenacion = ComponentModel.ListSortDirection.Descending
			Session("Propiedades_gvGertakariak") = Propiedades_gvGertakariak
			'------------------------------------------------------------------
		End If
		'------------------------------------------------------------------------------------
	End Sub
    ''' <summary>
    ''' Carga la pagina maestra
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (Page.IsPostBack) Then
            Dim cultureInfo As System.Globalization.CultureInfo
            If (Session("Cultura") Is Nothing) Then
                ddlIdioma.SelectedValue = Ticket.Culture
                cultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture(Ticket.Culture)
                ddlIdioma.SelectedValue = Ticket.Culture
            Else
                ddlIdioma.SelectedValue = Session("Cultura")
                cultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture(Session("Cultura"))
                ddlIdioma.SelectedValue = Session("Cultura")
            End If
            System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo
        End If
    End Sub
    ''' <summary>
    ''' Configura el mensaje de error, advertencia o informacion a mostrar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Render(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim tick As Sablib.ELL.Ticket = Ticket
        If (tick Is Nothing) Then
            lblUsuario.Text = String.Empty
        Else
            lblUsuario.Text = HttpUtility.HtmlDecode("<b>") & tick.NombreCompleto & HttpUtility.HtmlDecode("</b>")
        End If

        ltlFechaActual.Text = Now.ToLongDateString

        'If (m_traducido) Then
        lblMensaje.Text = m_Mensaje
        'Else
        'lblMensaje.Key = m_Mensaje
        'End If

        pnlMensaje.Visible = True

        If (m_limpiar) Then
            'lblMensaje.Text = String.Empty
            pnlMensaje.CssClass = "SinMensaje"
            imgMensaje.Visible = False
            upMensaje.Update()
        Else
            imgMensaje.Visible = True
            If (m_tipo = TipoMensaje._Info) Then
                pnlMensaje.CssClass = "MensajeInfoAdvertencia"
                imgMensaje.ImageUrl = "~/App_Themes/Tema1/Imagenes/info.gif"
                upMensaje.Update()
            ElseIf (m_tipo = TipoMensaje._Advertencia) Then
                pnlMensaje.CssClass = "MensajeInfoAdvertencia"
                imgMensaje.ImageUrl = "~/App_Themes/Tema1/Imagenes/advertencia.gif"
                upMensaje.Update()
            ElseIf (m_tipo = TipoMensaje._Error) Then
                pnlMensaje.CssClass = "MensajeError"
                imgMensaje.ImageUrl = "~/App_Themes/Tema1/Imagenes/error.gif"
                upMensaje.Update()
            Else
                pnlMensaje.Visible = False
            End If
        End If

        '------------------------------------------------------------------
        'Guardamos los elementos a conservar para la sesion.
        '------------------------------------------------------------------
		Session("Propiedades_gvGertakariak") = Propiedades_gvGertakariak
        '------------------------------------------------------------------

        ''------------------------------------------------------------------
        ''Objetos a traducir de la Master.
        ''------------------------------------------------------------------
        'ItzultzaileWeb.Itzuli(ltlUsuario)
        'ItzultzaileWeb.Itzuli(MenuMaster)
        'ItzultzaileWeb.Itzuli(lblMensaje)
        'ItzultzaileWeb.Itzuli(Literal1)
		''------------------------------------------------------------------
	End Sub
#End Region
#Region "Eventos de Objetos"
	'Private Sub uc_smpKaPlan_PreRender(sender As Object, e As EventArgs) Handles uc_smpKaPlan.PreRender
	'	If IsPostBack Then ItzultzaileWeb.TraducirWebControls(uc_smpKaPlan.Controls)
	'End Sub
#End Region
#Region "Menu"
    Private Sub MenuMaster_Init(sender As Object, e As System.EventArgs) Handles MenuMaster.Init
        '-------------------------------------------------------------------------------------
        'Eliminamos el ultimo elemento del menu porque es donde colocamos el "Mantenimiento", al que solo tiene acceso los "Administradores.
        '-------------------------------------------------------------------------------------
        Dim MenuPrincipal As Menu = sender
        If PerfilUsuario <> Perfil.Administrador Then MenuPrincipal.Items.RemoveAt(MenuPrincipal.Items.Count - 1) 'Eliminamos el menu de "Mantenimiento".
        If PerfilUsuario = Perfil.Consultor Then MenuPrincipal.Items.RemoveAt(MenuPrincipal.Items.Count - 1) 'Eliminamos el Menu para crear una nueva incidencia.
        '-------------------------------------------------------------------------------------
    End Sub

	''' <summary>
	''' Si se hace click en el menu de ayuda, se abrira un pdf
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
    Private Sub menu_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles MenuMaster.MenuItemClick
        Dim ElementoMenu As MenuItem = e.Item
		If Session("Propiedades_gvGertakariak") IsNot Nothing Then
			Propiedades_gvGertakariak = Session("Propiedades_gvGertakariak")
		Else
			Session("Propiedades_gvGertakariak") = Propiedades_gvGertakariak
		End If
        If ElementoMenu.Value = "Nuevo" Then
			Propiedades_gvGertakariak.IdSeleccionado = Nothing
			gvGertakariak_Propiedades.IdSeleccionado = Nothing
            Response.Redirect("~/Incidencia/Mantenimiento/DatosGenerales.aspx", False)
        End If
    End Sub
#End Region
#Region "Cambio de idioma"
    'Private Sub ddlIdioma_Init(sender As Object, e As System.EventArgs) Handles ddlIdioma.Init
    '#If DEBUG Then
    '		'-----------------------------------------------------------------------------------------------------
    '		'Mostramos el selector de idioma solo para hacer pruebas.
    '		'El selector de idioma no se muestra porque ya hay un selector en la página principal de la Intranet.
    '		'-----------------------------------------------------------------------------------------------------
    '		ddlIdioma.Visible = True
    '		'-----------------------------------------------------------------------------------------------------
    '#End If
    'End Sub
    ''' <summary>
    ''' Cambia el idioma de la aplicacion y lo guarda en base de datos, para que la proxima vez que entre, se le muestre en el ultimo idioma seleccionado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ddlIdioma_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlIdioma.SelectedIndexChanged
        Try
            Dim StrIdioma As String = ddlIdioma.SelectedValue
            Dim cultureInfo As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture(StrIdioma)
            System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo
            Session("Cultura") = cultureInfo.Name
            ltlFechaActual.Text = Now.ToLongDateString

            If Ticket Is Nothing Then
                Session.Clear()
                Session.Abandon()
            Else
                Ticket.Culture = cultureInfo.Name
                'Se guarda en base de datos el nuevo idioma
                Dim user As New SabLib.ELL.Usuario With {.Id = Ticket.IdUser}
                Dim userComp As New SabLib.BLL.UsuariosComponent
                user = userComp.GetUsuario(user, False)
                If user IsNot Nothing Then
                    user.Cultura = cultureInfo.Name
                    userComp.Save(user)
                End If
            End If

            Response.Redirect(Page.AppRelativeVirtualPath)
        Catch ex As Exception
            'Dim batzEx As New BatzException("errCambioIdioma", ex)
            'MensajeError = batzEx.Termino
        End Try
    End Sub
#End Region

    '---------------------------------------------------------------------------------------------------------
    'FROGA:2011-12-21: Se crea un scrip para mover el scroll de la pagina hasta donde se muestra el mensaje.
    'En las propiedades de los mensajes de la master añadimos "m_limpiar = False".
    '---------------------------------------------------------------------------------------------------------

    Private Sub lblMensaje_Init(sender As Object, e As System.EventArgs) Handles lblMensaje.Init
        LimpiarMensajes()
    End Sub
    Private Sub lblMensaje_Load(sender As Object, e As System.EventArgs) Handles lblMensaje.Load
        Dim lbl As Label = lblMensaje

        Dim Clave As String = lbl.ID & "JS" 'Nombre clave del bloque del JavaScript
        Dim FuncionJS As String = "f" & lbl.ID 'Nombre de la funcion JavaScript
        Dim Tipo As System.Type = Me.GetType

        If Not Page.ClientScript.IsClientScriptBlockRegistered(Tipo, Clave) _
           Or Not Page.ClientScript.IsStartupScriptRegistered(Tipo, Clave) Then
            ScriptManager.RegisterClientScriptBlock(Page, Tipo, Clave, _
                "function " & FuncionJS & "(sender, args) { " & vbCrLf & _
                    "if(document.getElementById('" & lbl.ClientID & "') != null) {" & vbCrLf & _
                        "//alert(document.getElementById('" & lbl.ClientID & "') + '-' + document.getElementById('" & lbl.ClientID & "').innerHTML); " & vbCrLf & _
                        "if(document.getElementById('" & lbl.ClientID & "').innerHTML != '') { " & vbCrLf & _
                            "//alert(document.getElementById('" & lbl.ClientID & "').innerHTML); " & vbCrLf & _
                            "document.getElementById('" & lbl.ClientID & "').scrollIntoView(false); " & vbCrLf & _
                        "}" & vbCrLf & _
                    "}" & vbCrLf & _
                "return false;}" _
                , True)
            ScriptManager.RegisterStartupScript(Page, Tipo, Clave, "Sys.WebForms.PageRequestManager.getInstance().add_endRequest(" & FuncionJS & ");", True)
        End If
    End Sub

    '---------------------------------------------------------------------------------------------------------
End Class