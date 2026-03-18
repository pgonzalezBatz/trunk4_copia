Public Class DocsIntegrViaje
    Inherits PageBase

#Region "Properties"

    ''' <summary>
    ''' Guarda el id del viaje
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Private Property IdViaje As Integer
        Get
            Return CInt(ViewState("IdViaje"))
        End Get
        Set(value As Integer)
            ViewState("IdViaje") = value
        End Set
    End Property
#End Region

#Region "Page Load"

    ''' <summary>
    ''' Se carga la informacion de la pagina
    ''' Se comprueba que el usuario sea integrante del viaje, de que el viaje sea al extranjero y de que la actividad sea exenta
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Documentos del viaje"
                IdViaje = CInt(Request.QueryString("idViaje"))
                pnlInfo.Visible = False : pnlNoPermitido.Visible = False
                'Se realizan las comprobaciones
                Dim bPermitido As Boolean = False
                Dim viajesBLL As New BLL.ViajesBLL
                Dim oViaje As ELL.Viaje = viajesBLL.loadInfo(IdViaje)
                If (oViaje IsNot Nothing) Then
                    If (oViaje.Nivel <> ELL.Viaje.eNivel.Nacional) Then
                        Dim oIntegr As ELL.Viaje.Integrante = oViaje.ListaIntegrantes.FirstOrDefault(Function(o As ELL.Viaje.Integrante) o.Usuario.Id = Master.Ticket.IdUser)
                        If (oIntegr IsNot Nothing) Then
                            Dim activBLL As New BLL.ActividadesBLL
                            If (activBLL.loadInfo(oIntegr.IdActividad).ExentaIRPF) Then
                                bPermitido = True
                            Else
                                lblNoPermitido.Text = itzultzaileWeb.Itzuli("La actividad del usuario no es exenta")
                                log.Warn("Doc_Integrantes:La actividad del usuario del viaje " & IdViaje & " es no exenta")
                            End If
                        Else
                            lblNoPermitido.Text = itzultzaileWeb.Itzuli("Usted no es integrante del viaje")
                            log.Warn("Doc_Integrantes:El usuario " & Master.Ticket.NombreCompleto & " no es integrante del viaje " & IdViaje)
                        End If
                    Else
                        lblNoPermitido.Text = itzultzaileWeb.Itzuli("Solo es necesario subir documentos para viajes internacionales. Este viaje es de ambito nacional")
                        log.Warn("Doc_Integrantes:El viaje " & IdViaje & " es " & [Enum].GetName(GetType(ELL.Viaje.eNivel), oViaje.Nivel))
                    End If
                Else
                    lblNoPermitido.Text = itzultzaileWeb.Itzuli("El viaje no existe")
                End If
                If (bPermitido) Then
                    pnlInfo.Visible = True
                    mostrarDetalle()
                Else
                    pnlNoPermitido.Visible = True
                End If
            End If
        Catch batzEx As BatzException
            pnlNoPermitido.Visible = False
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            pnlNoPermitido.Visible = False
            log.Error("Doc_Integrantes:Error al ver los documentos del integrante del viaje " & IdViaje, ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al mostrar los documentos del viaje del usuario")
        End Try
    End Sub

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(labelDocInfo1) : itzultzaileWeb.Itzuli(btnSubirDoc) : itzultzaileWeb.Itzuli(btnVolver)
            itzultzaileWeb.Itzuli(labelDocTitulo) : itzultzaileWeb.Itzuli(labelDocAdj) : itzultzaileWeb.Itzuli(labelConfirmDeleteTitle)
            itzultzaileWeb.Itzuli(labelConfirmDeleteMessage) : itzultzaileWeb.Itzuli(labelCancelarDelete) : itzultzaileWeb.Itzuli(btnEliminarModal)
            itzultzaileWeb.Itzuli(btnVolver2)
        End If
    End Sub

#End Region

#Region "Mostrar detalle"

    ''' <summary>
    ''' Muestra el detalle de los documentos
    ''' </summary>    
    Private Sub mostrarDetalle()
        Dim viajesBLL As New BLL.ViajesBLL
        Dim lDocs As List(Of ELL.Viaje.DocumentoIntegrante) = viajesBLL.loadDocumentosIntegrante(IdViaje, Master.Ticket.IdUser)
        If (lDocs IsNot Nothing) Then lDocs = lDocs.OrderBy(Of String)(Function(o) o.Titulo).ToList
        txtDocTitulo.Text = String.Empty
        rptDocumentos.DataSource = lDocs
        rptDocumentos.DataBind()
    End Sub

