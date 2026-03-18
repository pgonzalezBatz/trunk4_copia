Public Class TiposReferenciaVenta
    Inherits PageBase

    Dim oTiposReferenciaVentaBLL As New BLL.TiposReferenciaVentaBLL

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
    Private Sub TiposReferenciaVenta_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            ' Forzamos la traducción del details view ya que de inicio está visible = false y no se estaba traduciendo
            ItzultzaileWeb.Itzuli(Me.gvTiposReferenciaVenta)
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
        gvTiposReferenciaVenta.DataSource = Nothing
        gvTiposReferenciaVenta.DataBind()
    End Sub

    ''' <summary>
    ''' Vincula los datos
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub BindDataView()
        Dim listaTiposReferenciaVenta As List(Of ELL.TiposReferenciaVenta) = Nothing

        listaTiposReferenciaVenta = oTiposReferenciaVentaBLL.CargarLista()

        If (listaTiposReferenciaVenta.Count > 0) Then
            gvTiposReferenciaVenta.DataSource = listaTiposReferenciaVenta
            gvTiposReferenciaVenta.DataBind()
            gvTiposReferenciaVenta.Caption = String.Empty
        Else
            gvTiposReferenciaVenta.DataSource = Nothing
            gvTiposReferenciaVenta.DataBind()
            gvTiposReferenciaVenta.Caption = "No records found"
        End If
    End Sub

    ''' <summary>
    ''' Guardar un nuevo registro de Tipo de Referencia
    ''' </summary>
    ''' <param name="nombre"></param>
    ''' <remarks></remarks>
    Private Sub GuardarRegistro(ByVal nombre As String, ByVal descripcion As String)
        Dim dh As New ELL.TiposReferenciaVenta With {.Nombre = nombre, .Descripcion = descripcion}

        If (oTiposReferenciaVentaBLL.GuardarTipoReferenciaVenta(dh)) Then
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
        Dim nombre As TextBox = DirectCast(gvTiposReferenciaVenta.Controls(0).Controls(0).FindControl("txtNombre"), TextBox)
        Dim descripcion As TextBox = DirectCast(gvTiposReferenciaVenta.Controls(0).Controls(0).FindControl("txtDescripcion"), TextBox)
        If Not (oTiposReferenciaVentaBLL.Existe(nombre.Text.ToLower)) Then
            GuardarRegistro(nombre.Text.Trim, descripcion.Text.Trim)
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
    Protected Sub gvTiposReferenciaVenta_OnRowCommand(sender As Object, e As GridViewCommandEventArgs)
        ' We are checking against the "ADD"
        If e.CommandName = "Add" Then
            Dim nombre As String = DirectCast(gvTiposReferenciaVenta.FooterRow.FindControl("txtNombre"), TextBox).Text
            Dim descripcion As String = DirectCast(gvTiposReferenciaVenta.FooterRow.FindControl("txtDescripcion"), TextBox).Text
            If Not (oTiposReferenciaVentaBLL.Existe(nombre.ToLower)) Then
                GuardarRegistro(nombre, descripcion)
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
    Protected Sub gvTiposReferenciaVenta_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs)
        gvTiposReferenciaVenta.EditIndex = -1
        BindDataView()
    End Sub

    ''' <summary>
    ''' Habilitar la edición de un registro
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvTiposReferenciaVenta_RowEditing(sender As Object, e As GridViewEditEventArgs)
        gvTiposReferenciaVenta.EditIndex = e.NewEditIndex
        BindDataView()
    End Sub

    ''' <summary>
    ''' Edición de un registro
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvTiposReferenciaVenta_RowUpdating(sender As Object, e As GridViewUpdateEventArgs)
        Dim row As GridViewRow = gvTiposReferenciaVenta.Rows(e.RowIndex)
        Dim txtNombre As TextBox = TryCast(row.FindControl("txtNombre"), TextBox)
        Dim txtDescripcion As TextBox = TryCast(row.FindControl("txtDescripcion"), TextBox)

        If (oTiposReferenciaVentaBLL.ModificarTipoReferenciaVenta(New ELL.TiposReferenciaVenta With {.Id = gvTiposReferenciaVenta.DataKeys(e.RowIndex).Value.ToString(), .Nombre = txtNombre.Text.Trim, .Descripcion = txtDescripcion.Text.Trim})) Then
            Master.MensajeInfo = "The type has been edited successfully".ToUpper
            gvTiposReferenciaVenta.EditIndex = -1
            BindDataView()
        Else
            Master.MensajeError = "An error occurred while editing the type".ToUpper
        End If
    End Sub

    ''' <summary>
    ''' Eliminación de un registro
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvTiposReferenciaVenta_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        Dim id As Integer = CInt(gvTiposReferenciaVenta.DataKeys(e.RowIndex).Value)

        If (oTiposReferenciaVentaBLL.EliminarTipoReferenciaVenta(id)) Then
            Master.MensajeInfo = "The type has been deleted successfully".ToUpper
            BindDataView()
        Else
            Master.MensajeError = "An error occurred while deleting the type".ToUpper
        End If
    End Sub

#End Region

End Class