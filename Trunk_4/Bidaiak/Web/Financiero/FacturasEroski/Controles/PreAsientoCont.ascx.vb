Imports BidaiakLib

Public Class PreAsientoCont
    Inherits UserControl

#Region "Eventos"

    Public Event Advertencia(ByVal mensaje As String)
    Public Event ErrorGenerado(ByVal mensaje As String)
    Public Event Finalizado()
    Private itzultzaileWeb As New Itzultzaile

#End Region

#Region "Page Load"

    ''' <summary>
    ''' Carga la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
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
            PageBase.log.Info("FACT_AGEN (PASO 4): Se van importar los asientos a una tabla temporal")
            PintarAsientos(bImportar)
            Return True
        Catch batzEx As BatzException
            RaiseEvent ErrorGenerado(batzEx.Termino)
            Return False
        Catch ex As Exception
            Dim sms As String = "Error al generar la visualizacion del asiento contable de facturas de Eroski"
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
        Dim ticket As Sablib.ELL.Ticket = CType(Session("Ticket"), Sablib.ELL.Ticket)
        Dim cuentasBLL As New BLL.DepartamentosBLL
        Dim idPlanta As Integer = CInt(Session("IdPlanta"))
        If (bImportar) Then
            cuentasBLL.DeleteAsientosContEroskiTmp(idPlanta)
            cuentasBLL.ImportarAsientosContEroskiTmp(idPlanta)
        End If
        Dim lAsientos As List(Of ELL.AsientoContableEroskiTmp) = cuentasBLL.loadAsientosContEroskiTmp(idPlanta, Integer.MinValue, True)
        Dim uoActual As String = String.Empty
        Dim totalIVA0, totalIVA8, totalIVA18, subTotalIVA0, subTotalIVA8, subTotalIVA18 As Decimal
        totalIVA0 = 0 : totalIVA8 = 0 : totalIVA18 = 0
        Dim factura As String = String.Empty
        For Each oAsiento As ELL.AsientoContableEroskiTmp In lAsientos
            If (factura <> oAsiento.Factura) Then
                If (factura <> String.Empty) Then
                    DibujarPie(subTotalIVA0, subTotalIVA8, subTotalIVA18) 'La primera vez no se pintara el pie
                    totalIVA0 += subTotalIVA0 : totalIVA8 += subTotalIVA8 : totalIVA18 += subTotalIVA18
                    subTotalIVA0 = 0 : subTotalIVA8 = 0 : subTotalIVA18 = 0
                End If
                factura = oAsiento.Factura
                DibujarCabeceraFactura(factura)
                uoActual = String.Empty
            End If
            If (uoActual <> oAsiento.UnidadOrganizativa) Then  'UO nueva
                If (uoActual <> String.Empty) Then
                    DibujarPie(subTotalIVA0, subTotalIVA8, subTotalIVA18) 'La primera vez no se pintara el pie
                    totalIVA0 += subTotalIVA0 : totalIVA8 += subTotalIVA8 : totalIVA18 += subTotalIVA18
                End If
                uoActual = oAsiento.UnidadOrganizativa
                subTotalIVA0 = 0 : subTotalIVA8 = 0 : subTotalIVA18 = 0
                DibujarCabecera(oAsiento.UnidadOrganizativa)
            End If
            DibujarLinea(oAsiento, subTotalIVA0, subTotalIVA8, subTotalIVA18, factura)
        Next
        DibujarPie(subTotalIVA0, subTotalIVA8, subTotalIVA18)
        totalIVA0 += subTotalIVA0 : totalIVA8 += subTotalIVA8 : totalIVA18 += subTotalIVA18
        DibujarPie(totalIVA0, totalIVA8, totalIVA18, True)
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
    ''' Dibuja la cabecera para separar las facturas
    ''' </summary>    
    ''' <param name="factura">Numero de factura a pintar</param>
    Private Sub DibujarCabeceraFactura(ByVal factura As String)
        Dim row As TableRow : Dim cell As TableCell
        row = New TableRow
        row.Style.Add("font-weight", "bold") : row.Style.Add("text-transform", "uppercase")
        Dim textoFactura As String = itzultzaileWeb.Itzuli("Factura") & ": " & factura
        If (factura.Contains("B")) Then
            textoFactura &= " (" & itzultzaileWeb.Itzuli("Servicios aereos") & ")"
        ElseIf (factura.Contains("S")) Then
            textoFactura &= " (" & itzultzaileWeb.Itzuli("Resto de servicios") & ")"
        End If
        cell = New TableCell With {.Text = textoFactura & "<br /><br />", .ColumnSpan = 5}
        row.Cells.Add(cell) : tAsientos.Rows.Add(row)
    End Sub

    ''' <summary>
    ''' Dibuja la cabecera de la nueva tabla
    ''' </summary>
    ''' <param name="unidadOrganizativa">Descripcion de la unidad organizativa</param>        
    Private Sub DibujarCabecera(ByVal unidadOrganizativa As String)
        Dim row As New TableRow With {.CssClass = "GridViewBHeaderStyle"}
        row.Style.Add("font-weight", "bold")
        row.Style.Add("text-transform", "uppercase")
        Dim cell As TableCell = New TableCell With {.Text = unidadOrganizativa} : row.Cells.Add(cell)
        cell = New TableCell With {.Text = itzultzaileWeb.Itzuli("Gasto exento iva")} : row.Cells.Add(cell)
        cell = New TableCell With {.Text = itzultzaileWeb.Itzuli("Gasto iva reducido")} : row.Cells.Add(cell)
        cell = New TableCell With {.Text = itzultzaileWeb.Itzuli("Gasto iva normal")} : row.Cells.Add(cell)
        cell = New TableCell With {.Text = itzultzaileWeb.Itzuli("detalle")} : row.Cells.Add(cell)
        tAsientos.Rows.Add(row)
    End Sub

    ''' <summary>
    ''' Dibuja la linea correspondiente al departamento
    ''' </summary>
    ''' <param name="oAsiento">Informacion del asiento</param>    
    ''' <param name="uoIva0">Parametro por referencia donde se sumara el importe de iva al 0 al de la unidad organizativa</param>
    ''' <param name="uoIva8">Parametro por referencia donde se sumara el importe de iva al 8 al de la unidad organizativa</param>
    ''' <param name="uoIva18">Parametro por referencia donde se sumara el importe de iva al 18 al de la unidad organizativa</param>
    ''' <param name="factura">Numero de factura</param>
    Private Sub DibujarLinea(ByVal oAsiento As ELL.AsientoContableEroskiTmp, ByRef uoIva0 As Decimal, ByRef uoIva8 As Decimal, ByRef uoIva18 As Decimal, ByVal factura As String)
        Dim marcarEnRojo As Boolean = False
        Dim row As New TableRow With {.CssClass = "GridViewBRowStyle"}
        row.Attributes.Add("onmouseover", "SartuPointer(this);")
        row.Attributes.Add("onmouseout", "IrtenPointer(this);")
        'Departamento
        Dim cell As New TableCell
        marcarEnRojo = (oAsiento.CodigoDepart = String.Empty OrElse oAsiento.Departamento = String.Empty OrElse oAsiento.Cuenta_18 <= 0) 'Sino tiene codigo ni nombre depto ni cuentas, significara que no es valido
        If (oAsiento.CodigoDepart <> String.Empty And oAsiento.Departamento <> String.Empty) Then
            cell.Text = oAsiento.Departamento
        ElseIf (oAsiento.CodigoDepart <> String.Empty And oAsiento.Departamento = String.Empty) Then
            cell.Text = oAsiento.CodigoDepart
        ElseIf (oAsiento.CodigoDepart = String.Empty And oAsiento.Departamento <> String.Empty) Then
            cell.Text = oAsiento.Departamento
        Else
            cell.Text = itzultzaileWeb.Itzuli("Sin departamento asociado")
        End If
        If (marcarEnRojo) Then cell.ForeColor = Drawing.Color.Red
        row.Cells.Add(cell)
        'IVA 0% 
        Dim baseExe_RegEsp As Decimal = oAsiento.BaseExe_0 + oAsiento.RegEsp
        cell = New TableCell With {.Text = baseExe_RegEsp}
        uoIva0 += baseExe_RegEsp
        If (marcarEnRojo) Then cell.ForeColor = Drawing.Color.Red
        row.Cells.Add(cell)
        'IVA 8%
        cell = New TableCell With {.Text = oAsiento.BaseIR_8}
        uoIva8 += oAsiento.BaseIR_8
        If (marcarEnRojo) Then cell.ForeColor = Drawing.Color.Red
        row.Cells.Add(cell)
        'IVA 18%
        cell = New TableCell With {.Text = oAsiento.BaseIG_18}
        uoIva18 += oAsiento.BaseIG_18
        If (marcarEnRojo) Then cell.ForeColor = Drawing.Color.Red
        row.Cells.Add(cell)
        'Imagen ir al detalle
        cell = New TableCell With {.HorizontalAlign = HorizontalAlign.Center}
        Dim hlDetalle As New HyperLink With {.CssClass = "tips"}
        hlDetalle.NavigateUrl = Me.ResolveUrl("~/Financiero/FacturasEroski/DetMovFactPopup.aspx?codDepto=" & oAsiento.CodigoDepart & "&factu=" & factura & "&d=" & Now.Millisecond)  'La llamada con los milisegundos es para que no cachee
        hlDetalle.Attributes("rel") = hlDetalle.NavigateUrl
        hlDetalle.Text = "<img src='../../App_Themes/Tema1/Images/ver.png' style='border:none;' />"
        hlDetalle.Attributes("title") = oAsiento.Departamento & " (" & oAsiento.CodigoDepart & ")"
        cell.Controls.Add(hlDetalle)
        row.Cells.Add(cell)
        tAsientos.Rows.Add(row)
    End Sub

    ''' <summary>
    ''' Dibuja el pie con los totales de la unidad
    ''' </summary> 
    ''' <param name="sinIva">Importe de la uo sin iva</param>   
    ''' <param name="iva8">Importe de la uo con el 8% de iva</param>
    ''' <param name="iva18">Importe de la uo con el 18% de iva</param>
    ''' <param name="TotalFinal">Indica si es el total de la pagina</param>
    Private Sub DibujarPie(ByVal sinIva As Decimal, ByVal iva8 As Decimal, ByVal iva18 As Decimal, Optional ByVal TotalFinal As Boolean = False)
        Dim row As New TableRow With {.CssClass = "GridViewBFooterStyle"}
        Dim cell As New TableCell With {.HorizontalAlign = HorizontalAlign.Center}
        cell.Text = If(TotalFinal, itzultzaileWeb.Itzuli("total"), itzultzaileWeb.Itzuli("Subtotal")).ToString.ToUpper        
        row.Cells.Add(cell)
        cell = New TableCell With {.Text = sinIva} : row.Cells.Add(cell)
        cell = New TableCell With {.Text = iva8} : row.Cells.Add(cell)
        cell = New TableCell With {.Text = iva18} : row.Cells.Add(cell)
        cell = New TableCell With {.Text = "&nbsp;"} : row.Cells.Add(cell)
        tAsientos.Rows.Add(row)
        row = New TableRow  'Linea de separacion
        cell = New TableCell With {.ColumnSpan = 5, .Text = "&nbsp;"} : row.Cells.Add(cell)
        tAsientos.Rows.Add(row)
    End Sub

#End Region

End Class