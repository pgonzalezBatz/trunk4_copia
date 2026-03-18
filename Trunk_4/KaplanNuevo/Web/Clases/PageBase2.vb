Imports System.IO
Imports System.Globalization.CultureInfo
Imports System.Xml

Public Class PageBase2
    Inherits Page

#Region "Variables compartidas"
    Public Shared PAG_INICIO As String = "~/Index.aspx"   '"~/CrearDocumento.aspx" 
    Public Shared PAG_INICIO_ADMINISTRADOR As String = "~/Paginas/Solicitudes/TramitarSolicitudes.aspx"
    Public Shared PAG_INICIO_PRODUCT_ENGINEER As String = "~/Paginas/Solicitudes/ReferenciaFinalVenta.aspx"
    Public Shared PAG_INICIO_DOCUMENTATION_TECHNICIAN As String = "~/Paginas/Solicitudes/TramitarSolicitudes.aspx"
    Public Shared PAG_INICIO_VALIDATIONS As String = "~/Paginas/Solicitudes/Validaciones.aspx"
    Public Shared PAG_PERMISODENEGADO As String = "~/PermisoDenegado.aspx"
    Public Shared log As log4net.ILog = log4net.LogManager.GetLogger("root.Kaplan")
    Public Shared plantaAdmin As Int32 = 999
    Public Shared plantaAdminNombre As String = ""
    Public Shared DirFicherosBajar As String = ""
    Public Shared DirFicherosSubir As String = ""
    Public ItzultzaileWeb As New LocalizationLib.Itzultzaile
#End Region

#Region "Informes Cognos"

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


    '''' <summary>
    '''' Obtiene los roles que pueden acceder a la página
    '''' </summary>
    '''' <value></value>
    '''' <returns></returns>
    '''' <remarks></remarks>
    Protected Overridable ReadOnly Property RolesAcceso As List(Of KaplanLib.ELL.Roles.RolUsuario)
        Get
            Return Nothing
        End Get
    End Property


    '''' <summary>
    '''' Obtiene el perfilde usuario de la sesión
    '''' </summary>
    '''' <value></value>
    '''' <returns></returns>
    Protected Property PerfilUsuario() As KaplanLib.ELL.PerfilUsuario
        Get
            If (Session("PerfilUsuario") Is Nothing) Then
                Return Nothing
            Else
                Return CType(Session("PerfilUsuario"), KaplanLib.ELL.PerfilUsuario)
            End If
        End Get
        Set(ByVal value As KaplanLib.ELL.PerfilUsuario)
            Session("PerfilUsuario") = value
        End Set
    End Property



#End Region

#Region "Propiedades"

    '''' <summary>
    '''' Obtiene el perfilde usuario de la sesión
    '''' </summary>
    '''' <value></value>
    '''' <returns></returns>
    'Protected Property PerfilUsuario() As SolicitudesSistemasLib.ELL.PerfilUsuario
    '    Get
    '        If (Session("PerfilUsuario") Is Nothing) Then
    '            Return Nothing
    '        Else
    '            Return CType(Session("PerfilUsuario"), SolicitudesSistemasLib.ELL.PerfilUsuario)
    '        End If
    '    End Get
    '    Set(ByVal value As SolicitudesSistemasLib.ELL.PerfilUsuario)
    '        Session("PerfilUsuario") = value
    '    End Set
    'End Property

    '''' <summary>
    '''' Obtiene los roles que pueden acceder a la página
    '''' </summary>
    '''' <value></value>
    '''' <returns></returns>
    '''' <remarks></remarks>
    'Protected Overridable ReadOnly Property RolesAcceso As List(Of SolicitudesSistemasLib.ELL.Roles.RolUsuario)
    '    Get
    '        Return Nothing
    '    End Get
    'End Property
#End Region

