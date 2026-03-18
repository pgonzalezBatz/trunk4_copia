Public Class HojasGastosSinViaje
    Inherits PageBase

#Region "Page Load"

    ''' <summary>
    ''' Se carga las cabeceras de las hojas de gastos del año actual
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Hojas de gastos sin viajes asociados"
                inicializar()
                If (Request.QueryString("anno") IsNot Nothing) Then ddlAnno.SelectedValue = CInt(Request.QueryString("anno"))
                mostrarHojasGastos()
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Traducciones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Viajes_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(lnkVerViajes) : itzultzaileWeb.Itzuli(labelInfo1)
            itzultzaileWeb.Itzuli(labelInfo2) : itzultzaileWeb.Itzuli(labelInfo3) : itzultzaileWeb.Itzuli(labelAno)
        End If
    End Sub

    ''' <summary>
    ''' Inicializa la pagina
    ''' </summary>
    Private Sub inicializar()
        pnlInfoSinVisa.Visible = False : pnlInfoConVisa.Visible = False
        cargarAños()
    End Sub

    ''' <summary>
    ''' Carga los años
    ''' Iran desde el año actual hasta el 2011 que es la HG mas vieja
    ''' </summary>
    Private Sub cargarAños()
        If (ddlAnno.Items.Count = 0) Then
            For ano As Integer = Now.Year To 2011 Step -1
                ddlAnno.Items.Add(New ListItem(ano, ano))
            Next
            ddlAnno.SelectedValue = -1
        End If
    End Sub

#End Region

#Region "Mostrar Hojas de gastos"

    ''' <summary>
    ''' Muestra el listado con las hojas de gastos del año seleccionado
    ''' </summary>    
    Private Sub mostrarHojasGastos()
        Try
            Dim bTieneVisa As Boolean = (Session("ConVisa") IsNot Nothing AndAlso CType(Session("ConVisa"), Boolean) = True)
            Dim hojaBLL As New BLL.HojasGastosBLL
            Dim fInicio, fFin As Date
            Dim anno As Integer = CInt(ddlAnno.SelectedValue)
            fInicio = New Date(anno, 1, 1) : fFin = New Date(anno, 12, 31)
            Dim lHojas As List(Of ELL.HojaGastos) = hojaBLL.loadHojasPorMesSinViaje(Master.Ticket.IdUser, Master.IdPlantaGestion, fInicio, fFin)
            inicializar()
            pnlInfoConVisa.Visible = bTieneVisa : pnlInfoSinVisa.Visible = Not bTieneVisa
            gvHG.DataSource = lHojas : gvHG.DataBind()
            gvHG.Columns(4).Visible = bTieneVisa
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al mostrar el detalle", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se muestran el listado de hojas del año correspondiente
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub ddlAnno_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlAnno.SelectedIndexChanged
        Try
            mostrarHojasGastos()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Vuelve al listado de los viajes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub lnkVerViajes_Click(sender As Object, e As EventArgs) Handles lnkVerViajes.Click
        Response.Redirect("Viajes.aspx", False)
    End Sub

