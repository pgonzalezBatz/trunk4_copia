Partial Public Class SeleccionViaje
    Inherits UserControl

    ''' <summary>
    ''' Cuando se selecciona un viaje, se lanza un evento
    ''' </summary>
    ''' <param name="idViaje">Id del viaje seleccionado</param>	
    Public Event ViajeSeleccionado(ByVal idViaje As Integer)

    Public Property IdViajeOmitir() As Integer
        Get
            If (ViewState("IdViajeOmitir") Is Nothing) Then
                Return Integer.MinValue
            Else
                Return CInt(ViewState("IdViajeOmitir"))
            End If
        End Get
        Set(ByVal value As Integer)
            ViewState("IdViajeOmitir") = value
        End Set
    End Property

    ''' <summary>
    ''' Año en el que se buscara
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public Property Año() As Integer
        Get
            If (ViewState("Anno") Is Nothing) Then
                Return 0
            Else
                Return CInt(ViewState("Anno"))
            End If
        End Get
        Set(ByVal value As Integer)
            ViewState("Anno") = value
        End Set
    End Property

    ''' <summary>
    ''' Mes en el que se buscara
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public Property Mes() As Integer
        Get
            If (ViewState("Mes") Is Nothing) Then
                Return 0
            Else
                Return CInt(ViewState("Mes"))
            End If
        End Get
        Set(ByVal value As Integer)
            ViewState("Mes") = value
        End Set
    End Property

    ''' <summary>
    ''' Que tengan anticipo
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public Property ConAntipo() As Boolean
        Get
            If (ViewState("Ant") Is Nothing) Then
                Return False
            Else
                Return CBool(ViewState("Ant"))
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("Ant") = value
        End Set
    End Property

    ''' <summary>
    ''' Id del usuario del que se buscaran los viajes
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public Property IdUsuario() As Integer
        Get
            If (ViewState("IdUsuario") Is Nothing) Then
                Return 0
            Else
                Return CInt(ViewState("IdUsuario"))
            End If
        End Get
        Set(ByVal value As Integer)
            ViewState("IdUsuario") = value
        End Set
    End Property

    ''' <summary>
    ''' Id del viaje que se buscara
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public Property IdViaje() As Integer
        Get
            If (ViewState("IdViaje") Is Nothing) Then
                Return 0
            Else
                Return CInt(ViewState("IdViaje"))
            End If
        End Get
        Set(ByVal value As Integer)
            ViewState("IdViaje") = value
        End Set
    End Property

    Private pg As New PageBase

#Region "Carga del control"

    ''' <summary>
    ''' Inicializa el control
    ''' </summary>	 
    Public Sub Inicializar()
        pnlError.Visible = False
        lblMensaje.Text = String.Empty
        cargarViajes()
    End Sub

#End Region

#Region "Gridview"

    ''' <summary>
    ''' Evento surgido al pulsar en el icono para quitar una persona del listado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Protected Sub SeleccionarViaje(ByVal sender As Object, ByVal e As EventArgs)
        Dim lnk As LinkButton = CType(sender, LinkButton)
        RaiseEvent ViajeSeleccionado(CInt(lnk.CommandArgument))
    End Sub

    ''' <summary>
    ''' Paginacion de viajes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Private Sub gvViajes_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvViajes.PageIndexChanging
        Try
            gvViajes.PageIndex = e.NewPageIndex
            cargarViajes()
        Catch batzEx As BatzException
            pnlError.Visible = True
            lblMensaje.Text = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    '''  Carga del listado de viajes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Private Sub gvViajes_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvViajes.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            pg.itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim viaje As ELL.Viaje = DirectCast(e.Row.DataItem, ELL.Viaje)
            Dim lnkSel As LinkButton = DirectCast(e.Row.FindControl("lnkSel"), LinkButton)
            lnkSel.CommandArgument = viaje.IdViaje
            DirectCast(e.Row.FindControl("lblIdViaje"), Label).Text = viaje.IdViaje
            DirectCast(e.Row.FindControl("lblDestino"), Label).Text = viaje.Destino
            DirectCast(e.Row.FindControl("lblFechaIda"), Label).Text = CType(viaje.FechaIda, Date).ToShortDateString
            DirectCast(e.Row.FindControl("lblFechaVuelta"), Label).Text = CType(viaje.FechaVuelta, Date).ToShortDateString
            Dim integr As String = String.Empty
            For Each oInt As SabLib.ELL.Usuario In viaje.getUsuariosIntegrantes
                If (integr <> String.Empty) Then integr &= ","
                integr &= oInt.NombreCompleto
            Next
            DirectCast(e.Row.FindControl("lblIntegrantes"), Label).Text = integr
        End If
    End Sub

#End Region

#Region "Eventos"

    ''' <summary>
    ''' Busca los viajes vivos (Comenzados pero no finalizados)
    ''' </summary>	
    Private Sub cargarViajes()
        Try
            Dim viajeBLL As New BLL.ViajesBLL
            Dim oViaje As New ELL.Viaje With {.IdPlanta = CInt(Session("IdPlanta"))}
            If (Año > 0 Or Mes > 0) Then
                Dim mesBusqD, mesBusqH As Integer
                If (Mes > 0) Then
                    mesBusqD = Mes : mesBusqH = Mes
                Else
                    mesBusqD = 1 : mesBusqH = 12
                End If
                oViaje.FechaIda = New DateTime(Año, mesBusqD, 1)
                oViaje.FechaVuelta = New DateTime(Año, mesBusqH, Date.DaysInMonth(Año, mesBusqH))
            End If
            If (IdViaje > 0) Then oViaje.IdViaje = IdViaje
            If (IdUsuario > 0) Then
                Dim oUser As New ELL.Viaje.Integrante With {.Usuario = New SabLib.ELL.Usuario With {.Id = IdUsuario}}
                Dim lUsers As New List(Of ELL.Viaje.Integrante)
                lUsers.Add(oUser)
                oViaje.ListaIntegrantes = lUsers
            End If
            'No sacamos todos los viajes. Como mucho los de hace dos años y podriamos seleccionar los del futuro
            oViaje.FechaIda = Now.AddYears(-2) : oViaje.FechaVuelta = Now.AddYears(1)
            Dim lViajes As List(Of ELL.Viaje) = viajeBLL.loadList(oViaje, False, oViaje.IdPlanta, bFilterState:=False)
            'Si se ha informado el viaje a omitir, habra que buscar el viaje en la lista y si esta, quitarlo
            If (IdViajeOmitir <> Integer.MinValue) Then
                Dim myViaje As ELL.Viaje = lViajes.Find(Function(o As ELL.Viaje) o.IdViaje = IdViajeOmitir)
                If (myViaje IsNot Nothing) Then lViajes.Remove(myViaje)
            End If
            If (lViajes IsNot Nothing AndAlso lViajes.Count > 0 AndAlso ConAntipo) Then
                lViajes = lViajes.FindAll(Function(o As ELL.Viaje) o.Anticipo IsNot Nothing)
            End If
            If (lViajes IsNot Nothing) Then
                lViajes = lViajes.FindAll(Function(o As ELL.Viaje) o.Estado = ELL.Viaje.eEstadoViaje.Validado) 'Si no está validado por el CEO, lo omitiremos
                lViajes.Sort(Function(o1, o2) o1.FechaIda > o2.FechaIda)
            End If
            gvViajes.DataSource = lViajes
            gvViajes.DataBind()
        Catch ex As Exception
            Throw New BatzException("errCompBuscar", ex)
        End Try
    End Sub

#End Region

End Class