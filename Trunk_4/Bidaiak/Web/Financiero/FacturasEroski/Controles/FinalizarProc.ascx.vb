Public Class FinalizarProc
    Inherits UserControl

#Region "Eventos"

    Public Event Advertencia(ByVal mensaje As String)
    Public Event ErrorGenerado(ByVal mensaje As String)
    Public Event Finalizado(ByVal idImportacion As Integer)
    Private itzultzaileWeb As New itzultzaile

#End Region

#Region "Page load"

    ''' <summary>
    ''' inicializa los controles
    ''' </summary>
    Private Sub inicializar(ByVal idPlanta As Integer)
        Dim bidaiakBLL As New BLL.BidaiakBLL
        Dim oEjec As BLL.BidaiakBLL.Ejecucion = bidaiakBLL.loadEjecucion(BLL.BidaiakBLL.TipoEjecucion.Factura_Eroski, idPlanta)
        Dim fecha As Date = New Date(oEjec.Anno, oEjec.Mes, Date.DaysInMonth(oEjec.Anno, oEjec.Mes)) '060320: Me dice Zubero que tiene que proponer la fecha de fin de mes
        txtFechaCon.Text = fecha.ToShortDateString : txtFechaEmi.Text = fecha.ToShortDateString
        txtFechaVen.Text = fecha.ToShortDateString : txtFechaFact.Text = fecha.ToShortDateString
        lblImporteTotal.Text = "0" : lblImporte.Text = "0" : lblIVA.Text = "0"
        pnlAsientosNoIntegrar.Visible = False : btnFinalizar.Enabled = True
        pnlExecuting.Style.Add("display", "none")
    End Sub

    ''' <summary>
    ''' Se traduce el control
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        itzultzaileWeb.Itzuli(labelInfo1) : itzultzaileWeb.Itzuli(labelInfo2) : itzultzaileWeb.Itzuli(pnlDatosNecesarios)
        itzultzaileWeb.Itzuli(labelFContab) : itzultzaileWeb.Itzuli(labelFEmision) : itzultzaileWeb.Itzuli(labelFVencimiento)
        itzultzaileWeb.Itzuli(btnFinalizar) : itzultzaileWeb.Itzuli(labelFFactura) : itzultzaileWeb.Itzuli(labelImporteTotal)
        itzultzaileWeb.Itzuli(labelTituloResumen) : itzultzaileWeb.Itzuli(labelEjecInfo1) : itzultzaileWeb.Itzuli(labelEjecInfo2)
        itzultzaileWeb.Itzuli(labelImporte) : itzultzaileWeb.Itzuli(labelIVA) : itzultzaileWeb.Itzuli(labelInfo3)
    End Sub

#End Region

#Region "Iniciar"

    ''' <summary>
    ''' Proceso para que se inicie el control
    ''' </summary>    
    ''' <param name="idPlanta">Id de la planta</param>
    Public Function Iniciar(ByVal idPlanta As Integer) As Boolean
        Try
            inicializar(idPlanta)
            Dim solicAgenBLL As New BLL.SolicAgenciasBLL
            Dim departBLL As New BLL.DepartamentosBLL
            Dim lFacturas As List(Of ELL.FakturaEroski) = solicAgenBLL.loadFacturasEroskiTmp(idPlanta)
            Dim lSinCuentas As List(Of ELL.AsientoContableEroskiTmp)
            Dim lAsientos As List(Of ELL.AsientoContableEroskiTmp) = departBLL.loadAsientosContEroskiTmp(idPlanta)
            Dim importeTotalFactura, importeSinIntegrar, importeFactura, IVAFactura As Decimal
            lSinCuentas = lAsientos.FindAll(Function(o) String.IsNullOrEmpty(o.Cuenta_0) And String.IsNullOrEmpty(o.Cuenta_8) And String.IsNullOrEmpty(o.Cuenta_18))
            If (lSinCuentas IsNot Nothing AndAlso lSinCuentas.Count > 0) Then
                pnlAsientosNoIntegrar.Visible = True : btnFinalizar.Enabled = False
                importeSinIntegrar = lSinCuentas.Sum(Function(o) o.BaseExe_0 + o.BaseIG_18 + o.BaseIR_8 + o.RegEsp + o.Cuota_18 + o.Cuota_8)  '+ CDec(o(13)) No se cuenta la CuotaRE
                lblNoIntegrar.Text = "Existen " & lSinCuentas.Count & " registros cuyos departamentos no estan en la estructura de Batz o no tienen cuentas asociadas y cuyo importe asciende a " & importeSinIntegrar & "<br />" _
                                   & "Hasta que no se solucionen, no se podra finalizar el proceso"
            End If
            importeTotalFactura = 0 : importeFactura = 0 : IVAFactura = 0
            'Repeater con las distintas facturas
            Dim importeAux, IVAAux As Decimal
            Dim hDistinctFact As New Hashtable
            For Each oFact As ELL.FakturaEroski In lFacturas
                importeAux = (oFact.BaseExe + oFact.BaseIG + oFact.BaseIR + oFact.RegEsp)
                IVAAux = (oFact.CuotaG + oFact.CuotaR)
                If (hDistinctFact.ContainsKey(oFact.Factura)) Then
                    hDistinctFact(oFact.Factura).Importe += importeAux
                    hDistinctFact(oFact.Factura).IVA += IVAAux
                Else
                    hDistinctFact.Add(oFact.Factura, New With {.Importe = importeAux, .IVA = IVAAux})
                End If
                importeTotalFactura += oFact.Importe
                importeFactura += importeAux
                IVAFactura += IVAAux
            Next
            lblImporteTotal.Text = importeTotalFactura
            lblImporte.Text = importeFactura
            lblIVA.Text = IVAFactura
            rptFacturas.DataSource = hDistinctFact : rptFacturas.DataBind()
            Return True
        Catch batzEx As BatzException
            RaiseEvent ErrorGenerado(itzultzaileWeb.Itzuli("Error al mostrar el resumen antes de finalizar"))
            Return False
        Catch ex As Exception
            Dim sms As String = "Error al mostrar el resumen antes de finalizar"
            PageBase.log.Error(sms, ex)
            RaiseEvent ErrorGenerado(itzultzaileWeb.Itzuli(sms))
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Se enlaza con el repeater
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub rptFacturas_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rptFacturas.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim item = e.Item.DataItem
            Dim nFactu As String = item.Key.ToString
            Dim import As Object = item.Value
            Dim lblNFactura As Label = CType(e.Item.FindControl("lblNFacturaF"), Label)
            Dim txtDocBatz As TextBox = CType(e.Item.FindControl("txtDocBatzF"), TextBox)
            Dim lblImporte As Label = CType(e.Item.FindControl("lblImporteF"), Label)
            Dim lblIVA As Label = CType(e.Item.FindControl("lblIVAF"), Label)
            Dim lblImporteTotal As Label = CType(e.Item.FindControl("lblImporteTotalF"), Label)
            Dim hfNFactura As HiddenField = CType(e.Item.FindControl("hfNFacturaF"), HiddenField)
            lblNFactura.Text = nFactu : hfNFactura.Value = nFactu
            If (nFactu.Contains("B")) Then
                lblNFactura.Text &= " (" & itzultzaileWeb.Itzuli("Servicios aereos") & ")"
            ElseIf (nFactu.Contains("S")) Then
                lblNFactura.Text &= " (" & itzultzaileWeb.Itzuli("Resto de servicios") & ")"
            End If
            If (import.Importe + import.IVA = 0) Then
                txtDocBatz.Text = "-" : txtDocBatz.Enabled = False
                txtDocBatz.ToolTip = "No va a Navision"
            Else
                txtDocBatz.Text = String.Empty : txtDocBatz.Enabled = True
            End If
            lblImporte.Text = import.Importe : lblIVA.Text = import.IVA : lblImporteTotal.Text = import.Importe + import.IVA
            End If
    End Sub

#End Region

#Region "Finalizar"

    ''' <summary>
    ''' Importa los datos de Eroski
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnFinalizarHidden_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFinalizarHidden.Click
        Finalizar_Importacion()
    End Sub

    ''' <summary>
    ''' Este evento se ejecuta despues de btnFinalizarHidden. En el evento anterior, al ser lanzado desde signalR, no puede interaccionar con la interfaz asi que se utiliza este para ello
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnFinalizar_Click(sender As Object, e As EventArgs) Handles btnFinalizar.Click
        IniciarImportacion()
    End Sub

    ''' <summary>
    ''' Inicia la importacion
    ''' </summary>
    Private Sub IniciarImportacion()
        Try
            Dim hubContext As Microsoft.AspNet.SignalR.IHubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext(Of SignalR.SignalRHub)()
            Dim hubClientContext = hubContext.Clients.Client(hfConnectionId.Value)
            Importar(hubClientContext)
        Catch ex As Exception
            PageBase.log.Error("Error al importar los datos de facturas eroski", ex)
            Session("Visas_Resul_State") = "2"
            Session("Visas_Resul_Message") = itzultzaileWeb.Itzuli("errImportarDatos")
        End Try
        btnFinalizarHidden_Click(Nothing, Nothing)
    End Sub

    ''' <summary>
    ''' Una vez ejecutado el proceso de signalR, en este evento se interacciona con la interfaz
    ''' </summary>
    Private Sub Finalizar_Importacion()
        Dim state As Integer = CType(Session("FacEroski_Resul_State"), Integer)
        Dim message As String = CType(Session("FacEroski_Resul_Message"), String)
        Session("FacEroski_Resul_State") = Nothing : Session("FacEroski_Resul_Message") = Nothing
        Select Case state
            Case 0 : RaiseEvent Finalizado(CInt(message))
            Case 1 : RaiseEvent Advertencia(message)
            Case 2 : RaiseEvent ErrorGenerado(message)
        End Select
    End Sub

    ''' <summary>
    ''' Finaliza el proceso
    ''' Guarda las facturas y realiza el asiento contable
    ''' </summary>    
    ''' <param name="hubContext">Contexto del hub para poderse comunicar con la interfaz</param>        
    Private Sub Importar(ByVal hubContext)
        Dim resul As Integer = 0
        Dim mensa As String = String.Empty
        Dim lAsientosContCab As List(Of ELL.AsientoContableCab) = getData(resul, mensa)
        Select Case resul
            Case 0 'OK
                PageBase.log.Info("FACT_AGEN (PASO 5): Se va a finalizar el proceso de importacion de facturas de agencia y generacion del asiento contable en NAVISION")
                Dim solicAgenBLL As New BLL.SolicAgenciasBLL
                '*******************************
                'TEST
                'Dim myHub As New SignalR.SignalRHub                
                'hubContext.Clients.sendMessage("Paso 1", "Realizando comprobaciones")
                'Threading.Thread.Sleep(3000)
                '*******************************
                Dim idImportacion As Integer = solicAgenBLL.ImportarFacturaEroski(lAsientosContCab, hubContext)
                PageBase.log.Info("FACT_AGEN: Ultimo paso finalizado")
                Session("FacEroski_Resul_State") = 0
                Session("FacEroski_Resul_Message") = idImportacion
            Case 1 'Faltan datos                    
                Session("FacEroski_Resul_State") = 1
                Session("FacEroski_Resul_Message") = itzultzaileWeb.Itzuli("Debe introducir todos los datos")
                RaiseEvent Advertencia(itzultzaileWeb.Itzuli("Debe introducir todos los datos"))
            Case 2 'Las cuentas no existen en Navision
                Session("FacEroski_Resul_State") = 1
                Session("FacEroski_Resul_Message") = mensa
                btnFinalizar.Enabled = False 'Al tener problema en las cuentas. Tiene que volver a empezar y para ello deshabilitamos el boton                    
                RaiseEvent Advertencia(mensa)
        End Select
    End Sub

    ''' <summary>
    ''' Obtiene los datos de la interfaz
    ''' </summary>
    ''' <param name="resul">0:Ok,1:Sin documento Batz,2:Las cuentas no existen en Navision</param>
    ''' <param name="mensa">Devolvera un texto en el caso de que devuelva un 2</param>
    ''' <returns></returns>
    Private Function getData(ByRef resul As Integer, ByRef mensa As String) As List(Of ELL.AsientoContableCab)
        Dim lAsientosCab As New List(Of ELL.AsientoContableCab)
        Dim ticket As SabLib.ELL.Ticket = CType(Session("Ticket"), SabLib.ELL.Ticket)
        Dim idPlanta As Integer = ticket.Plantas.First.Id
        Dim oAsientoCab As ELL.AsientoContableCab
        resul = 0
        'Se mira si estan informados todos los datos
        For Each item As RepeaterItem In rptFacturas.Items
            oAsientoCab = New ELL.AsientoContableCab With {.IdPlanta = idPlanta, .FechaContabilidad = CDate(txtFechaCon.Text), .FechaEmision = CDate(txtFechaEmi.Text), .FechaVencimiento = CDate(txtFechaVen.Text), .FechaFactura = CDate(txtFechaFact.Text)}
            oAsientoCab.Factura = CType(item.FindControl("hfNFacturaF"), HiddenField).Value
            oAsientoCab.DocumentoBatz = CType(item.FindControl("txtDocBatzF"), TextBox).Text
            If (oAsientoCab.DocumentoBatz = String.Empty) Then
                lAsientosCab.Clear()
                resul = 1
                Exit For 'Se sale para que le muestre el mensaje de que faltan elementos
            Else
                lAsientosCab.Add(oAsientoCab)
            End If
        Next
        'Se mira si las cuentas a insertar existen en Navision
        If (resul = 0) Then
            Dim solicAgenBLL As New BLL.SolicAgenciasBLL
            Dim lCuentasCont As List(Of Integer) = solicAgenBLL.loadAsientosContablesEroskiTmp(idPlanta)
            Dim navBLL As New BLL.NavisionBLL
            Dim mensaCuentas As String = String.Empty
            If (Not navBLL.existenCuentasContable(idPlanta, lCuentasCont, mensaCuentas)) Then
                resul = 2
                mensa = itzultzaileWeb.Itzuli("Las siguientes cuentas contables no existen en Navision") & ":" & mensaCuentas & "<br />" & itzultzaileWeb.Itzuli("Actualice las cuentas contables de la aplicacion y despues BORRE EL PROCESO y vuelva a ejecutarlo desde el principio")
            End If
        End If
        Return lAsientosCab
    End Function

#End Region

End Class