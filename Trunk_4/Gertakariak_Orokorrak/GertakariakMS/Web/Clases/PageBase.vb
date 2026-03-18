Imports System.Io
Imports System.Globalization.CultureInfo
Imports System.Xml
Imports GertakariakMSWeb_Raiz

Public Class PageBase
	Inherits Page

	Public ItzultzaileWeb As New Itzultzaile

#Region "Eventos de Pagina"
	Private Sub Page_Error(sender As Object, e As EventArgs) Handles Me.Error
		'If Server.GetLastError IsNot Nothing Then log.Error(Server.GetLastError.ToString, Server.GetLastError)
		'-------------------------------------------------------------------------------------------
		If Server.GetLastError IsNot Nothing Then
			Dim ex As Exception = Server.GetLastError
			log.Error(ex.ToString, ex)
			'If TypeOf ex Is HttpException Then Response.Redirect(PAG_PERMISODENEGADO & "?msg=" & ex.Message.Substring(0, ex.Message.IndexOf("(")))
			If TypeOf ex Is HttpException Then Response.Redirect(PAG_PERMISODENEGADO & "?msg=" & If(ex.Message.IndexOf("(") < 0, ex.Message, ex.Message.Substring(0, ex.Message.IndexOf("("))))
		End If
		'-------------------------------------------------------------------------------------------
	End Sub
	Private Sub Page_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
		'--------------------------------------------------------
		'Elementos de la pagina que no deben traducirse
		'--------------------------------------------------------
		ItzultzaileWeb.ObjetosNoTraducibles.Add("linkCSS")
		ItzultzaileWeb.ObjetosNoTraducibles.Add("cphHEAD")
		ItzultzaileWeb.ObjetosNoTraducibles.Add("ToolkitScriptManager_MPWeb")
		'--------------------------------------------------------
	End Sub

	Private Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
		'------------------------------------------------------------------------------------------------------------
		'Evitamos que la MasterPage se traduzca automaticamente.
		'------------------------------------------------------------------------------------------------------------
		'If Not IsPostBack Then
		'    ItzultzaileWeb.ObjetosNoTraducibles.Add("Head1")
		'    '------------------------------------------------------
		'    'ItzultzaileWeb.ObjetosNoTraducibles.Add("form1") 'Evita que se traduzca cualquier contenido de la pagina
		'    ItzultzaileWeb.ObjetosNoTraducibles.Add("ToolkitScriptManager_MPWeb")
		'    ItzultzaileWeb.ObjetosNoTraducibles.Add("ltlUsuario")
		'    ItzultzaileWeb.ObjetosNoTraducibles.Add("lblUsuario")
		'    ItzultzaileWeb.ObjetosNoTraducibles.Add("ltlFechaActual")
		'    ItzultzaileWeb.ObjetosNoTraducibles.Add("menu2")
		'    ItzultzaileWeb.ObjetosNoTraducibles.Add("upMensaje")
		'    ItzultzaileWeb.ObjetosNoTraducibles.Add("upAccionesDia")
		'    '------------------------------------------------------
		'    ItzultzaileWeb.ObjetosNoTraducibles.Add("cphPie")
		'End If
		'------------------------------------------------------------------------------------------------------------
	End Sub
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
            Response.Redirect(PageBase.PAG_PERMISODENEGADO)
        End If
	End Sub

	''' <summary>
	''' En este evento se encuentran cargados todos los controles de la página y podemos usar el traductor "Itzultzaile".
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub Page_PreRenderComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRenderComplete
#If DEBUG Then
		'log.Debug("TraducirWebControls - INICIO")
		ItzultzaileWeb.TraducirWebControls(Page.Controls)
		'log.Warn("TraducirWebControls - Fin")
#Else
        'ItzultzaileWeb.TraducirObjetos(Page.Controls)
		ItzultzaileWeb.TraducirWebControls(Page.Controls)
#End If
	End Sub
#End Region

#Region "Variables compartidas"

	Public Shared PAG_PERMISODENEGADO As String = "~/PermisoDenegado.aspx"
    Public Shared log As log4net.ILog = log4net.LogManager.GetLogger("root.GertakariakMS")

#End Region

#Region "Informes Cognos"

    'Public Shared COGNOS_CULTURA As String = "[CULTURA]"
    'Public Shared COGNOS_REFERENCIA As String = "[REFERENCIA]"
    'Public Shared COGNOS_IDEMPRESA As String = "[ID_EMPRESA]"
    'Public Shared COGNOS_CODARTICULO As String = "[COD_ARTICULO]"
    'Public Shared COGNOS_CODPROVEEDOR As String = "[COD_PROVEEDOR]"
    'Public Shared COGNOS_CODCARACTERISTICA As String = "[COD_CARACTERISTICA]"
    'Public Shared COGNOS_LANTEGIRH As String = "[LANTEGI_RH]"
    'Public Shared COGNOS_IDPROYECTO As String = "[ID_PROYECTO]"
    'Public Shared COGNOS_FECHADESDE As String = "[FECHA_DESDE]"
    'Public Shared COGNOS_FECHAHASTA As String = "[FECHA_HASTA]"
    'Public Shared COGNOS_TIPO As String = "[TIPO]"
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

