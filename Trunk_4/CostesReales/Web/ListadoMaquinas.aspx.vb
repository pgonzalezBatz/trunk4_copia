Public Class ListadoMaquinas
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        cargarGrid()
        Master.Title("Listado de máquinas")
    End Sub

    Private Sub cargarGrid()
        Dim datos As DataTable
        datos = ListadoMaquinasBLL.Obtener()
        grdListadoMaquinas.DataSource = datos
        grdListadoMaquinas.DataBind()

    End Sub

    Protected Sub grdInventarioAjusteManual_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdListadoMaquinas.PageIndexChanging
        If grdListadoMaquinas.EditIndex <> -1 Then
            e.Cancel = True
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "info_msg", "alert('Cancele la edición antes de cambiar de página.');", True)
        Else
            Dim grdListadoMaquinas As GridView = CType(sender, GridView)
            grdListadoMaquinas.PageIndex = e.NewPageIndex
            cargarGrid()
        End If

    End Sub

End Class