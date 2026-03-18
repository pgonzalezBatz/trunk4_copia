Public Class CCPorNegocio
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            pnlCCPorNegocio.Visible = True
            btnNuevo.Visible = True
            cargarGrid()
            Master.Title("Cuentas de Venta e Inventario PT y PC")
        End If

    End Sub

    Private Sub cargarGrid()
        Dim datos As DataTable
        datos = CCPorNegocioBLL.ObtenerLantegi()
        grdCCPorNegocio.DataSource = datos
        grdCCPorNegocio.DataBind()
        cargarComboLantegis(ddlLantegiForm)

    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim cc As String = txtCCForm.Text
        Dim lantegiSeleccionadoId As String = ddlLantegiForm.SelectedValue()
        Dim aplica_ventas As Boolean = chkAplicaVentas.Checked
        Dim query As String = "INSERT INTO T_CtaContable_Negocio (CC, Lantegi_id, Aplica_Ventas) VALUES ('" + cc.ToString() + "','" + lantegiSeleccionadoId.ToString() + "', '" + aplica_ventas.ToString() + "')"
        Memcached.SQLServerDirectAccess.NoQuery(query, Master.Cx)
        pnlCCPorNegocio.Controls.Add(New Label With {.Text = "CC Por Negocio añadida", .CssClass = "alert alert-success"})
        cargarGrid()

    End Sub

    Protected Sub grdCCPorNegocio_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdCCPorNegocio.RowEditing
        grdCCPorNegocio.EditIndex = e.NewEditIndex
        ccEditando = Convert.ToInt32((CType(grdCCPorNegocio.Rows(e.NewEditIndex).Cells(2).Controls(1), Label)).Text)
        cargarGrid()

    End Sub

    Protected Sub grdCCPorNegocio_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdCCPorNegocio.RowUpdating
        btnNuevo.Visible = False
        Dim cc As Integer = Convert.ToInt32((CType(grdCCPorNegocio.Rows(e.RowIndex).Cells(1).Controls(1), TextBox)).Text)
        Dim aplica_ventas As Boolean = Convert.ToBoolean((CType(grdCCPorNegocio.Rows(e.RowIndex).Cells(3).Controls(0), CheckBox)).Checked)
        Dim lantegi As String = Convert.ToInt32((CType(grdCCPorNegocio.Rows(e.RowIndex).Cells(4).Controls(1), DropDownList)).SelectedValue)
        CCPorNegocioDAL.Actualizar(cc, lantegi, aplica_ventas, ccEditando)
        grdCCPorNegocio.EditIndex = -1
        pnlCCPorNegocio.Controls.Add(New Label With {.Text = "Registro actualizado", .CssClass = "alert alert-info"})
        btnNuevo.Visible = True
        cargarGrid()

    End Sub

    Protected Sub grdCCPorNegocio_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdCCPorNegocio.RowDeleting
        Dim cc As Integer = (CType(grdCCPorNegocio.Rows(e.RowIndex).Cells(1).Controls(1), Label)).Text
        CCPorNegocioDAL.Eliminar(cc)
        pnlCCPorNegocio.Controls.Add(New Label With {.Text = "Cuenta Contable eliminada", .CssClass = "alert alert-danger"})
        cargarGrid()

    End Sub

    Protected Sub grdCCPorNegocio_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdCCPorNegocio.RowCancelingEdit
        grdCCPorNegocio.EditIndex = -1
        cargarGrid()

    End Sub

    Protected Sub grdCCPorNegocio_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdCCPorNegocio.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.Cells(4).Controls.Count > 0 Then
                If True Then
                    Dim crtl As Control = e.Row.Cells(4).Controls(1)
                    If TypeOf crtl Is DropDownList Then
                        Dim cmbLantegis As DropDownList = CType(e.Row.Cells(4).Controls(1), DropDownList)
                        If Not ccEditando.Equals("") Then
                            cargarComboLantegis(cmbLantegis)
                            cmbLantegis.SelectedValue = ccEditando.ToString()
                        Else
                            cargarComboLantegis(cmbLantegis)
                        End If
                    End If
                End If
            End If
        End If

    End Sub

    Protected Sub grdCCPorNegocio_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdCCPorNegocio.PageIndexChanging
        If grdCCPorNegocio.EditIndex <> -1 Then
            e.Cancel = True
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "info_msg", "alert('Cancele la edición antes de cambiar de página.');", True)
        Else
            Dim grdCCPorNegocio As GridView = CType(sender, GridView)
            grdCCPorNegocio.PageIndex = e.NewPageIndex
            cargarGrid()
        End If

    End Sub

    Private Sub cargarComboLantegis(cmbLantegis As DropDownList)
        cmbLantegis.DataTextField = "LANTEGI"
        cmbLantegis.DataValueField = "LANTEGI_ID"
        Dim dt As DataTable = CCPorNegocioBLL.ObtenerComboLantegis()
        Dim ddr As DataRow = dt.NewRow()
        cmbLantegis.DataSource = dt
        cmbLantegis.DataBind()

    End Sub

End Class