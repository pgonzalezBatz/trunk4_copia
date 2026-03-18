Public Class DetPresupuestoFactura
    Inherits PageBase

    Private total, totalObjetivo As Decimal
    Private hUsuarios As Hashtable

#Region "Page Load"

    ''' <summary>
    ''' Carga de la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Detalle del presupuesto/factura"
                mostrarDetalle(CInt(Request.QueryString("id")))
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al cargar")
        End Try
    End Sub

    ''' <summary>
    ''' Traducciones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelDivViaje) : itzultzaileWeb.Itzuli(labelViaje) : itzultzaileWeb.Itzuli(labelFechas)
            itzultzaileWeb.Itzuli(labelSolicitante) : itzultzaileWeb.Itzuli(labelRespVal) : itzultzaileWeb.Itzuli(labelViajeros)
            itzultzaileWeb.Itzuli(labelDivFactura) : itzultzaileWeb.Itzuli(labelDivPresupuesto) : itzultzaileWeb.Itzuli(labelEstado)
            itzultzaileWeb.Itzuli(labelPresupTotal) : itzultzaileWeb.Itzuli(labelFacturadoTotal)
        End If
    End Sub

#End Region

#Region "Mostrar detalle"

    ''' <summary>
    ''' Se muestra el detalle del presupuesto/factura
    ''' </summary>
    ''' <param name="idViaje">Id del viaje</param>
    Private Sub mostrarDetalle(ByVal idViaje As Integer)
        Dim oViaje As ELL.Viaje = InfoViaje(idViaje)
        InfoPresupuesto(oViaje)
        InfoFactura(idViaje)
    End Sub

