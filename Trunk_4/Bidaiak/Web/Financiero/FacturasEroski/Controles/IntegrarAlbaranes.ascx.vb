Imports BidaiakLib
Public Class IntegrarAlbaranes
    Inherits UserControl

#Region "Propiedades"

    ''' <summary>
    ''' Id de la planta actual
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Private Property IdPlanta As Integer
        Get
            Return CInt(Session("IdPlanta"))
        End Get
        Set(ByVal value As Integer)
            Session("IdPlanta") = value
        End Set
    End Property

#End Region

#Region "Eventos"

    Public Event Advertencia(ByVal mensaje As String)
    Public Event ErrorGenerado(ByVal mensaje As String)
    Public Event Finalizado(ByVal numRegistros As Integer, ByVal numGestionados As Integer)
    Private itzultzaileWeb As New Itzultzaile

#End Region

#Region "Page load"

    ''' <summary>
    ''' Carga la pagina al principio
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            inicializar()
        End If
    End Sub

    ''' <summary>
    ''' inicializa los controles
    ''' </summary>
    Private Sub inicializar()
        pnlImportar.Visible = True
        pnlYaImportado.Visible = False
        lblResul.Text = String.Empty
    End Sub


    ''' <summary>
    ''' Se traduce el control
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        itzultzaileWeb.Itzuli(labelInfo) : itzultzaileWeb.Itzuli(lnkImportar)
        itzultzaileWeb.Itzuli(imgImportar) : itzultzaileWeb.Itzuli(lblMensa)
        itzultzaileWeb.Itzuli(labelProcesando)
    End Sub

#End Region

#Region "Iniciar"

    ''' <summary>
    ''' Proceso para que se inicie el control
    ''' </summary>    
    ''' <param name="año">Año de la ejecucion</param>
    ''' <param name="mes">Mes de la ejecucion</param>
    Public Sub Iniciar(ByVal año As Integer, ByVal mes As Integer)
        inicializar()
        hfAnno.Value = año : hfMes.Value = mes
    End Sub

#End Region

#Region "Importar"

    ''' <summary>
    ''' Importa los datos de las facturas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnImportarHidden_Click(sender As Object, e As EventArgs) Handles btnImportarHidden.Click
        Finalizar_Importacion()
        'Try
        '    Dim hubContext As Microsoft.AspNet.SignalR.IHubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext(Of SignalR.SignalRHub)()
        '    Dim hubClientContext = hubContext.Clients.Client(hfConnectionId.Value)
        '    Importar(hubClientContext)
        'Catch ex As Exception
        '    PageBase.log.Error("Error al importar los datos", ex)
        '    RaiseEvent ErrorGenerado(itzultzaileWeb.Itzuli("errImportarDatos"))
        'End Try
    End Sub

    ''' <summary>
    ''' Este evento se ejecuta despues de btnFinalizarHidden. En el evento anterior, al ser lanzado desde signalR, no puede interaccionar con la interfaz asi que se utiliza este para ello
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub lnkImportar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkImportar.Click
        IniciarImportacion()
    End Sub

    ''' <summary>
    ''' Este evento se ejecuta despues de btnFinalizarHidden. En el evento anterior, al ser lanzado desde signalR, no puede interaccionar con la interfaz asi que se utiliza este para ello
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub imgImportar_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles imgImportar.Click
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
            PageBase.log.Error("Error al importar los datos de visas", ex)
            Session("Visas_Resul_State") = "2"
            Session("Visas_Resul_Message") = itzultzaileWeb.Itzuli("errImportarDatos")
        End Try
        btnImportarHidden_Click(Nothing, Nothing)
    End Sub

    ''' <summary>
    ''' Una vez ejecutado el proceso de signalR, en este evento se interacciona con la interfaz
    ''' </summary>
    Private Sub Finalizar_Importacion()
        Dim state As Integer = CType(Session("FacEroski_Resul_State"), Integer)
        Dim message As String = CType(Session("FacEroski_Resul_Message"), String)
        Session("FacEroski_Resul_State") = Nothing : Session("FacEroski_Resul_Message") = Nothing
        Select Case state
            Case 0
                Dim text As String() = message.Split("_")
                RaiseEvent Finalizado(CInt(text(0)), CInt(text(1)))
            Case 1
                lblResul.Text = message
                pnlYaImportado.Visible = True
                pnlImportar.Visible = False
            Case 2 : RaiseEvent ErrorGenerado(message)
        End Select
    End Sub

    ''' <summary>
    ''' Realiza el proceso de importacion
    ''' </summary>    
    ''' <param name="hubContext">Contexto del hub para poderse comunicar con la interfaz</param>        
    Private Sub Importar(ByVal hubContext)
        Dim solAgenciaBLL As New BLL.SolicAgenciasBLL
        Dim bidaiak As New BLL.BidaiakBLL
        PageBase.log.Info("FACT_AGEN (PASO 2): Se va a proceder a importar el fichero de agencia de facturas/albaranes a una tabla temporal")
        Dim oImportacion As BLL.BidaiakBLL.Importacion = bidaiak.loadImportacionDoc(2, CInt(hfAnno.Value), CInt(hfMes.Value), IdPlanta)
        'NAV:Cuando se quiera volver a procesar un fichero ya subido
        'NAV:importacion = Nothing
        If (oImportacion IsNot Nothing) Then
            Dim sms As String = itzultzaileWeb.Itzuli("El fichero de facturas Eroski del año y mes seleccionado ya habia sido subido anteriormente asi que la importacion ha sido cancelada")
            PageBase.log.Info("IMPORT_VISA: El fichero de facturas Eroski del año y mes seleccionado ya habia sido importado anteriormente asi que la importacion ha sido cancelada")
            Session("FacEroski_Resul_State") = 1
            Session("FacEroski_Resul_Message") = sms
        Else  'No consta como importado en la tabla de importaciones
            Dim myFactura As ArrayList = solAgenciaBLL.ImportarFacturaEroskiTmp(IdPlanta, hubContext)
            If (myFactura IsNot Nothing) Then
                If (myFactura.Item(0) = 0) Then 'El fichero ya estaba importado, no se ha importado ningun registro
                    Dim sms As String = itzultzaileWeb.Itzuli("El fichero ya habia sido subido anteriormente asi que la importacion ha sido cancelada")
                    PageBase.log.Info("FACT_AGEN: El fichero ya habia sido importado anteriormente asi que la importacion ha sido cancelada")
                    Session("FacEroski_Resul_State") = 1
                    Session("FacEroski_Resul_Message") = sms
                Else
                    PageBase.log.Info("FACT_AGEN: Importacion realizada con exito. Numero de registros: " & myFactura.Item(0) & " | Numero gestionados ok: " & myFactura.Item(1))
                    Session("FacEroski_Resul_State") = 0
                    Session("FacEroski_Resul_Message") = myFactura.Item(0) & "_" & myFactura.Item(1)
                End If
            Else
                Session("FacEroski_Resul_State") = 2
                Session("FacEroski_Resul_Message") = itzultzaileWeb.Itzuli("Error al importar") & ". " & itzultzaileWeb.Itzuli("Contacte con el administrador")
            End If
        End If
    End Sub

#End Region

End Class