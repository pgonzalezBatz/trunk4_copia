Public Class VerLiquidacionesFacturas
    Inherits PageBase

    Private departmentInfo As List(Of String())

#Region "Page Load"

    ''' <summary>
    ''' Inicializa la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Liquidaciones/Facturas"
                pnlInfoLiq.Visible = False
                cargarCabeceraLiquidSolicitadas()
                btnAvisarEmail.OnClientClick = "$('#confirmEmail').modal('show'); return false;"
                If (Request.QueryString("idLiq") IsNot Nothing) Then
                    mostrarDetalle(CInt(Request.QueryString("idLiq")))
                End If
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se le indica lo que tiene que traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelMensaje) : itzultzaileWeb.Itzuli(labelSelLiq) : itzultzaileWeb.Itzuli(labelFTrans)
            itzultzaileWeb.Itzuli(labelEmpresa) : itzultzaileWeb.Itzuli(btnDescargar) : itzultzaileWeb.Itzuli(btnAvisarEmail)
            itzultzaileWeb.Itzuli(labelConfirmTitle) : itzultzaileWeb.Itzuli(labelConfirmMessage) : itzultzaileWeb.Itzuli(btnContinuar)
            itzultzaileWeb.Itzuli(labelCancelar)
        End If
    End Sub

#End Region

