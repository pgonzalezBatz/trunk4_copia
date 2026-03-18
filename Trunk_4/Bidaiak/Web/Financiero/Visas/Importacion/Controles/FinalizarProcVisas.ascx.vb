Imports BidaiakLib.BLL

Public Class FinalizarProcVisas
    Inherits UserControl

#Region "Eventos"

    Public Event Advertencia(ByVal mensaje As String)
    Public Event ErrorGenerado(ByVal mensaje As String)
    Public Event Finalizado(ByVal numTarjetasAsig As Integer, ByVal idImportacion As Integer)
    Private itzultzaileWeb As New itzultzaile

#End Region

#Region "Propiedades"

    ''' <summary>
    ''' Id de la planta actual
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Private ReadOnly Property IdPlanta As Integer
        Get
            Return CInt(Session("IdPlanta"))
        End Get
    End Property

#End Region

#Region "Iniciar"

    ''' <summary>
    ''' Inicia la pantalla
    ''' </summary>    
    Public Function iniciar() As Boolean
        Try
            Dim visasBLL As New BLL.VisasBLL
            Dim gastos As Decimal() = visasBLL.loadImporteTotalVisaConSinViajeTmp(IdPlanta)
            lblGConVisa.Text = gastos(0) & " €"
            lblGSinVisa.Text = gastos(1) & " €"
            lblGCuotaVisa.Text = gastos(2) & " €"
            lblGVExcepcion.Text = gastos(3) & " €"
            pnlExecuting.Style.Add("display", "none")
            PageBase.log.Info("IMPORT_VISA:Gastos asociados a viajes: " & gastos(0) & " | Gastos sin asociar a ningun viaje:" & gastos(1) & " | Gastos de cuotas: " & gastos(2) & " | Gastos de excepcion: " & gastos(3))
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
    ''' Se traduce el control
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        itzultzaileWeb.Itzuli(labelInfo1) : itzultzaileWeb.Itzuli(labelInfo2) : itzultzaileWeb.Itzuli(labelInfo3)
        itzultzaileWeb.Itzuli(labelGVViaje) : itzultzaileWeb.Itzuli(labelGVSinViaje) : itzultzaileWeb.Itzuli(btnFinalizar)
        itzultzaileWeb.Itzuli(labelInfo4) : itzultzaileWeb.Itzuli(labelInfo5) : itzultzaileWeb.Itzuli(labelInfo6)
        itzultzaileWeb.Itzuli(labelTituloResumen) : itzultzaileWeb.Itzuli(labelEjecInfo1) : itzultzaileWeb.Itzuli(labelEjecInfo2)
        itzultzaileWeb.Itzuli(labelGVCuotas) : itzultzaileWeb.Itzuli(labelDocBatz) : itzultzaileWeb.Itzuli(labelGVExcepcion)
    End Sub

#End Region

