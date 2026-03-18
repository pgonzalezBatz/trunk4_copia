Public Class InventarioAjusteManual
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            pnlInventarioAjusteManual.Visible = True
            pnlNuevo.Visible = False
            cargarGrid()
            Master.Title("Inventario ajuste manual")
        End If

    End Sub

    Private Sub cargarGrid()
        Dim datos As DataTable
        datos = InventarioAjusteManualBLL.Obtener()
        grdInventarioAjusteManual.DataSource = datos
        grdInventarioAjusteManual.DataBind()

    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim fecha As DateTime = New DateTime(Today.Year, Today.Month, Today.Day)
        Dim fechaId As Integer = Convert.ToInt32(fecha.ToString("yyyyMMdd"))
        Dim referencia As String = txtReferencia.Text
        Dim unidadesAjuste As Integer = Convert.ToInt32(txtUnidadesAjuste.Text)
        InventarioAjusteManualBLL.Nuevo(fechaId, referencia, unidadesAjuste)
        pnlInventarioAjusteManual.Controls.Add(New Label With {.Text = "Inventario guardado", .CssClass = "alert alert-info"})
        pnlNuevo.Visible = False
        cargarGrid()

    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Response.Redirect("InventarioAjusteManual.aspx")

    End Sub

    Protected Sub grdInventarioAjusteManual_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdInventarioAjusteManual.RowEditing
        grdInventarioAjusteManual.EditIndex = e.NewEditIndex
        Dim idEditando = (CType(grdInventarioAjusteManual.Rows(e.NewEditIndex).Cells(0).Controls(1), Label)).Text
        cargarGrid()

    End Sub

    Protected Sub grdInventarioAjusteManual_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdInventarioAjusteManual.RowUpdating
        Dim fechaId As Integer = (CType(grdInventarioAjusteManual.Rows(e.RowIndex).Cells(0).Controls(1), TextBox)).Text
        Dim referencia As String = (CType(grdInventarioAjusteManual.Rows(e.RowIndex).Cells(1).Controls(1), TextBox)).Text
        Dim unidadesAjuste As Integer = (CType(grdInventarioAjusteManual.Rows(e.RowIndex).Cells(2).Controls(1), TextBox)).Text
        InventarioAjusteManualDAL.Actualizar(fechaId, referencia, unidadesAjuste)
        grdInventarioAjusteManual.EditIndex = -1
        pnlInventarioAjusteManual.Controls.Add(New Label With {.Text = "Registro actualizado", .CssClass = "alert alert-info"})
        cargarGrid()

    End Sub

    Protected Sub grdInventarioAjusteManual_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdInventarioAjusteManual.RowCancelingEdit
        grdInventarioAjusteManual.EditIndex = -1
        cargarGrid()

    End Sub

    Protected Sub grdInventarioAjusteManual_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdInventarioAjusteManual.RowDeleting
        Dim id As String = (CType(grdInventarioAjusteManual.Rows(e.RowIndex).Cells(0).Controls(1), Label)).Text
        InventarioAjusteManualDAL.Eliminar(id)
        cargarGrid()
        pnlInventarioAjusteManual.Controls.Add(New Label With {.Text = "Ajuste manual eliminado", .CssClass = "alert alert-danger"})
        cargarGrid()

    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        txtReferencia.Text = ""
        txtUnidadesAjuste.Text = ""
        pnlNuevo.Visible = True

    End Sub

    Protected Sub grdInventarioAjusteManual_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdInventarioAjusteManual.PageIndexChanging
        If grdInventarioAjusteManual.EditIndex <> -1 Then
            e.Cancel = True
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "info_msg", "alert('Cancele la edición antes de cambiar de página.');", True)
        Else
            Dim grdInventarioAjusteManual As GridView = CType(sender, GridView)
            grdInventarioAjusteManual.PageIndex = e.NewPageIndex
            cargarGrid()
        End If

    End Sub

    Protected Sub grdInventarioAjusteManual_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdInventarioAjusteManual.RowDataBound
        'Select Case e.Row.RowType
        '   Case DataControlRowType.DataRow

        '        Dim ctrlEliminar As ImageButton = CType(e.Row.Cells(3).Controls(0), ImageButton)
        '        ctrlEliminar.OnClientClick = "return confirm('¿Esta seguro?');"
        'End Select
    End Sub

    Protected Sub grdInventarioAjusteManual_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdInventarioAjusteManual.RowCommand
        'If e.CommandName = "Edit" Then
        '    Dim index As Integer = Convert.ToInt32(e.CommandArgument)
        '    Dim row As GridViewRow = grdInventarioAjusteManual.Rows(index)
        '    Dim item As ListItem = New ListItem()
        '    item.Text = Server.HtmlDecode(row.Cells(2).Text) & " " + Server.HtmlDecode(row.Cells(3).Text)

        '    'If Not ContactsListBox.Items.Contains(item) Then
        '    '    ContactsListBox.Items.Add(item)
        '    'End If
        'End If
    End Sub

End Class