Public Class TransmissionMode
    Inherits PageBase

    Dim oTransmissionModeBLL As New BLL.TransmissionModeBLL

#Region "PROPIEDADES"

    Protected Overrides ReadOnly Property RolesAcceso As System.Collections.Generic.List(Of ELL.Roles.RolUsuario)
        Get
            Dim roles As New List(Of ELL.Roles.RolUsuario)
            roles.Add(ELL.Roles.RolUsuario.Administrador)
            roles.Add(ELL.Roles.RolUsuario.DocumentationTechnician)
            Return roles
        End Get
    End Property

#End Region

#Region "CARGA PÁGINA"

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub TransmissionMode_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            ' Forzamos la traducción del details view ya que de inicio está visible = false y no se estaba traduciendo
            ItzultzaileWeb.Itzuli(Me.gvTransmissionMode)
        End If
    End Sub

    ''' <summary>
    ''' Page_Load de la página
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Try
                ComprobarAcceso()
            Catch ex As Exception
                Response.Redirect("~/PermisoDenegado.aspx")
            End Try

            If Not Page.IsPostBack Then
                Initialize()
            End If

            Page.MaintainScrollPositionOnPostBack = True
        Catch ex As Exception
            log.Error("Error al cargar la página", ex)
        End Try
    End Sub

#End Region

#Region "METODOS"

    ''' <summary>
    ''' Comprueba que el perfil tenga acceso a la página
    ''' </summary>
    Private Sub ComprobarAcceso()
        ' Hay que comprobar que alguno de los roles del usuario está contenido en la lista de roles de acceso de la pagina
        Dim tieneAcceso As Boolean = ExisteRolEnPagina(PerfilUsuario.IdRol)

        ' El administrador puede entrar a todas la páginas aunque no se haya definido su rol explicitamente en cada página
        If ((RolesAcceso Is Nothing OrElse Not tieneAcceso) AndAlso Not (PerfilUsuario.IdRol = ELL.Roles.RolUsuario.Administrador)) Then
            Dim segmentos As String() = Page.Request.Url.Segments
            Dim pag As String = segmentos(Page.Request.Url.Segments.GetUpperBound(0))
            'WriteLog("Se ha intentado acceder a una ruta que su perfil no lo permite. Pag(" & pag & ")", TipoLog.Warn, Nothing)
            Response.Redirect("~/PermisoDenegado.aspx", False)
        End If
    End Sub

    ''' <summary>
    ''' Comprueba que el rol del usuario está contenido en la lista de roles de acceso de la pagina
    ''' </summary>
    ''' <param name="rolUsuario"></param>
    ''' <returns>True si existe alguno. False en caso contrario</returns>
    ''' <remarks></remarks>
    Private Function ExisteRolEnPagina(ByVal rolUsuario As Integer) As Boolean
        Dim idRol As Integer = Integer.MinValue
        Dim existe As Boolean = False
        If (RolesAcceso IsNot Nothing) Then
            existe = RolesAcceso.Exists(Function(f) f = [Enum].Parse(GetType(ELL.Roles.RolUsuario), rolUsuario.ToString()))
            If (existe) Then
                Return existe
            End If
        End If

        Return existe
    End Function

    ''' <summary>
    ''' Inicializa las variables y vincula los datos
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Initialize()
        ClearGridView()
        BindDataView()
    End Sub

    ''' <summary>
    ''' Limpia los datos vinculados
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ClearGridView()
        gvTransmissionMode.DataSource = Nothing
        gvTransmissionMode.DataBind()
    End Sub

    ''' <summary>
    ''' Vincula los datos
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub BindDataView()
        Dim listaTransmissionMode As List(Of ELL.TransmissionMode) = Nothing

        listaTransmissionMode = oTransmissionModeBLL.CargarLista()

        If (listaTransmissionMode.Count > 0) Then
            gvTransmissionMode.DataSource = listaTransmissionMode
            gvTransmissionMode.DataBind()
            gvTransmissionMode.Caption = String.Empty
        Else
            gvTransmissionMode.DataSource = Nothing
            gvTransmissionMode.DataBind()
            gvTransmissionMode.Caption = "No records found"
        End If
    End Sub

    ''' <summary>
    ''' Guardar un nuevo registro de Transmission Mode
    ''' </summary>
    ''' <param name="nombre"></param>
    ''' <remarks></remarks>
    Private Sub GuardarRegistro(ByVal nombre As String, ByVal descripcion As String, ByVal obsoleto As Boolean)
        Dim tm As New ELL.TransmissionMode With {.Nombre = nombre, .Descripcion = descripcion, .Obsoleto = obsoleto}

        If (oTransmissionModeBLL.GuardarTransmissionMode(tm)) Then
            Master.MensajeInfo = "The transmission mode has been saved successfully".ToUpper
            BindDataView()
        Else
            Master.MensajeError = "An error occurred while saving the transmission mode".ToUpper
        End If
    End Sub

