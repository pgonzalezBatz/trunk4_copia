Public Class Departamentos
    Inherits PageBase

    Dim oDepartamentos As New BLL.DepartamentosBLL

#Region "Propiedades"

    'Protected Overrides ReadOnly Property RolesAcceso As System.Collections.Generic.List(Of ELL.Roles.RolUsuario)
    '    Get
    '        Dim roles As New List(Of ELL.Roles.RolUsuario)
    '        roles.Add(ELL.Roles.RolUsuario.Administrador)

    '        Return roles
    '    End Get
    'End Property

#End Region

#Region "Miembros"

    Private m_ViewMode As ViewMode = ViewMode.GridView

#End Region

#Region "Propiedades"

    ''' <summary>
    ''' Permite al usuario introducir nuevos registros
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property AllowRecordInserting As Boolean
        Get
            Return dvDepartamento.AutoGenerateInsertButton
        End Get
    End Property

#End Region

#Region "Enumerados"

    Protected Enum ViewMode
        Unknown
        GridView
        DetailsView
    End Enum

#End Region

#Region "Page Load"

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Departamentos_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            ' Forzamos la traducción del details view ya que de inicio está visible = false y no se estaba traduciendo
            ItzultzaileWeb.Itzuli(Me.dvDepartamento)
        End If
    End Sub

    ''' <summary>
    ''' Muestra las colas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Try
                'ComprobarAcceso()
            Catch ex As Exception
                Response.Redirect(PageBase.PAG_PERMISODENEGADO)
            End Try

            If Not Page.IsPostBack Then
                Initialize()
            End If
           
            Page.MaintainScrollPositionOnPostBack = True
        Catch ex As Exception
           Global_asax.log.Error("Error al cargar la página", ex)
        End Try
    End Sub

#End Region

