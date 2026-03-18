Imports TelefoniaLib

Partial Public Class GrupoExtension
    Inherits PageBase

    Dim oGrupo As ELL.GrupoExtension
    Dim grupoComp As BLL.GrupoExtComponent

#Region "Page Load"

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(btnNuevo) : itzultzaileWeb.Itzuli(labelGrupo) : itzultzaileWeb.Itzuli(rfvGrupo)
            itzultzaileWeb.Itzuli(labelLibre) : itzultzaileWeb.Itzuli(btnGuardar) : itzultzaileWeb.Itzuli(btnEliminar)
        End If
    End Sub

    ''' <summary>
    ''' Carga el listado de grupos de extensiones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Grupos de extensiones"
                cargarGrupos()
            End If
            ConfigurarEventos()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Metodo para configurar ejecucion de scripts a controles, asignacion de textos,..
    ''' </summary>
    Private Sub ConfigurarEventos()
        Dim sms As String = itzultzaileWeb.Itzuli("confirmarEliminar")
        cfEliminar.ConfirmText = sms
    End Sub

#End Region

#Region "Botones Ver info e nuevo"

    ''' <summary>
    ''' Visualiza un pop-up con la informacion de un grupo, donde se puede registrar uno nuevo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub btnNuevo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNuevo.Click
        mostrarDetalle(Integer.MinValue)
    End Sub

    ''' <summary>
    ''' Carga los datos de un grupo
    ''' </summary>
    ''' <param name="idGrup">Identificador del grupo</param>
    Private Sub mostrarDetalle(ByVal idGrup As Integer)
        Try
            Dim titulo As String
            inicializarControles()
            If (idGrup = Integer.MinValue) Then  'nuevo
                titulo = "Nuevo"
                btnEliminar.Visible = False
                btnGuardar.ValidationGroup = "Grupo"
                setPaneles(True, False, False)
            Else 'modificar
                titulo = "Detalle"
                grupoComp = New BLL.GrupoExtComponent
                oGrupo = New ELL.GrupoExtension()
                oGrupo.Id = idGrup
                oGrupo = grupoComp.getGrupo(oGrupo)
                lblGrupo.Text = oGrupo.Nombre
                chbLibre.Checked = oGrupo.Libre
                btnGuardar.ValidationGroup = String.Empty
                btnEliminar.Visible = True
                setPaneles(False, True, False)
            End If
            lblTitPopPup.Text = titulo
            btnEliminar.CommandArgument = idGrup
            btnGuardar.CommandArgument = idGrup
            mpeGrupoExt.Show()
        Catch ex As Exception
            Dim batzEx As New BatzException("errVerInfo", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Cargar Datos"

    ''' <summary>
    ''' Carga todos las compañias de la planta que se esta gestionando
    ''' </summary>
    Private Sub cargarGrupos()
        Try
            Dim listGrupos As List(Of ELL.GrupoExtension)
            grupoComp = New BLL.GrupoExtComponent
            listGrupos = grupoComp.getGrupos(Master.Ticket.IdPlantaActual)
            listGrupos.Sort(New ELL.GrupoExtension.SortClass(GridViewSortExpresion, GridViewSortDirection))
            gvGrupos.DataSource = listGrupos            
            gvGrupos.DataBind()        
        Catch ex As Exception
            Dim batzEx As New BatzException("errMostrarGrupos", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Limpiar controles y gestion paneles"

    ''' <summary>
    ''' Limpia los controles para poder registrar un nuevo item
    ''' </summary>
    Private Sub inicializarControles()
        lblGrupo.Text = String.Empty
        txtGrupo.Text = String.Empty
        chbLibre.Checked = False
        setPaneles(False, False, False)
    End Sub

    ''' <summary>
    ''' Visualiza u oculta los paneles parametrizados
    ''' </summary>
    ''' <param name="pNuevo">Visualizar u ocultar el panel de nuevo telefono</param>
    ''' <param name="pExistente">Visualizar u ocultar el panel de un tlfno exitente</param>
    ''' <param name="pError">Visualizar u ocultar el panel de error</param>
    Private Sub setPaneles(ByVal pNuevo As Boolean, ByVal pExistente As Boolean, ByVal pError As Boolean)
        pnlNuevo.Visible = pNuevo
        pnlExistente.Visible = pExistente
        pnlError.Visible = pError
    End Sub

#End Region

#Region "Guardar Datos"

    ''' <summary>
    ''' Guarda el grupo si es nuevo y lo modifica si ya existiera
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnGuardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGuardar.Click
        Try
            If (Page.IsValid) Then
                grupoComp = New BLL.GrupoExtComponent
                oGrupo = New ELL.GrupoExtension With {.IdPlanta = Master.Ticket.IdPlantaActual, .Libre = chbLibre.Checked}
                If (CInt(btnGuardar.CommandArgument) = Integer.MinValue) Then 'nuevo
                    oGrupo.Nombre = txtGrupo.Text
                Else 'antiguo
                    oGrupo.Id = CInt(btnGuardar.CommandArgument)
                    oGrupo.Nombre = lblGrupo.Text
                End If
                grupoComp.Save(oGrupo)
                If (oGrupo.Id <> Integer.MinValue) Then
                    log.Info("Se ha modificado el grupo de extensiones - " & oGrupo.Nombre & " (" & oGrupo.Id & ")")
                Else
                    log.Info("Se ha insertado un nuevo grupo de extensiones - " & oGrupo.Nombre)
                End If
                Master.MensajeInfo = "grupoGuardado"
                cargarGrupos()
            Else
                mpeGrupoExt.Show()
            End If
        Catch batzEx As BatzException
            mpeGrupoExt.Show()
            WriteTitulo()
            pnlError.Visible = True
            lblError.Text = batzEx.Termino
        Catch ex As Exception
            mpeGrupoExt.Show()
            WriteTitulo()
            Dim batzEx As New BatzException("errGuardarGrupo", ex)
            pnlError.Visible = True
            lblError.Text = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Eliminar"

    ''' <summary>
    ''' Se elimina el registro seleccionado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnEliminar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        Try
            grupoComp = New BLL.GrupoExtComponent
            grupoComp.Delete(CInt(btnEliminar.CommandArgument))
            Master.MensajeInfo = "grupoBorrado"
            log.Info("Se ha eliminado el grupo de extensiones - " & btnEliminar.CommandArgument)
            cargarGrupos()
        Catch batzEx As BatzException
            mpeGrupoExt.Show()
            WriteTitulo()            
            pnlError.Visible = True
            lblError.Text = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Escribe el titulo de la popup
    ''' </summary>
    Private Sub WriteTitulo()
        lblTitPopPup.Text = If(txtGrupo.Visible, "Nuevo", "Detalle")
    End Sub

#End Region

#Region "GridView"

#Region "Orden"

    ''' <summary>
    ''' Evento para ordenar las columnas del resultado de la busqueda
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvGrupos_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvGrupos.Sorting
        Try
            GridViewSortDirection = If(GridViewSortDirection = SortDirection.Ascending, SortDirection.Descending, SortDirection.Ascending)
            GridViewSortExpresion = If(GridViewSortExpresion Is Nothing, ELL.GrupoExtension.PropertyNames.NOMBRE, e.SortExpression)
            cargarGrupos()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Indica la direccion en la que hay que ordenar
    ''' </summary>
    ''' <value></value>
    ''' <returns>Orden</returns>
    Public Property GridViewSortDirection() As SortDirection
        Get
            If (ViewState("sortDirection") Is Nothing) Then
                ViewState("sortDirection") = SortDirection.Ascending
            End If
            Return CType(ViewState("sortDirection"), SortDirection)
        End Get
        Set(ByVal value As SortDirection)
            ViewState("sortDirection") = value
        End Set
    End Property

    ''' <summary>
    ''' Indica la expresion de ordenacion
    ''' </summary>
    ''' <value></value>
    ''' <returns>Expresion</returns>
    Public Property GridViewSortExpresion() As String
        Get
            If (ViewState("sortExpresion") Is Nothing) Then
                ViewState("sortExpresion") = ELL.GrupoExtension.PropertyNames.NOMBRE
            End If
            Return ViewState("sortExpresion").ToString()
        End Get
        Set(ByVal value As String)
            ViewState("sortExpresion") = value
        End Set
    End Property

#End Region

#Region "Paginación"

    ''' <summary>
    ''' Se realiza la paginacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvGrupos_Paging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvGrupos.PageIndexChanging
        Try
            gvGrupos.PageIndex = e.NewPageIndex
            cargarGrupos()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "RowDataBound"

    ''' <summary>
    ''' <para>Añade la gestion de ordenes y estilos al pasar el raton por las lineas</para>
    ''' <para>Marca en verde, aquellos grupos libres</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvGrupos_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvGrupos.RowDataBound
        Try
            If (e.Row.RowType = DataControlRowType.Header) Then
                itzultzaileWeb.TraducirWebControls(e.Row.Controls)
            ElseIf e.Row.RowType = DataControlRowType.DataRow Then
                Dim oGrupo As ELL.GrupoExtension = e.Row.DataItem
                If (oGrupo.Libre) Then e.Row.CssClass = "fondoVerde"
                e.Row.Attributes.Add("onmouseover", "SartuN(this);")
                e.Row.Attributes.Add("onmouseout", "IrtenN(this);")
                e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gvGrupos, "Select$" + CStr(e.Row.DataItem.ID))
            End If
        Catch ex As Exception
            Dim batz As New BatzException("errMostrarGrupos", ex)
        End Try
    End Sub

#End Region

#Region "RowCommand"

    ''' <summary>
    ''' <para>Evento que captura el click del raton en cualquier area de la fila.</para>
    ''' <para>Redirecciona al detalle del usuario seleccionado</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvGrupos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvGrupos.RowCommand
        Try
            If (e.CommandName = "Select") Then
                mostrarDetalle(CInt(e.CommandArgument))
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#End Region

End Class