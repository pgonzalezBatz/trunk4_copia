Imports Oracle.ManagedDataAccess.Client

Partial Public Class MPWeb
    Inherits System.Web.UI.MasterPage
#Region "Propiedades"
    Public PageBase As New PageBase
    Public WithEvents ascx_Mensajes As Global.IstrikuWebRaiz.Mensajes

#End Region
#Region "Visualizacion de mensajes"
    <Obsolete("Usar el sistema de mensajes de 'ascx_Mensajes'")>
    Private m_Mensaje As String
    <Obsolete("Usar el sistema de mensajes de 'ascx_Mensajes'")>
    Private m_tipo As Integer
    <Obsolete("Usar el sistema de mensajes de 'ascx_Mensajes'")>
    Private m_traducido As Boolean
    <Obsolete("Usar el sistema de mensajes de 'ascx_Mensajes'")>
    Private m_limpiar As Boolean

    Public Enum TipoMensaje As Integer
        _Info = 1
        _Advertencia = 2
        _Error = 3
    End Enum
    <Obsolete("Usar el sistema de mensajes de 'ascx_Mensajes'")>
    Public WriteOnly Property MensajeInfo() As String
        Set(ByVal value As String)
            m_Mensaje = value
            m_tipo = TipoMensaje._Info
            m_traducido = True
        End Set
    End Property
    <Obsolete("Usar el sistema de mensajes de 'ascx_Mensajes'")>
    Public WriteOnly Property MensajeAdvertencia() As String
        Set(ByVal value As String)
            m_Mensaje = value
            m_tipo = TipoMensaje._Advertencia
            m_traducido = True
        End Set
    End Property
    <Obsolete("Usar el sistema de mensajes de 'ascx_Mensajes'")>
    Public WriteOnly Property MensajeError() As String
        Set(ByVal value As String)
            m_Mensaje = value
            m_tipo = TipoMensaje._Error
            m_traducido = True
        End Set
    End Property
    <Obsolete("Usar el sistema de mensajes de 'ascx_Mensajes'")>
    Public Sub LimpiarMensajes()
        m_limpiar = True
    End Sub
#End Region
#Region "Ticket"
    ''' <summary>
    ''' Obtiene el ticket de la session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property Ticket() As SabLib.ELL.Ticket
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
#End Region
#Region "Eventos de pagina"
    ''' <summary>
    ''' Establece la url de la hoja de estilo depediendo si se entra por http o https
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Not (Page.IsPostBack) Then
            linkCSS.Href = Request.Url.Scheme & "://intranet2.batz.es/BaliabideOrokorrak/estiloIntranet.css"
        End If
    End Sub

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ComprobacionPerfil()
    End Sub
    ''' <summary>
    ''' Configura el mensaje de error, advertencia o informacion a mostrar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Render(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim tick As SabLib.ELL.Ticket = Ticket
        If (tick Is Nothing) Then
            lblUsuario.Text = String.Empty
            labelPlanta.Text = String.Empty
        Else
            lblUsuario.Text = HttpUtility.HtmlDecode("<b>") & tick.NombreCompleto & HttpUtility.HtmlDecode("</b>")
            Dim plantasBLL = New SabLib.BLL.PlantasComponent
            If Ticket.IdSession <> "" Then
                Dim sPlanta = If(Session("IDPLANTA"), Ticket.IdPlanta)
                Dim planta = plantasBLL.GetPlanta(sPlanta).Nombre
                labelPlanta.Text = HttpUtility.HtmlDecode("<b>") & planta & HttpUtility.HtmlDecode("</b>")
            End If
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
    End Sub
#End Region

#Region "Eventos de Objetos"
    Private Sub act_ToolkitScriptManager_AsyncPostBackError(sender As Object, e As AsyncPostBackErrorEventArgs) Handles act_ToolkitScriptManager.AsyncPostBackError
        Global_asax.log.Error(e.Exception)
    End Sub
    Private Sub btnCerrarSession_Click(sender As Object, e As ImageClickEventArgs) Handles btnCerrarSession.Click
        Try
            Global_asax.log.Debug(String.Format("Usuario {0} a cerrado la sesion", Ticket.NombreCompleto))
            Session.Clear()
            Session.Abandon()
            Response.Redirect("~/Login.aspx", False)
        Catch ex As Exception
            Global_asax.log.Error(ex)
            ascx_Mensajes.MensajeError(ex)
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
        If String.Compare(Page.Request.Url.Segments.LastOrDefault, "Login.aspx", True) = 0 _
            Or String.Compare(Page.Request.Url.Segments.LastOrDefault, "IndiceFecuencia.aspx", True) = 0 _
            Or PageBase.PerfilUsuario Is Nothing Then
            mnOpciones.Visible = False
            pnlCerrarSesion.Visible = False
        ElseIf PageBase.PerfilUsuario <> PageBase.Perfil.Administrador AndAlso PageBase.Perfilusuario <> PageBase.Perfil.AdministradorPlanta Then
            If Not IsPostBack Then
                '---------------------------------------------------------------------------------------------------------------------------
                'Eliminamos los nodos que este perfil no puede ver
                '---------------------------------------------------------------------------------------------------------------------------
                Dim NodoPrincipal As MenuItem = mnOpciones.FindItem("")
                Dim aNodoItem(NodoPrincipal.ChildItems.Count - 1) As MenuItem
                NodoPrincipal.ChildItems.CopyTo(aNodoItem, 0) 'Obtenemos todos los nodos en un array.
                Dim lNodo_Borrar As IEnumerable(Of MenuItem) = From Reg As MenuItem In aNodoItem
                                                               Where Reg.NavigateUrl = "~/Mantenimiento/Usuarios/Listado.aspx" OrElse Reg.NavigateUrl = "~/Mantenimiento/Plantas.aspx"
                                                               Select Reg
                lNodo_Borrar.ToList.ForEach(Sub(Nodo) Nodo.Parent.ChildItems.Remove(Nodo))
                '---------------------------------------------------------------------------------------------------------------------------
            End If
        ElseIf PageBase.PerfilUsuario = PageBase.Perfil.AdministradorPlanta Then
            If Not IsPostBack Then
                Dim NodoPrincipal As MenuItem = mnOpciones.FindItem("")
                Dim aNodoItem(NodoPrincipal.ChildItems.Count - 1) As MenuItem
                NodoPrincipal.ChildItems.CopyTo(aNodoItem, 0)
                Dim lNodo_Borrar As IEnumerable(Of MenuItem) = From Reg As MenuItem In aNodoItem
                                                               Where Reg.NavigateUrl = "~/Mantenimiento/Plantas.aspx"
                                                               Select Reg
                lNodo_Borrar.ToList.ForEach(Sub(Nodo) Nodo.Parent.ChildItems.Remove(Nodo))
            End If
        End If
    End Sub
#End Region
End Class