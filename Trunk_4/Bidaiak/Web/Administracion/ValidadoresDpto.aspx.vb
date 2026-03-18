Public Class ValidadoresDpto
    Inherits PageBase

#Region "Page Load"

    ''' <summary>
    ''' Se inicializa la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Try
                Master.SetTitle = "Validador por departamento"
                inicializar()
            Catch batzEx As BatzException
                Master.MensajeError = batzEx.Termino
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelInfo) : itzultzaileWeb.Itzuli(labelDpto) : itzultzaileWeb.Itzuli(labelVal)
            itzultzaileWeb.Itzuli(btnAnadir) : itzultzaileWeb.Itzuli(labelConfirmDeleteTitle) : itzultzaileWeb.Itzuli(labelConfirmMessage)
            itzultzaileWeb.Itzuli(labelCancelar)
        End If
    End Sub

    ''' <summary>
    ''' Inicializa los controles
    ''' </summary>	
    Private Sub inicializar()
        searchUser.limpiar()
        cargarDepartamentos()
        cargarDepartamentosValidador()
    End Sub

    ''' <summary>
    ''' Carga los departamentos existentes
    ''' </summary>    
    Private Sub cargarDepartamentos()
        Try
            ddlDpto.Items.Clear()
            ddlDpto.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), String.Empty))
            Dim deptBLL As New SabLib.BLL.DepartamentosComponent
            Dim lDept As List(Of SabLib.ELL.Departamento) = deptBLL.GetDepartamentos(SabLib.BLL.Interface.IDepartamentosComponent.EDepartamentos.Activos, Master.IdPlantaGestion)
            If (lDept IsNot Nothing) Then lDept = lDept.OrderBy(Of String)(Function(o) o.Nombre).ToList
            ddlDpto.DataSource = lDept
            ddlDpto.DataTextField = SabLib.ELL.Departamento.COLUMN_NAME_NOMBRE
            ddlDpto.DataValueField = SabLib.ELL.Departamento.COLUMN_NAME_ID
            ddlDpto.DataBind()
        Catch ex As Exception
            Throw New BatzException("Error al cargar los departamentos", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga los departamentos asignados a un validador
    ''' </summary>    
    Private Sub cargarDepartamentosValidador()
        Dim bidaiakBLL As New BLL.BidaiakBLL
        gvDptos.DataSource = bidaiakBLL.getValidadoresDptos(Master.IdPlantaGestion)
        gvDptos.DataBind()
    End Sub

#End Region

#Region "Gridview"

    ''' <summary>
    ''' Se enlazan los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvDptos_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvDptos.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header OrElse e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Try
                Dim item As String() = e.Row.DataItem
                Dim deptoBLL As New SabLib.BLL.DepartamentosComponent
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                Dim lnk As LinkButton = CType(e.Row.FindControl("lnkElim"), LinkButton)
                CType(e.Row.FindControl("lblDepartamento"), Label).Text = item(0) & " - " & deptoBLL.GetDepartamento(New SabLib.ELL.Departamento With {.Id = item(0), .IdPlanta = Master.IdPlantaGestion}).Nombre
                CType(e.Row.FindControl("lblNombreCompleto"), Label).Text = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = CInt(item(1))}, False).NombreCompleto
                lnk.ToolTip = itzultzaileWeb.Itzuli("Eliminar")
                lnk.Attributes.Add("href", "#confirmDelete")
                lnk.OnClientClick = "document.getElementById('" & hfIdDptoDelete.ClientID & "').value='" & item(0) & "';"
            Catch batzEx As BatzException
                Master.MensajeError = batzEx.Termino
            Catch ex As Exception
                Master.MensajeError = itzultzaileWeb.Itzuli("Error al mostrar los datos del listado")
            End Try
        End If
    End Sub

#End Region

#Region "Botones"

    ''' <summary>
    ''' Añade un registro nuevo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnAnadir_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAnadir.Click
        Try
            If (ddlDpto.SelectedValue <> String.Empty AndAlso searchUser.SelectedId <> String.Empty) Then
                Dim bidaiakBLL As New BLL.BidaiakBLL
                If (bidaiakBLL.getValidadorDpto(ddlDpto.SelectedValue, Master.IdPlantaGestion) = 0) Then
                    bidaiakBLL.addValidadorDpto(ddlDpto.SelectedValue, searchUser.SelectedId, Master.IdPlantaGestion)
                    Master.MensajeInfo = itzultzaileWeb.Itzuli("datosGuardados")
                    log.Info("Se ha añadido un departamento de validacion: Depto=> (" & ddlDpto.SelectedValue & ") " & ddlDpto.SelectedItem.Text.Trim & " | Validador=> (" & searchUser.SelectedId & ") " & searchUser.Texto)
                    inicializar()
                Else
                    Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("El departamento seleccionado ya ha sido añadido")
                End If
            Else
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("debeRellenarDatos")
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            log.Error("Error al añadir el validador departamento", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Elimina el registro
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnEliminarModal_Click(sender As Object, e As EventArgs) Handles btnEliminarModal.Click
        Try
            Dim bidaiakBLL As New BLL.BidaiakBLL
            Dim idDpto As String = hfIdDptoDelete.Value
            bidaiakBLL.deleteValidadorDpto(idDpto, Master.IdPlantaGestion)
            Master.MensajeInfo = itzultzaileWeb.Itzuli("Registro borrado")
            log.Info("Se ha borrado un departamento de validacion: Depto=> " & idDpto)
            inicializar()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

End Class