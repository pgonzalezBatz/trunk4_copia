Partial Public Class _Default1
    Inherits PageBase
#Region "Propiedades"
    ''' <summary>
    ''' Propiedades para aplicar en la busqueda.
    ''' </summary>
    ''' <remarks></remarks>
    Dim FiltroGTK As New gtkFiltro
    Dim BBDD As New BatzBBDD.Entities_Gertakariak
    Public ListaGTK As List(Of BatzBBDD.GERTAKARIAK)

    ''' <summary>
    ''' Caracteristicas del Filtro.
    ''' </summary>
    ''' <remarks></remarks>
    Dim Estructuras As New List(Of gtkEstructura)

    'Familias que hay en las incidencias.
    'Dim lstFamilias As New List(Of PrismaLib.Asset_BLL)
    Dim lstFamilias As IQueryable(Of BatzBBDD.W_ASSET)
    'Instalaciones que hay en las incidencias.
    'Dim lstInstalaciones As New List(Of PrismaLib.Asset_BLL)
    Dim lstInstalaciones As IQueryable(Of BatzBBDD.W_ASSET)

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

#Region "Eventos Pagina"
    Private Sub _Default1_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        '--------------------------------------------------------------------------
        'Objetos que no traducimos despues de cargar la pagina por primera vez.
        '--------------------------------------------------------------------------
#If DEBUG Then
        'ItzultzaileWeb.ObjetosNoTraducibles.Add("cp") 'Evitamos que se traduzca automaticamente el contenido de la pagina.
#Else
		'If IsPostBack Then ItzultzaileWeb.ObjetosNoTraducibles.Add("cp") 'Evitamos que se traduzca automaticamente el contenido de la pagina.
#End If

        ItzultzaileWeb.ObjetosNoTraducibles.Add("dlFamilias") 'Selector de Familias
        ItzultzaileWeb.ObjetosNoTraducibles.Add("cblResponsables") 'Selector de Usuarios
        '--------------------------------------------------------------------------

        If Session("FiltroGTK") Is Nothing Then
            '-----------------------------------------------------------
            'Valores por defecto cuando se carga por 1ª vez el Filtro.
            'Comprobamos el perfil del usuario para aplicarle un "Filtro" inicial.
            '-----------------------------------------------------------
#If DEBUG Then
            'Dim lW_GCPROVEE = From reg In BBDD.W_GCPROVEE Select reg Take 10
            'If lW_GCPROVEE.Any Then
            '    Dim i As Integer
            '    i = lW_GCPROVEE.Count
            'End If

            'Dim lW_ASSET = From reg In BBDD.W_ASSET Select reg Take 10
            'If lW_ASSET.Any Then
            '    Dim i As Integer
            '    i = lW_ASSET.Count
            'End If

            'If Not IsPostBack Then
            'FiltroGTK.Descripcion = "linea"
            'FiltroGTK.Descripcion = "9414"
            '				'FiltroGTK.FechaPrevistaFin = Date.Today	'Con este parametro obtenemos las incidencias con "Fecha de Revisión" hoy.
            '				'ItzultzaileWeb.ObjetosNoTraducibles
            'Try
            '	Dim gtkMS As New gtkMS
            '	Dim ID As Integer = gtkMS.CrearIncidencia(179, "DescProblema - " & Now, 410450, 222, "DescAccion - " & Now)
            '	Master.MensajeInfo = ID
            'Catch ex As Exception
            '	log.Error(ex)
            '	Master.MensajeError = ex.Message
            'End Try

            'Response.Redirect("~/Incidencia/Detalle.aspx?IdIncidencia=16770", False)
            'Response.Redirect("~/Incidencia/Detalle.aspx", False)
            'End If
            'FiltroGTK.Estado = gtkFiltro.EstadoIncidencia.Abierta
            'Master.PerfilUsuario = MPWeb.Perfil.Consultor
            'If (Master.PerfilUsuario = MPWeb.Perfil.Administrador Or Master.PerfilUsuario = MPWeb.Perfil.Usuario) Then
            '	FiltroGTK.FechaPrevista = Date.Today : FiltroGTK.FechaPrevistaFin = Date.Today	'Con este parametros obtenemos las incidencias con "Fecha Prevista de Cierre" hoy.
            'Else
            '	'Filtrar solo por las que afecten al usuario y esten abiertas.
            '	Dim Ticket As SabLib.ELL.Ticket = Session("Ticket")
            '	Dim lResponsables As New List(Of Integer)
            '	'lResponsables.Add(New SabLib.BLL.UsuariosComponent().GetUsuario(New SabLib.ELL.Usuario With {.Id = Ticket.IdUser}, False).Id)
            '	lResponsables.Add(Ticket.IdUser)

            '	If lResponsables IsNot Nothing Then FiltroGTK.Responsables = lResponsables
            'End If
#Else
			FiltroGTK.Estado = gtkFiltro.EstadoIncidencia.Abierta
			If (Master.PerfilUsuario = MPWeb.Perfil.Administrador Or Master.PerfilUsuario = MPWeb.Perfil.Usuario) Then
				FiltroGTK.FechaPrevista = Date.Today : FiltroGTK.FechaPrevistaFin = Date.Today	'Con este parametros obtenemos las incidencias con "Fecha Prevista de Cierre" hoy.
			Else
				'Filtrar solo por las que afecte al usuario y esten abiertas.
				Dim Ticket As SabLib.ELL.Ticket = Session("Ticket")
				Dim lResponsables As New List(Of Integer)

				lResponsables.Add(New SabLib.BLL.UsuariosComponent().GetUsuario(New SabLib.ELL.Usuario With {.Id = Ticket.IdUser}, False).Id)

				If lResponsables IsNot Nothing Then FiltroGTK.Responsables = lResponsables
			End If
#End If
            Session("FiltroGTK") = FiltroGTK    'Actualizamos el filtro para que "AccionesPendiente" pueda usarlo.
            '-----------------------------------------------------------
        Else
            FiltroGTK = Session("FiltroGTK")
        End If
        '-------------------------------------------------------
    End Sub

    Private Sub _Default1_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Try
            CargarGridView()
            imgEliminarFiltro.Visible = (Not String.IsNullOrWhiteSpace(FiltroGTK.Descripcion) _
                                             Or (FiltroGTK.Estado IsNot Nothing) _
                                             Or (FiltroGTK.FechaPrevista IsNot Nothing) _
                                             Or (FiltroGTK.FechaPrevistaFin IsNot Nothing) _
                                             Or (FiltroGTK.Responsables IsNot Nothing AndAlso FiltroGTK.Responsables.Any) _
                                             Or (FiltroGTK.Caracteristicas IsNot Nothing AndAlso FiltroGTK.Caracteristicas.Any) _
                                             Or (FiltroGTK.Activos IsNot Nothing AndAlso FiltroGTK.Activos.Any)
                                             )
            PintarFiltro()
        Catch ex As Exception
            log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Private Sub _Default1_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        'Session("FiltroGTK") = FiltroGTK
        BBDD.Connection.Close()
        BBDD.Dispose()
    End Sub
    Private Sub rblEstados_Init(sender As Object, e As System.EventArgs) Handles rblEstados.Init
        Dim NombreEstado As String
        rblEstados.Items.Add(New ListItem("todas", String.Empty))
        For Each Estado As gtkFiltro.EstadoIncidencia In [Enum].GetValues(GetType(gtkFiltro.EstadoIncidencia))
            NombreEstado = [Enum].GetName(GetType(gtkFiltro.EstadoIncidencia), Estado)
            rblEstados.Items.Add(New ListItem(NombreEstado, Estado))
        Next
    End Sub
