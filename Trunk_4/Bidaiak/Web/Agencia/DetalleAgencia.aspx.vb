Public Class DetalleAgencia
    Inherits PageBase

    ''' <summary>
    ''' Gestiona la lista de albaranes
    ''' </summary>
    ''' <returns></returns>
    Private Property Albaranes As List(Of String)
        Get
            If (ViewState("alb") Is Nothing) Then
                ViewState("alb") = New List(Of String)
            End If
            Return CType(ViewState("alb"), List(Of String))
        End Get
        Set(value As List(Of String))
            ViewState("alb") = value
        End Set
    End Property

    ''' <summary>
    ''' Obtiene el Id del viaje
    ''' </summary>
    ''' <returns></returns>
    Private ReadOnly Property IdViaje As Integer
        Get
            Return CInt(Request.QueryString("idViaje"))
        End Get
    End Property

    ''' <summary>
    ''' Se carga los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Detalle de la solicitud"
                mostrarDetalle()
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al cargar")
        End Try
    End Sub

    ''' <summary>
    ''' Traducciones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub DetalleAgencia_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelDivCab) : itzultzaileWeb.Itzuli(labelIdViaje) : itzultzaileWeb.Itzuli(lnkModificarViaje) : itzultzaileWeb.Itzuli(labelDestino)
            itzultzaileWeb.Itzuli(labelTipoViaje) : itzultzaileWeb.Itzuli(labelSol) : itzultzaileWeb.Itzuli(labelEstado) : itzultzaileWeb.Itzuli(labelFechaida)
            itzultzaileWeb.Itzuli(labelFechaVuelta) : itzultzaileWeb.Itzuli(labelProyectos) : itzultzaileWeb.Itzuli(labelDivPresup)
            itzultzaileWeb.Itzuli(labelUO) : itzultzaileWeb.Itzuli(labelValid) : itzultzaileWeb.Itzuli(lblEstadoValidacion) : itzultzaileWeb.Itzuli(lnkVerPresupuesto)
            itzultzaileWeb.Itzuli(labelDivInt) : itzultzaileWeb.Itzuli(labelDivRec) : itzultzaileWeb.Itzuli(labelDescripSolic) : itzultzaileWeb.Itzuli(labelComenSolic)
            itzultzaileWeb.Itzuli(labelCabDivAlb) : itzultzaileWeb.Itzuli(rfvAlbaran) : itzultzaileWeb.Itzuli(btnAddAlbaran) : itzultzaileWeb.Itzuli(chbGestionado)
            itzultzaileWeb.Itzuli(btnGuardar) : itzultzaileWeb.Itzuli(btnVolver) : itzultzaileWeb.Itzuli(labelConductor)
        End If
    End Sub

    ''' <summary>
    ''' Muestra el detalle de un viaje
    ''' </summary>
    Private Sub mostrarDetalle()
        inicializarDetalle()
        Dim viajesBLL As New BLL.ViajesBLL
        Dim presupBLL As New BLL.PresupuestosBLL
        Dim userBLL As New SabLib.BLL.UsuariosComponent
        Dim oViaje As ELL.Viaje = viajesBLL.loadInfo(IdViaje)
        Dim proyectoOF As String = String.Empty
        lblIdViaje.Text = "V" & IdViaje
        lblDestino.Text = oViaje.Destino
        lblSol.Text = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = oViaje.IdUserSolicitador}, False).NombreCompleto
        lblTipoViaje.Text = itzultzaileWeb.Itzuli([Enum].GetName(GetType(ELL.Viaje.eNivel), oViaje.Nivel).Replace("_", " "))
        lblFechaIda.Text = oViaje.FechaIda.ToShortDateString
        lblFechaVuelta.Text = oViaje.FechaVuelta.ToShortDateString
        For Each item In oViaje.Proyectos
            proyectoOF &= If(proyectoOF <> String.Empty, "<br />", String.Empty) & item.Descripcion
        Next
        lblProyectos.Text = If(proyectoOF = String.Empty, itzultzaileWeb.Itzuli("Sin proyecto"), proyectoOF)
        lblEstado.Text = itzultzaileWeb.Itzuli([Enum].GetName(GetType(ELL.SolicitudAgencia.EstadoAgencia), oViaje.SolicitudAgencia.Estado).Replace("_", " ")).ToUpper
        Select Case oViaje.SolicitudAgencia.Estado
            Case ELL.SolicitudAgencia.EstadoAgencia.cancelada
                lblEstado.CssClass = "label label-danger"
            Case ELL.SolicitudAgencia.EstadoAgencia.Cerrada
                lblEstado.CssClass = "label label-default"
            Case Else
                lblEstado.CssClass = "label label-info"
        End Select
        lblUnidadOrg.Text = oViaje.UnidadOrganizativa.Nombre
        Dim oPresup As ELL.Presupuesto = presupBLL.loadInfo(IdViaje)
        If (oPresup IsNot Nothing) Then
            lnkVerPresupuesto.CommandArgument = If(oPresup.PresupuestoNuevo, 1, 0)
            hfIdValidador.Value = oPresup.IdUsuarioResponsable
            lblValidador.Text = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = oPresup.IdUsuarioResponsable}, False).NombreCompleto
            lblEstadoValidacion.Text = itzultzaileWeb.Itzuli([Enum].GetName(GetType(ELL.Presupuesto.EstadoPresup), oPresup.Estado)).ToString.ToUpper
            Select Case oPresup.Estado
                Case ELL.Presupuesto.EstadoPresup.Enviado
                    divPresup.Attributes.Add("class", "bg-info")
                Case ELL.Presupuesto.EstadoPresup.Validado
                    Dim lEstados As List(Of ELL.Presupuesto.HistoricoEstado) = oPresup.Estados.OrderByDescending(Function(f) f.ChangeDate).ToList
                    If (lEstados.Item(1).State <> ELL.Presupuesto.EstadoPresup.Enviado) Then lblEstadoValidacion.Text = itzultzaileWeb.Itzuli("Autovalidado").ToString.ToUpper
                    divPresup.Attributes.Add("class", "bg-success")
                Case ELL.Presupuesto.EstadoPresup.Rechazado
                    divPresup.Attributes.Add("class", "bg-danger")
                Case ELL.Presupuesto.EstadoPresup.Creado
                    divPresup.Attributes.Add("class", "bg-warning")
            End Select
        Else
            'Todavia no se ha creado
            lblEstadoValidacion.Text = itzultzaileWeb.Itzuli("Presupuesto no generado").ToString.ToUpper
            divPresup.Attributes.Add("class", "bg-warning")
            lnkVerPresupuesto.Enabled = False  'Para las solicitudes anteriores al 041213 no se va a utilizar esto. Solo para las nuevas
        End If
        oViaje.ListaIntegrantes = oViaje.ListaIntegrantes.OrderBy(Of String)(Function(o) o.Usuario.NombreCompleto).ToList
        gvIntegrantes.DataSource = oViaje.ListaIntegrantes
        gvIntegrantes.DataBind()
        Dim servicios As List(Of ELL.SolicitudAgencia.Linea) = oViaje.SolicitudAgencia.ServiciosSolicitados.OrderBy(Of Integer)(Function(o) o.Tipo).ToList
        For Each serv As ELL.SolicitudAgencia.Linea In servicios
            If (serv.Tipo = ELL.SolicitudAgencia.Linea.TipoLinea.Servicios) Then
                lblServReq.Text &= If(lblServReq.Text <> String.Empty, ",", "") & itzultzaileWeb.Itzuli(serv.ServicioAgencia.Nombre)
                If (serv.ServicioAgencia.RequiereUsuario And serv.IdUserReq <> Integer.MinValue) Then
                    'lblServReq.Text &= If(serv.NavegadorGPS, " " & itzultzaileWeb.Itzuli("Con navegador GPS"), String.Empty)
                    divCocheAlq.Visible = True
                    lblConductor.Text = oViaje.ListaIntegrantes.Find(Function(o) o.Usuario.Id = serv.IdUserReq).Usuario.NombreCompleto
                End If
            End If
        Next
        lblComentariosSolic.Text = If(oViaje.SolicitudAgencia.ComentariosUsuario = String.Empty, "S/C", oViaje.SolicitudAgencia.ComentariosUsuario)
        lblDescripcionSolic.Text = If(oViaje.Descripcion = String.Empty, "S/C", oViaje.Descripcion)
        chbGestionado.Enabled = (oViaje.SolicitudAgencia.Estado <> ELL.SolicitudAgencia.EstadoAgencia.Cerrada And oViaje.SolicitudAgencia.Estado <> ELL.SolicitudAgencia.EstadoAgencia.cancelada)
        chbGestionado.Checked = (oViaje.SolicitudAgencia.Estado = ELL.SolicitudAgencia.EstadoAgencia.Gestionado Or oViaje.SolicitudAgencia.Estado = ELL.SolicitudAgencia.EstadoAgencia.Cerrada)
        txtAlbaran.Attributes.Add("placeholder", itzultzaileWeb.Itzuli("Albaranes del viaje"))
        Albaranes = oViaje.SolicitudAgencia.Albaranes
        rptAlbaranes.DataSource = Albaranes
        rptAlbaranes.DataBind()
    End Sub

    ''' <summary>
    ''' Inicializa los controles del detalle
    ''' </summary>    
    Private Sub inicializarDetalle()
        lblIdViaje.Text = String.Empty : lblDestino.Text = String.Empty : lblTipoViaje.Text = String.Empty
        lblFechaIda.Text = String.Empty : lblFechaVuelta.Text = String.Empty
        lblEstado.Text = String.Empty : hfIdValidador.Value = String.Empty
        chbGestionado.Checked = False : chbGestionado.Enabled = True
        lblConductor.Text = String.Empty : divCocheAlq.Visible = False
        txtAlbaran.Text = String.Empty : lblServReq.Text = String.Empty : lblProyectos.Text = String.Empty
        rptAlbaranes.DataSource = Nothing : rptAlbaranes.DataBind() : Albaranes = Nothing
        lblComentariosSolic.Text = String.Empty : lblDescripcionSolic.Text = String.Empty
        gvIntegrantes.DataSource = Nothing : gvIntegrantes.DataBind()
        lblInfo.ToolTip = itzultzaileWeb.Itzuli("Fecha de llegada al origen (Ej: Fecha de llegada al aeropuerto de Bilbao)")
    End Sub

    ''' <summary>
    ''' Agrega el albaran al listado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnAddAlbaran_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddAlbaran.Click
        If (txtAlbaran.Text = String.Empty) Then
            Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Rellene el albaran")
        Else
            Albaranes.Add(txtAlbaran.Text.Replace(" ", ""))
            txtAlbaran.Text = String.Empty
            rptAlbaranes.DataSource = Albaranes
            rptAlbaranes.DataBind()
        End If
    End Sub

    ''' <summary>
    ''' Quita el albaran del listado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>        
    Protected Sub lnkQuitarAlb_Click(sender As Object, e As EventArgs)
        Dim lnk As LinkButton = CType(sender, LinkButton)
        Albaranes.Remove(lnk.CommandArgument)
        rptAlbaranes.DataSource = Albaranes
        rptAlbaranes.DataBind()
    End Sub

    ''' <summary>
    ''' Se enlazan los albaranes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub rptAlbaranes_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rptAlbaranes.ItemDataBound
        If (e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item) Then
            Dim myAlbaran As String = e.Item.DataItem
            DirectCast(e.Item.FindControl("lblAlbaran"), Label).Text = myAlbaran
            DirectCast(e.Item.FindControl("lnkQuitarAlb"), LinkButton).CommandArgument = myAlbaran
        End If
    End Sub

    ''' <summary>
    ''' Se enlazan los integrantes del viaje
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Private Sub gvIntegrantes_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvIntegrantes.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim integrante As ELL.Viaje.Integrante = DirectCast(e.Row.DataItem, ELL.Viaje.Integrante)
            Dim lblNombrePersona As Label = DirectCast(e.Row.FindControl("lblNombrePersona"), Label)
            Dim epsilonBLL As New BLL.Epsilon(Master.IdPlantaGestion)
            Dim info As String() = epsilonBLL.GetInfoPersona(integrante.Usuario.Dni)
            If (info IsNot Nothing) Then
                lblNombrePersona.Text = info(1).Trim & " " & info(2).Trim & " " & info(3).Trim
            Else
                lblNombrePersona.Text = integrante.Usuario.NombreCompleto & " (*)"
            End If
            DirectCast(e.Row.FindControl("lblNumTrabajador"), Label).Text = integrante.Usuario.CodPersona
            DirectCast(e.Row.FindControl("lblDNI"), Label).Text = integrante.Usuario.Dni
            DirectCast(e.Row.FindControl("lblFDesde"), Label).Text = integrante.FechaIda.ToShortDateString
            DirectCast(e.Row.FindControl("lblFHasta"), Label).Text = integrante.FechaVuelta.ToShortDateString
            Dim depComp As New SabLib.BLL.DepartamentosComponent
            Dim oDep As New SabLib.ELL.Departamento With {.Id = integrante.Usuario.IdDepartamento, .IdPlanta = Master.IdPlantaGestion}
            oDep = depComp.GetDepartamento(oDep)
            If (oDep IsNot Nothing) Then DirectCast(e.Row.FindControl("lblDepartamento"), Label).Text = oDep.Nombre
        End If
    End Sub

    ''' <summary>
    ''' Se redirige a la pagina de solicitud de un viaje para su modificacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub lnkModificarViaje_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkModificarViaje.Click
        Response.Redirect("~\Viaje\SolicitudViaje.aspx?id=" & IdViaje)
    End Sub

    ''' <summary>
    ''' Guarda los cambios
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Private Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Try
            Dim solAgenciaBLL As New BLL.SolicAgenciasBLL
            Dim bGestionado As Boolean = False
            Dim oSolAgen As ELL.SolicitudAgencia = solAgenciaBLL.loadInfo(IdViaje, False)
            Dim estadoOriginal As Integer = oSolAgen.Estado
            bGestionado = (oSolAgen.Estado = ELL.SolicitudAgencia.EstadoAgencia.Gestionado)
            'Gestion del estado
            Select Case oSolAgen.Estado
                Case ELL.SolicitudAgencia.EstadoAgencia.solicitado
                    If (chbGestionado.Checked) Then
                        oSolAgen.Estado = ELL.SolicitudAgencia.EstadoAgencia.Gestionado
                    Else
                        oSolAgen.Estado = ELL.SolicitudAgencia.EstadoAgencia.Tramite
                    End If
                Case ELL.SolicitudAgencia.EstadoAgencia.Tramite
                    If (chbGestionado.Checked) Then oSolAgen.Estado = ELL.SolicitudAgencia.EstadoAgencia.Gestionado
                Case ELL.SolicitudAgencia.EstadoAgencia.Gestionado
                    If (Not chbGestionado.Checked) Then oSolAgen.Estado = ELL.SolicitudAgencia.EstadoAgencia.Tramite
            End Select
            oSolAgen.Albaranes = Albaranes
            solAgenciaBLL.Save(oSolAgen, 0) 'No hace falta especificar el id del usuario creador del viaje
            Dim mensa As String = "AGENCIA: Se han guardado los datos de la solicitud de agencia del viaje (" & lblIdViaje.Text & ")."
            'Comprobamos si ha cambiado de estado
            If (estadoOriginal <> oSolAgen.Estado) Then
                mensa &= "Su nuevo estado es: " & [Enum].GetName(GetType(ELL.SolicitudAgencia.EstadoAgencia), oSolAgen.Estado)
            End If
            log.Info(mensa)
            Volver()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            log.Error("Error al guardar los datos de la solicitud de agencia del viaje (" & lblIdViaje.Text & ")", ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("errGuardar")
        End Try
    End Sub

    ''' <summary>
    ''' Redirecciona al presupuesto
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub lnkVerPresupuesto_Click(sender As Object, e As EventArgs) Handles lnkVerPresupuesto.Click
        Dim presupNuevo As Boolean = (lnkVerPresupuesto.CommandArgument = "1")
        If (presupNuevo) Then
            Response.Redirect("PresupServiciosNew.aspx?idViaje=" & IdViaje, False)
        Else
            Response.Redirect("PresupServicios.aspx?idViaje=" & IdViaje, False)
        End If
    End Sub

    ''' <summary>
    ''' Vuelve al listado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Private Sub btnVolver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnVolver.Click
        Volver()
    End Sub

    ''' <summary>
    ''' Vuelve al listado
    ''' </summary>
    Private Sub Volver()
        Response.Redirect("SolicitudAgencia.aspx?returnIdViaje=" & IdViaje, False)
    End Sub

End Class