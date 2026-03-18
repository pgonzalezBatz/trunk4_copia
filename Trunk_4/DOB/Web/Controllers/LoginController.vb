Imports System.Web.Mvc

Public Class LoginController
    Inherits BaseController

#Region "Métodos"

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="seleccionarRol"></param>
    ''' <param name="cargaInicial"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function Index(Optional ByVal seleccionarRol As Boolean = False, Optional ByVal cargaInicial As Boolean = False) As ActionResult
        TempData("seleccionarRol") = seleccionarRol
        TempData("cargaInicial") = cargaInicial
        If (seleccionarRol) Then
            CargarUsuariosRoles()
        End If
        Return View()
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function AccesoDenegado() As ActionResult
        Return View("AccesoDenegado")
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function Salir() As ActionResult
        Dim url As String = "/homeintranet"

        Session("Ticket") = Nothing
        Session.Abandon()
        Session.Clear()
        Return Redirect(url)
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    <HttpPost>
    Function SeleccionarRol(ByVal Roles As Integer) As ActionResult
        Dim idRol As Integer = Integer.MinValue

        ' Si Roles es Integer.MaxValue quiere decir que el rol se ha añadido dinamicamente y su rol es consultor
        If (Roles = Integer.MaxValue) Then
            Dim usuarioRol As ELL.UsuarioRol = BLL.UsuariosRolBLL.CargarListado(Ticket.IdUser).FirstOrDefault(Function(f) f.IdRol <> Roles).ShallowCopy()

            ' Le cambiamos el rol a consultor y le ponemos el id planta
            Dim rol As ELL.Rol = BLL.RolesBLL.ObtenerRol(ELL.Rol.RolUsuario.Consultor)
            Dim planta As ELL.Planta = BLL.PlantasBLL.ObtenerPlanta(ELL.Planta.PLANTA_BATZ_GROUP)
            usuarioRol.Id = Integer.MaxValue
            usuarioRol.IdRol = rol.Id
            usuarioRol.DescripcionRol = rol.Descripcion
            usuarioRol.IdPlanta = planta.Id
            usuarioRol.Planta = planta.Planta
            idRol = rol.Id

            RolActual = usuarioRol
        Else
            RolActual = BLL.UsuariosRolBLL.Obtener(Roles)
            idRol = RolActual.IdRol
        End If

        'Quitamos de la lista de roles del usuario el resto de roles (menos administrador si es que lo tiene)
        'RolesUsuario.RemoveAll(Function(f) f.IdRol <> idRol AndAlso f.IdRol <> ELL.Rol.RolUsuario.Administrador)

        TempData("RegenerarMenu") = True

        Return RedirectToAction("Index", "Login")
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub CargarUsuariosRoles()
        Dim usuariosRoles As List(Of ELL.UsuarioRol) = BLL.UsuariosRolBLL.CargarListado(Ticket.IdUser)
        Dim rolesAux As New List(Of ELL.UsuarioRol)
        rolesAux.AddRange(usuariosRoles)

        ' Quitamos el rol de administrador de la lista de roles de usuario. No es un rol seleccionable
        rolesAux.RemoveAll(Function(f) f.IdRol = ELL.Rol.RolUsuario.Administrador)

        Dim roles As List(Of Mvc.SelectListItem) = rolesAux.Select(Function(f) New Mvc.SelectListItem With {.Text = f.DescripcionCompleta, .Value = f.Id}).OrderBy(Function(f) f.Text).ToList()
        ViewData("Roles") = roles
    End Sub

#End Region

End Class