#End Region
#Region "Eventos de Objetos"
#Region "Eventos del Listado (gvGertakariak)"
    Private Sub gvGertakariak_Load(sender As Object, e As System.EventArgs) Handles gvGertakariak.Load
        Dim Tabla As GridView = sender
        Tabla.ShowFooter = False
    End Sub
    Private Sub gvGertakariak_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvGertakariak.PreRender
        Dim Tabla As GridView = sender
        Tabla.Ordenar(If(String.IsNullOrWhiteSpace(Master.Propiedades_gvGertakariak.CampoOrdenacion), Tabla.DataKeyNames(0).ToString, Master.Propiedades_gvGertakariak.CampoOrdenacion), If(Master.Propiedades_gvGertakariak.DireccionOrdenacion Is Nothing, SortDirection.Descending, Master.Propiedades_gvGertakariak.DireccionOrdenacion.GetValueOrDefault))
        '--------------------------------------------------------------------------------------------------------
        'Al regresar al Listado calculamos la pagina que hay que mostrar respecto al Registro seleccionado.
        '--------------------------------------------------------------------------------------------------------
        If Tabla.DataSource IsNot Nothing AndAlso Tabla.DataSource.count > 0 AndAlso Master.Propiedades_gvGertakariak.IdSeleccionado IsNot Nothing _
            AndAlso Page.Request.UrlReferrer IsNot Nothing AndAlso String.Compare(Page.Request.UrlReferrer.Segments.LastOrDefault.Replace("/", ""), Page.Request.ApplicationPath.Replace("/", ""), True) <> 0 _
            AndAlso String.Compare(Page.Request.UrlReferrer.Segments.LastOrDefault, Page.Request.Url.Segments.LastOrDefault, True) <> 0 Then
            Dim Lista As List(Of Object) = Tabla.DataSource
            Dim TipoObjeto As Type = Lista.First.GetType
            Dim Propiedades As List(Of Reflection.PropertyInfo) = TipoObjeto.GetProperties.ToList
            Dim Propiedad As System.Reflection.PropertyInfo = Propiedades.Find(Function(pi As System.Reflection.PropertyInfo) String.Compare(pi.Name, "Id", True) = 0)
            Dim PosicionReg As Integer = Lista.FindIndex(Function(o) o.GetType.GetProperty(Propiedad.Name).GetValue(o, Nothing) = Master.Propiedades_gvGertakariak.IdSeleccionado)
            If PosicionReg >= 0 Then
                Dim PaginaActual As Integer = Math.Floor(If(PosicionReg < 0, 0, PosicionReg) / Tabla.PageSize)
                Master.Propiedades_gvGertakariak.Pagina = PaginaActual
            End If
        End If
        '--------------------------------------------------------------------------------------------------------
        Tabla.PageIndex = If(Master.Propiedades_gvGertakariak.Pagina, 0)
        Tabla.DataBind()
        'If IsPostBack Then ItzultzaileWeb.Itzuli(gvGertakariak) : ItzultzaileWeb.TraducirWebControls(gvGertakariak.Controls)

        lblNumRegistros.Text = If(ListaGTK Is Nothing, Nothing, ListaGTK.Count)  'Pintamos el numero de registros que se carga en el GridView.
    End Sub
    Private Sub gvGertakariak_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvGertakariak.RowCreated
        Dim Tabla As GridView = sender
        Dim Fila As GridViewRow = e.Row
        If Fila.DataItem IsNot Nothing Then
            Dim Incidencia As BatzBBDD.GERTAKARIAK = Fila.DataItem
            Dim lblInstalacion As WebControls.Label = Fila.FindControl("lblInstalacion")

            'Indicamos se es el registro seleccionado.
            If Incidencia.ID = Master.Propiedades_gvGertakariak.IdSeleccionado Then
                Fila.RowState = DataControlRowState.Selected
                If Incidencia.ACCIONES.Any Then
                    Dim cpeAcciones As CollapsiblePanelExtender = Fila.FindControl("cpeAcciones")
                    cpeAcciones.Collapsed = False
                End If
            End If

            '-----------------------------------------------------------------------------------------
            'Seleccion del registro seleccionando la fila.
            'Ponemos a cada celda el evento de carga del detalle excepto a la que abre el detalle.
            '-----------------------------------------------------------------------------------------
            For i As Integer = 1 To Fila.Cells.Count - 1
                Fila.Cells(i).Attributes.Add("onmouseover", "this.style.cursor=""pointer""")
                Fila.Cells(i).Attributes("OnClick") = Page.ClientScript.GetPostBackEventReference(sender, "Select$" + Fila.RowIndex.ToString) 'Funciona si EnableEventValidation="false"
            Next
            '-----------------------------------------------------------------------------------------

            '-----------------------------------------------------------------------------------------
            'Mostramos los responsables de la incidencia.
            '-----------------------------------------------------------------------------------------
            If Incidencia.RESPONSABLES_GERTAKARIAK.FirstOrDefault IsNot Nothing Then
                Dim Lista As System.Web.UI.WebControls.BulletedList = Fila.FindControl("blResponsables")
                Lista.DataSource = Incidencia.RESPONSABLES_GERTAKARIAK.Select(Function(Usr) New With {.NombreCompleto = Usr.SAB_USUARIOS.NOMBRE & " " & Usr.SAB_USUARIOS.APELLIDO1 & " " & Usr.SAB_USUARIOS.APELLIDO2}.NombreCompleto)
            End If
            '-----------------------------------------------------------------------------------------
            '-----------------------------------------------------------------------------------------
            If lblInstalacion IsNot Nothing And (Not String.IsNullOrWhiteSpace(Incidencia.IDACTIVO)) Then
                lblInstalacion.Text = (From Activo As BatzBBDD.W_ASSET In BBDD.W_ASSET Where Activo.COMPANY = "IGORRE_S" And Activo.ASSET = Incidencia.IDACTIVO Select Activo.ASSETNAME).FirstOrDefault
            End If
            '-----------------------------------------------------------------------------------------

            '-----------------------------------------------------------------------------------------
            'Desarrollo / Avance de la incidencia en base a la media ponderada. "Avance Incidencia"="Suma Avance Acciones"/"Nº Acciones".
            '-----------------------------------------------------------------------------------------
            Dim pnlDes_Cont As WebControls.Panel = Fila.FindControl("pnlDes_Cont")
            Dim pnlDes_Bar As WebControls.Panel = Fila.FindControl("pnlDes_Bar")
            If pnlDes_Bar IsNot Nothing Then
                If Incidencia.ACCIONES.FirstOrDefault Is Nothing Then
                    pnlDes_Bar.Width = Unit.Percentage(If(Incidencia.FECHACIERRE Is Nothing, "0", "100"))
                Else
                    pnlDes_Bar.Width = Unit.Percentage(Incidencia.ACCIONES.Sum(Function(o) o.REALIZACION) / Incidencia.ACCIONES.Count)
                End If
                If pnlDes_Cont IsNot Nothing Then pnlDes_Cont.ToolTip = Unit.Percentage(pnlDes_Bar.Width.Value).ToString
            End If
            '-----------------------------------------------------------------------------------------
        End If
    End Sub
    Private Sub gvGertakariak_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvGertakariak.RowDataBound
        Dim Fila As GridViewRow = e.Row
        If Fila.RowType = DataControlRowType.DataRow Then
            'Dim Incidencia As gtkMatenimientoSist = If(e.Row.DataItem, Nothing)
            Dim Incidencia As BatzBBDD.GERTAKARIAK = If(e.Row.DataItem, Nothing)
            ''--------------------------------------------------------------------
            ''Indicamos el estado de Incidencia.
            ''- Abierta: RowStyleAdvertencia, AlternatingRowStyleAdvertencia
            ''--------------------------------------------------------------------
            'If Incidencia.FECHACIERRE Is Nothing Then
            '	If Fila.RowState = DataControlRowState.Normal Then
            '		Fila.CssClass = "RowStyleAdvertencia"
            '	ElseIf Fila.RowState = DataControlRowState.Alternate Then
            '		Fila.CssClass = "AlternatingRowStyleAdvertencia"
            '	End If
            'End If
            ''--------------------------------------------------------------------

            '--------------------------------------------------------------------
            'Indicamos el estado de Incidencia.
            '--------------------------------------------------------------------
            Dim imgEstado_GV As WebControls.Image = Fila.FindControl("imgEstado")
            Dim pnlEstado_GV As WebControls.Panel = Fila.FindControl("pnlEstado")
            imgEstado_GV.Visible = Not (Incidencia.FECHACIERRE Is Nothing)
            pnlEstado_GV.Visible = Not (imgEstado_GV.Visible)
            '--------------------------------------------------------------------

            '--------------------------------------------------------------------
            Dim gvAcciones As New GridView
            gvAcciones = Fila.FindControl("gvAcciones")
            gvAcciones.DataSource = Incidencia.ACCIONES
            gvAcciones.DataBind()
            '--------------------------------------------------------------------
        End If
    End Sub
    'Private Sub Page_PreRenderComplete1(sender As Object, e As EventArgs) Handles Me.PreRenderComplete
    'If IsPostBack Then
    '	'If gvGertakariak.HeaderRow IsNot Nothing Then ItzultzaileWeb.TraducirWebControls(gvGertakariak.HeaderRow.Controls)
    '	If gvGertakariak.FooterRow IsNot Nothing Then
    '		'ItzultzaileWeb.TraducirWebControls(gvGertakariak.FooterRow.Controls)
    '	End If
    'End If
    'End Sub

    Private Sub gvGertakariak_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvGertakariak.SelectedIndexChanged
        Master.Propiedades_gvGertakariak.IdSeleccionado = gvGertakariak.SelectedDataKey.Value
        gvGertakariak_Propiedades.IdSeleccionado = If(String.IsNullOrWhiteSpace(gvGertakariak.SelectedDataKey.Value), New Nullable(Of Integer), CInt(gvGertakariak.SelectedDataKey.Value))
        Response.Redirect("~/Incidencia/Detalle.aspx", False)
    End Sub
    Private Sub gvGertakariak_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvGertakariak.PageIndexChanging
        Dim Tabla As GridView = sender
        Tabla.PageIndex = e.NewPageIndex
        Master.Propiedades_gvGertakariak.Pagina = Tabla.PageIndex
    End Sub
    Private Sub gvGertakariak_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvGertakariak.Sorting
        Dim Tabla As GridView = sender
        '-------------------------------------------------------------------------------------------------------------
        'Criterio de Ordenacion:
        'Ascendente si no hay criterio o es Descendente el criterio que existe o el campo de ordenacion es distinto.
        '-------------------------------------------------------------------------------------------------------------
        If IsPostBack Then
            If Master.Propiedades_gvGertakariak.DireccionOrdenacion IsNot Nothing _
             AndAlso Master.Propiedades_gvGertakariak.DireccionOrdenacion.Value = SortDirection.Ascending _
             And Master.Propiedades_gvGertakariak.CampoOrdenacion = e.SortExpression Then
                Master.Propiedades_gvGertakariak.DireccionOrdenacion = SortDirection.Descending
            ElseIf Master.Propiedades_gvGertakariak.DireccionOrdenacion.GetValueOrDefault = SortDirection.Descending _
             Or Master.Propiedades_gvGertakariak.DireccionOrdenacion Is Nothing _
             Or Master.Propiedades_gvGertakariak.CampoOrdenacion <> e.SortExpression Then
                Master.Propiedades_gvGertakariak.DireccionOrdenacion = SortDirection.Ascending
            End If
        End If
        '-------------------------------------------------------------------------------------------------------------
        Master.Propiedades_gvGertakariak.CampoOrdenacion = e.SortExpression
    End Sub
    Private Sub gvGertakariak_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gvGertakariak.RowEditing
        Try
            Dim Tabla As GridView = sender
            e.Cancel = True
            gvGertakariak_Propiedades.IdSeleccionado = CType(Tabla.DataKeys(e.NewEditIndex).Value, Integer)

            Dim Reg As BatzBBDD.GERTAKARIAK = (From gtk As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK Where gtk.ID = gvGertakariak_Propiedades.IdSeleccionado Select gtk).SingleOrDefault
            Reg.FECHACIERRE = Now.Date
            BBDD.SaveChanges()

            Response.Redirect("~/Default.aspx", False)
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub

    '''<summary>
    '''Control gvGertakariak.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvAcciones As Global.System.Web.UI.WebControls.GridView
    ''' <summary>
    ''' Proceso para la generacion de las lineas de la tabla secundaria (Acciones).
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Sub gvAcciones_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gvAcciones.RowCreated
        Dim Fila As GridViewRow = e.Row
        Dim chkAccion As New CheckBox
        Dim lblObservacion As New Label
        If Fila.RowType = DataControlRowType.DataRow Then
            chkAccion = Fila.FindControl("chkAccion")
            lblObservacion = Fila.FindControl("lblObservacion")
            If chkAccion IsNot Nothing Then
                Dim gvPrincipal As GridView = sender.Parent.Parent.Parent.Parent.Parent 'GridView Principal donde pintamos por cada fila un "gvAcciones".
                Dim FilaGvPrincipal As GridViewRow = sender.Parent.Parent.Parent 'Fila del GridView principal donde pintamos el "gvAcciones".
                If (Master.PerfilUsuario = MPWeb.Perfil.Administrador Or Master.PerfilUsuario = MPWeb.Perfil.Usuario) And chkAccion IsNot Nothing Then
                    chkAccion.Visible = True
                ElseIf Master.PerfilUsuario = MPWeb.Perfil.Consultor Then
                    If gvGertakariak.DataSource IsNot Nothing Then
                        If FilaGvPrincipal.DataItem IsNot Nothing Then
                            Dim Ticket As SabLib.ELL.Ticket = Session("Ticket")
                            '==========================================================================================================================
                            'Dim Incidencia As gtkMatenimientoSist = FilaGvPrincipal.DataItem
                            ''--------------------------------------------------------------------------------------------------------
                            ''1-Comprobamos si es Responsable de la "Incidencia" para modificar la "Fecha Prevista de Cierre".
                            ''--------------------------------------------------------------------------------------------------------
                            'If chkAccion.Visible = False Then chkAccion.Visible = (Incidencia.Responsables IsNot Nothing AndAlso Incidencia.Responsables.Exists(Function(o) o.Id = Ticket.IdUser))
                            ''--------------------------------------------------------------------------------------------------------
                            ''--------------------------------------------------------------------------------------------------------
                            ''2-Comprobamos si es Responsable de la "Accion" para modificar la "Fecha Prevista de Cierre".
                            ''--------------------------------------------------------------------------------------------------------
                            'If Incidencia.ACCIONES IsNot Nothing Then
                            '	Dim Accion As gtkAcciones = Fila.DataItem
                            '	If chkAccion.Visible = False Then chkAccion.Visible = (Accion.Responsables IsNot Nothing AndAlso Accion.Responsables.Exists(Function(o) o.Id = Ticket.IdUser))
                            'End If
                            ''--------------------------------------------------------------------------------------------------------
                            '==========================================================================================================================
                            '--------------------------------------------------------------------------------------------------------
                            'FROGA:2013-05-27:
                            '--------------------------------------------------------------------------------------------------------
                            Dim Incidencia As BatzBBDD.GERTAKARIAK = FilaGvPrincipal.DataItem
                            '--------------------------------------------------------------------------------------------------------
                            '1-Comprobamos si es Responsable de la "Incidencia" para modificar la "Fecha Prevista de Cierre".
                            '--------------------------------------------------------------------------------------------------------
                            If chkAccion.Visible = False Then chkAccion.Visible = (Incidencia.RESPONSABLES_GERTAKARIAK.FirstOrDefault IsNot Nothing AndAlso Incidencia.RESPONSABLES_GERTAKARIAK.Any(Function(o) o.ID = Ticket.IdUser))
                            '--------------------------------------------------------------------------------------------------------
                            '--------------------------------------------------------------------------------------------------------
                            '2-Comprobamos si es Responsable de la "Accion" para modificar la "Fecha Prevista de Cierre".
                            '--------------------------------------------------------------------------------------------------------
                            If Incidencia.ACCIONES.FirstOrDefault IsNot Nothing Then
                                Dim Accion As BatzBBDD.ACCIONES = Fila.DataItem
                                If chkAccion.Visible = False Then chkAccion.Visible = (Accion.ACCIONES_USUARIOS.FirstOrDefault IsNot Nothing AndAlso Accion.ACCIONES_USUARIOS.Any(Function(o) o.IDUSUARIO = Ticket.IdUser))
                            End If
                            '--------------------------------------------------------------------------------------------------------
                            '--------------------------------------------------------------------------------------------------------
                            '==========================================================================================================================
                        End If
                    End If
                End If
                If lblObservacion IsNot Nothing And Fila.DataItem IsNot Nothing Then
                    Dim gtkAcciones As BatzBBDD.ACCIONES = Fila.DataItem

                    Dim gtkAccionesObservaciones As New gtkAccionesObservaciones
                    gtkAccionesObservaciones.IdAccion = gtkAcciones.ID
                    Dim Listado As List(Of gtkAccionesObservaciones) = gtkAccionesObservaciones.Listado()
                    'If Listado IsNot Nothing AndAlso Listado.Count > 0 Then
                    If Listado IsNot Nothing AndAlso Listado.Any Then
                        Dim gtkAcOb As New gtkAccionesObservaciones
                        gtkAcOb = (From ao As gtkAccionesObservaciones In Listado Select ao Order By ao.Fecha).LastOrDefault
                        lblObservacion.Text = gtkAcOb.Descripcion
                        lblObservacion.ToolTip = gtkAcOb.Fecha
                    End If
                    '------------------------------------------------------------------------------------------------------
                    'Demora
                    '------------------------------------------------------------------------------------------------------
                    Dim Demora As Label = Fila.FindControl("lblDemora")
                    If Demora IsNot Nothing Then
                        Demora.Text = DateDiff(DateInterval.WeekOfYear, gtkAcciones.FECHAINICIO.Value, If(IsDate(gtkAcciones.FECHAFIN), gtkAcciones.FECHAFIN.Value, Now), Microsoft.VisualBasic.FirstDayOfWeek.Monday, FirstWeekOfYear.FirstFourDays)
                    End If
                    '------------------------------------------------------------------------------------------------------
                End If
                If chkAccion.Visible = True Then gvPrincipal.ShowFooter = True 'Mostros el boton de modificacion de la "Fecha Prevista de Cierre".
            End If
            If IsPostBack Then
                Dim btnNuevaObservacion As ImageButton = Fila.FindControl("btnNuevaObservacion")
                'If btnNuevaObservacion IsNot Nothing Then ItzultzaileWeb.Itzuli(btnNuevaObservacion)
            End If
        ElseIf Fila.RowType = DataControlRowType.Header Then
            'If IsPostBack Then ItzultzaileWeb.TraducirWebControls(Fila.Controls)
        End If
    End Sub
