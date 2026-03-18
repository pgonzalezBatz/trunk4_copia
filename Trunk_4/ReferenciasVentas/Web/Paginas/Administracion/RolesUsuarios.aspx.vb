Public Class RolesUsuarios
    Inherits PageBase

    Dim oUsuariosBLL As New BLL.UsuariosBLL

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
    Private Sub RolesUsuarios_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            ' Forzamos la traducción del ListView
            ItzultzaileWeb.Itzuli(Me.gvRolUsuario)
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
            Log.Error("Error al cargar la página", ex)
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
            Response.Redirect(PageBase.PAG_PERMISODENEGADO, False)
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
        BindRoles()
        BindDataView()
    End Sub

    ''' <summary>
    ''' Limpia los datos vinculados
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ClearGridView()
        gvRolUsuario.DataSource = Nothing
        gvRolUsuario.DataBind()
    End Sub

    ''' <summary>
    ''' Vincula los datos
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub BindRoles()
        ddlRol.Items.Clear()

        Dim items As Array
        items = System.Enum.GetNames(GetType(ELL.Roles.RolUsuario))
        For Each item As String In items
            If Not (item.ToLower.Contains("admin")) Then
                Dim val = DirectCast([Enum].Parse(GetType(ELL.Roles.RolUsuario), item), ELL.Roles.RolUsuario)
                ddlRol.Items.Add(New ListItem(System.Text.RegularExpressions.Regex.Replace(item, "([A-Z])", " $1").Trim(), val))
            End If
        Next
    End Sub

    ''' <summary>
    ''' Vincula los datos
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub BindDataView()
        Dim listaUsuarios As List(Of ELL.Usuarios) = Nothing

        'Cargamos la lista de usuarios de un rol especifico
        listaUsuarios = oUsuariosBLL.CargarUsuarios(ddlRol.SelectedValue)

        If (listaUsuarios.Count > 0) Then
            gvRolUsuario.DataSource = listaUsuarios
            gvRolUsuario.DataBind()
        Else
            gvRolUsuario.DataSource = Nothing
            gvRolUsuario.DataBind()
        End If

        'Cargamos la lista de usuarios de un rol

        Dim listaUsuariosExcepcional As New Dictionary(Of String, Integer)
        Dim gtkUsuario As New SabLib.ELL.Usuario
        Dim UsuariosComponent As New SabLib.BLL.UsuariosComponent
        Dim nombreRol As ELL.Roles.RolUsuario = CType(ddlRol.SelectedValue, ELL.Roles.RolUsuario)
        Dim cadenaIdSab As String() = System.Configuration.ConfigurationManager.AppSettings(nombreRol.ToString).ToString.Split(",")
        If (cadenaIdSab.Count > 0) Then
            For Each cadena In cadenaIdSab
                If Not (String.IsNullOrEmpty(cadena)) Then
                    gtkUsuario = UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = cadena}, False)
                    listaUsuariosExcepcional.Add(gtkUsuario.NombreCompleto.ToUpper, cadena)
                End If
            Next
            gvRolUsuarioExcepcion.DataSource = listaUsuariosExcepcional
            gvRolUsuarioExcepcion.DataBind()
        Else
            gvRolUsuarioExcepcion.DataSource = Nothing
            gvRolUsuarioExcepcion.DataBind()
        End If
    End Sub

#End Region

#Region "HANDLERS"

    ''' <summary>
    ''' Cambio de rol
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub ddlRol_SelectedIndexChanged(sender As Object, e As EventArgs)
        BindDataView()
    End Sub





#End Region

End Class