#Region "Finalizar"

    ''' <summary>
    ''' Finaliza el proceso
    ''' Guarda las facturas y realiza el asiento contable
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnFinalizar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFinalizar.Click
        Try
            PageBase.log.Info("IMPORT_VISA (PASO 5): Se va a finalizar el proceso de importacion de movimientos de visa y generacion del asiento contable")
            Dim hubContext As Microsoft.AspNet.SignalR.IHubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext(Of SignalR.SignalRHub)()
            Dim hubClientContext = hubContext.Clients.Client(hfConnectionId.Value)
            Dim resul As Integer = 0
            Dim mensa As String = String.Empty
            getData(resul, mensa)
            Select Case resul
                Case 0 'Todo OK
                    Dim numTarjetasAsig As Integer = 0
                    Dim visasBLL As New BLL.VisasBLL
                    Dim lVisasTmp As List(Of String()) = visasBLL.loadVisasTmp(IdPlanta)
                    Dim lVisasResul = From visa As String() In lVisasTmp Where CInt(visa(16)) = 1 Select visa(1) Distinct  'Se seleccionan las que hay que asignar y que tengan distintos numeros de tarjetas
                    If (lVisasResul IsNot Nothing) Then numTarjetasAsig = lVisasResul.Count
                    Dim idImportacion As Integer = visasBLL.ImportarVisas(IdPlanta, txtDocBatz.Text, hubClientContext)
                    PageBase.log.Info("IMPORT_VISA: Ultimo paso finalizado con el idImportacion " & idImportacion)
                    Session("Visas_Resul_State") = 0
                    Session("Visas_Resul_Message") = idImportacion & "_" & numTarjetasAsig
                    'RaiseEvent Finalizado(numTarjetasAsig, idImportacion)
                Case 1 'Faltan datos
                    Session("Visas_Resul_State") = 1
                    Session("Visas_Resul_Message") = itzultzaileWeb.Itzuli("Debe introducir el documento de Batz")
                Case 2 'Las cuentas no existen en Navision
                    Session("Visas_Resul_State") = 1
                    Session("Visas_Resul_Message") = mensa
                    btnFinalizar.Enabled = False 'Al tener problema en las cuentas. Tiene que volver a empezar y para ello deshabilitamos el boton
            End Select
        Catch batzEx As BatzException
            PageBase.log.Error("IMPORT_VISA: El proceso ha fallado:" & batzEx.Termino)
            Session("Visas_Resul_State") = 2
            Session("Visas_Resul_Message") = batzEx.Termino
        End Try
        btnFinalizarHidden_Click(Nothing, Nothing)
    End Sub

    ''' <summary>
    ''' Este evento se ejecuta despues de btnFinalizarHidden. En el evento anterior, al ser lanzado desde signalR, no puede interaccionar con la interfaz asi que se utiliza este para ello
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnFinalizarHidden_Click(sender As Object, e As EventArgs) Handles btnFinalizarHidden.Click
        Dim state As Integer = CType(Session("Visas_Resul_State"), Integer)
        Dim message As String = CType(Session("Visas_Resul_Message"), String)
        Session("Visas_Resul_State") = Nothing : Session("Visas_Resul_Message") = Nothing
        Select Case state
            Case 0
                Dim idImportacion, numTarjetasAsignadas As Integer
                Dim datos As String() = message.Split("_")
                idImportacion = CInt(datos(0))
                numTarjetasAsignadas = CInt(datos(1))
                RaiseEvent Finalizado(numTarjetasAsignadas, idImportacion)
            Case 1 : RaiseEvent Advertencia(message)
            Case 2 : RaiseEvent ErrorGenerado(message)
        End Select
    End Sub

    ''' <summary>
    ''' Obtiene los datos de la interfaz
    ''' </summary>
    ''' <param name="resul">0:Ok,1:Sin documento Batz,2:Las cuentas no existen en Navision</param>    
    ''' <param name="mensa">Devolvera un texto en el caso de que devuelva un 2</param>    
    Private Sub getData(ByRef resul As Integer, ByRef mensa As String)
        Dim lAsientosCab As New List(Of ELL.AsientoContableCab)
        Dim ticket As SabLib.ELL.Ticket = CType(Session("Ticket"), SabLib.ELL.Ticket)
        Dim idPlanta As Integer = ticket.Plantas.First.Id
        resul = 0
        If (txtDocBatz.Text = String.Empty) Then
            resul = 1
        Else 'Se mira si las cuentas a insertar existen en Navision                        
            Dim cuentasBLL As New BLL.DepartamentosBLL
            Dim lAsientosRegister As List(Of String())
            lAsientosRegister = cuentasBLL.loadAsientosContVisasTmp(idPlanta)
            Dim lCuentasContAux As List(Of String) = (From asiento In lAsientosRegister Select asiento(8)).Distinct.ToList
            Dim lCuentasCont As New List(Of Integer)
            lCuentasContAux.ForEach(Sub(o) lCuentasCont.Add(CInt(o)))
            'For Each asiento In lAsientosRegister
            'If (Not lCuentasCont.Exists(Function(o) o = CInt(asiento(8)))) Then lCuentasCont.Add(CInt(asiento(8)))
            'Next            
            Dim navBLL As New BLL.NavisionBLL
            Dim mensaCuentas As String = String.Empty
            If (Not navBLL.existenCuentasContable(idPlanta, lCuentasCont, mensaCuentas)) Then
                resul = 2
                mensa = itzultzaileWeb.Itzuli("Las siguientes cuentas contables no existen en Navision") & ":" & mensaCuentas & "<br />" & itzultzaileWeb.Itzuli("Actualice las cuentas contables de la aplicacion y despues BORRE EL PROCESO y vuelva a ejecutarlo desde el principio")
            End If
        End If
    End Sub

#End Region

End Class