#Region "Page Load"
    ''' <summary>
    ''' <para>Si no tiene ticket, se vuelve a logear</para>    
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim segmentos As String() = Page.Request.Url.Segments
        Dim pag As String = segmentos(Page.Request.Url.Segments.GetUpperBound(0))
        If Session("Ticket") Is Nothing Then
            'TODO:Quitar IF de  compilacion condicional.
            '#If Not Debug Then
            '     Response.Redirect(PageBase.PAG_PERMISODENEGADO)
            '#End If
        End If

        '  Session("Ticket") = Nothing

        ' Registramos el .js de jquery
        'Page.ClientScript.RegisterClientScriptInclude("jQuery", ResolveUrl("~/Scripts/jquery-1.7.1.min.js"))
        'Page.ClientScript.RegisterClientScriptInclude("jQuery", ResolveUrl("~/Scripts/jquery.js"))
        'Page.ClientScript.RegisterClientScriptInclude("jQuery", ResolveUrl("~/Scripts/thickbox-compressed.js"))
    End Sub
#End Region

#Region "Cultura"
    ''' <summary>
    ''' Inicializa la cultura
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub inicializarCultura()
        MyBase.InitializeCulture()
    End Sub
    Protected Overrides Sub InitializeCulture()
        Try
            If Ticket Is Nothing Then
                Dim cultureInfo As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture("es-ES")
                System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo
                Ticket.Culture = cultureInfo.Name
                Culture = cultureInfo.Name
            Else
                Culture = Ticket.Culture
            End If
            Ticket.Culture = CurrentCulture.Name
            MyBase.InitializeCulture()
        Catch

        End Try
    End Sub
#End Region

#Region "Traducciones"
    ''' <summary>
    ''' En este evento se encuentran cargados todos los controles de la página y podemos usar el traductor "Itzultzaile".
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_PreRenderComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRenderComplete
        'ItzultzaileWeb.TraducirObjetos(Page.Controls)
        ItzultzaileWeb.TraducirWebControls(Page.Controls)
    End Sub
#End Region

#Region "log4net"

    ''' <summary>
    ''' Escribe en el log un error
    ''' </summary>
    ''' <param name="key"></param>
    ''' <param name="ex"></param>
    ''' <remarks></remarks>
    Public Shared Function LogError(ByVal key As String, ByVal ex As Exception) As String
        Try
            'Dim loc As New LocalizationLib2.AccesoGenerico
            Dim termino As String = LocalizationLib.AccesoGenerico.GetTerminoStatic(key, System.Globalization.CultureInfo.CurrentCulture.Name, System.Configuration.ConfigurationManager.AppSettings.Get("LocalPath"))
            log.Error(termino, ex)
            Return termino
        Catch
            Return String.Empty
        End Try
    End Function

#End Region

#Region "Informes Cognos"

    ''' <summary>
    ''' Obtiene la ruta del informe de la key proporcionada
    ''' </summary>
    ''' <param name="key">Nombre del informe</param>
    ''' <returns>Ruta del mismo</returns>
    ''' <remarks></remarks>
    Public Function consultarInformeCognos(ByVal key As String, ByVal scheme As String) As String
        Try
            Dim ruta As String = String.Empty
            Dim xmlDoc As New XmlDocument
            Dim m_nodelist As XmlNodeList
            xmlDoc.Load(Server.MapPath("~") & "\App_Data\InformesCognos.xml")
            m_nodelist = xmlDoc.SelectNodes("Informes/Informe")
            For Each m_node As XmlNode In m_nodelist
                If (m_node.Attributes.GetNamedItem("name").Value = key) Then
                    'ruta = Request.Url.Scheme & "://" & m_node.InnerText
                    ruta = scheme & "://" & m_node.InnerText
                    Exit For
                End If
            Next
            Return ruta
        Catch ex As Exception
            log.Error(Server.MapPath("~") & "\App_Data\InformesCognos.xml")
            log.Error("Error al leer la ruta del informe de cognos con la key=" & key)
            Return String.Empty
        End Try
    End Function

#End Region

End Class