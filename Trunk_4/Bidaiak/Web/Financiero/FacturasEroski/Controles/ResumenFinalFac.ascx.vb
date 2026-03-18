Public Class ResumenFinalFac
    Inherits UserControl

#Region "Eventos"

    Public Event Advertencia(ByVal mensaje As String)
    Public Event ErrorGenerado(ByVal mensaje As String)
    Public Event Finalizado()
    Private itzultzaileWeb As New itzultzaile

    ''' <summary>
    ''' Indica el modo del control
    ''' </summary>    
    Public Enum Mode As Integer
        View = 1
        Import = 2
    End Enum

    ''' <summary>
    ''' Indicara el modo del control
    ''' Para saber si esta en modo vista o esta finalizando la importacion
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public Property Modo As Mode
        Get
            Return CType(ViewState("modo"), Mode)
        End Get
        Set(value As Mode)
            ViewState("modo") = value
        End Set
    End Property

#End Region

#Region "Iniciar"

    ''' <summary>
    ''' Se traduce el control
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        itzultzaileWeb.Itzuli(labelInfo1) : itzultzaileWeb.Itzuli(labelDocBatz) : itzultzaileWeb.Itzuli(labelInfo2)
        itzultzaileWeb.Itzuli(labelFContab) : itzultzaileWeb.Itzuli(labelFEmision) : itzultzaileWeb.Itzuli(labelFVenc)
        itzultzaileWeb.Itzuli(labelNFactura) : itzultzaileWeb.Itzuli(labelImporte) : itzultzaileWeb.Itzuli(labelIVA)
        itzultzaileWeb.Itzuli(labelImporteTotal) : itzultzaileWeb.Itzuli(labelFFact) : itzultzaileWeb.Itzuli(labelDivCab)
    End Sub

    ''' <summary>
    ''' Proceso para que se inicie el control
    ''' Si viene de un paso erroneo, el idImportacion sera menor que 0
    ''' </summary>        
    ''' <param name="idImportacion">Id de la importacion</param>
    ''' <param name="mensaje">Mensaje a mostrar</param>
    Public Function Iniciar(ByVal idImportacion As Integer, Optional ByVal mensaje As String = "") As Boolean
        Try
            pnlAsientos.Visible = False : pnlSinCuenta.Visible = False
            pnlCabecera.Visible = (Modo = Mode.Import) : labelInfo1.Visible = (Modo = Mode.Import)
            If (idImportacion > 0) Then
                If (Modo = Mode.Import) Then PageBase.log.Info("IMPORT_VISA (PASO 6): Se va a mostrar un resumen de los asientos contables de facturacion a integrar en Navision")
                hfIdImportacion.Value = idImportacion
                PintarAsientos(idImportacion, 0)
                lblResul.Text = itzultzaileWeb.Itzuli("El proceso ha finalizado con exito")
                pnlCabecera.CssClass = "alert alert-success"
                RaiseEvent Finalizado()
                Return True
            Else
                pnlCabecera.CssClass = "alert alert-danger"
                lblResul.Text = itzultzaileWeb.Itzuli(mensaje)
                Return False
            End If
        Catch batzEx As BatzException
            pnlCabecera.CssClass = "alert alert-danger"
            If (Modo = Mode.Import) Then
                lblResul.Text = itzultzaileWeb.Itzuli("La importacion se ha realizado con exito pero ha ocurrido un error al generar la visualizacion del resumen de asientos contables de facturacion de Eroski")
            Else
                lblResul.Text = itzultzaileWeb.Itzuli("Ha ocurrido un error al generar la visualizacion del resumen de asientos contables de facturas Eroski")
            End If
            RaiseEvent ErrorGenerado(batzEx.Termino)
            Return False
        Catch ex As Exception
            pnlCabecera.CssClass = "alert alert-danger"
            If (Modo = Mode.Import) Then
                lblResul.Text = itzultzaileWeb.Itzuli("La importacion se ha realizado con exito pero ha ocurrido un error al generar la visualizacion del resumen de asientos contables de facturacion de Eroski")
            Else
                lblResul.Text = itzultzaileWeb.Itzuli("Ha ocurrido un error al generar la visualizacion del resumen de asientos contables de facturas Eroski")
            End If
            RaiseEvent ErrorGenerado(lblResul.Text)
            Return False
        End Try
    End Function

