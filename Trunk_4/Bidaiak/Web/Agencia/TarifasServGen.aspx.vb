Public Class TarifasServGen
    Inherits PageBase

#Region "Page Load"

    ''' <summary>
    ''' Inicializa el mantenimiento
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                cargarListado()
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
    Private Sub BusquedaPersonas_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelListadoServ) : itzultzaileWeb.Itzuli(lnkNuevo) : itzultzaileWeb.Itzuli(labelTitleModal)
            itzultzaileWeb.Itzuli(btnActivar) : itzultzaileWeb.Itzuli(labelDServicio) : itzultzaileWeb.Itzuli(labelConfirmDeleteTitleTarifa)
            itzultzaileWeb.Itzuli(labelNivel) : itzultzaileWeb.Itzuli(btnGuardar) : itzultzaileWeb.Itzuli(btnEliminar)
            itzultzaileWeb.Itzuli(btnVolver) : itzultzaileWeb.Itzuli(lnkTarifaLinea) : itzultzaileWeb.Itzuli(labelConfirmCancelTarifa)
            itzultzaileWeb.Itzuli(labelLServicio) : itzultzaileWeb.Itzuli(labelLAnno) : itzultzaileWeb.Itzuli(labelLTarifa)
            itzultzaileWeb.Itzuli(rfvTarifa) : itzultzaileWeb.Itzuli(btnGuardarLineaM)
            itzultzaileWeb.Itzuli(btnEliminarLineaM) : itzultzaileWeb.Itzuli(lnkReplicar) : itzultzaileWeb.Itzuli(labelTitleModalReplicar)
            itzultzaileWeb.Itzuli(labelInfo) : itzultzaileWeb.Itzuli(btnReplicar) : itzultzaileWeb.Itzuli(labelCancelar)
            itzultzaileWeb.Itzuli(labelConfirmDeleteTitleTarifa) : itzultzaileWeb.Itzuli(labelConfirmCancelTarifa) : itzultzaileWeb.Itzuli(labelCancelarTarifa)
            itzultzaileWeb.Itzuli(btnEliminarModalTarifa) : itzultzaileWeb.Itzuli(labelCancelarLinea) : itzultzaileWeb.Itzuli(labelConfirmCancelLinea)
            itzultzaileWeb.Itzuli(btnEliminarModalLinea)
        End If
    End Sub

#End Region

