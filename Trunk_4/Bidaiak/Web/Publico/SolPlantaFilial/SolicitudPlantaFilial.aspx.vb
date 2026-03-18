Public Class SolicitudPlantaFilial
    Inherits PageBase

#Region "Property"

    ''' <summary>
    ''' Propiedad para saber que solicitudes se tienen que visualizar
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
    ''' Carga la pagina con las solicitudes de plantas filiales de los viajes entre las fechas del mes actual
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Solicitudes de plantas filiales"
                If (Session("Gerente") IsNot Nothing) Then
                    Dim mes, año As Integer
                    inicializarControles()
                    VerSinAprobar = If(Request.QueryString("vsa") IsNot Nothing, Request.QueryString("vsa"), 1)
                    mes = If(Request.QueryString("Mes") IsNot Nothing, CInt(Request.QueryString("Mes")), Now.Month)
                    año = If(Request.QueryString("Anio") IsNot Nothing, CInt(Request.QueryString("Anio")), Now.Year)
                    CargarMesesYAños(mes, año)
                    BuscarSolicitudesPlantas()
                Else
                    log.Warn("Se ha denegado el acceso a la pagina de Solicitudes de plantas filiales por no ser " & Master.Ticket.NombreCompleto & " gerente de ninguna planta")
                    Response.Redirect("~/PermisoDenega.aspx?mensa=3", False)
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
            itzultzaileWeb.Itzuli(labelTitle) : itzultzaileWeb.Itzuli(rbtPorFechas)
            itzultzaileWeb.Itzuli(rbtSinAprobar) : itzultzaileWeb.Itzuli(btnSearchF)
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
            ddlMes.Items.Clear() : ddlAño.Items.Clear()
            mesesPintar = 12
            'Meses
            For i = 1 To mesesPintar
                ddlMes.Items.Add(New ListItem(MonthName(i).ToUpper, i))
            Next i
            ddlMes.SelectedIndex = month - 1
            'Años
            ddlAño.Items.Add(New ListItem(año - 1, año - 1))
            ddlAño.Items.Add(New ListItem(año, año))
            ddlAño.SelectedIndex = ddlAño.Items.IndexOf(ddlAño.Items.FindByValue(year))
        Catch ex As Exception
            Throw New BatzException("errIKScargandoFechas", ex)
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
            BuscarSolicitudesPlantas()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Busca las solicitudes de plantas filiales en la fecha seleccionada
    ''' </summary>    
    Private Sub BuscarSolicitudesPlantas()
        Dim solicitBLL As New BLL.ViajesBLL
        Dim fechaInicio, fechaFin As Date
        Dim estado As Integer = Integer.MinValue
        fechaInicio = Date.MinValue : fechaFin = Date.MinValue
        If (VerSinAprobar = 0) Then
            fechaInicio = New Date(ddlAño.SelectedValue, ddlMes.SelectedValue, 1)
            fechaFin = New Date(ddlAño.SelectedValue, ddlMes.SelectedValue, Date.DaysInMonth(ddlAño.SelectedValue, ddlMes.SelectedValue))
        Else
            estado = ELL.Viaje.SolicitudPlantaFilial.EstadoSolFilial.Solicitado
        End If
        Dim lSolicitudes As List(Of String()) = solicitBLL.loadSolPlantasFiliales(Master.Ticket.IdUser, fechaInicio, fechaFin, estado)
        If (lSolicitudes IsNot Nothing AndAlso lSolicitudes.Count > 0) Then lSolicitudes = lSolicitudes.OrderBy(Of Date)(Function(o) CDate(o(1))).ToList
        gvSolicitudes.DataSource = lSolicitudes
        gvSolicitudes.DataBind()
    End Sub

#End Region

#Region "Gridview"

    ''' <summary>
    ''' Evento que surge al pulsar en el detalle
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvSolicitudes_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvSolicitudes.RowCommand
        Dim params As String() = e.CommandArgument.ToString.Split("|")
        Dim url As String = "DetSolPlanta.aspx?IdViaje=" & params(0) & "&IdPlanta=" & params(1) & "&vsa=" & VerSinAprobar
        If (rbtPorFechas.Checked) Then url &= "&Mes=" & ddlMes.SelectedValue & "&Anio=" & ddlAño.SelectedValue
        Response.Redirect(url)
    End Sub

    ''' <summary>
    ''' Enlaza los controles
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvSolicitudes_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvSolicitudes.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim sol = e.Row.DataItem
            Dim imgEstado As Image = CType(e.Row.FindControl("imgEstado"), Image)
            CType(e.Row.FindControl("lblIdViaje"), Label).Text = sol(0)
            CType(e.Row.FindControl("lblFechaInicio"), Label).Text = CType(sol(1), Date).ToShortDateString
            CType(e.Row.FindControl("lblFechaFin"), Label).Text = CType(sol(2), Date).ToShortDateString
            CType(e.Row.FindControl("lblPlanta"), Label).Text = sol(4)
            Select Case CInt(sol(5))
                Case ELL.Viaje.SolicitudPlantaFilial.EstadoSolFilial.Solicitado
                    imgEstado.ImageUrl = "~/App_Themes/Tema1/Images/Espera.png"
                    imgEstado.ToolTip = itzultzaileWeb.Itzuli("Solicitada")
                Case ELL.Viaje.SolicitudPlantaFilial.EstadoSolFilial.Aprobada
                    imgEstado.ImageUrl = "~/App_Themes/Tema1/Images/Aceptada.png"
                    imgEstado.ToolTip = itzultzaileWeb.Itzuli("Aprobada")
                Case ELL.Viaje.SolicitudPlantaFilial.EstadoSolFilial.Rechazada
                    imgEstado.ImageUrl = "~/App_Themes/Tema1/Images/Denegada.png"
                    imgEstado.ToolTip = itzultzaileWeb.Itzuli("Rechazada")
            End Select
            Dim params As String = sol(0) & "|" & sol(3)  'IdViaje|IdPlanta
            e.Row.ToolTip = itzultzaileWeb.Itzuli("Hacer click para ir al detalle")
            e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gvSolicitudes, "Select$" + CStr(params))
        End If
    End Sub

    ''' <summary>
    ''' Paginacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvSolicitudes_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvSolicitudes.PageIndexChanging
        Try
            gvSolicitudes.PageIndex = e.NewPageIndex
            BuscarSolicitudesPlantas()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

End Class