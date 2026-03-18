Partial Public Class AsignacionVisas
    Inherits PageBase

#Region "Vista Listado"

#Region "Page Load e inicializaciones"

    ''' <summary>
    ''' Inicializa la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Master.SetTitle = "Asignacion de Visas"
            Try
                gvVisas.Attributes("CurrentSortField") = "Nombre"
                gvVisas.Attributes("CurrentSortDirection") = SortDirection.Ascending
                searchUserF.PlaceHolder = itzultzaileWeb.Itzuli("Nº visa, nombre, apellidos, nombre de usuario o numero de trabajador")
                cargarVisas()
            Catch batzEx As BatzException
                Master.MensajeError = batzEx.Termino
            Catch ex As Exception
                Master.MensajeError = itzultzaileWeb.Itzuli("errCargarPagina")
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub AsignacionVisas_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(lnkAsignacion) : itzultzaileWeb.Itzuli(labelTitle) : itzultzaileWeb.Itzuli(labelNumTarj)
            itzultzaileWeb.Itzuli(chbObsoleto) : itzultzaileWeb.Itzuli(btnGuardar) : itzultzaileWeb.Itzuli(btnVolver)
            itzultzaileWeb.Itzuli(labelAsiga)
        End If
    End Sub

    ''' <summary>
    ''' Carga el listado de visas
    ''' </summary>	
    Private Sub cargarVisas()
        Dim visasBLL As New BLL.VisasBLL
        Dim myVisa As New ELL.Visa With {.IdPlanta = Master.IdPlantaGestion}
        If (searchUserF.SelectedId <> String.Empty) Then myVisa.Propietario = New SabLib.ELL.Usuario With {.Id = searchUserF.SelectedId}
        Dim lVisas As List(Of ELL.Visa) = visasBLL.loadList(myVisa, Master.IdPlantaGestion)
        If (lVisas.Count > 0) Then OrdenarLista(lVisas, gvVisas.Attributes("CurrentSortField"), gvVisas.Attributes("CurrentSortDirection"))
        gvVisas.DataSource = lVisas
        gvVisas.DataBind()
        mvVisas.ActiveViewIndex = 0
    End Sub

#End Region

#Region "Eventos Gridview"

    ''' <summary>
    '''  Carga del listado de integrantes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Private Sub gvVisas_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvVisas.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim visa As ELL.Visa = DirectCast(e.Row.DataItem, ELL.Visa)
            DirectCast(e.Row.FindControl("lblAsignadoA"), Label).Text = visa.Propietario.NombreCompleto
            If (visa.Obsoleta) Then
                e.Row.ToolTip = itzultzaileWeb.Itzuli("Tarjeta obsoleta")
                e.Row.CssClass = "danger"
            End If
            e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gvVisas, "Select$" + CStr(visa.Id))
        End If
    End Sub

    ''' <summary>
    ''' <para>Evento que captura el click del raton en cualquier area de la fila.</para>
    ''' <para>Redirecciona al detalle</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Protected Sub gvVisas_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvVisas.RowCommand
        Try
            If (e.CommandName = "Select") Then
                mostrarDetalle(e.CommandArgument)
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Paginacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvVisas_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvVisas.PageIndexChanging
        Try
            gvVisas.PageIndex = e.NewPageIndex
            cargarVisas()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Ordenacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvVisas_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs) Handles gvVisas.Sorting
        Try
            gvVisas.Attributes("CurrentSortField") = e.SortExpression
            If (gvVisas.Attributes("CurrentSortDirection") Is Nothing) Then
                gvVisas.Attributes("CurrentSortDirection") = SortDirection.Ascending
            Else
                gvVisas.Attributes("CurrentSortDirection") = If(gvVisas.Attributes("CurrentSortDirection") = SortDirection.Ascending, SortDirection.Descending, SortDirection.Ascending)
            End If
            cargarVisas()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Ordena una lista sin conocer el tipo que es
    ''' </summary>
    ''' <param name="lista"></param>    
    ''' <param name="CampoOrden"></param>
    ''' <param name="DireccionCampo"></param>        
    Private Sub OrdenarLista(ByRef lista As List(Of ELL.Visa), Optional ByVal CampoOrden As String = Nothing, Optional ByVal DireccionCampo As SortDirection = SortDirection.Ascending)
        Select Case CampoOrden
            Case "NumTarjeta"
                If (DireccionCampo = SortDirection.Ascending) Then
                    lista = lista.OrderBy(Of String)(Function(o) o.NumTarjeta).ToList
                Else
                    lista = lista.OrderByDescending(Of String)(Function(o) o.NumTarjeta).ToList
                End If
            Case "Nombre"
                If (DireccionCampo = SortDirection.Ascending) Then
                    lista = lista.OrderBy(Of String)(Function(o) o.Propietario.NombreCompleto).ToList
                Else
                    lista = lista.OrderByDescending(Of String)(Function(o) o.Propietario.NombreCompleto).ToList
                End If
        End Select
    End Sub

#End Region

#Region "Botones"

    ''' <summary>
    ''' Al seleccionar un usuario o visa del filtro, se realiza la busqueda
    ''' </summary>
    ''' <param name="id"></param>
    Private Sub searchUserF_ItemSeleccionado(id As Integer) Handles searchUserF.ItemSeleccionado
        Try
            cargarVisas()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Configura para realizar una nueva asignacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnAsignacion_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkAsignacion.Click
        Try
            mostrarDetalle(Integer.MinValue)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#End Region

