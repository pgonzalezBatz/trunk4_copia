Imports System.Xml

Public Class Site
    Inherits System.Web.UI.MasterPage

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

    Public WriteOnly Property MensajeInfo() As String
        Set(ByVal value As String)
            m_Mensaje = value
            m_tipo = TipoMensaje._Info
            m_traducido = True
            m_limpiar = False
        End Set
    End Property

    Public WriteOnly Property MensajeAdvertencia() As String
        Set(ByVal value As String)
            m_Mensaje = value
            m_tipo = TipoMensaje._Advertencia
            m_traducido = True
            m_limpiar = False
        End Set
    End Property

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
            menu2.Visible = value
        End Set
    End Property

    Public itzultzaileWeb As New LocalizationLib.Itzultzaile
#End Region

#Region "Eventos de pagina"

    ''' <summary>
    ''' Establece la url de la hoja de estilo depediendo si se entra por http o https
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Request.UserAgent.IndexOf("AppleWebKit") > 0 Then
            Request.Browser.Adapters.Clear()
        End If

        If Not (Page.IsPostBack) Then linkCSS.Href = Request.Url.Scheme & "://intranet.batz.es/BaliabideOrokorrak/estiloIntranet.css"
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
                cultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture(Ticket.Culture)
            Else
                cultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture(Session("Cultura"))
            End If
            'Menu
            Dim xmlDoc As XmlDocument = TratarMenu()
            MenuDataBind(xmlDoc)
            System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo
        End If
        Me.InsertarCssYScripts()
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
                imgMensaje.ImageUrl = "~/App_Themes/Batz/Imagenes/info.gif"
                upMensaje.Update()
            ElseIf (m_tipo = TipoMensaje._Advertencia) Then
                pnlMensaje.CssClass = "MensajeInfoAdvertencia"
                imgMensaje.ImageUrl = "~/App_Themes/Batz/Imagenes/advertencia.gif"
                upMensaje.Update()
            ElseIf (m_tipo = TipoMensaje._Error) Then
                pnlMensaje.CssClass = "MensajeError"
                imgMensaje.ImageUrl = "~/App_Themes/Batz/Imagenes/error.gif"
                upMensaje.Update()
            Else
                pnlMensaje.Visible = False
            End If
        End If
    End Sub

    Private Sub lblMensaje_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblMensaje.Init
        LimpiarMensajes()
    End Sub

    Private Sub lblMensaje_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblMensaje.Load
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

#End Region

#Region "Métodos"

    ''' <summary>
    ''' Añade css y js comunes
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InsertarCssYScripts()
        ' '' jQuery.
        'Dim jQueryMin As New HtmlGenericControl("script")
        'jQueryMin.Attributes("type") = "text/javascript"
        'jQueryMin.Attributes("src") = Me.ResolveUrl("js/jquery-1.11.1-min.js")
        'Me.head.Controls.Add(jQueryMin)


    End Sub

#End Region