#Region "Mostrar detalle"

    ''' <summary>
    ''' Carga la cabecera de las liquidaciones solicitadas
    ''' </summary>    
    Private Sub cargarCabeceraLiquidSolicitadas()
        Dim hojasBLL As New BLL.HojasGastosBLL
        Dim lLiq As List(Of ELL.HojaGastos.Liquidacion.Cabecera) = hojasBLL.loadCabecerasLiquidacionesEmitidas(Master.IdPlantaGestion, ELL.HojaGastos.Liquidacion.TipoLiq.Comision_Servicios)
        If (lLiq IsNot Nothing AndAlso lLiq.Count > 0) Then
            lLiq = lLiq.OrderByDescending(Of Date)(Function(o) o.FechaEmision).ToList
            ddlLiq.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))
            Dim plantBLL As New SabLib.BLL.PlantasComponent
            Dim lPlantas As New List(Of SabLib.ELL.Planta)
            Dim oPlanta As SabLib.ELL.Planta
            For Each oLiq As ELL.HojaGastos.Liquidacion.Cabecera In lLiq
                oPlanta = lPlantas.Find(Function(o As SabLib.ELL.Planta) o.Id = oLiq.IdPlanta)
                If (oPlanta Is Nothing) Then
                    oPlanta = plantBLL.GetPlanta(oLiq.IdPlanta)
                    If (oPlanta IsNot Nothing) Then lPlantas.Add(oPlanta)
                End If
                ddlLiq.Items.Add(New ListItem(oLiq.FechaEmision.ToShortDateString & "-" & If(oPlanta IsNot Nothing, oPlanta.Nombre, itzultzaileWeb.Itzuli("Planta desconocida")), oLiq.id))
            Next
        Else
            ddlLiq.Items.Add(New ListItem(itzultzaileWeb.Itzuli("No existen registros"), Integer.MinValue))
        End If
    End Sub

    ''' <summary>
    ''' Carga el detalle de una liquidacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ddlLiq_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlLiq.SelectedIndexChanged
        Try
            If ((ddlLiq.SelectedValue) > 0) Then
                mostrarDetalle(CInt(ddlLiq.SelectedValue))
            Else
                pnlInfoLiq.Visible = False
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Seleccione un elemento")
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Muestra el detalle de la liquidacion
    ''' </summary>
    ''' <param name="idCab">Id de la cabecera</param>    
    Private Sub mostrarDetalle(ByVal idCab As Integer)
        Try
            pnlInfoLiq.Visible = True
            Dim hojasBLL As New BLL.HojasGastosBLL
            Dim plantBLL As New SabLib.BLL.PlantasComponent
            ddlLiq.SelectedValue = idCab
            Dim oCabecera As ELL.HojaGastos.Liquidacion.Cabecera = hojasBLL.loadCabeceraLiquidacionEmitida(idCab, ELL.HojaGastos.Liquidacion.TipoLiq.Comision_Servicios)
            lblFTrans.Text = oCabecera.FechaEmision.ToShortDateString
            lblEmpresa.Text = plantBLL.GetPlanta(oCabecera.IdPlanta).Nombre
            Dim lHojas As List(Of ELL.HojaGastos.Liquidacion) = hojasBLL.loadHojasLiquidacion(idCab, ELL.HojaGastos.Liquidacion.TipoLiq.Factura)
            lHojas = lHojas.OrderBy(Of String)(Function(o) o.Usuario.NombreCompleto).ToList
            gvHojasLiq.DataSource = lHojas
            gvHojasLiq.DataBind()
        Catch ex As Exception
            Throw New BatzException("Error al mostrar el detalle", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Descarga el fichero del banco
    ''' Si es de la liquidacion actual, mostrara lo que haya en el commandArgument
    ''' Si es de emisiones anteriores, hara la consulta y se descargara el fichero
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnDescargar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDescargar.Click
        Try
            Dim data() As Byte = Nothing
            Dim hojaBLL As New BLL.HojasGastosBLL
            Dim fecha As Date = CDate(ddlLiq.SelectedItem.Text.Split("-")(0)) 'Fecha-Nombre planta
            Dim extension As String = If(fecha < New Date(2016, 2, 1), ".txt", ".xml") 'Antes del 01/02/2016 eran txt. Ahora son xml
            data = hojaBLL.loadFicheroBancoLiq(CInt(ddlLiq.SelectedValue), ELL.HojaGastos.Liquidacion.TipoLiq.Comision_Servicios)
            log.Info("Se va a descargar el fichero del banco de comision de servicios de la fecha " & ddlLiq.SelectedItem.Text)
            Response.Clear()
            Response.AppendHeader("Content-Disposition", "attachment; filename=BidaiGastuak_ComisionServ" & fecha.ToShortDateString.Replace("/", "") & extension)
            Response.OutputStream.Write(data, 0, data.Length)
            Response.OutputStream.Flush()
            Response.OutputStream.Close()
            Response.End()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            If Not (ex.GetType.Name = "ThreadAbortException") Then
                log.Error("Error al descargar el ficheor del banco", ex)
                Master.MensajeError = itzultzaileWeb.Itzuli("Error al descargar el fichero del banco")
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Se avisa por email de que se ha realizado el pago
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnContinuar_Click(sender As Object, e As EventArgs) Handles btnContinuar.Click
        Dim bEnviado As Boolean = False
        If (ConfigurationManager.AppSettings("avisarPorEmail") = "1") Then
            Try
                Dim idCab As Integer = CInt(ddlLiq.SelectedValue)
                Dim perfBLL As New BLL.BidaiakBLL
                Dim hojasBLL As New BLL.HojasGastosBLL
                Dim emailsAccesoDirecto, emailsAccesoPortal, subject, body, bodyEmail, sHojasLiq As String
                Dim idRecurso As Integer = CInt(ConfigurationManager.AppSettings("RecursoWeb_Admon"))
                emailsAccesoDirecto = String.Empty : emailsAccesoPortal = String.Empty : sHojasLiq = String.Empty
                Dim paramBLL As New SabLib.BLL.ParametrosBLL
                Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor
                Dim lLiquidaciones As List(Of ELL.HojaGastos.Liquidacion) = hojasBLL.loadHojasLiquidacion(idCab, ELL.HojaGastos.Liquidacion.TipoLiq.Factura)
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                For Each oLiq As ELL.HojaGastos.Liquidacion In lLiquidaciones
                    Try
                        If (oLiq.Usuario IsNot Nothing) Then
                            Dim sPerfil As String() = perfBLL.loadProfile(Master.IdPlantaGestion, oLiq.Usuario.Id, idRecurso)
                            If (sPerfil(1) = "0") Then 'Acceso por portal
                                If (emailsAccesoPortal <> String.Empty) Then emailsAccesoPortal &= ";"
                                emailsAccesoPortal = oLiq.Usuario.Email
                            Else 'Acceso directo
                                If (emailsAccesoDirecto <> String.Empty) Then emailsAccesoDirecto &= ";"
                                emailsAccesoDirecto = oLiq.Usuario.Email
                            End If
                        End If
                        If (emailsAccesoDirecto = String.Empty AndAlso emailsAccesoPortal = String.Empty) Then
                            log.Warn("AVISO_LIQUIDACION_FACT: No se ha encontrado ningun email para avisar de la liquidacion del usuario (" & oLiq.Usuario.Id & ")")
                        Else
                            sHojasLiq = String.Join(",", oLiq.Hojas.Select(Function(o) o.IdHoja))
                            body = "Se ha iniciado el tramite de la liquidacion de las hojas de gastos [" & sHojasLiq & "] .<br />En breve recibirá el cobro por transferencia bancaria"
                            subject = "Liquidacion de hojas de gastos"
                            If (emailsAccesoPortal <> String.Empty) Then
                                bodyEmail = PageBase.getBodyHmtl("Liquidacion", String.Empty, body, String.Empty, True)
                                SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoPortal, subject, bodyEmail, serverEmail)
                                log.Info("AVISO_LIQUIDACION_FACT:Se ha enviado un email a " & oLiq.Usuario.NombreCompleto & " para avisarle de que se le han liquidado sus hojas (" & sHojasLiq & ") con acceso por el portal => " & emailsAccesoPortal)
                                bEnviado = True
                            End If
                            If (emailsAccesoDirecto <> String.Empty) Then
                                bodyEmail = PageBase.getBodyHmtl("Liquidacion", String.Empty, body, String.Empty, False)
                                SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoDirecto, subject, bodyEmail, serverEmail)
                                log.Info("AVISO_LIQUIDACION_FACT:Se ha enviado un email a " & oLiq.Usuario.NombreCompleto & " para avisarle de que se le han liquidado sus hojas (" & sHojasLiq & ") con acceso directo => " & emailsAccesoDirecto)
                                bEnviado = True
                            End If
                        End If
                    Catch ex As Exception
                        log.Error("AVISO_LIQUIDACION_FACT: No se ha podido avisar al usuario " & oLiq.Usuario.Id, ex)
                    End Try
                Next
            Catch ex As Exception
                log.Error("AVISO_LIQUIDACION_FACT: Error al intentar avisar a los usuarios de que se ha iniciado el tramite de la liquidacion", ex)
            End Try
        End If
        If (bEnviado) Then
            Master.MensajeInfo = itzultzaileWeb.Itzuli("Se ha avisado a los usuarios")
        Else
            Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("No se ha podido avisar a ningun usuario")
        End If
    End Sub

    ''' <summary>
    ''' Enlaza los datos con el gridview
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvHojasLiq_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvHojasLiq.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oLiq As ELL.HojaGastos.Liquidacion = e.Row.DataItem
            Dim lblPersona As Label = CType(e.Row.FindControl("lblPersona"), Label)
            Dim hlViajeHoja As HyperLink = CType(e.Row.FindControl("hlViajeHoja"), HyperLink)
            Dim lblOrganizacion As Label = CType(e.Row.FindControl("lblOrganizacion"), Label)
            Dim lblLantegi As Label = CType(e.Row.FindControl("lblLantegi"), Label)
            Dim infoEpsilon As String()
            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Dim epsilonBLL As New BLL.Epsilon(Master.IdPlantaGestion)
            If (departmentInfo Is Nothing) Then departmentInfo = New List(Of String())
            If (oLiq.Usuario.IdDepartamento <> String.Empty) Then
                Dim info As String() = departmentInfo.Find(Function(o As String()) o(0) = oLiq.Usuario.IdDepartamento) '0:IdDepto,1:CodOrg,2:Organizacion,3:Lantegi
                If (info IsNot Nothing) Then
                    If (info(1) = "00985") Then lblOrganizacion.Text = info(2) 'Unicamente si es de sistemas, se pintara la organizacion
                    lblLantegi.Text = info(3)
                Else
                    info = New String() {oLiq.Usuario.IdDepartamento, "", "", ""}
                    infoEpsilon = epsilonBLL.getInfoOrdenDepartamento(oLiq.Usuario.IdDepartamento)
                    If (infoEpsilon IsNot Nothing) Then
                        info(1) = infoEpsilon(0)
                        If (info(1) = "00985") Then
                            info(2) = infoEpsilon(3)
                            lblOrganizacion.Text = infoEpsilon(3) 'Unicamente si es de sistemas, se pintara la organizacion
                        End If
                    End If
                    info(3) = epsilonBLL.getInfoLantegi(oLiq.Usuario.IdDepartamento)
                    lblLantegi.Text = info(3)
                    departmentInfo.Add(info)
                End If
            End If
            lblPersona.Text = oLiq.Usuario.NombreCompleto & " (" & oLiq.Usuario.CodPersona & ")"
            If (oLiq.Usuario.DadoBaja) Then e.Row.CssClass = "danger"
            CType(e.Row.FindControl("lblIdUser"), Label).Text = oLiq.Usuario.Id
            Dim myHoja As ELL.HojaGastos.Liquidacion.Hoja = oLiq.Hojas.First
            hlViajeHoja.NavigateUrl = "~/Viaje/HojaGastos.aspx?id=" & myHoja.IdHoja & "&orig=LIQ_FAC"
            If (myHoja.IdHojaLibre <> Integer.MinValue) Then
                hlViajeHoja.Text = "H" & myHoja.IdHojaLibre
            ElseIf (myHoja.IdViaje <> Integer.MinValue) Then
                hlViajeHoja.Text = "V" & myHoja.IdViaje
            End If
            CType(e.Row.FindControl("lblId"), Label).Text = myHoja.IdHoja
            CType(e.Row.FindControl("lblImportes"), Label).Text = myHoja.ImporteEuros
            CType(e.Row.FindControl("lblLiquidacion"), Label).Text = myHoja.ImporteEuros & " €"
            CType(e.Row.FindControl("lblFVal"), Label).Text = myHoja.FechaValidacion.ToShortDateString
            CType(e.Row.FindControl("lblCuenta"), Label).Text = If(oLiq.CuentaContable > 0, oLiq.CuentaContable, String.Empty)
        ElseIf (e.Row.RowType = DataControlRowType.Footer) Then
            Dim lblTotal As Label = CType(e.Row.FindControl("lblTotal"), Label)
            Dim labelTotal As Label = CType(e.Row.FindControl("labelTotal"), Label)
            Dim total As Decimal = 0
            For Each gvr As GridViewRow In gvHojasLiq.Rows
                Dim lblImp As Label = CType(gvr.FindControl("lblImportes"), Label)
                total += DecimalValue(lblImp.Text.Trim)
            Next
            If (labelTotal IsNot Nothing) Then itzultzaileWeb.Itzuli(labelTotal)
            lblTotal.Text = total & "€"
        End If
    End Sub

#End Region

End Class