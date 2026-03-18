Public Class DepartamentosPersonas
    Inherits PageBase

#Region "Page Load"

    ''' <summary>
    ''' Se carga la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Master.SetTitle = "Listado de departamentos y personas"
        End If
    End Sub

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(labelInfo) : itzultzaileWeb.Itzuli(btnExportar)
        End If
    End Sub

#End Region

#Region "Exportar"

    ''' <summary>
    ''' Exporta el listado de departamentos y personas a un fichero csv
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
        Dim rutaTemp As String = String.Empty
        Try
            Dim bidaiakBLL As New BLL.BidaiakBLL
            rutaTemp = ConfigurationManager.AppSettings("Documentos") & "\DeptoPersonas_" & Now.ToString("ddMMyyyy") & ".csv"
            Dim numReg As Integer = bidaiakBLL.ExportarDepartamentosPersonas(rutaTemp, Master.IdPlantaGestion)
            Master.MensajeInfo = "Se han exportado " & numReg & " registros de Batz con exito"
            log.Info("Se han exportado " & numReg & " registros del listado de departamentos-personas")
            btnExportar.Enabled = False
        Catch batzEx As SabLib.BatzException
            Master.MensajeError = batzEx.Termino
            Exit Sub
        Catch ex As Exception
            log.Error("Error al exportar el listado de departamentos personas", ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al exportar")
            Exit Sub
        End Try
        abrirDocumento(rutaTemp, IO.File.ReadAllBytes(rutaTemp))
    End Sub

    ''' <summary>
    ''' Abre el fichero
    ''' </summary>
    ''' <param name="fileName">Nombre del fichero</param>    
    ''' <param name="file">Fichero</param>
    Private Sub abrirDocumento(ByVal fileName As String, ByVal file As Byte())
        Response.Clear() : Response.ClearHeaders() : Response.ClearContent()
        Response.Buffer = True
        Response.AppendHeader("Content-Disposition", "attachment; filename=" & fileName)
        Response.ContentType = "text/csv"
        Response.BinaryWrite(file)
        Response.End()
    End Sub

#End Region

End Class