Public Class InfoViaje
    Inherits PageBase

    ''' <summary>
    ''' Carga de la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Master.SetTitle = "Informacion del viaje"
            Dim idViaje As Integer = Request.QueryString("id")
            pnlNotificaciones.Visible = False
            btnVolver.CommandArgument = idViaje
            lblIdViajeGenerado.Text = "V" & idViaje
            lblMensajeResul.Text = itzultzaileWeb.Itzuli("Solicitud de viaje guardada con exito") & "<br />" & itzultzaileWeb.Itzuli("Este numero de viaje sera el identificador por el que se podran consultar los datos del viaje y el que habra que indicar al entregar las hojas de gastos")
            btnAgregarDocCliente.Visible = False : divDocsCli.Visible = False
            btnAgregarDocCliente.CommandArgument = "0" : lnkDocsCliente.CommandArgument = "0"
            Dim bVisualizarDocCliente As Boolean = False
            Try
                Dim viajesBLL As New BLL.ViajesBLL
                Dim oViaje As ELL.Viaje = viajesBLL.loadInfo(idViaje)
                'Se comprueba si el viaje es de tipo de desplazamiento de cliente o de otros, si tiene integrantes con actividades exentas, si su viaje es al extranjero y no ha añadido ningun documento.05/06/13: Se añade lo de otros
                If (oViaje.Nivel <> ELL.Viaje.eNivel.Nacional AndAlso (oViaje.TipoDesplazamiento = ELL.Viaje.TipoDesplaz.Cliente OrElse oViaje.TipoDesplazamiento = ELL.Viaje.TipoDesplaz.Otros)) Then
                    If (oViaje.DocumentosCliente Is Nothing OrElse (oViaje.DocumentosCliente IsNot Nothing AndAlso oViaje.DocumentosCliente.Count = 0)) Then
                        Dim activBLL As New BLL.ActividadesBLL
                        For Each integ As ELL.Viaje.Integrante In oViaje.ListaIntegrantes
                            If (oViaje.Nivel <> ELL.Viaje.eNivel.Nacional AndAlso activBLL.loadInfo(integ.IdActividad).ExentaIRPF) Then
                                btnAgregarDocCliente.CommandArgument = idViaje
                                bVisualizarDocCliente = True
                                Exit For
                            End If
                        Next
                    End If
                End If
                'Si se tiene que visualizar el boton de agregar doc al cliente y es la primera vez, se comprueba si tiene algun documento en xbat
                If (bVisualizarDocCliente) Then
                    Dim xbatBLL As New BLL.XbatBLL
                    Dim lProyXBAT As List(Of ELL.DocumentoProyecto)
                    Dim lProy As List(Of ELL.Viaje.Proyecto) = viajesBLL.loadProyectos(idViaje)
                    If (lProy IsNot Nothing AndAlso lProy.Count > 0) Then
                        For Each oProy As ELL.Viaje.Proyecto In lProy
                            If (oProy.IdPrograma > 0) Then
                                lProyXBAT = xbatBLL.GetDocumentosProyecto(oProy.IdPrograma)
                                If (lProyXBAT IsNot Nothing AndAlso lProyXBAT.Count > 0) Then
                                    divDocsCli.Visible = True : lnkDocsCliente.CommandArgument = idViaje
                                    Exit For
                                End If
                            End If
                        Next
                    End If
                    If (Not divDocsCli.Visible) Then btnAgregarDocCliente.Visible = True
                End If
            Catch ex As Exception
                log.Error("Ha ocurrido un error comprobando si deberia subir algun documento de cliente", ex)
            End Try
            Dim bNotifInteg As Boolean = If(Request.QueryString("nIn") Is Nothing, False, CBool(Request.QueryString("nIn")))
            Dim bNotifAgencia As Boolean = If(Request.QueryString("nAg") Is Nothing, False, CBool(Request.QueryString("nAg")))
            Dim bNotifFinan As Boolean = If(Request.QueryString("nFi") Is Nothing, False, CBool(Request.QueryString("nFi")))
            Dim bNotifGerente As Boolean = If(Request.QueryString("nGe") Is Nothing, False, CBool(Request.QueryString("nGe")))
            If (bNotifInteg OrElse bNotifAgencia OrElse bNotifFinan OrElse bNotifGerente) Then
                pnlNotificaciones.Visible = True
                If (bNotifInteg) Then AddNotif(itzultzaileWeb.Itzuli("Integrantes del viaje"))
                If (bNotifAgencia) Then AddNotif(itzultzaileWeb.Itzuli("Personal de agencia de viajes"))
                If (bNotifFinan) Then AddNotif(itzultzaileWeb.Itzuli("Personal de financiero"))
                If (bNotifGerente) Then AddNotif(itzultzaileWeb.Itzuli("Gerente de la planta filial"))
            End If
        End If
    End Sub

    ''' <summary>
    ''' Traducciones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub InfoViaje_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        itzultzaileWeb.Itzuli(btnVolver) : itzultzaileWeb.Itzuli(btnAgregarDocCliente)
        itzultzaileWeb.Itzuli(lnkDocsCliente) : itzultzaileWeb.Itzuli(labelInfo)
    End Sub

    ''' <summary>
    ''' Añade la notificacion al ul del resultado
    ''' </summary>
    ''' <param name="notif">Notificacion</param>
    Private Sub AddNotif(notif As String)
        Dim newLi As New HtmlGenericControl("li")
        newLi.Attributes.Add("class", "list-group-item text-info")
        newLi.InnerText = notif
        ulNotif.Controls.Add(newLi)
    End Sub

    ''' <summary>
    ''' Se muestra el detalle del viaje para añadir documentos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnAgregarDocCliente_Click(sender As Object, e As EventArgs) Handles btnAgregarDocCliente.Click, lnkDocsCliente.Click
        Response.Redirect("SolicitudViaje.aspx?id=" & btnVolver.CommandArgument, False)
    End Sub

    ''' <summary>
    ''' Vuelve al listado de solicitudes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnVolver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnVolver.Click
        Dim url As String = "Viajes.aspx"
        Dim idViaje As Integer = CInt(btnVolver.CommandArgument)
        Dim origViajes As Boolean = If(Request.QueryString("ov") Is Nothing, False, CBool(Request.QueryString("ov")))
        If (hasProfile(BLL.BidaiakBLL.Profiles.Agencia)) Then
            url = "~\Agencia\SolicitudAgencia.aspx?idViaje=" & idViaje
        ElseIf (hasProfile(BLL.BidaiakBLL.Profiles.Financiero) AndAlso Not origViajes) Then
            url = "~\Financiero\Anticipos\GestionAnticipos.aspx?idViaje=" & idViaje
        End If
        Response.Redirect(url, False)
    End Sub

End Class