#Region "Gridview"

    ''' <summary>
    ''' Se enlazan los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvHG_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvHG.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header OrElse e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oHoja As ELL.HojaGastos = e.Row.DataItem
            Dim lblEstado As Label = CType(e.Row.FindControl("lblEstado"), Label)
            Dim imgHG As ImageButton = CType(e.Row.FindControl("imgHG"), ImageButton)
            Dim imgGastosVisa As ImageButton = CType(e.Row.FindControl("imgGastosVisa"), ImageButton)
            CType(e.Row.FindControl("lblMes"), Label).Text = StrConv(MonthName(oHoja.FechaDesde.Month), VbStrConv.ProperCase)
            CType(e.Row.FindControl("lblMesAnno"), Label).Text = oHoja.FechaDesde.Month & "|" & oHoja.FechaDesde.Year
            'Se comprueba el estado de la hoja
            imgHG.CommandArgument = e.Row.RowIndex
            imgGastosVisa.CommandArgument = e.Row.RowIndex
            If (oHoja.Id > 0) Then
                CType(e.Row.FindControl("lblId"), Label).Text = oHoja.Id
                CType(e.Row.FindControl("lblSinIdViaje"), Label).Text = oHoja.IdSinViaje
                Select Case oHoja.Estado
                    Case ELL.HojaGastos.eEstado.Enviada
                        imgHG.ImageUrl = "../App_Themes/Tema1/Images/EstadosHojas/Enviada.png"
                        imgHG.ToolTip = itzultzaileWeb.Itzuli("Hoja de gastos enviada")
                        lblEstado.Text = itzultzaileWeb.Itzuli("Enviada al validador")
                        lblEstado.CssClass = "label label-primary"
                    Case ELL.HojaGastos.eEstado.Validada
                        imgHG.ImageUrl = "../App_Themes/Tema1/Images/EstadosHojas/Aceptada.png"
                        imgHG.ToolTip = itzultzaileWeb.Itzuli("Hoja de gastos validada")
                        lblEstado.Text = itzultzaileWeb.Itzuli("Validada")
                        lblEstado.CssClass = "label label-success"
                    Case ELL.HojaGastos.eEstado.Liquidada
                        imgHG.ImageUrl = "../App_Themes/Tema1/Images/EstadosHojas/Liquidada.png"
                        imgHG.ToolTip = itzultzaileWeb.Itzuli("Hoja de gastos liquidada")
                        lblEstado.Text = itzultzaileWeb.Itzuli("Integrada")
                        lblEstado.CssClass = "label label-info"
                    Case ELL.HojaGastos.eEstado.NoValidada
                        imgHG.ImageUrl = "../App_Themes/Tema1/Images/EstadosHojas/Rechazada.png"
                        imgHG.ToolTip = itzultzaileWeb.Itzuli("Hoja de gastos rechazada")
                        lblEstado.Text = itzultzaileWeb.Itzuli("Rechazada")
                        lblEstado.CssClass = "label label-danger"
                    Case ELL.HojaGastos.eEstado.Rellenada
                        imgHG.ImageUrl = "../App_Themes/Tema1/Images/EstadosHojas/SinEnviar.png"
                        imgHG.ToolTip = itzultzaileWeb.Itzuli("Continuar rellenando hoja de gastos")
                        lblEstado.Text = itzultzaileWeb.Itzuli("Sin enviar")
                        lblEstado.CssClass = "label label-default"
                    Case ELL.HojaGastos.eEstado.Transferida
                        imgHG.ImageUrl = "../App_Themes/Tema1/Images/EstadosHojas/Transferida.png"
                        imgHG.ToolTip = itzultzaileWeb.Itzuli("Hoja de gastos transferida a su empresa para que le realice el pago")
                        lblEstado.Text = itzultzaileWeb.Itzuli("Transferida")
                        lblEstado.CssClass = "label label-info"
                End Select
            Else  'No existe
                imgHG.ImageUrl = "../App_Themes/Tema1/Images/EstadosHojas/Crear.png"
                imgHG.ToolTip = itzultzaileWeb.Itzuli("Crear hoja de gastos")
            End If
            If (oHoja.FechaDesde.Year = Now.Year AndAlso oHoja.FechaDesde.Month > Now.Month) Then
                imgHG.Visible = False : imgGastosVisa.Visible = False
            Else
                imgHG.Visible = True : imgGastosVisa.Visible = True
            End If
            'Ver si tiene la persona gastos de visa en las fechas
            If (Session("ConVisa") IsNot Nothing AndAlso CType(Session("ConVisa"), Boolean) = True) Then
                Dim visasBLL As New BLL.VisasBLL
                Dim bFicheroVisasCargado As Boolean = visasBLL.FicheroVisasCargado(oHoja.FechaHasta.Month, oHoja.FechaHasta.Year, Master.IdPlantaGestion)
                If (bFicheroVisasCargado) Then 'Si se ha cargado el fichero de visas, se mira si tiene algun movimiento
                    Dim lMov As List(Of ELL.Visa.Movimiento) = visasBLL.loadMovimientos(Master.Ticket.IdUser, Nothing, Master.IdPlantaGestion, oHoja.FechaDesde, oHoja.FechaHasta, False)
                    If (lMov IsNot Nothing AndAlso lMov.Count > 0) Then  'Tiene movimientos
                        imgGastosVisa.ImageUrl = "~/App_Themes/Tema1/Images/EstadosHojas/ConVisa.png"
                        imgGastosVisa.ToolTip = itzultzaileWeb.Itzuli("Se han cargado automaticamente sus gastos de visa")
                    Else
                        imgGastosVisa.OnClientClick = "return false;"
                        imgGastosVisa.Style.Add("cursor", "default")
                        imgGastosVisa.ImageUrl = "~/App_Themes/Tema1/Images/EstadosHojas/SinVisa.png"
                        imgGastosVisa.ToolTip = itzultzaileWeb.Itzuli("No se han realizado pagos con visa")
                    End If
                Else 'Todavia no se ha cargado el fichero de visas
                    imgGastosVisa.OnClientClick = "return false;"
                    imgGastosVisa.Style.Add("cursor", "default")
                    imgGastosVisa.ImageUrl = "~/App_Themes/Tema1/Images/EstadosHojas/WaitingVisa.png"
                    imgGastosVisa.ToolTip = itzultzaileWeb.Itzuli("Esperando a la carga del extracto de visas")
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Eventos al hacer click en las imagenes del listado nuevo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvHG_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvHG.RowCommand
        If (e.CommandName = "ver") Then
            Dim row As GridViewRow = gvHG.Rows(CInt(e.CommandArgument))
            Dim idHoja As Integer = Integer.MinValue
            If (CType(row.Cells(0).Controls(0), Label).Text <> String.Empty) Then idHoja = CInt(CType(row.Cells(0).Controls(0), Label).Text)
            If (idHoja = Integer.MinValue) Then
                Dim datos As String() = CType(row.Cells(1).Controls(0), Label).Text.Split("|")
                Dim fIni, fFin As Date
                fIni = New Date(CInt(datos(1)), CInt(datos(0)), 1) : fFin = New Date(CInt(datos(1)), CInt(datos(0)), Date.DaysInMonth(CInt(datos(1)), CInt(datos(0))))
                Response.Redirect("HojaGastos.aspx?id=" & Integer.MinValue & "&fIni=" & fIni.Ticks & "&fFin=" & fFin.Ticks, False)
            Else
                Dim hojasBLL As New BLL.HojasGastosBLL
                Dim oHoja As ELL.HojaGastos = hojasBLL.loadHoja(idHoja, False)
                Response.Redirect("HojaGastos.aspx?id=" & idHoja & "&fIni=" & oHoja.FechaDesde.Ticks & "&fFin=" & oHoja.FechaHasta.Ticks, False)
            End If
        End If
    End Sub

#End Region

#End Region

End Class