Public Class ValViajes
    Inherits PageBase

#Region "Property"

    ''' <summary>
    ''' Propiedad para saber que viajes se tienen que visualizar
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
    ''' Carga los viajes entre las fechas del mes actual
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Validacion de viajes"
                Dim mes, año As Integer
                inicializarControles()
                VerSinValidar = If(Request.QueryString("vsv") IsNot Nothing, Request.QueryString("vsv"), 1)
                mes = If(Request.QueryString("Mes") IsNot Nothing, CInt(Request.QueryString("Mes")), Now.Month)
                año = If(Request.QueryString("Anio") IsNot Nothing, CInt(Request.QueryString("Anio")), Now.Year)
                CargarMesesYAños(mes, año)
                BuscarViajes()
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
    Private Sub ValViajes_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
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
            BuscarViajes()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Busca los viajes entre las fechas seleccionadas
    ''' </summary>    
    Private Sub BuscarViajes()
        Try
            Dim viaBLL As New BLL.ViajesBLL
            Dim fechaInicio, fechaFin As Date
            Dim estado As Integer = -1
            If (VerSinValidar = 1) Then
                fechaInicio = DateTime.MinValue : fechaFin = DateTime.MinValue
                estado = ELL.Viaje.eEstadoViaje.Pendiente_validacion
            Else
                fechaInicio = New Date(ddlAño.SelectedValue, ddlMes.SelectedValue, 1)
                fechaFin = New Date(ddlAño.SelectedValue, ddlMes.SelectedValue, Date.DaysInMonth(ddlAño.SelectedValue, ddlMes.SelectedValue))
            End If
            Dim lViajes As List(Of ELL.Viaje) = viaBLL.loadListViajes(New ELL.Viaje With {.FechaIda = fechaInicio, .FechaVuelta = fechaFin, .IdPlanta = Master.IdPlantaGestion, .Estado = estado}, False, Master.IdPlantaGestion)  'Pasar fechas
            If (VerSinValidar = 0) Then 'Todos
                lViajes = lViajes.FindAll(Function(o As ELL.Viaje) o.Estado <> ELL.Viaje.eEstadoViaje.Cancelado)
            End If
            lViajes = lViajes.OrderBy(Of String)(Function(o) o.IdViaje).ToList
            gvViajes.DataSource = lViajes
            gvViajes.DataBind()
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al buscar los viajes", ex)
        End Try
    End Sub

#End Region

#Region "Gridview"

    ''' <summary>
    ''' Enlaza los controles
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvViajes_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvViajes.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oViaje As ELL.Viaje = e.Row.DataItem
            Dim imgEstado As Image = CType(e.Row.FindControl("imgEstado"), Image)
            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Dim user As New SabLib.ELL.Usuario With {.Id = oViaje.IdUserSolicitador}
            user = userBLL.GetUsuario(user, True)
            CType(e.Row.FindControl("lblIdViaje"), Label).Text = oViaje.IdViaje
            CType(e.Row.FindControl("lblPersona"), Label).Text = user.NombreCompleto
            CType(e.Row.FindControl("lblFechaCreacion"), Label).Text = oViaje.FechaSolicitud.ToShortDateString
            Select Case oViaje.Estado
                Case ELL.Viaje.eEstadoViaje.Pendiente_validacion
                    imgEstado.ImageUrl = "~/App_Themes/Tema1/Images/EstadosValidacion/Sin_Validar.png"
                    imgEstado.ToolTip = itzultzaileWeb.Itzuli("Sin validar")
                Case ELL.Viaje.eEstadoViaje.Validado
                    imgEstado.ImageUrl = "~/App_Themes/Tema1/Images/EstadosValidacion/Validado.png"
                    imgEstado.ToolTip = itzultzaileWeb.Itzuli("Validada")
                Case ELL.Viaje.eEstadoViaje.No_validado
                    imgEstado.ImageUrl = "~/App_Themes/Tema1/Images/EstadosValidacion/Rechazada.png"
                    imgEstado.ToolTip = itzultzaileWeb.Itzuli("Rechazada")
                Case ELL.Viaje.eEstadoViaje.Cancelado
                    imgEstado.ImageUrl = "~/App_Themes/Tema1/Images/EstadosValidacion/Rechazada.png"
                    imgEstado.ToolTip = itzultzaileWeb.Itzuli("Integrada")
            End Select
            e.Row.ToolTip = itzultzaileWeb.Itzuli("Hacer click para ir al detalle")
            e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gvViajes, "Select$" + CStr(oViaje.IdViaje))
        End If
    End Sub

    ''' <summary>
    ''' Evento de una fila del gridview.
    ''' Redirige a la hoja de gastos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvViajes_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvViajes.RowCommand
        If (e.CommandName = "Select") Then
            Dim param As String() = e.CommandArgument.ToString.Split(New Char() {"_"}, StringSplitOptions.RemoveEmptyEntries)
            Dim url As String = "~/Viaje/SolicitudViaje.aspx?id=" & param(0) & "&orig=ValVia"
            Response.Redirect(url)
        End If
    End Sub

    ''' <summary>
    ''' Paginacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvViajes_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvViajes.PageIndexChanging
        Try
            gvViajes.PageIndex = e.NewPageIndex
            BuscarViajes()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

End Class