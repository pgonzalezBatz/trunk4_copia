Public Partial Class GridViewAnticipo
    Inherits UserControl

#Region "Properties"

    ''' <summary>
    ''' Id de la planta de gestion
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public Property IdPlantaGestion As Integer
        Get
            Return CInt(ViewState("IdPlantaGest_" & Me.ID))
        End Get
        Set(ByVal value As Integer)
            ViewState("IdPlantaGest_" & Me.ID) = value
        End Set
    End Property

    ''' <summary>
    ''' Estados
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public Property EstadosAnticipo As List(Of Integer)
        Get
            Return CType(ViewState("Estado_" & Me.ID), List(Of Integer))
        End Get
        Set(ByVal value As List(Of Integer))
            ViewState("Estado_" & Me.ID) = value
        End Set
    End Property

    ''' <summary>
    ''' Año a mostrar
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public Property AñoMostrar As Integer
        Get
            If (ViewState("Año_" & Me.ID) Is Nothing) Then
                Return Integer.MinValue
            Else
                Return CInt(ViewState("Año_" & Me.ID))
            End If
        End Get
        Set(ByVal value As Integer)
            ViewState("Año_" & Me.ID) = value
        End Set
    End Property

    ''' <summary>
    ''' Mes a mostrar
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public Property MesMostrar As Integer
        Get
            If (ViewState("Mes_" & Me.ID) Is Nothing) Then
                Return Integer.MinValue
            Else
                Return CInt(ViewState("Mes_" & Me.ID))
            End If
        End Get
        Set(ByVal value As Integer)
            ViewState("Mes_" & Me.ID) = value
        End Set
    End Property

    ''' <summary>
    ''' Usuario a mostrar
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public Property Id_Usuario As Integer
        Get
            If (ViewState("IdUser_" & Me.ID) Is Nothing) Then
                Return Integer.MinValue
            Else
                Return CInt(ViewState("IdUser_" & Me.ID))
            End If
        End Get
        Set(ByVal value As Integer)
            ViewState("IdUser_" & Me.ID) = value
        End Set
    End Property

    ''' <summary>
    ''' Identificador del viaje
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public Property Id_Viaje As Integer
        Get
            If (ViewState("IdViaje_" & Me.ID) Is Nothing) Then
                Return Integer.MinValue
            Else
                Return CInt(ViewState("IdViaje_" & Me.ID))
            End If
        End Get
        Set(ByVal value As Integer)
            ViewState("IdViaje_" & Me.ID) = value
        End Set
    End Property

    ''' <summary>
    ''' Viaje que se manda buscar
    ''' </summary>
    ''' <returns></returns>
    Private Property IdViajeToSearch As Integer
        Get
            If (ViewState("IdViajeToSearch_" & Me.ID) Is Nothing) Then
                Return Integer.MinValue
            Else
                Return CInt(ViewState("IdViajeToSearch_" & Me.ID))
            End If
        End Get
        Set(ByVal value As Integer)
            ViewState("IdViajeToSearch_" & Me.ID) = value
        End Set
    End Property

    ''' <summary>
    ''' Limpia el resultado y oculta el grid
    ''' </summary>
    Public Sub Limpiar()
        gvAnticipos.DataSource = Nothing
        gvAnticipos.DataBind()
        gvAnticipos.Visible = False
    End Sub

    ''' <summary>
    ''' Borra las variables de busqueda
    ''' </summary>
    Public Sub ClearVariables()
        IdPlantaGestion = Integer.MinValue
        MesMostrar = Integer.MinValue
        AñoMostrar = Integer.MinValue
        Id_Usuario = Integer.MinValue
        Id_Viaje = Integer.MinValue
        IdViajeToSearch = Integer.MinValue
    End Sub

#End Region

