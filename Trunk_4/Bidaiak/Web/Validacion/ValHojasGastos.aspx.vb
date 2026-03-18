Public Class ValHojasGastos
    Inherits PageBase

#Region "Property"

    ''' <summary>
    ''' Propiedad para saber que movimientos se tienen que visualizar
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public Property VerSinValidar As Integer
        Get
            If (hfOpcionSel.Value = String.Empty) Then
                Return 0
            Else
                Return CInt(hfOpcionSel.Value)
            End If
        End Get
        Set(ByVal value As Integer)
            hfOpcionSel.Value = value
        End Set
    End Property

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
                Master.SetTitle = "Validacion de hojas de gastos"
                Dim mes, año As Integer
                inicializarControles()
                VerSinValidar = If(Request.QueryString("vsv") IsNot Nothing, Request.QueryString("vsv"), 1)
                mes = If(Request.QueryString("Mes") IsNot Nothing, CInt(Request.QueryString("Mes")), Now.Month)
                año = If(Request.QueryString("Anio") IsNot Nothing, CInt(Request.QueryString("Anio")), Now.Year)
                CargarMesesYAños(mes, año)
                BuscarHojasGastos()
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
    Private Sub ValHojasGastos_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelTitle) : itzultzaileWeb.Itzuli(rbtPorFechas) : itzultzaileWeb.Itzuli(rbtSinValidar)
            itzultzaileWeb.Itzuli(btnSearchF)
        End If
    End Sub

    ''' <summary>
    ''' Se inicializan los controles
    ''' </summary>    
    Private Sub inicializarControles()
        rbtPorFechas.Checked = True
        VerSinValidar = 0  'Se inicializa con la opcion de seleccionar mes y año
        rbtPorFechas.Attributes.Add("onclick", "ChangeRadio('0');")
        rbtSinValidar.Attributes.Add("onclick", "ChangeRadio('1');")
    End Sub

    ''' <summary>
    ''' <para>Dado un mes y un año, carga los desplegable solo con los valores que podria elegir</para>
    ''' <para>Para el año actual, solo se podra elegir hasta el mes actual</para>
    ''' </summary>
    ''' <param name="month">Mes</param>
    ''' <param name="year">Año</param>
    Private Sub CargarMesesYAños(ByVal month As Integer, ByVal year As Integer)
        Try
            Dim i As Integer
            Dim año As Integer = CInt(Now.Date.Year)
            Dim mesesPintar As Integer = month
            ddlMes.Items.Clear()
            ddlAño.Items.Clear()
            'Si no es el año actual, pintara todos los meses del año
            mesesPintar = If(Now.Date.Year <> year, 12, Now.Date.Month)
            'Meses
            For i = 1 To mesesPintar
                ddlMes.Items.Add(New ListItem(MonthName(i).ToUpper, i))
            Next i
            'Cuando se cambio del año anterior al año actual, puede que el mes marcado no se tenga que visualizar en el año actual, asi que se mostrar el maximo que se puede mostrar
            ddlMes.SelectedIndex = If(month > ddlMes.Items.Count, ddlMes.Items.Count - 1, month - 1)
            'Años
            ddlAño.Items.Add(New ListItem(año - 1, año - 1))
            ddlAño.Items.Add(New ListItem(año, año))
            ddlAño.SelectedIndex = ddlAño.Items.IndexOf(ddlAño.Items.FindByValue(year))
        Catch ex As Exception
            Throw New BatzException("errIKScargandoFechas", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se cargan los meses que le correspondan al año
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub ddlAño_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlAño.SelectedIndexChanged
        Try
            CargarMesesYAños(1, ddlAño.SelectedValue)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Buscar hojas"

    ''' <summary>
    ''' Evento del boton buscar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnSearchF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearchF.Click
        Try
            BuscarHojasGastos()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Busca las hojas de gastos del validador entre las fechas seleccionadas
    ''' </summary>    
    Private Sub BuscarHojasGastos()
        Try
            Dim hojaBLL As New BLL.HojasGastosBLL
            Dim fechaInicio, fechaFin As Date
            If (VerSinValidar) Then
                fechaInicio = DateTime.MinValue : fechaFin = DateTime.MinValue
            Else
                fechaInicio = New Date(ddlAño.SelectedValue, ddlMes.SelectedValue, 1)
                fechaFin = New Date(ddlAño.SelectedValue, ddlMes.SelectedValue, Date.DaysInMonth(ddlAño.SelectedValue, ddlMes.SelectedValue))
            End If
            Dim lHojas As List(Of ELL.HojaGastos) = hojaBLL.loadHojas(Master.IdPlantaGestion, Master.Ticket.IdUser, Integer.MinValue, fechaInicio, fechaFin, False, False)  'Pasar fechas
            Select Case VerSinValidar
                Case 0 'Todos
                    lHojas = lHojas.FindAll(Function(o As ELL.HojaGastos) o.Validador.Id = Master.Ticket.IdUser And o.Estado <> ELL.HojaGastos.eEstado.Rellenada)
                    'Case 1 'Validados
                    '    lHojas = lHojas.FindAll(Function(o As ELL.HojaGastos) o.Validador.Id = Master.Ticket.IdUser And (o.Estado = ELL.HojaGastos.eEstado.Validada Or o.Estado = ELL.HojaGastos.eEstado.Liquidada))
                Case 1 'Sin Validar/Enviada
                    lHojas = lHojas.FindAll(Function(o As ELL.HojaGastos) o.Validador.Id = Master.Ticket.IdUser And o.Estado = ELL.HojaGastos.eEstado.Enviada)
            End Select
            lHojas = lHojas.OrderBy(Of String)(Function(o) o.Usuario.NombreCompleto).ToList
            gvHojas.DataSource = lHojas
            gvHojas.DataBind()
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al buscar las hojas de gastos", ex)
        End Try
    End Sub

