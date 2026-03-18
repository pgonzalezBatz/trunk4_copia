Public Class PorcentajeDepartamento
    Inherits System.Web.UI.Page

    Public Property idProcesoEditando As Integer
        Get
            If ViewState("idProcesoEditando") Is Nothing Then ViewState("idProcesoEditando") = -1
            Return CType(ViewState("idProcesoEditando"), Integer)
        End Get

        Set(ByVal value As Integer)
            ViewState("idProcesoEditando") = value
        End Set

    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            pnlPorcentajeDepartamento.Visible = True
            'pnlNuevo.Visible = False
            cargarGrid()
            Master.Title("Porcentajes por departamento")
        End If

    End Sub

    Private Sub cargarGrid()
        Dim dtDepartamentos As DataTable = PorcentajeDepartamentoBLL.Obtener()
        Dim dtProcesos As DataTable = ProcesoBLL.Obtener()
        Dim dtPorcentajes As DataTable = PorcentajePorProcesoDepartamentoBLL.Obtener()
        Dim datosMostrar As New DataTable
        datosMostrar.Columns.Add(New DataColumn("PROCESO_ID"))
        datosMostrar.Columns.Add(New DataColumn("PROCESO"))
        For Each item As DataRow In dtDepartamentos.Rows
            datosMostrar.Columns.Add(New DataColumn(item(1)))
        Next
        For i = 0 To dtProcesos.Rows.Count - 1 Step 1
            Dim currentI = i
            Dim dr As DataRow = datosMostrar.NewRow()
            dr(0) = dtProcesos.Rows(i)(0)
            dr(1) = dtProcesos.Rows(i)(1)
            For j = 0 To dtDepartamentos.Rows.Count - 1 Step 1
                Dim currentJ = j
                Dim query = From v In dtPorcentajes.AsEnumerable()
                            Where v.Field(Of Integer)("ID_PROCESO") = dtProcesos.Rows(currentI)(0) And v.Field(Of Integer)("ID_DPTO") = dtDepartamentos.Rows(currentJ)(0)
                            Select v
                If (query.Count > 0) Then
                    Dim porc As Decimal = query.FirstOrDefault()(2)
                    dr(j + 2) = porc
                End If
            Next
            datosMostrar.Rows.Add(dr)
        Next

        Dim drTotal As DataRow = datosMostrar.NewRow()
        For i As Integer = 2 To datosMostrar.Columns.Count - 1
            Dim sum As Decimal = 0.0
            Dim pcr As String = ""

            For j As Integer = 0 To datosMostrar.Rows.Count - 1
                If Not String.IsNullOrEmpty(datosMostrar.Rows(j)(i).ToString()) Then
                    sum += CType(datosMostrar.Rows(j)(i), Decimal)
                    pcr = sum.ToString("0.00") + " %"
                End If
            Next
            drTotal(i) = sum
        Next
        datosMostrar.Rows.Add(drTotal)
        grdPorcentajeDepartamento.DataSource = datosMostrar
        grdPorcentajeDepartamento.DataBind()
        If grdPorcentajeDepartamento.Rows.Count > 0 Then
            grdPorcentajeDepartamento.Rows(grdPorcentajeDepartamento.Rows.Count - 1).Cells(0).Controls(0).Visible = False
            grdPorcentajeDepartamento.Rows(grdPorcentajeDepartamento.Rows.Count - 1).Cells(1).Controls(1).Visible = False
        End If

    End Sub

    Protected Sub grdPorcentajeDepartamento_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdPorcentajeDepartamento.RowEditing
        grdPorcentajeDepartamento.EditIndex = e.NewEditIndex
        idProcesoEditando = grdPorcentajeDepartamento.Rows(e.NewEditIndex).Cells(2).Text
        cargarGrid()

    End Sub


    Protected Sub grdPorcentajeDepartamento_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdPorcentajeDepartamento.RowUpdating
        Dim dtDepartamentos As DataTable = PorcentajeDepartamentoBLL.Obtener()
        Dim prc As String
        Dim erro As Boolean = False
        Dim prcTotal As Decimal = 0
        For c = 4 To (dtDepartamentos.Rows.Count + 4) - 1 Step 1
            For r = 0 To grdPorcentajeDepartamento.Rows.Count - 2 Step 1
                If e.RowIndex = r Then
                    prc = CType(grdPorcentajeDepartamento.Rows(r).Cells(c).Controls(0), TextBox).Text
                Else
                    prc = grdPorcentajeDepartamento.Rows(r).Cells(c).Text
                End If
                If Not prc.Equals("") Then
                    Try
                        prcTotal += CType(prc, Decimal)
                    Catch

                    End Try
                End If
            Next
            If prcTotal > 100 Then
                erro = True
            End If
            prcTotal = 0
        Next
        If erro Then
            Dim myScript As String = "window.alert('El porcentaje supera el 100%');"
            ClientScript.RegisterStartupScript(Me.GetType(), "myScript", myScript, True)
        Else
            PorcentajePorProcesoDepartamentoBLL.Eliminar(idProcesoEditando)
            For i = 4 To grdPorcentajeDepartamento.Rows(0).Cells.Count - 1 Step 1
                prc = CType(grdPorcentajeDepartamento.Rows(e.RowIndex).Cells(i).Controls(0), TextBox).Text
                If Not prc.Equals("") Then
                    PorcentajePorProcesoDepartamentoBLL.Nuevo(idProcesoEditando, CType(dtDepartamentos.Rows(i - 4)(0), Integer), prc)
                End If

            Next
            grdPorcentajeDepartamento.EditIndex = -1
            grdPorcentajeDepartamento.Controls.Add(New Label With {.Text = "Porcentaje actualizado", .CssClass = "alert alert-info"})
            cargarGrid()
        End If

    End Sub

    Protected Sub grdPorcentajeDepartamento_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdPorcentajeDepartamento.RowDeleting
        Dim maquina As String = (CType(grdPorcentajeDepartamento.Rows(e.RowIndex).Cells(1).Controls(1), Label)).Text
        PorcentajeDepartamentoDAL.Eliminar(maquina)
        pnlPorcentajeDepartamento.Controls.Add(New Label With {.Text = "Porcentaje eliminado", .CssClass = "alert alert-danger"})
        cargarGrid()

    End Sub

    Protected Sub grdPorcentajeDepartamento_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdPorcentajeDepartamento.RowCancelingEdit
        grdPorcentajeDepartamento.EditIndex = -1
        cargarGrid()

    End Sub

    Protected Sub grdPorcentajeDepartamento_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdPorcentajeDepartamento.PageIndexChanging
        If grdPorcentajeDepartamento.EditIndex <> -1 Then
            e.Cancel = True
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "info_msg", "alert('Cancele la edición antes de cambiar de página.');", True)
        Else
            Dim grdMaquinaClasificada As GridView = CType(sender, GridView)
            grdMaquinaClasificada.PageIndex = e.NewPageIndex
            cargarGrid()
        End If

    End Sub

    Protected Sub grdPorcentajeDepartamento_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdPorcentajeDepartamento.RowDataBound
        If grdPorcentajeDepartamento.EditIndex = -1 Then
            If e.Row.RowType = DataControlRowType.DataRow Then
                For j As Integer = 4 To e.Row.Cells.Count - 1
                    Dim encoded As String = e.Row.Cells(j).Text
                    If encoded <> "&nbsp;" Then
                        e.Row.Cells(j).Text = e.Row.Cells(j).Text + "%"
                        If e.Row.Cells(j).Text = "0%" Then e.Row.Cells(j).Text = ""
                    End If
                Next
            End If
        End If
    End Sub

    'Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
    '    Dim proceso As String = ddlProcesos.SelectedValue
    '    Dim departamento As String = ddlDepartamentos.SelectedValue
    '    Dim porcentaje As String = txtPorcentaje.Text
    '    Dim procesoSeleccionadoId As String = "SELECT ID from T_PROCESOS WHERE PROCESO like '%" + proceso + "%'"
    '    Dim departamentoSeleccionadoId As String = ddlProcesos.SelectedValue
    '    Dim proceso_id As Integer = Memcached.SQLServerDirectAccess.SeleccionarEscalar(Of Integer)(procesoSeleccionadoId, ms.cx)
    '    Dim departamento_id As Integer = Memcached.SQLServerDirectAccess.SeleccionarEscalar(Of Integer)(departamentoSeleccionadoId, ms.Cx)
    '    PorcentajeDepartamentoDAL.Nuevo(proceso_id, departamento_id, porcentaje)
    '    pnlPorcentajeDepartamento.Controls.Add(New Label With {.Text = "Porcentaje añadido", .CssClass = "alert alert-success"})
    '    pnlNuevo.Visible = False
    '    cargarGrid()
    'End Sub

    'Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
    '    pnlNuevo.Visible = False

    'End Sub

End Class