Public Class CCGastos
    Inherits System.Web.UI.Page

    Public Property ccEditando As Integer
        Get
            If ViewState("ccEditando") Is Nothing Then ViewState("ccEditando") = ""
            Return ViewState("ccEditando").ToString()
        End Get

        Set(ByVal value As Integer)
            ViewState("ccEditando") = value
        End Set

    End Property

    Public Property gastosVentaIdEditando As Integer
        Get
            If ViewState("gastosVentaIdEditando") Is Nothing Then ViewState("gastosVentaIdEditando") = ""
            Return ViewState("gastosVentaIdEditando").ToString()
        End Get

        Set(ByVal value As Integer)
            ViewState("gastosVentaIdEditando") = value
        End Set

    End Property

    Public Property gastosVentaEditando As String
        Get
            If ViewState("gastosVentaEditando") Is Nothing Then ViewState("gastosVentaEditando") = ""
            Return ViewState("gastosVentaEditando").ToString()
        End Get

        Set(ByVal value As String)
            ViewState("gastosVentaEditando") = value
        End Set

    End Property

    Public Property tipoRepartoIdEditando As Integer
        Get
            If ViewState("tipoRepartoIdEditando") Is Nothing Then ViewState("tipoRepartoIdEditando") = ""
            Return ViewState("tipoRepartoIdEditando")
        End Get

        Set(ByVal value As Integer)
            ViewState("tipoRepartoIdEditando") = value
        End Set

    End Property

    Public Property tipoRepartoEditando As String
        Get
            If ViewState("tipoRepartoEditando") Is Nothing Then ViewState("tipoRepartoEditando") = ""
            Return ViewState("tipoRepartoEditando").ToString()
        End Get

        Set(ByVal value As String)
            ViewState("tipoRepartoEditando") = value
        End Set

    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            pnlCCPorVenta.Visible = True
            pnlNuevo.Visible = False
            btnNuevo.Visible = True
            cargarGrid()
            Master.Title("Gastos de la 164 y Materia Prima")
        End If

    End Sub

    Private Sub cargarGrid()
        Dim datos As DataTable
        datos = CCGastoPorVentaBLL.Obtener()
        grdCCGastosPorVenta.DataSource = datos
        grdCCGastosPorVenta.DataBind()

    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        ddlPartidaGasto.Items.Add("Seleccione partida de gasto...")
        For Each item As String() In Utilidades.ObtenerQuerySQLSERVERParametros("SELECT Partida_Gasto FROM M_Partidas_Gasto ORDER BY Partida_Gasto ASC")
            ddlPartidaGasto.Items.Add(item(0))
        Next
        chkExcepcionCarga.Checked = False
        ddlTipoReparto.Visible = True
        pnlNuevo.Visible = True
        txtCC.Text = ""
        ddlTipoReparto.ClearSelection()
        cargarTiposReparto()
        pnlNuevo.Visible = True

    End Sub

    Private Sub cargarTiposReparto()
        ddlTipoReparto.Items.Clear()
        ddlTipoReparto.Items.Add("Seleccione tipo de reparto...")
        For Each item As String() In Utilidades.ObtenerQuerySQLSERVERParametros("SELECT Tipo_Reparto FROM M_TIPOS_REPARTO")
            ddlTipoReparto.Items.Add(item(0))
        Next

    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim cc As Integer = txtCC.Text
        Dim partidagasto As String = ddlPartidaGasto.SelectedValue
        Dim excepcionCarga As Boolean = chkExcepcionCarga.Checked
        Dim tipoRepartoSeleccionado As String = ddlTipoReparto.SelectedValue()
        Dim tipoRepartoSeleccionadoId As String = "SELECT ID from M_Tipos_Reparto WHERE Tipo_Reparto LIKE '%" + tipoRepartoSeleccionado + "%'"
        Dim tipoRepartoId As Integer = Memcached.SQLServerDirectAccess.SeleccionarEscalar(Of Integer)(tipoRepartoSeleccionadoId, Master.Cx)
        CCGastoPorVentaBLL.Nuevo(cc, partidagasto, excepcionCarga, tipoRepartoId)
        pnlCCPorVenta.Controls.Add(New Label With {.Text = "CC Por Gasto añadida", .CssClass = "alert alert-success"})
        pnlNuevo.Visible = False
        cargarGrid()

    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        pnlNuevo.Visible = False

    End Sub

    Protected Sub grdCCGastosPorVenta_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdCCGastosPorVenta.RowEditing
        grdCCGastosPorVenta.EditIndex = e.NewEditIndex
        ccEditando = (CType(grdCCGastosPorVenta.Rows(e.NewEditIndex).Cells(1).Controls(1), Label)).Text
        gastosVentaEditando = (CType(grdCCGastosPorVenta.Rows(e.NewEditIndex).Cells(2).Controls(1), Label)).Text
        tipoRepartoEditando = (CType(grdCCGastosPorVenta.Rows(e.NewEditIndex).Cells(4).Controls(1), Label)).Text
        gastosVentaIdEditando = Convert.ToInt32((CType(grdCCGastosPorVenta.Rows(e.NewEditIndex).Cells(5).Controls(1), Label)).Text)
        tipoRepartoIdEditando = Convert.ToInt32((CType(grdCCGastosPorVenta.Rows(e.NewEditIndex).Cells(6).Controls(1), Label)).Text)
        cargarGrid()

    End Sub

    Protected Sub grdCCGastosPorVenta_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdCCGastosPorVenta.RowUpdating
        btnNuevo.Visible = False
        pnlNuevo.Visible = False
        Dim cc As Integer = Convert.ToInt32((CType(grdCCGastosPorVenta.Rows(e.RowIndex).Cells(1).Controls(1), TextBox)).Text)
        Dim gastosVentaId As Integer = Convert.ToInt32((CType(grdCCGastosPorVenta.Rows(e.RowIndex).Cells(2).Controls(1), DropDownList)).SelectedValue)
        Dim queryGastosVenta As String = "SELECT Partida_Gasto FROM M_Partidas_Gasto WHERE ID = '" + gastosVentaId.ToString() + "'"
        Dim gastosVenta As String = Memcached.SQLServerDirectAccess.SeleccionarEscalar(Of String)(queryGastosVenta, Master.Cx)
        Dim excepcionCarga As Boolean = Convert.ToBoolean((CType(grdCCGastosPorVenta.Rows(e.RowIndex).Cells(3).Controls(0), CheckBox)).Checked)
        Dim tipoRepartoId As Integer = Convert.ToInt32((CType(grdCCGastosPorVenta.Rows(e.RowIndex).Cells(4).Controls(1), DropDownList)).SelectedValue)
        Dim queryTipoReparto As String = "SELECT Partida_Gasto FROM M_Partidas_Gasto WHERE ID = '" + tipoRepartoId.ToString() + "'"
        Dim tipoReparto As String = Memcached.SQLServerDirectAccess.SeleccionarEscalar(Of String)(queryTipoReparto, Master.Cx)
        CCGastoPorVentaDAL.Actualizar(cc, gastosVenta, excepcionCarga, tipoRepartoId, ccEditando)
        grdCCGastosPorVenta.EditIndex = -1
        pnlCCPorVenta.Controls.Add(New Label With {.Text = "Registro actualizado", .CssClass = "alert alert-info"})
        btnNuevo.Visible = True
        cargarGrid()

    End Sub

    Protected Sub grdCCGastosPorVenta_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdCCGastosPorVenta.RowDeleting
        Dim cc As Integer = (CType(grdCCGastosPorVenta.Rows(e.RowIndex).Cells(1).Controls(1), Label)).Text
        CCGastoPorVentaDAL.Eliminar(cc)
        pnlCCPorVenta.Controls.Add(New Label With {.Text = "Cuenta Contable eliminada", .CssClass = "alert alert-danger"})
        cargarGrid()

    End Sub

    Protected Sub grdCCGastosPorVenta_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdCCGastosPorVenta.RowCancelingEdit
        grdCCGastosPorVenta.EditIndex = -1
        cargarGrid()

    End Sub

    Protected Sub grdCCGastosPorVenta_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdCCGastosPorVenta.PageIndexChanging
        If grdCCGastosPorVenta.EditIndex <> -1 Then
            e.Cancel = True
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "info_msg", "alert('Cancele la edición antes de cambiar de página.');", True)
        Else
            Dim grdCCPorNegocio As GridView = CType(sender, GridView)
            grdCCPorNegocio.PageIndex = e.NewPageIndex
            cargarGrid()
        End If

    End Sub

    Protected Sub grdCCGastosPorVenta_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdCCGastosPorVenta.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.Cells(3).Controls.Count > 0 Then
                If True Then
                    Dim crtl As Control = e.Row.Cells(2).Controls(1)
                    If TypeOf crtl Is DropDownList Then
                        Dim cmbGastosVenta As DropDownList = CType(e.Row.Cells(2).Controls(1), DropDownList)
                        Dim cmbTipoReparto As DropDownList = CType(e.Row.Cells(4).Controls(1), DropDownList)
                        If Not ccEditando.Equals("") Then
                            cargarComboGastosVenta(cmbGastosVenta)
                            cmbGastosVenta.SelectedValue = gastosVentaIdEditando.ToString()
                            cargarComboTipoReparto(cmbTipoReparto)
                            cmbTipoReparto.SelectedValue = tipoRepartoIdEditando.ToString()
                        Else
                            cargarComboGastosVenta(cmbGastosVenta)
                            cargarComboTipoReparto(cmbTipoReparto)
                        End If
                    End If
                End If
            End If
        End If

    End Sub

    Private Sub cargarComboGastosVenta(cmbGastosVenta As DropDownList)
        cmbGastosVenta.DataTextField = "Partida_Gasto"
        cmbGastosVenta.DataValueField = "ID"
        Dim dt As DataTable = CCGastoPorVentaBLL.ObtenerComboGastosVenta()
        Dim ddr As DataRow = dt.NewRow()
        cmbGastosVenta.DataSource = dt
        cmbGastosVenta.DataBind()

    End Sub

    Private Sub cargarComboTipoReparto(cmbTipoReparto As DropDownList)
        cmbTipoReparto.DataTextField = "Tipo_Reparto"
        cmbTipoReparto.DataValueField = "ID"
        Dim dt As DataTable = CCGastoPorVentaBLL.ObtenerComboTipoReparto()
        Dim ddr As DataRow = dt.NewRow()
        cmbTipoReparto.DataSource = dt
        cmbTipoReparto.DataBind()

    End Sub

End Class