#End Region
    Private Sub dlCaracteristicas_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dlCaracteristicas.ItemDataBound
        Dim ListItem As DataListItem = e.Item
        Dim tvCaracteristicas As TreeView = ListItem.FindControl("tvCaracteristicas")
        Dim TreeView As TreeView = ListItem.DataItem
        If TreeView.Nodes.Count > 0 Then tvCaracteristicas.Nodes.Add(TreeView.Nodes.Item(0))
        tvCaracteristicas.DataBind()
    End Sub
    Private Sub dlFamilias_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dlFamilias.ItemDataBound
        Dim ListItem As DataListItem = e.Item
        Dim tvFamilias As TreeView = ListItem.FindControl("tvFamilias")
        '-----------------------------------------------------------------------
        'Cargamos la estructura para su correspondiente Familia.
        '-----------------------------------------------------------------------
        Dim Familia As BatzBBDD.W_ASSET = ListItem.DataItem
        If Familia IsNot Nothing AndAlso tvFamilias IsNot Nothing Then
            Dim Nodo As New TreeNode With
                    {.SelectAction = TreeNodeSelectAction.Expand,
                    .Value = Familia.ASSET, .Text = Familia.ASSETNAME}
            tvFamilias.Nodes.Add(Nodo)
            Dim lstInstalaciones2 As IQueryable(Of BatzBBDD.W_ASSET) = From Ins As BatzBBDD.W_ASSET In BBDD.W_ASSET
                                                                       Join gtk As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK On gtk.IDACTIVO Equals Ins.ASSET
                                                                       Where Ins.COMPANY = "IGORRE_S" And Ins.PARENTASSET = Familia.ASSET
                                                                       Select Ins Distinct
            For Each Instalacion As BatzBBDD.W_ASSET In lstInstalaciones2
                Dim ChildNode As New TreeNode With {.SelectAction = TreeNodeSelectAction.Expand, .Value = Instalacion.ASSET, .Text = Instalacion.ASSETNAME}
                Nodo.ChildNodes.Add(ChildNode)
                If FiltroGTK.Activos IsNot Nothing AndAlso FiltroGTK.Activos.Any And FiltroGTK.Activos.Contains(ChildNode.Value) Then
                    ChildNode.Checked = True
                    If ChildNode.Parent IsNot Nothing Then ChildNode.Parent.Expanded = ChildNode.Checked
                End If
            Next
        End If
        tvFamilias.DataBind() 'Mantenemos los nodos expandidos despues de filtrar.
        '-----------------------------------------------------------------------
    End Sub
