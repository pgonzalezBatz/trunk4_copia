Public Class Types
    Inherits PageBase

    Dim oTypeBLL As New BLL.TypeBLL

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
    Private Sub Types_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            ' Forzamos la traducción del details view ya que de inicio está visible = false y no se estaba traduciendo
            ItzultzaileWeb.Itzuli(Me.gvType)
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
        gvType.DataSource = Nothing
        gvType.DataBind()
    End Sub

    ''' <summary>
    ''' Vincula los datos
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub BindDataView()
        Dim listaType As List(Of ELL.Type) = Nothing

        listaType = oTypeBLL.CargarLista()

        If (listaType.Count > 0) Then
            gvType.DataSource = listaType
            gvType.DataBind()
            gvType.Caption = String.Empty
        Else
            gvType.DataSource = Nothing
            gvType.DataBind()
            gvType.Caption = "No records found"
        End If
    End Sub

    ''' <summary>
    ''' Guardar un nuevo registro de Tipo
    ''' </summary>
    ''' <param name="nombre"></param>
    ''' <remarks></remarks>
    Private Sub GuardarRegistro(ByVal nombre As String, ByVal descripcion As String, ByVal obsoleto As Boolean)
        Dim tipo As New ELL.Type With {.Nombre = nombre, .Descripcion = descripcion, .Obsoleto = obsoleto}

        If (oTypeBLL.GuardarTipo(tipo)) Then
            Master.MensajeInfo = "The type has been saved successfully".ToUpper
            BindDataView()
        Else
            Master.MensajeError = "An error occurred while saving the type".ToUpper
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
        Dim nombre As TextBox = DirectCast(gvType.Controls(0).Controls(0).FindControl("txtNombre"), TextBox)
        Dim descripcion As TextBox = DirectCast(gvType.Controls(0).Controls(0).FindControl("txtDescripcion"), TextBox)
        Dim chkObsoleto As CheckBox = DirectCast(gvType.Controls(0).Controls(0).FindControl("chkObsoleto"), CheckBox)
        If Not (oTypeBLL.Existe(nombre.Text.ToLower)) Then
            GuardarRegistro(nombre.Text.Trim, descripcion.Text.Trim, chkObsoleto.Checked)
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
    Protected Sub gvType_OnRowCommand(sender As Object, e As GridViewCommandEventArgs)
        ' We are checking against the "ADD"
        If e.CommandName = "Add" Then
            Dim nombre As String = DirectCast(gvType.FooterRow.FindControl("txtNombre"), TextBox).Text
            Dim descripcion As String = DirectCast(gvType.FooterRow.FindControl("txtDescripcion"), TextBox).Text
            Dim obsoleto As Boolean = DirectCast(gvType.FooterRow.FindControl("chkObsoleto"), CheckBox).Checked
            If Not (oTypeBLL.Existe(nombre.ToLower)) Then
                GuardarRegistro(nombre, descripcion, obsoleto)
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
    Protected Sub gvType_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs)
        gvType.EditIndex = -1
        BindDataView()
    End Sub

    ''' <summary>
    ''' Habilitar la edición de un registro
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvType_RowEditing(sender As Object, e As GridViewEditEventArgs)
        gvType.EditIndex = e.NewEditIndex
        BindDataView()
    End Sub

    ''' <summary>
    ''' Edición de un registro
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvType_RowUpdating(sender As Object, e As GridViewUpdateEventArgs)
        Dim row As GridViewRow = gvType.Rows(e.RowIndex)
        Dim txtNombre As TextBox = TryCast(row.FindControl("txtNombre"), TextBox)
        Dim txtDescripcion As TextBox = TryCast(row.FindControl("txtDescripcion"), TextBox)
        Dim chkObsoleto As CheckBox = TryCast(row.FindControl("chkObsoleto"), CheckBox)

        If (oTypeBLL.ModificarTipo(New ELL.Type With {.Id = gvType.DataKeys(e.RowIndex).Value.ToString(), .Nombre = txtNombre.Text.Trim, .Descripcion = txtDescripcion.Text.Trim, .Obsoleto = chkObsoleto.Checked})) Then
            Master.MensajeInfo = "The type has been edited successfully".ToUpper
            gvType.EditIndex = -1
            BindDataView()
        Else
            Master.MensajeError = "An error occurred while editing the type".ToUpper
        End If
    End Sub

#End Region

End Class