#Region "Evento"

	''' <summary>
	''' Evento para ver el detalle
	''' </summary>
    ''' <param name="idAnticipo">Id del anticipo</param>
    Public Event mostrarDetalle(ByVal idAnticipo As Integer)

    ''' <summary>
    ''' Evento de error
    ''' </summary>
    ''' <param name="mensa">Mensaje</param>    
    Public Event errorAnticipo(ByVal mensa As String)

    Private pg As New PageBase

#End Region

#Region "Funciones publicas"

    ''' <summary>
    ''' Pinta el gridview
    ''' </summary>	 
    ''' <param name="idViajeSearch">Id del viaje a buscar en el control</param>
    ''' <param name="bExiste">Indica si existe dicho viaje en el grid</param>
    ''' <returns>Devuelve el numero de elementos del listado</returns>
    Public Function PintarControl(ByVal idViajeSearch As Integer, ByRef bExiste As Boolean) As Integer
        Try
            Dim count As Integer = 0
            Dim viajesBLL As New BLL.ViajesBLL
            IdViajeToSearch = idViajeSearch
            Dim lAnticipos As List(Of Object) = viajesBLL.loadListWithAnticipo(IdPlantaGestion, EstadosAnticipo, AñoMostrar, MesMostrar, Id_Usuario, Id_Viaje)
            If (gvAnticipos.Attributes("CurrentSortField") Is Nothing) Then
                gvAnticipos.Attributes("CurrentSortField") = "FechaNecesidad"
                gvAnticipos.Attributes("CurrentSortDirection") = SortDirection.Ascending
            End If
            Dim anticipos As List(Of Object) = lAnticipos.FindAll(Function(o) CoincideEstado(CType(o.Estado, ELL.Anticipo.EstadoAnticipo), EstadosAnticipo))
            If (anticipos IsNot Nothing AndAlso anticipos.Count > 0) Then
                OrdenarLista(anticipos, gvAnticipos.Attributes("CurrentSortField"), gvAnticipos.Attributes("CurrentSortDirection"))
                count = anticipos.Count
                bExiste = anticipos.Exists(Function(o) o.IdViaje = idViajeSearch)
            Else
                bExiste = False
            End If
            gvAnticipos.Visible = True
            gvAnticipos.DataSource = anticipos
            gvAnticipos.DataBind()
            Return count
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al pintar el control GridViewAnticipo", ex)
        End Try
    End Function

    ''' <summary>
    ''' Busca un objeto en una lista
    ''' </summary>
    ''' <param name="estObj">Estado del objeto</param>
    ''' <param name="lista">lista donde abra que buscar</param>
    ''' <returns></returns>    
    Private Function CoincideEstado(ByVal estObj As ELL.Anticipo.EstadoAnticipo, ByVal lista As List(Of Integer)) As Boolean
        For Each est As Integer In lista
            If (est = estObj) Then
                Return True
            End If
        Next
        Return False
    End Function

    ''' <summary>
    ''' Ordena una lista sin conocer el tipo que es
    ''' </summary>
    ''' <param name="lista"></param>    
    ''' <param name="CampoOrden"></param>
    ''' <param name="DireccionCampo"></param>        
    Private Sub OrdenarLista(ByRef lista As List(Of Object), Optional ByVal CampoOrden As String = Nothing, Optional ByVal DireccionCampo As SortDirection = SortDirection.Ascending)
        Select Case CampoOrden
            Case "IdViaje"
                If (DireccionCampo = SortDirection.Ascending) Then
                    lista = lista.OrderBy(Of Integer)(Function(o) o.IdViaje).ToList
                Else
                    lista = lista.OrderByDescending(Of Integer)(Function(o) o.IdViaje).ToList
                End If
            Case "Destino"
                If (DireccionCampo = SortDirection.Ascending) Then
                    lista = lista.OrderBy(Of String)(Function(o) o.Destino).ToList
                Else
                    lista = lista.OrderByDescending(Of String)(Function(o) o.Destino).ToList
                End If
            Case "FechaNecesidad"
                If (DireccionCampo = SortDirection.Ascending) Then
                    lista = lista.OrderBy(Of Date)(Function(o) o.FechaNecesidad).ToList
                Else
                    lista = lista.OrderByDescending(Of Date)(Function(o) o.FechaNecesidad).ToList
                End If
            Case "Solicitante"
                If (DireccionCampo = SortDirection.Ascending) Then
                    lista = lista.OrderBy(Of String)(Function(o) o.Solicitante).ToList
                Else
                    lista = lista.OrderByDescending(Of String)(Function(o) o.Solicitante).ToList
                End If
            Case "Liquidador"
                If (DireccionCampo = SortDirection.Ascending) Then
                    lista = lista.OrderBy(Of String)(Function(o) o.Liquidador).ToList
                Else
                    lista = lista.OrderByDescending(Of String)(Function(o) o.Liquidador).ToList
                End If
        End Select
    End Sub

