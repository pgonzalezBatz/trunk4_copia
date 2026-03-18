Public Class DetSolPlanta
    Inherits PageBase

#Region "Properties"

    ''' <summary>
    ''' Id del viaje
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public Property IdViaje As Integer
        Get
            Return CInt(ViewState("IdViaje"))
        End Get
        Set(ByVal value As Integer)
            ViewState("IdViaje") = value
        End Set
    End Property

    ''' <summary>
    ''' Id de la planta
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public Property IdPlanta As Integer
        Get
            Return CInt(ViewState("IdPlanta"))
        End Get
        Set(ByVal value As Integer)
            ViewState("IdPlanta") = value
        End Set
    End Property

    ''' <summary>
    ''' Ver sin aprobar. Es para saber si luego hay que mostrar solo los no aprobados o rejirnos por fechas
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public Property VerSinAprobar As Boolean
        Get
            Return CInt(ViewState("vsa"))
        End Get
        Set(ByVal value As Boolean)
            ViewState("vsa") = value
        End Set
    End Property

    ''' <summary>
    ''' Mes
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public Property Mes As Integer
        Get
            Return CInt(ViewState("Mes"))
        End Get
        Set(ByVal value As Integer)
            ViewState("Mes") = value
        End Set
    End Property

    ''' <summary>
    ''' Año
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public Property Año As Integer
        Get
            Return CInt(ViewState("Año"))
        End Get
        Set(ByVal value As Integer)
            ViewState("Año") = value
        End Set
    End Property

#End Region

#Region "Page Load"

    ''' <summary>
    ''' Carga la pagina con la solicitud de plantas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Detalle de la solicitud de planta"
                IdViaje = CInt(Request.QueryString("IdViaje"))
                IdPlanta = CInt(Request.QueryString("IdPlanta"))
                VerSinAprobar = If(Request.QueryString("vsa") Is Nothing, False, CBool(Request.QueryString("vsa")))
                If (Request.QueryString("Mes") IsNot Nothing) Then Mes = CInt(Request.QueryString("Mes"))
                If (Request.QueryString("Anio") IsNot Nothing) Then Año = CInt(Request.QueryString("Anio"))
                mostrarDetalle()
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelEstado) : itzultzaileWeb.Itzuli(labelViaje) : itzultzaileWeb.Itzuli(labelFechas)
            itzultzaileWeb.Itzuli(labelDescrip) : itzultzaileWeb.Itzuli(labelObserv) : itzultzaileWeb.Itzuli(btnAprobar)
            itzultzaileWeb.Itzuli(btnRechazar) : itzultzaileWeb.Itzuli(btnVolver) : itzultzaileWeb.Itzuli(lblNoExiste)
            itzultzaileWeb.Itzuli(labelTitleModal) : itzultzaileWeb.Itzuli(labelConfirmMessageModal) : itzultzaileWeb.Itzuli(btnRechazarModalM)
            itzultzaileWeb.Itzuli(labelCancelarModal)
        End If
    End Sub

#End Region