#End Region

#Region "Gridview"

    ''' <summary>
    ''' Enlaza los controles
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvHojas_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvHojas.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oHoja As ELL.HojaGastos = e.Row.DataItem
            Dim visasBLL As New BLL.VisasBLL
            Dim lblCodigo As Label = CType(e.Row.FindControl("lblCodigo"), Label)
            Dim imgEstado As Image = CType(e.Row.FindControl("imgEstado"), Image)
            Dim lVisas As List(Of ELL.Visa) = visasBLL.loadList(New ELL.Visa With {.Propietario = New SabLib.ELL.Usuario With {.Id = oHoja.Usuario.Id}}, Master.IdPlantaGestion, False)
            CType(e.Row.FindControl("lblPersona"), Label).Text = oHoja.Usuario.NombreCompleto
            CType(e.Row.FindControl("lblFechaEnvio"), Label).Text = oHoja.GetFechaEstado(ELL.HojaGastos.eEstado.Enviada).ToShortDateString
            If (oHoja.IdViaje = Integer.MinValue) Then
                lblCodigo.Text = "H" & oHoja.IdSinViaje
            Else
                Dim viajeBLL As BLL.ViajesBLL = New BLL.ViajesBLL
                Dim oViaje As ELL.Viaje = viajeBLL.loadInfo(oHoja.IdViaje, False, False, True)
                lblCodigo.Text = "V" & oHoja.IdViaje & "(" & oViaje.Destino & ")"
            End If
            Select Case oHoja.Estado
                Case ELL.HojaGastos.eEstado.Enviada
                    imgEstado.ImageUrl = "~/App_Themes/Tema1/Images/EstadosValidacion/Sin_Validar.png"
                    imgEstado.ToolTip = itzultzaileWeb.Itzuli("Sin validar")
                Case ELL.HojaGastos.eEstado.Validada
                    imgEstado.ImageUrl = "~/App_Themes/Tema1/Images/EstadosValidacion/Validado.png"
                    imgEstado.ToolTip = itzultzaileWeb.Itzuli("Validada")
                Case ELL.HojaGastos.eEstado.NoValidada
                    imgEstado.ImageUrl = "~/App_Themes/Tema1/Images/EstadosValidacion/Rechazada.png"
                    imgEstado.ToolTip = itzultzaileWeb.Itzuli("Rechazada")
                Case ELL.HojaGastos.eEstado.Liquidada
                    imgEstado.ImageUrl = "~/App_Themes/Tema1/Images/EstadosValidacion/Integrado.png"
                    imgEstado.ToolTip = itzultzaileWeb.Itzuli("Integrada")
                Case ELL.HojaGastos.eEstado.Transferida
                    imgEstado.ImageUrl = "~/App_Themes/Tema1/Images/EstadosValidacion/Integrado.png"
                    imgEstado.ToolTip = itzultzaileWeb.Itzuli("Transferida")
            End Select
            e.Row.ToolTip = itzultzaileWeb.Itzuli("Hacer click para ir al detalle")
            Dim param As String = oHoja.Id & "_" & If(oHoja.IdViaje <> Integer.MinValue, oHoja.Usuario.Id, "")  'Para las hojas de viaje, como puede haber mas de un usuario, se le pasara el Id del usuario
            e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gvHojas, "Select$" + param)
        End If
    End Sub

    ''' <summary>
    ''' Evento de una fila del gridview.
    ''' Redirige a la hoja de gastos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvHojas_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvHojas.RowCommand
        If (e.CommandName = "Select") Then
            Dim param As String() = e.CommandArgument.ToString.Split(New Char() {"_"}, StringSplitOptions.RemoveEmptyEntries)
            Dim url As String = "~/Viaje/HojaGastos.aspx?id=" & param(0) & "&orig=ValHG"
            If (param.Length = 2) Then url &= "&iduser=" & param(1)
            Response.Redirect(url)
        End If
    End Sub

    ''' <summary>
    ''' Paginacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvHojas_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvHojas.PageIndexChanging
        Try
            gvHojas.PageIndex = e.NewPageIndex
            BuscarHojasGastos()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

End Class