#End Region
#Region "Acciones"
    Protected Sub imgFiltrar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgFiltrar.Click
        CargarFiltro()
        Session("FiltroGTK") = FiltroGTK 'Actualizamos el filtro para que "AccionesPendiente" pueda usarlo.
    End Sub
    Private Sub imgFiltrar2_Click(sender As Object, e As ImageClickEventArgs) Handles imgFiltrar2.Click
        imgFiltrar_Click(sender, e)
    End Sub
    Private Sub imgEliminarFiltro_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgEliminarFiltro.Click
        FiltroGTK = New gtkFiltro
        Session("FiltroGTK") = FiltroGTK 'Actualizamos el filtro para que "AccionesPendiente" pueda usarlo.
    End Sub
    Public Sub btnFechaRevision_Click(sender As Object, e As System.EventArgs)
        Dim IdAcciones As New List(Of Integer)
        Dim FechaRevision As New Nullable(Of Date)
        Try
            If gvGertakariak.Controls IsNot Nothing Then
                For Each gvFila As GridViewRow In gvGertakariak.Controls.Item(0).Controls
                    If gvFila.RowType = DataControlRowType.Footer Then
                        'Obtenemos la nueva Fecha Prevista de Cierre.
                        Dim Fec As TextBox = gvFila.FindControl("txtFechaRevision")
                        FechaRevision = If(Fec.Text.Trim Is String.Empty, New Nullable(Of Date), CDate(Fec.Text.Trim))
                    ElseIf gvFila.RowType = DataControlRowType.DataRow Then
                        '--------------------------------------------------------------------------------------------------
                        'Obtenemos los identificadores de las acciones a las que se les actualizara la Fecha Prevista de Cierre.
                        '--------------------------------------------------------------------------------------------------
                        Dim gvAcciones As GridView = gvFila.FindControl("gvAcciones")
                        If gvAcciones IsNot Nothing AndAlso gvAcciones.Rows IsNot Nothing AndAlso gvAcciones.Rows.Count > 0 Then
                            For Each FilaAccion As GridViewRow In gvAcciones.Rows
                                Dim chkAccion As CheckBox = FilaAccion.FindControl("chkAccion")
                                If chkAccion.Checked Then
                                    IdAcciones.Add(gvAcciones.DataKeys(FilaAccion.DataItemIndex).Value)
                                End If
                            Next
                        End If
                        '--------------------------------------------------------------------------------------------------
                    End If
                Next

                If FechaRevision IsNot Nothing Then
                    If IdAcciones.Any Then
                        For Each IdAccion In IdAcciones
                            '------------------------------------------------------------------------------------------
                            'Dim gtkAccion As New gtkAcciones
                            'gtkAccion.Cargar(IdAccion)
                            ''----------------------------------------------------------
                            ''Validacion Accion.
                            ''----------------------------------------------------------
                            'If FechaRevision < gtkAccion.FechaInicio Or FechaRevision > gtkAccion.FechaFin Then
                            '	Dim gtkGA As New gtkGertakariak_Acciones With {.IdAccion = IdAccion}
                            '	gtkGA = gtkGA.Listado()(0)
                            '	Throw New ApplicationException(ItzultzaileWeb.Itzuli("incidencia") & ":" & gtkGA.IdIncidencia & " / " & ItzultzaileWeb.Itzuli("Fecha Prevista de Cierre") & " -> " & ItzultzaileWeb.Itzuli("fechaIncorrecta"), New ApplicationException)
                            'Else
                            '	gtkAccion.FechaPrevista = FechaRevision
                            '	gtkAccion.Guardar()
                            'End If
                            '----------------------------------------------------------
                            '------------------------------------------------------------------------------------------
                            '2014-10-13:
                            '------------------------------------------------------------------------------------------
                            Dim Accion As BatzBBDD.ACCIONES = (From Reg As BatzBBDD.ACCIONES In BBDD.ACCIONES Where Reg.ID = IdAccion Select Reg).SingleOrDefault
                            If Accion IsNot Nothing Then
                                '----------------------------------------------------------
                                'Validacion Accion.
                                '----------------------------------------------------------
                                If FechaRevision < Accion.FECHAINICIO Then
                                    Dim msg As String = "'Fecha Prevista de Cierre' MENOR que la 'Fecha de Inicio' de la accion."
                                    Throw New ApplicationException(ItzultzaileWeb.Itzuli("incidencia") & ": " & Accion.GERTAKARIAK.FirstOrDefault.ID & "<HR>" & msg)
                                    'ElseIf FechaRevision > Accion.FECHAFIN Then
                                    '    Dim msg As String = "'Fecha Prevista de Cierre' MAYOR que la 'Fecha de Fin' de la accion."
                                    '    Throw New ApplicationException(ItzultzaileWeb.Itzuli("incidencia") & ": " & Accion.GERTAKARIAK.FirstOrDefault.ID & "<HR>" & msg)
                                Else
                                    Accion.FECHAPREVISTA = FechaRevision
                                    BBDD.SaveChanges()
                                End If
                            End If
                            '------------------------------------------------------------------------------------------
                        Next
                        'No poner codigo despues de esta linea si EndResponse=False pq permite que la funcion se ejecute hasta el final.
                        Response.Redirect(Request.UrlReferrer.AbsolutePath, False)
                    End If
                End If
            End If
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    ''' <summary>
    ''' Accedemos a la pantalla para crear una nueva "Observacion" de la accion e incidencia en curso.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Sub btnNuevaObservacion_Click(sender As Object, e As System.EventArgs)
        Dim Boton As ImageButton = sender
        Dim IdAccion As Integer
        Dim gvAccion As GridView = Boton.Parent.Parent.Parent.Parent
        Dim gvrAccion As GridViewRow = Boton.Parent.Parent

        'Seleccionamos la incidencia en curso
        gvGertakariak.SelectedIndex = CType(Boton.Parent.Parent.Parent.Parent.Parent.Parent.Parent, GridViewRow).RowIndex
        Master.Propiedades_gvGertakariak.IdSeleccionado = gvGertakariak.SelectedDataKey.Value
        gvGertakariak_Propiedades.IdSeleccionado = If(String.IsNullOrWhiteSpace(gvGertakariak.SelectedDataKey.Value), New Nullable(Of Integer), CInt(gvGertakariak.SelectedDataKey.Value))

        'Identificamos la accion a la que se va ha agregar una nueve observacion
        IdAccion = gvAccion.DataKeys(gvrAccion.RowIndex)(Array.Find(gvAccion.DataKeyNames, Function(o) String.Compare(o, "ID", True) = 0))

        Response.Redirect("~/Incidencia/Mantenimiento/Observaciones.aspx?IdAccion= " & IdAccion, False)
    End Sub
