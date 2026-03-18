Imports System.Data.SqlClient
Imports System.IO

Public Class DiferenciaInventario
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'pnlDiferenciaInventario.Visible = True
        'cargarGrid()
        Master.Title("Diferencia inventario")
        'avisoImportacion.Visible = False
        importarABD.Visible = False
    End Sub

    Private Sub cargarGrid()
        Dim datos As DataTable
        If Session("filtroFecha") Is Nothing OrElse Session("filtroFecha").Equals("") Then
            datos = DiferenciaInventarioBLL.Obtener()
        Else
            datos = DiferenciaInventarioBLL.ObtenerFiltrados(Session("filtroFecha"))
        End If
        grdDiferenciaInventario.DataSource = datos
        grdDiferenciaInventario.DataBind()

    End Sub

    Protected Sub btnConsultar_Click(sender As Object, e As EventArgs) Handles btnConsultar.Click
        pnlDiferenciaInventario.Visible = True
        titleBD.Visible = True
        grdDiferenciaInventario.Visible = True
        cargarGrid()

    End Sub

    Private Sub cargarDatosArchivo()
        Dim fechaArchivo As String = ""
        Dim csvPath As String = Server.MapPath("~/Files/") + Path.GetFileName(FileUpload1.PostedFile.FileName)

        If FileUpload1.PostedFile.FileName.Equals("") OrElse Not FileUpload1.HasFile Then

            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "info_msg", "alert('Debe seleccionar un archivo a importar');", True)
            Return
        End If

        If (FileUpload1.HasFile) Then
            FileUpload1.SaveAs(csvPath)
        Else
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "info_msg", "alert('Cancele la edición antes de cambiar de página.');", True)
        End If
        Dim dt As New DataTable()
        dt.Columns.AddRange(New DataColumn(3) {New DataColumn("FECHA ID", GetType(Integer)), New DataColumn("categoría", GetType(String)), New DataColumn("referencia", GetType(String)), New DataColumn("Precio Inventario", GetType(Decimal))})
        Dim csvData As String = File.ReadAllText(csvPath)
        Dim j As Integer = 0
        For Each row As String In csvData.Split(ControlChars.Lf)
            If Not String.IsNullOrEmpty(row) Then
                Dim i As Integer = 0
                If j > 0 Then
                    dt.Rows.Add()
                    'Execute a loop over the columns.  
                    Dim data As New List(Of String)
                    For Each cell As String In row.Split(";"c)
                        dt.Rows(j - 1)(i) = cell
                        i += 1
                        data.Add(cell)
                    Next
                    If (fechaArchivo.Equals("")) Then
                        fechaArchivo = CInt(data(0))
                    End If

                    'Dim insertString = "INSERT INTO [T_Diferencia_Inventario] ([FECHA_ID],[CATEGORIA] ,[REFERENCIA],[PRECIO_INVENTARIO]) VALUES (@FECHA,@CATEGORIA,@REFERENCIA,@PRECIO)"

                    'Dim lParametros As New List(Of SqlParameter)
                    'Dim p1 As SqlParameter
                    'Dim p2 As SqlParameter
                    'Dim p3 As SqlParameter
                    'Dim p4 As SqlParameter
                    'p1 = New SqlParameter("@FECHA", SqlDbType.Int) : p1.Value = CInt(data(0)) : lParametros.Add(p1)
                    'p2 = New SqlParameter("@CATEGORIA", SqlDbType.NVarChar) : p2.Value = data(1) : lParametros.Add(p2)
                    'p3 = New SqlParameter("@REFERENCIA", SqlDbType.NVarChar) : p3.Value = data(2) : lParametros.Add(p3)
                    'p4 = New SqlParameter("@PRECIO", SqlDbType.Float) : p4.Value = CDbl(data(3).Trim()) : lParametros.Add(p4)

                    'Memcached.SQLServerDirectAccess.NoQuery(insertString, connectionString, lParametros.ToArray)
                End If
            End If
            j += 1
        Next
        grdImportarListado.DataSource = dt
        grdImportarListado.DataBind()
        'avisoImportacion.Visible = True
        importarABD.Visible = True
    End Sub

    Protected Sub importarABD_Click(sender As Object, e As EventArgs) Handles importarABD.Click
        importarABD.Visible = False
        'avisoImportacion.Visible = False
        grdImportarListado.Visible = False
        titleFile.Visible = False
        Dim ms As New MPCR
        For Each data As GridViewRow In grdImportarListado.Rows
            Dim insertString = "INSERT INTO [T_Diferencia_Inventario] ([FECHA_ID],[CATEGORIA] ,[REFERENCIA],[PRECIO_INVENTARIO]) VALUES (@FECHA,@CATEGORIA,@REFERENCIA,@PRECIO)"
            Dim lParametros As New List(Of SqlParameter)
            Dim p1 As SqlParameter : p1 = New SqlParameter("@FECHA", SqlDbType.Int) : p1.Value = CInt(data.Cells(0).Text) : lParametros.Add(p1)
            Dim p2 As SqlParameter : p2 = New SqlParameter("@CATEGORIA", SqlDbType.NVarChar) : p2.Value = data.Cells(1).Text : lParametros.Add(p2)
            Dim p3 As SqlParameter : p3 = New SqlParameter("@REFERENCIA", SqlDbType.NVarChar) : p3.Value = data.Cells(2).Text : lParametros.Add(p3)
            Dim p4 As SqlParameter : p4 = New SqlParameter("@PRECIO", SqlDbType.Float) : p4.Value = CType(data.Cells(3).Text.Trim(), Decimal) : lParametros.Add(p4)
            Memcached.SQLServerDirectAccess.NoQuery(insertString, ms.Cx, lParametros.ToArray)
        Next

    End Sub

    Protected Sub btnCargarDatosArchivo_Click(sender As Object, e As EventArgs) Handles btnCargarDatosArchivo.Click
        cargarDatosArchivo()

    End Sub

    Protected Sub grdDiferenciaInventario_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdDiferenciaInventario.RowEditing
        grdDiferenciaInventario.EditIndex = e.NewEditIndex
        cargarGrid()

    End Sub

    Protected Sub grdDiferenciaInventario_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdDiferenciaInventario.RowUpdating
        btnInventarioManual.Visible = False
        Dim fechaID As Integer = (CType(grdDiferenciaInventario.Rows(e.RowIndex).Cells(0).Controls(1), TextBox)).Text
        Dim categoria As String = (CType(grdDiferenciaInventario.Rows(e.RowIndex).Cells(1).FindControl("txtCategoria"), TextBox)).Text
        Dim referencia As String = (CType(grdDiferenciaInventario.Rows(e.RowIndex).Cells(2).FindControl("txtReferencia"), TextBox)).Text
        Dim precioInventario As Decimal = Convert.ToDecimal(CType(grdDiferenciaInventario.Rows(e.RowIndex).Cells(3).FindControl("txtPrecioInventario"), TextBox).Text)
        DiferenciaInventarioDAL.Actualizar(fechaID, categoria, referencia, precioInventario)
        grdDiferenciaInventario.EditIndex = -1
        pnlDiferenciaInventario.Controls.Add(New Label With {.Text = "Registro actualizado", .CssClass = "alert alert-info"})
        btnInventarioManual.Visible = True
        cargarGrid()

    End Sub

    Protected Sub grdDiferenciaInventario_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdDiferenciaInventario.RowCancelingEdit
        grdDiferenciaInventario.EditIndex = -1
        cargarGrid()

    End Sub

    Protected Sub grdDiferenciaInventario_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdDiferenciaInventario.RowDeleting
        Dim fechaID As Integer = (CType(grdDiferenciaInventario.Rows(e.RowIndex).Cells(0).Controls(1), Label)).Text
        Dim categoria As String = (CType(grdDiferenciaInventario.Rows(e.RowIndex).Cells(1).Controls(1), Label)).Text
        Dim referencia As String = (CType(grdDiferenciaInventario.Rows(e.RowIndex).Cells(2).Controls(1), Label)).Text
        Dim precioInventario As Decimal = Convert.ToDecimal((CType(grdDiferenciaInventario.Rows(e.RowIndex).Cells(3).Controls(1), Label)).Text)
        DiferenciaInventarioDAL.Eliminar(fechaID, categoria, referencia, precioInventario)
        pnlDiferenciaInventario.Controls.Add(New Label With {.Text = "Registro eliminado", .CssClass = "alert alert-danger"})
        cargarGrid()

    End Sub

    Protected Sub grdDiferenciaInventario_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdDiferenciaInventario.RowDataBound
        'For Each cell As DataControlFieldCell In e.Row.Cells
        '    For Each control As Control In cell.Controls
        '        Dim button As ImageButton = TryCast(control, ImageButton)
        '        If button IsNot Nothing AndAlso button.CommandName = "Delete" Then button.OnClientClick = "if (!confirm('Está seguro de " & "querer borrar este registro?')) return false;modificado=false;"
        '    Next
        'Next

    End Sub

    Protected Sub grdDiferenciaInventario_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdDiferenciaInventario.PageIndexChanging
        If grdDiferenciaInventario.EditIndex <> -1 Then
            e.Cancel = True
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "info_msg", "alert('Cancele la edición antes de cambiar de página.');", True)
        Else
            Dim grdDiferenciaInventario As GridView = CType(sender, GridView)
            grdDiferenciaInventario.PageIndex = e.NewPageIndex
            cargarGrid()
        End If

    End Sub

    Protected Sub btnInventarioManual_Click(sender As Object, e As EventArgs) Handles btnInventarioManual.Click
        Response.Redirect("InventarioAjusteManual.aspx")

    End Sub

    Protected Sub btnFiltro_Click(sender As Object, e As EventArgs) Handles btnFiltro.Click
        Dim fecha = fechaFiltroData.Value
        Dim datos As DataTable

        Dim paramFecha As Integer = Integer.MinValue
        If Not String.IsNullOrEmpty(fecha) Then
            Dim fechaDate As New Date(fecha.Substring(0, 4), 12, 31)
            'fechaDate = fechaDate.AddMonths(1).AddDays(-1)
            paramFecha = fechaDate.Year * 10000 + fechaDate.Month * 100 + fechaDate.Day
            Session("filtroFecha") = paramFecha
            datos = DiferenciaInventarioBLL.ObtenerFiltrados(paramFecha)
        Else
            Session("filtroFecha") = Nothing
            datos = DiferenciaInventarioBLL.Obtener()
        End If


        grdDiferenciaInventario.DataSource = datos
        grdDiferenciaInventario.DataBind()
    End Sub


    Protected Sub btnFiltroOff_Click(sender As Object, e As EventArgs) Handles btnFiltroOff.Click
        Session("filtroFecha") = Nothing
        fechaFiltroData.Value = ""

        Dim datos As DataTable
        datos = DiferenciaInventarioBLL.Obtener()
        grdDiferenciaInventario.DataSource = datos
        grdDiferenciaInventario.DataBind()
    End Sub

End Class