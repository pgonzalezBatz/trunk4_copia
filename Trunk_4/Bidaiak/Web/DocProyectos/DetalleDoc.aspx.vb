Public Class DetalleDoc
    Inherits PageBase

#Region "Properties"

    ''' <summary>
    ''' Obtiene el id del documento
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Private Property IdDoc As Integer
        Get
            If (ViewState("IdDoc") Is Nothing) Then ViewState("IdDoc") = 0
            Return CInt(ViewState("IdDoc"))
        End Get
        Set(value As Integer)
            ViewState("IdDoc") = value
        End Set
    End Property

    ''' <summary>
    ''' Obtiene el id del proyecto
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Private Property IdProyecto As Integer
        Get
            If (ViewState("IdProy") Is Nothing) Then ViewState("IdProy") = 0
            Return CInt(ViewState("IdProy"))
        End Get
        Set(value As Integer)
            ViewState("IdProy") = value
        End Set
    End Property

#End Region

#Region "Page Load"

    ''' <summary>
    ''' Muestra el detalle del documento
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Detalle del documento"
                If (Request.QueryString("IdDoc") IsNot Nothing) Then IdDoc = Request.QueryString("IdDoc")
                IdProyecto = Request.QueryString("IdProy")
                mostrarDetalle()
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Traduccion de controles
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelCliente) : itzultzaileWeb.Itzuli(labelCliente) : itzultzaileWeb.Itzuli(labelDescrip)
            itzultzaileWeb.Itzuli(labelDoc) : itzultzaileWeb.Itzuli(btnGuardar) : itzultzaileWeb.Itzuli(btnEliminar)
            itzultzaileWeb.Itzuli(btnVolver) : itzultzaileWeb.Itzuli(labelConfirmDeleteTitle) : itzultzaileWeb.Itzuli(labelConfirmCancel)
            itzultzaileWeb.Itzuli(btnEliminarModal)
        End If
    End Sub

#End Region

#Region "Cargar datos"

    ''' <summary>
    ''' Se muestra el detalle del documento
    ''' </summary>    
    Private Sub mostrarDetalle()
        Try
            inicializar()
            Dim xbatBLL As New BLL.XbatBLL
            Dim info As String() = xbatBLL.GetInfoClienteProyecto(IdProyecto).FirstOrDefault
            If (info IsNot Nothing) Then
                lblProy.Text = info(1)
                lblCliente.Text = info(3)
                divDoc.Visible = (IdDoc > 0) : btnEliminar.Visible = (IdDoc > 0)
                If (IdDoc > 0) Then
                    Dim viajesBLL As New BLL.ViajesBLL
                    Dim oDocProy As ELL.DocumentoProyecto = xbatBLL.GetDocumentoProyecto(IdDoc)
                    txtDescrip.Text = oDocProy.Descripcion
                    hkDocumento.NavigateUrl = "~/Publico/ViewDocument.aspx?tipo=docProy&id=" & IdDoc
                    'El boton de eliminar no estara habilitado si esta relacionado con un viaje y si es el ultimo doc asignado a dicho proyecto
                    Dim lDocsProy As List(Of ELL.DocumentoProyecto) = xbatBLL.GetDocumentosProyecto(oDocProy.IdProyecto)
                    btnEliminar.Enabled = Not (lDocsProy.Count = 1 AndAlso viajesBLL.existeViajeConProyecto(oDocProy.IdProyecto))
                    If (Not btnEliminar.Enabled) Then btnEliminar.ToolTip = itzultzaileWeb.Itzuli("No se puede dejar el proyecto sin documentos ya que existe algun viaje con el que esta relacionado")
                End If
            Else
                Master.MensajeError = itzultzaileWeb.Itzuli("Error al obtener la informacion del documento")
            End If
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New Exception("Error al mostrar el detalle", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se inicializan los controles
    ''' </summary>    
    Private Sub inicializar()
        lblCliente.Text = String.Empty : lblProy.Text = String.Empty
        txtDescrip.Text = String.Empty : btnGuardar.CommandArgument = String.Empty
        btnEliminar.Visible = False
        btnEliminar.OnClientClick = "$('#confirmDelete').modal('show'); return false;"
    End Sub

#End Region

#Region "Botones"

    ''' <summary>
    ''' Guarda los datos del documento
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Try
            If (txtDescrip.Text = String.Empty OrElse (Not fuDocumento.HasFile And divDoc.Visible = False)) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Introduzca todos los datos")
            Else
                Dim xbatBLL As New BLL.XbatBLL
                Dim oDocProy As New ELL.DocumentoProyecto With {.IdProyecto = IdProyecto, .Descripcion = txtDescrip.Text.Trim}
                With oDocProy
                    If (fuDocumento.HasFile) Then
                        .Adjunto = fuDocumento.FileBytes
                        .ContentType = fuDocumento.PostedFile.ContentType
                    End If
                    If (IdDoc > 0) Then
                        .Id = IdDoc
                    Else
                        .IdUsuario = Master.Ticket.IdUser
                    End If
                End With
                xbatBLL.SaveDocumentoProyecto(oDocProy)
                If (IdDoc > 0) Then
                    log.Info("Se ha actualizado el documento '" & oDocProy.Descripcion & "' (" & IdDoc & ") del proyecto " & IdProyecto)
                Else
                    log.Info("Se ha añadido el documento '" & oDocProy.Descripcion & "' del proyecto " & IdProyecto)
                End If
                Volver()
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            log.Error("Error al guardar el documento del proyecto " & IdProyecto, ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al guardar")
        End Try
    End Sub

    ''' <summary>
    ''' Elimina el documento
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnEliminarModal_Click(sender As Object, e As EventArgs) Handles btnEliminarModal.Click
        Try
            Dim xbatBLL As New BLL.XbatBLL
            xbatBLL.DeleteDocumentoProyecto(IdDoc)
            log.Info("Se ha eliminado el documento " & IdDoc & " del proyecto " & IdProyecto)
            Volver()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Vuelve al listado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        Volver()
    End Sub

    ''' <summary>
    ''' Funcion de vuelta
    ''' </summary>    
    Private Sub Volver()
        Response.Redirect("ListadoDocs.aspx?idProy=" & IdProyecto, False)
    End Sub

#End Region

End Class