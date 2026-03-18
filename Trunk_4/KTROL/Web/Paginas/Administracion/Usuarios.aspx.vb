Public Class Usuarios
    Inherits PageBase

    Dim oUsuarios As New BLL.UsuariosBLL

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
            Return dvUsuario.AutoGenerateInsertButton
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
    Private Sub Usuarios_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            ' Forzamos la traducción del details view ya que de inicio está visible = false y no se estaba traduciendo
            ItzultzaileWeb.Itzuli(Me.dvUsuario)
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
                Response.Redirect("~/PermisoDenegado.aspx")
            End Try

            If Not Page.IsPostBack Then
                Initialize()
            End If
            AgregarScriptBusquedaUsuarios()

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
        Dim bHasGridRows As Boolean = (gvUsuarios.Rows.Count > 0)
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
        gvUsuarios.DataSource = Nothing
        gvUsuarios.DataBind()

        dvUsuario.DataSource = Nothing
        dvUsuario.DataBind()
    End Sub

    ''' <summary>
    ''' Vincula los datos
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub BindDataViews()
        BindDataViews(0)
    End Sub

    ''' <summary>
    ''' Vincula los datos
    ''' </summary>
    ''' <param name="idUsuario">Identificador del usuario</param>
    ''' <remarks></remarks>
    Protected Sub BindDataViews(ByVal idUsuario As Integer)
        Dim listaUsuarios As List(Of ELL.Usuarios) = Nothing
        If (idUsuario > 0) Then
            listaUsuarios = New List(Of ELL.Usuarios)
            listaUsuarios.Add(oUsuarios.ObtenerUsuarioPorId(idUsuario))
            dvUsuario.DataSource = listaUsuarios
            dvUsuario.DataBind()

            SetBehavior(ViewMode.DetailsView)
        Else
            listaUsuarios = oUsuarios.CargarUsuarios()

            If (listaUsuarios.Count > 0) Then
                gvUsuarios.DataSource = listaUsuarios
                gvUsuarios.DataBind()
            Else
                If (Me.AllowRecordInserting) Then
                    dvUsuario.DataSource = listaUsuarios
                    dvUsuario.DataBind()
                Else
                    gvUsuarios.DataSource = Nothing
                    gvUsuarios.DataBind()
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
        Dim txt As TextBox = CType(dvUsuario.FindControl("txtInsertUsuario"), TextBox)
        Dim hidden As HiddenField = CType(dvUsuario.FindControl("hfIdUsuario"), HiddenField)
        Dim imagen As HtmlGenericControl = CType(Me.dvUsuario.FindControl("imgSeleccion"), HtmlGenericControl)
        If (txt IsNot Nothing) Then
            txt.Text = String.Empty
        End If
        If (hidden IsNot Nothing) Then
            hidden.Value = String.Empty
            imagen.Attributes("class") = "imagen-no-seleccionado"
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
        Dim _ctl As Label = CType(dvUsuario.FindControl("lblId"), Label)
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
        Dim _ctl As TextBox = CType(dvUsuario.FindControl(sControlName), TextBox)
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
        Dim _ctl As DropDownList = CType(dvUsuario.FindControl(sControlName), DropDownList)
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
    Public Function GetHiddenFieldValue(ByVal sControlName As String) As String
        Dim sFieldValue As String = String.Empty
        Dim _ctl As HiddenField = CType(dvUsuario.FindControl(sControlName), HiddenField)
        If (_ctl IsNot Nothing) Then
            sFieldValue = _ctl.Value
        End If

        Return sFieldValue
    End Function

    ''' <summary>
    ''' Agrega un script de búsqueda de proveedores
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub AgregarScriptBusquedaUsuarios()
        Dim txtInsertUsuario As TextBox = CType(Me.dvUsuario.FindControl("txtInsertUsuario"), TextBox)
        Dim hfIdUsuario As HiddenField = CType(Me.dvUsuario.FindControl("hfIdUsuario"), HiddenField)
        Dim helper As HtmlGenericControl = CType(Me.dvUsuario.FindControl("helper"), HtmlGenericControl)
        Dim imagen As HtmlGenericControl = CType(Me.dvUsuario.FindControl("imgSeleccion"), HtmlGenericControl)

        If (Not hfIdUsuario Is Nothing AndAlso Not String.IsNullOrEmpty(hfIdUsuario.Value)) Then
            imagen.Attributes("class") = "imagen-seleccionado"
        Else
            imagen.Attributes("class") = "imagen-no-seleccionado"
        End If

        Dim cultura As String = CultureUser()

        Dim script As String = "initBusquedaUsuarios('" & txtInsertUsuario.ClientID & "', '" & hfIdUsuario.ClientID & "', '" & helper.ClientID & "','../BuscarUsuarios.aspx',false,'','" & imagen.ClientID & "', '" & cultura & "');"
        ScriptManager.RegisterStartupScript(Page, Me.GetType(), "usuarios", script, True)
    End Sub

    ''' <summary>
    ''' Obtiene la cultura del usuario logueado
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CultureUser() As String
        Dim persona As New Sablib.ELL.Ticket
        Dim culture As String = "es-ES"

        If (Session("Ticket") IsNot Nothing) Then
            persona = CType(Session("Ticket"), Sablib.ELL.Ticket)
            culture = persona.Culture
        End If

        Return culture
    End Function
#End Region

#Region "Handlers"

    ''' <summary>
    ''' Cambia a modo inserción en el details
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lbtnAgregarNuevoUsuario_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnAgregarNuevoUsuario.Click
        Dim ddlRolUsuario As DropDownList = CType(dvUsuario.FindControl("ddlInsertRolUsuario"), DropDownList)

        ' Llenamos el dropdownlist de exito
        If (ddlRolUsuario IsNot Nothing) Then
            ddlRolUsuario.Items.Clear()
            Dim Items As String() = System.Enum.GetNames(GetType(ELL.Usuarios.RolesUsuario))
            For Each item As String In Items
                Dim val = DirectCast([Enum].Parse(GetType(ELL.Usuarios.RolesUsuario), item), ELL.Usuarios.RolesUsuario)
                ddlRolUsuario.Items.Add(New ListItem(System.Text.RegularExpressions.Regex.Replace(item, "([A-Z])", " $1").Trim(), val))
            Next

            ddlRolUsuario.SelectedIndex = 0
        End If

        dvUsuario.ChangeMode(DetailsViewMode.Insert)
        SetBehavior(ViewMode.DetailsView)

        'Borramos los campos
        BorrarCampos()
    End Sub

    ''' <summary>
    ''' Editando la fila en el grid
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvUsuarios_RowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs) Handles gvUsuarios.RowEditing
        dvUsuario.ChangeMode(DetailsViewMode.Edit)
        Dim idUsuario As Integer = GetGridViewRowId(gvUsuarios.Rows(e.NewEditIndex))
        BindDataViews(idUsuario)
        SetBehavior(ViewMode.DetailsView)
    End Sub

    ''' <summary>
    ''' Vuelve al modo grid
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lbtnVolverLista_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnVolver.Click
        Me.dvUsuario.ChangeMode(DetailsViewMode.Insert)
        BindDataViews()
        SetBehavior(ViewMode.GridView)
    End Sub

    ''' <summary>
    ''' Cambio de página en el grid
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvUsuarios_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvUsuarios.PageIndexChanging
        gvUsuarios.PageIndex = e.NewPageIndex
        BindDataViews()
    End Sub

    ''' <summary>
    ''' Insertando un nuevo registro en el details
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub dvUsuario_ItemInserting(ByVal sender As Object, ByVal e As DetailsViewInsertEventArgs) Handles dvUsuario.ItemInserting
        Dim correcto As Boolean = False

        Try
            Dim usuario As New ELL.Usuarios()
            Dim idUsuario As String = Me.GetHiddenFieldValue("hfIdUsuario")
            If (String.IsNullOrEmpty(idUsuario)) Then
                Master.MensajeError = ItzultzaileWeb.Itzuli("Usuario campo obligatorio")
                Return
            Else
                correcto = True
                usuario.CodPersona = CInt(idUsuario)
            End If
            usuario.IdRol = CInt(GetDropDownListFieldValue("ddlInsertRolUsuario"))

            If (correcto) Then
                If Not (oUsuarios.ExisteUsuario(idUsuario)) Then
                    If (oUsuarios.GuardarUsuario(usuario)) Then
                        Master.MensajeInfo = ItzultzaileWeb.Itzuli("El usuario se ha creado correctamente")
                        BindDataViews()
                    Else
                        Master.MensajeError = ItzultzaileWeb.Itzuli("Se ha producido un error al crear el usuario")
                        dvUsuario.ChangeMode(DetailsViewMode.Insert)
                        SetBehavior(ViewMode.GridView)
                    End If
                    dvUsuario.ChangeMode(DetailsViewMode.Insert)
                    SetBehavior(ViewMode.GridView)
                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("El usuario ya existe")
                    dvUsuario.ChangeMode(DetailsViewMode.Insert)
                    SetBehavior(ViewMode.GridView)
                End If

            End If
        Catch ex As Exception
           Global_asax.log.Error("Se ha producido un error al crear el usuario", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Modificar los datos de un usuario
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub dvUsuario_ItemUpdating(sender As Object, e As DetailsViewUpdateEventArgs) Handles dvUsuario.ItemUpdating
        Dim usuario As New ELL.Usuarios With
            {.CodPersona = GetDetailsViewId(),
             .IdRol = GetDropDownListFieldValue("ddlEditRolUsuario")}
        Try
            If (oUsuarios.ModificarUsuario(usuario)) Then
                Master.MensajeInfo = ItzultzaileWeb.Itzuli("El usuario se ha guardado correctamente")
            Else
                Master.MensajeError = ItzultzaileWeb.Itzuli("Se ha producido un error al guardar el usuario")
            End If
            BindDataViews()
            dvUsuario.ChangeMode(DetailsViewMode.Insert)
            SetBehavior(ViewMode.GridView)
        Catch ex As Exception
           Global_asax.log.Error("Se ha producido un error al guardar el usuario", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Databound del detailsview
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub dvUsuario_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles dvUsuario.DataBound
        If (dvUsuario.DataItem IsNot Nothing) Then
            Dim usuario As ELL.Usuarios = CType(dvUsuario.DataItem, ELL.Usuarios)

            Dim ddlRolUsuario As DropDownList = CType(dvUsuario.FindControl("ddlEditRolUsuario"), DropDownList)

            ' Llenamos el dropdownlist de roles
            If (ddlRolUsuario IsNot Nothing) Then
                ddlRolUsuario.Items.Clear()
                'ddlRolUsuario.Items.Add(New ListItem("Administrador", ELL.Usuarios.RolesUsuario.Administrador))
                'ddlRolUsuario.Items.Add(New ListItem("Operario", ELL.Usuarios.RolesUsuario.Operario))
                'ddlRolUsuario.Items.Add(New ListItem("Calidad", ELL.Usuarios.RolesUsuario.Calidad))
                Dim Items As String() = System.Enum.GetNames(GetType(ELL.Usuarios.RolesUsuario))
                For Each item As String In Items
                    Dim val = DirectCast([Enum].Parse(GetType(ELL.Usuarios.RolesUsuario), item), ELL.Usuarios.RolesUsuario)
                    ddlRolUsuario.Items.Add(New ListItem(System.Text.RegularExpressions.Regex.Replace(item, "([A-Z])", " $1").Trim(), val))
                Next
                ddlRolUsuario.SelectedValue = usuario.IdRol
            End If
        End If
    End Sub

    ''' <summary>
    ''' Cambio de modo del details
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub dvUsuario_ModeChanging(ByVal sender As Object, ByVal e As DetailsViewModeEventArgs) Handles dvUsuario.ModeChanging
        If (e.CancelingEdit = False) Then
            dvUsuario.ChangeMode(e.NewMode)
            Dim id As Integer = GetGridViewRowId(gvUsuarios.SelectedRow)
            BindDataViews(id)
        Else
            dvUsuario.ChangeMode(DetailsViewMode.Insert)
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
            Dim idUsuario As String = gvUsuarios.DataKeys(fila.RowIndex).Value.ToString()

            If (oUsuarios.EliminarUsuario(idUsuario)) Then
                Master.MensajeInfo = ItzultzaileWeb.Itzuli("El usuario se elimino correctamente")
                BindDataViews(0)
                dvUsuario.ChangeMode(DetailsViewMode.Insert)
                SetBehavior(ViewMode.GridView)
            Else
                Master.MensajeError = ItzultzaileWeb.Itzuli("Se ha producido un error al eliminar el usuario")
            End If
            
        Catch ex As Exception
            Master.MensajeError = ItzultzaileWeb.Itzuli("Se ha producido un error al eliminar el usuario")
        End Try
    End Sub

#End Region

End Class