#End Region

#Region "Eventos gridview"

    ''' <summary>
    ''' Se enlazan los anticipos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Private Sub gvAnticipos_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvAnticipos.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header OrElse e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            pg.itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim anticipo = DirectCast(e.Row.DataItem, Object)
            Dim destino As String = anticipo.Destino
            If (destino = String.Empty) Then destino = anticipo.DestinoTarifa
            DirectCast(e.Row.FindControl("lblIdViaje"), Label).Text = "V" & anticipo.IdViaje
            DirectCast(e.Row.FindControl("lblDestino"), Label).Text = destino
            DirectCast(e.Row.FindControl("lblFechaRequiere"), Label).Text = CType(anticipo.FechaNecesidad, DateTime).ToShortDateString
            DirectCast(e.Row.FindControl("lblSolicitante"), Label).Text = anticipo.Solicitante
            DirectCast(e.Row.FindControl("lblLiquidador"), Label).Text = anticipo.Liquidador
            DirectCast(e.Row.FindControl("lblSolicitado"), Label).Text = If(anticipo.AnticipoSolicitado = String.Empty, pg.itzultzaileWeb.Itzuli("Transferencia"), anticipo.AnticipoSolicitado)
            If (anticipo.IdViaje = IdViajeToSearch) Then e.Row.CssClass = "info"
            e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gvAnticipos, "Select$" & CStr(anticipo.IdViaje))
        End If
    End Sub

    ''' <summary>
    ''' <para>Evento que captura el click del raton en cualquier area de la fila.</para>
    ''' <para>Redirecciona al detalle</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Protected Sub gvAnticipos_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvAnticipos.RowCommand
        Try
            If (e.CommandName = "Select") Then
                RaiseEvent mostrarDetalle(CInt(e.CommandArgument))
            End If
        Catch batzEx As BatzException
            RaiseEvent errorAnticipo(batzEx.Termino)
        Catch ex As Exception
            RaiseEvent errorAnticipo("Error al mostrar el detalle")
        End Try
    End Sub

    ''' <summary>
    ''' Paginacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvAnticipos_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvAnticipos.PageIndexChanging
        Try
            gvAnticipos.PageIndex = e.NewPageIndex
            PintarControl(IdViajeToSearch, False)
        Catch batzEx As BatzException
            RaiseEvent errorAnticipo(batzEx.Termino)
        End Try
    End Sub

    ''' <summary>
    ''' Ordenacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvAnticipos_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs) Handles gvAnticipos.Sorting
        Try
            gvAnticipos.Attributes("CurrentSortField") = e.SortExpression
            If (gvAnticipos.Attributes("CurrentSortDirection") Is Nothing) Then
                gvAnticipos.Attributes("CurrentSortDirection") = SortDirection.Ascending
            Else
                gvAnticipos.Attributes("CurrentSortDirection") = If(gvAnticipos.Attributes("CurrentSortDirection") = SortDirection.Ascending, SortDirection.Descending, SortDirection.Ascending)
            End If
            PintarControl(IdViajeToSearch, False)
        Catch batzEx As BatzException
            RaiseEvent errorAnticipo(batzEx.Termino)
        End Try
    End Sub

#End Region

End Class