Partial Public Class GestionAnticipos
    Inherits PageBase

    Private anticBLL As New BLL.AnticiposBLL

#Region "Page Load"

    ''' <summary>
    ''' Carga la vista del listado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Listado de anticipos"
                txtIdViajeF.Attributes.Add("placeholder", itzultzaileWeb.Itzuli("Nº de viaje"))
                searchUserF.PlaceHolder = itzultzaileWeb.Itzuli("Nombre, apellidos, nombre de usuario o numero de trabajador")
                If (Request.QueryString("returnIdViaje") Is Nothing) Then Session("filterAnt") = Nothing
                If (Not String.IsNullOrEmpty(Request.QueryString("idViaje"))) Then
                    Response.Redirect("DetalleAnticipo.aspx?idViaje=" & Request.QueryString("idViaje"), False)
                Else
                    txtIdViajeF.Text = String.Empty : searchUserF.Limpiar()
                    cargarAnticipos(Request.QueryString("returnIdViaje") IsNot Nothing, False)
                End If
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Message
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("errCargarPagina")
        End Try
    End Sub

    ''' <summary>
    ''' Busca utilizando el filtro
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnSearchF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearchF.Click
        Try
            Dim bFiltro As Boolean = (txtIdViajeF.Text <> String.Empty Or searchUserF.SelectedId <> String.Empty)  'Sino se rellena nada del filtro, se muestran de nuevo todos
            cargarAnticipos(bFiltro, True)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Resetea los valores del filtro
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnResetF_Click(sender As Object, e As EventArgs) Handles btnResetF.Click
        Try
            txtIdViajeF.Text = String.Empty
            searchUserF.Limpiar()
            cargarAnticipos(False, True)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' Escribe el titulo dependiendo en la vista en la que se encuentre
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelTitle) : itzultzaileWeb.Itzuli(btnSearchF) : itzultzaileWeb.Itzuli(btnResetF)
            itzultzaileWeb.Itzuli(labelResulCerradoSinFilter)
        End If
    End Sub

    ''' <summary>
    ''' Se configura el texto de la ayuda
    ''' </summary>    
    Private Sub TextoAyuda()
        'texto.AppendLine("- En esta página se tendra un control de las solicitudes de anticipos recibidas y facilmente, se verán en que estado se encuentran.")
        'texto.AppendLine("- Funcionamiento de los distintos estados:")
        'texto.AppendLine("   *Solicitados: Peticiones recibidas. Mientras se mantenga este estado, la persona que solicitó el anticipo podrá realizar cambios.")
        'texto.AppendLine("   *Preparados: Se mandara un email al responsable del anticipo para que sepa que ya puede pasarse a por recogerlo.")
        'texto.AppendLine("   *Entregados: Se entrega el anticipo al responsable y se queda marcado el movimiento")
        'texto.AppendLine("   *Cerrados: Al finalizar, se puede cerrar el anticipo")
        'texto.AppendLine("- En el detalle de la solicitud, se podra gestionar el anticipo")
        'texto.AppendLine("- Se podran modificar cierta informacion del viaje como fechas, destino, integrantes pulsando el link Modificar viaje.")
    End Sub

#End Region

#Region "Detalle"

    ''' <summary>
    ''' Carga el listado con las solicitudes de anticipos
    ''' </summary>
    ''' <param name="bBuscarFiltro">True si la llamada viene del filtro. False en caso contrario</param>
    ''' <param name="bOriginSearch">True se ha pulsado el boton de buscar. En este caso, no tomamos en cuenta el returnIdViaje</param>
    Private Sub cargarAnticipos(ByVal bBuscarFiltro As Boolean, ByVal bOriginSearch As Boolean)
        Dim estAgen As List(Of Integer)
        Dim count As Integer
        Dim bPestañaSel As Boolean = False
        If (Not Page.IsPostBack AndAlso Session("filterAnt") IsNot Nothing) Then
            Dim filter As String() = CType(Session("filterAnt"), String())
            txtIdViajeF.Text = filter(0)
            searchUserF.SelectedId = filter(1)
            searchUserF.Texto = filter(2)
            bBuscarFiltro = True 'Lo activamos para que haga el filtro
        End If
        'Se comprueba si existen anticipos cancelados en los que se haya entregado el anticipo pero no haya sido devuelto
        Dim lAnticiposNoDevueltos As List(Of String()) = anticBLL.loadListCanceladosNoDevueltos(Master.IdPlantaGestion)
        If (lAnticiposNoDevueltos IsNot Nothing AndAlso lAnticiposNoDevueltos.Count > 0) Then
            pnlAnticiposCancelados.Visible = True
            rptAnticCancel.DataSource = lAnticiposNoDevueltos
            rptAnticCancel.DataBind()
        Else
            pnlAnticiposCancelados.Visible = False
        End If
        gvaSolicitados.ClearVariables()
        gvaSolicitados.IdPlantaGestion = Master.IdPlantaGestion
        estAgen = New List(Of Integer) : estAgen.Add(ELL.Anticipo.EstadoAnticipo.solicitado)
        gvaSolicitados.EstadosAnticipo = estAgen
        If (bBuscarFiltro) Then
            If (searchUserF.SelectedId <> String.Empty And searchUserF.Texto.Trim <> String.Empty) Then gvaSolicitados.Id_Usuario = CInt(searchUserF.SelectedId)
            If (txtIdViajeF.Text.Trim <> String.Empty) Then gvaSolicitados.Id_Viaje = CInt(txtIdViajeF.Text)
        End If
        Dim returnIdViaje As Integer = If(bOriginSearch OrElse Request.QueryString("returnIdViaje") Is Nothing, 0, CInt(Request.QueryString("returnIdViaje")))
        Dim bExisteViaje As Boolean = False
        count = gvaSolicitados.PintarControl(returnIdViaje, bExisteViaje)
        tabP1.HeaderText = itzultzaileWeb.Itzuli("Solicitados") & " <b>( " & count & " )</b> "
        tabP1.ToolTip = itzultzaileWeb.Itzuli("Nuevas solicitudes que no se han revisado")
        If (bBuscarFiltro AndAlso count > 0) Then
            If (returnIdViaje > 0 AndAlso bExisteViaje) Then 'Si viene del detalle y esta aqui el viaje, se marca la pestaña
                tabPaneles.ActiveTab = tabP1
                bPestañaSel = True
            ElseIf (returnIdViaje = 0) Then 'Si no viene del detalle, siempre se marca la primera pestaña
                tabPaneles.ActiveTab = tabP1
                bPestañaSel = True
            End If
        End If
        gvaPreparados.ClearVariables()
        gvaPreparados.IdPlantaGestion = Master.IdPlantaGestion
        estAgen = New List(Of Integer) : estAgen.Add(ELL.Anticipo.EstadoAnticipo.Preparado)
        gvaPreparados.EstadosAnticipo = estAgen
        If (bBuscarFiltro) Then
            If (searchUserF.SelectedId <> String.Empty And searchUserF.Texto.Trim <> String.Empty) Then gvaPreparados.Id_Usuario = CInt(searchUserF.SelectedId)
            If (txtIdViajeF.Text.Trim <> String.Empty) Then gvaPreparados.Id_Viaje = CInt(txtIdViajeF.Text)
        End If
        count = gvaPreparados.PintarControl(returnIdViaje, bExisteViaje)
        tabP2.HeaderText = itzultzaileWeb.Itzuli("Preparados") & " <b>( " & count & " )</b> "
        tabP2.ToolTip = itzultzaileWeb.Itzuli("Solicitudes preparadas, esperando que el usuario venga a buscar el anticipo")
        If (bBuscarFiltro AndAlso Not bPestañaSel AndAlso count > 0) Then
            If (returnIdViaje > 0 AndAlso bExisteViaje) Then
                tabPaneles.ActiveTab = tabP2
                bPestañaSel = True
            ElseIf (returnIdViaje = 0) Then
                tabPaneles.ActiveTab = tabP2
                bPestañaSel = True
            End If
        End If
        gvaEntregados.ClearVariables()
        gvaEntregados.IdPlantaGestion = Master.IdPlantaGestion
        estAgen = New List(Of Integer) : estAgen.Add(ELL.Anticipo.EstadoAnticipo.Entregado)
        gvaEntregados.EstadosAnticipo = estAgen
        If (bBuscarFiltro) Then
            If (searchUserF.SelectedId <> String.Empty And searchUserF.Texto.Trim <> String.Empty) Then gvaEntregados.Id_Usuario = CInt(searchUserF.SelectedId)
            If (txtIdViajeF.Text.Trim <> String.Empty) Then gvaEntregados.Id_Viaje = CInt(txtIdViajeF.Text)
        End If
        count = gvaEntregados.PintarControl(returnIdViaje, bExisteViaje)
        tabP3.HeaderText = itzultzaileWeb.Itzuli("Entregados") & " <b>( " & count & " )</b> "
        tabP3.ToolTip = itzultzaileWeb.Itzuli("Solicitudes con los anticipos entregados")
        If (bBuscarFiltro AndAlso Not bPestañaSel AndAlso count > 0) Then
            If (returnIdViaje > 0 AndAlso bExisteViaje) Then
                tabPaneles.ActiveTab = tabP3
                bPestañaSel = True
            ElseIf (returnIdViaje = 0) Then
                tabPaneles.ActiveTab = tabP3
                bPestañaSel = True
            End If
        End If
        gvaCerrados.ClearVariables()
        gvaCerrados.IdPlantaGestion = Master.IdPlantaGestion
        estAgen = New List(Of Integer) : estAgen.Add(ELL.Anticipo.EstadoAnticipo.cerrado) : estAgen.Add(ELL.Anticipo.EstadoAnticipo.cancelada)
        gvaCerrados.EstadosAnticipo = estAgen
        Dim conRegistros As Boolean = False
        If (bBuscarFiltro) Then
            If (searchUserF.SelectedId <> String.Empty And searchUserF.Texto.Trim <> String.Empty) Then gvaCerrados.Id_Usuario = CInt(searchUserF.SelectedId)
            If (txtIdViajeF.Text.Trim <> String.Empty) Then gvaCerrados.Id_Viaje = CInt(txtIdViajeF.Text)
            If (gvaCerrados.Id_Usuario > 0 OrElse gvaCerrados.Id_Viaje > 0) Then
                count = gvaCerrados.PintarControl(returnIdViaje, bExisteViaje)
                conRegistros = True
                tabP4.HeaderText = itzultzaileWeb.Itzuli("Cerrados") & " <b>( " & count & " )</b> "
                If (Not bPestañaSel AndAlso count > 0) Then
                    If (returnIdViaje > 0 AndAlso bExisteViaje) Then
                        tabPaneles.ActiveTab = tabP4
                        bPestañaSel = True
                    ElseIf (returnIdViaje = 0) Then
                        tabPaneles.ActiveTab = tabP4
                        bPestañaSel = True
                    End If
                End If
            Else 'Sin filtros, no se busca                
                tabP4.HeaderText = itzultzaileWeb.Itzuli("Cerrados")
                tabP4.ToolTip = itzultzaileWeb.Itzuli("Viajes terminados y anticipos liquidados")
            End If
        Else
            tabP4.HeaderText = itzultzaileWeb.Itzuli("Cerrados")
            tabP4.ToolTip = itzultzaileWeb.Itzuli("Viajes terminados y anticipos liquidados")
        End If
        If (Not bBuscarFiltro) Then tabPaneles.ActiveTab = tabP1
        labelResulCerradoSinFilter.Visible = Not conRegistros
        If (Not conRegistros) Then gvaCerrados.Limpiar()
        'Se actualiza el filtro
        If (txtIdViajeF.Text = String.Empty AndAlso searchUserF.SelectedId = String.Empty) Then
            Session("filterAnt") = Nothing
        Else
            Dim sFiltro(2) As String
            sFiltro(0) = If(txtIdViajeF.Text <> String.Empty, txtIdViajeF.Text, "")
            sFiltro(1) = If(searchUserF.SelectedId <> String.Empty, searchUserF.SelectedId, "")
            sFiltro(2) = If(searchUserF.SelectedId <> String.Empty, searchUserF.Texto, "")
            Session("filterAnt") = sFiltro
        End If
    End Sub

    ''' <summary>
    ''' Se enlazan los anticipos no devueltos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub rptAnticCancel_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) Handles rptAnticCancel.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim item As String() = e.Item.DataItem
            Dim lnk As LinkButton = CType(e.Item.FindControl("lnkAnticCancel"), LinkButton)
            lnk.Text = itzultzaileWeb.Itzuli("Anticipo") & " " & item(0) & " - " & item(1)    'Ejemplo: Anticipo 11 - Luxemburgo
            lnk.CommandArgument = item(0)
        End If
    End Sub

    ''' <summary>
    ''' Al pulsar en un link de los anticipos entregados de viajes cancelados, se redirige a la pagina del anticipo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub LinkCancelacion(ByVal sender As Object, ByVal e As EventArgs)
        Dim lnk As LinkButton = CType(sender, LinkButton)
        mostrarDetalle(CInt(lnk.CommandArgument))
    End Sub

    ''' <summary>
    ''' Muestra el detalle de un viaje
    ''' </summary>
    ''' <param name="idViaje">Id del viaje</param>	
    Private Sub mostrarDetalle(ByVal idViaje As Integer) Handles gvaSolicitados.mostrarDetalle, gvaPreparados.mostrarDetalle, gvaEntregados.mostrarDetalle, gvaCerrados.mostrarDetalle
        Response.Redirect("DetalleAnticipo.aspx?idViaje=" & idViaje, False)
    End Sub

    ''' <summary>
    ''' Error ocurrido en el control
    ''' </summary>
    ''' <param name="mensaje"></param>    
    Private Sub gridviews_errorAnticipo(ByVal mensaje As String) Handles gvaSolicitados.errorAnticipo, gvaPreparados.errorAnticipo, gvaEntregados.errorAnticipo, gvaCerrados.errorAnticipo
        Master.MensajeError = mensaje
    End Sub

#End Region

End Class