#Region "Vista Detalle"

    ''' <summary>
    ''' Muestra el detalle de una asignacion
    ''' </summary>
    ''' <param name="id"></param>	
    Private Sub mostrarDetalle(ByVal id As Integer)
        Dim bNueva As Boolean = (id = Integer.MinValue)
        inicializarDetalle()
        pnlNuevo1.Visible = bNueva : pnlNuevo2.Visible = bNueva
        pnlEdicion1.Visible = Not bNueva : pnlEdicion2.Visible = Not bNueva
        If (Not bNueva) Then
            Dim visasBLL As New BLL.VisasBLL
            Dim oVisa As ELL.Visa = visasBLL.loadInfo(New ELL.Visa With {.Id = id})
            lblNumTarjeta.Text = oVisa.NumTarjeta
            lblNombrePersona.Text = oVisa.Propietario.NombreCompleto
            searchUserNew.SelectedId = oVisa.Propietario.Id
            chbObsoleto.Checked = oVisa.Obsoleta
            divObsoleto.Visible = True
            btnGuardar.CommandArgument = id
        End If
        mvVisas.ActiveViewIndex = 1
    End Sub

    ''' <summary>
    ''' Inicializa los controles del detalle
    ''' </summary>	
    Private Sub inicializarDetalle()
        txtNumTarjeta.Text = String.Empty : lblNumTarjeta.Text = String.Empty : lblNombrePersona.Text = String.Empty
        chbObsoleto.Checked = False : divObsoleto.Visible = False
        searchUserNew.Limpiar()
        btnGuardar.Visible = True : btnGuardar.CommandArgument = String.Empty
        pnlNuevo1.Visible = True : pnlNuevo2.Visible = True : pnlEdicion1.Visible = False : pnlEdicion2.Visible = False
    End Sub

    ''' <summary>
    ''' Guarda la asignacion o cambios realizados
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Private Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Try
            If (btnGuardar.CommandArgument = String.Empty AndAlso (txtNumTarjeta.Text.Trim = String.Empty OrElse searchUserNew.SelectedId = String.Empty)) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("debeRellenarDatos")
            Else
                Dim antesObsoleta As Boolean = False
                Dim visasBLL As New BLL.VisasBLL
                Dim oVisa As New ELL.Visa With {.IdPlanta = Master.IdPlantaGestion, .Propietario = New SabLib.ELL.Usuario With {.Id = CInt(searchUserNew.SelectedId)}}
                If (btnGuardar.CommandArgument <> String.Empty) Then
                    oVisa.Id = CInt(btnGuardar.CommandArgument)
                    oVisa.Obsoleta = chbObsoleto.Checked
                    oVisa.NumTarjeta = lblNumTarjeta.Text
                    Dim myVisaOld As ELL.Visa = visasBLL.loadInfo(New ELL.Visa With {.NumTarjeta = oVisa.NumTarjeta})
                    antesObsoleta = myVisaOld.Obsoleta
                Else  'Nuevo
                    oVisa.NumTarjeta = txtNumTarjeta.Text.Trim
                    oVisa.FechaEntrega = CDate(Now.ToShortDateString)
                    Dim oVisaExistente As ELL.Visa = visasBLL.loadInfo(oVisa)
                    If (oVisaExistente IsNot Nothing) Then
                        Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("La tarjeta visa ya esta asociada") & ": " & oVisaExistente.Propietario.NombreCompleto
                        Exit Sub
                    End If
                End If
                If (oVisa.Id <= 0) Then
                    'Se comprueba si puede recibir visas
                    Dim bidaiakBLL As New BLL.BidaiakBLL
                    If (Not bidaiakBLL.PuedeRecibirVisasAnticipos(oVisa.Propietario, Master.IdPlantaGestion)) Then
                        Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("El usuario no tiene permitido recibir una tarjeta de visa. En caso de ser necesario, hablar con RRHH")
                        Exit Sub
                    End If
                End If
                visasBLL.Save(oVisa)
                If (btnGuardar.CommandArgument = String.Empty) Then
                    log.Info("ASIGNACION_VISAS: Se ha realizado una nueva asignacion de visa. Tarjeta " & txtNumTarjeta.Text & " a la persona " & searchUserNew.Texto & "(" & searchUserNew.SelectedId & ")")
                Else
                    Dim mensa As String = "ASIGNACION_VISAS: La tarjeta " & lblNumTarjeta.Text & " "
                    Dim bWrite As Boolean = False
                    If (chbObsoleto.Checked And Not antesObsoleta) Then
                        mensa &= "se ha marcado como obsoleta"
                        bWrite = True
                    ElseIf (Not chbObsoleto.Checked And antesObsoleta) Then
                        mensa &= "se ha desmarcado de obsoleta"
                        bWrite = True
                    End If
                    If (bWrite) Then log.Info(mensa)
                End If
                Volver()
                Master.MensajeInfo = itzultzaileWeb.Itzuli("datosGuardados")
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Vuelve al listado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Private Sub imgVolver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnVolver.Click
        Volver()
    End Sub

    ''' <summary>
    ''' Vuelve al listado
    ''' </summary>	
    Private Sub Volver()
        Try
            cargarVisas()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("errCargarPagina")
        End Try
    End Sub

#End Region

End Class