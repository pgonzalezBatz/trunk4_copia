Public Class CCGastosMOI
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

    Public Property gastoMOIEditando As String
        Get
            If ViewState("gastoMOIEditando") Is Nothing Then ViewState("gastoMOIEditando") = ""
            Return ViewState("gastoMOIEditando").ToString()
        End Get

        Set(ByVal value As String)
            ViewState("gastoMOIEditando") = value
        End Set

    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            pnlCCGastoMOI.Visible = True
            pnlNuevo.Visible = False
            btnNuevo.Visible = True
            cargarGrid()
            Master.Title("Cuentas de gastos MOI")
        End If

    End Sub

    Private Sub cargarGrid()
        Dim datos As DataTable
        datos = CCGastoMOIBLL.Obtener()
        grdGastosMOI.DataSource = datos
        grdGastosMOI.DataBind()

    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        txtCC.Text = ""
        ddlGastoMOI.Items.Clear()
        ddlGastoMOI.Items.Add("Seleccione gasto MOI...")
        For Each item As String() In Utilidades.ObtenerQuerySQLSERVERParametros("SELECT Gasto_MOI FROM M_Gastos_MOI ORDER BY Gasto_MOI ASC")
            ddlGastoMOI.Items.Add(item(0))
        Next
        pnlNuevo.Visible = True

    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim cc As Integer = txtCC.Text
        Dim gastoMOI As String = ddlGastoMOI.SelectedValue()
        CCGastoMOIBLL.Nuevo(cc, gastoMOI)
        pnlCCGastoMOI.Controls.Add(New Label With {.Text = "CC Por Gasto añadida", .CssClass = "alert alert-success"})
        pnlNuevo.Visible = False
        cargarGrid()

    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        pnlCCGastoMOI.Visible = True
        pnlNuevo.Visible = False
        Response.Redirect("CCGastosMOI.aspx")

    End Sub

    Protected Sub grdGastosMOI_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdGastosMOI.RowEditing
        grdGastosMOI.EditIndex = e.NewEditIndex
        ccEditando = (CType(grdGastosMOI.Rows(e.NewEditIndex).Cells(1).Controls(1), Label)).Text
        gastoMOIEditando = (CType(grdGastosMOI.Rows(e.NewEditIndex).Cells(2).Controls(1), Label)).Text
        cargarGrid()

    End Sub

    Protected Sub grdGastosMOI_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdGastosMOI.RowUpdating
        btnNuevo.Visible = False
        pnlNuevo.Visible = False
        Dim cc As Integer = Convert.ToInt32((CType(grdGastosMOI.Rows(e.RowIndex).Cells(1).Controls(1), TextBox)).Text)
        Dim gastoMOI As String = CType(grdGastosMOI.Rows(e.RowIndex).Cells(2).Controls(1), DropDownList).SelectedValue
        'Dim gastoMOIID As String = CType(grdGastosMOI.Rows(e.RowIndex).Cells(3).Controls(1), TextBox).Text
        'CCGastoMOIDAL.Actualizar(cc, gastoMOI, gastoMOIID)
        CCGastoMOIDAL.Actualizar(cc, gastoMOI)
        grdGastosMOI.EditIndex = -1
        pnlCCGastoMOI.Controls.Add(New Label With {.Text = "Registro actualizado", .CssClass = "alert alert-info"})
        btnNuevo.Visible = True
        cargarGrid()

    End Sub

    Protected Sub grdGastosMOI_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdGastosMOI.RowDeleting
        Dim cc As Integer = Convert.ToInt32((CType(grdGastosMOI.Rows(e.RowIndex).Cells(1).Controls(1), Label)).Text)
        CCGastoMOIDAL.Eliminar(cc)
        pnlCCGastoMOI.Controls.Add(New Label With {.Text = "Cuenta Contable eliminada", .CssClass = "alert alert-danger"})
        cargarGrid()

    End Sub

    Protected Sub grdGastosMOI_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdGastosMOI.RowCancelingEdit
        grdGastosMOI.EditIndex = -1
        cargarGrid()

    End Sub

    Protected Sub grdGastosMOI_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdGastosMOI.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.Cells(3).Controls.Count > 0 Then
                If True Then
                    Dim crtl As Control = e.Row.Cells(2).Controls(1)
                    If TypeOf crtl Is DropDownList Then
                        Dim cmbGastosMOI As DropDownList = CType(e.Row.Cells(2).Controls(1), DropDownList)
                        If Not ccEditando.Equals("") Then
                            cargarComboGastosMOI(cmbGastosMOI)
                            cmbGastosMOI.SelectedValue = gastoMOIEditando
                        Else
                            cargarComboGastosMOI(cmbGastosMOI)
                        End If
                    End If
                End If
            End If
        End If

    End Sub

    Protected Sub grdGastosMOI_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdGastosMOI.PageIndexChanging
        If grdGastosMOI.EditIndex <> -1 Then
            e.Cancel = True
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "info_msg", "alert('Cancele la edición antes de cambiar de página.');", True)
        Else
            Dim grdGastosMOI As GridView = CType(sender, GridView)
            grdGastosMOI.PageIndex = e.NewPageIndex
            cargarGrid()
        End If

    End Sub

    Private Sub cargarComboGastosMOI(cmbGastosMOI As DropDownList)
        cmbGastosMOI.DataTextField = "Gasto_MOI"
        cmbGastosMOI.DataValueField = "ID"
        Dim dt As DataTable = CCGastoMOIBLL.ObtenerComboGastosMOI()
        Dim ddr As DataRow = dt.NewRow()
        cmbGastosMOI.DataSource = dt
        cmbGastosMOI.DataBind()

    End Sub

End Class