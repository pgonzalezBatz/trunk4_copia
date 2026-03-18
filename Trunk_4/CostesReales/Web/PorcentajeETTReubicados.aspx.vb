Public Class PorcentajeETTReubicados
    Inherits System.Web.UI.Page

    Public Property dptoEditando As Integer
        Get
            If ViewState("dptoEditando") Is Nothing Then ViewState("dptoEditando") = ""
            Return ViewState("dptoEditando").ToString()
        End Get

        Set(ByVal value As Integer)
            ViewState("dptoEditando") = value
        End Set

    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            pnlDepartamentos.Visible = True
            pnlNuevo.Visible = False
            btnNuevo.Visible = True
            cargarGrid()
            Master.Title("Porcentajes por departamento para el ejercicio vigente")
        End If

    End Sub

    Private Sub cargarGrid()
        Dim datos As DataTable
        'datos = PorcentajeETTReubicadosBLL.Obtener()

        'Dim paramFecha = Integer.MinValue
        'Dim paramDpto = Integer.MinValue
        'If Session("filtroFecha") IsNot Nothing Then paramFecha = CInt(Session("filtroFecha"))
        'If Session("filtroDpto") IsNot Nothing Then paramDpto = CInt(Session("filtroDpto"))
        Dim paramFecha = If(Session("filtroFecha") Is Nothing, Integer.MinValue, CInt(Session("filtroFecha")))
        Dim paramDpto = If(Session("filtroDpto") Is Nothing, Integer.MinValue, CInt(Session("filtroDpto")))
        datos = PorcentajeETTReubicadosBLL.ObtenerFiltrados(paramFecha, paramDpto)
        grdPorcentajeETTReubicados.DataSource = datos
        For Each drFila As DataRow In datos.Rows
            datos.Columns.Item(2).ColumnName = "Reubicados"
            datos.Columns.Item(3).ColumnName = "ETT"
        Next
        grdPorcentajeETTReubicados.DataBind()
    End Sub

    Protected Sub btnFiltro_Click(sender As Object, e As EventArgs) Handles btnFiltro.Click
        Dim fecha = fechaFiltroData.Value
        Dim dpto = dptoFiltroData.Text

        Dim paramFecha As Integer = Integer.MinValue
        Dim paramDpto As Integer = Integer.MinValue
        If Not String.IsNullOrEmpty(fecha) Then
            Dim fechaDate As New Date(fecha.Substring(0, 4), fecha.Substring(5, 2), 1)
            fechaDate = fechaDate.AddMonths(1).AddDays(-1)
            paramFecha = fechaDate.Year * 10000 + fechaDate.Month * 100 + fechaDate.Day
            Session("filtroFecha") = paramFecha
        Else
            Session("filtroFecha") = Nothing
        End If
        If Not String.IsNullOrEmpty(dpto) Then
            paramDpto = CInt(dpto)
            Session("filtroDpto") = paramDpto
        Else
            Session("filtroDpto") = Nothing
        End If

        Dim datos As DataTable
        datos = PorcentajeETTReubicadosBLL.ObtenerFiltrados(paramFecha, paramDpto)
        grdPorcentajeETTReubicados.DataSource = datos
        grdPorcentajeETTReubicados.DataBind()
    End Sub

    Protected Sub btnFiltroOff_Click(sender As Object, e As EventArgs) Handles btnFiltroOff.Click
        Session("filtroFecha") = Nothing
        Session("filtroDpto") = Nothing
        fechaFiltroData.Value = ""
        dptoFiltroData.Text = ""

        Dim datos As DataTable
        datos = PorcentajeETTReubicadosBLL.ObtenerFiltrados(Integer.MinValue, Integer.MinValue)
        grdPorcentajeETTReubicados.DataSource = datos
        grdPorcentajeETTReubicados.DataBind()
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        ddlDepartamentos.ClearSelection()
        txtReubicadosForm.Text = ""
        txtETTForm.Text = ""
        txtFecha.Text = ""
        cargarDepartamentos()
        pnlNuevo.Visible = True

    End Sub

    Protected Sub cargarDepartamentos()
        ddlDepartamentos.Items.Clear()
        ddlDepartamentos.Items.Add("Seleccione departamento...")
        For Each item As String() In Utilidades.ObtenerQuerySQLSERVERParametros("SELECT DPTO FROM T_DPTO")
            ddlDepartamentos.Items.Add(item(0))
        Next
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim db As New PorcentajeETTReubicadosDAL()
        Dim departamento As String = ddlDepartamentos.SelectedValue
        Dim departamentoSeleccionado As String = ddlDepartamentos.SelectedValue()
        Dim departamentoSeleccionadoId As String = "SELECT ID FROM T_DPTO WHERE DPTO LIKE '%" + departamentoSeleccionado + "%'"
        Dim departamento_id As Integer = Memcached.SQLServerDirectAccess.SeleccionarEscalar(Of Integer)(departamentoSeleccionadoId, Master.Cx)
        Dim reubicados As Single? = If(String.IsNullOrEmpty(txtReubicadosForm.Text), DBNull.Value, Convert.ToSingle(txtReubicadosForm.Text))
        Dim ett As Single? = If(String.IsNullOrEmpty(txtETTForm.Text), Nothing, Convert.ToSingle(txtETTForm.Text))
        Dim fecha1 As Integer = CInt(txtFecha.Text)
        Dim fechaDate As Date = New Date(txtFecha.Text.Substring(0, 4), txtFecha.Text.Substring(4, 2), 1)
        Dim day = fechaDate.AddMonths(1).AddDays(-1).Day()
        Dim finalDate As Date = New Date(txtFecha.Text.Substring(0, 4), txtFecha.Text.Substring(4, 2), day)
        Dim fecha As Integer = finalDate.Year * 10000 + finalDate.Month * 100 + finalDate.Day
        If PorcentajeETTReubicadosBLL.Existe(departamentoSeleccionado, fecha).Rows.Count = 0 Then
            db.Nuevo(departamento_id, reubicados, ett, fecha)
            pnlDepartamentos.Controls.Add(New Label With {.Text = "Porcentaje añadido", .CssClass = "alert alert-success"})
        Else
            db.Actualizar(departamento_id, reubicados, ett, fecha)
            pnlDepartamentos.Controls.Add(New Label With {.Text = "Porcentaje actualizado", .CssClass = "alert alert-success"})
        End If
        pnlNuevo.Visible = False
        cargarGrid()

    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        pnlNuevo.Visible = False

    End Sub

    Protected Sub grdPorcentajeETTReubicados_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdPorcentajeETTReubicados.RowEditing
        grdPorcentajeETTReubicados.EditIndex = e.NewEditIndex
        dptoEditando = (CType(grdPorcentajeETTReubicados.Rows(e.NewEditIndex).Cells(1).Controls(1), Label)).Text
        cargarGrid()

    End Sub

    Protected Sub grdPorcentajeETTReubicados_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdPorcentajeETTReubicados.RowUpdating
        Dim db As New PorcentajeETTReubicadosDAL()
        btnNuevo.Visible = False
        pnlNuevo.Visible = False
        Dim departamento As String = CType(grdPorcentajeETTReubicados.Rows(e.RowIndex).Cells(2).Controls(1), TextBox).Text
        Dim reubicados As String = CType(grdPorcentajeETTReubicados.Rows(e.RowIndex).Cells(3).Controls(1), TextBox).Text
        Dim ett As String = CType(grdPorcentajeETTReubicados.Rows(e.RowIndex).Cells(4).Controls(1), TextBox).Text
        Dim fechaid As String = Convert.ToInt32(CType(grdPorcentajeETTReubicados.Rows(e.RowIndex).Cells(6).Controls(1), TextBox).Text)
        db.Actualizar(dptoEditando, reubicados, ett, fechaid)
        grdPorcentajeETTReubicados.EditIndex = -1
        pnlDepartamentos.Controls.Add(New Label With {.Text = "Porcentaje actualizado", .CssClass = "alert alert-info"})
        btnNuevo.Visible = True
        cargarGrid()

    End Sub

    Protected Sub grdPorcentajeETTReubicados_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdPorcentajeETTReubicados.RowDeleting
        Dim dpto As Integer = Convert.ToInt32(CType(grdPorcentajeETTReubicados.Rows(e.RowIndex).Cells(1).Controls(1), Label).Text)
        Dim fecha As Integer = Convert.ToInt32(CType(grdPorcentajeETTReubicados.Rows(e.RowIndex).Cells(6).Controls(1), Label).Text)
        PorcentajeETTReubicadosDAL.Eliminar(dpto, fecha)
        pnlDepartamentos.Controls.Add(New Label With {.Text = "Porcentaje eliminado", .CssClass = "alert alert-danger"})
        cargarGrid()

    End Sub

    Protected Sub grdPorcentajeETTReubicados_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdPorcentajeETTReubicados.RowCancelingEdit
        grdPorcentajeETTReubicados.EditIndex = -1
        cargarGrid()

    End Sub

    Protected Sub grdPorcentajeETTReubicados_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdPorcentajeETTReubicados.RowDataBound
        If grdPorcentajeETTReubicados.EditIndex = -1 Then
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim lblReubicados As Label = e.Row.Cells(3).Controls(1)
                If lblReubicados.Text <> "" Then
                    Dim numReubicados As Decimal = Convert.ToDecimal(lblReubicados.Text)
                    lblReubicados.Text = numReubicados.ToString("0.00") + "%"
                    If lblReubicados.Text = "0,00%" Then
                        lblReubicados.Text = ""
                    End If
                End If
                Dim lblETT As Label = e.Row.Cells(4).Controls(1)
                If lblETT.Text <> "" Then
                    Dim numETT As Decimal = Convert.ToDecimal(lblETT.Text)
                    lblETT.Text = numETT.ToString("0.00") + "%"
                    If lblETT.Text = "0,00%" Then
                        lblETT.Text = ""
                    End If
                End If
                Dim lblTotal As Label = e.Row.Cells(5).Controls(1)
                If lblTotal.Text <> "" Then
                    Dim numTotal As Decimal = Convert.ToDecimal(lblTotal.Text)
                    lblTotal.Text = numTotal.ToString("0.00") + "%"
                    If lblTotal.Text = "0,00%" Then
                        lblTotal.Text = ""
                    End If
                End If
            End If
        End If
    End Sub

    Protected Sub grdPorcentajeETTReubicados_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdPorcentajeETTReubicados.PageIndexChanging
        If grdPorcentajeETTReubicados.EditIndex <> -1 Then
            e.Cancel = True
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "info_msg", "alert('Cancele la edición antes de cambiar de página.');", True)
        Else
            Dim grdCCPorNegocio As GridView = CType(sender, GridView)
            grdCCPorNegocio.PageIndex = e.NewPageIndex
            cargarGrid()
        End If
    End Sub


End Class