#End Region

#Region "HANDLERS"

    ''' <summary>
    ''' Agregar un registro cuando el listado está vacío
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs)
        Dim nombre As TextBox = DirectCast(gvTransmissionMode.Controls(0).Controls(0).FindControl("txtNombre"), TextBox)
        Dim descripcion As TextBox = DirectCast(gvTransmissionMode.Controls(0).Controls(0).FindControl("txtDescripcion"), TextBox)
        Dim chkObsoleto As CheckBox = DirectCast(gvTransmissionMode.Controls(0).Controls(0).FindControl("chkObsoleto"), CheckBox)
        If Not (oTransmissionModeBLL.Existe(nombre.Text.ToLower)) Then
            GuardarRegistro(nombre.Text.ToLower, descripcion.Text.ToLower, chkObsoleto.Checked)
        Else
            'El nombre ya existe en base de datos
            Master.MensajeError = "Another element with the same name already exists".ToUpper
        End If
    End Sub

    ''' <summary>
    ''' RowCommand del gridview
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvTransmissionMode_OnRowCommand(sender As Object, e As GridViewCommandEventArgs)
        ' We are checking against the "ADD"
        If e.CommandName = "Add" Then
            Dim nombre As String = DirectCast(gvTransmissionMode.FooterRow.FindControl("txtNombre"), TextBox).Text
            Dim descripcion As String = DirectCast(gvTransmissionMode.FooterRow.FindControl("txtDescripcion"), TextBox).Text
            Dim obsoleto As Boolean = DirectCast(gvTransmissionMode.FooterRow.FindControl("chkObsoleto"), CheckBox).Checked
            If Not (oTransmissionModeBLL.Existe(nombre.ToLower)) Then
                GuardarRegistro(nombre.Trim, descripcion.Trim, obsoleto)
            Else
                'El nombre ya existe en base de datos
                Master.MensajeError = "Another element with the same name already exists".ToUpper
            End If
        End If
    End Sub

    ''' <summary>
    ''' Cancelación de la edición del grid
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvTransmissionMode_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs)
        gvTransmissionMode.EditIndex = -1
        BindDataView()
    End Sub

    ''' <summary>
    ''' Habilitar la edición de un registro del grid
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvTransmissionMode_RowEditing(sender As Object, e As GridViewEditEventArgs)
        gvTransmissionMode.EditIndex = e.NewEditIndex
        BindDataView()
    End Sub

    ''' <summary>
    ''' Edición de un registro
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvTransmissionMode_RowUpdating(sender As Object, e As GridViewUpdateEventArgs)
        Dim row As GridViewRow = gvTransmissionMode.Rows(e.RowIndex)
        Dim txtNombre As TextBox = TryCast(row.FindControl("txtNombre"), TextBox)
        Dim txtDescripcion As TextBox = TryCast(row.FindControl("txtDescripcion"), TextBox)
        Dim chkObsoleto As CheckBox = TryCast(row.FindControl("chkObsoleto"), CheckBox)

        'If Not (oTransmissionModeBLL.Existe(txtNombre.Text.ToLower)) Then
        If (oTransmissionModeBLL.ModificarTransmissionMode(New ELL.TransmissionMode With {.Id = gvTransmissionMode.DataKeys(e.RowIndex).Value.ToString(), .Nombre = txtNombre.Text.Trim, .Descripcion = txtDescripcion.Text.Trim, .Obsoleto = chkObsoleto.Checked})) Then
            Master.MensajeInfo = "The transmission mode has been edited successfully".ToUpper
            gvTransmissionMode.EditIndex = -1
            BindDataView()
        Else
            Master.MensajeError = "An error occurred while editing the transmission mode".ToUpper
        End If
        'Else
        ''El nombre ya existe en base de datos
        'Master.MensajeError = "Ya existe otro elemento con el mismo nombre"
        'End If
    End Sub

    ' ''' <summary>
    ' ''' Eliminación de un registro
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Protected Sub gvTransmissionMode_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)
    '    Dim id As Integer = CInt(gvTransmissionMode.DataKeys(e.RowIndex).Value)

    '    If (oTransmissionModeBLL.EliminarTransmissionMode(id)) Then
    '        Master.MensajeInfo = "Se ha eliminado satisfactoriamente"
    '        BindDataView()
    '    Else
    '        Master.MensajeError = "Ha ocurrido un error al eliminar"
    '    End If
    'End Sub

#End Region

End Class