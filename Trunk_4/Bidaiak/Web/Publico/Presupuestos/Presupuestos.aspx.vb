Public Class Presupuestos
    Inherits PageBase

#Region "Property"

    ''' <summary>
    ''' Propiedad para saber que presupuestos se tienen que visualizar
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public Property VerSinAprobar As Integer
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
    ''' Carga la pagina con los presupuestos de los viajes entre las fechas del mes actual
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Presupuestos de viajes"
                If (Session("ValPresup") IsNot Nothing) Then
                    Dim mes, año As Integer
                    inicializarControles()
                    VerSinAprobar = If(Request.QueryString("vsa") IsNot Nothing, Request.QueryString("vsa"), 1)
                    mes = If(Request.QueryString("Mes") IsNot Nothing, CInt(Request.QueryString("Mes")), Now.Month)
                    año = If(Request.QueryString("Anio") IsNot Nothing, CInt(Request.QueryString("Anio")), Now.Year)
                    CargarMesesYAños(mes, año)
                    BuscarPresupuestos()
                Else
                    log.Warn("Se ha denegado el acceso a la pagina de presupuestos de viajes por no ser " & Master.Ticket.NombreCompleto & " responsable de realizar esta accion")
                    Response.Redirect("~PermisoDenega.aspx?mensa=3", False)
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
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelTitle) : itzultzaileWeb.Itzuli(rbtPorFechas) : itzultzaileWeb.Itzuli(rbtSinAprobar)
            itzultzaileWeb.Itzuli(btnSearchF)
        End If
    End Sub

    ''' <summary>
    ''' Se inicializan los controles
    ''' </summary>    
    Private Sub inicializarControles()
        rbtPorFechas.Checked = True
        VerSinAprobar = 0  'Se inicializa con la opcion de seleccionar mes y año
        rbtPorFechas.Attributes.Add("onclick", "ChangeRadio('0');")
        rbtSinAprobar.Attributes.Add("onclick", "ChangeRadio('1');")
    End Sub

    ''' <summary>
    ''' <para>Dado un mes y un año, carga los desplegable solo con los valores que podria elegir</para>
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
            'Se pintan todos para que pueda ver presupuestos de viajes futuros
            mesesPintar = 12 'If(Now.Date.Year <> year, 12, Now.Date.Month)
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

#Region "Buscar solicitudes"

    ''' <summary>
    ''' Evento del boton buscar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnSearchF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearchF.Click
        Try
            VerSinAprobar = If(rbtSinAprobar.Checked, 1, 0)
            BuscarPresupuestos()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Busca los presupuestos en la fecha seleccionada
    ''' </summary>    
    Private Sub BuscarPresupuestos()
        Dim presupBLL As New BLL.PresupuestosBLL
        Dim fechaInicio, fechaFin As Date
        Dim estado As Integer = Integer.MinValue
        fechaInicio = Date.MinValue : fechaFin = Date.MinValue
        If (VerSinAprobar = 0) Then
            fechaInicio = New Date(ddlAño.SelectedValue, ddlMes.SelectedValue, 1)
            fechaFin = New Date(ddlAño.SelectedValue, ddlMes.SelectedValue, Date.DaysInMonth(ddlAño.SelectedValue, ddlMes.SelectedValue))
        Else
            estado = ELL.Presupuesto.EstadoPresup.Enviado
        End If
        Dim lPresupuestos As List(Of String()) = presupBLL.loadPresupuestos(Master.Ticket.IdUser, fechaInicio, fechaFin, estado)
        If (lPresupuestos IsNot Nothing AndAlso lPresupuestos.Count > 0) Then lPresupuestos = lPresupuestos.OrderBy(Of Date)(Function(o) CDate(o(1))).ToList
        gvPresupuestos.DataSource = lPresupuestos
        gvPresupuestos.DataBind()
    End Sub

#End Region

#Region "Gridview"

    ''' <summary>
    ''' Evento que surge al pulsar en el detalle
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvPresupuestos_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvPresupuestos.RowCommand
        Dim param As String() = e.CommandArgument.ToString.Split("_")
        Dim url As String = "DetPresupuesto"
        If (param(1) = "1") Then url &= "New"
        url &= ".aspx?idPresup=" & param(0) & "&vsa=" & VerSinAprobar
        If (rbtPorFechas.Checked) Then url &= "&Mes=" & ddlMes.SelectedValue & "&Anio=" & ddlAño.SelectedValue
        Response.Redirect(url)
    End Sub

    ''' <summary>
    ''' Enlaza los controles
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvPresupuestos_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvPresupuestos.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim presup = e.Row.DataItem
            Dim imgEstado As Image = CType(e.Row.FindControl("imgEstado"), Image)
            Dim idEstado As Integer = CInt(presup(3))
            CType(e.Row.FindControl("lblIdViaje"), Label).Text = presup(0)
            CType(e.Row.FindControl("lblFechaInicio"), Label).Text = CType(presup(1), Date).ToShortDateString
            CType(e.Row.FindControl("lblFechaFin"), Label).Text = CType(presup(2), Date).ToShortDateString
            imgEstado.ToolTip = itzultzaileWeb.Itzuli([Enum].GetName(GetType(ELL.Presupuesto.EstadoPresup), idEstado))
            Select Case idEstado
                Case ELL.Presupuesto.EstadoPresup.Enviado
                    imgEstado.ImageUrl = "~/App_Themes/Tema1/Images/Espera.png"
                Case ELL.Presupuesto.EstadoPresup.Validado
                    imgEstado.ImageUrl = "~/App_Themes/Tema1/Images/Aceptada.png"
                Case ELL.Presupuesto.EstadoPresup.Rechazado
                    imgEstado.ImageUrl = "~/App_Themes/Tema1/Images/Denegada.png"
            End Select
            e.Row.ToolTip = itzultzaileWeb.Itzuli("Hacer click para ir al detalle")
            Dim param As String = presup(0) & "_" & presup(6) 'IdPresup_Nuevo
            e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gvPresupuestos, "Select$" + param)
        End If
    End Sub

    ''' <summary>
    ''' Paginacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvPresupuestos_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvPresupuestos.PageIndexChanging
        Try
            gvPresupuestos.PageIndex = e.NewPageIndex
            BuscarPresupuestos()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

End Class