#Region "Cultura"
    ' ''' <summary>
    ' ''' Inicializa la cultura
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Public Sub inicializarCultura()
    '	MyBase.InitializeCulture()
    'End Sub
	Protected Overrides Sub InitializeCulture()
        'Try
        If Ticket Is Nothing Then
            Dim cultureInfo As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture("es-ES")
            System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo
			Culture = cultureInfo.Name
			Ticket = New SabLib.ELL.Ticket With {.Culture = cultureInfo.Name}
        Else
            Culture = Ticket.Culture
        End If
        'Ticket.Culture = CurrentCulture.Name
        MyBase.InitializeCulture()
        'Catch ex As Exception
        '	log.Error(ex)
        'End Try
    End Sub
#End Region

#Region "log4net"
	''' <summary>
	''' Escribe en el log un error
	''' </summary>
	''' <param name="key"></param>
	''' <param name="ex"></param>
	''' <remarks></remarks>
	<Obsolete("NO USAR", False)> _
	Public Shared Function LogError(ByVal key As String, ByVal ex As Exception) As String
		Try
			'----------------------------------------------------------------------------------------------------
			'Dim loc As New LocalizationLib.AccesoGenerico
			'Dim termino As String = loc.GetTermino(key, System.Globalization.CultureInfo.CurrentCulture.Name)
			'log.Error(termino, ex)
			'Return termino
			'----------------------------------------------------------------------------------------------------
			log.Error(key, ex)
			Return key
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

#Region "Funciones y Procesos"
    ''' <summary>
    ''' Devuelve la ruta (ImageUrl) de la imagen para el tipo de archivo correspondiente.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function ImagenDocumento(CONTENT_TYPE As String) As String
        If String.Compare("image/pjpeg", CONTENT_TYPE, True) = 0 _
            Or String.Compare("image/jpeg", CONTENT_TYPE, True) = 0 Then
            ImagenDocumento = "~/App_Themes/Batz/Imagenes/TipoArchivos/jpeg-icon32.png"
        ElseIf String.Compare("image/x-png", CONTENT_TYPE, True) = 0 Or String.Compare("image/png", CONTENT_TYPE, True) = 0 Then
            ImagenDocumento = "~/App_Themes/Batz/Imagenes/TipoArchivos/png-icon32.png"
        ElseIf String.Compare("image/bmp", CONTENT_TYPE, True) = 0 Then
            ImagenDocumento = "~/App_Themes/Batz/Imagenes/TipoArchivos/bmp-icon32.png"
        ElseIf String.Compare("text/plain", CONTENT_TYPE, True) = 0 _
            Or String.Compare("text/richtext", CONTENT_TYPE, True) = 0 Then
            ImagenDocumento = "~/App_Themes/Batz/Imagenes/TipoArchivos/text-icon32.png"
        ElseIf String.Compare("application/msword", CONTENT_TYPE, True) = 0 _
            Or String.Compare("application/vnd.openxmlformats-officedocument.wordprocessingml.document", CONTENT_TYPE, True) = 0 Then
            ImagenDocumento = "~/App_Themes/Batz/Imagenes/TipoArchivos/docx-win-icon32.png"
        ElseIf String.Compare("application/vnd.sealed-xls", CONTENT_TYPE, True) = 0 _
            Or String.Compare("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", CONTENT_TYPE, True) = 0 _
            Or String.Compare("application/vnd.ms-excel", CONTENT_TYPE, True) = 0 Then
            ImagenDocumento = "~/App_Themes/Batz/Imagenes/TipoArchivos/xlsx-win-icon32.png"
        ElseIf String.Compare("application/pdf", CONTENT_TYPE, True) = 0 Then
            ImagenDocumento = "~/App_Themes/Batz/Imagenes/TipoArchivos/pdf-icon32.png"
        ElseIf String.Compare("application/octet-stream", CONTENT_TYPE, True) = 0 Then
            ImagenDocumento = "~/App_Themes/Batz/Imagenes/TipoArchivos/Mail-icon32.png"
        ElseIf String.Compare("application/vnd.openxmlformats-officedocument.presentationml.presentation", CONTENT_TYPE, True) = 0 Then
            ImagenDocumento = "~/App_Themes/Batz/Imagenes/TipoArchivos/pptx-win-icon32.png"
        Else
            ImagenDocumento = "~/App_Themes/Batz/Imagenes/TipoArchivos/Document-Blank-icon32.png"
            log.Debug("Falta icono para el tipo de archivo: " & CONTENT_TYPE)
        End If
        Return ImagenDocumento
    End Function
