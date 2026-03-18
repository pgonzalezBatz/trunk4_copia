Imports Ionic.Zip

Public Class ConsultarNominasAdmin
    Inherits PageBase

    'Private Declare Auto Function LogonUser Lib "advapi32.dll" (ByVal un As String, ByVal domain As String, ByVal pw As String, ByVal LogonType As Integer, ByVal LogonProvider As Integer, ByRef Token As IntPtr) As Boolean
    'Public Declare Auto Function CloseHandle Lib "kernel32.dll" (ByVal handle As IntPtr) As Boolean

#Region "Page Load"

    ''' <summary>
    ''' Carga la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Consulta de nominas"
                WriteLog("Usuario del hilo:" & System.Security.Principal.WindowsIdentity.GetCurrent().Name, TipoLog.Info)
                inicializar()
            End If
            'CrearLinkDocumentos()
        Catch batzEx As SabLib.BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            WriteLog("Error al cargar la pagina de nominas", TipoLog.Err, ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("errCargar")
        End Try
    End Sub

    ''' <summary>
    ''' Traduccion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelMesAno)
            'itzultzaileWeb.Itzuli(labelTitleDoc)
            itzultzaileWeb.Itzuli(btnBuscar)
            'itzultzaileWeb.Itzuli(labelDNI)
            'itzultzaileWeb.Itzuli(rfvDNI)
            'itzultzaileWeb.Itzuli(imgImprimir)
            'itzultzaileWeb.Itzuli(labelTitleNominas)
        End If
    End Sub

    ''' <summary>
    ''' Inicializa los controles
    ''' </summary>    
    Private Sub inicializar()
        'pnlResultados.Visible = False
        pnlSinResultados.Visible = False
        tableHead.Visible = False
        descargaNominas.Visible = False
        pnlSinResultados.Visible = False
        'pnlOtrosDocs.Visible = False
        'pnlOharrakIgorre.Visible = (Master.Ticket.IdPlanta = 1) 'Solo sera visible para la planta de Igorre
        Dim fechaCargar As Date = Now.AddMonths(-1) 'Se carga el del mes anterior
        'CargarMesesYAños(fechaCargar.Month, fechaCargar.Year, ddlMes, ddlAño)
        'CargarMesesYAños(fechaCargar.Month, fechaCargar.Year, ddlMes2, ddlAño2)
    End Sub


    '''' <summary>
    '''' Crea los links de documentos de 10T e intereses
    '''' </summary>    
    'Private Sub CrearLinkDocumentos()
    '    Dim lDocs, lDoc10TInt As List(Of Object)
    '    lDocs = New List(Of Object)
    '    lDoc10TInt = CrearLinkDocumentos(0) '10T        
    '    If (lDoc10TInt IsNot Nothing) Then lDocs.AddRange(lDoc10TInt)
    '    lDoc10TInt = CrearLinkDocumentos(1) 'Int
    '    If (lDoc10TInt IsNot Nothing) Then lDocs.AddRange(lDoc10TInt)
    '    rptDocumentos.DataSource = lDocs
    '    rptDocumentos.DataBind()
    '    pnlOtrosDocs.Visible = (lDocs.Count > 0)
    'End Sub

    ''' <summary>
    ''' Accede a la ruta donde estan los documentos 10T y los intereses almacenados y busca el suyo. Se tendra que coger el que acaba en '_password' ya que el otro no tiene contraseña
    ''' </summary>    
    ''' <param name="tipo">0:10T,1:Intereses</param>
    ''' <returns>Devuelve la lista anonima con properties Anno, TipoDoc</returns>
    Private Function CrearLinkDocumentos(tipo As Integer) As List(Of Object)
        Try
            Dim lDocumentos As List(Of Object) = Nothing
            Dim lDocs As New Dictionary(Of Integer, String)
            Dim fileToSearch, tipoDoc, pathDocumentos As String
            Dim files() As IO.FileInfo
            Dim documentos As IO.DirectoryInfo
            tipoDoc = If(tipo = 0, "10T", "Intereses")
            Dim plantBLL As New SabLib.BLL.PlantasComponent
            Dim oPlanta As SabLib.ELL.Planta = plantBLL.GetPlanta(Master.Ticket.IdPlanta)
            Dim negBLL As New NominasLib.Nomina

            Dim sPlantNomina As String() = negBLL.ConsultarPlantaNomina(oPlanta.IdEpsilon)
            pathDocumentos = sPlantNomina(1) & "\" & tipoDoc

            'Dim tokenHandle As New IntPtr(0)
            'If LogonUser("batzserveradmin", "batznt", "mvpemqcv", 2, 0, tokenHandle) Then
            'Dim newId As New Security.Principal.WindowsIdentity(tokenHandle)
            'Using impersonatedUser As Security.Principal.WindowsImpersonationContext = newId.Impersonate()
            Dim dirAnnos As New IO.DirectoryInfo(pathDocumentos)
            If (dirAnnos.Exists) Then
                For Each oFolder As IO.DirectoryInfo In dirAnnos.GetDirectories()
                    If (IsNumeric(oFolder.Name)) Then
                        documentos = New IO.DirectoryInfo(pathDocumentos & "\" & oFolder.Name)
                        '''' nombre documentos previos a 2019: IDSAB_NOMBREPERSONA_password.pdf 
                        '''' nombre documentos 2019 y posterior: IDEPSILON_IDSAB_password.pdf
                        If CInt(oFolder.Name) < 2019 Then
                            fileToSearch = Master.Ticket.IdUser & "_*_password.pdf"
                        Else
                            fileToSearch = "*_" & Master.Ticket.IdUser & "_password.pdf"
                        End If
                        files = documentos.GetFiles(fileToSearch)
                        If (files IsNot Nothing AndAlso files.Count = 1) Then
                            lDocs.Add(CInt(oFolder.Name), files.First.FullName)
                        End If
                    End If
                Next
                If (lDocs.Count > 0) Then
                    lDocumentos = New List(Of Object)
                    'Se ordenan para que salgan del mas nuevo al mas antiguo
                    Dim sortedDict = (From entry In lDocs Order By entry.Key Descending).ToDictionary(Function(pair) pair.Key, Function(pair) pair.Value)
                    For Each entry As KeyValuePair(Of Integer, String) In sortedDict
                        lDocumentos.Add(New With {.Anno = entry.Key, .Tipo = tipo, .Path = entry.Value})
                    Next
                End If
            End If
            'End Using
            'CloseHandle(tokenHandle)
            'End If
            Return lDocumentos
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al obtener el documento", ex)
        End Try
    End Function

    '''' <summary>
    '''' Documentos 10T e intereses
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    'Private Sub rptDocumentos_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rptDocumentos.ItemDataBound
    '    If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
    '        Dim doc As Object = e.Item.DataItem
    '        Dim lnkDoc As LinkButton = CType(e.Item.FindControl("lnkDoc"), LinkButton)
    '        lnkDoc.Text = If(doc.Tipo = 0, "10T", itzultzaileWeb.Itzuli("Intereses")) & " (" & doc.Anno & ")"
    '        lnkDoc.CommandArgument = doc.Path & "|" & doc.Tipo
    '        AddHandler lnkDoc.Click, AddressOf DescargarDocumento
    '    End If
    'End Sub

    ''' <summary>
    ''' Se enlazan los controles
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub rptNominas_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) Handles rptNominas.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim nomina As String() = e.Item.DataItem
            Dim lblCodTra As Label = CType(e.Item.FindControl("codTra"), Label)
            Dim lblNomTra As Label = CType(e.Item.FindControl("nomTra"), Label)
            Dim lblFecha As Label = CType(e.Item.FindControl("fecha"), Label)
            Dim lblTipo As Label = CType(e.Item.FindControl("tipo"), Label)
            Dim lnkVer As LinkButton = CType(e.Item.FindControl("lnkVer"), LinkButton)
            lblCodTra.Text = nomina(0)
            lblNomTra.Text = nomina(1)
            lblFecha.Text = nomina(2)
            lblTipo.Text = nomina(3)
            lnkVer.CommandArgument = nomina(4) & "_" & nomina(6)
            lnkVer.CommandName = nomina(0) & "_" & nomina(1).Replace(" ", "-") & "_" & nomina(5)
        End If
    End Sub

#End Region

#Region "Botones"

    ''' <summary>
    ''' Busca si existen nominas a mostrar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        Try
            If String.IsNullOrEmpty(datepicker1.Value) OrElse String.IsNullOrEmpty(datepicker2.Value) OrElse String.IsNullOrEmpty(users.Value) Then
                lblError.Visible = True
                Exit Sub
            End If
            lblError.Visible = False
            Dim lNominas As List(Of String()) = NominasLib.Nomina.ConsultarNominasIgorre(datepicker1.Value, datepicker2.Value, users.Value)
            lNominas = lNominas.OrderBy(Function(o) CInt(o(0))).ThenByDescending(Function(o) o(5)).ToList
            tableHead.Visible = lNominas.Count > 0
            descargaNominas.Visible = tableHead.Visible
            pnlSinResultados.Visible = Not tableHead.Visible
            rptNominas.DataSource = lNominas
            rptNominas.DataBind()
        Catch batzEx As SabLib.BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    Private Sub descargaNominas_Click(ByVal sender As Object, ByVal e As EventArgs) Handles descargaNominas.Click
        Dim listNominas = rptNominas.Items
        Dim stream As New IO.MemoryStream
        Using zipFile As New ZipFile
            For Each nomina In listNominas
                Dim data = nomina.Controls(9).CommandArgument

                Dim idNomina As Integer = CInt(data.Split("_")(0))
                Dim DNI As String = data.Split("_")(1)
                Dim nombrePdf As String = nomina.Controls(9).CommandName

                Dim bNomina As Byte() = NominasLib.Nomina.DownloadNomina(idNomina)
                Dim nominaDesencr As Byte() = NominasLib.Encriptacion.DesprotegerPDF(bNomina, DNI)

                Dim counter = 0
                While zipFile.ContainsEntry(nombrePdf & "_" & counter & ".pdf")
                    counter += 1
                End While
                zipFile.AddEntry(nombrePdf & "_" & counter & ".pdf", nominaDesencr)
            Next
            'zipFile.Save(Response.OutputStream)
            zipFile.Save(stream)

        End Using
        Response.Clear()
        Response.AppendHeader("Content-Disposition", "attachment; filename=nominas.zip")
        Response.ContentType = "application/x-compressed"
        Response.OutputStream.Write(stream.ToArray, 0, stream.ToArray.Length)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
        Response.End()

    End Sub

    ''' <summary>
    ''' Evento click al pinchar en la imagen para visualizar el pdf
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub VerNomina(ByVal sender As Object, ByVal e As EventArgs)
        Dim img As LinkButton = CType(sender, LinkButton)
        Try
            Dim idNomina As Integer = CInt(img.CommandArgument.Split("_")(0))
            Dim DNI As String = img.CommandArgument.Split("_")(1)
            Dim nombrePdf As String = img.CommandName
            PageBase.WriteLog("Consulta de nomina (" & idNomina & ")", PageBase.TipoLog.Info, Nothing)
            DownloadNomina(idNomina, nombrePdf, DNI)
        Catch batzEx As SabLib.BatzException
            Master.MensajeError = batzEx.Termino
        Catch threadEx As Threading.ThreadAbortException
        Catch ex As Exception
            PageBase.WriteLog("Error al descargar la nomina (" & img.CommandArgument & ")", TipoLog.Err, ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al descargar la nomina")
        End Try
    End Sub

    '''' <summary>
    '''' Evento click al pinchar en el link en el movil para visualizar el pdf
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    'Protected Sub VerNominaMobile(ByVal sender As Object, ByVal e As EventArgs)
    '    Dim lnk As LinkButton = CType(sender, LinkButton)
    '    Try
    '        Dim idNomina As Integer = CInt(lnk.CommandArgument)
    '        PageBase.WriteLog("Consulta de nomina en el movil (" & idNomina & ")", PageBase.TipoLog.Info, Nothing)
    '        DownloadNomina(idNomina)
    '    Catch batzEx As SabLib.BatzException
    '        Master.MensajeError = batzEx.Termino
    '    Catch threadEx As Threading.ThreadAbortException
    '    Catch ex As Exception
    '        PageBase.WriteLog("Error al descargar la nomina (" & lnk.CommandArgument & ")", TipoLog.Err, ex)
    '        Master.MensajeError = itzultzaileWeb.Itzuli("Error al descargar la nomina")
    '    End Try
    'End Sub

    ''' <summary>
    ''' Se descarga la nomina
    ''' </summary>
    ''' <param name="idNomina">Id de la nomina</param>
    Private Sub DownloadNomina(ByVal idNomina As Integer, ByVal nombrePdf As String, ByVal DNI As String)
        Dim bNomina As Byte() = NominasLib.Nomina.DownloadNomina(idNomina)
        Dim nominaDesencr As Byte() = NominasLib.Encriptacion.DesprotegerPDF(bNomina, DNI)

        Response.Clear()
        Response.AppendHeader("Content-Disposition", "attachment; filename=" & nombrePdf & ".pdf")
        Response.ContentType = "application/pdf"
        Response.OutputStream.Write(nominaDesencr, 0, nominaDesencr.Length)
        Response.OutputStream.Flush()
        Response.OutputStream.Close()
        Response.End()
    End Sub

    ''' <summary>
    ''' Descarga un documento
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub DescargarDocumento(ByVal sender As Object, ByVal e As EventArgs)
        Dim link As LinkButton = CType(sender, LinkButton)
        Dim info As String() = link.CommandArgument.Split("|")
        If (info(0) <> String.Empty) Then
            PageBase.WriteLog("Consulta del " & If(info(1) = "0", "10T", "intereses") & "(" & info(0) & ")", PageBase.TipoLog.Info, Nothing)
            Dim bDoc As Byte() = Nothing
            'Dim tokenHandle As New IntPtr(0)
            'If LogonUser("batzserveradmin", "batznt", "pwd", 2, 0, tokenHandle) Then
            'Dim newId As New Security.Principal.WindowsIdentity(tokenHandle)
            'Using impersonatedUser As Security.Principal.WindowsImpersonationContext = newId.Impersonate()
            bDoc = IO.File.ReadAllBytes(info(0))
            'End Using
            'CloseHandle(tokenHandle)
            'End If
            Response.Clear()
            Response.AppendHeader("Content-Disposition", "attachment; filename=" & If(info(1) = "0", "Documento10T", "DocumentoIntereses") & ".pdf")
            Response.ContentType = "application/pdf"
            Response.OutputStream.Write(bDoc, 0, bDoc.Length)
            Response.OutputStream.Flush()
            Response.OutputStream.Close()
            Response.End()
        Else
            Dim mensa As String = If(info(1) = "0", "documento 10T", "documento de intereses")
            Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("No se encuentra el " & mensa)
            PageBase.WriteLog("No se ha encontrado el " & mensa, PageBase.TipoLog.Warn, Nothing)
        End If
    End Sub

    '''' <summary>
    '''' Evento click al pinchar en la imagen para imprimir la nomina
    '''' Esto solo podra accerse en un ordenador concreto. En el resto, no se visualizara
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>    
    'Protected Sub ImprimirNomina(ByVal sender As Object, ByVal e As EventArgs)
    '    Dim img As ImageButton = CType(sender, ImageButton)
    '    PageBase.WriteLog("Impresion de nomina (" & img.CommandArgument & "). Se va a solicitar el DNI para desencriptar el pdf", PageBase.TipoLog.Info, Nothing)
    '    'Se inicializan los controles y se abre la ventana
    '    lblError.Text = String.Empty : lblError.Visible = False
    '    txtDNI.Text = String.Empty : pnlDNI.Visible = True
    '    pnlImpreso.Visible = False : lblImpreso.Text = String.Empty
    '    imgImprimir.CommandArgument = img.CommandArgument : imgImprimir.Visible = True
    '    mpePopUp.Show()
    'End Sub

    '''' <summary>
    '''' Se imprime la nomina. Para ello, se desencripta la nomina y se copia en una ruta donde se mandara a imprimir
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>    
    'Private Sub imgImprimir_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles imgImprimir.Click
    '    Dim rutaPDFDesencriptado As String = Server.MapPath("~") & "/Temp/nomina" & Master.Ticket.IdTrabajador & ".nom"
    '    Try
    '        If (txtDNI.Text.Trim = String.Empty) Then
    '            lblError.Text = itzultzaileWeb.Itzuli("Introduzca el dato")
    '            lblError.Visible = True
    '            mpePopUp.Show()
    '        Else
    '            Dim nomina As Byte() = NominasLib.Nomina.DownloadNomina(CInt(imgImprimir.CommandArgument))
    '            Dim resul As Integer = If(nomina IsNot Nothing, NominasLib.Nomina.ImprimirNomina(nomina, txtDNI.Text.Trim.ToUpper, rutaPDFDesencriptado), 2)
    '            imgImprimir.Visible = False : pnlDNI.Visible = False : pnlImpreso.Visible = True : lblError.Visible = False
    '            Select Case resul
    '                Case 0
    '                    lblImpreso.Text = itzultzaileWeb.Itzuli("Se ha mandado a imprimir su nomina. Espere por favor")
    '                Case 1
    '                    pnlDNI.Visible = True : pnlImpreso.Visible = False : imgImprimir.Visible = True : lblError.Visible = True
    '                    lblError.Text = itzultzaileWeb.Itzuli("DNI incorrecto. Introduzcalo de nuevo")
    '                Case 2
    '                    lblImpreso.Text = itzultzaileWeb.Itzuli("No se ha podido recuperar su nomina. Contacto con el administrador")
    '                Case Else
    '                    lblImpreso.Text = itzultzaileWeb.Itzuli("Ha ocurrido un error al intentar imprimir. Contacto con el administrador")
    '            End Select
    '            mpePopUp.Show()
    '        End If
    '    Catch batzEx As SabLib.BatzException
    '        Master.MensajeError = batzEx.Termino
    '    Catch ex As Exception
    '        PageBase.WriteLog("Error al imprimir la nomina (" & imgImprimir.CommandArgument & ")", TipoLog.Err, ex)
    '        Master.MensajeError = itzultzaileWeb.Itzuli("Error al imprimir la nomina")
    '    Finally
    '        NominasLib.Nomina.deleteBat(rutaPDFDesencriptado)
    '    End Try
    'End Sub

#End Region

    'Inserta una nomina en la bbdd
    'Private Sub btninsertar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btninsertar.Click
    '    NominasLib.Encriptacion.ProtegerPDF("c:\Pruebas\Nominas\Nomina_11_7_1_1_990194.pdf", "c:\Pruebas\Nominas\Generados\Nomina_11_7_1_1_990194.pdf", "c:\Pruebas\Nominas\Protegidos\Nomina_11_7_1_1_990194.pdf", "72396454K", "c:\Pruebas\Nominas\Temp")
    'End Sub

End Class