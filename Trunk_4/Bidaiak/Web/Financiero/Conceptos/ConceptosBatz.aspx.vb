Public Class ConceptosBatz
    Inherits PageBase

#Region "Page Load"

    ''' <summary>
    ''' Carga la pagina de mantenimiento
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Conceptos Batz"
                mvConceptos.ActiveViewIndex = 0
                cargarConceptos()
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
    Private Sub ConceptosBatz_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(lnbNuevo) : itzultzaileWeb.Itzuli(labelNombre) : itzultzaileWeb.Itzuli(rfvNombre)
            itzultzaileWeb.Itzuli(chbRequiereDet) : itzultzaileWeb.Itzuli(chbMostrarHGRecibo) : itzultzaileWeb.Itzuli(chbObsoleto)
            itzultzaileWeb.Itzuli(btnGuardar) : itzultzaileWeb.Itzuli(btnVolver) : itzultzaileWeb.Itzuli(lblMensaje)
            itzultzaileWeb.Itzuli(chbMostrarHGSinRecibo) : itzultzaileWeb.Itzuli(labelInfo) : itzultzaileWeb.Itzuli(labelInfo2)
            itzultzaileWeb.Itzuli(labelInfo3) : itzultzaileWeb.Itzuli(labelInfo4)
        End If
    End Sub

    ''' <summary>
    ''' Inicializa el formulario de detalle
    ''' </summary>	
    Private Sub inicializarDetalle()
        txtNombre.Text = String.Empty : txtNombre.Enabled = True
        chbRequiereDet.Checked = False : chbRequiereDet.Enabled = True
        chbObsoleto.Checked = False : chbObsoleto.Enabled = True : chbObsoleto.Visible = False
        chbMostrarHGRecibo.Checked = False : chbMostrarHGRecibo.Enabled = True
        chbMostrarHGSinRecibo.Checked = False : chbMostrarHGSinRecibo.Enabled = True
        btnGuardar.CommandArgument = String.Empty
        pnlMensa.Visible = False
    End Sub

#End Region

#Region "Conceptos"

    ''' <summary>
    ''' Obtiene todos los conceptos
    ''' </summary>
    Private Sub cargarConceptos()
        Try
            mvConceptos.ActiveViewIndex = 0  'Se pone antes porque sino no se traduce
            Dim concepBLL As New BLL.ConceptosBLL
            Dim lConcept As List(Of ELL.Concepto) = concepBLL.loadList(Master.IdPlantaGestion)
            If (lConcept IsNot Nothing) Then Ordenar(lConcept, GridViewSortExpresion, GridViewSortDirection)
            gvConceptos.DataSource = lConcept
            gvConceptos.DataBind()
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al cargar los conceptos", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Al producirse un evento en una fila del listado, se redirige al detalle
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub gvConceptos_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvConceptos.RowCommand
        If (e.CommandName = "Select") Then
            Try
                mostrarDetalle(CInt(e.CommandArgument))
            Catch batzEx As BatzException
                Master.MensajeError = batzEx.Termino
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Se enlazan los datos con el gridview
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Private Sub gvConceptos_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvConceptos.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf e.Row.RowType = DataControlRowType.DataRow Then
            Dim oConcepto As ELL.Concepto = e.Row.DataItem
            CType(e.Row.FindControl("chbRequiereDet"), CheckBox).Checked = oConcepto.RequiereDetalle
            CType(e.Row.FindControl("chbMostrarHGRecibo"), CheckBox).Checked = oConcepto.MostrarHojaGastosRecibo
            CType(e.Row.FindControl("chbMostrarHGSinRecibo"), CheckBox).Checked = oConcepto.MostrarHojaGastosSinRecibo
            If (oConcepto.Obsoleto) Then
                e.Row.CssClass = "danger"
                e.Row.ToolTip = itzultzaileWeb.Itzuli("Concepto obsoleto")
            Else
                e.Row.ToolTip = itzultzaileWeb.Itzuli("Pinchar para ir al detalle")
            End If
            e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gvConceptos, "Select$" + CStr(oConcepto.Id))
        End If
    End Sub

#Region "Orden y paginación"

    ''' <summary>
    ''' Paginacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Private Sub gvConceptos_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvConceptos.PageIndexChanging
        Try
            gvConceptos.PageIndex = e.NewPageIndex
            cargarConceptos()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Evento para ordenar las columnas del resultado de la busqueda
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Protected Sub gvConceptos_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs) Handles gvConceptos.Sorting
        Try
            GridViewSortDirection = If(GridViewSortDirection = SortDirection.Ascending, SortDirection.Descending, SortDirection.Ascending)
            GridViewSortExpresion = If(GridViewSortExpresion Is Nothing, String.Empty, e.SortExpression)
            cargarConceptos()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Indica la direccion en la que hay que ordenar
    ''' </summary>
    ''' <value></value>
    ''' <returns>Orden</returns>
    Public Property GridViewSortDirection() As SortDirection
        Get
            If (ViewState("sortDirection") Is Nothing) Then
                ViewState("sortDirection") = SortDirection.Ascending
            End If
            Return CType(ViewState("sortDirection"), SortDirection)
        End Get
        Set(ByVal value As SortDirection)
            ViewState("sortDirection") = value
        End Set
    End Property

    ''' <summary>
    ''' Indica la expresion de ordenacion
    ''' </summary>
    ''' <value></value>
    ''' <returns>Expresion</returns>
    Public Property GridViewSortExpresion() As String
        Get
            If (ViewState("sortExpresion") Is Nothing) Then
                ViewState("sortExpresion") = "Nombre"
            End If
            Return ViewState("sortExpresion").ToString()
        End Get
        Set(ByVal value As String)
            ViewState("sortExpresion") = value
        End Set
    End Property

    ''' <summary>
    ''' Ordena la lista de conceptos
    ''' </summary>
    ''' <param name="lConceptos">Lista de conceptos</param>
    ''' <param name="sortExpr">Campo por el que hay que ordenar</param>
    ''' <param name="sortDir">Direccion de ordenacion</param>
    Private Sub Ordenar(ByRef lConceptos As List(Of ELL.Concepto), ByVal sortExpr As String, ByVal sortDir As SortDirection)
        Select Case sortExpr
            Case "Nombre"
                If (sortDir = SortDirection.Ascending) Then
                    lConceptos = lConceptos.OrderBy(Of String)(Function(o) o.Nombre.ToLower).ToList
                Else
                    lConceptos = lConceptos.OrderByDescending(Of String)(Function(o) o.Nombre.ToLower).ToList
                End If
        End Select
    End Sub

