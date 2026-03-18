Public Class CCGastosMOD
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

    Public Property gastoMODEditando As String
        Get
            If ViewState("gastoMODEditando") Is Nothing Then ViewState("gastoMODEditando") = ""
            Return ViewState("gastoMODEditando").ToString()
        End Get

        Set(ByVal value As String)
            ViewState("gastoMODEditando") = value
        End Set

    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            pnlCCGastoMOD.Visible = True
            pnlNuevo.Visible = False
            btnNuevo.Visible = True
            cargarGrid()
        End If

    End Sub

    Private Sub cargarGrid()
        Dim datos As DataTable
        datos = CCGastoMODBLL.Obtener()
        grdGastosMOD.DataSource = datos
        grdGastosMOD.DataBind()

    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        txtCC.Text = ""
        ddlGastoMOD.Items.Clear()
        ddlGastoMOD.Items.Add("Seleccione gasto MOD...")
        For Each item As String() In Utilidades.ObtenerQuerySQLSERVERParametros("SELECT Gasto_MOD FROM M_Gastos_MOD ORDER BY Gasto_MOD ASC")
            ddlGastoMOD.Items.Add(item(0))
        Next
        pnlNuevo.Visible = True

    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim cc As Integer = txtCC.Text
        Dim gastoMOD As String = ddlGastoMOD.SelectedValue()
        CCGastoMODBLL.Nuevo(cc, gastoMOD)
        pnlCCGastoMOD.Controls.Add(New Label With {.Text = "CC Por Gasto añadida", .CssClass = "alert alert-success"})
        pnlNuevo.Visible = False
        cargarGrid()

    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        pnlCCGastoMOD.Visible = True
        pnlNuevo.Visible = False
        Response.Redirect("CCGastosMOD.aspx")

    End Sub

    Protected Sub grdGastosMOD_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdGastosMOD.RowEditing
        grdGastosMOD.EditIndex = e.NewEditIndex
        ccEditando = (CType(grdGastosMOD.Rows(e.NewEditIndex).Cells(1).Controls(1), Label)).Text
        gastoMODEditando = (CType(grdGastosMOD.Rows(e.NewEditIndex).Cells(2).Controls(1), Label)).Text
        cargarGrid()

    End Sub

    Protected Sub grdGastosMOD_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdGastosMOD.RowUpdating
        btnNuevo.Visible = False
        pnlNuevo.Visible = False
        Dim cc As Integer = Convert.ToInt32((CType(grdGastosMOD.Rows(e.RowIndex).Cells(1).Controls(1), TextBox)).Text)
        Dim GastoMOD As String = CType(grdGastosMOD.Rows(e.RowIndex).Cells(2).Controls(1), DropDownList).SelectedValue
        'Dim GastoMODID As String = CType(grdGastosMOD.Rows(e.RowIndex).Cells(3).Controls(1), TextBox).Text
        'CCGastoMODDAL.Actualizar(cc, GastoMOD, GastoMODID)
        CCGastoMODDAL.Actualizar(cc, GastoMOD)
        grdGastosMOD.EditIndex = -1
        pnlCCGastoMOD.Controls.Add(New Label With {.Text = "Registro actualizado", .CssClass = "alert alert-info"})
        btnNuevo.Visible = True
        cargarGrid()

    End Sub

    Protected Sub grdGastosMOD_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdGastosMOD.RowDeleting
        Dim cc As Integer = Convert.ToInt32((CType(grdGastosMOD.Rows(e.RowIndex).Cells(1).Controls(1), Label)).Text)
        CCGastoMODDAL.Eliminar(cc)
        pnlCCGastoMOD.Controls.Add(New Label With {.Text = "Cuenta Contable eliminada", .CssClass = "alert alert-danger"})
        cargarGrid()

    End Sub

    Protected Sub grdGastosMOD_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdGastosMOD.RowCancelingEdit
        grdGastosMOD.EditIndex = -1
        cargarGrid()

    End Sub

    Protected Sub grdGastosMOD_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdGastosMOD.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.Cells(3).Controls.Count > 0 Then
                If True Then
                    Dim crtl As Control = e.Row.Cells(2).Controls(1)
                    If TypeOf crtl Is DropDownList Then
                        Dim cmbGastosMOD As DropDownList = CType(e.Row.Cells(2).Controls(1), DropDownList)
                        If Not ccEditando.Equals("") Then
                            cargarComboGastosMOD(cmbGastosMOD)
                            cmbGastosMOD.SelectedValue = gastoMODEditando
                        Else
                            cargarComboGastosMOD(cmbGastosMOD)
                        End If
                    End If
                End If
            End If
        End If

    End Sub

    Protected Sub grdGastosMOD_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdGastosMOD.PageIndexChanging
        If grdGastosMOD.EditIndex <> -1 Then
            e.Cancel = True
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "info_msg", "alert('Cancele la edición antes de cambiar de página.');", True)
        Else
            Dim grdGastosMOD As GridView = CType(sender, GridView)
            grdGastosMOD.PageIndex = e.NewPageIndex
            cargarGrid()
        End If

    End Sub

    Private Sub cargarComboGastosMOD(cmbGastosMOD As DropDownList)
        cmbGastosMOD.DataTextField = "Gasto_MOI"
        cmbGastosMOD.DataValueField = "ID"
        Dim dt As DataTable = CCGastoMODBLL.ObtenerComboGastosMOD()
        Dim ddr As DataRow = dt.NewRow()
        cmbGastosMOD.DataSource = dt
        cmbGastosMOD.DataBind()

    End Sub

End Class