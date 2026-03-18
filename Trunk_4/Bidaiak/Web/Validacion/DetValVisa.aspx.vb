Public Class DetValVisa
    Inherits PageBase

#Region "Properties"

    ''' <summary>
    ''' Mes a mostrar
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
    ''' Año a mostrar
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

    ''' <summary>
    ''' Id del usuario
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public Property IdUser As Integer
        Get
            Return CInt(ViewState("IdUser"))
        End Get
        Set(ByVal value As Integer)
            ViewState("IdUser") = value
        End Set
    End Property

    ''' <summary>
    ''' Ver sin validar. Es para saber si luego hay que mostrar solo los no validados o regirnos por fechas
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public Property VerSinValidar As Boolean
        Get
            Return CInt(ViewState("vsv"))
        End Get
        Set(ByVal value As Boolean)
            ViewState("vsv") = value
        End Set
    End Property

    Private numGastosSinJustificar As Integer

#End Region

#Region "Page Load"

    ''' <summary>
    ''' Carga la pagina con las hojas de gastos entre las fechas del mes actual
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Detalle de los gastos de VISA de una persona"
                IdUser = CInt(Request.QueryString("IdUser"))
                Mes = CInt(Request.QueryString("Mes"))
                Año = CInt(Request.QueryString("Anio"))
                VerSinValidar = CBool(Request.QueryString("vsv"))
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                lblPersona.Text = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = IdUser}, False).NombreCompleto
                btnConforme.Enabled = True : btnNoConforme.Enabled = True
                BuscarGastosVisa()
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
    Private Sub DetValVisa_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelGastosDe) : itzultzaileWeb.Itzuli(btnConforme)
            itzultzaileWeb.Itzuli(btnNoConforme) : itzultzaileWeb.Itzuli(btnVolver)
            itzultzaileWeb.Itzuli(labelInfoSinJustif)
        End If
    End Sub

#End Region

#Region "Buscar hojas"

    ''' <summary>
    ''' Busca los gastos de visa de la gente al cargo del validador en la fecha seleccionada
    ''' </summary>    
    Private Sub BuscarGastosVisa()
        Dim visaBLL As New BLL.VisasBLL
        Dim fechaInicio, fechaFin As Date
        fechaInicio = New Date(Año, Mes, 1)
        fechaFin = New Date(Año, Mes, Date.DaysInMonth(Año, Mes))
        Dim lGastosVisa As List(Of ELL.Visa.Movimiento) = visaBLL.loadMovimientos(IdUser, Integer.MinValue, Master.IdPlantaGestion, fechaInicio, fechaFin, False)  'Pasar fechas
        lGastosVisa = lGastosVisa.FindAll(Function(o As ELL.Visa.Movimiento) o.IdResponsable = Master.Ticket.IdUser)  'No obtiene los suyos, solo a los que valida
        lGastosVisa = lGastosVisa.OrderBy(Of Date)(Function(o) o.Fecha).ToList
        btnConforme.Visible = (lGastosVisa.Count > 0) : btnNoConforme.Visible = btnConforme.Visible
        numGastosSinJustificar = 0
        gvVisa.DataSource = lGastosVisa : gvVisa.DataBind()
        rptVisa.DataSource = lGastosVisa : rptVisa.DataBind()
        divGastosSinJustif.Visible = (numGastosSinJustificar > 0)
    End Sub

#End Region

