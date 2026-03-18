Imports System.Web.Mvc

Namespace Controllers
    Public Class UsuariosRolesController
        Inherits AdministracionController

#Region "Métodos"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Function Index() As ActionResult
            CargarUsuariosRoles(idPlanta:=If(Planta <> 0, Planta, Nothing))
            CargarRoles()
            CargarPlantas()

            Return View("Index")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="RolesFiltro"></param>
        ''' <param name="PlantasFiltro"></param>
        ''' <returns></returns>
        <HttpPost>
        Function Index(ByVal RolesFiltro As Integer?, ByVal PlantasFiltro As Integer?) As ActionResult
            ' Guardamos la planta en la cookies
            Planta = PlantasFiltro

            CargarUsuariosRoles(RolesFiltro, PlantasFiltro)
            CargarRoles()
            CargarPlantas()

            Return View("Index")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Function Ver() As ActionResult
            CargarUsuariosRoles(idPlanta:=RolActual.IdPlanta)

            Return View("Ver")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Function Eliminar(ByVal id As Integer) As ActionResult
            Try
                Dim usuarioRol As ELL.UsuarioRol = BLL.UsuariosRolBLL.Obtener(id)
                ' Un usuario no se puede borrar si es gerente o director y si tiene asignado algún objetivo
                If ((usuarioRol.IdRol = ELL.Rol.RolUsuario.Responsable OrElse usuarioRol.IdRol = ELL.Rol.RolUsuario.Lider_de_objetivos) AndAlso BLL.ObjetivosBLL.ExisteResponsable(usuarioRol.IdSab, usuarioRol.IdPlanta)) Then
                    MensajeAlerta(Utils.Traducir("El usuario/rol está asignado a algún objetivo"))
                    Return Index()
                End If

                BLL.UsuariosRolBLL.Eliminar(id, ConfigurationManager.AppSettings("RecursoWeb"), ConfigurationManager.AppSettings("IdGrupoSeguimiento"))
                MensajeInfo(Utils.Traducir("Usuario/Rol eliminado correctamente"))

                'RolesUsuario = BLL.UsuariosRolBLL.CargarListado(Ticket.IdUser)
                TempData("RegenerarMenu") = True
            Catch ex As Exception
                MensajeError(Utils.Traducir("Error al eliminar Usuario/Rol"))
                log.Error("Error al eliminar Usuario/Rol", ex)
            End Try

            Return RedirectToAction("Index")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Function Darbaja(ByVal id As Integer) As ActionResult
            Try
                BLL.UsuariosRolBLL.MarcarComoObsoleto(id)
                MensajeInfo(Utils.Traducir("Usuario/Rol dado de baja correctamente"))

                TempData("RegenerarMenu") = True
            Catch ex As Exception
                MensajeError(Utils.Traducir("Error al dar de baja Usuario/Rol"))
                log.Error("Error al dar de baja Usuario/Rol", ex)
            End Try

            Return RedirectToAction("Index")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="hfIdUsuarioRol"></param>
        ''' <param name="RolesCambio"></param>
        ''' <returns></returns>
        Function CambiarRol(ByVal hfIdUsuarioRol As Integer, ByVal RolesCambio As String) As ActionResult
            Try
                BLL.UsuariosRolBLL.CambiarRol(hfIdUsuarioRol, RolesCambio)
                MensajeInfo(Utils.Traducir("Usuario/Rol cambiado correctamente"))

                TempData("RegenerarMenu") = True
            Catch ex As Exception
                MensajeError(Utils.Traducir("Error al cambiar Usuario/Rol"))
                log.Error("Error al cambiar Usuario/Rol", ex)
            End Try

            Return RedirectToAction("Index")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="hfIdUsuario"></param>
        ''' <param name="Roles"></param>
        ''' <param name="Plantas"></param>
        ''' <returns></returns>
        <HttpPost>
        Function Añadir(ByVal hfIdUsuario As Integer, ByVal Roles As Integer, ByVal Plantas As Integer) As ActionResult
            ' Obtenemos los roles del usuario
            Dim listaUsuarioRol As List(Of ELL.UsuarioRol) = BLL.UsuariosRolBLL.CargarListado(hfIdUsuario).Where(Function(f) f.FechaBajaDOB = DateTime.MinValue).ToList()
            If (listaUsuarioRol.Exists(Function(f) f.IdPlanta = Plantas AndAlso f.IdRol = Roles)) Then
                MensajeAlerta(Utils.Traducir("El Usuario/Rol ya existe"))
                ViewData("Usuario") = listaUsuarioRol.FirstOrDefault(Function(f) f.IdPlanta = Plantas AndAlso f.IdRol = Roles)
                Return Index()
            End If

            'Un usuario/rol siempre tiene que tener una planta a no ser que sea el rol Administrador
            If (Roles <> ELL.Rol.RolUsuario.Administrador AndAlso Plantas = Integer.MinValue) Then
                MensajeAlerta(Utils.Traducir("El Usuario/Rol tiene que ir asociado a una planta"))
                ViewData("Usuario") = listaUsuarioRol.FirstOrDefault(Function(f) f.IdPlanta = Plantas AndAlso f.IdRol = Roles)
                Return Index()
            End If

            'Dentro de una misma planta un usuario no puede tener más de un rol
            If (listaUsuarioRol.Where(Function(f) f.IdPlanta = Plantas).Count > 0) Then
                MensajeAlerta(Utils.Traducir("Un usuario no puede tener más de un rol en la misma planta"))
                ViewData("Usuario") = listaUsuarioRol.FirstOrDefault(Function(f) f.IdPlanta = Plantas AndAlso f.IdRol = Roles)
                Return Index()
            End If

            Try
                Dim usuarioRol As New ELL.UsuarioRol With {.IdSab = hfIdUsuario, .IdRol = Roles, .IdPlanta = Plantas}
                BLL.UsuariosRolBLL.Guardar(usuarioRol, ConfigurationManager.AppSettings("RecursoWeb"), ConfigurationManager.AppSettings("IdGrupoSeguimiento"))
                MensajeInfo(Utils.Traducir("Usuario/Rol guardado correctamente"))

                'RolesUsuario = BLL.UsuariosRolBLL.CargarListado(Ticket.IdUser)
                TempData("RegenerarMenu") = True
            Catch ex As Exception
                MensajeError(Utils.Traducir("Error al guardar Usuario/Rol"))
                log.Error("Error al guardar Usuario/Rol", ex)
            End Try

            Return RedirectToAction("Index")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idRol"></param>
        ''' <param name="idPlanta"></param>
        Private Sub CargarUsuariosRoles(Optional ByVal idRol As Integer? = Nothing, Optional ByVal idPlanta As Integer? = Nothing)
            Dim listaIdRoles As New List(Of Integer)
            If (idRol IsNot Nothing AndAlso idRol <> Integer.MinValue) Then
                listaIdRoles.Add(idRol)
            End If

            ViewData("UsuariosRoles") = BLL.UsuariosRolBLL.CargarListado(idPlanta:=idPlanta, listaIdRoles:=listaIdRoles).OrderBy(Function(f) f.NombreUsuario).ToList()
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        Private Sub CargarRoles()
            ' Vamos a cargar todos los roles
            Dim roles As List(Of ELL.Rol) = BLL.RolesBLL.CargarListado()
            Dim rolesLI As List(Of Mvc.SelectListItem) = roles.Select(Function(f) New Mvc.SelectListItem With {.Text = Utils.Traducir(f.Descripcion), .Value = f.Id}).OrderBy(Function(f) f.Text).ToList()
            Dim rolesLIFiltro As List(Of Mvc.SelectListItem) = roles.Select(Function(f) New Mvc.SelectListItem With {.Text = Utils.Traducir(f.Descripcion), .Value = f.Id}).OrderBy(Function(f) f.Text).ToList()
            Dim rolesCambioLI As List(Of Mvc.SelectListItem) = roles.Where(Function(f) f.Id <> ELL.Rol.RolUsuario.Administrador).Select(Function(f) New Mvc.SelectListItem With {.Text = Utils.Traducir(f.Descripcion), .Value = f.Id}).OrderBy(Function(f) f.Text).ToList()

            ' Al filtro le metemos el elemento vacio
            rolesLIFiltro.Insert(0, New SelectListItem With {.Text = String.Format("<{0}>", Utils.Traducir("Todos")), .Value = CStr(Integer.MinValue)})

            ViewData("Roles") = rolesLI
            ViewData("RolesFiltro") = rolesLIFiltro
            ViewData("RolesCambio") = rolesCambioLI
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        Private Sub CargarPlantas()
            Dim plantas As List(Of ELL.Planta) = BLL.PlantasBLL.CargarListado().OrderBy(Function(f) f.Planta).ToList()
            Dim plantasLI As List(Of Mvc.SelectListItem) = plantas.Select(Function(f) New Mvc.SelectListItem With {.Text = f.Planta, .Value = f.Id}).OrderBy(Function(f) f.Text).ToList()
            Dim plantasLIFiltro As List(Of Mvc.SelectListItem) = plantas.Select(Function(f) New Mvc.SelectListItem With {.Text = f.Planta, .Value = f.Id}).OrderBy(Function(f) f.Text).ToList()

            plantasLI.Insert(0, New SelectListItem With {.Text = String.Format("<{0}>", Utils.Traducir("Seleccione uno")), .Value = CStr(Integer.MinValue)})
            plantasLIFiltro.Insert(0, New SelectListItem With {.Text = String.Format("<{0}>", Utils.Traducir("Todos")), .Value = CStr(Integer.MinValue)})

            ' Cargamos la planta de la cookies si es que hay una guardada
            If (Planta <> 0 AndAlso plantasLIFiltro.Exists(Function(f) f.Value = Planta)) Then
                plantasLIFiltro.First(Function(f) f.Value = Planta).Selected = True
            End If

            ViewData("Plantas") = plantasLI
            ViewData("PlantasFiltro") = plantasLIFiltro
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function BuscarUsuarios(ByVal q As String) As JsonResult
            Dim usuariosBLL As New SabLib.BLL.UsuariosComponent
            Dim plantasBLL As New SabLib.BLL.PlantasComponent
            Dim  usuarios = (From users In usuariosBLL.GetUsuariosBusquedaSAB_Optimizado(q) Where ((users.FechaBaja = DateTime.MinValue OrElse users.FechaBaja > DateTime.Today) AndAlso Not String.IsNullOrEmpty(users.IdDirectorioActivo)) Select New With {.Id = users.Id, .NombreCompleto = String.Format("{0} ({1})", users.NombreCompleto, plantasBLL.GetPlanta(users.IdPlanta).Nombre)}).ToList()

            Return Json(usuarios, JsonRequestBehavior.AllowGet)
        End Function

#End Region

    End Class
End Namespace