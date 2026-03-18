Imports BidaiakLib

Public Class ImportarVisas
    Inherits System.Web.UI.UserControl

#Region "Eventos"

    Public Event Advertencia(ByVal mensaje As String)
    Public Event ErrorGenerado(ByVal mensaje As String)
    Public Event Finalizado(ByVal numRegistros As Integer, ByVal numGestionados As Integer)
    Private itzultzaileWeb As New Itzultzaile

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
        pnlImportar.Visible = True : pnlYaImportado.Visible = False
        lblResul.Text = String.Empty
    End Sub

    ''' <summary>
    ''' Proceso para que se inicie el control
    ''' </summary>    
    ''' <param name="año">Año de la ejecucion</param>
    ''' <param name="mes">Mes de la ejecucion</param>
    Public Sub Iniciar(ByVal año As Integer, ByVal mes As Integer)
        inicializar()
        hfAnno.Value = año : hfMes.Value = mes
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

#Region "Importar"

    ''' <summary>
    ''' Importa los datos de las visas
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
        '    PageBase.log.Error("Error al importar los datos de visas", ex)
        '    Session("Visas_Resul_State") = "2"
        '    Session("Visas_Resul_Message") = itzultzaileWeb.Itzuli("errImportarDatos")
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
    ''' ''' Este evento se ejecuta despues de btnFinalizarHidden. En el evento anterior, al ser lanzado desde signalR, no puede interaccionar con la interfaz asi que se utiliza este para ello
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
        Dim state As Integer = CType(Session("Visas_Resul_State"), Integer)
        Dim message As String = CType(Session("Visas_Resul_Message"), String)
        Session("Visas_Resul_State") = Nothing : Session("Visas_Resul_Message") = Nothing
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
        Dim visaBLL As New BLL.VisasBLL
        Dim bidaiak As New BLL.BidaiakBLL
        PageBase.log.Info("IMPORT_VISA (PASO 2): Se va a proceder a importar el fichero de visas a una tabla temporal")
        Dim importacion As BLL.BidaiakBLL.Importacion = bidaiak.loadImportacionDoc(1, CInt(hfAnno.Value), CInt(hfMes.Value), IdPlanta)
        'NAV:Cuando se quiera volver a procesar un fichero ya subido
        'NAV:importacion = Nothing
        If (importacion IsNot Nothing) Then
            Dim sms As String = itzultzaileWeb.Itzuli("El fichero de visas del año y mes seleccionado ya habia sido subido anteriormente asi que la importacion ha sido cancelada")
            PageBase.log.Info("IMPORT_VISA: El fichero de visas del año y mes seleccionado ya habia sido importado anteriormente asi que la importacion ha sido cancelada")
            Session("Visas_Resul_State") = 1
            Session("Visas_Resul_Message") = sms
        Else  'No consta como importado en la tabla de imporaciones
            Dim lineaRepetida As String = String.Empty
            Dim myVisa As ArrayList = visaBLL.ImportarVisasTmp(IdPlanta, hubContext, lineaRepetida)
            If (myVisa IsNot Nothing) Then
                If (myVisa.Count = 0) Then 'El fichero ya estaba importado, no se ha importado ningun registro
                    Dim sms As String = itzultzaileWeb.Itzuli("El fichero ya habia sido subido anteriormente asi que la importacion ha sido cancelada")
                    sms &= " (" & lineaRepetida & ")"
                    PageBase.log.Info("IMPORT_VISA: El fichero ya habia sido importado anteriormente asi que la importacion ha sido cancelada (" & lineaRepetida & ")")
                    Session("Visas_Resul_State") = 1
                    Session("Visas_Resul_Message") = sms
                Else
                    Dim numRegistros As Integer = CInt(myVisa.Item(0)) + CInt(myVisa.Item(1))
                    PageBase.log.Info("IMPORT_VISA: Importacion realizada con exito.  [Num Registros=" & numRegistros & " | Asociados a viajes=" & myVisa.Item(0) & " | Sin viaje asociado=" & myVisa.Item(1) & " | No tratados (excepcion visa)=" & myVisa.Item(2) & "]")
                    Session("Visas_Resul_State") = 0
                    Session("Visas_Resul_Message") = myVisa.Item(0) & "_" & myVisa.Item(1)
                End If
            Else
                Session("Visas_Resul_State") = 2
                Session("FacEroski_Resul_Message") = itzultzaileWeb.Itzuli("Error al importar") & ". " & itzultzaileWeb.Itzuli("Contacte con el administrador")
            End If
        End If
    End Sub

#End Region

End Class