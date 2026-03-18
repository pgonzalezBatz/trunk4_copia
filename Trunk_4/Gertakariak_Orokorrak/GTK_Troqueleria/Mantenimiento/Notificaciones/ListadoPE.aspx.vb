Public Class ListadoPE
    Inherits PageBase
#Region "Propiedades"
    Public BBDD As New BatzBBDD.Entities_Gertakariak
#End Region

#Region "Eventos de Pagina"
    Private Sub Listado_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Try
            'CargarDatos()
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CargarDatos()
    End Sub
#End Region

#Region "Funciones y Procesos"
    Sub CargarDatos()
        Dim headerCells = tablePEs.Rows.Item(0).Cells
        For Each headerCell In headerCells
            headerCell.Style.Add("padding", "10px")
        Next
        Dim lItemOF = From Reg As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA Where Reg.IDITURRIA = My.Settings.IdPEs Select Reg Order By Reg.DESCRIPCION
        'dlPEs.DataSource = lItemOF.ToList : dlPEs.DataBind()
        For Each item In lItemOF
            Dim valueList = (From Reg As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA Where Reg.IDITURRIA = item.ID Select Reg).ToList
            For Each value In valueList
                Dim row As New TableRow
                Dim cell1 As New TableCell
                Dim cell2 As New TableCell
                Dim cell3 As New TableCell
                cell1.Text = item.DESCRIPCION
                cell1.Style.Add("padding", "8px")
                cell2.Text = value.DESCRIPCION
                cell2.Style.Add("padding", "8px")
                'cell3.Controls.Add(New ImageButton With {.})
                Dim deleteButton As New ImageButton With {
                    .ID = "btnBorrarPE_" & value.ID,
                    .ImageUrl = "~/App_Themes/Batz/IconosAcciones/Eliminar24.png",
                    .CommandArgument = value.ID
                }
                'deleteButton.CommandName = "Delete"
                AddHandler deleteButton.Click, AddressOf Me.deleteButton_Click
                cell3.Controls.Add(deleteButton)
                Dim confirmDelete As New AjaxControlToolkit.ConfirmButtonExtender With {
                    .ID = "confirmDeleteButtonExtender_" & value.ID,
                    .TargetControlID = "btnBorrarPE_" & value.ID,
                    .ConfirmText = "Desea eliminar"
                }
                cell3.Controls.Add(confirmDelete)
                cell3.Style.Add("padding", "8px")
                row.Cells.Add(cell1)
                row.Cells.Add(cell2)
                row.Cells.Add(cell3)
                row.Attributes.Add("class", "RowStyle")
                tablePEs.Rows.Add(row)
            Next
            'cell2.Text = String.Join(", ", valueList.Select(Function(f) f.DESCRIPCION).ToList)
            'row.Cells.Add(cell1)
            'row.Cells.Add(cell2)
            'tablePEs.Rows.Add(row)
        Next
    End Sub

    Private Sub deleteButton_Click(sender As Object, e As ImageClickEventArgs)
        Dim id = CInt(sender.CommandArgument)
        Dim Usr As BatzBBDD.ESTRUCTURA = (From Reg As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA Where Reg.ID = id Select Reg).SingleOrDefault
        If Usr IsNot Nothing Then
            BBDD.ESTRUCTURA.DeleteObject(Usr)
            BBDD.SaveChanges()
        End If
        Response.Redirect(HttpContext.Current.Request.Url.ToString(), True)
    End Sub

#End Region

End Class