#End Region

#End Region

#Region "Detalle"

    ''' <summary>
    ''' Muestra el detalle del concepto
    ''' </summary>
    ''' <param name="idConcept"></param>	
    Private Sub mostrarDetalle(ByVal idConcept As Integer)
        Try
            Dim conceptBLL As New BLL.ConceptosBLL
            inicializarDetalle()
            If (idConcept <> Integer.MinValue) Then
                Dim mapeado As Boolean = False
                Dim oConcepto As ELL.Concepto = conceptBLL.loadInfo(idConcept)
                If (oConcepto IsNot Nothing) Then
                    txtNombre.Text = oConcepto.Nombre
                    chbRequiereDet.Checked = oConcepto.RequiereDetalle
                    chbMostrarHGRecibo.Checked = oConcepto.MostrarHojaGastosRecibo
                    chbMostrarHGSinRecibo.Checked = oConcepto.MostrarHojaGastosSinRecibo
                    chbObsoleto.Checked = oConcepto.Obsoleto
                    chbObsoleto.Visible = True
                    If (oConcepto.Id = 0) Then
                        txtNombre.Enabled = False : chbRequiereDet.Enabled = False : chbMostrarHGRecibo.Enabled = False : chbMostrarHGSinRecibo.Enabled = False : chbObsoleto.Enabled = False 'El concepto 0 no se puede borrar
                        pnlMensa.Visible = True
                        lblMensaje.Text = itzultzaileWeb.Itzuli("No se puede modificar porque es el concepto con el que se relacionan por defecto los nuevos provenientes de los ficheros de visas y agencia")
                    End If
                    Dim lRelaciones As List(Of String()) = conceptBLL.loadRelaciones(Master.IdPlantaGestion, idConcept)
                    If (lRelaciones IsNot Nothing AndAlso lRelaciones.Count > 1) Then '>1 porque siempre que se genera un nuevo concepto, se mete en RELACION_CONCEPTOS. Asi que si tiene mas de uno, significara que tiene conceptos relacionados
                        txtNombre.Enabled = False
                        pnlMensa.Visible = True
                        lblMensaje.Text = itzultzaileWeb.Itzuli("No se puede cambiar el nombre del concepto porque esta relacionado")
                    End If
                    btnGuardar.CommandArgument = idConcept
                Else
                    Master.MensajeError = itzultzaileWeb.Itzuli("errCompDetalle")
                End If
            End If
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("errCompDetalle", ex)
        Finally
            mvConceptos.ActiveViewIndex = 1
        End Try
    End Sub

#End Region

#Region "Botones"

    ''' <summary>
    ''' Guarda los cambios de la sala
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Try
            If (Page.IsValid) Then
                If (txtNombre.Text = String.Empty) Then
                    Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Introduzca el datos")
                Else
                    Dim conceptBLL As New BLL.ConceptosBLL
                    Dim oConcepto As New ELL.Concepto With {.IdPlanta = Master.IdPlantaGestion}
                    Dim bNueva As Boolean = (btnGuardar.CommandArgument = String.Empty)
                    If (Not bNueva) Then
                        oConcepto.Id = CInt(btnGuardar.CommandArgument)
                        oConcepto.Obsoleto = chbObsoleto.Checked
                    End If
                    oConcepto.Nombre = txtNombre.Text.Trim
                    oConcepto.RequiereDetalle = chbRequiereDet.Checked
                    oConcepto.MostrarHojaGastosRecibo = chbMostrarHGRecibo.Checked
                    oConcepto.MostrarHojaGastosSinRecibo = chbMostrarHGSinRecibo.Checked
                    conceptBLL.Save(oConcepto)
                    cargarConceptos()
                    If (bNueva) Then
                        log.Info("Se ha registrado un nuevo concepto '" & oConcepto.Nombre & "'")
                    Else
                        If (oConcepto.Obsoleto) Then
                            log.Info("Se ha marcado o ya estaba marcado como obsoleto el concepto '" & oConcepto.Nombre & "'")
                        Else
                            log.Info("Se ha desmarcado como obsoleto o se han realizado cambios el concepto '" & oConcepto.Nombre & "'")
                        End If
                    End If
                    Master.MensajeInfo = itzultzaileWeb.Itzuli("datosGuardados")
                End If
            Else
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Introduzca el dato")
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            log.Error("Error al guardar los datos del concepto con id:" & btnGuardar.CommandArgument, ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("errGuardar")
        End Try
    End Sub

    ''' <summary>
    ''' Vuelve a la vista del listado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnVolver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnVolver.Click
        mvConceptos.ActiveViewIndex = 0
    End Sub

    ''' <summary>
    ''' Prepara el formulario para realizar un nuevo registro
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub lnbNuevo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnbNuevo.Click
        Try
            mostrarDetalle(Integer.MinValue)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

End Class