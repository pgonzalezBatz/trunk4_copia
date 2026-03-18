Public Class Actividades
    Inherits PageBase

#Region "Properties"

    ''' <summary>
    ''' Departamentos de la actividad
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Private Property Departamentos As List(Of SabLib.ELL.Departamento)
        Get
            If (ViewState("Dpto") IsNot Nothing) Then
                Return CType(ViewState("Dpto"), List(Of SabLib.ELL.Departamento))
            Else
                Return Nothing
            End If
        End Get
        Set(value As List(Of SabLib.ELL.Departamento))
            ViewState("Dpto") = value
        End Set
    End Property

#End Region

#Region "Page Load"

    ''' <summary>
    ''' Se cargan las actividades existentes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Actividades IRPF"
                inicializar()
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
    Private Sub Actividades_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(rblMostrar) : itzultzaileWeb.Itzuli(lnkNueva) : itzultzaileWeb.Itzuli(labelActividad)
            itzultzaileWeb.Itzuli(rfvActiv) : itzultzaileWeb.Itzuli(chbReqTexto) : itzultzaileWeb.Itzuli(chbObsoleta)
            itzultzaileWeb.Itzuli(btnSaveM) : itzultzaileWeb.Itzuli(lblMensaje) : itzultzaileWeb.Itzuli(chbPaP)
        End If
    End Sub

    ''' <summary>
    ''' Se inicializa la pagina
    ''' </summary>    
    Private Sub inicializar()
        gvActividades.Attributes("CurrentSortField") = "Nombre"
        gvActividades.Attributes("CurrentSortDirection") = SortDirection.Ascending
        txtFilter.Attributes.Add("placeholder", itzultzaileWeb.Itzuli("Filtrar actividades"))
        cargarRadiobutton()
        mostrarActividades()
    End Sub

    ''' <summary>
    ''' Carga los radiobutton con los tipos de exencion
    ''' </summary>    
    Private Sub cargarRadiobutton()
        If (rblMostrar.Items.Count = 0) Then
            rblMostrar.Items.Add(New ListItem("Mostrar todos", Integer.MinValue))
            rblMostrar.Items.Add(New ListItem("Exentas", 1))
            rblMostrar.Items.Add(New ListItem("No exentas", 0))
        End If
        rblMostrar.SelectedValue = Integer.MinValue
    End Sub

#End Region

