Public Class RPGenericos
    Inherits System.Web.UI.Page

    Public Property rpEditando As String
        Get
            If ViewState("rpEditando") Is Nothing Then ViewState("rpEditando") = ""
            Return ViewState("rpEditando").ToString()
        End Get

        Set(ByVal value As String)
            ViewState("rpEditando") = value
        End Set

    End Property

    Public Property lantegiEditando As Integer
        Get
            If ViewState("lantegiEditando") Is Nothing Then ViewState("lantegiEditando") = ""
            Return ViewState("lantegiEditando").ToString()
        End Get

        Set(ByVal value As Integer)
            ViewState("lantegiEditando") = value
        End Set

    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            pnlRPGenerico.Visible = True
            pnlNuevo.Visible = False
            btnNuevo.Visible = True
            cargarGrid()
            Master.Title("RP Genéricos")
        End If

    End Sub

    Private Sub cargarGrid()
        Dim datos As DataTable
        datos = RPGenericoBLL.ObtenerLantegi()
        grdRPGenericos.DataSource = datos
        grdRPGenericos.DataBind()

    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        txtRP.Text = ""
        ddlLantegis.ClearSelection()
        cargarLantegis()
        pnlNuevo.Visible = True
        btnCancelar.Visible = True

    End Sub

    Private Sub cargarLantegis()
        ddlLantegis.Items.Clear()
        ddlLantegis.Items.Add("Seleccione lantegi...")
        For Each item As String() In Utilidades.ObtenerQuerySQLSERVERParametros("SELECT LANTEGI FROM D_Business ORDER BY LANTEGI ASC")
            ddlLantegis.Items.Add(item(0))
        Next

    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim rp As String = txtRP.Text
        Dim lantegi As String = ddlLantegis.SelectedValue()
        Dim query As String = "SELECT lantegi_id FROM D_Business WHERE LANTEGI LIKE '%" + lantegi + "%'"
        Dim lantegiId As Integer = Memcached.SQLServerDirectAccess.SeleccionarEscalar(Of Integer)(query, Master.Cx)
        RPGenericoBLL.Nuevo(rp, lantegiId)
        pnlRPGenerico.Controls.Add(New Label With {.Text = "RP añadido", .CssClass = "alert alert-success"})
        pnlNuevo.Visible = False
        cargarGrid()

    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Response.Redirect("RPGenericos.aspx")

    End Sub

    Protected Sub grdRPGenericos_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdRPGenericos.RowEditing
        grdRPGenericos.EditIndex = e.NewEditIndex
        rpEditando = (CType(grdRPGenericos.Rows(e.NewEditIndex).Cells(1).Controls(1), Label)).Text
        lantegiEditando = (CType(grdRPGenericos.Rows(e.NewEditIndex).Cells(2).Controls(1), Label)).Text
        cargarGrid()

    End Sub

    Protected Sub grdRPGenericos_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdRPGenericos.RowUpdating
        Dim rp As String = (CType(grdRPGenericos.Rows(e.RowIndex).Cells(1).Controls(1), TextBox)).Text.Trim()
        'Dim lantegiId As Integer = Convert.ToInt32(CType(grdRPGenericos.Rows(e.RowIndex).Cells(2).FindControl("txtLantegiId"), TextBox).Text)
        Dim lantegi As Integer = Convert.ToInt32((CType(grdRPGenericos.Rows(e.RowIndex).Cells(3).Controls(1), DropDownList)).SelectedValue)
        RPGenericoDAL.Actualizar(rp, lantegi, lantegiEditando)
        grdRPGenericos.EditIndex = -1
        pnlRPGenerico.Controls.Add(New Label With {.Text = "RP actualizado", .CssClass = "alert alert-success"})
        cargarGrid()

    End Sub

    Protected Sub grdRPGenericos_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdRPGenericos.RowDeleting
        Dim rpId As String = (CType(grdRPGenericos.Rows(e.RowIndex).Cells(1).Controls(1), Label)).Text
        Dim lantegiId As Integer = (CType(grdRPGenericos.Rows(e.RowIndex).Cells(2).Controls(1), Label)).Text
        RPGenericoDAL.Eliminar(rpId, lantegiId)
        pnlRPGenerico.Controls.Add(New Label With {.Text = "RP eliminado", .CssClass = "alert alert-danger"})
        cargarGrid()

    End Sub

    Protected Sub grdRPGenericos_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdRPGenericos.RowCancelingEdit
        grdRPGenericos.EditIndex = -1
        cargarGrid()

    End Sub

    Protected Sub grdRPGenericos_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdRPGenericos.PageIndexChanging
        If grdRPGenericos.EditIndex <> -1 Then
            e.Cancel = True
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "info_msg", "alert('Cancele la edición antes de cambiar de página.');", True)
        Else
            Dim grdRPGenericos As GridView = CType(sender, GridView)
            grdRPGenericos.PageIndex = e.NewPageIndex
            cargarGrid()
        End If

    End Sub

    Protected Sub grdRPGenericos_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdRPGenericos.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.Cells(2).Controls.Count > 0 Then
                If True Then
                    Dim crtl As Control = e.Row.Cells(3).Controls(1)
                    If TypeOf crtl Is DropDownList Then
                        Dim cmbLantegis As DropDownList = CType(e.Row.Cells(3).Controls(1), DropDownList)
                        If Not rpEditando.Equals("") Then
                            cargarComboLantegis(cmbLantegis)
                            cmbLantegis.SelectedValue = lantegiEditando
                        Else
                            cargarComboLantegis(cmbLantegis)
                        End If
                    End If
                End If
            End If
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