#End Region

#Region "Eventos"

    ''' <summary>
    ''' Pinta los asientos
    ''' </summary>
    ''' <param name="idImportacion">Id de la importacion</param>
    ''' <param name="facturaIndex">Numero de factura a seleccionar</param>
    Private Sub PintarAsientos(ByVal idImportacion As Integer, ByVal facturaIndex As Integer)
        Try
            Dim cuentasBLL As New BLL.DepartamentosBLL
            Dim idPlanta As Integer = CInt(Session("IdPlanta"))
            Dim oCabFact As ELL.AsientoContableCab
            Dim lCabecerasFact As List(Of ELL.AsientoContableCab) = cuentasBLL.loadAsientosFactCabecera(idPlanta, idImportacion)
            lCabecerasFact = lCabecerasFact.OrderBy(Of String)(Function(o) o.Factura).ToList
            If (ddlNFactura.Items.Count = 0) Then
                Dim textoFactura As String
                For Each item In lCabecerasFact
                    textoFactura = item.Factura
                    If (item.Factura.Contains("B")) Then
                        textoFactura &= " (" & itzultzaileWeb.Itzuli("Servicios aereos") & ")"
                    ElseIf (item.Factura.Contains("S")) Then
                        textoFactura &= " (" & itzultzaileWeb.Itzuli("Resto de servicios") & ")"
                    End If
                    ddlNFactura.Items.Add(New ListItem(textoFactura, item.Factura))
                Next
                ddlNFactura.SelectedIndex = 0
                oCabFact = lCabecerasFact.First
            Else
                ddlNFactura.SelectedIndex = facturaIndex
                oCabFact = lCabecerasFact.Item(facturaIndex)
            End If
            pnlAsientos.Visible = True
            PintarDatosAsiento(oCabFact)
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("La importacion se ha realizado con exito pero ha ocurrido un error al generar la visualizacion del resumen de asientos contables de facturacion de Eroski", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Pinta los datos de cabecera de los asientos de una factura
    ''' </summary>
    ''' <param name="oCabFact">Cabecera de una factura</param>
    Private Sub PintarDatosAsiento(ByVal oCabFact As ELL.AsientoContableCab)
        lblDocBatz.Text = oCabFact.DocumentoBatz
        lblFContab.Text = oCabFact.FechaContabilidad.ToShortDateString
        lblFEmision.Text = oCabFact.FechaEmision.ToShortDateString
        lblFVenc.Text = oCabFact.FechaVencimiento.ToShortDateString
        lblImporte.Text = oCabFact.Importe
        lblIVA.Text = oCabFact.IVA
        lblImporteTotal.Text = oCabFact.ImporteTotal
        lblFFactura.Text = oCabFact.FechaFactura.ToShortDateString
        Dim cuentasBLL As New BLL.DepartamentosBLL
        Dim lAsientosFact As List(Of ELL.AsientoContableCab.Linea) = cuentasBLL.loadAsientosFactLineas(oCabFact.Id)
        If (lAsientosFact IsNot Nothing AndAlso lAsientosFact.Count > 0) Then
            Dim lAsientosFactCuenta As List(Of ELL.AsientoContableCab.Linea) = lAsientosFact.FindAll(Function(o) Not String.IsNullOrEmpty(o.Cuenta))
            lAsientosFactCuenta = lAsientosFactCuenta.OrderBy(Of Integer)(Function(o) o.Linea).ToList
            gvAsientos.DataSource = lAsientosFactCuenta
            gvAsientos.DataBind()
            Dim lblTotalImporte As Label = CType(gvAsientos.FooterRow.Cells(3).FindControl("lblTotalImporte"), Label)
            Dim lblTotalIva As Label = CType(gvAsientos.FooterRow.Cells(4).FindControl("lblTotalIva"), Label)
            lblTotalImporte.Text = lAsientosFactCuenta.Sum(Function(o) o.Importe)
            lblTotalIva.Text = lAsientosFactCuenta.Sum(Function(o) o.IVA)
            Dim lAsientosSinIntegrar As List(Of ELL.AsientoContableCab.Linea) = lAsientosFact.FindAll(Function(o) String.IsNullOrEmpty(o.Cuenta))
            If (lAsientosSinIntegrar IsNot Nothing AndAlso lAsientosSinIntegrar.Count > 0) Then
                pnlSinCuenta.Visible = True
                gvSinCuenta.DataSource = lAsientosSinIntegrar
                gvSinCuenta.DataBind()
                Dim lblTotalImporteSinCta As Label = CType(gvSinCuenta.FooterRow.Cells(1).FindControl("lblTotalImporte"), Label)
                lblTotalImporteSinCta.Text = lAsientosSinIntegrar.Sum(Function(o) o.Importe)
            End If
        Else
            gvAsientos.DataSource = Nothing : gvAsientos.DataBind()
        End If
    End Sub

    ''' <summary>
    ''' Se enlazan los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvAsientos_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvAsientos.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oAsiento As ELL.AsientoContableCab.Linea = e.Row.DataItem
            Dim tipoIva As String = String.Empty
            Dim epsilonBLL As New BLL.Epsilon(CInt(Session("IdPlanta")))
            CType(e.Row.FindControl("lblLinea"), Label).Text = oAsiento.Linea
            CType(e.Row.FindControl("lblCuenta"), Label).Text = oAsiento.Cuenta
            Select Case oAsiento.TipoIVA
                Case 0 : tipoIva = itzultzaileWeb.Itzuli("Normal")
                Case 1 : tipoIva = itzultzaileWeb.Itzuli("Reducido")
                Case 2 : tipoIva = itzultzaileWeb.Itzuli("Exento")
            End Select
            CType(e.Row.FindControl("lblTipoIva"), Label).Text = tipoIva
            CType(e.Row.FindControl("lblImporte"), Label).Text = oAsiento.Importe
            CType(e.Row.FindControl("lblIva"), Label).Text = oAsiento.IVA
            Dim info As String() = epsilonBLL.getInfoOrdenDepartamento(oAsiento.CodigoDepartamento)
            CType(e.Row.FindControl("lblDepartamento"), Label).Text = info(5)
            If (info(0) = "00985") Then 'Sistemas
                CType(e.Row.FindControl("lblOrganizacion"), Label).Text = info(3)
            End If
            CType(e.Row.FindControl("lblLantegi"), Label).Text = epsilonBLL.getInfoLantegi(oAsiento.CodigoDepartamento)
        End If
    End Sub

    ''' <summary>
    ''' Se enlazan los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvSinCuenta_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvSinCuenta.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oAsiento As ELL.AsientoContableCab.Linea = e.Row.DataItem
            Dim depBLL As SabLib.BLL.DepartamentosComponent = New SabLib.BLL.DepartamentosComponent
            Dim oDept As SabLib.ELL.Departamento = Nothing
            If (Not String.IsNullOrEmpty(oAsiento.CodigoDepartamento)) Then  'Si no es nulo el codigo de departamento
                oDept = depBLL.GetDepartamento(New SabLib.ELL.Departamento With {.Id = oAsiento.CodigoDepartamento, .IdPlanta = CInt(Session("IdPlanta"))})
            End If
            CType(e.Row.FindControl("lblDepart"), Label).Text = If(oDept Is Nothing, oAsiento.CodigoDepartamento, oDept.Nombre)
            CType(e.Row.FindControl("lblImporte"), Label).Text = oAsiento.Importe
        End If
    End Sub

    ''' <summary>
    ''' Se selecciona una factura de la que se tendran que mostrar los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ddlNFactura_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlNFactura.SelectedIndexChanged
        Try
            PintarAsientos(CInt(hfIdImportacion.Value), ddlNFactura.SelectedIndex)
        Catch ex As Exception
            pnlCabecera.CssClass = "alert alert-danger"
            lblResul.Text = itzultzaileWeb.Itzuli("Ha ocurrido un error al visualizar el resumen de asientos contables de la facturacion de Eroski seleccionada")
            RaiseEvent ErrorGenerado(lblResul.Text)
        End Try
    End Sub

#End Region

End Class