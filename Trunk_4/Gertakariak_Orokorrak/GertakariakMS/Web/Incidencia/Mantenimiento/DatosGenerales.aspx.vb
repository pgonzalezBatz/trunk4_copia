Imports System.Reflection

Public Class DatosGenerales
	Inherits PageBase

#Region "Propiedades"
	Dim Gertakaria As New gtkMatenimientoSist
	ReadOnly Property lstInstalacion As List(Of PrismaLib.Asset_BLL)
		Get
			Return New PrismaLib.Asset_BLL() With {.Company = PrismaLib.Entidades.Asset_ELL.Planta.IGORRE_S, .CompanyLevel = 4}.Listado
		End Get
	End Property
	Public Property PostBackUrl As String
		Get
			Return ViewState("PostBackUrl")
		End Get
		Set(ByVal value As String)
			ViewState("PostBackUrl") = value
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
#Region "Eventos de Pagina"
#If DEBUG Then
    Private Sub Page_PreInit1(sender As Object, e As System.EventArgs) Handles Me.PreInit
        Master.Propiedades_gvGertakariak.IdSeleccionado = IIf(Session("Propiedades_gvGertakariak") IsNot Nothing, Master.Propiedades_gvGertakariak.IdSeleccionado, 15777)
    End Sub
#End If
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Master.Propiedades_gvGertakariak.IdSeleccionado IsNot Nothing Then
                CargarDatos()
            Else
                '-------------------------------------------------------------------------------------------------------
                'Datos Iniciales
                '-------------------------------------------------------------------------------------------------------
                txtFechaApertura.Text = If(txtFechaApertura.Text.Trim = String.Empty, Date.Today, txtFechaApertura.Text.Trim)
                '-------------------------------------------------------------------------------------------------------
            End If
            'Catch ex As BatzException	'Guardar el Log del Error automaticamente
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception 'Hay que forzar el Log del Error manualmente
            log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Private Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        TituloPagina.Texto = "numIncidencia".Itzuli & ": " & If(Gertakaria.Id IsNot Nothing, Gertakaria.Id, Master.Propiedades_gvGertakariak.IdSeleccionado)
    End Sub
    Private Sub lblDemora_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblDemora.Load
        If IsPostBack Then
            '-------------------------------------------------------------------------
            'Cargamos las fechas en el objeto para hacer el calculo de la demora.
            '-------------------------------------------------------------------------
            Gertakaria.FechaApertura = If(IsDate(txtFechaApertura.Text), CDate(txtFechaApertura.Text), New Nullable(Of Date))
            Gertakaria.FechaCierre = If(IsDate(txtFechaCierre.Text), CDate(txtFechaCierre.Text), New Nullable(Of Date))
            '-------------------------------------------------------------------------
        End If
        lblDemora.Text = Gertakaria.Demora.ToString
    End Sub
    Private Sub ddlFamilia_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFamilia.Load
        If Not IsPostBack Then
            Dim lstFamilia As List(Of PrismaLib.Asset_BLL) = lFamilia()
            ddlFamilia.SelectedValue = (From i As PrismaLib.Asset_BLL In lstInstalacion Where i.Asset = Gertakaria.Instalacion Select i.ParentAsset).FirstOrDefault
            ddlFamilia.DataSource = lstFamilia.OrderBy(Function(o) o.AssetName)
            ddlFamilia.DataBind()
        End If
    End Sub
    Private Sub ddlInstalacion_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlInstalacion.Load
        If Not IsPostBack Then CargarInstalacion()
    End Sub
    Private Sub btnVolver_Click(sender As Object, e As System.EventArgs) Handles btnVolver.Click
        Response.Redirect(PostBackUrl, False)
    End Sub
    Private Sub btnVolver_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVolver.Load
        ''-------------------------------------------------------------------------------------
        ''La 1ª vez que se carga la pagina identificamos cual es la pagina origen y guardamos en el ViewState.
        ''El resto de veces asignamos el valor almacenado en el ViewState al control.
        ''-------------------------------------------------------------------------------------
        'If PostBackUrl Is Nothing Then PostBackUrl = Request.UrlReferrer.AbsolutePath
        'If Master.Propiedades_gvGertakariak.IdSeleccionado Is Nothing _
        ' And (PostBackUrl = "/Incidencia/Mantenimiento/DatosGenerales.aspx" Or PostBackUrl = "/Incidencia/Detalle.aspx") Then
        '	PostBackUrl = btnVolver.PostBackUrl
        'End If
        'btnVolver.PostBackUrl = PostBackUrl
        ''-------------------------------------------------------------------------------------

        '-----------------------------------------------------------------------------------------------------
        'La 1ª vez que se carga la pagina identificamos cual es la pagina origen y guardamos en el ViewState.
        'Si es una incidencia nueva vuelve a la pagina del listado.
        '-----------------------------------------------------------------------------------------------------