#Region "Info Viaje"

    ''' <summary>
    ''' Se carga la informacion del viaje
    ''' </summary>
    ''' <param name="idViaje">Id del viaje</param>
    ''' <returns></returns>
    Private Function InfoViaje(ByVal idViaje As Integer) As ELL.Viaje
        Try
            Dim viajesBLL As New BLL.ViajesBLL
            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Dim oViaje As ELL.Viaje = viajesBLL.loadInfo(idViaje, bSoloCabecera:=True)
            oViaje.ListaIntegrantes = viajesBLL.loadIntegrantes(idViaje)
            lblIdViaje.Text = oViaje.IdViaje & " - " & oViaje.Destino
            lblFIda.Text = oViaje.FechaIda.ToShortDateString
            lblFVuelta.Text = oViaje.FechaVuelta.ToShortDateString
            lblSolicitante.Text = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = oViaje.IdUserSolicitador}, False).NombreCompleto
            oViaje.ListaIntegrantes = oViaje.ListaIntegrantes.OrderBy(Of String)(Function(o) o.Usuario.NombreCompleto).ToList
            gvIntegrantes.DataSource = oViaje.ListaIntegrantes
            gvIntegrantes.DataBind()
            Return oViaje
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al cargar la informacion del viaje", ex)
        End Try
    End Function

    ''' <summary>
    ''' Enlaza los integrantes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvIntegrantes_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvIntegrantes.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim integ As ELL.Viaje.Integrante = CType(e.Row.DataItem, ELL.Viaje.Integrante)
            CType(e.Row.FindControl("lblIntegrante"), Label).Text = integ.Usuario.NombreCompleto
            CType(e.Row.FindControl("lblFechasViaje"), Label).Text = integ.FechaIda.ToShortDateString & " - " & integ.FechaVuelta.ToShortDateString
        End If
    End Sub

#End Region

#Region "Info Factura"

    ''' <summary>
    ''' Se carga la informacion de las facturas de un viaje
    ''' </summary>
    ''' <param name="idViaje">Id del viaje</param>
    Private Sub InfoFactura(ByVal idViaje As Integer)
        Try
            Dim factBLL As New BLL.SolicAgenciasBLL
            Dim lFacturas As List(Of ELL.FakturaEroski) = factBLL.loadFacturasEroskiViaje(idViaje)
            hUsuarios = New Hashtable
            lblFacturadoTotal.Text = Math.Round(lFacturas.Sum(Function(o) o.Importe), 2)
            lFacturas = lFacturas.OrderBy(Function(o) o.Producto).ThenByDescending(Function(o) o.FechaServicio).ToList
            gvFacturado.DataSource = lFacturas
            gvFacturado.DataBind()
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al cargar la informacion del presupuesto", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se enlazan las facturas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub gvFacturado_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvFacturado.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim fact As ELL.FakturaEroski = CType(e.Row.DataItem, ELL.FakturaEroski)
            CType(e.Row.FindControl("lblFactura"), Label).Text = fact.Factura
            CType(e.Row.FindControl("lblFechaServ"), Label).Text = fact.FechaServicio.ToShortDateString
            CType(e.Row.FindControl("lblDestino"), Label).Text = fact.Destino
            CType(e.Row.FindControl("lblProducto"), Label).Text = fact.Producto
            CType(e.Row.FindControl("lblProveedor"), Label).Text = fact.Proveedor
            CType(e.Row.FindControl("lblImporte"), Label).Text = fact.Importe
            If (Not hUsuarios.ContainsKey(fact.IdUser)) Then
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                Dim oUser As SabLib.ELL.Usuario = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = fact.IdUser}, False)
                hUsuarios.Add(fact.IdUser, oUser.NombreCompleto)
            End If
            CType(e.Row.FindControl("lblPersona"), Label).Text = hUsuarios.Item(fact.IdUser)
        End If
    End Sub

#End Region

#Region "Info Presupuesto"

    ''' <summary>
    ''' Se carga la informacion del presupuesto
    ''' </summary>
    ''' <param name="oViaje">Informacion del viaje</param>    
    Private Sub InfoPresupuesto(ByVal oViaje As ELL.Viaje)
        Try
            Dim presupBLL As New BLL.PresupuestosBLL
            total = 0 : totalObjetivo = 0
            Dim oPresup As ELL.Presupuesto = presupBLL.loadInfo(oViaje.IdViaje)
            If (oPresup Is Nothing) Then
                lblEstado.Text = itzultzaileWeb.Itzuli("Presupuesto no encontrado")
                lblEstado.CssClass = "label label-danger"
            Else
                lblEstado.Text = [Enum].GetName(GetType(ELL.Presupuesto.EstadoPresup), oPresup.Estado).ToUpper
                Select Case oPresup.Estado
                    Case ELL.Presupuesto.EstadoPresup.Validado
                        Dim lEstados As List(Of ELL.Presupuesto.HistoricoEstado) = oPresup.Estados.OrderByDescending(Function(f) f.ChangeDate).ToList
                        If (lEstados.Item(1).State <> ELL.Presupuesto.EstadoPresup.Enviado) Then lblEstado.Text = itzultzaileWeb.Itzuli("Autovalidado").ToString.ToUpper
                        lblEstado.CssClass = "label label-success"
                    Case ELL.Presupuesto.EstadoPresup.Rechazado
                        lblEstado.CssClass = "label label-danger"
                    Case ELL.Presupuesto.EstadoPresup.Enviado
                        lblEstado.CssClass = "label label-info"
                    Case ELL.Presupuesto.EstadoPresup.Creado, ELL.Presupuesto.EstadoPresup.Generado
                        lblEstado.CssClass = "label label-default"
                End Select
                Dim oServ As ELL.Presupuesto.Servicio = Nothing
                Dim costeObj, costeTotal As Decimal
                costeObj = 0 : costeTotal = costeTotal = 0
                For Each serv As ELL.Presupuesto.Servicio.Tipo_Servicio In [Enum].GetValues(GetType(ELL.Presupuesto.Servicio.Tipo_Servicio))
                    oServ = oPresup.Servicios.Find(Function(o) o.TipoServicio = serv)
                    If (oServ IsNot Nothing) Then
                        costeObj = oServ.TarifaObjetivoTotal(oViaje.ListaIntegrantes.Count)
                        costeTotal = oServ.TarifaRealTotal(oViaje.ListaIntegrantes.Count)
                        total += costeTotal : totalObjetivo += costeObj
                    Else
                        costeTotal = 0
                    End If
                    Select Case serv
                        Case ELL.Presupuesto.Servicio.Tipo_Servicio.Avion : lblAvion.Text = costeTotal & " €"
                        Case ELL.Presupuesto.Servicio.Tipo_Servicio.Hotel : lblHotel.Text = costeTotal & " €"
                        Case ELL.Presupuesto.Servicio.Tipo_Servicio.Coche_Alquiler : lblCocheAlq.Text = costeTotal & " €"
                    End Select
                Next
                If (total <= totalObjetivo) Then
                    lblTotal.CssClass = "label label-success"
                Else
                    lblTotal.CssClass = "label label-danger"
                End If
                lblTotal.Style.Add("font-size", "18px")
                lblTotal.Text = Math.Round(total, 2)
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                lblRespVal.Text = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = oPresup.IdUsuarioResponsable}, False).NombreCompleto
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al cargar la informacion del presupuesto", ex)
        End Try
    End Sub

    '''' <summary>
    '''' Se pintan los servicios aereos si tuvieran
    '''' </summary>
    '''' <param name="lServ">Lista de servicios</param>   
    '''' <param name="lIntegrantes">Lista de integrantes</param> 
    'Private Sub PintarServiciosAvion(ByVal lServ As List(Of ELL.Presupuesto.Servicio), ByVal lIntegrantes As List(Of ELL.Viaje.Integrante))
    '    Try
    '        lServ = lServ.FindAll(Function(o As ELL.Presupuesto.Servicio) o.TipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Avion)
    '        If (lServ IsNot Nothing AndAlso lServ.Count > 0) Then
    '            Dim tarifBLL As New BLL.TarifasServBLL
    '            Dim oTarifa As ELL.TarifaServicios
    '            Dim cssClass, tooltip As String
    '            Dim fIda As Date
    '            Dim styles As Dictionary(Of String, String) = Nothing
    '            Dim tarifaObjetivo, tarifaReal As Decimal
    '            Dim myDivPanelServ, myDivBodyServ As HtmlGenericControl
    '            Dim numIntegr As Integer = lIntegrantes.Count
    '            fIda = lIntegrantes.Min(Function(o) o.FechaIda)
    '            Dim oServ As ELL.Presupuesto.Servicio = lServ.First
    '            myDivPanelServ = Nothing : myDivBodyServ = Nothing
    '            AddHeader(itzultzaileWeb.Itzuli("Avion"), "panel panel-info", myDivPanelServ, myDivBodyServ)
    '            AddLinea(myDivBodyServ, New With {.HeaderText = itzultzaileWeb.Itzuli("Fecha"), .Value = GetDateTime(oServ.Fecha1), .cssClass = "", .Tooltip = "", .Styles = Nothing})
    '            AddLinea(myDivBodyServ, New With {.HeaderText = itzultzaileWeb.Itzuli("Ciudad origen"), .Value = oServ.Ciudad1, .cssClass = "", .Tooltip = "", .Styles = Nothing},
    '                                    New With {.HeaderText = itzultzaileWeb.Itzuli("Ciudad destino"), .Value = oServ.Ciudad2, .cssClass = "", .Tooltip = "", .Styles = Nothing})
    '            tarifaObjetivo = 0
    '            If (oServ.IdTarifaDestino <> Integer.MinValue) Then
    '                oTarifa = tarifBLL.loadTarifaInfo(oServ.IdTarifaDestino)
    '                Dim myTarifaAnno As ELL.TarifaServicios.Lineas = oTarifa.LineasTarifa.Find(Function(o As ELL.TarifaServicios.Lineas) o.Anno = fIda.Year)
    '                If (myTarifaAnno IsNot Nothing) Then tarifaObjetivo = (myTarifaAnno.TarifaAvion * numIntegr)
    '            End If
    '            totalObjetivo += tarifaObjetivo
    '            tarifaReal = (oServ.TarifaReal * numIntegr)
    '            total += tarifaReal
    '            cssClass = If(oServ.TarifaReal > tarifaObjetivo, "label label-danger", "label label-success")
    '            tooltip = itzultzaileWeb.Itzuli("Tarifa por persona") & " x " & numIntegr & " "
    '            tooltip &= " " & If(numIntegr > 1, itzultzaileWeb.Itzuli("personas").ToString.ToLower, itzultzaileWeb.Itzuli("persona").ToString.ToLower)
    '            styles = New Dictionary(Of String, String)
    '            styles.Add("font-size", "16px")
    '            AddLinea(myDivBodyServ, New With {.HeaderText = itzultzaileWeb.Itzuli("Objetivo"), .Value = tarifaObjetivo, .cssClass = "", .Tooltip = tooltip, .Styles = Nothing},
    '                                    New With {.HeaderText = itzultzaileWeb.Itzuli("Real"), .Value = tarifaReal, .CssClass = cssClass, .Tooltip = "", .Styles = styles})
    '            myDivPanelServ.Controls.Add(myDivBodyServ)
    '            phServicios.Controls.Add(myDivPanelServ)
    '        End If
    '    Catch batzEx As BatzException
    '        Throw batzEx
    '    Catch ex As Exception
    '        Throw New BatzException("Error al mostrar los servicios aereos", ex)
    '    End Try
    'End Sub

    '''' <summary>
    '''' Se pintan los servicios de hotel si tuvieran
    '''' </summary>
    '''' <param name="lServ">Lista de servicios</param>    
    '''' <param name="lIntegrantes">Lista de integrantes</param> 
    'Private Sub PintarServiciosHotel(ByVal lServ As List(Of ELL.Presupuesto.Servicio), ByVal lIntegrantes As List(Of ELL.Viaje.Integrante))
    '    Try
    '        lServ = lServ.FindAll(Function(o As ELL.Presupuesto.Servicio) o.TipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Hotel)
    '        If (lServ IsNot Nothing AndAlso lServ.Count > 0) Then
    '            Dim tarifBLL As New BLL.TarifasServBLL
    '            Dim oTarifa As ELL.TarifaServicios
    '            Dim cssClass, tooltip As String
    '            Dim fIda As Date
    '            Dim styles As Dictionary(Of String, String) = Nothing
    '            Dim tarifaObjetivo, tarifaReal As Decimal : Dim numDiasReserva As Integer
    '            Dim myDivPanelServ, myDivBodyServ As HtmlGenericControl
    '            Dim numIntegr As Integer = lIntegrantes.Count
    '            fIda = lIntegrantes.Min(Function(o) o.FechaIda)
    '            Dim oServ As ELL.Presupuesto.Servicio = lServ.First
    '            myDivPanelServ = Nothing : myDivBodyServ = Nothing
    '            AddHeader(itzultzaileWeb.Itzuli("Hotel"), "panel panel-info", myDivPanelServ, myDivBodyServ)
    '            AddLinea(myDivBodyServ, New With {.HeaderText = itzultzaileWeb.Itzuli("Fecha entrada"), .Value = GetDate(oServ.Fecha1), .cssClass = "", .Tooltip = "", .Styles = Nothing},
    '                                        New With {.HeaderText = itzultzaileWeb.Itzuli("Fecha salida"), .Value = GetDate(oServ.Fecha2), .cssClass = "", .Tooltip = "", .Styles = Nothing})
    '            AddLinea(myDivBodyServ, New With {.HeaderText = itzultzaileWeb.Itzuli("Ciudad"), .Value = oServ.Ciudad1, .cssClass = "", .Tooltip = "", .Styles = Nothing},
    '                                        New With {.HeaderText = itzultzaileWeb.Itzuli("Tipo habitacion"), .Value = If(oServ.TipoHabitacion >= 0, [Enum].GetName(GetType(ELL.Presupuesto.Servicio.Tipo_Habitacion), oServ.TipoHabitacion), String.Empty), .cssClass = "", .Tooltip = "", .Styles = Nothing})
    '            AddLinea(myDivBodyServ, New With {.HeaderText = itzultzaileWeb.Itzuli("Hotel"), .Value = oServ.Nombre, .cssClass = "", .Tooltip = "", .Styles = Nothing},
    '                                        New With {.HeaderText = itzultzaileWeb.Itzuli("Regimen"), .Value = If(oServ.Regimen >= 0, [Enum].GetName(GetType(ELL.Presupuesto.Servicio.eRegimen), oServ.Regimen).ToString.Replace("_", " "), String.Empty), .cssClass = "", .Tooltip = "", .Styles = Nothing})
    '            tarifaObjetivo = 0 : numDiasReserva = 0
    '            If (oServ.Fecha1 <> Date.MinValue AndAlso oServ.Fecha2 <> Date.MinValue) Then numDiasReserva = oServ.Fecha2.Subtract(oServ.Fecha1).Days
    '            If (oServ.IdTarifaDestino <> Integer.MinValue) Then
    '                oTarifa = tarifBLL.loadTarifaInfo(oServ.IdTarifaDestino)
    '                Dim myTarifaAnno As ELL.TarifaServicios.Lineas = oTarifa.LineasTarifa.Find(Function(o As ELL.TarifaServicios.Lineas) o.Anno = fIda.Year)
    '                If (myTarifaAnno IsNot Nothing) Then tarifaObjetivo = (myTarifaAnno.TarifaHotel * numDiasReserva * numIntegr)
    '            End If
    '            totalObjetivo += tarifaObjetivo
    '            tarifaReal = (oServ.TarifaReal * numDiasReserva * numIntegr)
    '            total += tarifaReal
    '            cssClass = If(oServ.TarifaReal > tarifaObjetivo, "label label-danger", "label label-success")
    '            tooltip = itzultzaileWeb.Itzuli("Tarifa por persona y noche") & " x " & numDiasReserva
    '            tooltip &= " " & If(numDiasReserva > 1, itzultzaileWeb.Itzuli("dias").ToString.ToLower, itzultzaileWeb.Itzuli("dia").ToString.ToLower) & " x " & numIntegr & " "
    '            tooltip &= " " & If(numIntegr > 1, itzultzaileWeb.Itzuli("personas").ToString.ToLower, itzultzaileWeb.Itzuli("persona").ToString.ToLower)
    '            styles = New Dictionary(Of String, String)
    '            styles.Add("font-size", "16px")
    '            AddLinea(myDivBodyServ, New With {.HeaderText = itzultzaileWeb.Itzuli("Objetivo"), .Value = tarifaObjetivo, .cssClass = "", .Tooltip = tooltip, .Styles = Nothing},
    '                                        New With {.HeaderText = itzultzaileWeb.Itzuli("Real"), .Value = tarifaReal, .cssClass = cssClass, .Tooltip = "", .Styles = styles})
    '            myDivPanelServ.Controls.Add(myDivBodyServ)
    '            phServicios.Controls.Add(myDivPanelServ)
    '        End If
    '    Catch batzEx As BatzException
    '        Throw batzEx
    '    Catch ex As Exception
    '        Throw New BatzException("Error al mostrar los servicios de hotel", ex)
    '    End Try
    'End Sub

    '''' <summary>
    '''' Se pintan los servicios de coche de alquiler si tuvieran
    '''' </summary>
    '''' <param name="lServ">Lista de servicios</param>  
    '''' <param name="lIntegrantes">Lista de integrantes</param>       
    'Private Sub PintarServiciosCocheAlquiler(ByVal lServ As List(Of ELL.Presupuesto.Servicio), ByVal lIntegrantes As List(Of ELL.Viaje.Integrante))
    '    Try
    '        lServ = lServ.FindAll(Function(o As ELL.Presupuesto.Servicio) o.TipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Coche_Alquiler)
    '        If (lServ IsNot Nothing AndAlso lServ.Count > 0) Then
    '            Dim numDiasReserva As Integer = 0
    '            Dim styles As Dictionary(Of String, String) = Nothing
    '            Dim tarifaReal, tarifaObjetivo As Decimal
    '            Dim tarifBLL As New BLL.TarifasServBLL
    '            Dim oTarifa As ELL.TarifaServicios
    '            Dim tooltip, cssClass As String
    '            Dim myDivPanelServ, myDivBodyServ As HtmlGenericControl
    '            Dim oServ As ELL.Presupuesto.Servicio = lServ.First
    '            myDivPanelServ = Nothing : myDivBodyServ = Nothing
    '            AddHeader(itzultzaileWeb.Itzuli("Coche"), "panel panel-info", myDivPanelServ, myDivBodyServ)
    '            AddLinea(myDivBodyServ, New With {.HeaderText = itzultzaileWeb.Itzuli("Fecha recogida"), .Value = GetDateTime(oServ.Fecha1), .cssClass = "", .Tooltip = "", .Styles = Nothing},
    '                                        New With {.HeaderText = itzultzaileWeb.Itzuli("Lugar recogida"), .Value = oServ.Ciudad1, .cssClass = "", .Tooltip = "", .Styles = Nothing})
    '            AddLinea(myDivBodyServ, New With {.HeaderText = itzultzaileWeb.Itzuli("Fecha devolucion"), .Value = GetDateTime(oServ.Fecha2), .cssClass = "", .Tooltip = "", .Styles = Nothing},
    '                                        New With {.HeaderText = itzultzaileWeb.Itzuli("Lugar devolucion"), .Value = oServ.Ciudad2, .cssClass = "", .Tooltip = "", .Styles = Nothing})
    '            AddLinea(myDivBodyServ, New With {.HeaderText = itzultzaileWeb.Itzuli("Categoria"), .Value = oServ.Categoria, .cssClass = "", .Tooltip = "", .Styles = Nothing})
    '            If (oServ.Fecha1 <> Date.MinValue AndAlso oServ.Fecha2 <> Date.MinValue) Then
    '                numDiasReserva = Math.Ceiling(CDate(oServ.Fecha2).Subtract(CDate(oServ.Fecha1)).TotalHours / 24)
    '                If (numDiasReserva = 0) Then numDiasReserva = 1 'Si se coge y se deja en el mismo dia, sera solo un dia
    '            End If
    '            tarifaObjetivo = 0
    '            If (oServ.IdTarifaDestino <> Integer.MinValue) Then
    '                oTarifa = tarifBLL.loadTarifaInfo(oServ.IdTarifaDestino)
    '                Dim myTarifaAnno As ELL.TarifaServicios.Lineas = oTarifa.LineasTarifa.Find(Function(o As ELL.TarifaServicios.Lineas) o.Anno = oServ.Fecha1.Year)
    '                If (myTarifaAnno IsNot Nothing) Then tarifaObjetivo = (myTarifaAnno.TarifaCocheAlquiler * numDiasReserva)
    '            End If
    '            tarifaReal = (oServ.TarifaReal * numDiasReserva)
    '            totalObjetivo += tarifaObjetivo
    '            total += tarifaReal
    '            cssClass = If(tarifaReal = 0, "label label-default", "label label-success")
    '            tooltip = itzultzaileWeb.Itzuli("Tarifa coche por dia") & " x " & numDiasReserva
    '            tooltip &= " " & If(numDiasReserva > 1, itzultzaileWeb.Itzuli("dias").ToString.ToLower, itzultzaileWeb.Itzuli("dia").ToString.ToLower)
    '            styles = New Dictionary(Of String, String)
    '            styles.Add("font-size", "16px")
    '            AddLinea(myDivBodyServ, New With {.HeaderText = itzultzaileWeb.Itzuli("Objetivo"), .Value = tarifaObjetivo, .cssClass = "", .Tooltip = tooltip, .Styles = Nothing},
    '                                        New With {.HeaderText = itzultzaileWeb.Itzuli("Real"), .Value = tarifaReal, .cssClass = cssClass, .Tooltip = "", .Styles = styles})
    '            myDivPanelServ.Controls.Add(myDivBodyServ)
    '            phServicios.Controls.Add(myDivPanelServ)
    '        End If
    '    Catch batzEx As BatzException
    '        Throw batzEx
    '    Catch ex As Exception
    '        Throw New BatzException("Error al mostrar los servicios de coche de alquiler", ex)
    '    End Try
    'End Sub

    '''' <summary>
    '''' Se pintan los servicios de tren si tuvieran
    '''' </summary>
    '''' <param name="lServ">Lista de servicios</param>
    '''' <param name="lIntegrantes">Lista de integrantes</param>         
    'Private Sub PintarServiciosTren(ByVal lServ As List(Of ELL.Presupuesto.Servicio), ByVal lIntegrantes As List(Of ELL.Viaje.Integrante))
    '    Try
    '        lServ = lServ.FindAll(Function(o As ELL.Presupuesto.Servicio) o.TipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Tren)
    '        If (lServ IsNot Nothing AndAlso lServ.Count > 0) Then
    '            Dim styles As Dictionary(Of String, String) = Nothing
    '            Dim tooltip, cssClass As String
    '            Dim tarifaReal As Decimal
    '            Dim myDivBody, myDivPanelServ, myDivBodyServ As HtmlGenericControl
    '            myDivBody = Nothing
    '            Dim numIntegr As Integer = lIntegrantes.Count
    '            Dim oServ As ELL.Presupuesto.Servicio = lServ.First
    '            myDivPanelServ = Nothing : myDivBodyServ = Nothing
    '            AddHeader(itzultzaileWeb.Itzuli("Tren"), "panel panel-info", myDivPanelServ, myDivBodyServ)
    '            AddLinea(myDivBodyServ, New With {.HeaderText = itzultzaileWeb.Itzuli("Fecha"), .Value = GetDateTime(oServ.Fecha1), .cssClass = "", .Tooltip = "", .Styles = Nothing},
    '                                        New With {.HeaderText = itzultzaileWeb.Itzuli("Clase"), .Value = oServ.Clase, .cssClass = "", .Tooltip = "", .Styles = Nothing})
    '            AddLinea(myDivBodyServ, New With {.HeaderText = itzultzaileWeb.Itzuli("Ciudad origen"), .Value = oServ.Ciudad1, .cssClass = "", .Tooltip = "", .Styles = Nothing},
    '                                        New With {.HeaderText = itzultzaileWeb.Itzuli("Ciudad destino"), .Value = oServ.Ciudad2, .cssClass = "", .Tooltip = "", .Styles = Nothing})
    '            tarifaReal = (oServ.TarifaReal * numIntegr)
    '            totalObjetivo += tarifaReal  'Se le suma la misma cantidad porque aqui no hay objetivo
    '            total += tarifaReal
    '            tooltip = itzultzaileWeb.Itzuli("Tarifa por persona") & " x " & numIntegr & " "
    '            tooltip &= " " & If(numIntegr > 1, itzultzaileWeb.Itzuli("personas").ToString.ToLower, itzultzaileWeb.Itzuli("persona").ToString.ToLower)
    '            cssClass = If(tarifaReal = 0, "label label-default", "label label-success") 'Como no hay objetivo, siempre se cumple
    '            styles = New Dictionary(Of String, String)
    '            styles.Add("font-size", "16px")
    '            AddLinea(myDivBodyServ, New With {.HeaderText = itzultzaileWeb.Itzuli("Objetivo"), .Value = tarifaReal, .cssClass = "", .Tooltip = tooltip, .Styles = Nothing},
    '                                        New With {.HeaderText = itzultzaileWeb.Itzuli("Real"), .Value = tarifaReal, .cssClass = cssClass, .Tooltip = "", .Styles = styles})
    '            myDivPanelServ.Controls.Add(myDivBodyServ)
    '            myDivBody.Controls.Add(myDivPanelServ)
    '            phServicios.Controls.Add(myDivBody)
    '        End If
    '    Catch batzEx As BatzException
    '        Throw batzEx
    '    Catch ex As Exception
    '        Throw New BatzException("Error al mostrar los servicios de tren", ex)
    '    End Try
    'End Sub

    ''' <summary>
    ''' Obtiene la fecha
    ''' </summary>
    ''' <param name="fecha">Fecha</param>
    ''' <returns></returns>    
    Private Function GetDate(ByVal fecha As Date) As String
        Dim myfecha As String = String.Empty
        If (fecha <> DateTime.MinValue) Then myfecha = fecha.ToShortDateString
        Return myfecha
    End Function

    ''' <summary>
    ''' Obtiene la fecha y la hora
    ''' </summary>
    ''' <param name="fecha">Fecha</param>
    ''' <returns></returns>    
    Private Function GetDateTime(ByVal fecha As DateTime) As String
        Dim myfecha As String = String.Empty
        If (fecha <> DateTime.MinValue) Then
            myfecha = fecha.ToShortDateString
            If (fecha.Hour <> 0 Or fecha.Minute <> 0) Then myfecha &= " " & fecha.ToShortTimeString
        End If
        Return myfecha
    End Function

    ''' <summary>
    ''' Añade los controles a la cabecera
    ''' </summary>
    ''' <param name="headerText"></param>
    Private Sub AddHeader(ByVal headerText As String, ByVal headerCssClass As String, ByRef divPanel As HtmlGenericControl, ByRef divBody As HtmlGenericControl)
        divPanel = New HtmlGenericControl With {.TagName = "div"}
        divPanel.Attributes.Add("class", headerCssClass)
        Dim myDivHeader As New HtmlGenericControl With {.TagName = "div"}
        myDivHeader.Attributes.Add("class", "panel-heading")
        Dim myLabel As New Label With {.Text = headerText}
        myLabel.Style.Add("font-weight", "bold")
        myDivHeader.Controls.Add(myLabel)
        divPanel.Controls.Add(myDivHeader)
        divBody = New HtmlGenericControl With {.TagName = "div"}
        divBody.Attributes.Add("class", "panel-body")
    End Sub

    ''' <summary>
    ''' Se añade una linea con posibles 4 columnas
    ''' </summary>
    ''' <param name="myDivBody">Div por referencia donde se añade la linea</param>    
    ''' <param name="value1">Objeto con los valores para las primeras dos columnas</param>    
    Private Sub AddLinea(ByRef myDivBody As HtmlGenericControl, ByVal value1 As Object, Optional ByVal value2 As Object = Nothing)
        Dim myDiv, myDiv2 As HtmlGenericControl
        Dim myLabel As Label
        myDiv = New HtmlGenericControl With {.TagName = "div"}
        myDiv.Attributes.Add("class", "row")
        myDiv2 = New HtmlGenericControl With {.TagName = "div"}
        myDiv2.Attributes.Add("class", "col-sm-2")
        myLabel = New Label With {.Text = value1.HeaderText}
        myDiv2.Controls.Add(myLabel)
        If (value1.Tooltip <> String.Empty) Then
            Dim span As New HtmlGenericControl With {.TagName = "span"} : span.Attributes.Add("class", "glyphicon glyphicon-info-sign text-info") : span.Attributes.Add("title", value1.Tooltip) : span.Style.Add("cursor", "help") : span.Style.Add("margin-left", "5px")
            myDiv2.Controls.Add(span)
        End If
        myDiv.Controls.Add(myDiv2)
        myDiv2 = New HtmlGenericControl With {.TagName = "div"}
        myDiv2.Attributes.Add("class", "col-sm-4")
        myLabel = New Label With {.Text = value1.Value} : myLabel.Style.Add("font-weight", "bold")
        If (value1.CssClass <> String.Empty) Then myLabel.Attributes.Add("class", value1.CssClass)
        If (value1.Styles IsNot Nothing) Then
            For Each myStyle In value1.Styles
                myLabel.Style.Add(myStyle.Key, myStyle.Value)
            Next
        End If
        myDiv2.Controls.Add(myLabel) : myDiv.Controls.Add(myDiv2)
        If (value2 IsNot Nothing) Then
            myDiv2 = New HtmlGenericControl With {.TagName = "div"}
            myDiv2.Attributes.Add("class", "col-sm-2")
            myLabel = New Label With {.Text = value2.HeaderText}
            myDiv2.Controls.Add(myLabel) : myDiv.Controls.Add(myDiv2)
            myDiv2 = New HtmlGenericControl With {.TagName = "div"}
            myDiv2.Attributes.Add("class", "col-sm-4")
            myLabel = New Label With {.Text = value2.Value} : myLabel.Style.Add("font-weight", "bold")
            If (value2.CssClass <> String.Empty) Then myLabel.Attributes.Add("class", value2.CssClass)
            If (value2.Styles IsNot Nothing) Then
                For Each myStyle In value2.Styles
                    myLabel.Style.Add(myStyle.Key, myStyle.Value)
                Next
            End If
            myDiv2.Controls.Add(myLabel) : myDiv.Controls.Add(myDiv2)
        End If
        myDivBody.Controls.Add(myDiv)
    End Sub

#End Region

#End Region

End Class