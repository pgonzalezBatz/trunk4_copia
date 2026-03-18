Public Class E56_Acciones
	Inherits PageBase

#Region "Propiedades"
	''' <summary>
	''' Entidades de la base de datos.
	''' </summary>
	''' <remarks></remarks>
	Dim BBDD As New BatzBBDD.Entities_Gertakariak
	Dim Incidencia As New BatzBBDD.GERTAKARIAK
	Dim Accion As New BatzBBDD.ACCIONES

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
	Public Property gvAcciones_Propiedades As gtkGridView
		Get
			If (Session("gvAcciones_Propiedades") Is Nothing) Then Session("gvAcciones_Propiedades") = New gtkGridView
			Return CType(Session("gvAcciones_Propiedades"), gtkGridView)
		End Get
		Set(value As gtkGridView)
			Session("gvAcciones_Propiedades") = value
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
#End Region

#Region "Eventos de Pagina"
	Private Sub Acciones_Init(sender As Object, e As System.EventArgs) Handles Me.Init
		Try
			'#If DEBUG Then
			'			If (gvGertakariak_Propiedades Is Nothing OrElse gvGertakariak_Propiedades.IdSeleccionado Is Nothing) Then
			'				gvGertakariak_Propiedades.IdSeleccionado = 23802
			'			End If
			'			txtFechaApertura.Text = Now.ToShortDateString
			'#End If
			CargarDatos()
		Catch ex As ApplicationException
			Master.ascx_Mensajes.MensajeError(ex)
		Catch ex As Exception
			Log.Error(ex)
			Master.ascx_Mensajes.MensajeError(ex)
		End Try
	End Sub
#End Region

#Region "Eventos de Objetos"
	Private Sub ace_txtDesc_Init(sender As Object, e As EventArgs) Handles ace_txtDesc.Init
		Dim obj As AjaxControlToolkit.AutoCompleteExtender = sender
		obj.ContextKey = FiltroGTK.TipoIncidencia
		obj.CompletionSetCount = 3 'IDTIPOACCION: 1-Contenedora (Inmediata), 2-Provisionales, 3-Definitivas, 4-Preventivas
		obj.UseContextKey = True
	End Sub

	Private Sub ace_txtEficacia_Init(sender As Object, e As EventArgs) Handles ace_txtEficacia.Init
		Dim obj As AjaxControlToolkit.AutoCompleteExtender = sender
		obj.ContextKey = FiltroGTK.TipoIncidencia
		obj.CompletionSetCount = 3 'IDTIPOACCION: 1-Contenedora (Inmediata), 2-Provisionales, 3-Definitivas, 4-Preventivas
		obj.UseContextKey = True
	End Sub
#Region "Panel de Acciones"
	Private Sub btnGuardar_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnGuardar.Click
        Try
            '------------------------------------------------
            'Comprobamos si es un registro NUEVO.
            '------------------------------------------------
            If Accion Is Nothing OrElse Accion.EntityKey Is Nothing Then
                Accion = New BatzBBDD.ACCIONES
                'Accion.GERTAKARIAK.Add(Incidencia)
                Incidencia.ACCIONES.Add(Accion)
            End If
            '------------------------------------------------
            Accion.FECHAINICIO = txtFechaApertura.Text
            Accion.FECHAPREVISTA = If(String.IsNullOrWhiteSpace(txtFechaPrevista.Text), New Nullable(Of Date), CDate(txtFechaPrevista.Text))
            'Accion.FECHAFIN = If(String.IsNullOrWhiteSpace(txtFechaCierre.Text), New Nullable(Of Date), CDate(txtFechaCierre.Text))
            Accion.DESCRIPCION = txtDesc.Text.Trim
            Accion.EFICACIA = txtEficacia.Text.Trim
            Accion.IDTIPOACCION = 3 '1-Contenedora (Inmediata), 2-Provisionales, 3-Definitivas, 4-Preventivas

            BBDD.SaveChanges()

            gvAcciones_Propiedades.IdSeleccionado = Accion.ID
            Response.Redirect("~/Incidencia/8D/Etapas/E56.aspx", True)
        Catch ex As Threading.ThreadAbortException
            'Evitamos el error "ThreadAbortException" para las redirecciones que fuerzan la finalización de la página (Response.Redirect("~", True)) y así agilizar el tiempo de carga. (ExpReg: (\W|^) Response.Redirect\("(\W|$) )
        Catch ex As ApplicationException
			Master.ascx_Mensajes.MensajeError(ex)
		Catch ex As Exception
			Log.Error(ex)
			Master.ascx_Mensajes.MensajeError(ex)
		End Try
	End Sub
	Private Sub btnEliminar_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnEliminar.Click
        Try
            BBDD.ACCIONES.DeleteObject(Accion)
            BBDD.SaveChanges()

            gvAcciones_Propiedades.IdSeleccionado = Nothing

            Response.Redirect("~/Incidencia/8D/Etapas/E56.aspx", True)
        Catch ex As Threading.ThreadAbortException
            'Evitamos el error "ThreadAbortException" para las redirecciones que fuerzan la finalización de la página (Response.Redirect("~", True)) y así agilizar el tiempo de carga. (ExpReg: (\W|^) Response.Redirect\("(\W|$) )
        Catch ex As Exception
			Log.Error(ex)
			Master.ascx_Mensajes.MensajeError(ex)
		End Try
	End Sub
#End Region
#End Region

#Region "Funciones y Procesos"
	Sub CargarDatos()
		If gvGertakariak_Propiedades.IdSeleccionado Is Nothing Then Throw New ApplicationException("Registro no seleccionado")
		'-----------------------------------------------------------------------------
		'Datos de la Incidencia
		'-----------------------------------------------------------------------------
		Incidencia = (From gtk As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK _
					  Where gtk.ID = gvGertakariak_Propiedades.IdSeleccionado Select gtk).SingleOrDefault
		'-----------------------------------------------------------------------------
		TituloNumNC.Texto = ItzultzaileWeb.Itzuli(TituloNumNC.Texto) & ": " & CodigoNC(Incidencia) 'Incidencia.ID
		If gvAcciones_Propiedades.IdSeleccionado IsNot Nothing Then
			Accion = (From Acc As BatzBBDD.ACCIONES In BBDD.ACCIONES Where Acc.ID = gvAcciones_Propiedades.IdSeleccionado Select Acc).SingleOrDefault

			txtFechaApertura.Text = If(IsDate(Accion.FECHAINICIO), Accion.FECHAINICIO.Value.ToShortDateString, New Nullable(Of Date))
			txtFechaPrevista.Text = If(IsDate(Accion.FECHAPREVISTA), Accion.FECHAPREVISTA.Value.ToShortDateString, New Nullable(Of Date))
			'txtFechaCierre.Text = If(IsDate(Accion.FECHAFIN), Accion.FECHAFIN.Value.ToShortDateString, New Nullable(Of Date))
			txtDesc.Text = Accion.DESCRIPCION
			txtEficacia.Text = Accion.EFICACIA
		Else
			'----------------------------------------------------------
			'Ocultamos ciertos controles cuando es una Nueva Acción.
			'----------------------------------------------------------
			btnEliminar.Visible = False
			txtFechaApertura.Text = If(IsDate(txtFechaApertura.Text), txtFechaApertura.Text, Now.Date)
			'----------------------------------------------------------
		End If
	End Sub
#End Region
End Class