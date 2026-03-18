Imports System.Xml
Imports LocalizationLib
Imports System.Globalization

Public Class Kaplan_Plantas
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
    Public elementos As String

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
            'menu2.Visible = value
        End Set
    End Property

    Public itzultzaileWeb As New LocalizationLib.Itzultzaile
#End Region
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

        'If Not (Page.IsPostBack) Then linkCSS.Href = Request.Url.Scheme & "://intranet.batz.es/BaliabideOrokorrak/estiloIntranet.css"
    End Sub

    ''' <summary>
    ''' Carga la pagina maestra
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '  If Not (Page.IsPostBack) Then
        Dim cultureInfo As System.Globalization.CultureInfo
        If (Session("Cultura") Is Nothing) Then
            cultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture(Ticket.Culture)
        Else
            cultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture(Session("Cultura"))
        End If
        PageBase.DirFicherosBajar = ""
        PageBase.DirFicherosSubir = ""
        'Menu 
        Dim xmlDoc As XmlDocument = TratarMenu()
        MenuDataBind(xmlDoc)
        System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo
        ' End If

        'Me.InsertarCssYScripts()
    End Sub

    ''' <summary>
    ''' Configura el mensaje de error, advertencia o informacion a mostrar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Render(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim tick As SabLib.ELL.Ticket = Ticket
        'If (tick Is Nothing) Then
        '    lblUsuario.Text = String.Empty
        '    lblPlanta.Text = String.Empty
        'Else
        '    lblUsuario.Text = HttpUtility.HtmlDecode("<b>") & tick.NombreCompleto & HttpUtility.HtmlDecode("</b>")
        '    lblPlanta.Text = HttpUtility.HtmlDecode("<b>") & PageBase.plantaAdminNombre & HttpUtility.HtmlDecode("</b>")
        'End If

        'ltlFechaActual.Text = Now.ToLongDateString

        'If (m_traducido) Then
        '       lblMensaje.Text = m_Mensaje
        'Else
        'lblMensaje.Key = m_Mensaje
        'End If

        '       pnlMensaje.Visible = True

        'If (m_limpiar) Then
        '    'lblMensaje.Text = String.Empty
        '    pnlMensaje.CssClass = "SinMensaje"
        '    imgMensaje.Visible = False
        '    '        upMensaje.Update()
        'Else
        '    imgMensaje.Visible = True
        '    If (m_tipo = TipoMensaje._Info) Then
        '        pnlMensaje.CssClass = "MensajeInfoAdvertencia"
        '        imgMensaje.ImageUrl = "~/App_Themes/Batz/Imagenes/info.gif"
        '        '             upMensaje.Update()
        '    ElseIf (m_tipo = TipoMensaje._Advertencia) Then
        '        pnlMensaje.CssClass = "MensajeInfoAdvertencia"
        '        imgMensaje.ImageUrl = "~/App_Themes/Batz/Imagenes/advertencia.gif"
        '        '             upMensaje.Update()
        '    ElseIf (m_tipo = TipoMensaje._Error) Then
        '        pnlMensaje.CssClass = "MensajeError"
        '        imgMensaje.ImageUrl = "~/App_Themes/Batz/Imagenes/error.gif"
        '        '             upMensaje.Update()
        '    Else
        '        pnlMensaje.Visible = False
        '    End If
        'End If
    End Sub

    'Private Sub lblMensaje_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblMensaje.Init
    '    LimpiarMensajes()
    'End Sub

    'Private Sub lblMensaje_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblMensaje.Load
    '    Dim lbl As Label = lblMensaje

    '    Dim Clave As String = lbl.ID & "JS" 'Nombre clave del bloque del JavaScript
    '    Dim FuncionJS As String = "f" & lbl.ID 'Nombre de la funcion JavaScript
    '    Dim Tipo As System.Type = Me.GetType

    '    If Not Page.ClientScript.IsClientScriptBlockRegistered(Tipo, Clave) _
    '       Or Not Page.ClientScript.IsStartupScriptRegistered(Tipo, Clave) Then
    '        ScriptManager.RegisterClientScriptBlock(Page, Tipo, Clave,
    '            "function " & FuncionJS & "(sender, args) { " & vbCrLf &
    '                "if(document.getElementById('" & lbl.ClientID & "') != null) {" & vbCrLf &
    '                    "//alert(document.getElementById('" & lbl.ClientID & "') + '-' + document.getElementById('" & lbl.ClientID & "').innerHTML); " & vbCrLf &
    '                    "if(document.getElementById('" & lbl.ClientID & "').innerHTML != '') { " & vbCrLf &
    '                        "//alert(document.getElementById('" & lbl.ClientID & "').innerHTML); " & vbCrLf &
    '                        "document.getElementById('" & lbl.ClientID & "').scrollIntoView(false); " & vbCrLf &
    '                    "}" & vbCrLf &
    '                "}" & vbCrLf &
    '            "return false;}" _
    '            , True)
    '        ScriptManager.RegisterStartupScript(Page, Tipo, Clave, "Sys.WebForms.PageRequestManager.getInstance().add_endRequest(" & FuncionJS & ");", True)
    '    End If
    'End Sub

    'Private Sub Page_Unload(sender As Object, e As EventArgs) Handles Me.Unload
    '	MyBase.OnPreRender()

    'End Sub

    'Protected Overrides Sub OnPreRenderComplete(ByVal e As System.EventArgs)
    '	Trace.Write("PR-S")
    '	MyBase.OnPreRenderComplete(e)
    '	Trace.Write("PR-E")
    'End Sub
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
            Dim traducido As String
            Dim bConstruirMenu As Boolean
            '       elementos = "<nav class='navbar navbar-default'>  <div class='container-fluid'>    <div class='navbar-header'>      <a class='navbar-brand' href='#'>WebSiteName</a>    </div>    <ul class='nav navbar-nav'>      <li class='active'><a href='#'>Home</a></li>"
            '  elementos = elementos & "</ul> </div></nav>"
            For i = 0 To mn.Items.Count - 1


                If (mn.Items(i)).ChildItems.Count > 0 Then



                    bConstruirMenu = True
                    mnItem = mn.Items.Item(i)
                    mnItem.Text = itzultzaileWeb.Itzuli(mnItem.Text.Itzuli)
                    elementos = elementos & " <li> <a href = '#' Class='dropdown-toggle' data-toggle='dropdown'>" & (mn.Items(i)).[Text] & "<b Class='caret'></b></a> <ul Class='dropdown-menu'> "
                    If (bConstruirMenu) Then
                        ConstruirMenu(mnItem)
                    End If

                    elementos = elementos & "</ul> </li>"

                Else 'no tiene submenu
                    traducido = itzultzaileWeb.Itzuli((mn.Items(i)).[Text].Itzuli)
                    elementos = elementos & "<li><a href=" & (mn.Items(i)).NavigateUrl & ">" & traducido & "</a></li>"

                End If
            Next


            If System.Configuration.ConfigurationManager.AppSettings("extranet").ToString = "1" Then
                elementos = elementos & " </ul> <ul class='nav navbar-nav navbar-right'>    <li> <a href = '#' Class='dropdown-toggle' data-toggle='dropdown'>" & itzultzaileWeb.Itzuli("Contacto") & "<b Class='caret'></b></a>  <ul Class='dropdown-menu'> <li><a href='#'>Tel. 946305000</a></li> <li><a href='#'>email: @batz.es</a></li> <li><a href='#'>horario de 08:00 a 17:00</a></li> </ul> </li>                        <li><a href='" & System.Configuration.ConfigurationManager.AppSettings("LinkInicioExtranet").ToString & "'><span class='glyphicon glyphicon-off'></span>" & itzultzaileWeb.Itzuli("Inicio") & " </a></li>     "
            Else
                elementos = elementos & " </ul> <ul class='nav navbar-nav navbar-right'>             <li> <a href = '#' Class='dropdown-toggle' data-toggle='dropdown'>" & itzultzaileWeb.Itzuli("Contacto") & "<b Class='caret'></b></a>  <ul Class='dropdown-menu'> <li><a href='#'>Tel. 946305000</a></li> <li><a href='#'>email: @batz.es</a></li> <li><a href='#'>horario de 08:00 a 17:00</a></li> </ul> </li>                                     <li><a href='" & System.Configuration.ConfigurationManager.AppSettings("LinkInicioIntranet").ToString & "'><span class='glyphicon glyphicon-off'></span>" & itzultzaileWeb.Itzuli("Salir") & " </a></li>     "
            End If

            Dim host As String = HttpContext.Current.Request.Url.AbsoluteUri
            If Right(host, 10) = "Extranet=1" Or Right(host, 10) = "Extranet=2" Then
                elementos = elementos & " </ul> <ul class='nav navbar-nav navbar-right'>                    <li><a href='" & System.Configuration.ConfigurationManager.AppSettings("LinkInicioExtranet").ToString & "'><span class='glyphicon glyphicon-off'></span>" & itzultzaileWeb.Itzuli("Inicio") & " </a></li>     "
            Else

            End If


        End If

    End Sub

    Private Sub ConstruirMenu(ByVal mnItem As MenuItem)
        Dim mnItem1 As MenuItem
        Dim i As Integer
        Dim bConstruirMenu As Boolean

        '    For i = mnItem.ChildItems.Count - 1 To 0 Step -1
        For i = 0 To mnItem.ChildItems.Count - 1
            bConstruirMenu = True
            mnItem1 = mnItem.ChildItems.Item(i)
            mnItem1.Text = itzultzaileWeb.Itzuli(mnItem1.Text)

            elementos = elementos & "<li><a href=" & (mnItem.ChildItems(i)).NavigateUrl & ">" & (mnItem1).[Text] & "</a></li>"


            If (bConstruirMenu) Then
                ConstruirMenu(mnItem1)
            End If


        Next i

    End Sub


    ''' <summary>
    ''' Enlaza el menú
    ''' </summary>	
    Private Sub MenuDataBind(ByVal xmlDoc As XmlDocument)
        Try
            XmlDataSource.Data = xmlDoc.OuterXml
            XmlDataSource.XPath = "Root/Item"
            XmlDataSource.DataBind()
            Menu.DataBind()
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
        '   xDoc.Load(Server.MapPath("~/App_Data/menu.xml"))
        If System.Configuration.ConfigurationManager.AppSettings("extranet").ToString = "1" Then
            xDoc.Load(Server.MapPath("~/App_Data/menuextranet.xml"))
        Else
            xDoc.Load(Server.MapPath("~/App_Data/menu.xml"))
        End If
        Dim host As String = HttpContext.Current.Request.Url.AbsoluteUri
        If Right(host, 10) = "Extranet=1" Or Right(host, 10) = "Extranet=2" Then
            xDoc.Load(Server.MapPath("~/App_Data/menuextranet2.xml"))
        End If

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
        'FALTA ESTO EN ROLES COPIAR DE OTRO  
        Dim perfilUsuario As New KaplanLib.ELL.PerfilUsuario
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





                'mirar si esta en bbdd entonces rol 2, si no rol 66 sin acceso
                Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL
                Dim listaType As List(Of KaplanLib.ELL.Rol)
                listaType = oDocBLL.CargarRol(Ticket.IdUser, PageBase.plantaAdmin)
                If listaType.Count > 0 Then
                    'Dim zam As Integer = 0
                    For z = 0 To listaType.Count - 1
                        If listaType(z).rol = 2 Then   'panta de zamudio
                            ' puede ser 2 o 9
                            'jon nuevo mirar si tiene permiso en zamudio
                            'si permiso le sumo 10
                            If listaType(0).Id = 9 Or listaType(0).Id = 19 Then
                                listaType(0).Id = 19
                            Else
                                listaType(0).Id = 12
                            End If
                        End If
                    Next






                    Session("PerfilUsuario").idrol = listaType(0).Id '2
                    idRol = listaType(0).Id

                Else




                    idRol = perfilUsuario.IdRol

                End If





            End If
        End If
        'If System.Configuration.ConfigurationManager.AppSettings("extranet").ToString <> "1" Then
        '    If Session("PerfilUsuario").IdRol = 99 Then
        '        PageBase.log.Debug("AccesoRecursoValido false ")
        '        Response.Redirect("~/PermisoDenegado2.aspx") '"~/PermisoDenegado.aspx"  PageBase.PAG_PERMISODENEGADO
        '    End If
        'End If


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



    Private Sub InsertarCssYScripts()
        'jquery
        Dim jQuery As New HtmlGenericControl("script")
        jQuery.Attributes("type") = "text/javascript"
        jQuery.Attributes("src") = Me.ResolveUrl("~/Scripts/jquery-1.10.2.js")
        Me.head.Controls.Add(jQuery)

        Dim jQueryUI As New HtmlGenericControl("script")
        jQueryUI.Attributes("type") = "text/javascript"
        jQueryUI.Attributes("src") = Me.ResolveUrl("~/Scripts/jquery-ui.js")
        Me.head.Controls.Add(jQueryUI)

        Dim jQueryAG As New HtmlGenericControl("script")
        jQueryAG.Attributes("type") = "text/javascript"
        jQueryAG.Attributes("src") = Me.ResolveUrl("~/Scripts/jquery.autogrow.js")
        Me.head.Controls.Add(jQueryAG)

        ' jQuery Cluetip
        Dim jQueryCluetip As New HtmlGenericControl("script")
        jQueryCluetip.Attributes("type") = "text/javascript"
        jQueryCluetip.Attributes("src") = Me.ResolveUrl("~/Scripts/jquery.cluetip.js")
        Me.head.Controls.Add(jQueryCluetip)

        ' jQuery Usuario
        Dim jQueryUsuario As New HtmlGenericControl("script")
        jQueryUsuario.Attributes("type") = "text/javascript"
        jQueryUsuario.Attributes("src") = Me.ResolveUrl("~/Scripts/usuarios.js")
        Me.head.Controls.Add(jQueryUsuario)

        ' jQuery Usuario
        Dim jQueryUsuarioDept As New HtmlGenericControl("script")
        jQueryUsuarioDept.Attributes("type") = "text/javascript"
        jQueryUsuarioDept.Attributes("src") = Me.ResolveUrl("~/Scripts/usuariosDepartamento.js")
        Me.head.Controls.Add(jQueryUsuarioDept)

        ' jQuery Numeric
        Dim jQueryNumeric As New HtmlGenericControl("script")
        jQueryNumeric.Attributes("type") = "text/javascript"
        jQueryNumeric.Attributes("src") = Me.ResolveUrl("~/Scripts/jquery.numeric.js")
        Me.head.Controls.Add(jQueryNumeric)

    End Sub


End Class