Public Class ValGastosVisa
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
                Master.SetTitle = "Validacion de gastos de VISA"
                Dim mes, año As Integer
                inicializarControles()
                VerSinValidar = If(Request.QueryString("vsv") IsNot Nothing, Request.QueryString("vsv"), 1)
                mes = If(Request.QueryString("Mes") IsNot Nothing, CInt(Request.QueryString("Mes")), Now.Month)
                año = If(Request.QueryString("Anio") IsNot Nothing, CInt(Request.QueryString("Anio")), Now.Year)
                CargarMesesYAños(mes, año)
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
    Private Sub ValGastosVisa_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
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
            VerSinValidar = If(rbtSinValidar.Checked, 1, 0)
            BuscarGastosVisa()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Busca los gastos de visa de la gente al cargo del validador en la fecha seleccionada
    ''' </summary>    
    Private Sub BuscarGastosVisa()
        Dim visaBLL As New BLL.VisasBLL
        Dim fechaInicio, fechaFin As Date
        If (VerSinValidar = 0) Then
            fechaInicio = New Date(ddlAño.SelectedValue, ddlMes.SelectedValue, 1)
            fechaFin = New Date(ddlAño.SelectedValue, ddlMes.SelectedValue, Date.DaysInMonth(ddlAño.SelectedValue, ddlMes.SelectedValue))
        End If
        Dim lGastosVisa As List(Of ELL.Visa.Movimiento) = visaBLL.loadMovimientos(Master.Ticket.IdUser, Integer.MinValue, Master.IdPlantaGestion, fechaInicio, fechaFin)  'Pasar fechas
        If (VerSinValidar = 1) Then
            lGastosVisa = lGastosVisa.FindAll(Function(o As ELL.Visa.Movimiento) o.IdResponsable = Master.Ticket.IdUser AndAlso o.Estado = ELL.Visa.Movimiento.eEstado.Justificado)  'Se omiten los cargados para que si hay un mes que todos son cargados, ni siquiera lo vea
        Else
            lGastosVisa = lGastosVisa.FindAll(Function(o As ELL.Visa.Movimiento) o.IdResponsable = Master.Ticket.IdUser)  'Obtiene todos
        End If
        Dim lVisas As New List(Of ELL.Visa.Movimiento)
        Dim idUserActual, mesActual, añoActual As Integer
        idUserActual = 0 : mesActual = 0 : añoActual = 0
        'En esta linea, esta agrupando por NombreUsuario,Fecha y Año y luego ordenando por 3 campos
        Dim lAux = lGastosVisa.GroupBy(Function(x) New With {Key x.IdUsuario, Key x.NombreUsuario, Key x.Fecha.Month, Key x.Fecha.Year}).Select(Function(group) New With {Key .Visa = group.Key}).OrderBy(Function(z) z.Visa.NombreUsuario).ThenBy(Function(t) t.Visa.Month).ThenBy(Function(y) y.Visa.Year)
        gvVisa.DataSource = lAux
        gvVisa.DataBind()
    End Sub

#End Region

#Region "Gridview"

    ''' <summary>
    ''' Evento que surge al pulsar en el detalle
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvVisa_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvVisa.RowCommand
        Dim params As String() = e.CommandArgument.ToString.Split("|")
        Response.Redirect("DetValVisa.aspx?IdUser=" & params(0) & "&Mes=" & params(1) & "&Anio=" & params(2) & "&vsv=" & VerSinValidar)
    End Sub

    ''' <summary>
    ''' Enlaza los controles
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvVisa_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvVisa.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim v = e.Row.DataItem
            CType(e.Row.FindControl("lblPersona"), Label).Text = v.Visa.NombreUsuario
            CType(e.Row.FindControl("lblMes"), Label).Text = MonthName(v.Visa.Month).ToUpper
            CType(e.Row.FindControl("lblAño"), Label).Text = v.Visa.Year
            Dim params As String = v.Visa.IdUsuario & "|" & v.Visa.Month & "|" & v.Visa.Year
            e.Row.ToolTip = itzultzaileWeb.Itzuli("Hacer click para ir al detalle")
            e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gvVisa, "Select$" + CStr(params))
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

End Class