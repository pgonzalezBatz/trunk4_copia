Public Class Comites
    Inherits PageBase

    Private indicadoresComite As List(Of ELL.Indicador)
    Private idAreaPaint As Integer

#Region "Eventos de la pagina"

    ''' <summary>
    ''' Se carga la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                cargarComites(0)
            End If
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("errCargarPagina")
        End Try
    End Sub

    ''' <summary>
    ''' Traducciones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(lnkNuevo) : itzultzaileWeb.Itzuli(lnkEditar) : itzultzaileWeb.Itzuli(labelComite)
            itzultzaileWeb.Itzuli(btnGuardar) : itzultzaileWeb.Itzuli(btnVolver) : itzultzaileWeb.Itzuli(btnEliminar)
            itzultzaileWeb.Itzuli(btnGuardarIndUp) : itzultzaileWeb.Itzuli(btnGuardarIndDown)
        End If
    End Sub

#End Region

#Region "Acciones"

    ''' <summary>
    ''' Carga el desplegable con los comites existentes
    ''' </summary>
    ''' <param name="idComiteLoad">Id del comite a cargar. Si viene a 0, no se seleccionara ninguno</param>
    Private Sub CargarComites(ByVal idComiteLoad As Integer)
        Dim comBLL As New BLL.ComitesComponent
        ddlComite.Items.Clear()
        ddlComite.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Seleccione uno"), 0))
        Dim lComites As List(Of ELL.Comite) = comBLL.loadListComites(New ELL.Comite)
        ddlComite.DataSource = lComites
        ddlComite.DataBind()
        ddlComite.SelectedValue = idComiteLoad
        lnkEditar.Visible = False : pnlIndicadores.Visible = False
        mvComites.ActiveViewIndex = 0
        ddlComite_SelectedIndexChanged(Nothing, Nothing)
    End Sub

    ''' <summary>
    ''' Se cargan todos los indicadores y se visualizan cuales están asignados al comite
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub DdlComite_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlComite.SelectedIndexChanged
        Try
            If (ddlComite.SelectedIndex = 0) Then
                lnkEditar.Visible = False
                pnlIndicadores.Visible = False
            Else
                lnkEditar.Visible = True
                Dim indBLL As New BLL.AreasComponent
                'Se obtienen los indicadores de los negocios de BMS y sistemas
                Dim lIndicadores As List(Of ELL.Indicador) = indBLL.loadListIndicadores(New ELL.Area With {.IdNegocio = 1}) 'Sistemas
                Dim lIndicadoresAux As List(Of ELL.Indicador) = indBLL.loadListIndicadores(New ELL.Area With {.IdNegocio = 5}) 'BMS
                If (lIndicadoresAux.Count > 0) Then lIndicadores.AddRange(lIndicadoresAux)
                lIndicadores = lIndicadores.OrderBy(Function(o) o.IdArea).ThenBy(Function(o) o.NumOrden).ToList
                Dim comitBLL As New BLL.ComitesComponent
                'Se cargan en memoria los valores que luego se utilizaran para cada iteracion del repeater
                '**************************************************************************************
                indicadoresComite = comitBLL.loadIndicadoresComite(CInt(ddlComite.SelectedValue))
                idAreaPaint = 0
                '**************************************************************************************
                pnlIndicadores.Visible = True
                rptIndicadores.DataSource = lIndicadores
                rptIndicadores.DataBind()
            End If
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("errCargarPagina")
        End Try
    End Sub

    ''' <summary>
    ''' Se intenta registrar un nuevo comite
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub LnkNuevo_Click(sender As Object, e As EventArgs) Handles lnkNuevo.Click
        inicializarDetalle()
    End Sub

    ''' <summary>
    ''' Se edita el comite
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub LnkEditar_Click(sender As Object, e As EventArgs) Handles lnkEditar.Click
        mostrarDetalle(CInt(ddlComite.SelectedValue))
    End Sub

    ''' <summary>
    ''' Se pintan los indicadores que podra seleccionar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub RptIndicadores_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rptIndicadores.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim indic As ELL.Indicador = e.Item.DataItem
            Dim lblNegArea As Label = CType(e.Item.FindControl("lblNegArea"), Label)
            Dim chbIndicador As CheckBox = CType(e.Item.FindControl("chbIndicador"), CheckBox)
            Dim trNegArea As HtmlTableRow = CType(e.Item.FindControl("trNegArea"), HtmlTableRow)
            If (indic.IdArea <> idAreaPaint) Then
                Dim areaBLL As New BLL.AreasComponent
                Dim oArea As ELL.Area = areaBLL.loadArea(indic.IdArea)
                trNegArea.Visible = True
                lblNegArea.Text = If(oArea.IdNegocio = 1, itzultzaileWeb.Itzuli("Sistemas"), "BMS") & " / " & oArea.Nombre
                idAreaPaint = indic.IdArea
            Else
                trNegArea.Visible = False
            End If
            chbIndicador.Text = indic.Nombre
            chbIndicador.Attributes.Add("id", indic.Id)
            chbIndicador.Checked = (indicadoresComite.Exists(Function(o) o.Id = indic.Id))
        End If
    End Sub

    ''' <summary>
    ''' Guarda los indicadores asociados al comite
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub BtnGuardarInd_Click(sender As Object, e As EventArgs) Handles btnGuardarIndUp.Click, btnGuardarIndDown.Click
        Try
            Dim comitBLL As New BLL.ComitesComponent
            Dim lIndicadores As New List(Of ELL.Indicador)
            Dim chbIndicador As CheckBox
            'Se recogen los datos de la interfaz
            For Each ind As RepeaterItem In rptIndicadores.Items
                chbIndicador = CType(ind.FindControl("chbIndicador"), CheckBox)
                If (chbIndicador.Checked) Then
                    lIndicadores.Add(New ELL.Indicador With {.Id = CInt(chbIndicador.Attributes.Item("id"))})
                End If
            Next
            comitBLL.SaveIndicadoresComite(CInt(ddlComite.SelectedValue), lIndicadores)
            log.Info("Se han asociado " & lIndicadores.Count & " al comite " & ddlComite.SelectedItem.Text)
            Master.MensajeInfo = itzultzaileWeb.Itzuli("Datos guardados")
            CargarComites(CInt(ddlComite.SelectedValue))
        Catch ex As Exception
            log.Error("No se han podido asociar indicadores al comite " & ddlComite.SelectedItem.Text, ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al guardar")
        End Try
    End Sub

#End Region

#Region "Edicion comite"

    ''' <summary>
    ''' Inicializa los controles del detalle
    ''' </summary>    
    Private Sub InicializarDetalle()
        mvComites.ActiveViewIndex = 1
        txtComite.Text = String.Empty
        btnGuardar.CommandArgument = String.Empty
        btnEliminar.Visible = False
        btnEliminar.OnClientClick = "return confirm('" & itzultzaileWeb.Itzuli("confirmarEliminar") & "');"
    End Sub

    ''' <summary>
    ''' Muestra el detalle del comite
    ''' </summary>
    ''' <param name="id">Id del comite</param>
    Private Sub MostrarDetalle(ByVal id As Integer)
        Try
            InicializarDetalle()
            If (id <> Integer.MinValue) Then
                Dim comitBLL As New BLL.ComitesComponent
                Dim oComite As ELL.Comite = comitBLL.loadComite(id)
                txtComite.Text = oComite.Nombre
                btnGuardar.CommandArgument = oComite.Id
                btnEliminar.Visible = True
            End If
        Catch batzEx As SabLib.BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar el detalle", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se guardan los datos de la causa
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub BtnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Try
            If (txtComite.Text.Trim = String.Empty) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Debe introducir todos los campos")
            Else
                Dim comitBLL As New BLL.ComitesComponent
                Dim oComite As ELL.Comite = Nothing
                If (btnGuardar.CommandArgument <> String.Empty) Then
                    oComite = comitBLL.loadComite(CInt(btnGuardar.CommandArgument))
                Else
                    oComite = New ELL.Comite
                End If
                oComite.Nombre = txtComite.Text.Trim
                comitBLL.SaveComite(oComite)
                If (oComite.Id = Integer.MinValue) Then
                    log.Info("Se ha registrado un nuevo comite " & oComite.Nombre)
                Else
                    log.Info("Se han actualizado los datos del comite (" & oComite.Id & ") " & oComite.Nombre)
                End If
                CargarComites(oComite.Id)
                Master.MensajeInfo = itzultzaileWeb.Itzuli("Datos guardados")
            End If
        Catch batzEx As SabLib.BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se elimina la causa
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub BtnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        Try
            Dim comitBLL As New BLL.ComitesComponent
            Dim idComite As Integer = CInt(btnGuardar.CommandArgument)
            Dim oComit As ELL.Comite = comitBLL.loadComite(idComite)
            oComit.Obsoleto = True
            comitBLL.SaveComite(oComit)
            log.Info("Se ha eliminado el comite " & idComite)
            CargarComites(0)
            Master.MensajeInfo = itzultzaileWeb.Itzuli("Comite eliminado")
        Catch batzEx As SabLib.BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Vuelve al listado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub BtnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        CargarComites(0)
    End Sub

#End Region

End Class