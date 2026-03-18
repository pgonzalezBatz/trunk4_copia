Imports System.IO

Public Class ImportarExportar
    Inherits PageBase

#Region "Page Load"

    ''' <summary>
    ''' Prepara la pagina para realizar la importacion o exportacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Master.SetTitle = "Importar/Exportar extensiones Matrici"
            pnlResulImportar.Visible = False
            pnlResulExportar.Visible = False
        End If
    End Sub

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(pnlAccion) : itzultzaileWeb.Itzuli(labelImport) : itzultzaileWeb.Itzuli(labelFichero)
            itzultzaileWeb.Itzuli(btnImportar) : itzultzaileWeb.Itzuli(labelExport) : itzultzaileWeb.Itzuli(btnExportar)
            itzultzaileWeb.TraducirWebControls(rdbAccion.Controls)
        End If
    End Sub

    ''' <summary>
    ''' Visualiza una vista u otra
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub rdbAccion_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdbAccion.SelectedIndexChanged
        If (rdbAccion.SelectedValue = 1) Then
            InicializarImportacion()
        Else
            InicializarExportacion()
        End If
    End Sub

#End Region

#Region "Importar"

    ''' <summary>
    ''' Inicializa la pagina de importacion
    ''' </summary>
    Private Sub InicializarImportacion()
        pnlResulImportar.Visible = False
        lblMensaImportar.Text = String.Empty
        btnImportar.Enabled = True
        mvAcciones.ActiveViewIndex = 0
    End Sub

    ''' <summary>
    ''' Importa el csv a la base de datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnImportar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImportar.Click
        Try
            If fuMatrici.HasFile Then
                Dim directorio As String = ConfigurationManager.AppSettings("FacturasTelefonia").ToString & "\"
                Dim fichero As String
                Dim tamañoMax As String = 10
                'Se comprueba primero que el tamaño del fichero a subir, no exceda el limite
                If (fuMatrici.PostedFile.ContentLength < (tamañoMax * 1000000)) Then
                    Try
                        If Not (Directory.Exists(directorio)) Then
                            Directory.CreateDirectory(directorio)
                        End If
                        'Se comprueba que no exista el fichero
                        fichero = directorio & Now.ToString("ddMMyyyy") & "_" & fuMatrici.FileName
                        If (File.Exists(fichero)) Then
                            Master.MensajeAdvertencia = "ficheroYaImportado"
                            log.Warn("Importacion/Exportacion Matrici: El fichero " & fichero & " ya habia sido importado")
                            Exit Sub
                        End If
                    Catch ex As Exception
                        Throw New Sablib.BatzException("errCrearDirectorio", ex)
                    End Try
                    fuMatrici.SaveAs(fichero)
                    'Una vez subido, se lee y se importa
                    Dim matriciBLL As New TelefoniaLib.BLL.MatriciComponent
                    Dim numTelefonos As Integer = matriciBLL.ImportarMatrici(fichero)
                    pnlResulImportar.Visible = True
                    lblMensaImportar.Text = "Se han importado " & numTelefonos & " telefonos de Matrici con exito"
                    log.Info("Importacion/Exportacion Matrici: Se han importado " & numTelefonos & " telefonos de Matrici con exito")
                    btnImportar.Enabled = False
                Else 'Se ha pasado de tamaño      
                    Dim smsError As String = itzultzaileWeb.Itzuli("tamañoMaximoFicheroSuperado")
                    smsError &= "(" & tamañoMax & " MB)"
                    log.Warn("Importacion/Exportacion Matrici: Se ha sobrepasado el tamaño maximo " & tamañoMax & " del fichero a subir")
                    Master.MensajeError = smsError
                End If
            End If
        Catch batzEx As Sablib.BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al importar")
        End Try
    End Sub

#End Region

#Region "Exportar"

    ''' <summary>
    ''' Inicializa la pagina de exportacion
    ''' </summary>
    Private Sub InicializarExportacion()
        pnlResulExportar.Visible = False
        lblMensaExportar.Text = String.Empty
        btnExportar.Enabled = True
        mvAcciones.ActiveViewIndex = 1
    End Sub

    ''' <summary>
    ''' Exporta la guia de telefonos de batz a un csv
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnExportar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportar.Click
        Try
            Dim matriciBLL As New TelefoniaLib.BLL.MatriciComponent            
            Dim rutaTemp As String = Server.MapPath("~") & "\Listados\Matrici\Temp\TelefonosBatz_" & Now.ToString("ddMMyyyy") & ".csv"
            Dim numTelefonos As Integer = matriciBLL.ExportarMatrici(rutaTemp, Master.Ticket.IdPlantaActual)
            pnlResulExportar.Visible = True
            abrirDocumento(rutaTemp, File.ReadAllBytes(rutaTemp))
            lblMensaExportar.Text = "Se han exportado " & numTelefonos & " telefonos de Batz con exito"
            log.Info("Importacion/Exportacion Matrici: Se han exportado " & numTelefonos & " telefonos de Batz con exito")
            btnExportar.Enabled = False
        Catch batzEx As Sablib.BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al exportar")
        End Try
    End Sub

    ''' <summary>
    ''' Abre el fichero
    ''' </summary>
    ''' <param name="fileName">Nombre del fichero</param>    
    ''' <param name="file">Fichero</param>
    Private Sub abrirDocumento(ByVal fileName As String, ByVal file As Byte())
        Try
            Dim fichero As String = fileName.Substring(fileName.LastIndexOf("\") + 1)
            Response.Clear()
            Response.AppendHeader("Content-Disposition", "attachment; filename=" & fichero)
            Response.OutputStream.Write(file, 0, file.Length)
            Response.OutputStream.Flush()
            Response.OutputStream.Close()
            Response.End()
        Catch ex As Threading.ThreadAbortException
            'Se produce siempre
        End Try
    End Sub

#End Region

End Class