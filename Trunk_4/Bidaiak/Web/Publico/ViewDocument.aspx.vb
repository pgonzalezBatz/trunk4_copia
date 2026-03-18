Public Class ViewDocument
    Inherits Page

    Private itzultzaileWeb As New itzultzaile

    ''' <summary>
    ''' Recibe un identificador y un tipo para saber de donde tiene que sacar el adjunto
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Session("Ticket") Is Nothing Then
                Dim cultureInfo As Globalization.CultureInfo = Globalization.CultureInfo.CreateSpecificCulture("es-ES")
                Threading.Thread.CurrentThread.CurrentCulture = cultureInfo
                Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo
                Culture = cultureInfo.Name
            Else
                Culture = CType(Session("Ticket"), SabLib.ELL.Ticket).Culture
            End If
            Dim tipo As String = Request.QueryString("tipo")
            Dim id As String = Request.QueryString("id")
            Select Case tipo.ToLower
                Case "docintegrviaje"  'Documento de una hoja de gastos
                    Dim viajesBLL As New BLL.ViajesBLL
                    Dim oDocInt As ELL.Viaje.DocumentoIntegrante = Nothing
                    Try
                        oDocInt = viajesBLL.loadDocumentoIntegrante(id)
                    Catch batzEx As BatzException
                        lblMensaje.Text = batzEx.Termino
                        Exit Sub
                    End Try
                    If (oDocInt IsNot Nothing) Then
                        DownloadFile(oDocInt.NombreFichero, oDocInt.ContentType, oDocInt.Documento)
                    Else
                        lblMensaje.Text = itzultzaileWeb.Itzuli("No se ha podido obtener el documento del integrante del viaje")
                        PageBase.log.Warn("No se ha podido obtener el documento del integrante del viaje " & id)
                    End If
                Case "doccliviaje"  'Documento de cliente de un viaje
                    Dim viajeBLL As New BLL.ViajesBLL
                    Dim oDocCli As ELL.Viaje.DocumentoCliente = Nothing
                    Try
                        oDocCli = viajeBLL.loadDocumentoCliente(id)
                    Catch batzEx As BatzException
                        lblMensaje.Text = batzEx.Termino
                        Exit Sub
                    End Try
                    If (oDocCli IsNot Nothing) Then
                        DownloadFile(oDocCli.NombreFichero, oDocCli.ContentType, oDocCli.Documento)
                    Else
                        lblMensaje.Text = itzultzaileWeb.Itzuli("No se ha podido obtener el documento de cliente del viaje")
                        PageBase.log.Warn("No se ha podido obtener el documento de cliente " & id)
                    End If
                Case "reciboant"   'Recibo de un anticipo
                    Dim fs As IO.FileStream = Nothing
                    Dim ruta As String = String.Empty
                    Try
                        ruta = BLL.GenerarPDF.ReciboAnticipoEntregado(id, Session("IdPlanta"), "eu-ES", Session.SessionID)  'Siempre en euskera
                        fs = IO.File.OpenRead(ruta)
                        Dim bytes(fs.Length) As Byte
                        fs.Read(bytes, 0, CInt(fs.Length))
                        fs.Close()
                        If (bytes IsNot Nothing) Then
                            DownloadFile("Recibo.pdf", "application/pdf", bytes)
                        Else
                            lblMensaje.Text = itzultzaileWeb.Itzuli("No se ha podido obtener el recibo del anticipo")
                            PageBase.log.Warn("No se ha podido obtener el recibo del anticipo " & id)
                        End If
                    Catch batzEx As BatzException
                        lblMensaje.Text = batzEx.Termino
                    Finally
                        If (fs IsNot Nothing) Then fs.Close()
                        IO.File.Delete(ruta)
                    End Try
                Case "devolucionant"   'Recibo de una devolucion de anticipo
                    Dim fs As IO.FileStream = Nothing
                    Dim ruta As String = String.Empty
                    Try
                        Dim idViaje As Integer = CInt(Request.QueryString("idViaje"))
                        ruta = BLL.GenerarPDF.ReciboDevolucionAnticipo(idViaje, id.Split("|"), Session("IdPlanta"), "eu-ES", Session.SessionID)  'Siempre en euskera
                        fs = IO.File.OpenRead(ruta)
                        Dim bytes(fs.Length) As Byte
                        fs.Read(bytes, 0, CInt(fs.Length))
                        fs.Close()
                        If (bytes IsNot Nothing) Then
                            DownloadFile("Recibo.pdf", "application/pdf", bytes)
                        Else
                            lblMensaje.Text = itzultzaileWeb.Itzuli("No se ha podido obtener el recibo de la devolucion del anticipo")
                            PageBase.log.Warn("No se ha podido obtener el recibo de la devolucion del anticipo " & id)
                        End If
                    Catch batzEx As BatzException
                        lblMensaje.Text = batzEx.Termino
                    Finally
                        If (fs IsNot Nothing) Then fs.Close()
                        IO.File.Delete(ruta)
                    End Try
                Case "docproy"  'Documentos de los proyectos
                    Dim xbatBLL As New BLL.XbatBLL
                    Dim oDocProy As ELL.DocumentoProyecto = Nothing
                    Try
                        oDocProy = xbatBLL.GetDocumentoProyecto(id)
                    Catch batzEx As BatzException
                        lblMensaje.Text = batzEx.Termino
                        Exit Sub
                    End Try
                    If (oDocProy IsNot Nothing) Then
                        DownloadFile(oDocProy.Descripcion, oDocProy.ContentType, oDocProy.Adjunto)
                    Else
                        lblMensaje.Text = itzultzaileWeb.Itzuli("No se ha podido obtener el documento del proyecto")
                        PageBase.log.Warn("No se ha podido obtener el documento del proyecto " & id)
                    End If
                Case "hg"  'Hoja de gastos
                    Dim fs As IO.FileStream = Nothing
                    Dim ruta As String = String.Empty
                    Try
                        ruta = BLL.GenerarPDF.HojaGastos(id, Session("IdPlanta"), CType(Session("Ticket"), SabLib.ELL.Ticket).Culture, Session.SessionID, Session("Perfil"))
                        fs = IO.File.OpenRead(ruta)
                        Dim bytes(fs.Length) As Byte
                        fs.Read(bytes, 0, CInt(fs.Length))
                        fs.Close()
                        If (bytes IsNot Nothing) Then
                            DownloadFile("Hoja_Gastos.pdf", "application/pdf", bytes)
                        Else
                            lblMensaje.Text = itzultzaileWeb.Itzuli("No se ha podido obtener la hoja de gastos")
                            PageBase.log.Warn("No se ha podido obtener la hoja de gastos " & id)
                        End If
                    Catch batzEx As BatzException
                        lblMensaje.Text = batzEx.Termino
                    Finally
                        If (fs IsNot Nothing) Then fs.Close()
                        IO.File.Delete(ruta)
                    End Try
            End Select
        End If
    End Sub

    ''' <summary>
    ''' Descarga el documento indicado
    ''' </summary>    
    ''' <param name="fileName">Nombre del documento</param>
    ''' <param name="contentType">Tipo de contenido</param>
    ''' <param name="file">Documento</param>
    Protected Sub DownloadFile(ByVal fileName As String, ByVal contentType As String, ByVal file As Byte())
        'Si el Explorador reconoce el archivo lo abrira dentro de él, sino pedira al usuario la accion a realizar.
        Response.Clear() : Response.ClearHeaders() : Response.ClearContent()
        Response.Buffer = True
        Response.AppendHeader("Content-Disposition", "inline; filename=" & fileName)
        Response.ContentType = contentType
        Response.BinaryWrite(file)
        Response.End()
    End Sub

End Class