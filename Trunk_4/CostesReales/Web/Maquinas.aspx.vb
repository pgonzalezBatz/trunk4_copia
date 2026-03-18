Public Class Maquinas
    Inherits System.Web.UI.Page

    Public Property maquinaEditando As String
        Get
            If ViewState("maquinaEditando") Is Nothing Then ViewState("maquinaEditando") = ""
            Return ViewState("maquinaEditando").ToString()
        End Get

        Set(ByVal value As String)
            ViewState("maquinaEditando") = value
        End Set

    End Property

    Public Property maquinaDesEditando As String
        Get
            If ViewState("maquinaDesEditando") Is Nothing Then ViewState("maquinaDesEditando") = ""
            Return ViewState("maquinaDesEditando").ToString()
        End Get

        Set(ByVal value As String)
            ViewState("maquinaDesEditando") = value
        End Set

    End Property

    Public Property procesoEditando As String
        Get
            If ViewState("procesoEditando") Is Nothing Then ViewState("procesoEditando") = ""
            Return ViewState("procesoEditando").ToString()
        End Get

        Set(ByVal value As String)
            ViewState("procesoEditando") = value
        End Set

    End Property

    Public Property plantaEditando As String
        Get
            If ViewState("plantaEditando") Is Nothing Then ViewState("plantaEditando") = ""
            Return ViewState("plantaEditando").ToString()
        End Get

        Set(ByVal value As String)
            ViewState("plantaEditando") = value
        End Set

    End Property

    Public Property kwhEditando As Integer
        Get
            If ViewState("kwhEditando") Is Nothing Then ViewState("kwhEditando") = ""
            Return ViewState("kwhEditando").ToString()
        End Get

        Set(ByVal value As Integer)
            ViewState("kwhEditando") = value
        End Set

    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            pnlMaquinaClasificada.Visible = True
            pnlNuevo.Visible = False
            btnNuevo.Visible = True
            cargarGrid()
            Master.Title("Máquinas")
        End If

    End Sub

    Private Sub cargarGrid()
        Dim datos As DataTable
        datos = MaquinaBLL.Obtener()
        grdMaquinaClasificada.DataSource = datos
        grdMaquinaClasificada.DataBind()

    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        txtMaquina.Text = ""
        txtDescripcion.Text = ""
        ddlProcesos.ClearSelection()
        ddlPlantas.ClearSelection()
        cargarProcesos()
        cargarPlantas()
        pnlNuevo.Visible = True

    End Sub

    Private Sub cargarProcesos()
        ddlProcesos.Items.Clear()
        ddlProcesos.Items.Add("Seleccione proceso...")
        For Each item As String() In Utilidades.ObtenerQuerySQLSERVERParametros("SELECT PROCESO FROM T_PROCESOS")
            ddlProcesos.Items.Add(item(0))
        Next

    End Sub

    Private Sub cargarPlantas()
        ddlPlantas.Items.Clear()
        ddlPlantas.Items.Add("Seleccione planta...")
        For Each item As String() In Utilidades.ObtenerQuerySQLSERVERParametros("SELECT PLANTA FROM T_PLANTAS")
            ddlPlantas.Items.Add(item(0))
        Next

    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim maquina As String = txtMaquina.Text
        Dim descripcion As String = txtDescripcion.Text
        Dim proceso As String = ddlProcesos.SelectedValue
        Dim procesoSeleccionado As String = ddlProcesos.SelectedValue()
        Dim procesoSeleccionadoId As String = "SELECT ID from T_PROCESOS WHERE PROCESO LIKE '%" + procesoSeleccionado + "%'"
        Dim planta As String = ddlPlantas.SelectedValue
        Dim plantaSeleccionada As String = ddlPlantas.SelectedValue()
        Dim plantaSeleccionadaId As String = "SELECT ID from T_PLANTAS WHERE PLANTA like '%" + plantaSeleccionada + "%'"
        Dim kwh As Integer = txtKwh.Text
        Dim proceso_id As Integer = Memcached.SQLServerDirectAccess.SeleccionarEscalar(Of Integer)(procesoSeleccionadoId, Master.Cx)
        Dim planta_id As Integer = Memcached.SQLServerDirectAccess.SeleccionarEscalar(Of Integer)(plantaSeleccionadaId, Master.Cx)
        MaquinaDAL.Nuevo(maquina, descripcion, proceso_id, planta_id, kwh)
        pnlMaquinaClasificada.Controls.Add(New Label With {.Text = "Máquina añadida", .CssClass = "alert alert-success"})
        pnlNuevo.Visible = False
        cargarGrid()

    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        pnlNuevo.Visible = False

    End Sub

    Protected Sub grdMaquinaClasificada_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdMaquinaClasificada.RowEditing
        grdMaquinaClasificada.EditIndex = e.NewEditIndex
        maquinaEditando = (CType(grdMaquinaClasificada.Rows(e.NewEditIndex).Cells(1).Controls(1), Label)).Text
        maquinaDesEditando = (CType(grdMaquinaClasificada.Rows(e.NewEditIndex).Cells(2).Controls(1), Label)).Text
        procesoEditando = (CType(grdMaquinaClasificada.Rows(e.NewEditIndex).Cells(6).Controls(1), Label)).Text
        plantaEditando = (CType(grdMaquinaClasificada.Rows(e.NewEditIndex).Cells(4).Controls(1), Label)).Text
        kwhEditando = (CType(grdMaquinaClasificada.Rows(e.NewEditIndex).Cells(5).Controls(1), Label)).Text
        cargarGrid()

    End Sub

    Protected Sub grdMaquinaClasificada_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdMaquinaClasificada.RowUpdating
        btnNuevo.Visible = False
        pnlNuevo.Visible = False
        Dim maquina As String = CType(grdMaquinaClasificada.Rows(e.RowIndex).Cells(1).Controls(1), Label).Text
        Dim descripcion As String = CType(grdMaquinaClasificada.Rows(e.RowIndex).Cells(2).Controls(1), TextBox).Text
        Dim proceso As Integer = Convert.ToInt32((CType((grdMaquinaClasificada.Rows(e.RowIndex).Cells(3).Controls(1)), DropDownList)).SelectedValue)
        Dim planta As Integer = Convert.ToInt32((CType((grdMaquinaClasificada.Rows(e.RowIndex).Cells(4).Controls(1)), DropDownList)).SelectedValue)
        Dim kwh As String = CType(grdMaquinaClasificada.Rows(e.RowIndex).Cells(5).Controls(1), TextBox).Text
        MaquinaDAL.Actualizar(maquina, descripcion, proceso, planta, kwh)
        grdMaquinaClasificada.EditIndex = -1
        grdMaquinaClasificada.Controls.Add(New Label With {.Text = "Registro actualizado", .CssClass = "alert alert-info"})
        btnNuevo.Visible = True
        cargarGrid()

    End Sub

    Protected Sub grdMaquinaClasificada_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdMaquinaClasificada.RowDeleting
        Dim maquina As String = (CType(grdMaquinaClasificada.Rows(e.RowIndex).Cells(1).Controls(1), Label)).Text
        MaquinaDAL.Eliminar(maquina)
        pnlMaquinaClasificada.Controls.Add(New Label With {.Text = "Máquina eliminada", .CssClass = "alert alert-danger"})
        cargarGrid()

    End Sub

    Protected Sub grdMaquinaClasificada_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdMaquinaClasificada.RowCancelingEdit
        grdMaquinaClasificada.EditIndex = -1
        cargarGrid()

    End Sub

    Protected Sub grdMaquinaClasificada_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdMaquinaClasificada.PageIndexChanging
        If grdMaquinaClasificada.EditIndex <> -1 Then
            e.Cancel = True
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "info_msg", "alert('Cancele la edición antes de cambiar de página.');", True)
        Else
            Dim grdMaquinaClasificada As GridView = CType(sender, GridView)
            grdMaquinaClasificada.PageIndex = e.NewPageIndex
            cargarGrid()
        End If

    End Sub

    Protected Sub grdMaquinaClasificada_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdMaquinaClasificada.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.Cells(3).Controls.Count > 0 Then
                If True Then
                    Dim crtl As Control = e.Row.Cells(3).Controls(1)
                    If TypeOf crtl Is DropDownList Then
                        Dim cmbProcesos As DropDownList = CType(e.Row.Cells(3).Controls(1), DropDownList)
                        Dim cmbPlantas As DropDownList = CType(e.Row.Cells(4).Controls(1), DropDownList)
                        Dim txtKwh As TextBox = CType(e.Row.Cells(5).Controls(1), TextBox)
                        If Not maquinaEditando.Equals("") Then
                            cargarComboProcesos(cmbProcesos)
                            cmbProcesos.SelectedValue = procesoEditando
                            cargarComboPlantas(cmbPlantas)
                            cmbPlantas.SelectedValue = plantaEditando
                            txtKwh.Text = kwhEditando
                        Else
                            cargarComboProcesos(cmbProcesos)
                            cargarComboPlantas(cmbPlantas)
                        End If
                    End If
                End If
            End If
        End If

    End Sub

    Private Sub cargarComboProcesos(cmbProcesos As Object)
        cmbProcesos.DataTextField = "PROCESO"
        cmbProcesos.DataValueField = "ID"
        Dim dt As DataTable = MaquinaBLL.ObtenerComboProcesos()
        Dim ddr As DataRow = dt.NewRow()
        cmbProcesos.DataSource = dt
        cmbProcesos.DataBind()

    End Sub

    Private Sub cargarComboPlantas(cmbPlantas As Object)
        cmbPlantas.DataTextField = "PLANTA"
        cmbPlantas.DataValueField = "ID"
        Dim dt As DataTable = MaquinaBLL.ObtenerComboPlantas()
        Dim ddr As DataRow = dt.NewRow()
        cmbPlantas.DataSource = dt
        cmbPlantas.DataBind()
    End Sub

End Class