#Region "Gridview"

    ''' <summary>
    ''' Enlaza los controles
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvVisa_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvVisa.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header) Then
            Dim chbSelectAll As CheckBox = CType(e.Row.FindControl("chbSelectAll"), CheckBox)
            chbSelectAll.Attributes("onclick") = "ChangeAllCheckBoxStates(this.checked);"
            CheckBoxIDsArray.Text = chbSelectAll.ClientID  'Se guarda el id en esta variable para que luego en el footer, sepa cual es
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oVisa As ELL.Visa.Movimiento = e.Row.DataItem
            Dim chbMarcar As CheckBox = CType(e.Row.FindControl("chbMarcar"), CheckBox)
            Dim lblHoja As Label = CType(e.Row.FindControl("lblHoja"), Label)
            Dim imgEstado As Image = CType(e.Row.FindControl("imgEstado"), Image)
            Dim lblComentario As Label = CType(e.Row.FindControl("lblComentario"), Label)
            Dim imgMasComentarios As Image = CType(e.Row.FindControl("imgMasComentarios"), Image)
            Dim labelSinJustificar As Label = CType(e.Row.FindControl("labelSinJustificar"), Label)
            If (oVisa.IdViaje <> Integer.MinValue) Then
                Dim viajeBLL As New BLL.ViajesBLL
                Dim oViaje As ELL.Viaje = viajeBLL.loadInfo(oVisa.IdViaje, False, False, True)
                lblHoja.Text = "V" & oViaje.IdViaje & " - " & oViaje.Destino
                lblHoja.CssClass = "text-info"
            ElseIf (oVisa.IdHoja <> Integer.MinValue) Then
                Dim hojaBLL As New BLL.HojasGastosBLL
                Dim oHoja As ELL.HojaGastos = hojaBLL.loadHoja(oVisa.IdHoja, False)
                lblHoja.Text = "H" & oHoja.IdSinViaje
                lblHoja.CssClass = "text-primary"
            Else
                lblHoja.Text = itzultzaileWeb.Itzuli("Sin viaje")
                lblHoja.CssClass = "text-warning"
            End If
            lblComentario.Visible = False : imgMasComentarios.Visible = False
            If (oVisa.Comentarios <> String.Empty) Then
                lblComentario.Visible = True
                If (oVisa.Comentarios.Length > 70) Then
                    imgMasComentarios.Visible = True
                    lblComentario.Text = oVisa.Comentarios.Substring(0, 70).Replace(vbCrLf, "<br />") & "..."
                    imgMasComentarios.ToolTip = oVisa.Comentarios
                Else
                    lblComentario.Text = oVisa.Comentarios.Replace(vbCrLf, "<br />")
                End If
            End If
            CType(e.Row.FindControl("lblFecha"), Label).Text = oVisa.Fecha.ToShortDateString
            CType(e.Row.FindControl("lblImporte"), Label).Text = oVisa.ImporteMonedaGasto & " " & oVisa.MonedaGasto.Abreviatura
            CType(e.Row.FindControl("lblImporteEur"), Label).Text = oVisa.ImporteEuros & " " & oVisa.Moneda.Abreviatura
            labelSinJustificar.Visible = False
            Select Case oVisa.Estado
                Case ELL.Visa.Movimiento.eEstado.Cargado
                    imgEstado.ImageUrl = "~/App_Themes/Tema1/Images/EstadosValidacion/Sin_Validar.png"
                    imgEstado.ToolTip = itzultzaileWeb.Itzuli("Cargado")
                    imgEstado.Visible = False : labelSinJustificar.Visible = True
                    chbMarcar.Enabled = False : chbMarcar.ToolTip = itzultzaileWeb.Itzuli("No se puede validar porque no se ha justificado")
                    numGastosSinJustificar += 1
                Case ELL.Visa.Movimiento.eEstado.Conforme
                    imgEstado.ImageUrl = "~/App_Themes/Tema1/Images/EstadosValidacion/Validado.png"
                    imgEstado.ToolTip = itzultzaileWeb.Itzuli("Conforme")
                Case ELL.Visa.Movimiento.eEstado.No_Conforme
                    imgEstado.ImageUrl = "~/App_Themes/Tema1/Images/EstadosValidacion/Rechazada.png"
                    imgEstado.ToolTip = itzultzaileWeb.Itzuli("No conforme")
                Case ELL.Visa.Movimiento.eEstado.Liquidado
                    imgEstado.ImageUrl = "~/App_Themes/Tema1/Images/EstadosValidacion/Integrado.png"
                    imgEstado.ToolTip = itzultzaileWeb.Itzuli("Integrado")
                    chbMarcar.Enabled = False : chbMarcar.ToolTip = itzultzaileWeb.Itzuli("No se puede validar porque ya esta integrado")
                Case ELL.Visa.Movimiento.eEstado.Justificado
                    imgEstado.ImageUrl = "~/App_Themes/Tema1/Images/EstadosValidacion/Justificado.png"
                    imgEstado.ToolTip = itzultzaileWeb.Itzuli("Justificado")
            End Select
        ElseIf (e.Row.RowType = DataControlRowType.Footer) Then
            Dim ArrayValues As New List(Of String)
            'Se añade el primero el de la cabecera
            ArrayValues.Add(String.Concat("'", CheckBoxIDsArray.Text, "'"))  'En la cabecera, se ha guardado el nombre del check de la cabecera
            Dim cont As Integer = 0
            For Each gvr As GridViewRow In gvVisa.Rows
                Dim cb As CheckBox = CType(gvr.FindControl("chbMarcar"), CheckBox)
                'If the checkbox is unchecked, ensure that the Header CheckBox is unchecked
                cb.Attributes("onclick") = "ChangeHeaderAsNeeded();"
                'Add the CheckBox's ID to the client-side CheckBoxIDs array
                ArrayValues.Add(String.Concat("'", cb.ClientID, "'"))
                If (cb.Enabled) Then cont += 1
            Next
            btnConforme.Enabled = (cont > 0) : btnNoConforme.Enabled = btnConforme.Enabled
            CheckBoxIDsArray.Text = "<script type=""text/javascript"">" & vbCrLf &
                   "<!--" & vbCrLf &
                   String.Concat("var CheckBoxIDs = new Array(", String.Join(",", ArrayValues.ToArray()), ");") & vbCrLf &
                   "// -->" & vbCrLf &
                   "</script>"
        End If
    End Sub

    ''' <summary>
    ''' Paginacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvVisa_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvVisa.PageIndexChanging
        Try
            gvVisa.PageIndex = e.NewPageIndex
            BuscarGastosVisa()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Repeater"

    ''' <summary>
    ''' Se enlazan las visas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub rptVisa_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rptVisa.ItemDataBound
        If (e.Item.ItemType = ListItemType.Header) Then
            itzultzaileWeb.TraducirWebControls(e.Item.Controls)
        ElseIf (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim oVisa As ELL.Visa.Movimiento = e.Item.DataItem
            DirectCast(e.Item.FindControl("lblFechaRpt"), Label).Text = oVisa.Fecha.ToShortDateString
            Dim lblHoja As Label = DirectCast(e.Item.FindControl("lblHojaRpt"), Label)
            Dim chbMarcarRpt As CheckBox = DirectCast(e.Item.FindControl("chbMarcarRpt"), CheckBox)
            If (oVisa.IdViaje <> Integer.MinValue) Then
                Dim viajeBLL As New BLL.ViajesBLL
                Dim oViaje As ELL.Viaje = viajeBLL.loadInfo(oVisa.IdViaje, False, False, True)
                lblHoja.Text = "V" & oViaje.IdViaje & " - " & oViaje.Destino
                lblHoja.CssClass = "text-info"
            ElseIf (oVisa.IdHoja <> Integer.MinValue) Then
                Dim hojaBLL As New BLL.HojasGastosBLL
                Dim oHoja As ELL.HojaGastos = hojaBLL.loadHoja(oVisa.IdHoja, False)
                lblHoja.Text = "H" & oHoja.IdSinViaje
                lblHoja.CssClass = "text-primary"
            Else
                lblHoja.Text = itzultzaileWeb.Itzuli("Sin viaje")
                lblHoja.CssClass = "text-warning"
            End If
            DirectCast(e.Item.FindControl("lblSectorRpt"), Label).Text = oVisa.Sector
            DirectCast(e.Item.FindControl("lblLocalidadRpt"), Label).Text = oVisa.Localidad
            DirectCast(e.Item.FindControl("lblEstablecimientoRpt"), Label).Text = oVisa.Establecimiento
            DirectCast(e.Item.FindControl("lblComentarioRpt"), Label).Text = oVisa.Comentarios
            DirectCast(e.Item.FindControl("lblImporteRpt"), Label).Text = oVisa.ImporteEuros & " " & oVisa.Moneda.Abreviatura
            DirectCast(e.Item.FindControl("lblImporteEurRpt"), Label).Text = oVisa.ImporteMonedaGasto & " " & oVisa.MonedaGasto.Abreviatura
            Dim lblEstado As Label = DirectCast(e.Item.FindControl("lblEstadoRpt"), Label)
            Select Case oVisa.Estado
                Case ELL.Visa.Movimiento.eEstado.Cargado
                    lblEstado.Text = itzultzaileWeb.Itzuli("Cargado")
                    lblEstado.CssClass = "label label-default"
                    numGastosSinJustificar += 1
                    chbMarcarRpt.Enabled = False
                Case ELL.Visa.Movimiento.eEstado.Conforme
                    lblEstado.Text = itzultzaileWeb.Itzuli("Conforme")
                    lblEstado.CssClass = "label label-success"
                Case ELL.Visa.Movimiento.eEstado.No_Conforme
                    lblEstado.Text = itzultzaileWeb.Itzuli("No Conforme")
                    lblEstado.CssClass = "label label-danger"
                Case ELL.Visa.Movimiento.eEstado.Liquidado
                    lblEstado.Text = itzultzaileWeb.Itzuli("Integrado")
                    lblEstado.CssClass = "label label-info"
                    chbMarcarRpt.Enabled = False
                Case ELL.Visa.Movimiento.eEstado.Justificado
                    lblEstado.Text = itzultzaileWeb.Itzuli("Justificado")
                    lblEstado.CssClass = "label label-primary"
            End Select
            If (e.Item.ItemIndex Mod 2 <> 0) Then DirectCast(e.Item.FindControl("divRptRow"), HtmlGenericControl).Attributes.Add("class", "odd")
            itzultzaileWeb.Itzuli(DirectCast(e.Item.FindControl("labelFechaRpt"), Label))
            itzultzaileWeb.Itzuli(DirectCast(e.Item.FindControl("labelHojaRpt"), Label))
            itzultzaileWeb.Itzuli(DirectCast(e.Item.FindControl("labelSectorRpt"), Label))
            itzultzaileWeb.Itzuli(DirectCast(e.Item.FindControl("labelLocalidadRpt"), Label))
            itzultzaileWeb.Itzuli(DirectCast(e.Item.FindControl("labelEstablecimientoRpt"), Label))
            itzultzaileWeb.Itzuli(DirectCast(e.Item.FindControl("labelComentarioRpt"), Label))
            itzultzaileWeb.Itzuli(DirectCast(e.Item.FindControl("labelImporteRpt"), Label))
            itzultzaileWeb.Itzuli(DirectCast(e.Item.FindControl("labelEstadoRpt"), Label))
            itzultzaileWeb.Itzuli(DirectCast(e.Item.FindControl("labelImporteEurRpt"), Label))
        End If
    End Sub


#End Region

#Region "Validar"

    ''' <summary>
    ''' Marca como conforme los gastos seleccionados
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnConforme_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConforme.Click
        Try
            Confirmar(True)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Marca como no conforme los gastos seleccionados
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnNoConforme_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNoConforme.Click
        Try
            Confirmar(False)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Confirma o no los seleccionados
    ''' </summary>
    ''' <param name="bConforme">True si es conforme</param>    
    Private Sub Confirmar(ByVal bConforme As Boolean)
        Try
            Dim oMov As ELL.Visa.Movimiento
            Dim lMov As New List(Of ELL.Visa.Movimiento)
            Dim sValidacion As String = String.Empty
            For Each row As GridViewRow In gvVisa.Rows
                If (CType(row.Cells(0).Controls(0), CheckBox).Checked) Then
                    If (sValidacion <> String.Empty) Then sValidacion &= ","
                    oMov = New ELL.Visa.Movimiento With {
                        .Id = CInt(gvVisa.DataKeys(row.RowIndex).Values("Id")),
                        .Fecha = CDate(gvVisa.DataKeys(row.RowIndex).Values("Fecha")),
                        .IdUsuario = CInt(gvVisa.DataKeys(row.RowIndex).Values("IdUsuario")),
                        .IdViaje = CInt(gvVisa.DataKeys(row.RowIndex).Values("IdViaje")),
                        .Estado = If(bConforme, ELL.Visa.Movimiento.eEstado.Conforme, ELL.Visa.Movimiento.eEstado.No_Conforme)
                        }
                    lMov.Add(oMov)
                    sValidacion &= oMov.Id
                End If
            Next
            If (lMov.Count = 0) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Debe seleccionar algun gasto para validar")
            Else
                Dim visaBLL As New BLL.VisasBLL
                visaBLL.CambiarEstadoMovimientos(lMov)
                BuscarGastosVisa()
                Master.MensajeInfo = itzultzaileWeb.Itzuli("Validacion realizada con exito")
                Dim mensaVariable As String = If(bConforme, "ha confirmado", "no ha confirmado")
                log.Info("VALIDACION_VISA: El validador " & Master.Ticket.NombreCompleto & " (" & Master.Ticket.IdUser & ") " & mensaVariable & " " & lMov.Count & " gastos de visa al usuario " & lblPersona.Text & ".Validaciones (" & sValidacion & ")")
            End If
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("errGuardar", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Vuelve al listado de gastos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnVolver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnVolver.Click
        Dim url As String = "ValGastosVisa.aspx?mes=" & Mes & "&anio=" & Año
        url &= If(VerSinValidar, "&vsv=1", "&vsv=0")
        Response.Redirect(url, False)
    End Sub

#End Region

End Class