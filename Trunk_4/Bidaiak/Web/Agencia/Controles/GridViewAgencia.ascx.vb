Partial Public Class GridViewAgencia
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
    ''' Estado
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public Property EstadosAgencia As List(Of Integer)
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
        gvSolicitud.DataSource = Nothing
        gvSolicitud.DataBind()
        gvSolicitud.Visible = False
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

    Private pg As New PageBase

#End Region

#Region "Evento"

    ''' <summary>
    ''' Evento para ver el detalle
    ''' </summary>
    ''' <param name="idAgencia">Id de la agencia</param>    
    Public Event mostrarDetalle(ByVal idAgencia As Integer)

    ''' <summary>
    ''' Evento de error
    ''' </summary>
    ''' <param name="mensaje">Mensaje</param>    
    Public Event ControlError(ByVal mensaje As String)

#End Region

#Region "Funciones publicas"

    ''' <summary>
    ''' Pinta el gridview
    ''' </summary>	
    ''' <param name="idViajeSearch">Id del viaje a buscar en el control</param>
    ''' <param name="bExiste">Indica si existe dicho viaje en el grid</param>
    Public Function PintarControl(ByVal idViajeSearch As Integer, ByRef bExiste As Boolean) As Integer
        Try
            Dim count As Integer = 0
            Dim viajesBLL As New BLL.ViajesBLL
            IdViajeToSearch = idViajeSearch
            Dim lSolicitudes As List(Of String()) = viajesBLL.loadListWithAgency(IdPlantaGestion, EstadosAgencia, AñoMostrar, MesMostrar, Id_Usuario, Id_Viaje)
            If (gvSolicitud.Attributes("CurrentSortField") Is Nothing) Then
                gvSolicitud.Attributes("CurrentSortField") = "FechaIda"
                gvSolicitud.Attributes("CurrentSortDirection") = SortDirection.Descending
            End If
            Dim solicitudes As List(Of String()) = lSolicitudes.FindAll(Function(o As String()) CoincideEstado(CType(o(4), ELL.SolicitudAgencia.EstadoAgencia), EstadosAgencia))
            If (solicitudes IsNot Nothing AndAlso solicitudes.Count > 0) Then
                OrdenarLista(solicitudes, gvSolicitud.Attributes("CurrentSortField"), gvSolicitud.Attributes("CurrentSortDirection"))
                count = solicitudes.Count
                bExiste = solicitudes.Exists(Function(o) CInt(o(0)) = idViajeSearch)
            Else
                bExiste = False
            End If
            gvSolicitud.Visible = True
            gvSolicitud.DataSource = solicitudes
            gvSolicitud.DataBind()
            Return count
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al pintar el control GridViewAgencia", ex)
        End Try
    End Function

    ''' <summary>
    ''' Busca un objeto en una lista
    ''' </summary>
    ''' <param name="estObj">Estado del objeto</param>
    ''' <param name="lista">lista donde abra que buscar</param>
    ''' <returns></returns>    
    Private Function CoincideEstado(ByVal estObj As ELL.SolicitudAgencia.EstadoAgencia, ByVal lista As List(Of Integer)) As Boolean
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
    Private Sub OrdenarLista(ByRef lista As List(Of String()), Optional ByVal CampoOrden As String = Nothing, Optional ByVal DireccionCampo As SortDirection = SortDirection.Ascending)
        Select Case CampoOrden
            Case "IdViaje"
                If (DireccionCampo = SortDirection.Ascending) Then
                    lista = lista.OrderBy(Of Int32)(Function(o) CInt(o(0))).ToList
                Else
                    lista = lista.OrderByDescending(Of Int32)(Function(o) CInt(o(0))).ToList
                End If
            Case "FechaIda"
                If (DireccionCampo = SortDirection.Ascending) Then
                    lista = lista.OrderBy(Of Date)(Function(o) CDate(o(2))).ToList
                Else
                    lista = lista.OrderByDescending(Of Date)(Function(o) CDate(o(2))).ToList
                End If
            Case "FechaVuelta"
                If (DireccionCampo = SortDirection.Ascending) Then
                    lista = lista.OrderBy(Of Date)(Function(o) CDate(o(3))).ToList
                Else
                    lista = lista.OrderByDescending(Of Date)(Function(o) CDate(o(3))).ToList
                End If
            Case "Destino"
                If (DireccionCampo = SortDirection.Ascending) Then
                    lista = lista.OrderBy(Of String)(Function(o) o(1)).ToList
                Else
                    lista = lista.OrderByDescending(Of String)(Function(o) o(1)).ToList
                End If
        End Select
    End Sub