#Region "Menu"

    ''' <summary>
    ''' A las paginas de listado, se les asigna la url. Se borran los items que por el perfil, no se puedan ver
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub menu_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles menu.DataBound
        Dim mnItem As MenuItem

        Dim mn As Menu = CType(sender, Menu)
        If (mn IsNot Nothing AndAlso mn.Items.Count > 0) Then
            Dim i As Integer
            Dim bConstruirMenu As Boolean
            For i = mn.Items.Count - 1 To 0 Step -1
                bConstruirMenu = True
                mnItem = mn.Items.Item(i)
                mnItem.Text = itzultzaileWeb.Itzuli(mnItem.Text.Itzuli)
                If (bConstruirMenu) Then
                    ConstruirMenu(mnItem)
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
        Dim bConstruirMenu As Boolean
        For i = mnItem.ChildItems.Count - 1 To 0 Step -1
            bConstruirMenu = True
            mnItem1 = mnItem.ChildItems.Item(i)
            mnItem1.Text = itzultzaileWeb.Itzuli(mnItem1.Text)
            If (bConstruirMenu) Then
                ConstruirMenu(mnItem1)
            End If
        Next
    End Sub

    ''' <summary>
    ''' Enlaza el menú
    ''' </summary>	
    Private Sub MenuDataBind(ByVal xmlDoc As XmlDocument)
        Try
            XmlDataSource.Data = xmlDoc.OuterXml
            XmlDataSource.XPath = "Root/Item"
            XmlDataSource.DataBind()
            menu.DataBind()
        Catch
        End Try
    End Sub

    ''' <summary>
    ''' Trata el menú y lo configura con respecto a los roles del usuario
    ''' </summary>
    ''' <remarks></remarks>
    Private Function TratarMenu() As XmlDocument
        Dim xDoc As New System.Xml.XmlDocument()
        Dim xDocCopia As New System.Xml.XmlDocument()
        xDoc.Load(Server.MapPath("~/App_Data/menu.xml"))

        Dim root As XmlNodeList = xDoc.GetElementsByTagName("Root")
        Dim elementoRoot As XmlElement = CType(root(0), XmlElement)
        Dim elementoRootCopia As XmlElement = xDocCopia.CreateElement("Root")

        TratarElementos(xDocCopia, elementoRoot, elementoRootCopia)
        xDocCopia.AppendChild(elementoRootCopia)

        Return xDocCopia
    End Function

    ''' <summary>
    ''' Trata los nodos del mené de manera recursiva
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub TratarElementos(ByVal xDocCopia As XmlDocument, ByVal elemento As XmlElement, ByVal elementoCopia As XmlElement)
        Dim existe As Boolean = True
        Dim perfilUsuario As New SolicitudesSistemasLib.ELL.PerfilUsuario
        Dim roles As String() = Nothing
        Dim rolAux As String = String.Empty
        Dim nodoAux As XmlNode = Nothing
        Dim elementoAux As XmlElement = Nothing
        Dim rolUsuario As String = String.Empty
        Dim idRol As String = String.Empty

        If (Session("PerfilUsuario") IsNot Nothing) Then
            perfilUsuario = Session("PerfilUsuario")
            If (perfilUsuario.IdRol <> Integer.MinValue) Then
                idRol = perfilUsuario.IdRol
            End If
        End If

        If (elemento.ChildNodes.Count > 0) Then
            For Each nodo As XmlNode In elemento.ChildNodes
                elementoAux = CType(nodo, XmlElement)
                roles = elementoAux.GetAttribute("roles").Split(",")
                For Each rol As String In roles
                    rolAux = rol.Trim()
                    If Not (String.IsNullOrEmpty(idRol)) Then
                        If (idRol.Equals(rolAux)) Then
                            nodoAux = xDocCopia.ImportNode(nodo, False)
                            elementoCopia.AppendChild(nodoAux)
                            Exit For
                        End If
                    End If
                Next

                TratarElementos(xDocCopia, CType(nodo, XmlElement), CType(nodoAux, XmlElement))
            Next
        End If
    End Sub

#End Region

#Region "Cambio de idioma"

    '    Private Sub ddlIdioma_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlIdioma.Init
    '#If DEBUG Then
    '        '-----------------------------------------------------------------------------------------------------
    '        'Mostramos el selector de idioma solo para hacer pruebas.
    '        'El selector de idioma no se muestra porque ya hay un selector en la página principal de la Intranet.
    '        '-----------------------------------------------------------------------------------------------------
    '        ddlIdioma.Visible = True
    '        '-----------------------------------------------------------------------------------------------------
    '#End If
    '    End Sub
    ' ''' <summary>
    ' ''' Cambia el idioma de la aplicacion y lo guarda en base de datos, para que la proxima vez que entre, se le muestre en el ultimo idioma seleccionado
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Protected Sub ddlIdioma_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlIdioma.SelectedIndexChanged
    '    Try
    '        Dim StrIdioma As String = ddlIdioma.SelectedValue
    '        Dim cultureInfo As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture(StrIdioma)
    '        System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo
    '        Session("Cultura") = cultureInfo.Name
    '        Ticket.Culture = cultureInfo.Name
    '        ltlFechaActual.Text = Now.ToLongDateString

    '        'Se guarda en base de datos el nuevo idioma
    '        Dim user As New Sablib.ELL.Usuario
    '        Dim userComp As New Sablib.BLL.UsuariosComponent
    '        user.Id = Ticket.IdUser
    '        user = userComp.GetUsuario(user)
    '        user.Cultura = cultureInfo.Name
    '        userComp.Save(user)

    '        'Response.Redirect("~/Default.aspx")
    '        Response.Redirect(Page.AppRelativeVirtualPath)
    '    Catch ex As Exception
    '        'Dim batzEx As New BatzException("errCambioIdioma", ex)
    '        'MensajeError = batzEx.Termino
    '    End Try
    'End Sub

#End Region

End Class