#End Region

#Region "Eventos"

    ''' <summary>
    ''' Se cargan los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub rptDocumentos_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rptDocumentos.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim oDoc As ELL.Viaje.DocumentoIntegrante = e.Item.DataItem
            Dim hkTitulo As HyperLink = CType(e.Item.FindControl("hkTitulo"), HyperLink)
            hkTitulo.Text = oDoc.Titulo
            hkTitulo.NavigateUrl = "~/Publico/ViewDocument.aspx?id=" & oDoc.Id & "&tipo=docintegrviaje"
            If (oDoc.FechaSubida <> DateTime.MinValue) Then
                hkTitulo.ToolTip = itzultzaileWeb.Itzuli("Subido el") & ": " & oDoc.FechaSubida.ToShortDateString & " - " & oDoc.FechaSubida.ToShortTimeString
            Else '031213: Todos los documentos antiguos, no tienen informado este campo
                hkTitulo.ToolTip = itzultzaileWeb.Itzuli("Abrir documento")
            End If
            CType(e.Item.FindControl("lnkEliminar"), LinkButton).OnClientClick = "$('#" & hfIdDoc.ClientID & "').val('" & oDoc.Id & "');$('#confirmDeleteDoc').modal('show'); return false;"
        End If
    End Sub

    ''' <summary>
    ''' Se elimina un documento
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub btnEliminarModal_Click(sender As Object, e As EventArgs) Handles btnEliminarModal.Click
        Try
            Dim idDoc As Integer = CInt(hfIdDoc.Value)
            Dim viajesBLL As New BLL.ViajesBLL
            viajesBLL.DeleteDocumentoIntegrante(idDoc)
            Master.MensajeInfo = itzultzaileWeb.Itzuli("Documento borrado")
            log.Info("Doc_Integrantes:Se ha borrado el documento " & idDoc & " del viaje " & IdViaje & " del usuario " & Master.Ticket.NombreCompleto)
            mostrarDetalle()
            ShowModalBoxDoc(False)
        Catch batzEx As BatzException
            ShowModalBoxDoc(True)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Muestra el panel modal para mostrar o no la pantalla de eliminar
    ''' </summary>
    ''' <param name="action">True lo muestra y false lo oculta</param>
    Private Sub ShowModalBoxDoc(ByVal action As Boolean)
        If (action) Then
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#confirmDeleteDoc').modal('show');", True)
        Else
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#confirmDeleteDoc').modal('hide');$('body').removeClass('modal-open');$('.modal-backdrop').hide();", True) '2º intruccion necesaria por estar dentro de un updatePanel
        End If
    End Sub

    ''' <summary>
    ''' Subi el documento a la hoja de gastos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnSubirDoc_Click(sender As Object, e As EventArgs) Handles btnSubirDoc.Click
        Try
            If (txtDocTitulo.Text = String.Empty Or Not fuDocumento.HasFile) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Debe seleccionar todos los datos")
            ElseIf (fuDocumento.FileName.Length > 75) Then
                log.Warn("Doc_Integrantes: Se ha excedido el maximo numero de caracteres del fichero a subir (" & fuDocumento.FileName.Length & ")")
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("La longitud maxima del nombre del fichero son 75 caracteres")
            Else
                Dim oDoc As New ELL.Viaje.DocumentoIntegrante With {.Documento = fuDocumento.FileBytes, .ContentType = fuDocumento.PostedFile.ContentType, .NombreFichero = fuDocumento.FileName, .IdViaje = IdViaje, .IdIntegrante = Master.Ticket.IdUser, .Titulo = txtDocTitulo.Text.Trim}
                Dim viajesBLL As New BLL.ViajesBLL
                Dim idDoc As Integer = viajesBLL.AddDocumentoIntegrante(oDoc)
                Master.MensajeInfo = itzultzaileWeb.Itzuli("Documento añadido")
                log.Info("Doc_Integrantes:Se ha subido el documento (" & idDoc & ") " & oDoc.Titulo & " al viaje " & IdViaje & " del usuario " & Master.Ticket.NombreCompleto)
                mostrarDetalle()
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Vuelve al listado de viajes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click, btnVolver2.Click
        Response.Redirect("Viajes.aspx", False)
    End Sub

#End Region

End Class