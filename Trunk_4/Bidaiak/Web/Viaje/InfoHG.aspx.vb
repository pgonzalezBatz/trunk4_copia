Public Class InfoHG
    Inherits PageBase

#Region "Eventos pagina"

    ''' <summary>
    ''' Inicializa la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Dim idHoja As Integer = Request.QueryString("idH")
                btnVolverHG.CommandArgument = idHoja
                pnlHGEnviada.Visible = False : pnlHGValidada.Visible = False : pnlError.Visible = False
                btnImprimir.Visible = False
                Dim hojasBLL As New BLL.HojasGastosBLL
                Dim oHoja As ELL.HojaGastos = hojasBLL.loadHoja(idHoja)
                Dim idH As String = If(oHoja.IdViaje <> Integer.MinValue, "V" & oHoja.IdViaje, "H" & oHoja.IdSinViaje)
                Dim bExento As Boolean = False
                'Se comprueba si la hoja pertenece a un viaje y en caso afirmativo, si su actividad esta exenta de IRPF tendra que haber recibido la tarjeta de embarque
                If (oHoja.IdViaje <> Integer.MinValue) Then
                    Dim viajeBLL As New BLL.ViajesBLL
                    Dim activBLL As New BLL.ActividadesBLL
                    Dim lInteg As List(Of ELL.Viaje.Integrante) = viajeBLL.loadIntegrantes(oHoja.IdViaje)
                    Dim oInteg As ELL.Viaje.Integrante = lInteg.Find(Function(o As ELL.Viaje.Integrante) o.Usuario.Id = oHoja.Usuario.Id)
                    Dim oActiv As ELL.Actividad = activBLL.loadInfo(oInteg.IdActividad)
                    If (oActiv.ExentaIRPF) Then
                        Dim viajesBLL As New BLL.ViajesBLL
                        Dim oViaje As ELL.Viaje = viajesBLL.loadInfo(oHoja.IdViaje)
                        bExento = (oViaje.Nivel <> ELL.Viaje.eNivel.Nacional)
                    End If
                End If
                'Comprobamos cual es su tipo de liquidacion para poner si tiene que entregarlo en administracion o en RRHH
                Dim bidaiakBLL As New BLL.BidaiakBLL
                Dim tipoLiqLugarEntrega As String = "administracion"
                If (bidaiakBLL.TipoLiquidacionUser(oHoja.Usuario.Id, Master.IdPlantaGestion) = ELL.ConvenioCategoria.TipoLiq.Factura) Then tipoLiqLugarEntrega = "<span style='color:#FF0000'>RRHH (!!NO SUBIR A ADMINISTRACION!!)</span>"
                If (oHoja.Usuario.Id = Master.Ticket.IdUser And oHoja.Estado = ELL.HojaGastos.eEstado.Enviada) Then
                    pnlHGEnviada.Visible = True
                    If (oHoja.IdViaje <> Integer.MinValue) Then
                        btnVolver.Text = itzultzaileWeb.Itzuli("Volver al listado de viajes")
                        btnVolver.CommandArgument = "HGV"
                    Else
                        btnVolver.Text = itzultzaileWeb.Itzuli("Volver al listado de hojas de gastos")
                        btnVolver.CommandArgument = "HGS"
                    End If
                    lblInfoEnviada.Text = itzultzaileWeb.Itzuli("La hoja de gastos [X] se ha enviado a [Y] para su validacion".Replace("[X]", idH).Replace("[Y]", oHoja.Validador.NombreCompleto)).ToUpper
                    lblInfoEnviada.Text &= "<br /><br />" & itzultzaileWeb.Itzuli("Una vez validada, debera entregar en " & tipoLiqLugarEntrega).ToUpper & ":"
                    lblInfoEnviada.Text &= "<br />&nbsp;&nbsp;&nbsp;-" & itzultzaileWeb.Itzuli("Hoja de gastos impresa").ToUpper
                    lblInfoEnviada.Text &= "<br />&nbsp;&nbsp;&nbsp;-" & itzultzaileWeb.Itzuli("Tickets de gastos").ToUpper
                    If (bExento) Then lblInfoEnviada.Text &= "<br />&nbsp;&nbsp;&nbsp;-" & itzultzaileWeb.Itzuli("Al estar exento de IRPF, tarjetas de embarque, facturas, etc..").ToUpper
                ElseIf (oHoja.Validador.Id = Master.Ticket.IdUser And oHoja.Estado = ELL.HojaGastos.eEstado.Validada) Then
                    pnlHGValidada.Visible = True : btnImprimir.Visible = True
                    lblInfoValidada.Text = itzultzaileWeb.Itzuli("La hoja de gastos [X] del usuario [Y] se ha marcado como validada".Replace("[X]", idH).Replace("[Y]", oHoja.Usuario.NombreCompleto)).ToUpper
                    lblInfoValidada.Text &= "<br /><br />" & itzultzaileWeb.Itzuli("El trabajador debera entregar en " & tipoLiqLugarEntrega).ToUpper & ":"
                    lblInfoValidada.Text &= "<br />&nbsp;&nbsp;&nbsp;-" & itzultzaileWeb.Itzuli("Hoja de gastos impresa").ToUpper & "(" & itzultzaileWeb.Itzuli("La puede imprimir usted").ToUpper & ")"
                    lblInfoValidada.Text &= "<br />&nbsp;&nbsp;&nbsp;-" & itzultzaileWeb.Itzuli("Tickets de gastos").ToUpper
                    If (bExento) Then lblInfoValidada.Text &= "<br />&nbsp;&nbsp;&nbsp;-" & itzultzaileWeb.Itzuli("Al estar exento de IRPF, tarjetas de embarque, facturas, etc..").ToUpper
                    If (Request.QueryString("Origen") = "ValHG") Then
                        btnVolver.Text = "Volver a la pagina de validaciones"
                    Else
                        If (oHoja.IdViaje <> Integer.MinValue) Then
                            btnVolver.Text = itzultzaileWeb.Itzuli("Volver al listado de viajes")
                            btnVolver.CommandArgument = "HGV"
                        Else
                            btnVolver.Text = itzultzaileWeb.Itzuli("Volver al listado de hojas de gastos")
                            btnVolver.CommandArgument = "HGS"
                        End If
                    End If
                ElseIf (oHoja.Usuario.Id = Master.Ticket.IdUser And oHoja.Estado = ELL.HojaGastos.eEstado.Validada) Then
                    pnlHGValidada.Visible = True : btnImprimir.Visible = True
                    lblInfoValidada.Text = itzultzaileWeb.Itzuli("Como su validador ya habia validado los gastos de visa, ya puede imprimir la hoja de gastos [X]".Replace("[X]", idH)).ToUpper
                    lblInfoValidada.Text &= "<br /><br />" & itzultzaileWeb.Itzuli("Debera entregar en " & tipoLiqLugarEntrega).ToUpper & ":"
                    lblInfoValidada.Text &= "<br />&nbsp;&nbsp;&nbsp;-" & itzultzaileWeb.Itzuli("Hoja de gastos impresa").ToUpper
                    lblInfoValidada.Text &= "<br />&nbsp;&nbsp;&nbsp;-" & itzultzaileWeb.Itzuli("Tickets de gastos").ToUpper
                    If (bExento) Then lblInfoValidada.Text &= "<br />&nbsp;&nbsp;&nbsp;-" & itzultzaileWeb.Itzuli("Al estar exento de IRPF, tarjetas de embarque, facturas, etc..").ToUpper
                Else
                    pnlError.Visible = True
                End If
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
    Private Sub HojaGastos_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(btnVolverHG) : itzultzaileWeb.Itzuli(btnVolver) : itzultzaileWeb.Itzuli(btnImprimir)
            itzultzaileWeb.Itzuli(labelError)
        End If
    End Sub