#If DEBUG Then
        If PostBackUrl Is Nothing Then PostBackUrl = "/Incidencia/Detalle.aspx"
#Else
		If PostBackUrl Is Nothing Then PostBackUrl = Request.UrlReferrer.AbsolutePath
#End If
        If Master.Propiedades_gvGertakariak.IdSeleccionado Is Nothing _
            And (PostBackUrl = "/Incidencia/Mantenimiento/DatosGenerales.aspx" Or PostBackUrl = "/Incidencia/Detalle.aspx") Then
            PostBackUrl = "~/Default.aspx" 'Pagina del listado
        End If
        '-----------------------------------------------------------------------------------------------------
    End Sub
    Private Sub tvEstructura_Load(sender As Object, e As System.EventArgs) Handles tvEstructura.Load
        '-------------------------------------------------------------------------
        'Cargar solo la estructura correspondiente al "Tipo de Incidencia".
        '-------------------------------------------------------------------------
        Dim IdEstructura As Integer = 4 'Identificador de la estructura que se quiere cargar. (4-Tipo de Incidencia)
        If Not IsPostBack Then CargarNodos(New gtkEstructura() With {.idTipoIncidencia = TipoIncidencia.gtkMantenimientoSist}.Listado.Where(Function(o) o.Id = IdEstructura).ToList, Nothing)
        '-------------------------------------------------------------------------
    End Sub
    Private Sub blTipoIncidencia_PreRender(sender As Object, e As System.EventArgs) Handles blTipoIncidencia.PreRender
        Dim Lista As WebControls.BulletedList = sender
        If Lista IsNot Nothing AndAlso Lista.Items.Count > 0 Then
            fsTipoIncidencia.Visible = False 'Ocultamos el boton que abre el selector de "Tipos de Incidencia".
            mpe_TipoIncidencia.TargetControlID = blTipoIncidencia.ID ' El elemento de la lista abre el selector de "Tipos de Incidencia".  
        End If
    End Sub
#End Region
#Region "Acciones"
    Private Sub btnAceptar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAceptar.Click
        blResponsables.Items.Clear()
        If SeleccionUsuarios.ListaResponsablesElegidos.Items.Count > 0 Then
            For Each Elegido As ListItem In SeleccionUsuarios.ListaResponsablesElegidos.Items
                blResponsables.Items.Add(Elegido)
            Next
        End If
    End Sub
    Private Sub btnGuardar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnGuardar.Click
        Try
            GuardarDatos()
            Response.Redirect("~/Incidencia/Detalle.aspx", False)
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Private Sub ddlFamilia_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFamilia.SelectedIndexChanged
        CargarInstalacion()
    End Sub
    Private Sub tvEstructura_SelectedNodeChanged(sender As Object, e As System.EventArgs) Handles tvEstructura.SelectedNodeChanged
        Dim tv As TreeView = sender
        blTipoIncidencia.Items.Clear()
        blTipoIncidencia.Items.Add(New ListItem With {.Text = tv.SelectedNode.Text, .Value = tv.SelectedNode.Value, .Selected = True})
    End Sub
