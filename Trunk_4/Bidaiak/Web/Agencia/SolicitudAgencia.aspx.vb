Partial Public Class SolicitudAgencia
    Inherits PageBase

#Region "Page Load"

    ''' <summary>
    ''' Carga la vista del listado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Solicitudes de agencia"
                txtIdViajeF.Attributes.Add("placeholder", itzultzaileWeb.Itzuli("Nº de viaje"))
                searchUserF.PlaceHolder = itzultzaileWeb.Itzuli("Nombre, apellidos, nombre de usuario o numero de trabajador")
                If (Request.QueryString("returnIdViaje") Is Nothing) Then Session("filterAge") = Nothing
                If (Not String.IsNullOrEmpty(Request.QueryString("idViaje"))) Then
                    Response.Redirect("DetalleAgencia.aspx?idViaje=" & Request.QueryString("idViaje"), False)
                Else
                    txtIdViajeF.Text = String.Empty : searchUserF.Limpiar()
                    cargarSolicitudes(Request.QueryString("returnIdViaje") IsNot Nothing, False)
                End If
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Message
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("errCargarPagina")
        End Try
    End Sub

    Private Sub TextoAyuda()
        '"   *Nuevas: Peticiones recibidas. Mientras se mantenga este estado, la persona que solicitó el servicio de agencia podrá realizar cambios."
        '"   *En Tramite: En este estado, la persona solicitante ya no podrá realizar ningun cambio. Para ello, simplemente habrá que entrar en la solicitud y pulsar el boton de guardar")
        '"   *Gestionado: Una vez que ya se sepan los datos de avion, hotel, precios, etc... se marcarán como Gestionada. Para ello, habra que seleccionar el check Gestionada y pulsar el boton de guardar")
        '"   *Cerrada: Al dia siguiente de la vuelta del viaje, se cerrará automaticamente")                        
    End Sub

    ''' <summary>
    ''' Escribe el titulo dependiendo en la vista en la que se encuentre
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub SolicitudAgencia_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(btnSearchF) : itzultzaileWeb.Itzuli(labelTitle) : itzultzaileWeb.Itzuli(btnResetF)
            itzultzaileWeb.Itzuli(labelResulCerradoSinFilter)
        End If
    End Sub

    ''' <summary>
    ''' Busca utilizando el filtro
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnSearchF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearchF.Click
        Try
            Dim bFiltro As Boolean = (txtIdViajeF.Text <> String.Empty Or searchUserF.SelectedId <> String.Empty)  'Sino se rellena nada del filtro, se muestran de nuevo todos
            cargarSolicitudes(bFiltro, True)
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
            cargarSolicitudes(False, True)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Detalle"

    ''' <summary>
    ''' Carga el listado con las solicitudes validadas y con servicio de agencia requerido
    ''' </summary>
    ''' <param name="bBuscarFiltro">True si la llamada viene del filtro. False en caso contrario</param>
    ''' <param name="bOriginSearch">True se ha pulsado el boton de buscar. En este caso, no tomamos en cuenta el returnIdViaje</param>
    Private Sub cargarSolicitudes(ByVal bBuscarFiltro As Boolean, ByVal bOriginSearch As Boolean)
        Dim estAgen As List(Of Integer)
        Dim count As Integer
        Dim bPestañaSel As Boolean = False
        If (Not Page.IsPostBack AndAlso Session("filterAge") IsNot Nothing) Then
            Dim filter As String() = CType(Session("filterAge"), String())
            txtIdViajeF.Text = filter(0)
            searchUserF.SelectedId = filter(1)
            searchUserF.Texto = filter(2)
            bBuscarFiltro = True 'Lo activamos para que haga el filtro
        End If
        'Solicitudes nuevas
        gvaNuevos.ClearVariables()
        gvaNuevos.IdPlantaGestion = Master.IdPlantaGestion
        estAgen = New List(Of Integer) : estAgen.Add(ELL.SolicitudAgencia.EstadoAgencia.solicitado)
        gvaNuevos.EstadosAgencia = estAgen
        If (bBuscarFiltro) Then
            If (searchUserF.SelectedId <> String.Empty And searchUserF.Texto.Trim <> String.Empty) Then gvaNuevos.Id_Usuario = CInt(searchUserF.SelectedId)
            If (txtIdViajeF.Text.Trim <> String.Empty) Then gvaNuevos.Id_Viaje = CInt(txtIdViajeF.Text)
        End If
        Dim returnIdViaje As Integer = If(bOriginSearch OrElse Request.QueryString("returnIdViaje") Is Nothing, 0, CInt(Request.QueryString("returnIdViaje")))
        Dim bExisteViaje As Boolean = False
        count = gvaNuevos.PintarControl(returnIdViaje, bExisteViaje)
        tabP1.HeaderText = itzultzaileWeb.Itzuli("Nuevas") & " <b>( " & count & " )</b> "
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
        'Solicitudes en tramite (Con cambios realizados pero sin marcar Gestionar)
        gvaTramite.ClearVariables()
        gvaTramite.IdPlantaGestion = Master.IdPlantaGestion
        estAgen = New List(Of Integer) : estAgen.Add(ELL.SolicitudAgencia.EstadoAgencia.Tramite)
        gvaTramite.EstadosAgencia = estAgen
        If (bBuscarFiltro) Then
            If (searchUserF.SelectedId <> String.Empty And searchUserF.Texto.Trim <> String.Empty) Then gvaTramite.Id_Usuario = CInt(searchUserF.SelectedId)
            If (txtIdViajeF.Text.Trim <> String.Empty) Then gvaTramite.Id_Viaje = CInt(txtIdViajeF.Text)
        End If
        count = gvaTramite.PintarControl(returnIdViaje, bExisteViaje)
        tabP2.HeaderText = itzultzaileWeb.Itzuli("En tramite") & " <b>( " & count & " )</b> "
        tabP2.ToolTip = itzultzaileWeb.Itzuli("Solicitudes revisadas pero sin marcar como gestionadas")
        If (bBuscarFiltro AndAlso Not bPestañaSel AndAlso count > 0) Then
            If (returnIdViaje > 0 AndAlso bExisteViaje) Then
                tabPaneles.ActiveTab = tabP2
                bPestañaSel = True
            ElseIf (returnIdViaje = 0) Then
                tabPaneles.ActiveTab = tabP2
                bPestañaSel = True
            End If
        End If
        'Solicitudes gestionadas
        gvaGestionados.ClearVariables()
        gvaGestionados.IdPlantaGestion = Master.IdPlantaGestion
        estAgen = New List(Of Integer) : estAgen.Add(ELL.SolicitudAgencia.EstadoAgencia.Gestionado)
        gvaGestionados.EstadosAgencia = estAgen
        If (bBuscarFiltro) Then
            If (searchUserF.SelectedId <> String.Empty And searchUserF.Texto.Trim <> String.Empty) Then gvaGestionados.Id_Usuario = CInt(searchUserF.SelectedId)
            If (txtIdViajeF.Text.Trim <> String.Empty) Then gvaGestionados.Id_Viaje = CInt(txtIdViajeF.Text)
        End If
        count = gvaGestionados.PintarControl(returnIdViaje, bExisteViaje)
        tabP3.HeaderText = itzultzaileWeb.Itzuli("Gestionadas") & " <b>( " & count & " )</b> "
        tabP3.ToolTip = itzultzaileWeb.Itzuli("Solicitudes gestionadas")
        If (bBuscarFiltro AndAlso Not bPestañaSel AndAlso count > 0) Then
            If (returnIdViaje > 0 AndAlso bExisteViaje) Then
                tabPaneles.ActiveTab = tabP3
                bPestañaSel = True
            ElseIf (returnIdViaje = 0) Then
                tabPaneles.ActiveTab = tabP3
                bPestañaSel = True
            End If
        End If
        'Solicitudes cerradas
        gvaCerrados.ClearVariables()
        gvaCerrados.IdPlantaGestion = Master.IdPlantaGestion
        estAgen = New List(Of Integer) : estAgen.Add(ELL.SolicitudAgencia.EstadoAgencia.Cerrada) : estAgen.Add(ELL.SolicitudAgencia.EstadoAgencia.cancelada)
        gvaCerrados.EstadosAgencia = estAgen
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
                tabP4.HeaderText = itzultzaileWeb.Itzuli("cerradas")
                tabP4.ToolTip = itzultzaileWeb.Itzuli("Solicitudes cerradas")
            End If
        Else
            tabP4.HeaderText = itzultzaileWeb.Itzuli("cerradas")
            tabP4.ToolTip = itzultzaileWeb.Itzuli("Solicitudes cerradas")
        End If
        If (Not bBuscarFiltro) Then tabPaneles.ActiveTab = tabP1
        labelResulCerradoSinFilter.Visible = Not conRegistros
        If (Not conRegistros) Then gvaCerrados.Limpiar()
        'Se actualiza el filtro
        If (txtIdViajeF.Text = String.Empty AndAlso searchUserF.SelectedId = String.Empty) Then
            Session("filterAge") = Nothing
        Else
            Dim sFiltro(2) As String
            sFiltro(0) = If(txtIdViajeF.Text <> String.Empty, txtIdViajeF.Text, "")
            sFiltro(1) = If(searchUserF.SelectedId <> String.Empty, searchUserF.SelectedId, "")
            sFiltro(2) = If(searchUserF.SelectedId <> String.Empty, searchUserF.Texto, "")
            Session("filterAge") = sFiltro
        End If
    End Sub

    ''' <summary>
    ''' Muestra el detalle de un viaje
    ''' </summary>
    ''' <param name="idViaje">Id del viaje</param>	
    Private Sub mostrarDetalle(ByVal idViaje As Integer) Handles gvaNuevos.mostrarDetalle, gvaTramite.mostrarDetalle, gvaGestionados.mostrarDetalle, gvaCerrados.mostrarDetalle
        Response.Redirect("DetalleAgencia.aspx?idViaje=" & idViaje, False)
    End Sub

    ''' <summary>
    ''' Error ocurrido en el control
    ''' </summary>
    ''' <param name="mensaje"></param>    
    Private Sub gvaCerrados_ControlError(ByVal mensaje As String) Handles gvaCerrados.ControlError, gvaNuevos.ControlError, gvaGestionados.ControlError, gvaTramite.ControlError
        Master.MensajeError = mensaje
    End Sub

#End Region

End Class