#Region "Mostrar detalle"

    ''' <summary>
    ''' Muestra el detalle de la solicitud
    ''' </summary>    
    Private Sub mostrarDetalle()
        Dim viajesBLL As New BLL.ViajesBLL
        Dim oSol As ELL.Viaje.SolicitudPlantaFilial = Nothing
        Dim lSolicitudes As List(Of ELL.Viaje.SolicitudPlantaFilial) = viajesBLL.loadSolPlantasFiliales(IdViaje, IdPlanta)
        If (lSolicitudes IsNot Nothing AndAlso lSolicitudes.Count > 0) Then oSol = lSolicitudes.First
        If (oSol Is Nothing) Then
            pnlNoExiste.Visible = True : pnlExiste.Visible = False
            btnAprobar.Visible = False : btnRechazar.Visible = False
            log.Warn("Se ha accedido al detalle de la solicitud de planta (V-" & IdViaje & "|P-" & IdPlanta & ") pero no se ha podido obtener. Puede que se haya cancelado")
        Else
            inicializarDetalle()
            Dim gerentBLL As New BLL.BidaiakBLL
            Dim oGerente As SabLib.ELL.Usuario = gerentBLL.loadGerentePlanta(oSol.IdPlantaFilial)
            If (oGerente IsNot Nothing) Then
                If (oGerente.Id <> Master.Ticket.IdUser) Then
                    log.Warn("El acceso al detalle de la solicitud planta no es accesible por el actual usuario ya que el gerente de la planta (" & oSol.IdPlantaFilial & ") es " & oGerente.NombreCompleto)
                    Response.Redirect("~/PermisoDenegado.aspx?mensa=3", False)
                    Exit Sub
                End If
            End If
            Dim oViaje As ELL.Viaje = viajesBLL.loadInfo(IdViaje, False, False, True)
            Dim plantBLL As New SabLib.BLL.PlantasComponent
            lblEstado.Text = [Enum].GetName(GetType(ELL.Viaje.SolicitudPlantaFilial.EstadoSolFilial), oSol.EstadoSolicitud)
            Select Case oSol.EstadoSolicitud
                Case ELL.Viaje.SolicitudPlantaFilial.EstadoSolFilial.Solicitado
                    lblEstado.CssClass = "label label-default"
                Case ELL.Viaje.SolicitudPlantaFilial.EstadoSolFilial.Aprobada
                    lblEstado.CssClass = "label label-success"
                Case ELL.Viaje.SolicitudPlantaFilial.EstadoSolFilial.Rechazada
                    lblEstado.CssClass = "label label-danger"
            End Select
            lblPlanta.Text = plantBLL.GetPlanta(oSol.IdPlantaFilial).Nombre
            lblViaje.Text = oViaje.IdViaje & " " & oViaje.Destino
            lblFechas.Text = oViaje.FechaIda.ToShortDateString & " - " & oViaje.FechaVuelta.ToShortDateString
            Dim descrip As String = oViaje.Descripcion.Replace(vbCrLf, "<br />")
            If (descrip = String.Empty) Then descrip = "Sin descripcion"
            lblDescrip.Text = descrip
            If (oSol.EstadoSolicitud = ELL.Viaje.SolicitudPlantaFilial.EstadoSolFilial.Solicitado) Then
                Dim activBLL As New BLL.ActividadesBLL
                Dim observ As String = itzultzaileWeb.Itzuli("Solicito los servicios de") & ":" & vbCrLf
                For Each oInteg As ELL.Viaje.Integrante In oViaje.ListaIntegrantes
                    observ &= oInteg.Usuario.NombreCompleto & "(" & activBLL.loadInfo(oInteg.IdActividad).Nombre & ")" & vbCrLf
                Next
                observ &= itzultzaileWeb.Itzuli("Entre las fechas") & " " & lblFechas.Text
                txtObserv.Text = observ
            Else
                txtObserv.Text = oSol.Observaciones
            End If
            gvPersonas.DataSource = oViaje.ListaIntegrantes
            gvPersonas.DataBind()
            btnAprobar.Visible = (oSol.EstadoSolicitud = ELL.Viaje.SolicitudPlantaFilial.EstadoSolFilial.Solicitado)
            btnRechazar.Visible = btnAprobar.Visible
        End If
    End Sub

    ''' <summary>
    ''' Inicializa los controles
    ''' </summary>    
    Private Sub inicializarDetalle()
        lblEstado.Text = String.Empty : lblViaje.Text = String.Empty
        lblFechas.Text = String.Empty : lblDescrip.Text = String.Empty
        txtObserv.Text = String.Empty
        gvPersonas.DataSource = Nothing : gvPersonas.DataBind()
        btnAprobar.Visible = True : btnRechazar.Visible = False
        pnlExiste.Visible = True : pnlNoExiste.Visible = False
    End Sub

#End Region

#Region "Gridview"

    ''' <summary>
    ''' Enlaza los controles
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvPersonas_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvPersonas.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oIntegrante As ELL.Viaje.Integrante = e.Row.DataItem
            Dim actiBLL As New BLL.ActividadesBLL
            CType(e.Row.FindControl("lblPersona"), Label).Text = oIntegrante.Usuario.NombreCompleto
            CType(e.Row.FindControl("lblActividad"), Label).Text = actiBLL.loadInfo(oIntegrante.IdActividad).Nombre
            CType(e.Row.FindControl("lblFechaIda"), Label).Text = oIntegrante.FechaIda.ToShortDateString
            CType(e.Row.FindControl("lblFechaVuelta"), Label).Text = oIntegrante.FechaVuelta.ToShortDateString
        End If
    End Sub

