Public Class TablasPorcentajes
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            pnlPorcentajes.Visible = True
            pnlNuevo.Visible = False
            cargarGrid()

        End If

    End Sub

    Private Sub cargarGrid()

        Dim datos As DataTable
        datos = PorcentajeBLL.Obtener()
        grdPorcentajes.DataSource = datos
        grdPorcentajes.DataBind()

    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click

        pnlNuevo.Visible = True
        txtDepartamento.Text = ""
        txtReubicados.Text = ""
        txtETT.Text = ""

    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click

        Dim dpto As Integer = txtDepartamento.Text
        'Dim reubicados As Decimal = txtReubicados.Text
        Dim reubicados = CSng(Convert.ToDouble(txtReubicados.Text))
        Dim ett = CSng(Convert.ToDouble(txtETT.Text))
        'Dim ett As Decimal = txtETT.Text.ToDecimal()
        Dim total As Decimal = reubicados + ett

        PorcentajeBLL.Nuevo(dpto, reubicados, ett, total)
        pnlPorcentajes.Controls.Add(New Label With {.Text = "Registro añadido", .CssClass = "alert alert-success"})
        pnlNuevo.Visible = False
        cargarGrid()

    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click

        pnlNuevo.Visible = True
        Response.Redirect("Porcentajes.aspx")

    End Sub

    Protected Sub grdPorcentajes_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdPorcentajes.RowEditing

        grdPorcentajes.EditIndex = e.NewEditIndex
        cargarGrid()

    End Sub

    Protected Sub grdPorcentajes_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdPorcentajes.RowUpdating

        btnNuevo.Visible = False
        pnlNuevo.Visible = False

        'Dim numActivo As String = Convert.ToString((CType(grdDiferenciaInventario.Rows(e.RowIndex).Cells(0).Controls(0), Label)).Text)
        Dim numActivo As String = (CType(grdPorcentajes.Rows(e.RowIndex).Cells(0).Controls(1), TextBox)).Text
        Dim criterioReparto As Integer = Convert.ToInt32(CType(grdPorcentajes.Rows(e.RowIndex).Cells(1).FindControl("txtCriterioReparto"), TextBox).Text)
        Dim planta As String = (CType(grdPorcentajes.Rows(e.RowIndex).Cells(2).FindControl("txtPlanta"), TextBox)).Text
        'If planta = Nothing Then
        '    planta = "null"
        'End If
        Dim proceso As Integer = Convert.ToInt32(CType(grdPorcentajes.Rows(e.RowIndex).Cells(3).FindControl("txtProceso"), TextBox).Text)
        'If proceso = Nothing Then
        '    proceso = "null"
        'End If

        AmortizacionDAL.Actualizar(numActivo, criterioReparto, planta, proceso)
        grdPorcentajes.EditIndex = -1
        grdPorcentajes.Controls.Add(New Label With {.Text = "Registro actualizado", .CssClass = "alert alert-info"})
        btnNuevo.Visible = True
        cargarGrid()

    End Sub


    Protected Sub grdPorcentajes_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdPorcentajes.RowDeleting

        Dim numActivo As String = (CType(grdPorcentajes.Rows(e.RowIndex).Cells(0).Controls(1), Label)).Text
        PorcentajeDAL.Eliminar(numActivo)
        grdPorcentajes.Controls.Add(New Label With {.Text = "Registro eliminado", .CssClass = "alert alert-danger"})
        cargarGrid()

    End Sub

    Protected Sub grdPorcentajes_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdPorcentajes.RowCancelingEdit

        grdPorcentajes.EditIndex = -1
        cargarGrid()

    End Sub

End Class