#Region "Mostrar Actividades"

    ''' <summary>
    ''' Muestra las actividades
    ''' </summary>    
    Private Sub mostrarActividades()
        Try
            Dim activBLL As New BLL.ActividadesBLL
            Dim bExentas As Nullable(Of Boolean) = Nothing
            If (CInt(rblMostrar.SelectedValue) <> Integer.MinValue) Then bExentas = CType(rblMostrar.SelectedValue, Boolean)
            Dim lActiv As List(Of ELL.Actividad) = activBLL.loadList(Master.IdPlantaGestion, bExentas, True, False, txtFilter.Text)
            If (lActiv.Count > 0) Then Ordenar(lActiv)
            gvActividades.DataSource = lActiv
            gvActividades.DataBind()
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al mostrar las actividades", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se introduce una nueva actividad
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub lnkNueva_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkNueva.Click
        Try
            mostrarDetalle(Integer.MinValue)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Busca las actividades que cumplan el filtro
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.ServerClick
        Try
            mostrarActividades()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Muestra el panel modal para introducir la cuenta
    ''' </summary>
    ''' <param name="action">True lo muestra y false lo oculta</param>
    Private Sub ShowModalBox(ByVal action As Boolean)
        If (action) Then
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#divModal').modal('show');", True)
        Else
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#divModal').modal('hide');$('body').removeClass('modal-open');$('.modal-backdrop').hide();", True) '2º intruccion necesaria por estar dentro de un updatePanel
        End If
    End Sub

#Region "Gridview"

    ''' <summary>
    ''' Se muestra el detalle de la actividad seleccionada
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvActividades_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvActividades.RowCommand
        Try
            If (e.CommandName = "Select") Then mostrarDetalle(CInt(e.CommandArgument))
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se enlazan los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvActividades_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvActividades.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oActiv As ELL.Actividad = e.Row.DataItem
            CType(e.Row.FindControl("chbExenta"), CheckBox).Checked = oActiv.ExentaIRPF
            CType(e.Row.FindControl("chbReqTexto"), CheckBox).Checked = oActiv.RequiereTexto
            CType(e.Row.FindControl("chbPaP"), CheckBox).Checked = oActiv.PuestaAPunto
            If (oActiv.Obsoleta) Then e.Row.CssClass = "danger"
            e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gvActividades, "Select$" + CStr(oActiv.Id))
        End If
    End Sub

    ''' <summary>
    ''' Paginacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvActividades_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvActividades.PageIndexChanging
        Try
            gvActividades.PageIndex = e.NewPageIndex
            mostrarActividades()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Ordenacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvActividades_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs) Handles gvActividades.Sorting
        Try
            gvActividades.Attributes("CurrentSortField") = e.SortExpression
            If (gvActividades.Attributes("CurrentSortDirection") Is Nothing) Then
                gvActividades.Attributes("CurrentSortDirection") = SortDirection.Ascending
            Else
                If (gvActividades.Attributes("CurrentSortDirection") = SortDirection.Ascending) Then
                    gvActividades.Attributes("CurrentSortDirection") = SortDirection.Descending
                Else
                    gvActividades.Attributes("CurrentSortDirection") = SortDirection.Ascending
                End If
            End If
            mostrarActividades()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Ordena la lista
    ''' </summary>
    ''' <param name="lista">Lista</param>
    Public Sub Ordenar(ByRef lista As List(Of ELL.Actividad))
        Select Case gvActividades.Attributes("CurrentSortField")
            Case "Nombre"
                If (gvActividades.Attributes("CurrentSortDirection") = SortDirection.Ascending) Then
                    lista = lista.OrderBy(Of String)(Function(o) o.Nombre).ToList
                Else
                    lista = lista.OrderByDescending(Of String)(Function(o) o.Nombre).ToList
                End If
        End Select
    End Sub

#End Region

#End Region

#Region "Mostrar Detalle"

    ''' <summary>
    ''' Muestra el detalle de una actividad
    ''' </summary>    
    ''' <param name="id">Id</param>
    Private Sub mostrarDetalle(ByVal id As Integer)
        Try
            inicializarDetalle()
            If (id <> Integer.MinValue) Then
                Dim activBLL As New BLL.ActividadesBLL
                Dim oActiv As ELL.Actividad = activBLL.loadInfo(id)
                btnSaveM.CommandArgument = id
                txtActividad.Text = oActiv.Nombre
                rblExento.SelectedValue = If(oActiv.ExentaIRPF, 1, 0)
                chbReqTexto.Checked = oActiv.RequiereTexto
                chbPaP.Checked = oActiv.PuestaAPunto
                chbObsoleta.Checked = oActiv.Obsoleta
                chbObsoleta.Visible = True
                If (activBLL.tieneIntegrantesRelacionados(id)) Then
                    pnlMensa.Visible = True
                    rblExento.Enabled = False
                    txtActividad.Enabled = False
                End If
                Dim lDepart As List(Of SabLib.ELL.Departamento) = oActiv.DepartamentosAfectados
                If (oActiv.DepartamentosAfectados Is Nothing OrElse (oActiv.DepartamentosAfectados IsNot Nothing AndAlso oActiv.DepartamentosAfectados.Count = 0)) Then
                    lDepart = New List(Of SabLib.ELL.Departamento)
                    lDepart.Add(New SabLib.ELL.Departamento With {.Id = 0, .IdPlanta = Master.IdPlantaGestion, .Nombre = itzultzaileWeb.Itzuli("Todos")})
                End If
                Departamentos = lDepart
                Departamentos = Departamentos.OrderBy(Of String)(Function(o) o.Nombre).ToList
                gvDepartamentos.DataSource = lDepart
                gvDepartamentos.DataBind()
                'Se quitan del desplegable todos los que esten aqui
                Dim index As Integer
                For Each oDept As SabLib.ELL.Departamento In lDepart
                    index = ddlDepartamentos.Items.IndexOf(ddlDepartamentos.Items.FindByValue(oDept.Id))
                    If (index > -1) Then ddlDepartamentos.Items.RemoveAt(index)
                Next
            End If
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al mostrar el detalle", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Inicializa el detalle
    ''' </summary>    
    Private Sub inicializarDetalle()
        txtActividad.Text = String.Empty
        chbReqTexto.Checked = False : chbObsoleta.Checked = False : chbObsoleta.Visible = False : chbPaP.Checked = False
        btnSaveM.CommandArgument = String.Empty
        cargarRadiobuttonDet()
        cargarDepartamentos()
        Departamentos = New List(Of SabLib.ELL.Departamento)
        Departamentos.Add(New SabLib.ELL.Departamento With {.Id = 0, .IdPlanta = Master.IdPlantaGestion, .Nombre = itzultzaileWeb.Itzuli("Todos")})
        gvDepartamentos.DataSource = Departamentos
        gvDepartamentos.DataBind()
        pnlMensa.Visible = False
        rblExento.Enabled = True : txtActividad.Enabled = True
        ShowModalBox(True)
    End Sub

    ''' <summary>
    ''' Carga los radiobutton con los tipos de exencion
    ''' </summary>    
    Private Sub cargarRadiobuttonDet()
        If (rblExento.Items.Count = 0) Then
            rblExento.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Exenta"), 1))
            rblExento.Items.Add(New ListItem(itzultzaileWeb.Itzuli("No exenta"), 0))
        End If
        rblExento.SelectedIndex = -1
    End Sub

    ''' <summary>
    ''' Carga la lista de departamentos. Dependeran de la planta
    ''' </summary>    
    Private Sub cargarDepartamentos()
        Dim lDepart As List(Of SabLib.ELL.Departamento)
        Dim deptBLL As New SabLib.BLL.DepartamentosComponent
        ddlDepartamentos.Items.Clear()
        lDepart = deptBLL.GetDepartamentos(SabLib.BLL.Interface.IDepartamentosComponent.EDepartamentos.Activos, Master.IdPlantaGestion)
        lDepart = lDepart.OrderBy(Of String)(Function(o) o.Nombre).ToList
        lDepart.Insert(0, New SabLib.ELL.Departamento With {.Id = 0, .IdPlanta = Master.IdPlantaGestion, .Nombre = itzultzaileWeb.Itzuli("Todos")})
        For Each oDpto As SabLib.ELL.Departamento In lDepart
            ddlDepartamentos.Items.Add(New ListItem(oDpto.Nombre, oDpto.Id))
        Next
        ddlDepartamentos.SelectedIndex = -1
    End Sub

    ''' <summary>
    ''' Se enlazan los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvDepartamentos_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvDepartamentos.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        End If
    End Sub

    ''' <summary>
    ''' Se agrega al gridview el elemento seleccionado y se quita del desplegable
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub lnkAddDpto_Click(sender As Object, e As EventArgs) Handles lnkAddDpto.Click
        Try
            If (CInt(ddlDepartamentos.SelectedValue) = "0") Then  'Se añaden todos
                'Primero se pasan al desplegable todos los del gridview
                For Each oDept As SabLib.ELL.Departamento In Departamentos
                    ddlDepartamentos.Items.Add(New ListItem(oDept.Nombre, oDept.Id))
                Next
                Departamentos.Clear()  'Se quitan todos de la lista
            Else
                If (Departamentos IsNot Nothing) Then  'Se comprueba si el todos esta en la lista
                    Dim oDept As SabLib.ELL.Departamento = Departamentos.Find(Function(o As SabLib.ELL.Departamento) o.Id = 0)
                    If (oDept IsNot Nothing) Then
                        ddlDepartamentos.Items.Insert(0, New ListItem(oDept.Nombre, oDept.Id))
                        Departamentos.Remove(oDept)
                    End If
                End If
            End If
            Departamentos.Add(New SabLib.ELL.Departamento With {.Id = ddlDepartamentos.SelectedValue, .Nombre = ddlDepartamentos.SelectedItem.Text})
            ddlDepartamentos.Items.RemoveAt(ddlDepartamentos.SelectedIndex)  'Se quita el elemento del desplegable
            gvDepartamentos.DataSource = Departamentos
            gvDepartamentos.DataBind()
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al quitar el departamento")
        End Try
        ShowModalBox(True)
    End Sub

    ''' <summary>
    ''' Se quita el departamento seleccionado y se añade al desplegable
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub lnkElim_Click(sender As Object, e As EventArgs)
        Try
            If (Departamentos.Count = 1) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("No puede quitar todos los departamentos")
            Else
                Dim lnk As LinkButton = CType(sender, LinkButton)
                Dim deptoBLL As New SabLib.BLL.DepartamentosComponent
                If (CInt(ddlDepartamentos.SelectedValue) = 0) Then
                    ddlDepartamentos.Items.Insert(1, New ListItem(lnk.CommandName, CInt(lnk.CommandArgument)))
                Else
                    ddlDepartamentos.Items.Add(New ListItem(lnk.CommandName, CInt(lnk.CommandArgument)))
                End If
                Departamentos.Remove(Departamentos.Find(Function(o As SabLib.ELL.Departamento) o.Id = (lnk.CommandArgument)))
                gvDepartamentos.DataSource = Departamentos
                gvDepartamentos.DataBind()
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al quitar el departamento")
        End Try
        ShowModalBox(True)
    End Sub

    ''' <summary>
    ''' Guarda los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnSaveM_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveM.Click
        Try
            If (txtActividad.Text.Trim = String.Empty Or rblExento.SelectedIndex = -1) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Introduzca todos los datos")
                ShowModalBox(True)
            Else
                Dim cambios As String = String.Empty
                Dim activBLL As New BLL.ActividadesBLL
                Dim oActiv As New ELL.Actividad With {.IdPlanta = Master.IdPlantaGestion, .Nombre = txtActividad.Text.Trim, .ExentaIRPF = rblExento.SelectedValue, .RequiereTexto = chbReqTexto.Checked,
                    .PuestaAPunto = chbPaP.Checked, .Obsoleta = chbObsoleta.Checked}
                If (btnSaveM.CommandArgument <> String.Empty) Then oActiv.Id = CInt(btnSaveM.CommandArgument)
                If (Departamentos IsNot Nothing AndAlso Departamentos.Count = 1 AndAlso Departamentos.First.Id = 0) Then Departamentos = Nothing 'Si solo esta todos, se quita
                oActiv.DepartamentosAfectados = Departamentos
                If (oActiv.Id <> Integer.MinValue) Then cambios = VerCambios(oActiv)
                activBLL.Save(oActiv)
                Master.MensajeInfo = itzultzaileWeb.Itzuli("datosGuardados")
                ShowModalBox(False)
                If (btnSaveM.CommandArgument = String.Empty) Then
                    log.Info("ACTIVIDADES_IRPF:Se ha registrado un nueva actividad '" & oActiv.Nombre & "'")
                Else
                    log.Info("ACTIVIDADES_IRPF:Se han modificado los datos de la actividad (" & oActiv.Id & "):" & If(cambios = String.Empty, "Sin cambios", cambios))
                End If
                Try
                    mostrarActividades()
                Catch ex As Exception
                    Master.MensajeError = itzultzaileWeb.Itzuli("Datos guardados pero error al mostrar el listado")
                End Try
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
            ShowModalBox(True)
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("errorGuardar")
            ShowModalBox(True)
        End Try
    End Sub

    ''' <summary>
    ''' Mira los cambios que se han producido
    ''' </summary>
    ''' <param name="oActivNew">Actividad nueva</param>
    ''' <returns></returns>    
    Private Function VerCambios(ByVal oActivNew As ELL.Actividad) As String
        Dim cambios As String = String.Empty
        Try
            Dim activBLL As New BLL.ActividadesBLL
            Dim oActivOld As ELL.Actividad = activBLL.loadInfo(oActivNew.Id)
            If (oActivNew.Nombre <> oActivOld.Nombre) Then cambios &= If(cambios <> String.Empty, ",", String.Empty) & "nombre"
            If (oActivNew.ExentaIRPF <> oActivOld.ExentaIRPF) Then
                cambios &= If(cambios <> String.Empty, ",", String.Empty)
                If (oActivNew.ExentaIRPF) Then
                    cambios &= "Se ha marcado como exenta"
                Else
                    cambios &= "Se ha marcado como no exenta"
                End If
            End If
            If (oActivNew.RequiereTexto <> oActivOld.RequiereTexto) Then
                cambios &= If(cambios <> String.Empty, ",", String.Empty)
                If (oActivNew.RequiereTexto) Then
                    cambios &= "Se ha marcado para que requiera texto"
                Else
                    cambios &= "Se ha marcado para que no requiera texto"
                End If
            End If
            If (oActivNew.PuestaAPunto <> oActivOld.PuestaAPunto) Then
                cambios &= If(cambios <> String.Empty, ",", String.Empty)
                If (oActivNew.PuestaAPunto) Then
                    cambios &= "Se ha marcado puesta a punto"
                Else
                    cambios &= "Se ha desmarcado puesta a punto"
                End If
            End If
            If (oActivNew.Obsoleta <> oActivOld.Obsoleta) Then
                cambios &= If(cambios <> String.Empty, ",", String.Empty)
                If (oActivNew.Obsoleta) Then
                    cambios &= "Se ha marcado como obsoleta"
                Else
                    cambios &= "Se ha desmarcado de obsoleta"
                End If
            End If
        Catch ex As Exception
            log.Error("Error al ver los cambios producidos en las actividad", ex)
            cambios = "Error, no se ha podido consultar el objeto antiguo"
        End Try
        Return cambios
    End Function

#End Region

End Class