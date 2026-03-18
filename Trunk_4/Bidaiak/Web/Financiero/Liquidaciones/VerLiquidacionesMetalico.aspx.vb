Public Class VerLiquidacionesMetalico
    Inherits PageBase

#Region "Page Load"

    Private departmentInfo As List(Of String())

    ''' <summary>
    ''' Inicializa la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Liquidaciones emitidas"
                pnlInfoLiq.Visible = False
                cargarFechasEmision()
                If (Request.QueryString("idLiq") IsNot Nothing) Then 'Se accede al detalle de una liquidacion
                    Dim idLiq As Integer = CInt(Request.QueryString("idLiq"))
                    ddlLiq.SelectedValue = idLiq
                    VerDetalleLiquidacion(idLiq)
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
            itzultzaileWeb.Itzuli(labelMensaje) : itzultzaileWeb.Itzuli(labelSelLiq) : itzultzaileWeb.Itzuli(btnDescargar)
        End If
    End Sub

#End Region

#Region "Mostrar detalle"

    ''' <summary>
    ''' Carga las fechas de emision anteriores a la fecha de hoy y superiores a 2017
    ''' </summary>    
    Private Sub cargarFechasEmision()
        Try
            If (ddlLiq.Items.Count = 0) Then
                ddlLiq.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))
                Dim hojasBLL As New BLL.HojasGastosBLL
                Dim lLiq As List(Of ELL.HojaGastos.Liquidacion.Cabecera) = hojasBLL.loadCabecerasLiquidacionesEmitidas(Master.IdPlantaGestion, ELL.HojaGastos.Liquidacion.TipoLiq.Metalico)
                If (lLiq IsNot Nothing AndAlso lLiq.Count > 0) Then
                    lLiq = lLiq.FindAll(Function(o) o.FechaEmision > New Date(2017, 12, 31))
                    lLiq = lLiq.OrderByDescending(Of Date)(Function(o) o.FechaEmision).ToList
                    For Each sLiq As ELL.HojaGastos.Liquidacion.Cabecera In lLiq
                        ddlLiq.Items.Add(New ListItem(sLiq.FechaEmision, sLiq.id))
                    Next
                End If
            End If
            ddlLiq.SelectedValue = Integer.MinValue
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al cargar las liquidaciones emitidas", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Al seleccionar una fecha, se muestra el grid con la liquidacion de esa fecha
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub ddlLiq_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlLiq.SelectedIndexChanged
        Try
            If (ddlLiq.SelectedValue = Integer.MinValue) Then
                pnlInfoLiq.Visible = False
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Seleccione uno")
            Else
                VerDetalleLiquidacion(CInt(ddlLiq.SelectedValue))
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            log.Error("Error al buscar un fichero de liquidacion de una fecha pasada " & ddlLiq.SelectedItem.Text, ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al buscar")
        End Try
    End Sub

    ''' <summary>
    ''' Muestra el detalle de una liquidacion
    ''' </summary>
    ''' <param name="idLiq">Id de la liquidacion</param>
    Private Sub VerDetalleLiquidacion(ByVal idLiq As Integer)
        Dim hojasGastosBLL As New BLL.HojasGastosBLL
        pnlInfoLiq.Visible = True
        Dim lLiquidaciones As List(Of ELL.HojaGastos.Liquidacion) = hojasGastosBLL.loadHojasLiquidacion(idLiq, ELL.HojaGastos.Liquidacion.TipoLiq.Metalico)
        If (lLiquidaciones IsNot Nothing) Then lLiquidaciones = lLiquidaciones.OrderBy(Function(o) o.Usuario.NombreCompleto).ToList
        btnDescargar.CommandArgument = idLiq
        gvHojasLiq.DataSource = lLiquidaciones
        gvHojasLiq.DataBind()
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
            Dim fecha As Date = CDate(ddlLiq.SelectedItem.Text)
            Dim extension As String = ".xml" 'Antes del 01/02/2016 era un txt pero esas fechas no se muestran
            data = hojaBLL.loadFicheroBancoLiq(CInt(ddlLiq.SelectedValue), ELL.HojaGastos.Liquidacion.TipoLiq.Metalico)
            log.Info("Se va a descargar el fichero del banco de la fecha " & ddlLiq.SelectedItem.Text)
            Response.Clear()
            Response.AppendHeader("Content-Disposition", "attachment; filename=BidaiGastuak" & fecha.ToShortDateString.Replace("/", "") & extension)
            Response.OutputStream.Write(data, 0, data.Length)
            Response.OutputStream.Flush()
            Response.OutputStream.Close()
            Response.End()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            If Not (ex.GetType.Name = "ThreadAbortException") Then
                Master.MensajeError = itzultzaileWeb.Itzuli("Error al descargar el fichero del banco")
            End If
        End Try
    End Sub

#End Region

#Region "Gridview"

    ''' <summary>
    ''' Enlaza los datos con el gridview
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvHojasLiq_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvHojasLiq.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header OrElse e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oLiq As ELL.HojaGastos.Liquidacion = e.Row.DataItem
            Dim hlViajeHoja As HyperLink = CType(e.Row.FindControl("hlViajeHoja"), HyperLink)
            Dim lblOrganizacion As Label = CType(e.Row.FindControl("lblOrganizacion"), Label)
            Dim lblLantegi As Label = CType(e.Row.FindControl("lblLantegi"), Label)
            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Dim infoEpsilon As String()
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
            CType(e.Row.FindControl("lblPersona"), Label).Text = oLiq.Usuario.NombreCompleto & " (" & oLiq.Usuario.CodPersona & ")"
            If (oLiq.Usuario.DadoBaja) Then e.Row.CssClass = "danger"
            Dim myHoja As ELL.HojaGastos.Liquidacion.Hoja = oLiq.Hojas.First
            hlViajeHoja.NavigateUrl = "~/Viaje/HojaGastos.aspx?id=" & myHoja.IdHoja & "&orig=LIQ"
            If (myHoja.IdHojaLibre <> Integer.MinValue) Then
                hlViajeHoja.Text = "H" & myHoja.IdHojaLibre
            ElseIf (myHoja.IdViaje <> Integer.MinValue) Then
                hlViajeHoja.Text = "V" & myHoja.IdViaje
            End If
            CType(e.Row.FindControl("lblLiquidacion"), Label).Text = myHoja.ImporteEuros & " €"
            CType(e.Row.FindControl("lblImportes"), Label).Text = myHoja.ImporteEuros
            CType(e.Row.FindControl("lblFVal"), Label).Text = myHoja.FechaValidacion.ToShortDateString
            CType(e.Row.FindControl("lblCuenta"), Label).Text = If(oLiq.CuentaContable > 0, oLiq.CuentaContable, String.Empty)
        ElseIf (e.Row.RowType = DataControlRowType.Footer) Then
            Dim total As Decimal = 0
            For Each gvr As GridViewRow In gvHojasLiq.Rows
                Dim lblImp As Label = CType(gvr.FindControl("lblImportes"), Label)
                total += DecimalValue(lblImp.Text.Trim)
            Next
            itzultzaileWeb.Itzuli(CType(e.Row.FindControl("labelTotal"), Label))
            CType(e.Row.FindControl("lblTotal"), Label).Text = total & "€"
        End If
    End Sub

#End Region

End Class