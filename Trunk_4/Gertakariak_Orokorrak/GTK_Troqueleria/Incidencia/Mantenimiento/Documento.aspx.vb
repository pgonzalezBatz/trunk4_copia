Public Class Documento
	Inherits PageBase

#Region "Propiedades"
	Dim BBDD As New BatzBBDD.Entities_Gertakariak
	Dim Documento As New BatzBBDD.DOCUMENTOS
	Dim Incidencia As New BatzBBDD.GERTAKARIAK

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

	''' <summary>
	''' Estructura donde se almacenamos las propiedades que queremos del GridView.
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Property gvDocumentos_Propiedades As gtkGridView
		Get
			If (Session("gvDocumentos_Propiedades") Is Nothing) Then Session("gvDocumentos_Propiedades") = New gtkGridView
			Return CType(Session("gvDocumentos_Propiedades"), gtkGridView)
		End Get
		Set(value As gtkGridView)
			Session("gvDocumentos_Propiedades") = value
		End Set
	End Property

	''' <summary>
	''' Estructura donde se almacenamos las propiedades que queremos del GridView.
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Property gvAcciones_Propiedades As gtkGridView
		Get
			If (Session("gvAcciones_Propiedades") Is Nothing) Then Session("gvAcciones_Propiedades") = New gtkGridView
			Return CType(Session("gvAcciones_Propiedades"), gtkGridView)
		End Get
		Set(value As gtkGridView)
			Session("gvAcciones_Propiedades") = value
		End Set
	End Property
#End Region

#Region "Eventos de Pagina"
	Private Sub Documento_Init(sender As Object, e As System.EventArgs) Handles Me.Init
		'#If DEBUG Then
		'		If gvGertakariak_Propiedades Is Nothing OrElse gvGertakariak_Propiedades.IdSeleccionado Is Nothing Then _
		'					gvGertakariak_Propiedades.IdSeleccionado = 23825
		'		If gvDocumentos_Propiedades Is Nothing OrElse gvDocumentos_Propiedades.IdSeleccionado Is Nothing Then _
		'					gvDocumentos_Propiedades.IdSeleccionado = 601
		'#End If
		Dim section As Web.Configuration.HttpRuntimeSection = ConfigurationManager.GetSection("system.web/httpRuntime")
		lblMaxRequestLength.Text = " " & section.MaxRequestLength / 1000 & " MB = " & section.MaxRequestLength & " KB"
	End Sub

	Private Sub Documento_InitComplete(sender As Object, e As System.EventArgs) Handles Me.InitComplete
		Documento = (From Doc As BatzBBDD.DOCUMENTOS In BBDD.DOCUMENTOS Where Doc.ID = gvDocumentos_Propiedades.IdSeleccionado Select Doc).FirstOrDefault
	End Sub
	'Private Sub Documento_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad

	'End Sub
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If Not IsPostBack Then
			If Documento IsNot Nothing Then
				txtNombre.Text = Documento.NOMBRE
				lblExtension.Text = Documento.EXTENSION
				txtTitulo.Text = Documento.TITULO
				txtDescripcion.Text = Documento.DESCRIPCION

				'-------------------------------------------------------------------------------------------
				'Ponemos el icono correspondiente para cada tipo de archivo.
				'-------------------------------------------------------------------------------------------
				hlDoc.NavigateUrl &= "?Id_Doc=" & gvDocumentos_Propiedades.IdSeleccionado

				'Dim CONTENT_TYPE As String = Right(Documento.CONTENT_TYPE, (Documento.CONTENT_TYPE.Length - InStr(Documento.CONTENT_TYPE, "/")))
				imgDoc.ImageUrl = ImagenDocumento(Documento.CONTENT_TYPE)
				'-------------------------------------------------------------------------------------------
			Else
				btnEliminar.Visible = False
				hlDoc.Visible = False
			End If
		End If
	End Sub
	Private Sub Documento_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
		Try
			CargarDatos()
		Catch ex As ApplicationException
			Log.Debug(ex)
			Master.ascx_Mensajes.MensajeError(ex)
		Catch ex As Exception
			Log.Error(ex)
			Master.ascx_Mensajes.MensajeError(ex)
		End Try
	End Sub
#End Region