#End Region

#Region "Botones"

    ''' <summary>
    ''' Muestra el panel modal para introducir un comentario de rechazo
    ''' </summary>
    Private Sub ShowModalBox()
        ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#divModalRej').modal('show');", True)
    End Sub

    ''' <summary>
    ''' Marca como aprobada la solicitud
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnAprobar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAprobar.Click
        Try
            SaveChanges(ELL.Viaje.SolicitudPlantaFilial.EstadoSolFilial.Aprobada)
            log.Info("El gerente " & Master.Ticket.NombreCompleto & " ha aprobado la solicitud del viaje " & IdViaje & " para la planta " & lblPlanta.Text & "(" & IdPlanta & ")")
            Master.MensajeInfo = itzultzaileWeb.Itzuli("Solicitud aprobada")
            AvisarPorEmail(ELL.Viaje.SolicitudPlantaFilial.EstadoSolFilial.Aprobada)
            Volver()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Muestra el pop up
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnRechazar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRechazar.Click
        txtRechazo.Text = String.Empty
        ShowModalBox()
    End Sub

    ''' <summary>
    ''' Marca como rechazada la solicitud
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>  
    Private Sub btnRechazarModalM_Click(sender As Object, e As EventArgs) Handles btnRechazarModalM.Click
        Try
            If (txtRechazo.Text.Trim = String.Empty) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Introduzca un comentario")
                ShowModalBox()
            Else
                SaveChanges(ELL.Viaje.SolicitudPlantaFilial.EstadoSolFilial.Rechazada)
                log.Info("El gerente " & Master.Ticket.NombreCompleto & " ha rechazado la solicitud del viaje " & IdViaje & " para la planta " & lblPlanta.Text & "(" & IdPlanta & ")")
                Master.MensajeInfo = itzultzaileWeb.Itzuli("Solicitud rechazada")
                AvisarPorEmail(ELL.Viaje.SolicitudPlantaFilial.EstadoSolFilial.Rechazada)
                Volver()
            End If
        Catch batzEx As BatzException
            ShowModalBox()
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Guarda los cambios de la solicitud
    ''' </summary>
    ''' <param name="estado">Estado nuevo</param>    
    Private Sub SaveChanges(estado As ELL.Viaje.SolicitudPlantaFilial.EstadoSolFilial)
        Dim solBLL As New BLL.ViajesBLL
        Dim oSol As ELL.Viaje.SolicitudPlantaFilial = Nothing
        Dim lSolicitudes As List(Of ELL.Viaje.SolicitudPlantaFilial) = solBLL.loadSolPlantasFiliales(IdViaje, IdPlanta)
        If (lSolicitudes IsNot Nothing) Then oSol = lSolicitudes.First
        If (oSol IsNot Nothing) Then
            oSol.Observaciones = txtObserv.Text.Trim
            If (estado = ELL.Viaje.SolicitudPlantaFilial.EstadoSolFilial.Rechazada) Then oSol.Observaciones &= vbCrLf & "-----------------------------" & vbCrLf & txtRechazo.Text.Trim
            oSol.EstadoSolicitud = estado
            solBLL.SaveSolPlantaFilial(oSol)
        End If
    End Sub

    ''' <summary>
    ''' Vuelve al listado de gastos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnVolver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnVolver.Click
        Volver()
    End Sub

    ''' <summary>
    ''' Crea la direccion de retorno
    ''' </summary>    
    Private Sub Volver()
        Dim url As String = "SolicitudPlantaFilial.aspx?"
        url &= If(VerSinAprobar, "vsa=1", "vsa=0")
        If (Mes > 0) Then url &= "&mes=" & Mes & "&anio=" & Año
        Response.Redirect(url, False)
    End Sub

#End Region

