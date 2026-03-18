Public Class ResumenFinal
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
        itzultzaileWeb.Itzuli(labelContrapartida) : itzultzaileWeb.Itzuli(labelContrapartida2)
        itzultzaileWeb.Itzuli(labelInfo1) : itzultzaileWeb.Itzuli(labelInfo2) : itzultzaileWeb.Itzuli(labelCuota)
        itzultzaileWeb.Itzuli(labelCuenta) ': itzultzaileWeb.Itzuli(labelCuentaVisaExcep) : itzultzaileWeb.Itzuli(labelLantegiVisaExcep)
        itzultzaileWeb.Itzuli(labelImporteVisaExcep)
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
                If (Modo = Mode.Import) Then PageBase.log.Info("IMPORT_VISA (PASO 6): Se va a mostrar un resumen de los asientos contables de visa a integrar en Navision")
                PintarAsientos(idImportacion)
                lblResul.Text = itzultzaileWeb.Itzuli("El proceso ha finalizado con exito") & ". " & itzultzaileWeb.Itzuli("Se han relacionado todas las tarjetas")
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
                lblResul.Text = itzultzaileWeb.Itzuli("La importacion se ha realizado con exito pero ha ocurrido un error al generar la visualizacion del resumen de asientos contables de visa")
            Else
                lblResul.Text = itzultzaileWeb.Itzuli("Ha ocurrido un error al generar la visualizacion del resumen de asientos contables de visa")
            End If
            RaiseEvent ErrorGenerado(batzEx.Termino)
            Return False
        Catch ex As Exception
            pnlCabecera.CssClass = "alert danger-success"
            If (Modo = Mode.Import) Then
                lblResul.Text = itzultzaileWeb.Itzuli("La importacion se ha realizado con exito pero ha ocurrido un error al generar la visualizacion del resumen de asientos contables de visa")
            Else
                lblResul.Text = itzultzaileWeb.Itzuli("Ha ocurrido un error al generar la visualizacion del resumen de asientos contables de visa")
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
    Private Sub PintarAsientos(ByVal idImportacion As Integer)
        Try
            Dim cuentasBLL As New BLL.DepartamentosBLL
            Dim idPlanta As Integer = CInt(Session("IdPlanta"))
            '0:ID,1:CUENTA,2:IMPORTE,3:FECHA_INSERCION,4:ID_PLANTA,5:ID_IMPORTACION,6:COD_DEPART,7:TIPO,8:LANTEGI
            Dim lCuentasAll As List(Of String()) = cuentasBLL.loadAsientosContVisas(idPlanta, idImportacion)
            'Con departamento
            If (lCuentasAll IsNot Nothing AndAlso lCuentasAll.Count > 0) Then
                Dim cuentasConDepart As List(Of String()) = lCuentasAll.FindAll(Function(o As String()) o(1) <> String.Empty AndAlso o(7) = "0" AndAlso o(6) <> String.Empty) 'Omitimos el asiento de contrapartida
                pnlAsientos.Visible = True
                'Hay que contar que puede venir un asiento de contrapartida y otro de contrapartida de cuota
                lblContrapartida.Text = cuentasConDepart.Sum(Function(o As String()) PageBase.DecimalValue(o(2))) & " €"
                gvAsientos.DataSource = cuentasConDepart
                gvAsientos.DataBind()
                If (Modo = Mode.Import) Then PageBase.log.Info("IMPORT_VISA (PASO 6): Importe de la cuenta de contrapartida:" & lblContrapartida.Text)
            End If
            'Cuotas
            Dim lCuentasCuota As List(Of String()) = lCuentasAll.FindAll(Function(o As String()) o(7) = "1" AndAlso o(6) <> String.Empty) 'Omitimos el asiento de cuota de contrapartida
            If (lCuentasCuota IsNot Nothing AndAlso lCuentasCuota.Count > 0) Then
                lblCuota.Text = lCuentasCuota.Sum(Function(o As String()) PageBase.DecimalValue(o(2))) & " €"
                lblCtaCuota.Text = lCuentasCuota(0)(1)  'Se coge la cuenta del primero que sera la misma para todos
                If (Modo = Mode.Import) Then PageBase.log.Info("IMPORT_VISA (PASO 6): Importe de la cuenta de cuotas:" & lblCuota.Text)
            Else
                lblCuota.Text = "0 €"
                lblCtaCuota.Text = String.Empty
            End If
            'Visas Excepcion
            Dim movBLL As New BLL.VisasBLL
            Dim lMovs As List(Of ELL.Visa.Movimiento) = movBLL.loadMovimientosExcepcion(New ELL.Visa.Movimiento With {.IdImportacion = idImportacion}, CInt(Session("IdPlanta")))
            If (lMovs.Count > 0) Then lMovs.Sort(Function(o1, o2) o1.NumTarjeta < o2.NumTarjeta)
            lblImporteVisaExcep.Text = PageBase.DecimalValue(lMovs.Sum(Function(o) o.ImporteEuros)) & " €"
            'Dim lCuentasExcep As List(Of String()) = lCuentasAll.FindAll(Function(o As String()) o(7) = "2")
            'If (lCuentasExcep IsNot Nothing AndAlso lCuentasExcep.Count = 1) Then
            '    lblImporteVisaExcep.Text = lCuentasExcep.Sum(Function(o As String()) PageBase.DecimalValue(o(2))) & " €"
            '    'lblCuentaVisaExcep.Text = lCuentasExcep(0)(1)
            '    'lblLantegiVisaExcep.Text = lCuentasExcep(0)(8)
            '    If (Modo = Mode.Import) Then PageBase.log.Info("IMPORT_VISA (PASO 6): Importe de la cuenta de visas de excepcion:" & lblImporteVisaExcep.Text)
            'Else
            '    lblImporteVisaExcep.Text = "0 €"
            '    'lblCuentaVisaExcep.Text = String.Empty : lblLantegiVisaExcep.Text = String.Empty
            'End If
            'Sin departamento
            Dim lCuentasSinDepart As List(Of String()) = lCuentasAll.FindAll(Function(o As String()) o(1) = String.Empty AndAlso o(7) = "0" AndAlso o(6) <> String.Empty)
            If (lCuentasSinDepart IsNot Nothing AndAlso lCuentasSinDepart.Count > 0) Then
                pnlSinCuenta.Visible = True
                lblContrapartida2.Text = lCuentasSinDepart.Sum(Function(o As String()) PageBase.DecimalValue(o(2))) & " €"
                gvSinCuenta.DataSource = lCuentasSinDepart
                gvSinCuenta.DataBind()
                If (Modo = Mode.Import) Then PageBase.log.Info("IMPORT_VISA (PASO 6): Existen departamentos que ya no existen. Importe de lo que no ira a la cuenta de contrapartida:" & lblContrapartida2.Text)
            End If
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            If (Modo = Mode.Import) Then
                Throw New BatzException("La importacion se ha realizado con exito pero ha ocurrido un error al generar la visualizacion del resumen de asientos contables de visa", ex)
            Else
                Throw New BatzException("Ha ocurrido un error al generar la visualizacion del resumen de asientos contables de visa", ex)
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Se enlazan los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvAsientosCuotas_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvAsientos.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim sAsiento As String() = e.Row.DataItem
            Dim epsilonBLL As New BLL.Epsilon(CInt(Session("IdPlanta")))
            Dim info As String() = epsilonBLL.getInfoOrdenDepartamento(sAsiento(6))
            CType(e.Row.FindControl("lblDepart"), Label).Text = If(info(5) IsNot Nothing AndAlso info(5) <> String.Empty, info(5), sAsiento(6))
            If (info(0) = "00985") Then CType(e.Row.FindControl("lblOrganizacion"), Label).Text = info(3) 'Unicamente si es de sistemas, se pintara la organizacion                        
            CType(e.Row.FindControl("lblCuenta"), Label).Text = sAsiento(1) : CType(e.Row.FindControl("lblImporte"), Label).Text = sAsiento(2) & " €"
            CType(e.Row.FindControl("lblLantegi"), Label).Text = epsilonBLL.getInfoLantegi(sAsiento(6))
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
            Dim sAsiento As String() = e.Row.DataItem
            Dim depBLL As SabLib.BLL.DepartamentosComponent = New SabLib.BLL.DepartamentosComponent
            Dim oDept As SabLib.ELL.Departamento = depBLL.GetDepartamento(New SabLib.ELL.Departamento With {.Id = CInt(sAsiento(6)), .IdPlanta = CInt(Session("IdPlanta"))})
            CType(e.Row.FindControl("lblDepart"), Label).Text = If(oDept Is Nothing, sAsiento(6), oDept.Nombre)
            CType(e.Row.FindControl("lblImporte"), Label).Text = sAsiento(2) & " €"
        End If
    End Sub

#End Region

End Class