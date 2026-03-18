Imports TelefoniaLib

Partial Public Class RepartidorAlveolos
    Inherits PageBase

    ''' <summary>
    ''' Muestra los alveolos y su colocación en el armario
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Repartidor"
                Dim lAlveolos As List(Of ELL.Alveolo)
                Dim alvComp As New BLL.AlveoloComponent
                Dim oAlv As New ELL.Alveolo With {.IdPlanta = Master.Ticket.IdPlantaActual}
                lAlveolos = alvComp.getAlveolos(oAlv)
                If (lAlveolos IsNot Nothing AndAlso lAlveolos.Count > 0) Then
                    'lAlveolos.Sort(Function(o1 As ELL.Alveolo, o2 As ELL.Alveolo) o1.Id < o2.Id)
                    pintarListadoAlveolos(lAlveolos)
                End If
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Pinta dinamicamente la lista de alveolos (siguiendo el algoritmo de la aplicacion de telefonia antigua)
    ''' </summary>
    ''' <param name="lAlveolos">Lista de alveolos</param>
    Private Sub pintarListadoAlveolos(ByVal lAlveolos As List(Of ELL.Alveolo))
        Try
            Dim table As HtmlTable
            Dim tr, trRuta, trExt As HtmlTableRow
            Dim td, tdRuta, tdExt As HtmlTableCell
            Dim myLabel, myLabel2 As Label
            Dim hlLink As HyperLink
            Dim row, col, maxFila As Integer
            Dim oAlv As ELL.Alveolo
            Dim lAlveolosRow As List(Of ELL.Alveolo)
            Dim extComp As New BLL.ExtensionComponent
            Dim oExt As ELL.Extension = Nothing
            Dim lExt As List(Of ELL.Extension) = Nothing
            maxFila = lAlveolos.Max(Function(o As ELL.Alveolo) o.PosicionFila)
            For row = 1 To maxFila
                tr = New HtmlTableRow
                td = New HtmlTableCell With {.Width = 1}
                myLabel = New Label With {.Text = row, .CssClass = "negrita"} : td.Controls.Add(myLabel) : tr.Cells.Add(td)
                td = New HtmlTableCell
                table = New HtmlTable
                trRuta = New HtmlTableRow
                tdRuta = New HtmlTableCell With {.Align = "center"}
                myLabel = New Label With {.Text = "NºEQP", .CssClass = "negrita"} : tdRuta.Controls.Add(myLabel) : trRuta.Cells.Add(tdRuta)
                table.Rows.Add(trRuta)
                trRuta = New HtmlTableRow
                tdRuta = New HtmlTableCell With {.Align = "center"}
                myLabel = New Label With {.Text = "NºExt", .CssClass = "negrita"} : tdRuta.Controls.Add(myLabel) : trRuta.Cells.Add(tdRuta)
                table.Rows.Add(trRuta)
                td.Controls.Add(table)
                tr.Cells.Add(td)
                'Se pinta el recuadro con las extensiones y los alveolos
                td = New HtmlTableCell With {.ColSpan = 10}
                table = New HtmlTable With {.Width = "100%"} : table.Style.Add("border", "solid 1px #000000")
                trRuta = New HtmlTableRow
                trExt = New HtmlTableRow
                lAlveolosRow = lAlveolos.FindAll(Function(o As ELL.Alveolo) o.PosicionFila = row)
                If (lAlveolosRow.Count = 0) Then 'No hay ningun alveolo para esa fila
                    For k As Integer = 1 To 10
                        tdRuta = New HtmlTableCell With {.Align = "center", .Width = "10%"}
                        myLabel = New Label With {.Text = "&nbsp;"}
                        tdRuta.Controls.Add(myLabel)
                        trRuta.Cells.Add(tdRuta)
                    Next
                Else
                    For col = 1 To 10
                        oAlv = lAlveolosRow.Find(Function(o As ELL.Alveolo) o.PosicionFila = row And o.PosicionColumna = col)
                        tdRuta = New HtmlTableCell With {.Align = "center", .Width = "10%"}
                        tdExt = New HtmlTableCell With {.Align = "center", .Width = "10%"}
                        myLabel = New Label : myLabel2 = New Label : hlLink = New HyperLink
                        If (oAlv Is Nothing) Then
                            myLabel.Text = "&nbsp;" : myLabel.Text = "&nbsp;"
                            tdRuta.Controls.Add(myLabel)
                        Else
                            hlLink.NavigateUrl = "~/Mantenimientos/Alveolo/Alveolo.aspx?id=" & oAlv.Id
                            hlLink.Text = oAlv.Ruta : hlLink.Style.Add("color", If(oAlv.Estado, "green", "red"))
                            lExt = extComp.getExtensiones(New ELL.Extension With {.IdAlveolo = oAlv.Id}, Master.Ticket.IdPlantaActual, False) 'Si tuviera mas de una extension activas, mostraria el listado                            
                            If (lExt IsNot Nothing AndAlso lExt.Count > 0) Then
                                Dim extensiones As String = String.Empty
                                For Each myExt As ELL.Extension In lExt
                                    extensiones &= If(extensiones = String.Empty, "", ",") & myExt.Extension
                                Next
                                If (extensiones <> String.Empty) Then myLabel2.Text = extensiones
                            Else
                                myLabel2.Text = "---"
                            End If
                            tdRuta.Controls.Add(hlLink)
                        End If
                        trRuta.Cells.Add(tdRuta)
                        tdExt.Controls.Add(myLabel2) : trExt.Cells.Add(tdExt)
                    Next
                End If
                table.Rows.Add(trRuta) : table.Rows.Add(trExt)
                td.Controls.Add(table)
                tr.Cells.Add(td)
                tAlveolos.Rows.Add(tr)
                'Añadimos fila en blanco
                tr = New HtmlTableRow
                For k As Integer = 1 To 12
                    td = New HtmlTableCell
                    myLabel = New Label With {.Text = "&nbsp;"} : td.Controls.Add(myLabel)
                Next
                tr.Cells.Add(td)
                tAlveolos.Rows.Add(tr)
            Next
            'For row = 1 To 21
            '    tr = New HtmlTableRow
            '    'Indice de la fila
            '    td = New HtmlTableCell With {.Width = 1}
            '    myLabel = New Label With {.Text = row}
            '    myLabel.CssClass = "negrita"
            '    td.Controls.Add(myLabel)
            '    tr.Cells.Add(td)
            '    'NºEQP y NºExt
            '    td = New HtmlTableCell
            '    table = New HtmlTable
            '    tr2 = New HtmlTableRow
            '    td2 = New HtmlTableCell
            '    myLabel = New Label With {.Text = "NºEQP"}
            '    myLabel.CssClass = "negrita"
            '    td2.Controls.Add(myLabel)
            '    tr2.Cells.Add(td2)
            '    table.Rows.Add(tr2)
            '    tr2 = New HtmlTableRow
            '    td2 = New HtmlTableCell
            '    myLabel = New Label With {.Text = "NºExt"}
            '    myLabel.CssClass = "negrita"
            '    td2.Controls.Add(myLabel)
            '    tr2.Cells.Add(td2)
            '    table.Rows.Add(tr2)
            '    td.Controls.Add(table)
            '    tr.Cells.Add(td)
            '    'Se pinta el recuadro con las extensiones y los alveolos
            '    td = New HtmlTableCell With {.ColSpan = 10}
            '    table = New HtmlTable
            '    table.Style.Add("border", "solid 1px #000000")
            '    table.Width = "100%"
            '    tr2 = New HtmlTableRow  'fila con la posicion
            '    tr3 = New HtmlTableRow  'fila con la extension
            '    For col = 1 To 10
            '        If (indexAlv > lAlveolos.Count - 1) Then
            '            'se insertan tantas columnas como falten hasta llegar a 10
            '            For k As Integer = col To 10
            '                td2 = New HtmlTableCell With {.Align = "center", .Width = "10%"}
            '                myLabel = New Label With {.Text = "&nbsp;"}
            '                td2.Controls.Add(myLabel)
            '                tr2.Cells.Add(td2)
            '            Next
            '            Exit For
            '        End If
            '        oAlv = lAlveolos.Item(indexAlv)
            '        td2 = New HtmlTableCell With {.Align = "center", .Width = "10%"}
            '        myLabel = New Label
            '        If ((oAlv.IdTipo = idTipoAlv_Analogico And row = 4) Or (oAlv.Ruta = "1-3-0" And row = 19)) Then
            '            For k As Integer = col To 10
            '                td2 = New HtmlTableCell With {.Align = "center", .Width = "10%"}
            '                myLabel = New Label With {.Text = "&nbsp;"}
            '                td2.Controls.Add(myLabel)
            '                tr2.Cells.Add(td2)
            '            Next                        
            '            Exit For                    
            '        Else
            '            myLabel.Text = oAlv.Ruta
            '            If (oAlv.Estado) Then
            '                myLabel.Style.Add("color", "green")
            '            Else
            '                myLabel.Style.Add("color", "red")
            '            End If
            '        End If
            '        td2.Controls.Add(myLabel)
            '        tr2.Cells.Add(td2)

            '        td2 = New HtmlTableCell With {.Align = "center"}
            '        myLabel = New Label

            '        myLabel.Text = "---"
            '        myLabel.Style.Add("color", "blue")
            '        oExt = New ELL.Extension With {.IdAlveolo = oAlv.Id}
            '        'oExt = extComp.getExtension(oExt, Master.Ticket.IdPlantaActual, False, False)
            '        'If (oExt IsNot Nothing) Then myLabel.Text = oExt.Extension
            '        'Si tuviera mas de una extension activas, mostraria el listado
            '        lExt = extComp.getExtensiones(oExt, Master.Ticket.IdPlantaActual, False)
            '        If (lExt IsNot Nothing AndAlso lExt.Count > 0) Then
            '            Dim extensiones As String = String.Empty
            '            For Each myExt As ELL.Extension In lExt
            '                extensiones &= If(extensiones = String.Empty, "", ",") & myExt.Extension
            '            Next
            '            If (extensiones <> String.Empty) Then myLabel.Text = extensiones
            '        End If
            '        td2.Controls.Add(myLabel)
            '        tr3.Cells.Add(td2)
            '        indexAlv += 1
            '    Next
            '    table.Rows.Add(tr2)
            '    table.Rows.Add(tr3)
            '    td.Controls.Add(table)
            '    tr.Cells.Add(td)
            '    tAlveolos.Rows.Add(tr)
            '    'Añadimos fila en blanco
            '    tr = New HtmlTableRow
            '    For k As Integer = 1 To 12
            '        td = New HtmlTableCell
            '        myLabel = New Label With {.Text = "&nbsp;"}
            '        td.Controls.Add(myLabel)                    
            '    Next
            '    tr.Cells.Add(td)
            '    tAlveolos.Rows.Add(tr)
            'Next
        Catch ex As Exception
            Master.MensajeError = "errMostrandoAlveolos"
        End Try
    End Sub

End Class