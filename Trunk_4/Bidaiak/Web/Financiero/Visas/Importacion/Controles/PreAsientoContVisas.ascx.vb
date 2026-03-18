Public Class PreAsientoContVisas
    Inherits UserControl

#Region "Eventos"

    Public Event Advertencia(ByVal mensaje As String)
    Public Event ErrorGenerado(ByVal mensaje As String)
    Public Event Finalizado()
    Private itzultzaileWeb As New itzultzaile

#End Region

#Region "Page Load"

    ''' <summary>
    ''' Carga la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            inicializar()
        End If
    End Sub

    ''' <summary>
    ''' Inicializa los controles del control
    ''' </summary>    
    Private Sub inicializar()
        btnGenerar.Enabled = True
    End Sub

#End Region

#Region "Iniciar"

    ''' <summary>
    ''' Proceso para que se inicie el control
    ''' </summary>    
    ''' <param name="bImportar">True si se ha de importar.False para tratar los registros ya existentes en la base de datos</param>
    Public Function Iniciar(ByVal bImportar As Boolean) As Boolean
        Try
            inicializar()
            PageBase.log.Info("IMPORT_VISA (PASO 4): Se van importar los asientos a una tabla temporal")
            PintarAsientos(bImportar)
            Return True
        Catch batzEx As BatzException
            RaiseEvent ErrorGenerado(itzultzaileWeb.Itzuli("Error al generar la visualizacion del asiento contable de visas"))
            Return False
        Catch ex As Exception
            Dim sms As String = "Error al generar la visualizacion del asiento contable de visas"
            PageBase.log.Error(sms, ex)
            RaiseEvent ErrorGenerado(itzultzaileWeb.Itzuli(sms))
            Return False
        End Try
    End Function

#End Region

#Region "Eventos"

    ''' <summary>
    ''' Pinta los asientos
    ''' </summary>    
    ''' <param name="bImportar">Indica si se deberan importar los asientos. Si es false, significara que viene del detalle o que se continua la ejecucion que se habia quedado en este paso y debera coger los resultados de la tabla temporal</param>
    Private Sub PintarAsientos(ByVal bImportar As Boolean)
        Dim ticket As SabLib.ELL.Ticket = CType(Session("Ticket"), SabLib.ELL.Ticket)
        Dim cuentasBLL As New BLL.DepartamentosBLL
        Dim bContinuar As Boolean = True
        Dim lAsientosRegister, lAsientosAux As List(Of String())
        Dim idPlanta As Integer = CInt(Session("IdPlanta"))
        If (bImportar) Then
            cuentasBLL.DeleteAsientosContVisasTmp(idPlanta)
            cuentasBLL.ImportarAsientosContVisasTmp(idPlanta)
        End If
        lAsientosRegister = cuentasBLL.loadAsientosContVisasTmp(idPlanta, String.Empty, True)
        If (lAsientosRegister.Count > 0) Then
            'Se pinta la tabla de visa
            lAsientosAux = lAsientosRegister.FindAll(Function(o) o(10) = "0")
            Dim uoActual As String = String.Empty
            Dim totalImporte, subTotalImporte As Decimal
            totalImporte = 0
            For Each sAsiento As String() In lAsientosAux
                If (uoActual <> sAsiento(9)) Then  'UO nueva
                    If (uoActual <> String.Empty) Then
                        DibujarPie(subTotalImporte, tAsientos) 'La primera vez no se pintara el pie
                        totalImporte += subTotalImporte
                    End If
                    uoActual = sAsiento(9)
                    subTotalImporte = 0
                    DibujarCabecera(sAsiento(9), tAsientos)
                End If
                If (Not DibujarLinea(sAsiento, subTotalImporte, tAsientos)) Then bContinuar = False
            Next
            DibujarPie(subTotalImporte, tAsientos)
            totalImporte += subTotalImporte
            DibujarPie(totalImporte, tAsientos, True)

            'Se pinta la tabla de cuotas
            lAsientosAux = lAsientosRegister.FindAll(Function(o) o(10) = "1")
            If (lAsientosAux IsNot Nothing AndAlso lAsientosAux.Count > 0) Then
                totalImporte = 0
                DibujarCabecera(itzultzaileWeb.Itzuli("Cuotas de tarjetas"), tAsientosCuotas)
                For Each sAsiento As String() In lAsientosAux
                    DibujarLinea(sAsiento, totalImporte, tAsientosCuotas)
                Next
                DibujarPie(totalImporte, tAsientosCuotas, True)
            End If

            'Se pinta la tabla de visas excepcion
            'lAsientosAux = lAsientosRegister.FindAll(Function(o) o(10) = "2")
            'If (lAsientosAux IsNot Nothing AndAlso lAsientosAux.Count > 0) Then
            '    totalImporte = 0
            '    DibujarCabecera(itzultzaileWeb.Itzuli("Visas excepcion: Gasto de consumible"), tAsientosCuotas)
            '    For Each sAsiento As String() In lAsientosAux
            '        DibujarLinea(sAsiento, totalImporte, tAsientosCuotas)
            '    Next
            '    DibujarPie(totalImporte, tAsientosCuotas, True)
            'End If
        Else
            btnGenerar.Enabled = False
            RaiseEvent Advertencia("Ha ocurrido un error al recuperar los asientos contables")
        End If
    End Sub

    ''' <summary>
    ''' Continua al siguiente paso
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnGenerar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGenerar.Click
        RaiseEvent Finalizado()
    End Sub

#End Region

#Region "Dibujar tabla"

    ''' <summary>
    ''' Dibuja la cabecera de la nueva tabla
    ''' </summary>
    ''' <param name="unidadOrganizativa">Descripcion de la unidad organizativa</param>    
    ''' <param name="myTable">Tabla donde se agrega</param>
    Private Sub DibujarCabecera(ByVal unidadOrganizativa As String, ByVal myTable As Table)
        Dim row As New TableRow With {.CssClass = "GridViewBHeaderStyle"}
        row.Style.Add("font-weight", "bold")
        row.Style.Add("text-transform", "uppercase")
        Dim cell As New TableCell With {.Text = unidadOrganizativa} : row.Cells.Add(cell)
        cell = New TableCell With {.Text = itzultzaileWeb.Itzuli("Cuenta")} : row.Cells.Add(cell)
        cell = New TableCell With {.Text = itzultzaileWeb.Itzuli("Importe")} : row.Cells.Add(cell)
        cell = New TableCell With {.Text = itzultzaileWeb.Itzuli("detalle")} : row.Cells.Add(cell)
        myTable.Rows.Add(row)
    End Sub

    ''' <summary>
    ''' Dibuja la linea correspondiente al departamento
    ''' </summary>
    ''' <param name="sAsiento">Informacion del asiento</param>        
    ''' <param name="uoImporte">Parametro por referencia donde se sumara el importe al de la unidad organizativa</param>
    ''' <param name="myTable">Tabla donde se añadiran</param>
    ''' <returns>Booleano que indica si la linea se ha dibujado bien. Si se ha dibujado mal, significara que no tiene una cuenta asociada</returns>
    Private Function DibujarLinea(ByVal sAsiento As String(), ByRef uoImporte As Decimal, ByVal myTable As Table) As Boolean
        Dim bLineaOk As Boolean = True
        Dim row As New TableRow With {.CssClass = "GridViewBRowStyle"}
        row.Attributes.Add("onmouseover", "SartuPointer(this);")
        row.Attributes.Add("onmouseout", "IrtenPointer(this);")
        Dim cell As New TableCell With {.Text = sAsiento(4)} : row.Cells.Add(cell) 'Departamento
        cell = New TableCell   ' Cuenta        
        If (String.IsNullOrEmpty(sAsiento(8))) Then
            cell.Text = itzultzaileWeb.Itzuli("Sin cuenta")
            row.CssClass = "trRojo"  'Se marca para que se sepa cual             
            bLineaOk = False
        Else
            cell.Text = sAsiento(8)
        End If
        row.Cells.Add(cell)
        cell = New TableCell With {.Text = sAsiento(5) & " €"} : cell.Style.Add("text-align", "right") : row.Cells.Add(cell)   'Importe        
        uoImporte += PageBase.DecimalValue(sAsiento(5))
        cell = New TableCell With {.HorizontalAlign = HorizontalAlign.Center}   'Imagen ir al detalle        
        If (sAsiento(10) = "1" OrElse sAsiento(10) = "0") Then 'No aplica a las visas excepcion
            Dim hlDetalle As New HyperLink
            hlDetalle.CssClass = "tips"
            If (sAsiento(10) = "1") Then 'Cuota
                hlDetalle.Attributes("title") = itzultzaileWeb.Itzuli("Cuotas") & " (" & sAsiento(8) & ")"
            ElseIf (sAsiento(10) = "0") Then 'Movimiento            
                hlDetalle.Attributes("title") = sAsiento(4) & " (" & sAsiento(8) & ")"
            End If
            hlDetalle.NavigateUrl = Me.ResolveUrl("~/Financiero/Visas/DetMovVisasPopup.aspx?codDepto=" & sAsiento(3) & "&tipomov=" & sAsiento(10) & "&d=" & Now.Millisecond)  'La llamada con los milisegundos es para que no cache
            hlDetalle.Attributes("rel") = hlDetalle.NavigateUrl
            hlDetalle.Text = "<img src='../../../App_Themes/Tema1/Images/ver.png' style='border:none;' />"
            cell.Controls.Add(hlDetalle)
        ElseIf (sAsiento(10) = "2") Then 'Visas excepcion
            Dim hlDetalle As New HyperLink
            hlDetalle.CssClass = "tips"
            hlDetalle.Attributes("title") = itzultzaileWeb.Itzuli("Visas excepcion") & " (" & sAsiento(8) & ")"
            hlDetalle.NavigateUrl = Me.ResolveUrl("~/Financiero/Visas/DetMovVisasPopup.aspx?tipomov=" & sAsiento(10) & "&d=" & Now.Millisecond)  'La llamada con los milisegundos es para que no cache
            hlDetalle.Attributes("rel") = hlDetalle.NavigateUrl
            hlDetalle.Text = "<img src='../../../App_Themes/Tema1/Images/ver.png' style='border:none;' />"
            cell.Controls.Add(hlDetalle)
        End If
        row.Cells.Add(cell)
        myTable.Rows.Add(row)
        Return bLineaOk
    End Function

    ''' <summary>
    ''' Dibuja el pie con los totales de la unidad
    ''' </summary> 
    ''' <param name="importe">Importe de la uo</param>   
    ''' <param name="myTable">Tabla donde se añade</param>
    ''' <param name="TotalFinal">Indica si es el total de la pagina</param>    
    Private Sub DibujarPie(ByVal importe As Decimal, ByVal myTable As Table, Optional ByVal TotalFinal As Boolean = False)
        Dim row As New TableRow With {.CssClass = "GridViewBFooterStyle"}
        Dim cell As New TableCell With {.HorizontalAlign = HorizontalAlign.Center}
        cell.Text = If(TotalFinal, itzultzaileWeb.Itzuli("total"), itzultzaileWeb.Itzuli("Subtotal")).ToString.ToUpper
        row.Cells.Add(cell)
        cell = New TableCell With {.Text = "&nbsp;"} : row.Cells.Add(cell)
        cell = New TableCell With {.Text = importe & " €"} : cell.Style.Add("text-align", "right") : row.Cells.Add(cell)
        cell = New TableCell With {.Text = "&nbsp;"} : row.Cells.Add(cell)
        myTable.Rows.Add(row)
        row = New TableRow 'Linea de separacion
        cell = New TableCell With {.ColumnSpan = 3, .Text = "&nbsp;"} : row.Cells.Add(cell)
        myTable.Rows.Add(row)
    End Sub

#End Region

End Class