#Region "Eventos de Objetos"
	Private Sub btnAceptar_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnAceptar.Click
		Try
			Validar()

			'-------------------------------------------------------------------------------------------------------
			'Identificamos si el documento es nuevo y que tabla lo estamos creando (GERTAKARIAK o ACCIONES).
			'-------------------------------------------------------------------------------------------------------
			If Documento Is Nothing Then
				Documento = New BatzBBDD.DOCUMENTOS
				Documento.FECHACREACION = Now
				If gvGertakariak_Propiedades.IdSeleccionado IsNot Nothing And gvAcciones_Propiedades.IdSeleccionado Is Nothing Then
					Dim GERTAKARIA As BatzBBDD.GERTAKARIAK = _
										   (From gtk As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK Where gtk.ID = gvGertakariak_Propiedades.IdSeleccionado Select gtk).SingleOrDefault
					Documento.GERTAKARIAK.Add(GERTAKARIA)
				ElseIf gvAcciones_Propiedades.IdSeleccionado IsNot Nothing Then
					Dim Accion As BatzBBDD.ACCIONES = _
									(From Acc As BatzBBDD.ACCIONES In BBDD.ACCIONES Where Acc.ID = gvAcciones_Propiedades.IdSeleccionado Select Acc).FirstOrDefault
					Documento.ACCIONES.Add(Accion)
				End If
			End If
			'--------------------------------------------------------------------------------------------------------------------------------

			If Not String.IsNullOrWhiteSpace(txtNombre.Text) Then
				Documento.NOMBRE = txtNombre.Text
			Else
				If fuDocumento IsNot Nothing Then Documento.NOMBRE = Left(fuDocumento.FileName, InStrRev(fuDocumento.FileName, ".") - 1)
			End If

			Documento.TITULO = txtTitulo.Text
			Documento.DESCRIPCION = txtDescripcion.Text

			If fuDocumento IsNot Nothing AndAlso fuDocumento.FileBytes IsNot Nothing AndAlso fuDocumento.FileBytes.Length > 0 Then
				Documento.EXTENSION = (From ext As String In fuDocumento.FileName.Split(".") Select ext).LastOrDefault
				Documento.DOCUMENTO = fuDocumento.FileBytes
				Documento.CONTENT_TYPE = fuDocumento.PostedFile.ContentType
			End If

			'-----------------------------------------------------
			'Indicamos a que pagina debe regresar.
			'Comprobamos con que entidad esta relacionado.
			'-----------------------------------------------------
			Dim VolverUrl As String = "~/Incidencia/Detalle.aspx"

			'If Documento.GERTAKARIAK IsNot Nothing AndAlso Documento.GERTAKARIAK.Any Then
			'	VolverUrl = "~/Incidencia/Detalle.aspx"
			'ElseIf Documento.ACCIONES IsNot Nothing AndAlso Documento.ACCIONES.Any Then
			'	VolverUrl = "~/Incidencia/Mantenimiento/Acciones.aspx"
			'Else
			'	VolverUrl = "~/Incidencia/Detalle.aspx"
			'End If

			BBDD.SaveChanges()
			Response.Redirect(VolverUrl, False)
			'-----------------------------------------------------
		Catch ex As ApplicationException
			Master.ascx_Mensajes.MensajeError(ex)
		Catch ex As Exception
			Log.Error(ex)
			Master.ascx_Mensajes.MensajeError(ex)
		End Try
	End Sub
	Private Sub btnEliminar_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnEliminar.Click
		Try
			'-----------------------------------------------------
			'Indicamos a que pagina debe regresar.
			'Comprobamos con que entidad esta relacionado.
			'-----------------------------------------------------
			Dim VolverUrl As String
			If Documento.GERTAKARIAK IsNot Nothing AndAlso Documento.GERTAKARIAK.any Then
				VolverUrl = "~/Incidencia/Detalle.aspx"
			ElseIf Documento.ACCIONES IsNot Nothing AndAlso Documento.ACCIONES.any Then
				VolverUrl = "~/Incidencia/Mantenimiento/Acciones.aspx"
			Else
				VolverUrl = "~/Incidencia/Detalle.aspx"
			End If
			'-----------------------------------------------------

			BBDD.DOCUMENTOS.DeleteObject(Documento)
			BBDD.SaveChanges()
			gvDocumentos_Propiedades.IdSeleccionado = Nothing
			Response.Redirect(VolverUrl, False)
		Catch ex As Exception
			Log.Error(ex)
			'Master.MensajeError = ex.ToString
			Master.ascx_Mensajes.MensajeError(ex)
		End Try
	End Sub
#End Region

#Region "Funciones y Procesos"
	Sub Validar()
		If Documento Is Nothing And (fuDocumento.FileBytes Is Nothing OrElse fuDocumento.FileBytes.Length = 0) _
			OrElse (Documento IsNot Nothing AndAlso Documento.DOCUMENTO Is Nothing And (fuDocumento.FileBytes IsNot Nothing OrElse fuDocumento.FileBytes.Length > 0)) Then
			Throw New ApplicationException("Seleccione un Archivo")
		End If
	End Sub

	Sub CargarDatos()
		Try
			If gvGertakariak_Propiedades.IdSeleccionado Is Nothing Then Throw New ApplicationException("Registro no seleccionado")
			'-----------------------------------------------------------------------------
			'Datos de la Incidencia
			'-----------------------------------------------------------------------------
			Incidencia = (From gtk As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK _
						  Where gtk.ID = gvGertakariak_Propiedades.IdSeleccionado Select gtk).SingleOrDefault
			'-----------------------------------------------------------------------------
			TituloNumNC.Texto = ItzultzaileWeb.Itzuli(TituloNumNC.Texto) & ": " & CodigoNC(Incidencia) 'Incidencia.ID
		Catch ex As ApplicationException
			Throw
		Catch ex As Exception
			Log.Error(ex)
			Throw
		End Try
	End Sub
#End Region

	

End Class