#End Region
End Class

Public Class gtkFiltro
	''' <summary>
	''' Texto que se va a buscar en diferentes tablas.
	''' </summary>
	Private _descripcion As String
	''' <summary>
	''' Campo para realizar busquedas sobre las Acciones.
	''' </summary>
	Private _FechaPrevista As Nullable(Of Date)
	''' <summary>
	''' Campo para realizar busquedas sobre las Acciones.
	''' </summary>
	Private _FechaPrevistaFin As Nullable(Of Date)
	''' <summary>
	''' Estado de las Incidencias.
	''' </summary>
	Private _Estado As Nullable(Of EstadoIncidencia)
	''' <summary>
	''' Identificadores de los responsables de las Incidencias y Acciones.
	''' </summary>
	Private _Responsables As List(Of Integer)
	''' <summary>
	''' Identificadores de los responsables de las Incidencias y Acciones.
	''' </summary>
	Private _Caracteristicas As List(Of Integer)
	''' <summary>
	''' Identificadores de los responsables de las Incidencias y Acciones.
	''' </summary>
	Private _Activos As List(Of String)

	''' <summary>
	''' Texto que se va a buscar en diferentes tablas.
	''' </summary>
	Public Property Descripcion As String
		Get
			Return _descripcion
		End Get
		Set(ByVal value As String)
			_descripcion = value
		End Set
	End Property

	''' <summary>
	''' Campo para realizar busquedas sobre las Acciones.
	''' Representa la "Fecha Prevista de Fin" de la accion o "Fecha de Revision" de la accion.
	''' </summary>
	Public Property FechaPrevista As Nullable(Of Date)
		Get
			Return _FechaPrevista
		End Get
		Set(value As Nullable(Of Date))
			_FechaPrevista = value
		End Set
	End Property
	''' <summary>
	''' Campo para realizar busquedas sobre las Acciones.
	''' Representa la "Fecha Prevista de Fin" de la accion o "Fecha de Revision" de la accion.
	''' </summary>
	Public Property FechaPrevistaFin As Nullable(Of Date)
		Get
			Return _FechaPrevistaFin
		End Get
		Set(value As Nullable(Of Date))
			_FechaPrevistaFin = value
		End Set
	End Property

	''' <summary>
	''' Estado de las Incidencias.
	''' </summary>
	''' <value></value>
	Public Property Estado As Nullable(Of EstadoIncidencia)
		Get
			Return _Estado
		End Get
		Set(value As Nullable(Of EstadoIncidencia))
			_Estado = value
		End Set
	End Property

	''' <summary>
	''' Identificadores de los responsables de las Incidencias y Acciones.
	''' </summary>
	Public Property Responsables As List(Of Integer)
		Get
			If _Responsables Is Nothing Then
				_Responsables = New List(Of Integer)
			End If
			Return _Responsables
		End Get
		Set(value As List(Of Integer))
			_Responsables = value
		End Set
	End Property

	''' <summary>
	''' Identificadores de caracteristicas o propiedades (gtkEstructuras) que tiene asignada una Incidencia.
	''' </summary>
	Public Property Caracteristicas As List(Of Integer)
		Get
			If _Caracteristicas Is Nothing Then
				_Caracteristicas = New List(Of Integer)
			End If
			Return _Caracteristicas
		End Get
		Set(value As List(Of Integer))
			_Caracteristicas = value
		End Set
	End Property

	''' <summary>
	''' Identificadores de los responsables de las Incidencias y Acciones.
	''' </summary>
	Public Property Activos As List(Of String)
		Get
			If _Activos Is Nothing Then
				_Activos = New List(Of String)
			End If
			Return _Activos
		End Get
		Set(value As List(Of String))
			_Activos = value
		End Set
	End Property

	''' <summary>
	''' Estado de la Incidencia. 
	''' </summary>
	''' <remarks></remarks>
	Public Enum EstadoIncidencia
		''' <summary>
		''' Incidencia SIN "Fecha de Cierre" (GERTAKARIA.FECHACIERRE = NULL)
		''' </summary>
		''' <remarks></remarks>
		Abierta
		''' <summary>
		''' Incidencia CON "Fecha de Cierre" (GERTAKARIA.FECHACIERRE != NULL)
		''' </summary>
		''' <remarks></remarks>
		Cerrada
	End Enum
End Class