#End Region
#Region "Funciones y Procesos"
    ''' <summary>
    ''' Recogemos los datos de la pagina para componer el Filtro.
    ''' </summary>
    ''' <remarks></remarks>
    Sub CargarFiltro()
        'log.Debug("CargarFiltro()")
        FiltroGTK.Descripcion = If(txtBuscar.Text.Trim Is String.Empty, Nothing, txtBuscar.Text.Trim)
        FiltroGTK.FechaPrevista = If(txtFechaRevision.Text.Trim Is String.Empty, New Nullable(Of Date), CDate(txtFechaRevision.Text.Trim))
        FiltroGTK.FechaPrevistaFin = If(txtFechaRevisionFin.Text.Trim Is String.Empty, New Nullable(Of Date), CDate(txtFechaRevisionFin.Text.Trim))
        FiltroGTK.Estado = If(rblEstados.SelectedValue Is String.Empty, New Nullable(Of gtkFiltro.EstadoIncidencia), CType(rblEstados.SelectedValue, gtkFiltro.EstadoIncidencia))

        If cblResponsables.Items.Count > 0 Then
            Dim lResponsables As New List(Of Integer)
            For Each ResCHK As ListItem In cblResponsables.Items
                If ResCHK.Selected Then lResponsables.Add(ResCHK.Value)
            Next
            FiltroGTK.Responsables = lResponsables
        End If

        '-------------------------------------------------------------------------
        'Recuperamos y guardamos en el filtro las caracteristicas seleccionadas.
        '-------------------------------------------------------------------------
        If dlCaracteristicas.Controls IsNot Nothing Then
            Dim lCarasteristicas As New List(Of Integer)
            For Each dliCaracteristica As DataListItem In dlCaracteristicas.Controls
                Dim tvCaracteristicas As TreeView = dliCaracteristica.FindControl("tvCaracteristicas")
                If tvCaracteristicas IsNot Nothing Then
                    If tvCaracteristicas.CheckedNodes IsNot Nothing Then
                        For Each Nodo As TreeNode In tvCaracteristicas.CheckedNodes
                            lCarasteristicas.Add(Nodo.Value)
                        Next
                    End If
                End If
            Next
            FiltroGTK.Caracteristicas = lCarasteristicas
        End If
        '-------------------------------------------------------------------------

        '------------------------------------------------------------------------------
        'Recuperamos y guardamos en el filtro las Instalaciones (Asset) seleccionadas.
        '------------------------------------------------------------------------------
        If dlFamilias.Controls IsNot Nothing Then
            Dim lAsset As New List(Of String)
            For Each dliAsset As DataListItem In dlFamilias.Controls
                Dim tvFamilias As TreeView = dliAsset.FindControl("tvFamilias")
                If tvFamilias IsNot Nothing Then
                    If tvFamilias.CheckedNodes IsNot Nothing Then
                        For Each Nodo As TreeNode In tvFamilias.CheckedNodes
                            lAsset.Add(Nodo.Value)
                        Next
                    End If
                End If
            Next
            FiltroGTK.Activos = lAsset
        End If
        '------------------------------------------------------------------------------
        'log.Warn("CargarFiltro()")
    End Sub

    Sub PintarFiltro()
        'log.Debug("PintarFiltro()")
        '-------------------------------------------------------------------------------------
        'Obtenemos informacion del resultado de las busquedas para pintar el filtro.
        '-------------------------------------------------------------------------------------
        Dim lInstalaciones As New List(Of String)
        Dim lResponsables As New Dictionary(Of String, String)


        'log.Debug("PintarFiltro() - 1")
        If ListaGTK IsNot Nothing AndAlso ListaGTK.Any Then
            '--------------------------------------------------------------------------------------------
            'FROGA:2013-05-22: Obtenemos la lista de Responsables de las "Incidencias" y "Acciones"
            '--------------------------------------------------------------------------------------------
            'JOIN - Se consigue SIN "DefaultIfEmpty"
            '--------------------------------------------------------------------------------------------
            Dim ListaResponsables = ((From GtkMS As BatzBBDD.GERTAKARIAK In ListaGTK
                                      From Resp As BatzBBDD.RESPONSABLES_GERTAKARIAK In GtkMS.RESPONSABLES_GERTAKARIAK
                                      Where (Resp.SAB_USUARIOS.FECHABAJA Is Nothing OrElse Resp.SAB_USUARIOS.FECHABAJA > Date.Today)
                                      Let Id = Resp.IDUSUARIO _
                    , NombreCompleto As String = Resp.SAB_USUARIOS.NOMBRE & " " & Resp.SAB_USUARIOS.APELLIDO1 & " " & Resp.SAB_USUARIOS.APELLIDO2
                                      Select Id, NombreCompleto Distinct) _
                    .Union _
                    (From GtkMS As BatzBBDD.GERTAKARIAK In ListaGTK
                     From ACCIONES As BatzBBDD.ACCIONES In GtkMS.ACCIONES
                     From ACCIONES_USUARIOS As BatzBBDD.ACCIONES_USUARIOS In ACCIONES.ACCIONES_USUARIOS
                     Where ACCIONES_USUARIOS.SAB_USUARIOS.FECHABAJA Is Nothing OrElse ACCIONES_USUARIOS.SAB_USUARIOS.FECHABAJA > Date.Today
                     Let Id = ACCIONES_USUARIOS.IDUSUARIO _
                     , NombreCompleto As String = ACCIONES_USUARIOS.SAB_USUARIOS.NOMBRE & " " & ACCIONES_USUARIOS.SAB_USUARIOS.APELLIDO1 & " " & ACCIONES_USUARIOS.SAB_USUARIOS.APELLIDO2
                     Select Id, NombreCompleto Distinct)).OrderBy(Function(o) o.NombreCompleto)

            For Each Resp In ListaResponsables
                lResponsables.Add(Resp.Id, Resp.NombreCompleto)
            Next

            '--------------------------------------------------------------------------------------------

            '----------------------------------------------------
            'Obtenemos las Instalaciones.
            '----------------------------------------------------
            For Each Incidencia As BatzBBDD.GERTAKARIAK In ListaGTK
                If Not String.IsNullOrWhiteSpace(Incidencia.IDACTIVO) _
                    AndAlso Not lInstalaciones.Exists(Function(o As String) o = Incidencia.IDACTIVO) Then
                    lInstalaciones.Add(Incidencia.IDACTIVO)
                End If
            Next
            '----------------------------------------------------
        End If
        'log.Warn("PintarFiltro() - 1")
        '-------------------------------------------------------------------------------------

        If FiltroGTK IsNot Nothing Then
            'log.Debug("PintarFiltro() - 2")
            txtBuscar.Text = FiltroGTK.Descripcion
            txtFechaRevision_CalendarExtender.SelectedDate = FiltroGTK.FechaPrevista
            txtFechaRevisionFin_CalendarExtender.SelectedDate = FiltroGTK.FechaPrevistaFin
            txtFechaRevision.Text = txtFechaRevision_CalendarExtender.SelectedDate.ToString
            txtFechaRevisionFin.Text = txtFechaRevisionFin_CalendarExtender.SelectedDate.ToString
            rblEstados.SelectedValue = If(FiltroGTK.Estado, String.Empty)
            'log.Warn("PintarFiltro() - 2")
            '-----------------------------------------------------------------------
            'Responsables de las Incidencias y Acciones.
            '-----------------------------------------------------------------------
            cblResponsables.DataValueField = "Key"
            cblResponsables.DataTextField = "Value"
            cblResponsables.DataSource = lResponsables
            cblResponsables.DataBind()

            If FiltroGTK.Responsables IsNot Nothing AndAlso FiltroGTK.Responsables.Any Then
                For Each Id As Integer In FiltroGTK.Responsables
                    Dim li As ListItem = If(cblResponsables.Items.FindByValue(Id), Nothing)
                    If li IsNot Nothing Then li.Selected = True
                Next
            End If
            '-----------------------------------------------------------------------
            'log.Warn("PintarFiltro() - 3")
            '-----------------------------------------------------------------------

            If ListaGTK IsNot Nothing AndAlso ListaGTK.Any Then
                'log.Debug("PintarFiltro() - 4.1")
                BuscarNodos()
                'log.Warn("PintarFiltro() - 4.1")
                'log.Debug("PintarFiltro() - 4.2")
                dlCaracteristicas.DataSource = lTreeView
                dlCaracteristicas.DataBind()
                'log.Warn("PintarFiltro() - 4.2")
            End If
            'log.Warn("PintarFiltro() - 4")

            '-----------------------------------------------------------------------

            '====================================================================================================================
            'Cargamos el listado de estructuras expecifico.
            '====================================================================================================================
            '-----------------------------------------------------------------------
            '1.-Seleccionamos las "Caracteristicas" para todos los arboles.
            '-----------------------------------------------------------------------
            'log.Debug("PintarFiltro() - 5")
            If FiltroGTK.Caracteristicas IsNot Nothing AndAlso FiltroGTK.Caracteristicas.Any Then
                If dlCaracteristicas.Items IsNot Nothing AndAlso dlCaracteristicas.Items.Count > 0 Then
                    For Each dlCaracteristica As DataListItem In dlCaracteristicas.Items
                        If dlCaracteristica.HasControls Then
                            '-------------------------------------------------------
                            'Buscamos los Arboles de las caracteristicas.
                            '-------------------------------------------------------
                            For Each Control As Object In dlCaracteristica.Controls
                                If String.Compare(Control.GetType.Name, "TreeView", True) = 0 Then
                                    Dim tvCaracteristicas As TreeView = Control
                                    SeleccionarNodos(tvCaracteristicas.Nodes, FiltroGTK.Caracteristicas)
                                End If
                            Next
                            '-------------------------------------------------------
                        End If
                    Next
                End If
            End If
            'log.Warn("PintarFiltro() - 5")
            '-----------------------------------------------------------------------

            '-----------------------------------------------------------------------------------------------
            '2.-Creamos y pintamos las familias para el "Filtro".
            '-----------------------------------------------------------------------------------------------
            lstInstalaciones = From Instalacion As BatzBBDD.W_ASSET In BBDD.W_ASSET
                               Where Instalacion.COMPANY = "IGORRE_S" And lInstalaciones.Contains(Instalacion.ASSET)
                               Select Instalacion Distinct Order By Instalacion.ASSETNAME Distinct
            lstFamilias = From Familia As BatzBBDD.W_ASSET In BBDD.W_ASSET
                          Join Instalacion As BatzBBDD.W_ASSET In lstInstalaciones On Familia.ASSET Equals Instalacion.PARENTASSET
                          Where Familia.COMPANY = "IGORRE_S"
                          Select Familia Order By Familia.ASSETNAME Distinct
            dlFamilias.DataSource = lstFamilias
            dlFamilias.DataBind()
            'log.Warn("PintarFiltro() - 7")
            '-----------------------------------------------------------------------------------------------
            '====================================================================================================================
        End If

    End Sub

    Sub CargarGridView()
        'log.Debug("CargarGridView()")
        Try

            ''--------------------------------------------------------------------------
            ''Dim gtkObj As New gtkMatenimientoSist
            ' ''#If DEBUG Then
            ' ''ListaGTK = gtkObj.Listado(FiltroGTK).OrderByDescending(Function(o) o.Id).Take(3).ToList
            ' ''#Else
            ''ListaGTK = gtkObj.Listado(FiltroGTK)
            ' ''#End If
            ''If ListaGTK IsNot Nothing AndAlso ListaGTK.Count > 0 Then gvGertakariak.DataSource = ListaGTK
            ''--------------------------------------------------------------------------
            ''FROGA:2013-04-17: Cambiamos "GertakariakLib2" por "Entity".
            ''-------------------------------------------------------------------------
            ''IDTIPOINCIDENCIA = 6 -> Sistemas Mantenimiento. Identificador del Tipo de Incidencia y Tipo de Documento
            ''-------------------------------------------------------------------------
            ''Dim BBDD As New BatzBBDD.Entities_Gertakariak

            ''LEFT JOIN - Se consigue con "DefaultIfEmpty"
            ''---------------------------------------------------------------------------------------------------------------------------------
            'Dim Lista As IQueryable(Of BatzBBDD.GERTAKARIAK) = _
            '	From gtkMant As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK _
            '	From gtkAccion As BatzBBDD.ACCIONES In gtkMant.ACCIONES.DefaultIfEmpty _
            '		From gtkAccionUSR As BatzBBDD.ACCIONES_USUARIOS In gtkAccion.ACCIONES_USUARIOS.DefaultIfEmpty _
            '	From gtkEstruc As BatzBBDD.ESTRUCTURA In gtkMant.ESTRUCTURA.DefaultIfEmpty _
            '	From gtkResp As BatzBBDD.RESPONSABLES_GERTAKARIAK In gtkMant.RESPONSABLES_GERTAKARIAK.DefaultIfEmpty _
            '		Where gtkMant.IDTIPOINCIDENCIA = 6 _
            '		And (If(FiltroGTK.FechaPrevista IsNot Nothing And FiltroGTK.FechaPrevistaFin IsNot Nothing, _
            '				gtkAccion.FECHAPREVISTA >= FiltroGTK.FechaPrevista And gtkAccion.FECHAPREVISTA <= FiltroGTK.FechaPrevistaFin, _
            '				If(FiltroGTK.FechaPrevista IsNot Nothing Or FiltroGTK.FechaPrevistaFin IsNot Nothing, _
            '				   gtkAccion.FECHAPREVISTA >= FiltroGTK.FechaPrevista Or gtkAccion.FECHAPREVISTA <= FiltroGTK.FechaPrevistaFin, True = True))) _
            '		And If(FiltroGTK.Caracteristicas.Count > 0, FiltroGTK.Caracteristicas.Contains(gtkEstruc.ID), True = True) _
            '		And If(FiltroGTK.Responsables.Count > 0, FiltroGTK.Responsables.Contains(gtkResp.IDUSUARIO) Or FiltroGTK.Responsables.Contains(gtkAccionUSR.IDUSUARIO), True = True) _
            '		And If(FiltroGTK.Activos.Count > 0, FiltroGTK.Activos.Contains(gtkMant.IDACTIVO), True = True) _
            '	Select gtkMant Distinct
            ''---------------------------------------------------------------------------------------------------------------------------------

            ''---------------------------------------------------------------------------------------------------------------------------------
            ''Sustitucion de la linea "And If(FiltroGTK.Estado Is Nothing, True = True, If(FiltroGTK.Estado = gtkFiltro.EstadoIncidencia.Abierta, gtkMant.FECHACIERRE Is Nothing, gtkMant.FECHACIERRE IsNot Nothing)) _"
            ''porque en los servidores no funcina por temas del FrameWork. En local si funciona.
            ''---------------------------------------------------------------------------------------------------------------------------------
            'If FiltroGTK.Estado IsNot Nothing Then
            '	If FiltroGTK.Estado = gtkFiltro.EstadoIncidencia.Abierta Then
            '		Lista = From gtkMant As BatzBBDD.GERTAKARIAK In Lista Where gtkMant.FECHACIERRE Is Nothing Select gtkMant Distinct
            '	Else
            '		Lista = From gtkMant As BatzBBDD.GERTAKARIAK In Lista Where gtkMant.FECHACIERRE IsNot Nothing Select gtkMant Distinct
            '	End If
            'End If
            ''---------------------------------------------------------------------------------------------------------------------------------

            ''============================================================================================================
            ''Transformamos el resultado en una lista para poder usar funciones que Oracle no soporta.
            ''============================================================================================================
            ''_____________________________________________________________________________________________________________
            ''log.Debug("DataSource = ListaGTK2")
            'ListaGTK = Lista.ToList
            ''---------------------------------------------------------------------------------------------------------
            ''Texto a buscar
            ''---------------------------------------------------------------------------------------------------------
            'If FiltroGTK.Descripcion IsNot Nothing Then
            '	Dim aPrefixText As String() = FiltroGTK.Descripcion.Split(" ")
            '	For Each TxTBusqueda As String In aPrefixText
            '		'Transformamos el texto en una expresion regular.
            '		Dim ExpReg As Regex = New Regex(SabLib.BLL.Utils.TextoLike(TxTBusqueda), RegexOptions.IgnoreCase)
            '		'LEFT JOIN (de 3 tablas)
            '		ListaGTK = (From gtkMant As BatzBBDD.GERTAKARIAK In ListaGTK _
            '					From gtkAccion As BatzBBDD.ACCIONES In gtkMant.ACCIONES.DefaultIfEmpty(New BatzBBDD.ACCIONES) _
            '					From gtkAccionObs As BatzBBDD.ACCIONES_OBSERVACIONES In gtkAccion.ACCIONES_OBSERVACIONES.DefaultIfEmpty(New BatzBBDD.ACCIONES_OBSERVACIONES) _
            '						Where ExpReg.IsMatch(gtkMant.ID) _
            '						Or If(gtkMant.DESCRIPCIONPROBLEMA Is Nothing, Nothing, ExpReg.IsMatch(gtkMant.DESCRIPCIONPROBLEMA)) _
            '						Or If(gtkMant.TITULO Is Nothing, Nothing, ExpReg.IsMatch(gtkMant.TITULO)) _
            '						Or If(gtkMant.CAUSAPROBLEMA Is Nothing, Nothing, ExpReg.IsMatch(gtkMant.CAUSAPROBLEMA)) _
            '						Or If(gtkMant.OBSERVACIONESCOSTE Is Nothing, Nothing, ExpReg.IsMatch(gtkMant.OBSERVACIONESCOSTE)) _
            '						Or If(gtkAccion.DESCRIPCION Is Nothing, Nothing, ExpReg.IsMatch(gtkAccion.DESCRIPCION)) _
            '						Or If(gtkAccion.EFICACIA Is Nothing, Nothing, ExpReg.IsMatch(gtkAccion.EFICACIA)) _
            '						Or If(gtkAccionObs.DESCRIPCION Is Nothing, Nothing, ExpReg.IsMatch(gtkAccionObs.DESCRIPCION)) _
            '					Select gtkMant).ToList
            '	Next
            'End If
            ''---------------------------------------------------------------------------------------------------------
            'If ListaGTK IsNot Nothing AndAlso ListaGTK.FirstOrDefault IsNot Nothing Then gvGertakariak.DataSource = ListaGTK
            ''log.Warn("DataSource = ListaGTK2")

            ''-----------------------------------------------------------------------------------------
            ''Pasamos el resultado del filtro al control de Acciones 
            ''para poder definer las acciones parciales (acciones que se ajustan al filtro)
            ''-----------------------------------------------------------------------------------------
            ''Dim AP As AccionesPendientes
            ''AP = Master.FindControl("cuAccionesPendientes")
            ''AP.lGTK = ListaGTK
            ''-----------------------------------------------------------------------------------------

            ''_____________________________________________________________________________________________________________
            ''FROGA:2013-05-24: No conseguimos que baje el tiempo. 
            ''"ListaGTK2 = Lista.ToList" tarda lo mismo que "gvGertakariak.DataSource = l2.ToList".
            ''_____________________________________________________________________________________________________________
            ''---------------------------------------------------------------------------------------------------------
            ''Texto a buscar
            ''---------------------------------------------------------------------------------------------------------
            ' ''log.Debug("DataSource = Lista")
            ''Dim l2 As IEnumerable(Of BatzBBDD.GERTAKARIAK) = From l In Lista.AsEnumerable Select l

            ''If FiltroGTK.Descripcion IsNot Nothing Then
            ''	Dim aPrefixText As String() = FiltroGTK.Descripcion.Split(" ")
            ''	For Each TxTBusqueda As String In aPrefixText
            ''		'Transformamos el texto en una expresion regular.
            ''		Dim ExpReg As Regex = New Regex(SabLib.BLL.Utils.TextoLike(TxTBusqueda), RegexOptions.IgnoreCase)
            ''		l2 = From gtkMant As BatzBBDD.GERTAKARIAK In l2 _
            ''			 From gtkAccion As BatzBBDD.ACCIONES In gtkMant.ACCIONES.DefaultIfEmpty(New BatzBBDD.ACCIONES) _
            ''			 From gtkAccionObs As BatzBBDD.ACCIONES_OBSERVACIONES In gtkAccion.ACCIONES_OBSERVACIONES.DefaultIfEmpty(New BatzBBDD.ACCIONES_OBSERVACIONES) _
            ''			 Where ExpReg.IsMatch(gtkMant.ID) _
            ''				Or If(gtkMant.DESCRIPCIONPROBLEMA Is Nothing, Nothing, ExpReg.IsMatch(gtkMant.DESCRIPCIONPROBLEMA)) _
            ''				Or If(gtkMant.TITULO Is Nothing, Nothing, ExpReg.IsMatch(gtkMant.TITULO)) _
            ''				Or If(gtkMant.CAUSAPROBLEMA Is Nothing, Nothing, ExpReg.IsMatch(gtkMant.CAUSAPROBLEMA)) _
            ''				Or If(gtkMant.OBSERVACIONESCOSTE Is Nothing, Nothing, ExpReg.IsMatch(gtkMant.OBSERVACIONESCOSTE)) _
            ''				Or If(gtkAccion.DESCRIPCION Is Nothing, Nothing, ExpReg.IsMatch(gtkAccion.DESCRIPCION)) _
            ''				Or If(gtkAccion.EFICACIA Is Nothing, Nothing, ExpReg.IsMatch(gtkAccion.EFICACIA)) _
            ''				Or If(gtkAccionObs.DESCRIPCION Is Nothing, Nothing, ExpReg.IsMatch(gtkAccionObs.DESCRIPCION)) _
            ''			 Select gtkMant Distinct
            ''	Next
            ''End If
            ' ''---------------------------------------------------------------------------------------------------------

            ''If Lista.FirstOrDefault IsNot Nothing Then gvGertakariak.DataSource = l2.Take(20)
            ' ''log.Warn("DataSource = Lista")
            ''_____________________________________________________________________________________________________________

            ''============================================================================================================

            ListaGTK = ListadoIncidencias(FiltroGTK)
            If ListaGTK IsNot Nothing AndAlso ListaGTK.FirstOrDefault IsNot Nothing Then gvGertakariak.DataSource = ListaGTK
        Catch ex As Exception
            log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
        'log.Warn("CargarGridView()")
    End Sub

    Public Function ListadoIncidencias(ByVal FiltroGTK As gtkFiltro) As List(Of BatzBBDD.GERTAKARIAK)
        ListadoIncidencias = Nothing
        Try
            '-------------------------------------------------------------------------
            'IDTIPOINCIDENCIA = 6 -> Sistemas Mantenimiento. Identificador del Tipo de Incidencia y Tipo de Documento
            '-------------------------------------------------------------------------
            'LEFT JOIN - Se consigue con "DefaultIfEmpty"
            '---------------------------------------------------------------------------------------------------------------------------------
            Dim Lista As IQueryable(Of BatzBBDD.GERTAKARIAK) =
                From gtkMant As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK
                From gtkAccion As BatzBBDD.ACCIONES In gtkMant.ACCIONES.DefaultIfEmpty
                From gtkAccionUSR As BatzBBDD.ACCIONES_USUARIOS In gtkAccion.ACCIONES_USUARIOS.DefaultIfEmpty
                From gtkEstruc As BatzBBDD.ESTRUCTURA In gtkMant.ESTRUCTURA.DefaultIfEmpty
                From gtkResp As BatzBBDD.RESPONSABLES_GERTAKARIAK In gtkMant.RESPONSABLES_GERTAKARIAK.DefaultIfEmpty
                Where gtkMant.IDTIPOINCIDENCIA = 6 _
                    And (If(FiltroGTK.FechaPrevista IsNot Nothing And FiltroGTK.FechaPrevistaFin IsNot Nothing,
                            gtkAccion.FECHAPREVISTA >= FiltroGTK.FechaPrevista And gtkAccion.FECHAPREVISTA <= FiltroGTK.FechaPrevistaFin,
                            If(FiltroGTK.FechaPrevista IsNot Nothing Or FiltroGTK.FechaPrevistaFin IsNot Nothing,
                               gtkAccion.FECHAPREVISTA >= FiltroGTK.FechaPrevista Or gtkAccion.FECHAPREVISTA <= FiltroGTK.FechaPrevistaFin, True = True))) _
                    And If(FiltroGTK.Caracteristicas.Any, FiltroGTK.Caracteristicas.Contains(gtkEstruc.ID), True = True) _
                    And If(FiltroGTK.Responsables.Any, FiltroGTK.Responsables.Contains(gtkResp.IDUSUARIO) Or FiltroGTK.Responsables.Contains(gtkAccionUSR.IDUSUARIO), True = True) _
                    And If(FiltroGTK.Activos.Any, FiltroGTK.Activos.Contains(gtkMant.IDACTIVO), True = True)
                Select gtkMant Distinct
            '---------------------------------------------------------------------------------------------------------------------------------

            '---------------------------------------------------------------------------------------------------------------------------------
            'Sustitucion de la linea "And If(FiltroGTK.Estado Is Nothing, True = True, If(FiltroGTK.Estado = gtkFiltro.EstadoIncidencia.Abierta, gtkMant.FECHACIERRE Is Nothing, gtkMant.FECHACIERRE IsNot Nothing)) _"
            'porque en los servidores no funcina por temas del FrameWork. En local si funciona.
            '---------------------------------------------------------------------------------------------------------------------------------
            If FiltroGTK.Estado IsNot Nothing Then
                If FiltroGTK.Estado = gtkFiltro.EstadoIncidencia.Abierta Then
                    Lista = From gtkMant As BatzBBDD.GERTAKARIAK In Lista Where gtkMant.FECHACIERRE Is Nothing Select gtkMant Distinct
                Else
                    Lista = From gtkMant As BatzBBDD.GERTAKARIAK In Lista Where gtkMant.FECHACIERRE IsNot Nothing Select gtkMant Distinct
                End If
            End If
            '---------------------------------------------------------------------------------------------------------------------------------

            '============================================================================================================
            'Transformamos el resultado en una lista para poder usar funciones que Oracle no soporta.
            '============================================================================================================
            '_____________________________________________________________________________________________________________
            'log.Debug("DataSource = ListaGTK2")
            ListadoIncidencias = Lista.ToList
            '---------------------------------------------------------------------------------------------------------
            'Texto a buscar
            '---------------------------------------------------------------------------------------------------------
            If FiltroGTK.Descripcion IsNot Nothing Then
                Dim aPrefixText As String() = FiltroGTK.Descripcion.Split(" ")
                For Each TxTBusqueda As String In aPrefixText
                    'Transformamos el texto en una expresion regular.
                    Dim ExpReg As Regex = New Regex(SabLib.BLL.Utils.TextoLike(TxTBusqueda), RegexOptions.IgnoreCase)
                    'LEFT JOIN (de 3 tablas)
                    ListadoIncidencias = (From gtkMant As BatzBBDD.GERTAKARIAK In ListadoIncidencias
                                          From gtkAccion As BatzBBDD.ACCIONES In gtkMant.ACCIONES.DefaultIfEmpty(New BatzBBDD.ACCIONES)
                                          From gtkAccionObs As BatzBBDD.ACCIONES_OBSERVACIONES In gtkAccion.ACCIONES_OBSERVACIONES.DefaultIfEmpty(New BatzBBDD.ACCIONES_OBSERVACIONES)
                                          Where ExpReg.IsMatch(gtkMant.ID) _
                                    Or If(gtkMant.DESCRIPCIONPROBLEMA Is Nothing, Nothing, ExpReg.IsMatch(gtkMant.DESCRIPCIONPROBLEMA)) _
                                    Or If(gtkMant.TITULO Is Nothing, Nothing, ExpReg.IsMatch(gtkMant.TITULO)) _
                                    Or If(gtkMant.CAUSAPROBLEMA Is Nothing, Nothing, ExpReg.IsMatch(gtkMant.CAUSAPROBLEMA)) _
                                    Or If(gtkMant.OBSERVACIONESCOSTE Is Nothing, Nothing, ExpReg.IsMatch(gtkMant.OBSERVACIONESCOSTE)) _
                                    Or If(gtkAccion.DESCRIPCION Is Nothing, Nothing, ExpReg.IsMatch(gtkAccion.DESCRIPCION)) _
                                    Or If(gtkAccion.EFICACIA Is Nothing, Nothing, ExpReg.IsMatch(gtkAccion.EFICACIA)) _
                                    Or If(gtkAccionObs.DESCRIPCION Is Nothing, Nothing, ExpReg.IsMatch(gtkAccionObs.DESCRIPCION))
                                          Select gtkMant Distinct).ToList
                Next
            End If
            '---------------------------------------------------------------------------------------------------------

        Catch ex As Exception
            log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
        Return ListadoIncidencias
    End Function