#End Region

#Region "Eventos gridview"

    ''' <summary>
    ''' Se enlazan las solicitudes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Private Sub gvSolicitud_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvSolicitud.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header OrElse e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            pg.itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim solicitud As String() = DirectCast(e.Row.DataItem, String())
            Dim destino As String = solicitud(8)
            If (destino Is Nothing OrElse (destino IsNot Nothing AndAlso destino.Length = 0)) Then destino = solicitud(1)
            DirectCast(e.Row.FindControl("lblIdViaje"), Label).Text = "V" & solicitud(0)
            DirectCast(e.Row.FindControl("lblDestino"), Label).Text = destino
            DirectCast(e.Row.FindControl("lblFechaIda"), Label).Text = CType(solicitud(2), Date).ToShortDateString
            DirectCast(e.Row.FindControl("lblFechaVuelta"), Label).Text = CType(solicitud(3), Date).ToShortDateString
            DirectCast(e.Row.FindControl("chbAlbaran"), CheckBox).Checked = (CInt(solicitud(5)) > 0)
            Dim estadoPresup As String = pg.itzultzaileWeb.Itzuli([Enum].GetName(GetType(ELL.Presupuesto.EstadoPresup), CInt(solicitud(6))))
            Dim lblEstadoPresup As Label = DirectCast(e.Row.FindControl("lblEstadoPresup"), Label)
            Select Case CInt(solicitud(6))
                Case ELL.Presupuesto.EstadoPresup.Creado, ELL.Presupuesto.EstadoPresup.Generado
                    lblEstadoPresup.CssClass = "label label-warning"
                Case ELL.Presupuesto.EstadoPresup.Enviado
                    lblEstadoPresup.CssClass = "label label-info"
                Case ELL.Presupuesto.EstadoPresup.Validado
                    lblEstadoPresup.CssClass = "label label-success"
                Case ELL.Presupuesto.EstadoPresup.Rechazado
                    lblEstadoPresup.CssClass = "label label-danger"
            End Select
            lblEstadoPresup.Text = estadoPresup
            If (CInt(solicitud(0)) = IdViajeToSearch) Then e.Row.CssClass = "info"
            e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gvSolicitud, "Select$" + CStr(solicitud(0)))
        End If
    End Sub

    ''' <summary>
    ''' <para>Evento que captura el click del raton en cualquier area de la fila.</para>
    ''' <para>Redirecciona al detalle</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Protected Sub gvSolicitudes_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvSolicitud.RowCommand
        If (e.CommandName = "Select") Then
            Try
                RaiseEvent mostrarDetalle(e.CommandArgument)
            Catch batzEx As BatzException
                RaiseEvent ControlError(batzEx.Termino)
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Paginacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvSolicitud_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvSolicitud.PageIndexChanging
        Try
            gvSolicitud.PageIndex = e.NewPageIndex
            PintarControl(IdViajeToSearch, False)
        Catch batzEx As BatzException
            RaiseEvent ControlError(batzEx.Termino)
        End Try
    End Sub

    ''' <summary>
    ''' Ordenacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvSolicitud_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs) Handles gvSolicitud.Sorting
        Try
            gvSolicitud.Attributes("CurrentSortField") = e.SortExpression
            If (gvSolicitud.Attributes("CurrentSortDirection") Is Nothing) Then
                gvSolicitud.Attributes("CurrentSortDirection") = SortDirection.Ascending
            Else
                gvSolicitud.Attributes("CurrentSortDirection") = If(gvSolicitud.Attributes("CurrentSortDirection") = SortDirection.Ascending, SortDirection.Descending, SortDirection.Ascending)
            End If
            PintarControl(IdViajeToSearch, False)
        Catch batzEx As BatzException
            RaiseEvent ControlError(batzEx.Termino)
        End Try
    End Sub

#End Region

End Class