#Region "Vista Listado"

    ''' <summary>
    ''' Carga el listado con las tarifas
    ''' </summary>
    Private Sub cargarListado()
        Try
            Master.SetTitle = "Tarifas servicios genericas"
            CargarServicios(ddlServicios)
            Dim tarifBLL As New BLL.TarifasServBLL
            mvTarifas.ActiveViewIndex = 0
            Dim lTarifas As List(Of ELL.TarifaServiciosGenericas) = tarifBLL.loadTarifaGenList(New ELL.TarifaServiciosGenericas With {.IdPlanta = Master.IdPlantaGestion, .TipoServicio = CInt(ddlServicios.SelectedValue)}, Master.IdPlantaGestion)
            If (lTarifas IsNot Nothing) Then lTarifas = lTarifas.OrderBy(Of String)(Function(o) o.NombreServicio).ToList
            gvTarifas.DataSource = lTarifas
            gvTarifas.DataBind()
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al cargar el listado", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga el desplegable de servicios
    ''' </summary>
    Private Sub CargarServicios(ByVal drop As DropDownList)
        If (drop.Items.Count = 0) Then
            Dim servBLL As New BLL.ServicioDeAgenciaBLL
            drop.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Todos"), -1))
            For Each iServ As Integer In [Enum].GetValues(GetType(ELL.Presupuesto.Servicio.Tipo_Servicio))
                drop.Items.Add(New ListItem(itzultzaileWeb.Itzuli([Enum].GetName(GetType(ELL.Presupuesto.Servicio.Tipo_Servicio), iServ).Replace("_", " ")), iServ))
            Next
        End If
    End Sub

    ''' <summary>
    ''' Se buscan los resultados
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>  
    Private Sub btnSearch_ServerClick(sender As Object, e As EventArgs) Handles btnSearch.ServerClick
        Try
            cargarListado()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se redirige al detalle de una tarifa
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub gvTarifas_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvTarifas.RowCommand
        Try
            If (e.CommandName = "Select") Then
                mostrarDetalle(e.CommandArgument)
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se enlazan los datos con la tabla
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvTarifas_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvTarifas.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oTarif As ELL.TarifaServiciosGenericas = e.Row.DataItem
            CType(e.Row.FindControl("lblServicio"), Label).Text = oTarif.NombreServicio
            CType(e.Row.FindControl("lblNivel"), Label).Text = itzultzaileWeb.Itzuli([Enum].GetName(GetType(ELL.Viaje.eNivel), oTarif.Nivel).Replace("_", " "))
            If (oTarif.Obsoleta) Then
                e.Row.CssClass = "danger"
                e.Row.ToolTip = itzultzaileWeb.Itzuli("obsoleto")
            End If
            e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gvTarifas, "Select$" + CStr(oTarif.Id))
        End If
    End Sub

    ''' <summary>
    ''' Paginacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvTarifas_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvTarifas.PageIndexChanging
        Try
            gvTarifas.PageIndex = e.NewPageIndex
            cargarListado()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Abre el detalle para registrar una nueva tarifa
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub lnkNuevo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkNuevo.Click
        Try
            mostrarDetalle(Integer.MinValue)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Detalle"

    ''' <summary>
    ''' Muestra el detalle del objeto
    ''' </summary>    
    ''' <param name="id">Id de la tarifa</param>
    Private Sub mostrarDetalle(ByVal id As Integer)
        inicializarDetalle()
        ddlDServicio.SelectedIndex = 0
        If (id <> Integer.MinValue) Then
            Dim tarifBLL As New BLL.TarifasServBLL
            Dim oTarif As ELL.TarifaServiciosGenericas = tarifBLL.loadTarifaGenInfo(id)
            divTarifas.Visible = True
            btnGuardar.CommandArgument = oTarif.Id
            btnEliminar.Visible = True : lnkTarifaLinea.Visible = True
            ddlDServicio.SelectedValue = oTarif.TipoServicio : ddlDServicio.Enabled = False
            ddlNivel.SelectedValue = oTarif.Nivel
            btnActivar.Visible = (oTarif.Obsoleta)
            If (oTarif.LineasTarifa IsNot Nothing) Then oTarif.LineasTarifa = oTarif.LineasTarifa.OrderByDescending(Of Integer)(Function(o) o.Anno).ToList
            gvTarifasAnno.DataSource = oTarif.LineasTarifa
            gvTarifasAnno.DataBind()
        End If
    End Sub

    ''' <summary>
    ''' Inicializa el formulario de detalle
    ''' </summary>    
    Private Sub inicializarDetalle()
        mvTarifas.ActiveViewIndex = 1
        Master.SetTitle = "Detalle tarifa"
        CargarServicios(ddlDServicio)
        ddlDServicio.Enabled = True
        cargarNiveles()
        divTarifas.Visible = False
        gvTarifas.DataSource = Nothing : gvTarifas.DataBind()
        gvTarifasAnno.DataSource = Nothing : gvTarifasAnno.DataBind()
        btnGuardar.CommandArgument = String.Empty
        btnEliminar.Visible = False : lnkTarifaLinea.Visible = False
        btnActivar.Visible = False
        btnEliminar.OnClientClick = "$('#confirmDeleteTarifa').modal('show'); return false;"
    End Sub

    ''' <summary>
    ''' Carga los niveles posibles
    ''' </summary>    
    Private Sub cargarNiveles()
        If (ddlNivel.Items.Count = 0) Then
            ddlNivel.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))
            Dim texto As String
            For Each item As Integer In [Enum].GetValues(GetType(ELL.Viaje.eNivel))
                texto = [Enum].GetName(GetType(ELL.Viaje.eNivel), item).Replace("_", " ")
                ddlNivel.Items.Add(New ListItem(itzultzaileWeb.Itzuli(texto), item))
            Next
        End If
        ddlNivel.SelectedIndex = 0
    End Sub

    ''' <summary>
    ''' Activa de nuevo la tarifa
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnActivar_Click(sender As Object, e As EventArgs) Handles btnActivar.Click
        Try
            Dim tarifBLL As New BLL.TarifasServBLL
            Dim oTarifa As ELL.TarifaServiciosGenericas = tarifBLL.loadTarifaGenInfo(CInt(btnGuardar.CommandArgument))
            oTarifa.Obsoleta = False
            tarifBLL.SaveTarifaGen(oTarifa)
            log.Info("Se ha activado de nuevo la tarifa generica")
            Master.MensajeInfo = itzultzaileWeb.Itzuli("Tarifa activada")
            Volver()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Guarda los datos de la cabecera de la tarifa
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Try
            If (ddlDServicio.SelectedValue = "-1" OrElse CInt(ddlNivel.SelectedValue) <= 0) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Introduzca todos los datos")
            Else
                Dim tarifBLL As New BLL.TarifasServBLL
                Dim oTarifa As New ELL.TarifaServiciosGenericas
                If (btnGuardar.CommandArgument = String.Empty) Then
                    oTarifa.IdPlanta = Master.IdPlantaGestion
                Else
                    oTarifa = tarifBLL.loadTarifaGenInfo(CInt(btnGuardar.CommandArgument))
                End If
                Dim lTarifas As List(Of ELL.TarifaServiciosGenericas) = tarifBLL.loadTarifaGenList(New ELL.TarifaServiciosGenericas With {.IdPlanta = Master.IdPlantaGestion, .TipoServicio = CInt(ddlDServicio.SelectedValue), .Nivel = CInt(ddlNivel.SelectedValue)}, Master.IdPlantaGestion, False)
                If (lTarifas.Count > 0) Then
                    If ((oTarifa.Id = Integer.MinValue) OrElse (oTarifa.Id <> Integer.MinValue AndAlso lTarifas.First.Id <> oTarifa.Id)) Then
                        Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Ya existe una tarifa de ese servicio/nivel")
                        Exit Sub
                    End If
                End If
                oTarifa.TipoServicio = CInt(ddlDServicio.SelectedValue)
                oTarifa.Nivel = CInt(ddlNivel.SelectedValue)
                tarifBLL.SaveTarifaGen(oTarifa)
                log.Info("Se han guardado los datos de la tarifa generica")
                Master.MensajeInfo = itzultzaileWeb.Itzuli("Datos guardados")
                mostrarDetalle(oTarifa.Id)
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Elimina la tarifa
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnEliminarModalTarifa_Click(sender As Object, e As EventArgs) Handles btnEliminarModalTarifa.Click
        Try
            Dim tarifBLL As New BLL.TarifasServBLL
            Dim idTarifa As Integer = CInt(btnGuardar.CommandArgument)
            tarifBLL.DeleteGen(idTarifa)
            log.Info("Se ha borrado la tarifa generica " & idTarifa)
            Master.MensajeInfo = itzultzaileWeb.Itzuli("Elemento borrado")
            Volver()
            ShowModalBoxTarifa(False)
        Catch batzEx As BatzException
            ShowModalBoxTarifa(True)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Vuelve al listado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub bntVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        Try
            Volver()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Vuelve al listado
    ''' </summary>    
    Private Sub Volver()
        mvTarifas.ActiveViewIndex = 0
        cargarListado()
    End Sub

    ''' <summary>
    ''' Se enlazan los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvTarifasAnno_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvTarifasAnno.RowDataBound
        If (e.Row.RowType = DataControlRowType.EmptyDataRow Or e.Row.RowType = DataControlRowType.Header) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oLinea As ELL.TarifaServiciosGenericas.Lineas = e.Row.DataItem
            Dim params As String = oLinea.IdTarifa & "|" & oLinea.Anno.ToString
            e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gvTarifasAnno, "click$" + CStr(params))
        End If
    End Sub

    ''' <summary>
    ''' Se abre un pop up
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvTarifasAnno_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvTarifasAnno.RowCommand
        Try
            If (e.CommandName = "click") Then
                Dim params As String() = e.CommandArgument.ToString.Split("|")
                mostrarDetalleLinea(CInt(params(0)), CInt(params(1)))
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Muestra el detalle de una linea
    ''' </summary>
    ''' <param name="idTarifa">Id de la tarifa</param>    
    ''' <param name="anno">Año</param>
    Private Sub mostrarDetalleLinea(ByVal idTarifa As Integer, ByVal anno As Integer)
        inicializarDetalleLinea()
        Dim tarifBLL As New BLL.TarifasServBLL
        Dim oTarifa As ELL.TarifaServiciosGenericas = tarifBLL.loadTarifaGenInfo(idTarifa)
        btnGuardarLineaM.CommandArgument = idTarifa
        lblLServicio.Text = oTarifa.NombreServicio
        btnEliminarLineaM.Visible = (anno > 0)
        cargarAnnos(anno > 0)
        If (anno > 0) Then 'Es una modificacion            
            btnEliminarLineaM.CommandArgument = anno : btnEliminarLineaM.Visible = True
            Dim oLinea As ELL.TarifaServiciosGenericas.Lineas = oTarifa.LineasTarifa.Find(Function(o As ELL.TarifaServiciosGenericas.Lineas) o.Anno = anno)
            ddlAnno.SelectedValue = oLinea.Anno
            ddlAnno.Enabled = False
            txtTarifa.Text = oLinea.Tarifa
        End If
        ShowModalBoxTarifa(True)
    End Sub

    ''' <summary>
    ''' Muestra el panel modal para cambiar los datos de tarifa
    ''' </summary>
    ''' <param name="action">True lo muestra y false lo oculta</param>
    Private Sub ShowModalBoxTarifa(ByVal action As Boolean)
        If (action) Then
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#divModal').modal('show');", True)
        Else
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#divModal').modal('hide');$('body').removeClass('modal-open');$('.modal-backdrop').hide();", True) '2º intruccion necesaria por estar dentro de un updatePanel
        End If
    End Sub

    ''' <summary>
    ''' Muestra el panel modal para replicar las tarifas
    ''' </summary>
    ''' <param name="action">True lo muestra y false lo oculta</param>
    Private Sub ShowModalBoxReplicar(ByVal action As Boolean)
        If (action) Then
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modalRep", "$('#divModalReplicar').modal('show');", True)
        Else
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modalRep", "$('#divModalReplicar').modal('hide');$('body').removeClass('modal-open');$('.modal-backdrop').hide();", True) '2º intruccion necesaria por estar dentro de un updatePanel
        End If
    End Sub

    ''' <summary>
    ''' Inicializa los controles del detalle de una linea
    ''' </summary>    
    Private Sub inicializarDetalleLinea()
        txtTarifa.Text = "0"
        btnGuardarLineaM.CommandArgument = String.Empty
        ddlAnno.Enabled = True : btnEliminarLineaM.Visible = False
        btnEliminarLineaM.CommandArgument = String.Empty
        btnEliminarLineaM.OnClientClick = "$('#confirmDeleteLinea').modal('show'); return false;"
    End Sub

    ''' <summary>
    ''' Carga los años en los que se puede dar de alta servicios
    ''' Se muestran desde el 2013 hasta el año actual. Si no se muestra desde el 2013, puede que llegara un año que no coincidiera
    ''' con los años a mostrar y fallaria
    ''' Se quitaran aquellos que esten ya registrados
    ''' </summary>    
    Private Sub cargarAnnos(bEsModificacion As Boolean)
        ddlAnno.Items.Clear()
        For anno As Integer = 2019 To Now.Year + 1
            ddlAnno.Items.Add(anno)
        Next
        If (bEsModificacion) Then
            btnGuardarLineaM.Visible = True
        Else
            For Each gvRow As GridViewRow In gvTarifasAnno.Rows
                ddlAnno.Items.Remove(gvRow.Cells(0).Text)
            Next
            'Si es nuevo habra que quitar los años ya registrados
            If (ddlAnno.Items.Count = 0) Then
                ddlAnno.Items.Add(itzultzaileWeb.Itzuli("No se pueden añadir mas años"))
                btnGuardarLineaM.Visible = False
            Else
                btnGuardarLineaM.Visible = True
            End If
        End If
        ddlAnno.SelectedIndex = -1
    End Sub

    ''' <summary>
    ''' Guarda la linea
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnGuardarLineaM_Click(sender As Object, e As EventArgs) Handles btnGuardarLineaM.Click
        Try
            If (txtTarifa.Text = String.Empty) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Introduzca todos los datos")
            Else
                Dim tarifBLL As New BLL.TarifasServBLL
                Dim oLinea As New ELL.TarifaServiciosGenericas.Lineas With {.IdTarifa = CInt(btnGuardarLineaM.CommandArgument), .Tarifa = CDec(txtTarifa.Text), .Anno = CInt(ddlAnno.SelectedValue)}
                tarifBLL.SaveTarifaGenLinea(oLinea, (Not btnEliminarLineaM.Visible))
                log.Info("Se han guardado los datos de la linea de la tarifa generica " & oLinea.IdTarifa)
                Master.MensajeInfo = itzultzaileWeb.Itzuli("Datos guardados")
                mostrarDetalle(oLinea.IdTarifa)
                ShowModalBoxTarifa(False)
            End If
        Catch batzEx As BatzException
            ShowModalBoxTarifa(True)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Elimina la linea
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnEliminarModalLinea_Click(sender As Object, e As EventArgs) Handles btnEliminarModalLinea.Click
        Try
            Dim tarifBLL As New BLL.TarifasServBLL
            Dim idTarifa, anno As Integer
            idTarifa = CInt(btnGuardarLineaM.CommandArgument)
            anno = CInt(ddlAnno.SelectedValue)
            tarifBLL.DeleteLineaGen(idTarifa, anno)
            log.Info("Se ha borrado la linea generica " & btnGuardarLineaM.CommandArgument & " del año " & anno)
            Master.MensajeInfo = itzultzaileWeb.Itzuli("Elemento borrado")
            mostrarDetalle(idTarifa)
            ShowModalBoxTarifa(False)
        Catch batzEx As BatzException
            ShowModalBoxTarifa(True)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Nueva linea de tarifa
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub lnkTarifaLinea_Click(sender As Object, e As EventArgs) Handles lnkTarifaLinea.Click
        Try
            mostrarDetalleLinea(CInt(btnGuardar.CommandArgument), 0)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Replicar"

    ''' <summary>
    ''' Se muestra el panel para replicar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub lnkReplicar_Click(sender As Object, e As EventArgs) Handles lnkReplicar.Click
        cargarAnnos()
        ShowModalBoxReplicar(True)
    End Sub

    ''' <summary>
    ''' Carga el desplegable con los años
    ''' </summary>    
    Private Sub cargarAnnos()
        If (ddlAnnoRep.Items.Count = 0) Then
            If (Now.Year <> 2019) Then ddlAnnoRep.Items.Add(New ListItem(Now.Year - 1, Now.Year - 1))  'Para que empiece en 2019
            ddlAnnoRep.Items.Add(New ListItem(Now.Year, Now.Year))
            ddlAnnoRep.Items.Add(New ListItem(Now.Year + 1, Now.Year + 1))
        End If
        ddlAnnoRep.SelectedValue = Now.Year
    End Sub

    ''' <summary>
    ''' Replica las tarifas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnReplicar_Click(sender As Object, e As EventArgs) Handles btnReplicar.Click
        Try
            Dim solAgenBLL As New BLL.TarifasServBLL
            log.Info("Se van a replicar las tarifas genericas en el año " & ddlAnnoRep.SelectedValue)
            solAgenBLL.ReplicarTarifasGen(CInt(ddlAnnoRep.SelectedValue), Master.IdPlantaGestion)
            Master.MensajeInfo = itzultzaileWeb.Itzuli("Tarifas replicadas")
            ShowModalBoxReplicar(False)
        Catch batzEx As BatzException
            ShowModalBoxReplicar(True)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

End Class