#Region "Métodos"

    ''' <summary>
    ''' Comprueba que el perfil tenga acceso a la página
    ''' </summary>
    Private Sub ComprobarAcceso()
        If (Session("Ticket") Is Nothing AndAlso Session("PerfilUsuario") Is Nothing) Then
            Response.Redirect(PageBase.PAG_PERMISODENEGADO)
        End If
    End Sub

    ' ''' <summary>
    ' ''' Comprueba que el perfil tenga acceso a la página
    ' ''' </summary>
    'Private Sub ComprobarAcceso()
    '    ' Hay que comprobar que alguno de los roles del usuario está contenido en la lista de roles de acceso de la pagina
    '    Dim tieneAcceso As Boolean = ExisteRolEnPagina(PerfilUsuario.RolesUsuario)

    '    ' El administrador puede entrar a todas la páginas aunque no se haya definido su rol explicitamente en cada página
    '    If ((RolesAcceso Is Nothing OrElse Not tieneAcceso) AndAlso
    '         Not PerfilUsuario.RolesUsuario.Exists(Function(f) f.IdRol = ELL.Roles.RolUsuario.Administrador)) Then
    '        Dim segmentos As String() = Page.Request.Url.Segments
    '        Dim pag As String = segmentos(Page.Request.Url.Segments.GetUpperBound(0))
    '        'WriteLog("Se ha intentado acceder a una ruta que su perfil no lo permite. Pag(" & pag & ")", TipoLog.Warn, Nothing)
    '        Response.Redirect("~/PermisoDenegado.aspx", False)
    '    End If
    'End Sub

    ' ''' <summary>
    ' ''' Comprueba que alguno de los roles del usuario está contenido en la lista de roles de acceso de la pagina
    ' ''' </summary>
    ' ''' <param name="listaRolesUsuario"></param>
    ' ''' <returns>True si existe alguno. False en caso contrario</returns>
    ' ''' <remarks></remarks>
    'Private Function ExisteRolEnPagina(ByVal listaRolesUsuario As List(Of ELL.UsuarioRol)) As Boolean
    '    Dim idRol As Integer = Integer.MinValue
    '    Dim existe As Boolean = False
    '    If (RolesAcceso IsNot Nothing) Then
    '        For Each usuarioRol As ELL.UsuarioRol In listaRolesUsuario
    '            idRol = usuarioRol.IdRol
    '            existe = RolesAcceso.Exists(Function(f) f = [Enum].Parse(GetType(ELL.Roles.RolUsuario), idRol.ToString()))
    '            If (existe) Then
    '                Return existe
    '            End If
    '        Next
    '    End If

    '    Return existe
    'End Function

    ''' <summary>
    ''' Inicializa las variables y vincula los datos
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Initialize()
        ClearDataViews()
        BindDataViews()
    End Sub

    ''' <summary>
    ''' Establece el modo de funcionamiento del grid y el details
    ''' </summary>
    ''' <param name="vmNewViewMode"></param>
    ''' <remarks></remarks>
    Protected Sub SetBehavior(ByVal vmNewViewMode As ViewMode)
        m_ViewMode = vmNewViewMode
        SetBehavior()
    End Sub

    ''' <summary>
    ''' Gestiona el modo de funcionamiento del grid y el details
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub SetBehavior()
        Dim bHasGridRows As Boolean = (gvDepartamentos.Rows.Count > 0)
        If ((bHasGridRows = False) AndAlso (Me.AllowRecordInserting = True)) Then
            m_ViewMode = ViewMode.DetailsView
        End If

        Select Case m_ViewMode
            Case ViewMode.Unknown
                pnlGridView.Visible = bHasGridRows
                pnlDetailsView.Visible = Not bHasGridRows
            Case ViewMode.GridView
                pnlGridView.Visible = True
                pnlDetailsView.Visible = False
            Case ViewMode.DetailsView
                pnlGridView.Visible = False
                pnlDetailsView.Visible = True
        End Select

        lbtnVolver.Visible = bHasGridRows
    End Sub

    ''' <summary>
    ''' Limpia los datos vinculados
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ClearDataViews()
        gvDepartamentos.DataSource = Nothing
        gvDepartamentos.DataBind()

        dvDepartamento.DataSource = Nothing
        dvDepartamento.DataBind()
    End Sub

    ''' <summary>
    ''' Vincula los datos
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub BindDataViews()
        BindPlantas()
        BindDataViews(0)
    End Sub

    ''' <summary>
    ''' Carga las plantas existentes de Batz
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindPlantas()
        Dim listaPlantas As List(Of Sablib.ELL.Planta)
        Dim oPlantas As New Sablib.BLL.PlantasComponent
        listaPlantas = oPlantas.GetPlantas(True)
        ddlPlantas.DataSource = listaPlantas
        ddlPlantas.DataBind()
    End Sub

    ''' <summary>
    ''' Vincula los datos
    ''' </summary>
    ''' <param name="idDepartamento">identificador del departamento</param>
    ''' <remarks></remarks>
    Protected Sub BindDataViews(ByVal idDepartamento As Integer)
        Dim listaDepartamentos As List(Of ELL.Departamentos) = Nothing
        If (idDepartamento > 0) Then
            listaDepartamentos = New List(Of ELL.Departamentos)
            listaDepartamentos.Add(oDepartamentos.CargarDepartamento(idDepartamento))
            dvDepartamento.DataSource = listaDepartamentos
            dvDepartamento.DataBind()

            Dim rbl As RadioButtonList = CType(dvDepartamento.FindControl("rblEditTipo"), RadioButtonList)
            If (rbl IsNot Nothing) Then
                If (listaDepartamentos(0).DptoEsCalidad) Then
                    rbl.Items(0).Selected = True
                ElseIf (listaDepartamentos(0).DptoEsOperario) Then
                    rbl.Items(1).Selected = True
                End If
            End If
            SetBehavior(ViewMode.DetailsView)
        Else
            listaDepartamentos = oDepartamentos.CargarDepartamentos(ddlPlantas.SelectedValue())


            If (listaDepartamentos.Count > 0) Then
                gvDepartamentos.DataSource = listaDepartamentos
                gvDepartamentos.DataBind()
            Else
                If (Me.AllowRecordInserting) Then
                    dvDepartamento.DataSource = listaDepartamentos
                    dvDepartamento.DataBind()
                Else
                    gvDepartamentos.DataSource = Nothing
                    gvDepartamentos.DataBind()
                End If
            End If

            SetBehavior()
        End If
    End Sub

    ''' <summary>
    ''' Se borran los valores de los campos del details
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BorrarCampos()
        Dim txt As TextBox = CType(dvDepartamento.FindControl("txtInsertRutaAcceso"), TextBox)

        If (txt IsNot Nothing) Then
            txt.Text = String.Empty
        End If

        Dim ddl As DropDownList = CType(dvDepartamento.FindControl("ddlInsertDepartamento"), DropDownList)
        If (ddl IsNot Nothing) Then
            ddl.SelectedIndex = 0
        End If

        Dim rbl As RadioButtonList = CType(dvDepartamento.FindControl("rblInsertTipo"), RadioButtonList)
        If (rbl IsNot Nothing) Then
            rbl.SelectedIndex = 0
        End If
    End Sub

    ''' <summary>
    ''' Devuelve la clave primaria del grid de fila
    ''' </summary>
    ''' <param name="row"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetGridViewRowId(ByVal row As GridViewRow) As Integer
        Dim id As Integer = Integer.MinValue
        Dim _ctl As Label = CType(row.FindControl("lblId"), Label)
        If (_ctl IsNot Nothing) Then
            id = CInt(_ctl.Text)
        End If

        Return id
    End Function

    ''' <summary>
    ''' Devuelve la clave primaria del details
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetDetailsViewId() As Integer
        Dim id As Integer = Integer.MinValue
        Dim _ctl As Label = CType(dvDepartamento.FindControl("lblId"), Label)
        If (_ctl IsNot Nothing) Then
            id = CInt(_ctl.Text)
        End If

        Return id
    End Function

    ''' <summary>
    ''' Devuelve el valor del control de la fila del details
    ''' </summary>
    ''' <param name="sControlName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetTextFieldValue(ByVal sControlName As String) As String
        Dim sFieldValue As String = String.Empty
        Dim _ctl As TextBox = CType(dvDepartamento.FindControl(sControlName), TextBox)
        If (_ctl IsNot Nothing) Then
            sFieldValue = _ctl.Text.Trim()
        End If

        Return sFieldValue
    End Function

    ''' <summary>
    ''' Devuelve el valor del control de la fila del details
    ''' </summary>
    ''' <param name="sControlName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDropDownListFieldValue(ByVal sControlName As String) As String
        Dim sFieldValue As String = String.Empty
        Dim _ctl As DropDownList = CType(dvDepartamento.FindControl(sControlName), DropDownList)
        If (_ctl IsNot Nothing) Then
            sFieldValue = _ctl.SelectedValue
        End If

        Return sFieldValue
    End Function

    ''' <summary>
    ''' Devuelve el valor del control de la fila del details
    ''' </summary>
    ''' <param name="sControlName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetRadioButtonListFieldValue(ByVal sControlName As String) As String
        Dim sFieldValue As String = String.Empty
        Dim _ctl As RadioButtonList = CType(dvDepartamento.FindControl(sControlName), RadioButtonList)
        If (_ctl IsNot Nothing) Then
            sFieldValue = _ctl.SelectedValue
        End If

        Return sFieldValue
    End Function

    ''' <summary>
    ''' Devuelve el valor del control de la fila del details
    ''' </summary>
    ''' <param name="sControlName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDropDownListFieldText(ByVal sControlName As String) As String
        Dim sFieldValue As String = String.Empty
        Dim _ctl As DropDownList = CType(dvDepartamento.FindControl(sControlName), DropDownList)
        If (_ctl IsNot Nothing) Then
            sFieldValue = _ctl.SelectedItem.Text.Trim
        End If

        Return sFieldValue
    End Function

    ''' <summary>
    ''' Devuelve el valor del control de la fila del details
    ''' </summary>
    ''' <param name="sControlName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetHiddenFieldValue(ByVal sControlName As String) As String
        Dim sFieldValue As String = String.Empty
        Dim _ctl As HiddenField = CType(dvDepartamento.FindControl(sControlName), HiddenField)
        If (_ctl IsNot Nothing) Then
            sFieldValue = _ctl.Value
        End If

        Return sFieldValue
    End Function