#End Region

#Region "Botones"

    ''' <summary>
    ''' Vuelve al detalle de la hoja de gastos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnVolverHG_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnVolverHG.Click
        Dim hojasBLL As New BLL.HojasGastosBLL
        Dim oHoja As ELL.HojaGastos = hojasBLL.loadHoja(CInt(Request.QueryString("idH")), False)
        If (oHoja.IdViaje <> Integer.MinValue) Then
            Response.Redirect("HojaGastos.aspx?idViaje=" & oHoja.IdViaje, False)
        Else
            Response.Redirect("HojaGastos.aspx?id=" & oHoja.Id, False)
        End If
    End Sub

    ''' <summary>
    ''' Vuelve a la pagina de la que procedia
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnVolver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnVolver.Click
        Select Case Request.QueryString("Origen")
            Case "ValHG"
                Response.Redirect("..\Validacion\ValHojasGastos.aspx", False)
            Case Else
                If (btnVolver.CommandArgument = "HGS") Then
                    Response.Redirect("HojasGastosSinViaje.aspx", False)
                Else
                    Response.Redirect("Viajes.aspx", False)
                End If
        End Select
    End Sub

    ''' <summary>
    ''' Manda imprimir la hoja de gastos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnImprimir_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnImprimir.Click
        Dim fs As IO.FileStream = Nothing
        Try
            Dim hojasBLL As New BLL.HojasGastosBLL
            Dim oHoja As ELL.HojaGastos = hojasBLL.loadHoja(CInt(Request.QueryString("idH")), False)
            Dim idH As String = If(oHoja.IdViaje <> Integer.MinValue, "V" & oHoja.IdViaje, "H" & oHoja.IdSinViaje)
            Dim ruta As String = BLL.GenerarPDF.HojaGastos(oHoja.Id, Master.IdPlantaGestion, Master.Ticket.Culture, Session.SessionID, Session("Perfil"))
            fs = IO.File.OpenRead(ruta)
            Dim bytes(fs.Length) As Byte
            fs.Read(bytes, 0, CInt(fs.Length))
            fs.Close()
            log.Info("HOJA DE GASTOS: El validador " & Master.Ticket.NombreCompleto & " a generado el pdf para imprimir la HG [" & idH & "]")
            Response.Clear()
            Response.AppendHeader("Content-Disposition", "attachment; filename=Hoja_Gastos_" & idH & ".pdf")
            Response.OutputStream.Write(bytes, 0, bytes.Length)
            Response.OutputStream.Flush()
            Response.OutputStream.Close()
            Response.End()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Finally
            If (fs IsNot Nothing) Then fs.Close()
        End Try
    End Sub

#End Region

End Class