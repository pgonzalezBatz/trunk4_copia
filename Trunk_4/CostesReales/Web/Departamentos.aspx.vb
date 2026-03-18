Public Class Departamentos
    Inherits System.Web.UI.Page

    Public Property departamentoEditando As String
        Get
            If ViewState("departamentoEditando") Is Nothing Then ViewState("departamentoEditando") = ""
            Return ViewState("departamentoEditando").ToString()
        End Get

        Set(ByVal value As String)
            ViewState("departamentoEditando") = value
        End Set

    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            pnlDepartamentos.Visible = True
            pnlNuevo.Visible = False
            btnNuevo.Visible = True
            cargarGrid()
        End If

    End Sub

    Private Sub cargarGrid()
        Dim datos As DataTable
        datos = DepartamentoBLL.Obtener()
        grdDepartamentos.DataSource = datos
        grdDepartamentos.DataBind()

    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        txtDepartamento.Text = ""
        pnlNuevo.Visible = True

    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim departamento As Integer = txtDepartamento.Text
        Dim QueryMaxId As String = "SELECT MAX(Id) FROM T_Dpto"
        Dim id As Integer = Memcached.SQLServerDirectAccess.SeleccionarEscalar(Of Integer)(QueryMaxId, ConfigurationManager.ConnectionStrings("BI_CR_Igorre").ConnectionString)
        DepartamentoDAL.Nuevo(departamento, id + 1)
        pnlDepartamentos.Controls.Add(New Label With {.Text = "Departamento añadido", .CssClass = "alert alert-success"})
        pnlNuevo.Visible = False
        cargarGrid()

    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        pnlNuevo.Visible = False

    End Sub

    Protected Sub grdDepartamentos_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdDepartamentos.RowEditing
        grdDepartamentos.EditIndex = e.NewEditIndex
        departamentoEditando = (CType(grdDepartamentos.Rows(e.NewEditIndex).Cells(1).Controls(1), Label)).Text
        cargarGrid()

    End Sub

    Protected Sub grdDepartamentos_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdDepartamentos.RowUpdating
        btnNuevo.Visible = False
        pnlNuevo.Visible = False
        Dim departamento As String = CType(grdDepartamentos.Rows(e.RowIndex).Cells(2).Controls(1), TextBox).Text
        Dim id As String = "SELECT ID from T_Dpto WHERE DPTO like '%" + departamento + "%'"
        Dim departamentoId As Integer = Memcached.SQLServerDirectAccess.SeleccionarEscalar(Of Integer)(id, ConfigurationManager.ConnectionStrings("BI_CR_Igorre").ConnectionString)
        DepartamentoDAL.Actualizar(departamentoId, departamento)
        grdDepartamentos.EditIndex = -1
        pnlDepartamentos.Controls.Add(New Label With {.Text = "Departamento actualizado", .CssClass = "alert alert-info"})
        btnNuevo.Visible = True
        cargarGrid()

    End Sub

    Protected Sub grdDepartamentos_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdDepartamentos.RowDeleting
        Dim id As String = (CType(grdDepartamentos.Rows(e.RowIndex).Cells(1).Controls(1), Label)).Text
        DepartamentoDAL.Eliminar(id)
        pnlDepartamentos.Controls.Add(New Label With {.Text = "Departamento eliminado", .CssClass = "alert alert-danger"})
        cargarGrid()

    End Sub

    Protected Sub grdDepartamentos_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdDepartamentos.RowCancelingEdit
        grdDepartamentos.EditIndex = -1
        cargarGrid()

    End Sub

    Protected Sub grdDepartamentos_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdDepartamentos.PageIndexChanging
        If grdDepartamentos.EditIndex <> -1 Then
            e.Cancel = True
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "info_msg", "alert('Cancele la edición antes de cambiar de página.');", True)
        Else
            Dim grdCCPorNegocio As GridView = CType(sender, GridView)
            grdCCPorNegocio.PageIndex = e.NewPageIndex
            cargarGrid()
        End If

    End Sub

    Protected Sub btnETTReubicados_Click(sender As Object, e As EventArgs) Handles btnETTReubicados.Click
        Response.Redirect("PorcentajeETTReubicados.aspx")

    End Sub

    Protected Sub btnDepartamento_Click(sender As Object, e As EventArgs) Handles btnDepartamento.Click
        Response.Redirect("PorcentajeDepartamento.aspx")

    End Sub

End Class