#Region "Crear TreeViews del Filtro"
    Dim lNodos As New List(Of TreeNode)
    Dim lCaracteristicas As New List(Of Nullable(Of Integer))
    Dim lTreeView As New List(Of TreeView)

    Dim lEstructuras As New List(Of gtkEstructura)
    Dim lEstructuras2 As IEnumerable(Of BatzBBDD.ESTRUCTURA) = Nothing

    Sub BuscarNodos()
        '--------------------------------------------------------------------------------------------------------------------------------------
        'Obtenemos todas las caracteristicas que estan asignadas a las incidencias
        '--------------------------------------------------------------------------------------------------------------------------------------
        'FROGA:2013-05-28:
        '--------------------------------------------------------------------------------------------------------------------------------------
        If ListaGTK IsNot Nothing AndAlso ListaGTK.Any Then
            'log.Debug("BuscarNodos - 1")

            lCaracteristicas = (From gtkMant As BatzBBDD.GERTAKARIAK In ListaGTK
                                From gtkEstruc As BatzBBDD.ESTRUCTURA In gtkMant.ESTRUCTURA.DefaultIfEmpty(Nothing)
                                Where gtkEstruc IsNot Nothing
                                Select CType(gtkEstruc.ID, Nullable(Of Integer)) Distinct).ToList
            If lCaracteristicas IsNot Nothing Then
                For Each Id As Integer In lCaracteristicas
                    BuscarEstructuras(Id)
                Next

                For Each NodoPadre As gtkEstructura In lEstructuras.FindAll(Function(o) o.IdIturria Is Nothing)
                    Dim TreeView As New TreeView
                    CargarTreeView(TreeView, NodoPadre)
                    lTreeView.Add(TreeView)
                Next
            End If
            'log.Warn("BuscarNodos - 1")


        End If
        ''---------------------------------------------------------------------------------------------------
        ''FROGA:2013-05-31:
        ''---------------------------------------------------------------------------------------------------
        'lEstructuras2 = From gtkMant As BatzBBDD.GERTAKARIAK In ListaGTK _
        '	 From gtkEstruc As BatzBBDD.ESTRUCTURA In gtkMant.ESTRUCTURA _
        '	 Join gtkEst As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA On gtkEstruc.IDITURRIA Equals gtkEst.ID _
        '	 Where gtkEstruc IsNot Nothing _
        '	 Select gtkEstruc Distinct

        'If lEstructuras2.FirstOrDefault IsNot Nothing Then

        '	For Each IdEst As Integer In lCaracteristicas
        '		BuscarEstructuras2(IdEst)
        '	Next


        '	For Each NodoPadre As BatzBBDD.ESTRUCTURA In lEstructuras2
        '		Dim TreeView As New TreeView
        '		CargarTreeView2(TreeView, NodoPadre)
        '		lTreeView.Add(TreeView)
        '	Next
        'End If

        'log.Warn("BuscarNodos - 2")
        ''---------------------------------------------------------------------------------------------------
        '--------------------------------------------------------------------------------------------------------------------------------------

    End Sub

    Sub BuscarEstructuras(ID As Integer)
        Dim Estructura As New gtkEstructura
        Estructura.Cargar(ID)
        If lEstructuras IsNot Nothing AndAlso Estructura.Id IsNot Nothing _
            AndAlso lEstructuras.Exists(Function(e) e.Id = Estructura.Id) = False Then
            lEstructuras.Add(Estructura)
        End If
        If Estructura.IdIturria IsNot Nothing Then
            BuscarEstructuras(Estructura.IdIturria)
        End If
    End Sub

    Sub CargarTreeView(TreeView As TreeView, Estructura As gtkEstructura, Optional TreeNodo As TreeNode = Nothing)
        If Estructura IsNot Nothing Then
            If lEstructuras.Exists(Function(o) o.Id = Estructura.Id) Then
                '-------------------------------------------------------------------------------
                'Creamos el nodo. 
                'Solo se puede seleccionar si la caracteristica esta en el resultado del listado.
                'Expandimos el Nodo Primario y los Nodos Secundarios para que se muestren los seleccionados.
                '-------------------------------------------------------------------------------
                Dim Nodo As New TreeNode With
                    {.SelectAction = TreeNodeSelectAction.Expand,
                     .Value = Estructura.Id, .Text = Estructura.Descripcion,
                     .ShowCheckBox = (lCaracteristicas.Contains(Estructura.Id)),
                     .Expanded = (TreeNodo Is Nothing OrElse FiltroGTK.Caracteristicas IsNot Nothing AndAlso FiltroGTK.Caracteristicas.Any)}
                '-------------------------------------------------------------------------------

                '-------------------------------------------------------------------------------
                'Indicamos si el nodo es "Primario" o "Secundario".
                '-------------------------------------------------------------------------------
                If TreeNodo Is Nothing Then TreeView.Nodes.Add(Nodo) Else TreeNodo.ChildNodes.Add(Nodo)
                '-------------------------------------------------------------------------------

                '-------------------------------------------------------------------------------
                'Generamos el siguiente Nodo.
                '---------------------------------------------------------------------------------------------
                If Estructura.Nodos IsNot Nothing Then
                    For Each EstructuraNodo As gtkEstructura In Estructura.Nodos
                        If Estructura.Nodos IsNot Nothing Then CargarTreeView(TreeView, EstructuraNodo, Nodo)
                    Next
                End If
                '-------------------------------------------------------------------------------
            End If
        End If
    End Sub

    ''' <summary>
    ''' Proceso para marcar como seleccionado el nodo y expandir el arbol hasta él.
    ''' </summary>
    ''' <param name="TreeNodeCollection"></param>
    ''' <param name="lKey">Listada del "Value" de los Nodos (TreeNode).</param>
    ''' <remarks></remarks>
    Sub SeleccionarNodos(TreeNodeCollection As TreeNodeCollection, lKey As List(Of Integer))
        If TreeNodeCollection IsNot Nothing AndAlso TreeNodeCollection.Count > 0 Then
            For Each Nodo As TreeNode In TreeNodeCollection
                If lKey.Contains(Nodo.Value) Then Nodo.Checked = True : Nodo.Parent.Expanded = Nodo.Checked
                If Nodo.ChildNodes.Count > 0 Then SeleccionarNodos(Nodo.ChildNodes, lKey)
            Next
        End If
    End Sub
