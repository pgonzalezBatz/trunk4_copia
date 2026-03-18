Public Class Lantegis
    Inherits System.Web.UI.Page

    Public Property lantegiIdEditando As Integer
        Get
            If ViewState("lantegiIdEditando") Is Nothing Then ViewState("lantegiIdEditando") = ""
            Return ViewState("lantegiIdEditando").ToString()
        End Get

        Set(ByVal value As Integer)
            ViewState("lantegiIdEditando") = value
        End Set

    End Property

    Public Property lantegiEditando As String
        Get
            If ViewState("lantegiEditando") Is Nothing Then ViewState("lantegiEditando") = ""
            Return ViewState("lantegiEditando").ToString()
        End Get

        Set(ByVal value As String)
            ViewState("lantegiEditando") = value
        End Set

    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            pnlLantegi.Visible = True
            pnlNuevo.Visible = False
            btnNuevo.Visible = True
            cargarGrid()
            Master.Title("Lantegis")
        End If

    End Sub

    Private Sub cargarGrid()
        Dim datos As DataTable
        datos = LantegiBLL.Obtener()
        grdLantegi.DataSource = datos
        grdLantegi.DataBind()

    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        txtLantegiID.Text = ""
        txtLantegi.Text = ""
        txtGrupoProducto.Text = ""
        pnlNuevo.Visible = True

    End Sub


    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim lantegiID As Integer = txtLantegiID.Text
        Dim lantegi As String = txtLantegi.Text
        Dim grupoProducto As Integer = txtGrupoProducto.Text
        If LantegiBLL.Buscar(lantegiID, lantegi).Rows.Count = 0 Then
            LantegiDAL.Nuevo(lantegiID, lantegi, grupoProducto)
            pnlLantegi.Controls.Add(New Label With {.Text = "Lantegi añadido", .CssClass = "alert alert-success"})
            pnlNuevo.Visible = False
            cargarGrid()
        Else
            pnlLantegi.Controls.Add(New Label With {.Text = "El lantegi ya existe", .CssClass = "alert alert-info"})
            'pnlNuevo.Visible = False
        End If
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        pnlNuevo.Visible = False

    End Sub

    Protected Sub grdLantegi_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdLantegi.RowEditing
        grdLantegi.EditIndex = e.NewEditIndex
        lantegiIdEditando = (CType(grdLantegi.Rows(e.NewEditIndex).Cells(1).Controls(1), Label)).Text
        lantegiEditando = (CType(grdLantegi.Rows(e.NewEditIndex).Cells(2).Controls(1), Label)).Text
        cargarGrid()
    End Sub

    Protected Sub grdLantegi_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdLantegi.RowUpdating
        Dim lantegiID As Integer = CType((grdLantegi.Rows(e.RowIndex).Cells(1).Controls(1)), TextBox).Text
        Dim lantegi As String = CType(grdLantegi.Rows(e.RowIndex).Cells(2).Controls(1), DropDownList).SelectedValue
        Dim grupoProducto As String = CType((grdLantegi.Rows(e.RowIndex).Cells(3).Controls(1)), TextBox).Text
        Dim queryLantegi As String = "SELECT LANTEGI FROM D_BUSINESS WHERE LANTEGI_ID = '" + lantegi + "'"
        Dim nombreLantegi As String = Memcached.SQLServerDirectAccess.SeleccionarEscalar(Of String)(queryLantegi, Master.Cx)
        If LantegiBLL.Buscar(lantegiID, nombreLantegi).Rows.Count = 0 Then
            LantegiDAL.Actualizar(lantegiID, nombreLantegi, grupoProducto)
            grdLantegi.EditIndex = -1
            grdLantegi.Controls.Add(New Label With {.Text = "Lantegi actualizado", .CssClass = "alert alert-info"})
            btnNuevo.Visible = True
            cargarGrid()
        Else
            pnlLantegi.Controls.Add(New Label With {.Text = "El lantegi ya existe", .CssClass = "alert alert-info"})
            pnlNuevo.Visible = False
        End If

    End Sub

    Protected Sub grdLantegi_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdLantegi.RowDeleting
        Dim lantegiID As String = (CType(grdLantegi.Rows(e.RowIndex).Cells(1).Controls(1), Label)).Text
        LantegiDAL.Eliminar(lantegiID)
        pnlLantegi.Controls.Add(New Label With {.Text = "Lantegi eliminado", .CssClass = "alert alert-danger"})
        cargarGrid()
    End Sub

    Protected Sub grdLantegi_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdLantegi.RowCancelingEdit
        grdLantegi.EditIndex = -1
        cargarGrid()

    End Sub

    Protected Sub grdLantegi_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdLantegi.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.Cells(3).Controls.Count > 0 Then
                If True Then
                    Dim crtl As Control = e.Row.Cells(2).Controls(1)
                    If TypeOf crtl Is DropDownList Then
                        Dim cmbLantegis As DropDownList = CType(e.Row.Cells(2).Controls(1), DropDownList)
                        If Not lantegiEditando.Equals("") Then
                            cargarComboLantegis(cmbLantegis)
                            cmbLantegis.SelectedValue = lantegiIdEditando.ToString()
                        Else
                            cargarComboLantegis(cmbLantegis)
                        End If
                    End If
                End If
            End If
        End If

    End Sub

    Protected Sub grdLantegi_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdLantegi.PageIndexChanging
        If grdLantegi.EditIndex <> -1 Then
            e.Cancel = True
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "info_msg", "alert('Cancele la edición antes de cambiar de página.');", True)
        Else
            Dim grdLantegi As GridView = CType(sender, GridView)
            grdLantegi.PageIndex = e.NewPageIndex
            cargarGrid()
        End If

    End Sub

    Private Sub cargarComboLantegis(cmbLantegis As Object)
        cmbLantegis.DataTextField = "LANTEGI"
        cmbLantegis.DataValueField = "LANTEGI_ID"
        Dim dt As DataTable = LantegiBLL.ObtenerComboLantegis()
        Dim ddr As DataRow = dt.NewRow()
        cmbLantegis.DataSource = dt
        cmbLantegis.DataBind()
    End Sub

End Class