Public Class Detalle
	Inherits PageBase

#Region "Propiedades"
	Dim BBDD As New BatzBBDD.Entities_Gertakariak
    Dim Incidencia As New BatzBBDD.GERTAKARIAK

    Public Property PostBackUrl As String
        Get
            Return ViewState("PostBackUrl")
        End Get
        Set(ByVal value As String)
            ViewState("PostBackUrl") = value
        End Set
    End Property

    Private Property _Responsable As Boolean
    ''' <summary>
    ''' Indicamos si el usuario en curso es responsable de la incidencia para mostrarle los botones.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Responsable As Boolean
        Get
            Return _Responsable
        End Get
        Set(value As Boolean)
            _Responsable = value
        End Set
    End Property

    Public ReadOnly Property Ticket As SabLib.ELL.Ticket
        Get
            Return Session("Ticket")
        End Get
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
#Region "Eventos Página"
    '#If DEBUG Then
    Private Sub Page_Init1(sender As Object, e As EventArgs) Handles Me.Init
        If Not IsPostBack Then
            tcIncidencia.ActiveTabIndex = tcIncidencia.Controls.IndexOf(tcIncidencia.FindControl(tpDatosGenerales.ID))
        End If
    End Sub
    '#End If
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Try
			If Not IsPostBack Then
				'Comprobamos si el identificador de la incidencia se esta pasando por "Request".
				If Request("IdIncidencia") IsNot Nothing Then
					Master.Propiedades_gvGertakariak.IdSeleccionado = Request("IdIncidencia")
					gvGertakariak_Propiedades.IdSeleccionado = If(String.IsNullOrWhiteSpace(Request("IdIncidencia")), New Nullable(Of Integer), CInt(Request("IdIncidencia")))
				End If
			End If
			If Master.Propiedades_gvGertakariak.IdSeleccionado Is Nothing Then Response.Redirect("~", False) Else CargarDatos()
		Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
			Dim msg As String = "IdIncidencia: " & Incidencia.ID
			msg &= vbNewLine & "Master.Propiedades_gvGertakariak.IdSeleccionado: " & Master.Propiedades_gvGertakariak.IdSeleccionado
			log.Error(msg, ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
	End Sub
    'Private Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
    '	TituloPagina.Texto = ItzultzaileWeb.Itzuli("numIncidencia") & ": " & Incidencia.ID
    'End Sub
    Private Sub tcIncidencia_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles tcIncidencia.Load
		If Not IsPostBack Then
			'-------------------------------------------------------------
			'Indicamos que pestaña es la que hay que activar.
			'-------------------------------------------------------------
			Dim NombrePagina As String = If(Request.UrlReferrer Is Nothing, String.Empty, Request.UrlReferrer.AbsolutePath.Trim)
			If NombrePagina <> String.Empty Then
				Select Case NombrePagina.Substring(NombrePagina.LastIndexOf("/") + 1).ToUpper
					Case "Detalle.aspx".ToUpper
						tcIncidencia.ActiveTabIndex = tcIncidencia.Controls.IndexOf(tcIncidencia.FindControl(tpDatosGenerales.ID)) ' 0
					Case "Accion.aspx".ToUpper, "Observaciones.aspx".ToUpper
						tcIncidencia.ActiveTabIndex = tcIncidencia.Controls.IndexOf(tcIncidencia.FindControl(tpAcciones.ID)) '1
					Case "Documento.aspx".ToUpper
						tcIncidencia.ActiveTabIndex = tcIncidencia.Controls.IndexOf(tcIncidencia.FindControl(tp_Documentos.ID))	'2	'Panel de "Documentos"
				End Select
			End If
			'-------------------------------------------------------------
		End If
	End Sub

	Protected WithEvents gvObservaciones As Global.System.Web.UI.WebControls.GridView
	Private Sub gvObservaciones_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvObservaciones.RowCreated
		LisView_gvObservaciones_RowCreated(sender, e)
	End Sub
#End Region
#Region "Eventos de Objetos"
#Region "Acciones"
	Private Sub btnEliminar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnEliminar.Click
        Try
            If Incidencia Is Nothing Then Throw New ApplicationException("noExisteNingunRegistro")
            BBDD.DeleteObject(Incidencia)
            BBDD.SaveChanges()

            'btnVolver_Click(Nothing, Nothing)
            Response.Redirect("~/Default.aspx", False)
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
	End Sub
	Private Sub btnEditar_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnEditar.Click
		'Debe ir siempre a la pagina de "Datos Genereles".
		Response.Redirect("~/Incidencia/Mantenimiento/DatosGenerales.aspx", False)
	End Sub
	Private Sub btnNuevaAccion_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnNuevaAccion.Click
		Response.Redirect("~/Incidencia/Mantenimiento/Accion.aspx", False)	'Debe ir siempre a esta pagina.
	End Sub
	Private Sub btnNuevaIncidencia_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnNuevaIncidencia.Click
		Master.Propiedades_gvGertakariak.IdSeleccionado = Nothing
		gvGertakariak_Propiedades.IdSeleccionado = Nothing
		Response.Redirect("~/Incidencia/Mantenimiento/DatosGenerales.aspx", False)
	End Sub
#Region "Contenedora"
	Private Sub lvAcciones_ItemCreated(sender As Object, e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles lvAcciones_Contenedoras.ItemCreated
		Dim ListView As ListView = sender
		If ListView.Controls.Count > 0 Then
			Dim lvDataItem As ListViewDataItem = e.Item
			'Dim Accion As gtkAcciones = lvDataItem.DataItem
			Dim Accion As BatzBBDD.ACCIONES = lvDataItem.DataItem
			If Accion IsNot Nothing Then
				Dim Observaciones As List(Of gtkAccionesObservaciones) = New gtkAccionesObservaciones() With {.IdAccion = Accion.ID}.Listado
				gvObservaciones = lvDataItem.FindControl("gvObservaciones")
				gvObservaciones.DataSource = Observaciones
				gvObservaciones.Ordenar("Fecha", SortDirection.Descending)

				Dim lvAccion As HtmlTable = lvDataItem.FindControl("lvAccion")
				If lvAccion IsNot Nothing Then lvAccion.BgColor = If(lvDataItem.DisplayIndex Mod 2 = 0, "#F7F6F3", "#FFFFFF")

				Dim lblDemora As Label = lvDataItem.FindControl("lblDemora")
                lblDemora.Text = If(Accion.FECHAINICIO Is Nothing, String.Empty,
                                    DateDiff(DateInterval.WeekOfYear, Accion.FECHAINICIO.Value, If(IsDate(Accion.FECHAFIN), Accion.FECHAFIN.Value, Now), Microsoft.VisualBasic.FirstDayOfWeek.Monday, FirstWeekOfYear.FirstFourDays))
            End If
		End If
	End Sub
	Private Sub lvAcciones_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles lvAcciones_Contenedoras.ItemDataBound
		ListView_ItemDataBound(sender, e)
	End Sub
	Private Sub lvAcciones_ItemCommand(sender As Object, e As System.Web.UI.WebControls.ListViewCommandEventArgs) Handles lvAcciones_Contenedoras.ItemCommand
		ListView_ItemCommand(sender, e)
	End Sub
	Private Sub lvAcciones_ItemEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewEditEventArgs) Handles lvAcciones_Contenedoras.ItemEditing
		ListView_ItemEditing(sender, e)
	End Sub
#End Region
#Region "Provisionales"
	Private Sub lvAcciones_Provisional_ItemCommand(sender As Object, e As ListViewCommandEventArgs) Handles lvAcciones_Provisional.ItemCommand
		ListView_ItemCommand(sender, e)
	End Sub
	Private Sub lvAcciones_Provisional_ItemCreated(sender As Object, e As ListViewItemEventArgs) Handles lvAcciones_Provisional.ItemCreated
		Dim ListView As ListView = sender
		If ListView.Controls.Count > 0 Then
			Dim lvDataItem As ListViewDataItem = e.Item
			'Dim Accion As gtkAcciones = lvDataItem.DataItem
			Dim Accion As BatzBBDD.ACCIONES = lvDataItem.DataItem
			If Accion IsNot Nothing Then
				Dim Observaciones As List(Of gtkAccionesObservaciones) = New gtkAccionesObservaciones() With {.IdAccion = Accion.ID}.Listado
				gvObservaciones = lvDataItem.FindControl("gvObservaciones_Provisional")
				gvObservaciones.DataSource = Observaciones
				gvObservaciones.Ordenar("Fecha", SortDirection.Descending)

				Dim lvAccion As HtmlTable = lvDataItem.FindControl("lvAccion")
				If lvAccion IsNot Nothing Then lvAccion.BgColor = If(lvDataItem.DisplayIndex Mod 2 = 0, "#F7F6F3", "#FFFFFF")

				Dim lblDemora As Label = lvDataItem.FindControl("lblDemora")
				lblDemora.Text = If(Accion.FECHAINICIO Is Nothing, String.Empty, _
									DateDiff(DateInterval.WeekOfYear, Accion.FECHAINICIO.Value, If(IsDate(Accion.FECHAFIN), Accion.FECHAFIN.Value, Now), Microsoft.VisualBasic.FirstDayOfWeek.Monday, FirstWeekOfYear.FirstFourDays))
			End If
		End If
	End Sub
	Private Sub lvAcciones_Provisional_ItemDataBound(sender As Object, e As ListViewItemEventArgs) Handles lvAcciones_Provisional.ItemDataBound
		ListView_ItemDataBound(sender, e)
	End Sub
	Private Sub lvAcciones_Provisional_ItemEditing(sender As Object, e As ListViewEditEventArgs) Handles lvAcciones_Provisional.ItemEditing
		ListView_ItemEditing(sender, e)
	End Sub
#End Region
#Region "Definitivas"
	Private Sub lvAcciones_Definitivas_ItemCommand(sender As Object, e As ListViewCommandEventArgs) Handles lvAcciones_Definitivas.ItemCommand
		ListView_ItemCommand(sender, e)
	End Sub
	Private Sub lvAcciones_Definitivas_ItemCreated(sender As Object, e As ListViewItemEventArgs) Handles lvAcciones_Definitivas.ItemCreated
		Dim ListView As ListView = sender
		If ListView.Controls.Count > 0 Then
			Dim lvDataItem As ListViewDataItem = e.Item
			'Dim Accion As gtkAcciones = lvDataItem.DataItem
			Dim Accion As BatzBBDD.ACCIONES = lvDataItem.DataItem
			If Accion IsNot Nothing Then
				Dim Observaciones As List(Of gtkAccionesObservaciones) = New gtkAccionesObservaciones() With {.IdAccion = Accion.ID}.Listado
				gvObservaciones = lvDataItem.FindControl("gvObservaciones_Definitiva")
				gvObservaciones.DataSource = Observaciones
				gvObservaciones.Ordenar("Fecha", SortDirection.Descending)

				Dim lvAccion As HtmlTable = lvDataItem.FindControl("lvAccion")
				If lvAccion IsNot Nothing Then lvAccion.BgColor = If(lvDataItem.DisplayIndex Mod 2 = 0, "#F7F6F3", "#FFFFFF")

				Dim lblDemora As Label = lvDataItem.FindControl("lblDemora")
				lblDemora.Text = If(Accion.FECHAINICIO Is Nothing, String.Empty, _
									DateDiff(DateInterval.WeekOfYear, Accion.FECHAINICIO.Value, If(IsDate(Accion.FECHAFIN), Accion.FECHAFIN.Value, Now), Microsoft.VisualBasic.FirstDayOfWeek.Monday, FirstWeekOfYear.FirstFourDays))
			End If
		End If
	End Sub
	Private Sub lvAcciones_Definitivas_ItemDataBound(sender As Object, e As ListViewItemEventArgs) Handles lvAcciones_Definitivas.ItemDataBound
		ListView_ItemDataBound(sender, e)
	End Sub
	Private Sub lvAcciones_Definitivas_ItemEditing(sender As Object, e As ListViewEditEventArgs) Handles lvAcciones_Definitivas.ItemEditing
		ListView_ItemEditing(sender, e)
	End Sub
#End Region
#Region "Preventivas"
	Private Sub lvAcciones_Preventivas_ItemCommand(sender As Object, e As ListViewCommandEventArgs) Handles lvAcciones_Preventivas.ItemCommand
		ListView_ItemCommand(sender, e)
	End Sub
	Private Sub lvAcciones_Preventivas_ItemCreated(sender As Object, e As ListViewItemEventArgs) Handles lvAcciones_Preventivas.ItemCreated
		Dim ListView As ListView = sender
		If ListView.Controls.Count > 0 Then
			Dim lvDataItem As ListViewDataItem = e.Item
			'Dim Accion As gtkAcciones = lvDataItem.DataItem
			Dim Accion As BatzBBDD.ACCIONES = lvDataItem.DataItem
			If Accion IsNot Nothing Then
				Dim Observaciones As List(Of gtkAccionesObservaciones) = New gtkAccionesObservaciones() With {.IdAccion = Accion.ID}.Listado
				gvObservaciones = lvDataItem.FindControl("gvObservaciones_Preventiva")
				gvObservaciones.DataSource = Observaciones
				gvObservaciones.Ordenar("Fecha", SortDirection.Descending)

				Dim lvAccion As HtmlTable = lvDataItem.FindControl("lvAccion")
				If lvAccion IsNot Nothing Then lvAccion.BgColor = If(lvDataItem.DisplayIndex Mod 2 = 0, "#F7F6F3", "#FFFFFF")

				Dim lblDemora As Label = lvDataItem.FindControl("lblDemora")
				lblDemora.Text = If(Accion.FECHAINICIO Is Nothing, String.Empty, _
									DateDiff(DateInterval.WeekOfYear, Accion.FECHAINICIO.Value, If(IsDate(Accion.FECHAFIN), Accion.FECHAFIN.Value, Now), Microsoft.VisualBasic.FirstDayOfWeek.Monday, FirstWeekOfYear.FirstFourDays))
			End If
		End If
	End Sub
	Private Sub lvAcciones_Preventivas_ItemDataBound(sender As Object, e As ListViewItemEventArgs) Handles lvAcciones_Preventivas.ItemDataBound
		ListView_ItemDataBound(sender, e)
	End Sub
	Private Sub lvAcciones_Preventivas_ItemEditing(sender As Object, e As ListViewEditEventArgs) Handles lvAcciones_Preventivas.ItemEditing
		ListView_ItemEditing(sender, e)
	End Sub
#End Region
#End Region
#Region "Panel de Documentos"
	Private Sub gvDocumentos_Init(sender As Object, e As System.EventArgs) Handles gvDocumentos.Init
		Dim Tabla As GridView = sender
		Tabla.CrearBotones()
	End Sub
	Private Sub gvDocumentos_RowCreated(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDocumentos.RowCreated
		Dim Tabla As GridView = sender
		Dim Fila As GridViewRow = e.Row
		If Fila.DataItem IsNot Nothing Then
			Dim Documento As BatzBBDD.DOCUMENTOS = Fila.DataItem
			'Indicamos se es el registro seleccionado.
			If Documento.ID = gvDocumentos_Propiedades.IdSeleccionado Then Fila.RowState = DataControlRowState.Selected

			'-------------------------------------------------------------------------------------------
			'Ponemos el icono correspondiente para cada tipo de archivo.
			'-------------------------------------------------------------------------------------------
			Dim hlDoc As New HyperLink
			Dim imgDoc As New Image
			hlDoc = Fila.FindControl("hlDoc")
			If hlDoc IsNot Nothing Then
				hlDoc.NavigateUrl &= "?Id_Doc=" & Documento.ID
				imgDoc = Fila.FindControl("imgDoc")

				'Dim CONTENT_TYPE As String = Right(Documento.CONTENT_TYPE, (Documento.CONTENT_TYPE.Length - InStr(Documento.CONTENT_TYPE, "/")))
				imgDoc.ImageUrl = ImagenDocumento(Documento.CONTENT_TYPE)
			End If
			'-------------------------------------------------------------------------------------------
		End If
	End Sub
	Private Sub gvDocumentos_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDocumentos.RowDataBound
		Dim Fila As GridViewRow = e.Row
		Fila.CrearAccionesFila()
	End Sub
	Private Sub gvDocumentos_RowEditing(sender As Object, e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvDocumentos.RowEditing
		Dim Tabla As System.Web.UI.WebControls.GridView = sender
		e.Cancel = True	'Cancelamos la ejecucion de este evento.

		Tabla.SelectedIndex = e.NewEditIndex
		gvDocumentos_SelectedIndexChanged(sender, e)
		gvDocumentos_Propiedades.IdSeleccionado = CType(Tabla.SelectedDataKey.Value, String)
		'gvAcciones_Propiedades = Nothing
		Response.Redirect("~/Incidencia/Mantenimiento/Documento.aspx")
	End Sub
	Private Sub gvDocumentos_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvDocumentos.SelectedIndexChanged
		Dim Tabla As GridView = sender
		gvDocumentos_Propiedades.IdSeleccionado = CType(Tabla.SelectedDataKey.Value, Integer)
	End Sub
	Private Sub gvDocumentos_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDocumentos.PageIndexChanging
		Dim Tabla As GridView = sender
		Tabla.PageIndex = e.NewPageIndex
		gvDocumentos_Propiedades.Pagina = Tabla.PageIndex
	End Sub
	Private Sub gvDocumentos_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvDocumentos.Sorting
		Dim Tabla As GridView = sender
		'-------------------------------------------------------------------------------------------------------------
		'Criterio de Ordenacion:
		'Ascendente si no hay criterio o es Descendente el criterio que existe o el campo de ordenacion es distinto.
		'-------------------------------------------------------------------------------------------------------------
		If IsPostBack Then
			If gvDocumentos_Propiedades.DireccionOrdenacion IsNot Nothing _
			 AndAlso gvDocumentos_Propiedades.DireccionOrdenacion.Value = SortDirection.Ascending _
			 And gvDocumentos_Propiedades.CampoOrdenacion = e.SortExpression Then
				gvDocumentos_Propiedades.DireccionOrdenacion = SortDirection.Descending
			ElseIf gvDocumentos_Propiedades.DireccionOrdenacion.GetValueOrDefault = SortDirection.Descending _
			 Or gvDocumentos_Propiedades.DireccionOrdenacion Is Nothing _
			 Or gvDocumentos_Propiedades.CampoOrdenacion <> e.SortExpression Then
				gvDocumentos_Propiedades.DireccionOrdenacion = SortDirection.Ascending
			End If
		End If
		'-------------------------------------------------------------------------------------------------------------
		gvDocumentos_Propiedades.CampoOrdenacion = e.SortExpression
	End Sub
	Private Sub gvDocumentos_PreRender(sender As Object, e As System.EventArgs) Handles gvDocumentos.PreRender
		Dim Tabla As GridView = sender
		'Tabla.Ordenar(gvDocumentos_Propiedades.CampoOrdenacion, gvDocumentos_Propiedades.DireccionOrdenacion.GetValueOrDefault)
		Tabla.Ordenar(If(String.IsNullOrWhiteSpace(gvDocumentos_Propiedades.CampoOrdenacion), Tabla.DataKeyNames(0).ToString, gvDocumentos_Propiedades.CampoOrdenacion), If(gvDocumentos_Propiedades.DireccionOrdenacion Is Nothing, SortDirection.Descending, gvDocumentos_Propiedades.DireccionOrdenacion.GetValueOrDefault))
		Tabla.PageIndex = If(gvDocumentos_Propiedades.Pagina, 0)
		Tabla.DataBind()
	End Sub
	Private Sub btnNuevoDocumento_Click(sender As Object, e As ImageClickEventArgs) Handles btnNuevoDocumento.Click
		gvDocumentos_Propiedades.IdSeleccionado = Nothing
		'gvAcciones_Propiedades = Nothing
		Response.Redirect("~/Incidencia/Mantenimiento/Documento.aspx")
	End Sub
#End Region
#End Region
#Region "Procesos y Funciones"
	Sub CargarDatos()
		'----------------------------------------------------------------
		'Cargamos los datos de la Incidencia.
		'----------------------------------------------------------------
		Incidencia = (From gtk As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK Where gtk.ID = Master.Propiedades_gvGertakariak.IdSeleccionado Select gtk).SingleOrDefault
        '----------------------------------------------------------------

        '-------------------------------------------------------
        'Pintamos los datos del Objeto.
        '-------------------------------------------------------
        If Incidencia Is Nothing Then
            Throw New ApplicationException("noExisteNingunRegistro", New ApplicationException)
        Else
            TituloPagina.Texto = ItzultzaileWeb.Itzuli("numIncidencia") & ": " & Incidencia.ID

            lblFechaApertura.Text = If(IsDate(Incidencia.FECHAAPERTURA), Incidencia.FECHAAPERTURA.Value.ToShortDateString, New Nullable(Of Date))
			lblFechaCierre.Text = If(IsDate(Incidencia.FECHACIERRE), Incidencia.FECHACIERRE.Value.ToShortDateString, New Nullable(Of Date))
			lblDemora.Text = If(Incidencia.FECHAAPERTURA Is Nothing, String.Empty, _
									DateDiff(DateInterval.WeekOfYear, Incidencia.FECHAAPERTURA.Value, If(IsDate(Incidencia.FECHACIERRE), Incidencia.FECHACIERRE.Value, Now), Microsoft.VisualBasic.FirstDayOfWeek.Monday, FirstWeekOfYear.FirstFourDays))
			lblDescripcionProblema.Text = If(Incidencia.DESCRIPCIONPROBLEMA Is Nothing, Nothing, Incidencia.DESCRIPCIONPROBLEMA.Replace(vbCrLf, "<BR/>"))

			'----------------------------------------------------------------------------------------------------
			'Obtenemos los Responsables de la NC.
			'----------------------------------------------------------------------------------------------------
			If Incidencia.RESPONSABLES_GERTAKARIAK.Any Then
				Dim fSAB As New SabLib.BLL.UsuariosComponent
				For Each Reg As BatzBBDD.RESPONSABLES_GERTAKARIAK In Incidencia.RESPONSABLES_GERTAKARIAK
					Dim Resp As SabLib.ELL.Usuario = fSAB.GetUsuario(New SabLib.ELL.Usuario With {.Id = Reg.IDUSUARIO}, False)
					blResponsables.Items.Add(New ListItem With {.Text = Resp.NombreCompleto, .Value = Resp.Id})
					Responsable = (Resp.Id = Ticket.IdUser)	  'Indicamos si el usuario en curso es resposable de esta incidencia.
				Next
			End If
			'----------------------------------------------------------------------------------------------------

			'----------------------------------------------------------------
			'Datos de la "Instalacion"
			'----------------------------------------------------------------
			If Not String.IsNullOrWhiteSpace(Incidencia.IDACTIVO) Then
                Dim Instalacion As BatzBBDD.W_ASSET = (From Reg As BatzBBDD.W_ASSET In BBDD.W_ASSET Where Reg.COMPANY = "IGORRE_S" And Reg.ASSET = Incidencia.IDACTIVO).SingleOrDefault
                lblInstalacion.Text = Instalacion.ASSETNAME
                lblFamilia.Text = Instalacion.ASSET 'ASSET1_ASSET.ASSETNAME
            End If
			'----------------------------------------------------------------

			'----------------------------------------------------------------
			'Tipo de Incidencia
			'----------------------------------------------------------------
			lblTipoIncidencia.Text = If(Incidencia.IDTIPOINCIDENCIA = 0, _
										String.Empty, _
										If(Incidencia.ESTRUCTURA.Any, Incidencia.ESTRUCTURA.SingleOrDefault.DESCRIPCION, String.Empty))
            '----------------------------------------------------------------
            '-------------------------------------------------------------------------------------
            'Mostramos u Ocultamos los paneles con los botones dependiendo del perfil del usuario 
            'y de si es reponsable de la Incidencia.
            '-------------------------------------------------------------------------------------
            If Master.PerfilUsuario = MPWeb.Perfil.Consultor And Responsable = False Then
                pnBotones_DatosGenerales.Visible = False
                pnBotones_Acciones.Visible = False
                'Response.Write("<script>console.log('false');</script>")
                'Else
                '    pnBotones_DatosGenerales.Visible = False
                '    pnBotones_Acciones.Visible = False
                '    Response.Write("<script>console.log('perf: " & Master.PerfilUsuario & "');</script>")
                '    Response.Write("<script>console.log('resp: " & Responsable & "');</script>")
            End If
			'-------------------------------------------------------------------------------------
			CargarAcciones(Incidencia.ID)

			'-------------------------------------------------------------------------------------------------------
			'Documentos
			'-------------------------------------------------------------------------------------------------------
			Dim lDocumentos As List(Of BatzBBDD.DOCUMENTOS)
			lDocumentos = Incidencia.DOCUMENTOS.OrderByDescending(Function(o) o.FECHACREACION).ToList
			gvDocumentos.DataSource = lDocumentos
			gvDocumentos.DataBind()
			'-------------------------------------------------------------------------------------------------------
		End If
		'-------------------------------------------------------
	End Sub
	Sub CargarAcciones(ByVal IdIncidencia As Integer)
		'Contenedora = 1 / Provisional = 2 / Definitiva = 3 / Preventiva = 4
		Dim lContenedoras = Incidencia.ACCIONES.Where(Function(o) o.IDTIPOACCION <= 1 Or o.IDTIPOACCION = Nothing).Select(Function(Reg) Reg).OrderBy(Function(Reg) Reg.FECHAINICIO).ThenBy(Function(Reg) Reg.FECHAPREVISTA).ThenBy(Function(Reg) Reg.FECHAFIN)
		Dim lProvisional = Incidencia.ACCIONES.Where(Function(o) o.IDTIPOACCION = 2).Select(Function(Reg) Reg).OrderBy(Function(Reg) Reg.FECHAINICIO).ThenBy(Function(Reg) Reg.FECHAPREVISTA).ThenBy(Function(Reg) Reg.FECHAFIN)
		Dim lDefinitiva = Incidencia.ACCIONES.Where(Function(o) o.IDTIPOACCION = 3).Select(Function(Reg) Reg).OrderBy(Function(Reg) Reg.FECHAINICIO).ThenBy(Function(Reg) Reg.FECHAPREVISTA).ThenBy(Function(Reg) Reg.FECHAFIN)
		Dim lPreventiva = Incidencia.ACCIONES.Where(Function(o) o.IDTIPOACCION = 4).Select(Function(Reg) Reg).OrderBy(Function(Reg) Reg.FECHAINICIO).ThenBy(Function(Reg) Reg.FECHAPREVISTA).ThenBy(Function(Reg) Reg.FECHAFIN)

		cpeContenedora.Collapsed = Not (lContenedoras.Any)
		cpeProvisional.Collapsed = Not (lProvisional.Any)
		cpeDefinitiva.Collapsed = Not (lDefinitiva.Any)
		cpePreventiva.Collapsed = Not (lPreventiva.Any)

		lvAcciones_Contenedoras.DataSource = lContenedoras : lvAcciones_Contenedoras.DataBind()
		lvAcciones_Provisional.DataSource = lProvisional : lvAcciones_Provisional.DataBind()
		lvAcciones_Definitivas.DataSource = lDefinitiva : lvAcciones_Definitivas.DataBind()
		lvAcciones_Preventivas.DataSource = lPreventiva : lvAcciones_Preventivas.DataBind()
	End Sub
#Region "Acciones"
	Private Sub ListView_ItemCreated(sender As Object, e As System.Web.UI.WebControls.ListViewItemEventArgs)
		Dim ListView As ListView = sender
		If ListView.Controls.Count > 0 Then
			Dim lvDataItem As ListViewDataItem = e.Item
			Dim Accion As BatzBBDD.ACCIONES = lvDataItem.DataItem
			If Accion IsNot Nothing Then
				Dim Observaciones As List(Of gtkAccionesObservaciones) = New gtkAccionesObservaciones() With {.IdAccion = Accion.ID}.Listado
				gvObservaciones = lvDataItem.FindControl("gvObservaciones")
				gvObservaciones.DataSource = Observaciones
				gvObservaciones.Ordenar("Fecha", SortDirection.Descending)

				Dim lvAccion As HtmlTable = lvDataItem.FindControl("lvAccion")
				If lvAccion IsNot Nothing Then lvAccion.BgColor = If(lvDataItem.DisplayIndex Mod 2 = 0, "#F7F6F3", "#FFFFFF")

				Dim lblDemora As Label = lvDataItem.FindControl("lblDemora")
				lblDemora.Text = If(Accion.FECHAINICIO Is Nothing, String.Empty, _
									DateDiff(DateInterval.WeekOfYear, Accion.FECHAINICIO.Value, If(IsDate(Accion.FECHAFIN), Accion.FECHAFIN.Value, Now), Microsoft.VisualBasic.FirstDayOfWeek.Monday, FirstWeekOfYear.FirstFourDays))
			End If
		End If
	End Sub
	Private Sub ListView_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs)
		Dim blResponsables As System.Web.UI.WebControls.BulletedList = e.Item.FindControl("blResponsablesAcciones")
		Dim lvDataItem As ListViewDataItem = e.Item
		'Dim Accion As gtkAcciones = lvDataItem.DataItem
		Dim Accion As BatzBBDD.ACCIONES = lvDataItem.DataItem

		Dim MostrarBotones As Boolean = (Master.PerfilUsuario = MPWeb.Perfil.Administrador Or Master.PerfilUsuario = MPWeb.Perfil.Usuario)
		'Dim Ticket As Sablib.ELL.Ticket = Session("Ticket")
		'------------------------------------------------------------
		'Buscamos los responsables de la accion para mostrarlos.
		'------------------------------------------------------------
		Dim lUsuarios As List(Of gtkAccionesUsuarios) = New gtkAccionesUsuarios() With {.IdAccion = Accion.ID}.Listado
		If lUsuarios IsNot Nothing AndAlso lUsuarios.Any Then
			For Each Usr As gtkAccionesUsuarios In lUsuarios
				Dim fSab As New SabLib.BLL.UsuariosComponent
				Dim UsrSab As SabLib.ELL.Usuario = fSab.GetUsuario(New SabLib.ELL.Usuario With {.Id = Usr.IdUsuario}, False)
				If UsrSab IsNot Nothing Then blResponsables.Items.Add(New ListItem With {.Text = UsrSab.NombreCompleto, .Value = UsrSab.Id})
				MostrarBotones = If(MostrarBotones = False AndAlso Ticket.IdUser = Usr.IdUsuario, True, MostrarBotones)
			Next
		End If
		'------------------------------------------------------------

		'-----------------------------------------------------------------------------------------
		'Ocultamos los elementos que el usuario Consultor no puede ver.
		'-----------------------------------------------------------------------------------------
		Dim btnEditarAccion As LinkButton = lvDataItem.FindControl("btnEditarAccion")
		Dim btnNuevaObservacion As LinkButton = lvDataItem.FindControl("btnNuevaObservacion")
		btnEditarAccion.Visible = MostrarBotones
		btnNuevaObservacion.Visible = btnEditarAccion.Visible
		'-----------------------------------------------------------------------------------------
	End Sub
	Private Sub ListView_ItemCommand(sender As Object, e As System.Web.UI.WebControls.ListViewCommandEventArgs)
		Dim Lista As ListView = sender
		Dim item As ListViewDataItem = e.Item
		If String.Compare(e.CommandName, "NuevaObservacion", True) = 0 Then
			Lista.SelectedIndex = item.DataItemIndex
			Response.Redirect("~/Incidencia/Mantenimiento/Observaciones.aspx?IdAccion=" & Lista.SelectedValue, False)
		End If
	End Sub
	Private Sub ListView_ItemEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewEditEventArgs)
		Dim ListView As ListView = sender
		e.Cancel = True
		ListView.SelectedIndex = e.NewEditIndex
		Response.Redirect("~/Incidencia/Mantenimiento/Accion.aspx?IdAccion=" & ListView.SelectedValue, True)
	End Sub
	Private Sub LisView_gvObservaciones_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
		Dim Tabla As GridView = sender
		Dim CrearAtributos As Boolean = True
		If e.Row.DataItem IsNot Nothing Then
			'-----------------------------------------------------------------------------------------------------
			'Comprobamos que el "Consultor" sea responsable de la "Accion" para poder modificar la "Observacion".
			'-----------------------------------------------------------------------------------------------------
			If Master.PerfilUsuario = MPWeb.Perfil.Consultor Then
				Dim Accion As BatzBBDD.ACCIONES = CType(Tabla.NamingContainer, ListViewDataItem).DataItem
				CrearAtributos = (Accion.ACCIONES_USUARIOS.Any AndAlso Accion.ACCIONES_USUARIOS.Where(Function(o) o.IDUSUARIO = Ticket.IdUser).Any)
			End If
			'-----------------------------------------------------------------------------------------------------

			If CrearAtributos = True Then
				'-----------------------------------------------------------------------------------------
				'Seleccion del registro seleccionando la fila.
				'-----------------------------------------------------------------------------------------
				e.Row.Attributes.Add("onmouseover", "this.style.cursor=""pointer""")
				e.Row.Attributes("OnClick") = "location.href='Mantenimiento/Observaciones.aspx?IdObservacion=" & Tabla.DataKeys(e.Row.DataItemIndex).Value & "';"
				'-----------------------------------------------------------------------------------------
				'Resaltamos el 1º registro de la tabla. ¿PQ?
				'If e.Row.DataItemIndex = 0 Then e.Row.RowState = DataControlRowState.Selected
			End If
		End If
	End Sub

#End Region
#End Region
End Class