#End Region
#End Region
#Region "Prototipo"
    ' ''' <summary>
    ' ''' Buscamos el padre del identificador de estructura que pasamos.
    ' ''' </summary>
    ' ''' <param name="IdEst">Identificador de estructura hijo del cual queremos saber el padre (Origen).</param>
    ' ''' <remarks></remarks>
    'Sub BuscarEstructuras2(IdEst As Integer)
    '	Dim IE_Estructura As IEnumerable(Of BatzBBDD.ESTRUCTURA) = _
    '		From Est As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA _
    '		Where Est.ID = IdEst _
    '		Select Est
    '	'And Not lEstructuras2.Contains(Est) _

    '	If IE_Estructura IsNot Nothing Then
    '		'	For Each Est As BatzBBDD.ESTRUCTURA In IE_Estructura
    '		'		If Not lEstructuras2.Contains(Est) Then lEstructuras2 = lEstructuras2.Union(IE_Estructura)
    '		'		If Est.IDITURRIA IsNot Nothing Then BuscarEstructuras2(Est.IDITURRIA)
    '		'	Next

    '		If Not lEstructuras2.Contains(IE_Estructura.ElementAt(0)) Then
    '			lEstructuras2 = lEstructuras2.Union(IE_Estructura)
    '		End If


    '		Dim i As Integer = lEstructuras2.Count

    '		If IE_Estructura.ElementAt(0).IDITURRIA IsNot Nothing Then BuscarEstructuras2(IE_Estructura.ElementAt(0).IDITURRIA)


    '	End If

    'End Sub

    'Sub CargarTreeView2(TreeView As TreeView, Estructura As BatzBBDD.ESTRUCTURA, Optional TreeNodo As TreeNode = Nothing)
    '	If Estructura IsNot Nothing Then
    '		If lEstructuras2.Contains(Estructura) Then
    '			'-------------------------------------------------------------------------------
    '			'Creamos el nodo. 
    '			'Solo se puede seleccionar si la caracteristica esta en el resultado del listado.
    '			'Expandimos el Nodo Primario y los Nodos Secundarios para que se muestren los seleccionados.
    '			'-------------------------------------------------------------------------------
    '			Dim Nodo As New TreeNode With _
    '				{.SelectAction = TreeNodeSelectAction.Expand, _
    '				 .Value = Estructura.ID, .Text = Estructura.DESCRIPCION, _
    '				 .ShowCheckBox = (lCaracteristicas.Contains(Estructura.ID)), _
    '				 .Expanded = (TreeNodo Is Nothing OrElse FiltroGTK.Caracteristicas IsNot Nothing AndAlso FiltroGTK.Caracteristicas.Count > 0)}
    '			'-------------------------------------------------------------------------------

    '			'-------------------------------------------------------------------------------
    '			'Indicamos si el nodo es "Primario" o "Secundario".
    '			'-------------------------------------------------------------------------------
    '			If TreeNodo Is Nothing Then TreeView.Nodes.Add(Nodo) Else TreeNodo.ChildNodes.Add(Nodo)
    '			'-------------------------------------------------------------------------------

    '			'-------------------------------------------------------------------------------
    '			'Generamos el siguiente Nodo.
    '			'---------------------------------------------------------------------------------------------
    '			Dim subNodo As IQueryable(Of BatzBBDD.ESTRUCTURA) = From Est As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA Where Est.IDITURRIA = Estructura.ID Select Est
    '			If subNodo.FirstOrDefault IsNot Nothing Then
    '				For Each EstructuraNodo As BatzBBDD.ESTRUCTURA In subNodo
    '					If EstructuraNodo IsNot Nothing Then CargarTreeView2(TreeView, EstructuraNodo, Nodo)
    '				Next
    '			End If
    '			'-------------------------------------------------------------------------------
    '		End If
    '	End If
    'End Sub
#End Region


End Class