#End Region
#Region "Funciones y Procesos"
    Sub CargarDatos()
        If Not IsPostBack Then
            '----------------------------------------------------------------
            'Cargamos los datos de la Incidencia.
            '----------------------------------------------------------------
            Gertakaria.Cargar(Master.Propiedades_gvGertakariak.IdSeleccionado)
            '----------------------------------------------------------------

            If Gertakaria.Id Is Nothing Then
                Throw New ApplicationException("noExisteNingunRegistro", New ApplicationException)
            Else
                '-------------------------------------------------------
                'Pintamos los datos del Objeto.
                '-------------------------------------------------------
                txtFechaApertura.Text = If(IsDate(Gertakaria.FechaApertura), Gertakaria.FechaApertura.Value.ToShortDateString, New Nullable(Of Date)) 'Gertakaria.FechaApertura.Value.ToShortDateString
                txtFechaCierre.Text = If(IsDate(Gertakaria.FechaCierre), Gertakaria.FechaCierre.Value.ToShortDateString, New Nullable(Of Date)) 'Gertakaria.FechaCierre.Value.ToShortDateString
                txtDescripcionProblema.Text = Gertakaria.DescripcionProblema

                '----------------------------------------------------------------------------------------------------
                'Obtenemos los Responsables de la NC.
                '----------------------------------------------------------------------------------------------------
                Dim lResponsables As List(Of SabLib.ELL.Usuario) = Gertakaria.Responsables
                If lResponsables IsNot Nothing Then
                    For Each item As SabLib.ELL.Usuario In lResponsables
                        Dim Usuario As New ListItem With {.Text = item.NombreCompleto, .Value = item.Id}
                        blResponsables.Items.Add(Usuario)
                        SeleccionUsuarios.ListaResponsablesElegidos.Items.Add(Usuario)
                    Next
                End If
                '----------------------------------------------------------------------------------------------------

                '----------------------------------------------------------------------------------------------------
                'Obtenemos la caracteristica "Tipo de Incidencia".
                '----------------------------------------------------------------------------------------------------
                If Gertakaria.TipoIncidencia IsNot Nothing Then
					Dim Estructura As New gtkEstructura : Estructura.Cargar(Gertakaria.TipoIncidencia)
					If Estructura IsNot Nothing Then blTipoIncidencia.Items.Add(New ListItem With {.Text = Estructura.Descripcion, .Value = Estructura.Id, .Selected = True})
				End If
				'----------------------------------------------------------------------------------------------------
			End If
		End If
	End Sub
	Sub GuardarDatos()
		Dim Trans As New GertakariakLib2.Transaccion
		Try
			Dim Ticket As SabLib.ELL.Ticket = Session("Ticket")
			Dim Acta As New gtkActas_MS

			Trans.Abrir()

			If Master.Propiedades_gvGertakariak.IdSeleccionado IsNot Nothing Then Gertakaria.Cargar(Master.Propiedades_gvGertakariak.IdSeleccionado)
			Gertakaria.FechaApertura = If(IsDate(txtFechaApertura.Text), CDate(txtFechaApertura.Text), New Nullable(Of Date))
			Gertakaria.FechaCierre = If(IsDate(txtFechaCierre.Text), CDate(txtFechaCierre.Text), New Nullable(Of Date))
			Gertakaria.DescripcionProblema = txtDescripcionProblema.Text
			Gertakaria.Instalacion = ddlInstalacion.SelectedValue
			'-------------------------------------------------------------------
			'Guardamos el identificador del creador solo cuando se crea la NC.
			'-------------------------------------------------------------------
			If Gertakaria.Id Is Nothing Or Gertakaria.Listado() Is Nothing Then
				Gertakaria.IdCreador = Ticket.IdUser
			End If
			'-------------------------------------------------------------------

			'-------------------------------------------------------------------
			'Registro de Actas
			'-------------------------------------------------------------------
			Acta.Nuevo = If(Gertakaria.Id Is Nothing, True, False) 'Indicamos en el acta si es un registro nuevo o una modificacion
			'-------------------------------------------------------------------

			Gertakaria.Guardar()

			'-------------------------------------------------------------------
			'Responsables
			'-------------------------------------------------------------------
			Dim ListadoResponsablesBBDD As List(Of gtkResponsable) = New gtkResponsable() With {.IdIncidencia = Gertakaria.Id}.Listado
			'Comprobamos que los Usuarios de la BB.DD no existen en el Objeto para eliminarlos de la BB.DD.
			If ListadoResponsablesBBDD IsNot Nothing AndAlso ListadoResponsablesBBDD.Any Then
				For Each Resp As gtkResponsable In ListadoResponsablesBBDD
					Dim Item As ListItem = blResponsables.Items.FindByValue(Resp.IdUsuario.ToString)
					If Item Is Nothing Then Resp.Eliminar()
				Next
			End If
			'Comprobamos que los Usuarios del Objeto NO existen en la BB.DD. para insertarlos.
			If blResponsables.Items IsNot Nothing AndAlso blResponsables.Items.Count > 0 Then
				For Each item As ListItem In blResponsables.Items
					Dim Elegido As ListItem = item
					If ListadoResponsablesBBDD Is Nothing OrElse Not ListadoResponsablesBBDD.Exists(Function(o As gtkResponsable) o.IdUsuario.ToString = Elegido.Value) Then
						Dim Responsable As New gtkResponsable With {.IdIncidencia = Gertakaria.Id, .IdUsuario = Elegido.Value}
						Responsable.Guardar()
					End If
				Next
			End If
			'-------------------------------------------------------------------

			'-------------------------------------------------------------------
			'Tipo Incidencia
			'-------------------------------------------------------------------
			Dim TipoIncidencia As New gtkCaracteristica
			If Gertakaria.Id IsNot Nothing And Gertakaria.TipoIncidencia IsNot Nothing Then TipoIncidencia.Cargar(Gertakaria.Id, Gertakaria.TipoIncidencia)
			If TipoIncidencia.IdIncidencia IsNot Nothing Then TipoIncidencia.Eliminar()
			If blTipoIncidencia IsNot Nothing AndAlso blTipoIncidencia.Items.Count > 0 Then
				TipoIncidencia.IdEstructura = blTipoIncidencia.SelectedValue
				TipoIncidencia.IdIncidencia = Gertakaria.Id
				TipoIncidencia.Guardar()
			End If
			'-------------------------------------------------------------------

			'-------------------------------------------------------------------
			'Validacion del Objeto
			'-------------------------------------------------------------------
			'No permitir cerrar la Incidencia si tiene acciones abiertas.
			If Gertakaria.FechaCierre IsNot Nothing And (Gertakaria.Acciones IsNot Nothing AndAlso Gertakaria.Acciones.Any) Then
				For Each Accion As gtkAcciones In Gertakaria.Acciones
					If Accion.FechaFin Is Nothing Then
						Throw New ApplicationException("AccionesSinCerrar", New ApplicationException)
					Else
						If Gertakaria.FechaCierre < Accion.FechaFin Then
							Dim Mensaje As String = "incidencia".Itzuli & "(" & "Fecha cierre".Itzuli & ") < " & "accion".Itzuli & "(" & "FechaFin".Itzuli & ")"
							Throw New ApplicationException(Mensaje, New ApplicationException)
						End If
					End If
				Next
			End If
			'-------------------------------------------------------------------

			'-------------------------------------------------------------------
			'Registro de Actas
			'-------------------------------------------------------------------
			Acta.Fecha = Now
			Acta.IdIncidencia = Gertakaria.Id
			Acta.IdAccion = Nothing
			Acta.IdObservacion = Nothing
			Acta.IdUsuario = Ticket.IdUser
			Acta.Guardar()
			'-------------------------------------------------------------------


			Trans.Cerrar()
			Master.Propiedades_gvGertakariak.IdSeleccionado = Gertakaria.Id
			gvGertakariak_Propiedades.IdSeleccionado = Gertakaria.Id
		Catch ex As Exception
			Trans.Rollback()
			Throw
		End Try
		'-------------------------------------------------------------------
	End Sub
	''' <summary>
	''' Mostramos las familias que tienen alguna "Instalacion".
	''' </summary>
	''' <returns></returns>
	''' <remarks></remarks>
	Function lFamilia() As List(Of PrismaLib.Asset_BLL)
		lFamilia = New PrismaLib.Asset_BLL() With {.Company = PrismaLib.Entidades.Asset_ELL.Planta.IGORRE_S, .CompanyLevel = 3}.Listado
		Dim lFamiliaSF As List(Of PrismaLib.Asset_BLL) = New PrismaLib.Asset_BLL() With {.Company = PrismaLib.Entidades.Asset_ELL.Planta.IGORRE_S, .CompanyLevel = 3}.Listado
		For Each f As PrismaLib.Asset_BLL In lFamiliaSF
			Dim Asset As String = f.Asset
			If New PrismaLib.Asset_BLL() With {.Company = PrismaLib.Entidades.Asset_ELL.Planta.IGORRE_S, .CompanyLevel = 4, .ParentAsset = f.Asset}.Listado Is Nothing Then
				lFamilia.Remove(lFamilia.Find(Function(o) o.Asset = Asset))
			End If
		Next
		Return lFamilia
	End Function
	Sub CargarInstalacion()
		ddlInstalacion.Items.Clear()
		Dim ddl_Listado As List(Of PrismaLib.Asset_BLL) = lstInstalacion.Where(Function(o) o.ParentAsset = ddlFamilia.SelectedValue).ToList
		ddl_Listado.Add(New PrismaLib.Asset_BLL)
		ddlInstalacion.DataSource = ddl_Listado.OrderBy(Function(o) o.AssetName)
		ddlInstalacion.DataBind()
		'-----------------------------------------------------------------------------
		'Seleccionamos el elemento si existe
		'-----------------------------------------------------------------------------
		Dim li As ListItem = ddlInstalacion.Items.FindByValue(Gertakaria.Instalacion)
		ddlInstalacion.SelectedValue = If(li Is Nothing, Nothing, li.Value)
		'-----------------------------------------------------------------------------
	End Sub
	''' <summary>
	''' Proceso para cargar los Nodos de "Tipo de Incidencia".
	''' </summary>
	''' <param name="lEstructura"></param>
	''' <param name="TreeNodo"></param>
	''' <remarks></remarks>
	Sub CargarNodos(lEstructura As List(Of gtkEstructura), TreeNodo As TreeNode)
		If lEstructura IsNot Nothing Then
			lEstructura = lEstructura.OrderBy(Function(o) o.Descripcion).ToList	'Ordenamos la lista
			For Each Estructura As gtkEstructura In lEstructura
				Dim Nodo As New TreeNode With {.Value = Estructura.Id, .Text = Estructura.Descripcion}
				Nodo.Selected = (blTipoIncidencia.Items IsNot Nothing AndAlso _
								 blTipoIncidencia.Items.Count > 0 AndAlso _
								 blTipoIncidencia.SelectedValue = Nodo.Value) 'Marcamos el nodo seleccionado.
				If TreeNodo Is Nothing Then
					tvEstructura.Nodes.Add(Nodo)
					Nodo.SelectAction = TreeNodeSelectAction.None
				Else
					TreeNodo.ChildNodes.Add(Nodo)
				End If
				If Estructura.Nodos IsNot Nothing Then
					Nodo.SelectAction = TreeNodeSelectAction.None
					CargarNodos(Estructura.Nodos, Nodo)
				End If
			Next
		End If
	End Sub
#End Region
End Class