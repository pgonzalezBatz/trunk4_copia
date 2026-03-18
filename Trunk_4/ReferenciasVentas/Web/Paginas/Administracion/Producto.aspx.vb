Public Class Producto
    Inherits PageBase

    Dim oProductoBLL As New BLL.ProductoBLL

#Region "Propiedades"

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
    Private Sub Productos_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            ' Forzamos la traducción del details view ya que de inicio está visible = false y no se estaba traduciendo
            ItzultzaileWeb.Itzuli(Me.gvProducto)
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
        gvProducto.DataSource = Nothing
        gvProducto.DataBind()
    End Sub

    ''' <summary>
    ''' Vincula los datos
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub BindDataView()
        Dim listaProductos As List(Of ELL.Producto) = Nothing

        listaProductos = oProductoBLL.CargarLista()

        If (listaProductos.Count > 0) Then
            gvProducto.DataSource = listaProductos
            gvProducto.DataBind()
            gvProducto.Caption = String.Empty
        Else
            gvProducto.DataSource = Nothing
            gvProducto.DataBind()
            gvProducto.Caption = "No records found"
        End If
    End Sub

    ''' <summary>
    ''' Guardar un nuevo registro de Type
    ''' </summary>
    ''' <param name="nombre"></param>
    ''' <remarks></remarks>
    Private Sub GuardarRegistro(ByVal nombre As String, ByVal descripcion As String, ByVal transmissionModeVisible As Boolean, ByVal tiposRelacionados As List(Of Integer), ByVal obsoleto As Boolean)
        Dim pr As New ELL.Producto With {.Nombre = nombre,
                                        .Descripcion = descripcion,
                                        .TransmissionModeVisible = transmissionModeVisible,
                                        .TiposRelacionados = tiposRelacionados,
                                        .Obsoleto = obsoleto}

        If (oProductoBLL.GuardarProducto(pr)) Then
            Master.MensajeInfo = "The record has been saved successfully".ToUpper
            BindDataView()
        Else
            Master.MensajeError = "An error occurred while saving the record".ToUpper
        End If
    End Sub

    ''' <summary>
    ''' Gestionar el producto
    ''' </summary>
    ''' <param name="idProducto">Id del producto</param>
    ''' <remarks></remarks>
    Private Sub GestionarProducto(ByVal idProducto As Integer)
        Try
            hfIdProducto.Value = idProducto
            MostrarModalPopupExtender(idProducto)
        Catch ex As Exception
            Master.MensajeError = "An error occurred while saving the data".ToUpper
            log.Error("An error occurred while saving the data", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Muestra el modal popup extender que gestiona el adjunto
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub MostrarModalPopupExtender(ByVal idProducto As Integer)
        Dim producto As ELL.Producto = Nothing
        If (idProducto <> 0) Then
            'Edición de un producto
            producto = oProductoBLL.CargarProducto(idProducto)
            lblTituloType.Text = String.Format("Edition of product '{0}'", producto.Nombre)
        Else
            lblTituloType.Text = "New product"
        End If
        CargarDatosModal(producto)
        mpe_Type.Show()
    End Sub

    ''' <summary>
    ''' Cargar los datos del pop up 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargarDatosModal(ByVal producto As ELL.Producto)
        If (producto IsNot Nothing) Then
            txtNombreProducto.Text = producto.Nombre
            txtDescripcionProducto.Text = producto.Descripcion
            chkObsoleto.Checked = producto.Obsoleto
            chkTransmissionModeVisible.Checked = producto.TransmissionModeVisible
            CargarTiposRelacionados(producto.Id)
        Else
            txtNombreProducto.Text = String.Empty
            txtDescripcionProducto.Text = String.Empty
            chkObsoleto.Checked = False
            chkTransmissionModeVisible.Checked = False
            CargarTiposRelacionados(0)
        End If
    End Sub

    ''' <summary>
    ''' Cargar los tipos relacionados con un producto
    ''' </summary>
    ''' <param name="idProducto"></param>
    ''' <remarks></remarks>
    Private Sub CargarTiposRelacionados(ByVal idProducto As Integer)
        Dim productoTipo As List(Of ELL.ProductoType)
        Dim types As New List(Of ELL.Type)
        Dim oType As New BLL.TypeBLL

        types = oType.CargarLista()
        chklTiposRelacionados.DataSource = types
        chklTiposRelacionados.DataBind()

        'Ponemos el check de ningún tipo siempre seleccionado
        chklTiposRelacionados.Items.FindByText("-").Selected = True
        chklTiposRelacionados.Items.FindByText("-").Enabled = False

        If (idProducto <> 0) Then
            productoTipo = oProductoBLL.CargarTiposProducto(idProducto)
            For Each tipo In productoTipo
                chklTiposRelacionados.Items.FindByValue(tipo.IdType).Selected = True
            Next
        End If
    End Sub

    ''' <summary>
    ''' Devuelve los tipos relacionados con el producto
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetTiposRelacionados() As List(Of Integer)
        Dim tipos As New List(Of Integer)
        For Each tipo As ListItem In chklTiposRelacionados.Items
            If (tipo.Selected) Then
                tipos.Add(tipo.Value)
            End If
        Next
        Return tipos
    End Function

#End Region

#Region "HANDLERS"

    ''' <summary>
    ''' Evento RowDataBound del gridview de productos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvProducto_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                Dim oTypeBLL As New BLL.TypeBLL

                Dim gvProductos As GridView = CType(sender, GridView)
                Dim idProducto As String = gvProductos.DataKeys(e.Row.RowIndex).Values(0).ToString()

                Dim lblRelationedTypes As Label = TryCast(e.Row.FindControl("lblRelationedTypes"), Label)
                Dim listaTypes As List(Of ELL.Type) = oTypeBLL.CargarTiposProducto(idProducto)
                For Each type In listaTypes
                    lblRelationedTypes.Text += type.Nombre + ", "
                Next
                lblRelationedTypes.Text = lblRelationedTypes.Text.Substring(0, lblRelationedTypes.Text.Length - 2)
            End If
        Catch ex As Exception
        End Try
    End Sub

    ''' <summary>
    ''' Agregar un registro cuando el listado está vacío
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs)
        Dim nombre As TextBox = DirectCast(gvProducto.Controls(0).Controls(0).FindControl("txtNombre"), TextBox)
        Dim descripcion As TextBox = DirectCast(gvProducto.Controls(0).Controls(0).FindControl("txtDescripcion"), TextBox)
        Dim chkTransmissionModeVisible As CheckBox = DirectCast(gvProducto.Controls(0).Controls(0).FindControl("chkTransmissionModeVisible"), CheckBox)
        Dim chkObsoleto As CheckBox = DirectCast(gvProducto.Controls(0).Controls(0).FindControl("chkObsoleto"), CheckBox)
        If Not (oProductoBLL.Existe(nombre.Text.Trim.ToLower)) Then
            GuardarRegistro(nombre.Text.Trim, descripcion.Text.Trim, chkTransmissionModeVisible.Checked, GetTiposRelacionados(), chkObsoleto.Checked)
        Else
            'El nombre ya existe en base de datos
            Master.MensajeError = "Another element with the same name already exists".ToUpper
        End If
    End Sub

    ''' <summary>
    ''' Linkbutton de edición de la fila del gridview de productos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lbEditar_Click(sender As Object, e As EventArgs)
        Dim btn As LinkButton = CType(sender, LinkButton)
        Dim row As GridViewRow = CType(btn.NamingContainer, GridViewRow)
        Dim idProducto As String = gvProducto.DataKeys(row.RowIndex).Value.ToString()
        GestionarProducto(idProducto)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnNuevoProducto_Click(sender As Object, e As EventArgs)
        GestionarProducto(0)
    End Sub

    ''' <summary>
    ''' Guardar los datos del producto
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Dim idProducto As String = hfIdProducto.Value
        Dim producto As New ELL.Producto With {.Nombre = txtNombreProducto.Text.Trim,
                                               .Descripcion = txtDescripcionProducto.Text.Trim,
                                               .Obsoleto = chkObsoleto.Checked,
                                               .TransmissionModeVisible = chkTransmissionModeVisible.Checked,
                                               .TiposRelacionados = GetTiposRelacionados()}

        If (idProducto = 0) Then
            'Guardar nuevo producto
            If (oProductoBLL.GuardarProducto(producto)) Then
                Master.MensajeInfo = "The product has been saved successfully".ToUpper
                BindDataView()
            Else
                Master.MensajeError = "An error occurred while saving the product".ToUpper
            End If
        Else
            'Edición de un producto
            producto.Id = idProducto
            If (oProductoBLL.ModificarProducto(producto)) Then
                Master.MensajeInfo = "The product has been edited successfully".ToUpper
                BindDataView()
            Else
                Master.MensajeError = "An error occurred while editing the product".ToUpper
            End If
        End If
        hfIdProducto.Value = String.Empty
    End Sub

#End Region

End Class