#End Region

#Region "Handlers"

    ''' <summary>
    ''' Cambia a modo inserción en el details
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lbtnAgregarNuevoDepartamento_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnAgregarNuevoDepartamento.Click
        Dim listaDepartamentos As List(Of Sablib.ELL.Departamento) = Nothing
        Dim dep As New Sablib.BLL.DepartamentosComponent

        Dim ddlDepartamento As DropDownList = CType(dvDepartamento.FindControl("ddlInsertDepartamento"), DropDownList)

        'Primero llenamos el combo de los departamentos activos
        listaDepartamentos = dep.GetDepartamentos(Sablib.BLL.Interface.IDepartamentosComponent.EDepartamentos.Activos, ddlPlantas.SelectedValue())

        If (ddlDepartamento IsNot Nothing) Then
            ddlDepartamento.DataSource = listaDepartamentos
            ddlDepartamento.DataBind()
            ddlDepartamento.SelectedIndex = 0
        End If

        dvDepartamento.ChangeMode(DetailsViewMode.Insert)
        SetBehavior(ViewMode.DetailsView)

        'Borramos los campos
        BorrarCampos()
    End Sub

    ''' <summary>
    ''' Vincula datos al details
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub dvDepartamento_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles dvDepartamento.DataBound
        If (dvDepartamento.DataItem IsNot Nothing) Then
            Dim departamento As ELL.Departamentos = CType(dvDepartamento.DataItem, ELL.Departamentos)

            Dim ddlDepartamento As DropDownList = CType(dvDepartamento.FindControl("ddlEditDepartamento"), DropDownList)

            If (ddlDepartamento IsNot Nothing) Then
                ddlDepartamento.DataSource = oDepartamentos.CargarDepartamentos(ddlPlantas.SelectedValue())
                ddlDepartamento.DataBind()
                ddlDepartamento.SelectedValue = departamento.Id
            End If
        End If
    End Sub

    ''' <summary>
    ''' Vuelve al modo grid
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lbtnVolver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnVolver.Click
        Me.dvDepartamento.ChangeMode(DetailsViewMode.Insert)
        BindDataViews()
        SetBehavior(ViewMode.GridView)
    End Sub

    ''' <summary>
    ''' Cambio de página en el grid
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvDepartamentos_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvDepartamentos.PageIndexChanging
        gvDepartamentos.PageIndex = e.NewPageIndex
        BindDataViews()
    End Sub


    ''' <summary>
    ''' Editando la fila en el grid
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvDepartamentos_RowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs) Handles gvDepartamentos.RowEditing
        dvDepartamento.ChangeMode(DetailsViewMode.Edit)
        Dim idDepartamento As Integer = GetGridViewRowId(gvDepartamentos.Rows(e.NewEditIndex))
        BindDataViews(idDepartamento)
        SetBehavior(ViewMode.DetailsView)
    End Sub

    ''' <summary>
    ''' Insertando un nuevo registro en el details
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub dvDepartamento_ItemInserting(ByVal sender As Object, ByVal e As DetailsViewInsertEventArgs) Handles dvDepartamento.ItemInserting
        Dim correcto As Boolean = False

        Try
            Dim departamento As New ELL.Departamentos()

            departamento.Nombre = GetDropDownListFieldText("ddlInsertDepartamento")
            departamento.CodigoDpto = GetDropDownListFieldValue("ddlInsertDepartamento")
            departamento.RutaAcceso = GetTextFieldValue("txtInsertRutaAcceso")
            Dim tipoDepartamento As String = GetRadioButtonListFieldValue("rblInsertTipo")
            If (tipoDepartamento.Equals("0")) Then
                departamento.DptoEsCalidad = True
                departamento.DptoEsOperario = False
            ElseIf (tipoDepartamento.Equals("1")) Then
                departamento.DptoEsCalidad = False
                departamento.DptoEsOperario = True
            End If
            departamento.IdPlanta = ddlPlantas.SelectedValue()


            If Not (oDepartamentos.existeDepartamento(departamento.CodigoDpto)) Then
                If (oDepartamentos.GuardarDepartamento(departamento)) Then
                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("El departamento se ha añadido correctamente")
                    dvDepartamento.ChangeMode(DetailsViewMode.Insert)
                    SetBehavior(ViewMode.GridView)
                    BindDataViews()
                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Se ha producido un error al añadir el departamento")
                End If

            Else
                Master.MensajeError = ItzultzaileWeb.Itzuli("El departamento ya existe")
            End If
        Catch ex As Exception
           Global_asax.log.Error("Se ha producido un error al añadir el departamento", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Actualizando un registro en el details
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub dvUsuarios_ItemUpdating(ByVal sender As Object, ByVal e As DetailsViewUpdateEventArgs) Handles dvDepartamento.ItemUpdating

        Dim departamento As New ELL.Departamentos With
            {.Id = GetDetailsViewId(),
             .RutaAcceso = GetTextFieldValue("txtEditRutaAcceso")}
        Dim tipoDepartamento As String = GetRadioButtonListFieldValue("rblInsertTipo")
        If (tipoDepartamento.Equals("0")) Then
            departamento.DptoEsCalidad = True
            departamento.DptoEsOperario = False
        ElseIf (tipoDepartamento.Equals("1")) Then
            departamento.DptoEsCalidad = False
            departamento.DptoEsOperario = True
        End If

        Try
            If (oDepartamentos.ModificarDepartamento(departamento)) Then
                Master.MensajeInfo = ItzultzaileWeb.Itzuli("El departamento se ha guardado correctamente")
            Else
                Master.MensajeError = ItzultzaileWeb.Itzuli("Se ha producido un error al guardar el departamento")
            End If
            BindDataViews()
            dvDepartamento.ChangeMode(DetailsViewMode.Insert)
            SetBehavior(ViewMode.GridView)
        Catch ex As Exception
           Global_asax.log.Error("Se ha producido un error al guardar el departamento", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Cambio de modo del details
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub dvDepartamento_ModeChanging(ByVal sender As Object, ByVal e As DetailsViewModeEventArgs) Handles dvDepartamento.ModeChanging
        If (e.CancelingEdit = False) Then
            dvDepartamento.ChangeMode(e.NewMode)
            Dim id As Integer = GetGridViewRowId(gvDepartamentos.SelectedRow)
            BindDataViews(id)
        Else
            dvDepartamento.ChangeMode(DetailsViewMode.Insert)
            SetBehavior(ViewMode.GridView)
        End If
    End Sub

    ''' <summary>
    ''' Evento del botón Eliminar correspondiente a la fila del grid seleccionado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub lnkEliminar_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim fila As GridViewRow = CType(CType(sender, Control).Parent.Parent, GridViewRow)
            Dim idDepartamento As Integer = gvDepartamentos.DataKeys(fila.RowIndex).Value.ToString()
            If (oDepartamentos.EliminarDepartamento(idDepartamento)) Then
                Master.MensajeInfo = ItzultzaileWeb.Itzuli("El departamento se eliminó correctamente")
                BindDataViews(0)
                dvDepartamento.ChangeMode(DetailsViewMode.Insert)
                SetBehavior(ViewMode.GridView)
            Else
                Master.MensajeError = ItzultzaileWeb.Itzuli("Se ha producido un error al eliminar el departamento")
            End If
        Catch ex As Exception
            Master.MensajeError = ItzultzaileWeb.Itzuli("Se ha producido un error al eliminar el departamento")
        End Try
    End Sub

    ''' <summary>
    ''' Cambio de index de la lista de plantas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ddlPlantas_SelectedIndexChanged(sender As Object, e As EventArgs)
        BindDataViews(0)
    End Sub

#End Region

End Class