#Region "Avisar por email"

    ''' <summary>
    ''' Se avisa al organizador del viaje de que se ha aprobado o denegado el viaje
    ''' </summary>
    ''' <param name="estado">Indica si se ha aprobado o no</param>
    Private Sub AvisarPorEmail(ByVal estado As ELL.Viaje.SolicitudPlantaFilial.EstadoSolFilial)
        If (ConfigurationManager.AppSettings("avisarPorEmail") = "1") Then
            Dim body, bodyEmail, linkUrl, subject As String
            Dim perfBLL As New BLL.BidaiakBLL
            Dim idRecurso As Integer = CInt(ConfigurationManager.AppSettings("RecursoWeb_Admon"))
            body = String.Empty
            Dim paramBLL As New SabLib.BLL.ParametrosBLL
            Dim viajesBLL As New BLL.ViajesBLL
            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor
            Dim nombrePlanta As String = String.Empty
            Try
                Dim emailsAccesoDirecto, emailsAccesoPortal As String
                emailsAccesoDirecto = String.Empty : emailsAccesoPortal = String.Empty
                Dim oViaje As ELL.Viaje = viajesBLL.loadInfo(IdViaje, False, False, True)
                Dim oOrganizador As SabLib.ELL.Usuario = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = oViaje.IdUserSolicitador})
                If (oOrganizador IsNot Nothing) Then
                    Dim sPerfil As String() = perfBLL.loadProfile(Master.IdPlantaGestion, oOrganizador.Id, idRecurso)
                    If (sPerfil(1) = "0") Then 'Acceso por portal
                        If (emailsAccesoPortal <> String.Empty) Then emailsAccesoPortal &= ";"
                        emailsAccesoPortal &= oOrganizador.Email
                    Else 'Acceso directo
                        If (emailsAccesoDirecto <> String.Empty) Then emailsAccesoDirecto &= ";"
                        emailsAccesoDirecto &= oOrganizador.Email
                    End If
                End If
                If (emailsAccesoDirecto = String.Empty AndAlso emailsAccesoPortal = String.Empty) Then
                    log.Info("No se han encontrado el email del organizador del viaje (V" & IdViaje & ") para avisarle de la respuesta del gerente")
                Else
                    Dim titulo As String = String.Empty
                    subject = String.Empty
                    Dim sEstado As String = If(estado = ELL.Viaje.SolicitudPlantaFilial.EstadoSolFilial.Aprobada, "aprobado", "rechazado")
                    body = "El gerente " & Master.Ticket.NombreCompleto & " ha " & sEstado & " la solicitud de su viaje V" & IdViaje & " entre las fechas " & lblFechas.Text & "<br /><br />" & txtObserv.Text.Replace(vbCrLf, "<br />")
                    subject = "Solicitud de planta " & If(estado = ELL.Viaje.SolicitudPlantaFilial.EstadoSolFilial.Aprobada, "Aprobada", "Rechazada") & " (V" & oViaje.IdViaje & ")"
                    titulo = "Solicitud de planta"
                    linkUrl = String.Empty
                    If (emailsAccesoPortal <> String.Empty) Then
                        bodyEmail = PageBase.getBodyHmtl(titulo, "V" & oViaje.IdViaje, body, linkUrl, True)
                        SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoPortal, subject, bodyEmail, serverEmail)
                        log.Info("Se ha enviado un email para indicar al organizador del viaje (V" & oViaje.IdViaje & ") que el gerente ha " & sEstado & " la solicitud con acceso por el portal => " & emailsAccesoPortal)
                    End If
                    If (emailsAccesoDirecto <> String.Empty) Then
                        bodyEmail = PageBase.getBodyHmtl(titulo, "V" & oViaje.IdViaje, body, linkUrl, False)
                        SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoDirecto, subject, bodyEmail, serverEmail)
                        log.Info("Se ha enviado un email para indicar al organizador del viaje (V" & oViaje.IdViaje & ") que el gerente ha " & sEstado & " la solicitud con acceso directo => " & emailsAccesoDirecto)
                    End If
                End If
            Catch ex As Exception
                log.Error("No se ha podido avisar al organizador del viaje (V" & IdViaje & ") de la respuesta del gerente a la solicitud de planta", ex)
            End Try
        End If
    End Sub

#End Region

End Class