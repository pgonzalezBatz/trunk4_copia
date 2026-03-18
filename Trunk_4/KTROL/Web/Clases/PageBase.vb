Imports System.Io
Imports System.Globalization.CultureInfo
Imports System.Xml
Imports System.Configuration

Public Class PageBase
    Inherits Page

#Region "Variables compartidas"
    Public Shared PAG_PERMISODENEGADO As String = "~/PermisoDenegado.aspx"
    Public ItzultzaileWeb As New Itzultzaile
#End Region

#Region "Informes Cognos"

    Public Shared COGNOS_CULTURA As String = "[CULTURA]"
    Public Shared COGNOS_REFERENCIA As String = "[REFERENCIA]"
    Public Shared COGNOS_IDEMPRESA As String = "[ID_EMPRESA]"
    Public Shared COGNOS_CODOPERACION As String = "[COD_OPERACION]"
    Public Shared COGNOS_FECHADESDE As String = "[FECHA_DESDE]"
    Public Shared COGNOS_FECHAHASTA As String = "[FECHA_HASTA]"
    Public Shared COGNOS_CARACTERISTICA As String = "[CARACTERISTICA]"
    Public Shared COGNOS_IDCONTROL As String = "[ID_CONTROL]"
    'Public Shared COGNOS_LANTEGI As String = "[LANTEGI]"
    'Public Shared COGNOS_NFACTURA As String = "[N_FACTURA]"
    'Public Shared COGNOS_SOLICITANTE As String = "[SOLICITANTE]"
    'Public Shared COGNOS_ESTADO As String = "[ESTADO]"
    'Public Shared COGNOS_IDFACTURA As String = "[ID_FACTURA]"

#End Region

#Region "Ticket"

    ''' <summary>
    ''' Obtiene el ticket de la session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property Ticket() As SABLib.ELL.Ticket
        Get
            If (Session("Ticket") Is Nothing) Then
                Return Nothing
            Else
                Return CType(Session("Ticket"), SABLib.ELL.Ticket)
            End If
        End Get
        Set(ByVal value As SABLib.ELL.Ticket)
            Session("Ticket") = value
        End Set
    End Property


    ''' <summary>
    ''' Comprueba que no haya caducado la sesión
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public ReadOnly Property PerfilUsuarioCorrecto() As Boolean
        Get
            If (Session("PerfilUsuario") IsNot Nothing) Then
                Dim perfil As ELL.PerfilUsuario = Session("PerfilUsuario")
                If (perfil.IdUsuario <> Integer.MinValue) Then
                    Return True
                End If
            End If
            Return False
        End Get
    End Property

#End Region

#Region "Eventos de Pagina"
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
			Response.Redirect(PageBase.PAG_PERMISODENEGADO)
			'#End If
		End If
		If Not (PerfilUsuarioCorrecto) Then
			Global_asax.log.Info("La sesión ha caducado. Se redirige a la pantalla de error")
			Response.Redirect("~/Error.aspx?t=1")
		End If
	End Sub
	''' <summary>
	''' En este evento se encuentran cargados todos los controles de la página y podemos usar el traductor "Itzultzaile".
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub Page_PreRenderComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRenderComplete
		ItzultzaileWeb.TraducirWebControls(Page.Controls)
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

#Region "log4net"

    ''' <summary>
    ''' Escribe en el log un error
    ''' </summary>
    ''' <param name="key"></param>
    ''' <param name="ex"></param>
    ''' <remarks></remarks>
    Public Shared Function LogError(ByVal key As String, ByVal ex As Exception) As String
        Try
            'Dim loc As New LocalizationLib.AccesoGenerico
            'Dim termino As String = loc.GetTermino(key, System.Globalization.CultureInfo.CurrentCulture.Name)
            Dim termino As String = LocalizationLib.AccesoGenerico.GetTerminoStatic(key, System.Globalization.CultureInfo.CurrentCulture.Name, System.Configuration.ConfigurationManager.AppSettings.Get("LocalPath"))
			Global_asax.log.Error(termino, ex)
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
			Global_asax.log.Error(Server.MapPath("~") & "\App_Data\InformesCognos.xml")
			Global_asax.log.Error("Error al leer la ruta del informe de cognos con la key=" & key)
			Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Abre una pagina con el window.open
    ''' </summary>
    ''' <param name="url">Url a abrir</param>
    ''' <remarks></remarks>
    Public Sub windowOpen(ByVal url As String)
        '---------------------------------------------------------------------------------------------------
        'Dim Script As String = "window.open('" & url & "','Informe');"
        '---------------------------------------------------------------------------------------------------
        'FROGA: 2012-08-09: Hacer que al abrir el informe por 2ª vez, sin que este cerrado, coja el foco.
        '---------------------------------------------------------------------------------------------------
        Dim Script As String = "var InformeCognos; "
        Script &= "function abrirVentana(){InformeCognos = window.open('" & url & "','Informe');}; "
        Script &= "function cerrarVentana(){if (InformeCognos != undefined){InformeCognos.close();}; }; "
        Script &= "cerrarVentana();abrirVentana();"
        '---------------------------------------------------------------------------------------------------
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "informe", Script, True)
    End Sub

#End Region

End Class