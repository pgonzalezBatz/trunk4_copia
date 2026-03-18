Imports System.Globalization
Imports System.Web.Mvc

Namespace Controllers.Gerencia
    Public Class ObjetivosController
        Inherits GerenciaController

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Function Index() As ActionResult
            CargarRetos(cargarElementoVacio:=True)
            CargarProcesos(cargarElementoVacio:=True)
            CargarResponsablesConObjetivos(cargarElementoVacio:=True, ejercicio:=Ejercicio)
            CargarEjercicios(ejercicio:=Ejercicio)

            CargarObjetivos(ejercicio:=Ejercicio)

            Return View("Index")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Retos"></param>
        ''' <param name="Procesos"></param>
        ''' <param name="Responsables"></param>
        ''' <param name="Ejercicios"></param>
        ''' <returns></returns>
        <HttpPost>
        Function Index(ByVal Retos As Integer, ByVal Procesos As Integer, ByVal Responsables As Integer, ByVal Ejercicios As Integer) As ActionResult
            ' Guardamos el ejercicio en la cookies
            Ejercicio = Ejercicios

            CargarRetos(cargarElementoVacio:=True)
            CargarProcesos(cargarElementoVacio:=True)
            CargarResponsablesConObjetivos(cargarElementoVacio:=True, ejercicio:=Ejercicio)
            CargarEjercicios(ejercicio:=Ejercicios)

            CargarObjetivos(idReto:=Retos, idProceso:=Procesos, idResponsable:=Responsables, ejercicio:=Ejercicios)
            ViewData("IdResponsable") = Responsables

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idObjetivo"></param>
        ''' <returns></returns>
        Function MisObjetivos(Optional ByVal idObjetivo As Integer? = Nothing) As ActionResult
            CargarRetos(cargarElementoVacio:=True)
            CargarProcesos(cargarElementoVacio:=True)
            CargarEjercicios(Ejercicio)

            CargarObjetivos(ejercicio:=Ejercicio, idResponsable:=Ticket.IdUser)

            ' Esto es sólo para que se despliguen las acciones de un objetivo en concreto a cargarse la página
            ViewData("IdObjetivo") = idObjetivo

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        <HttpPost>
        Function MisObjetivos(ByVal Retos As Integer, ByVal Procesos As Integer, ByVal Ejercicios As Integer) As ActionResult
            ' Guardamos la planta en la cookies
            Ejercicio = Ejercicios

            CargarRetos(cargarElementoVacio:=True)
            CargarProcesos(cargarElementoVacio:=True)
            CargarEjercicios(ejercicio:=Ejercicios)

            CargarObjetivos(idReto:=Retos, ejercicio:=Ejercicios, idProceso:=Procesos, idResponsable:=Ticket.IdUser)

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Function Ver(ByVal idObjetivo As Integer) As ActionResult
            CargarObjetivo(idObjetivo)
            Dim objetivo As ELL.Objetivo = CType(ViewData("Objetivo"), ELL.Objetivo)

            'CargarMeses(objetivo)
            CargarAños(objetivo)
            CargarDocumentos(idObjetivo)

            Return View("Ver")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Function CuadroMando() As ActionResult
            ViewData("ContainerFluid") = 1

            CargarEjercicios(ejercicio:=Ejercicio)
            CargarTiposAgrupacion(ELL.Objetivo.TipoAgrupacion.Reto)
            CargarResponsablesConObjetivos(cargarElementoVacio:=True, ejercicio:=Ejercicio)

            '' Vemos si el usuario actual se ha seleccionado en la lista de responsables
            '' Si no se ha seleccionado pasamos nothing como idResponsables a la búsqueda de objetivos para que los busque todos
            '' Si encuentra uno seleccionado pasamos ese usuario
            '' Esto está hecho porque un usuario puede ser consultor en una planta y no ser responsables de objetivos y la busqueda no devuelve nada
            'Dim responsablesLI As List(Of Mvc.SelectListItem) = CType(ViewData("Responsables"), List(Of Mvc.SelectListItem))
            'Dim responsable As Mvc.SelectListItem = responsablesLI.FirstOrDefault(Function(f) f.Selected)
            'Dim idResponsable As Integer? = Nothing

            'If (responsable IsNot Nothing) Then
            '    idResponsable = responsable.Value
            '    ViewData("IdResponsable") = idResponsable
            'End If

            CargarObjetivos(ejercicio:=Ejercicio, tipoAgrupacion:=ELL.Objetivo.TipoAgrupacion.Reto)

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Function CuadroMandoHijos(ByVal idObjetivoPadre As Integer) As ActionResult
            ViewData("ContainerFluid") = 1

            Dim objetivoPadre As ELL.Objetivo = BLL.ObjetivosBLL.ObtenerObjetivo(idObjetivoPadre)
            ViewData("Titulo") = String.Format("{0} > {1} {2}", Utils.Traducir("Cuadro de mando"), Utils.Traducir("Objetivos desplegados de"), objetivoPadre.Descripcion)

            Dim listaObjetivos As New List(Of ELL.Objetivo)
            CargarObjetivosHijos(listaObjetivos, idObjetivoPadre, 1)

            ' Metemos el objetivo padre el primero de la lista de la lista
            listaObjetivos.Insert(0, objetivoPadre)

            ViewData("Objetivos") = listaObjetivos

            TempData("menu") = False
            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idObjetivo"></param>
        ''' <param name="descripcion"></param>
        ''' <returns></returns>
        Function EvolucionObjetivo(ByVal idObjetivo As Integer, Optional ByVal descripcion As String = Nothing) As ActionResult
            ViewData("IdObjetivo") = idObjetivo
            ViewData("Descripcion") = descripcion
            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idObjetivo"></param>
        Private Sub CargarObjetivo(idObjetivo As Integer)
            ViewData("Objetivo") = BLL.ObjetivosBLL.ObtenerObjetivo(idObjetivo)
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="ejercicio"></param>
        ''' <param name="tipoAgrupacion"></param>
        ''' <param name="idResponsable"></param>
        ''' <param name="idReto"></param>
        ''' <param name="idProceso"></param>
        Private Sub CargarObjetivos(Optional ByVal ejercicio As Integer? = Nothing, Optional ByVal tipoAgrupacion As Integer? = Nothing, Optional ByVal idResponsable As Integer? = Nothing,
                                    Optional ByVal idReto As Integer? = Nothing, Optional ByVal idProceso As Integer? = Nothing)
            Dim listaObjetivos As List(Of ELL.Objetivo) = BLL.ObjetivosBLL.CargarListado(RolActual.IdPlanta, idResponsable:=idResponsable, ejercicio:=If(ejercicio = Integer.MinValue, Nothing, ejercicio), idReto:=idReto, idProceso:=idProceso)

            If (tipoAgrupacion IsNot Nothing) Then
                Select Case tipoAgrupacion
                    Case ELL.Objetivo.TipoAgrupacion.Proceso
                        listaObjetivos = listaObjetivos.OrderBy(Function(f) f.IdProceso).ToList()
                    Case ELL.Objetivo.TipoAgrupacion.Responsable
                        listaObjetivos = listaObjetivos.OrderBy(Function(f) f.IdResponsable).ToList()
                    Case ELL.Objetivo.TipoAgrupacion.Reto
                        listaObjetivos = listaObjetivos.OrderBy(Function(f) f.IdReto).ToList()
                End Select
            Else
                listaObjetivos = listaObjetivos.OrderBy(Function(f) f.Descripcion).ToList()
            End If

            ViewData("Objetivos") = listaObjetivos
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="listaObjetivos"></param>
        ''' <param name="idObjetivoPadre"></param>
        ''' <param name="nivelArbol"></param>
        Private Sub CargarObjetivosHijos(ByRef listaObjetivos As List(Of ELL.Objetivo), ByVal idObjetivoPadre As Integer, ByVal nivelArbol As Integer)
            Dim listaObjetivosAux As List(Of ELL.Objetivo) = BLL.ObjetivosBLL.CargarListadoPorPadre(idObjetivoPadre)

            If (listaObjetivosAux IsNot Nothing AndAlso listaObjetivosAux.Count > 0) Then
                listaObjetivosAux = listaObjetivosAux.OrderBy(Function(f) f.Descripcion).ToList()
            End If

            For Each objetivo In listaObjetivosAux
                ' Añadimos el padre
                objetivo.NivelArbol = nivelArbol
                listaObjetivos.Add(objetivo)

                ' Vamos a ver si tiene más hijos
                CargarObjetivosHijos(listaObjetivos, objetivo.Id, nivelArbol + 1)
            Next
        End Sub

        '''' <summary>
        '''' 
        '''' </summary>
        'Private Sub CargarMisObjetivos(Optional ByVal idResponsable As Integer? = Nothing)
        '    ViewData("Objetivos") = BLL.ObjetivosBLL.CargarListado(RolActual.IdPlanta, idResponsable)
        'End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Function Agregar() As ActionResult
            CargarRetos(retoDeBaja:=False)
            CargarProcesos(procesoDeBaja:=False)
            CargarResponsables(cargarBajas:=False)
            'CargarMeses()
            'CargarAños()
            CargarTiposIndicadores()
            CargarPeriodicidad()

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idObjetivo"></param> 
        ''' <returns></returns>
        Function Editar(ByVal idObjetivo As Integer) As ActionResult
            CargarObjetivo(idObjetivo)
            Dim objetivo As ELL.Objetivo = CType(ViewData("Objetivo"), ELL.Objetivo)

            CargarRetos(objetivo, retoDeBaja:=False)
            CargarProcesos(objetivo, procesoDeBaja:=False)
            CargarResponsables(objetivo, cargarBajas:=False)
            'CargarMeses(objetivo)
            'CargarAños(objetivo)
            CargarTiposIndicadores(objetivo)
            CargarPeriodicidad(objetivo)
            CargarDocumentos(idObjetivo)

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idObjetivo"></param>
        ''' <returns></returns>
        Function Eliminar(ByVal idObjetivo As Integer) As ActionResult
            Try
                ' Se quiere poder borrar un objetivo de una vez con sus acciones, documentos.....
                'If (BLL.AccionesBLL.ExisteObjetivo(idObjetivo)) Then
                '    MensajeAlerta(Utils.Traducir("El objetivo tiene asignada alguna acción"))
                '    Return Index()
                'End If

                ' Hay que verificar que el objetivo a eliminar no tenga hijos
                Dim listaObjetivosHijos As List(Of ELL.Objetivo) = BLL.ObjetivosBLL.CargarListadoPorPadre(idObjetivo)

                If (listaObjetivosHijos IsNot Nothing AndAlso listaObjetivosHijos.Count > 0) Then
                    MensajeAlerta(Utils.Traducir("No se puede eliminar el objetivo porque ya ha sido desplegado"))
                Else
                    BLL.ObjetivosBLL.Eliminar(idObjetivo)
                    MensajeInfo(Utils.Traducir("Objetivo eliminado correctamente"))
                End If
            Catch ex As Exception
                MensajeError(Utils.Traducir("Error al eliminar objetivo"))
                log.Error("Error al eliminar objetivo", ex)
            End Try

            Return RedirectToAction("Index")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        Private Sub CargarDocumentos(ByVal id As Integer)
            ViewData("Documentos") = BLL.DocumentosBLL.CargarListado(id, ELL.TipoDocumento.Tipo.Objetivo)
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="objetivo"></param>
        ''' <param name="cargarElementoVacio"></param>
        ''' <param name="retoDeBaja"></param>
        Private Sub CargarRetos(Optional ByVal objetivo As ELL.Objetivo = Nothing, Optional ByVal cargarElementoVacio As Boolean = False, Optional ByVal retoDeBaja As Boolean? = Nothing)
            Dim retos As List(Of ELL.Reto) = BLL.RetosBLL.CargarListado(RolActual.IdPlanta, retoDeBaja:=retoDeBaja).OrderBy(Function(f) f.Titulo).ToList()
            Dim retosLI As New List(Of Mvc.SelectListItem)
            Dim groupActivos = New SelectListGroup() With {.Name = Utils.Traducir("Activos")}
            Dim groupInactivos = New SelectListGroup() With {.Name = Utils.Traducir("Inactivos")}

            retosLI.AddRange(retos.Where(Function(f) f.FechaBaja = DateTime.MinValue).Select(Function(f) New Mvc.SelectListItem With {.Text = f.Titulo, .Value = f.Id, .Group = groupActivos}).OrderBy(Function(f) f.Text).ToList())
            retosLI.AddRange(retos.Where(Function(f) f.FechaBaja <> DateTime.MinValue).Select(Function(f) New Mvc.SelectListItem With {.Text = f.Titulo, .Value = f.Id, .Group = groupInactivos}).OrderBy(Function(f) f.Text).ToList())

            If (cargarElementoVacio) Then
                retosLI.Insert(0, New SelectListItem With {.Text = String.Format("<{0}>", Utils.Traducir("Todos")), .Value = CStr(Integer.MinValue)})
            End If

            If (objetivo IsNot Nothing AndAlso retosLI.Exists(Function(f) f.Value = objetivo.IdReto)) Then
                retosLI.First(Function(f) f.Value = objetivo.IdReto).Selected = True
            End If

            ViewData("Retos") = retosLI
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="objetivo"></param>
        ''' <param name="idPlantaDestino"></param>
        Private Sub CargarRetosDespliegue(ByVal objetivo As ELL.Objetivo, ByVal idPlantaDestino As Integer?)
            Dim retos As List(Of ELL.Reto) = BLL.RetosBLL.CargarListado(idPlanta:=idPlantaDestino).OrderBy(Function(f) f.Titulo).ToList()
            Dim retosLI As New List(Of Mvc.SelectListItem)
            Dim groupActivos = New SelectListGroup() With {.Name = Utils.Traducir("Activos")}
            Dim groupInactivos = New SelectListGroup() With {.Name = Utils.Traducir("Inactivos")}

            retosLI.AddRange(retos.Where(Function(f) f.FechaBaja = DateTime.MinValue).Select(Function(f) New Mvc.SelectListItem With {.Text = f.Titulo, .Value = f.Id, .Group = groupActivos}).OrderBy(Function(f) f.Text).ToList())
            retosLI.AddRange(retos.Where(Function(f) f.FechaBaja <> DateTime.MinValue).Select(Function(f) New Mvc.SelectListItem With {.Text = f.Titulo, .Value = f.Id, .Group = groupInactivos}).OrderBy(Function(f) f.Text).ToList())

            If (objetivo IsNot Nothing AndAlso retosLI.Exists(Function(f) f.Value = objetivo.IdReto)) Then
                retosLI.First(Function(f) f.Value = objetivo.IdReto).Selected = True
            End If

            ViewData("Retos") = retosLI
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="objetivo"></param>
        ''' <param name="cargarElementoVacio"></param>
        ''' <param name="procesoDeBaja"></param>
        Private Sub CargarProcesos(Optional ByVal objetivo As ELL.Objetivo = Nothing, Optional ByVal cargarElementoVacio As Boolean = False, Optional ByVal procesoDeBaja As Boolean? = Nothing)
            Dim procesos As List(Of ELL.Proceso) = BLL.ProcesosBLL.CargarListado(RolActual.IdPlanta, procesoDeBaja:=procesoDeBaja).OrderBy(Function(f) f.Orden).ToList()
            Dim procesosLI As New List(Of Mvc.SelectListItem)
            Dim groupActivos = New SelectListGroup() With {.Name = Utils.Traducir("Activos")}
            Dim groupInactivos = New SelectListGroup() With {.Name = Utils.Traducir("Inactivos")}

            procesosLI.AddRange(procesos.Where(Function(f) f.FechaBaja = DateTime.MinValue).Select(Function(f) New Mvc.SelectListItem With {.Text = f.CodigoDescripcion, .Value = f.Id, .Group = groupActivos}).OrderBy(Function(f) f.Text).ToList())
            procesosLI.AddRange(procesos.Where(Function(f) f.FechaBaja <> DateTime.MinValue).Select(Function(f) New Mvc.SelectListItem With {.Text = f.CodigoDescripcion, .Value = f.Id, .Group = groupInactivos}).OrderBy(Function(f) f.Text).ToList())

            If (cargarElementoVacio) Then
                procesosLI.Insert(0, New SelectListItem With {.Text = String.Format("<{0}>", Utils.Traducir("Todos")), .Value = CStr(Integer.MinValue)})
            End If

            If (objetivo IsNot Nothing AndAlso procesosLI.Exists(Function(f) f.Value = objetivo.IdProceso)) Then
                procesosLI.First(Function(f) f.Value = objetivo.IdProceso).Selected = True
            End If

            ViewData("Procesos") = procesosLI
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idPlanta"></param>
        ''' <param name="cargarElementoVacio"></param>
        ''' <param name="procesoDeBaja"></param>
        Private Sub CargarProcesosDespliegue(ByVal idPlanta As Integer, Optional ByVal cargarElementoVacio As Boolean = False, Optional ByVal procesoDeBaja As Boolean? = Nothing)
            Dim procesos As List(Of ELL.Proceso) = BLL.ProcesosBLL.CargarListado(idPlanta, procesoDeBaja:=procesoDeBaja).OrderBy(Function(f) f.Orden).ToList()
            Dim procesosLI As New List(Of Mvc.SelectListItem)
            Dim groupActivos = New SelectListGroup() With {.Name = Utils.Traducir("Activos")}
            Dim groupInactivos = New SelectListGroup() With {.Name = Utils.Traducir("Inactivos")}

            procesosLI.AddRange(procesos.Where(Function(f) f.FechaBaja = DateTime.MinValue).Select(Function(f) New Mvc.SelectListItem With {.Text = f.CodigoDescripcion, .Value = f.Id, .Group = groupActivos}).OrderBy(Function(f) f.Text).ToList())
            procesosLI.AddRange(procesos.Where(Function(f) f.FechaBaja <> DateTime.MinValue).Select(Function(f) New Mvc.SelectListItem With {.Text = f.CodigoDescripcion, .Value = f.Id, .Group = groupInactivos}).OrderBy(Function(f) f.Text).ToList())

            If (cargarElementoVacio) Then
                procesosLI.Insert(0, New SelectListItem With {.Text = String.Format("<{0}>", Utils.Traducir("Todos")), .Value = CStr(Integer.MinValue)})
            End If

            ViewData("Procesos") = procesosLI
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="objetivo"></param>
        ''' <param name="cargarElementoVacio"></param>
        ''' <param name="cargarBajas"></param>
        Private Sub CargarResponsables(Optional ByVal objetivo As ELL.Objetivo = Nothing, Optional ByVal cargarElementoVacio As Boolean = False, Optional ByVal cargarBajas As Boolean = True)
            Dim responsables As List(Of ELL.UsuarioRol) = BLL.UsuariosRolBLL.CargarListado(idPlanta:=RolActual.IdPlanta, listaIdRoles:=New List(Of Integer) From {ELL.Rol.RolUsuario.Responsable, ELL.Rol.RolUsuario.Lider_de_objetivos}).GroupBy(Function(f) f.IdSab).Select(Function(f) f.First).OrderBy(Function(f) f.Nombre).ToList()

            If (Not cargarBajas) Then
                responsables = responsables.Where(Function(f) (f.FechaBaja = DateTime.MinValue OrElse f.FechaBaja >= DateTime.Today) AndAlso f.FechaBajaDOB = DateTime.MinValue AndAlso f.FechaBajaDOB = DateTime.MinValue).ToList()
            End If

            Dim responsablesLI As List(Of Mvc.SelectListItem) = responsables.Select(Function(f) New Mvc.SelectListItem With {.Text = f.NombreUsuario, .Value = f.IdSab}).OrderBy(Function(f) f.Text).ToList()
            If (cargarElementoVacio) Then
                responsablesLI.Insert(0, New SelectListItem With {.Text = String.Format("<{0}>", Utils.Traducir("Todos")), .Value = CStr(Integer.MinValue)})
            End If

            If (objetivo IsNot Nothing AndAlso responsablesLI.Exists(Function(f) f.Value = objetivo.IdResponsable)) Then
                responsablesLI.First(Function(f) f.Value = objetivo.IdResponsable).Selected = True
            End If

            ViewData("Responsables") = responsablesLI
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="objetivo"></param>
        ''' <param name="cargarElementoVacio"></param>
        ''' <param name="cargarBajas"></param>
        ''' <param name="ejercicio"></param>
        Private Sub CargarResponsablesConObjetivos(Optional ByVal objetivo As ELL.Objetivo = Nothing, Optional ByVal cargarElementoVacio As Boolean = False, Optional ByVal cargarBajas As Boolean = True, Optional ByVal ejercicio As Integer? = Nothing)
            Dim responsables As New List(Of ELL.UsuarioRol)

            'Cargamos los objetivos de ese año
            Dim objetivos As List(Of ELL.Objetivo) = BLL.ObjetivosBLL.CargarListado(RolActual.IdPlanta, ejercicio:=ejercicio)

            'Cargamos todos los responsables
            Dim responsablesAux As List(Of ELL.UsuarioRol) = BLL.UsuariosRolBLL.CargarListado(idPlanta:=RolActual.IdPlanta, listaIdRoles:=New List(Of Integer) From {ELL.Rol.RolUsuario.Responsable, ELL.Rol.RolUsuario.Lider_de_objetivos}).GroupBy(Function(f) f.IdSab).Select(Function(f) f.First).OrderBy(Function(f) f.Nombre).ToList()

            For Each objetivoAux In objetivos
                If (responsablesAux.Exists(Function(f) f.IdSab = objetivoAux.IdResponsable) AndAlso Not responsables.Exists(Function(f) f.IdSab = objetivoAux.IdResponsable)) Then
                    responsables.Add(responsablesAux.FirstOrDefault(Function(f) f.IdSab = objetivoAux.IdResponsable))
                End If
            Next

            If (Not cargarBajas) Then
                responsables = responsables.Where(Function(f) (f.FechaBaja = DateTime.MinValue OrElse f.FechaBaja >= DateTime.Today) AndAlso f.FechaBajaDOB = DateTime.MinValue AndAlso f.FechaBajaDOB = DateTime.MinValue).ToList()
            End If

            ViewData("ListaResponsables") = responsables
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="anyo"></param>
        ''' <returns></returns>
        Public Function CargarResponsablesAsJson(ByVal anyo As Integer) As JsonResult
            'Cargamos los objetivos de ese año
            Dim objetivos As List(Of ELL.Objetivo) = BLL.ObjetivosBLL.CargarListado(RolActual.IdPlanta, ejercicio:=anyo)

            'Cargamos todos los responsables
            Dim responsablesAux As List(Of ELL.UsuarioRol) = BLL.UsuariosRolBLL.CargarListado(idPlanta:=RolActual.IdPlanta, listaIdRoles:=New List(Of Integer) From {ELL.Rol.RolUsuario.Responsable, ELL.Rol.RolUsuario.Lider_de_objetivos}).GroupBy(Function(f) f.IdSab).Select(Function(f) f.First).OrderBy(Function(f) f.Nombre).ToList()
            Dim responsables As New List(Of ELL.UsuarioRol)

            For Each objetivo In objetivos
                If (responsablesAux.Exists(Function(f) f.IdSab = objetivo.IdResponsable) AndAlso Not responsables.Exists(Function(f) f.IdSab = objetivo.IdResponsable)) Then
                    responsables.Add(responsablesAux.FirstOrDefault(Function(f) f.IdSab = objetivo.IdResponsable))
                End If
            Next

            'Ordenamos primero responsables de alta y luego de baja
            Dim responsablesAlta As IEnumerable(Of ELL.UsuarioRol) = responsables.Where(Function(f) Not f.EsBaja).ToList().ConvertAll(Function(f) f)
            Dim responsablesBaja As IEnumerable(Of ELL.UsuarioRol) = responsables.Where(Function(f) f.EsBaja).ToList().ConvertAll(Function(f) f)

            responsables.Clear()
            responsables.AddRange(responsablesAlta)
            responsables.AddRange(responsablesBaja)

            Return Json(responsables, JsonRequestBehavior.AllowGet)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idPlantaDestino"></param>
        Private Sub CargarResponsablesDespliegue(ByVal idPlantaDestino)
            Dim responsables As List(Of ELL.UsuarioRol) = BLL.UsuariosRolBLL.CargarListado(idPlanta:=idPlantaDestino, listaIdRoles:=New List(Of Integer) From {ELL.Rol.RolUsuario.Lider_de_objetivos}).Where(Function(f) f.FechaBajaDOB = DateTime.MinValue AndAlso (f.FechaBaja = DateTime.MinValue OrElse f.FechaBaja > DateTime.Today)).GroupBy(Function(f) f.IdSab).Select(Function(f) f.First).OrderBy(Function(f) f.Nombre).ToList()
            Dim responsablesLI As List(Of Mvc.SelectListItem) = responsables.Select(Function(f) New Mvc.SelectListItem With {.Text = f.NombreUsuario, .Value = f.IdSab}).OrderBy(Function(f) f.Text).ToList()

            ViewData("Responsables") = responsablesLI
        End Sub

        '''' <summary>
        '''' 
        '''' </summary>
        '''' <param name="objetivo"></param>
        'Private Sub CargarMeses(Optional ByVal objetivo As ELL.Objetivo = Nothing)
        '    Dim mesesLI As New List(Of Mvc.SelectListItem)

        '    mesesLI.Add(New SelectListItem With {.Value = "1", .Text = Utils.Traducir("Enero")})
        '    mesesLI.Add(New SelectListItem With {.Value = "2", .Text = Utils.Traducir("Febrero")})
        '    mesesLI.Add(New SelectListItem With {.Value = "3", .Text = Utils.Traducir("Marzo")})
        '    mesesLI.Add(New SelectListItem With {.Value = "4", .Text = Utils.Traducir("Abril")})
        '    mesesLI.Add(New SelectListItem With {.Value = "5", .Text = Utils.Traducir("Mayo")})
        '    mesesLI.Add(New SelectListItem With {.Value = "6", .Text = Utils.Traducir("Junio")})
        '    mesesLI.Add(New SelectListItem With {.Value = "7", .Text = Utils.Traducir("Julio")})
        '    mesesLI.Add(New SelectListItem With {.Value = "8", .Text = Utils.Traducir("Agosto")})
        '    mesesLI.Add(New SelectListItem With {.Value = "9", .Text = Utils.Traducir("Septiembre")})
        '    mesesLI.Add(New SelectListItem With {.Value = "10", .Text = Utils.Traducir("Octubre")})
        '    mesesLI.Add(New SelectListItem With {.Value = "11", .Text = Utils.Traducir("Noviembre")})
        '    mesesLI.Add(New SelectListItem With {.Value = "12", .Text = Utils.Traducir("Diciembre")})

        '    If (objetivo IsNot Nothing AndAlso mesesLI.Exists(Function(f) f.Value = objetivo.MesObjetivo)) Then
        '        mesesLI.First(Function(f) f.Value = objetivo.MesObjetivo).Selected = True
        '    Else
        '        'Seleccionamos el último mes
        '        mesesLI.Last().Selected = True
        '    End If

        '    ViewData("Meses") = mesesLI
        'End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="objetivo"></param>
        Private Sub CargarAños(Optional ByVal objetivo As ELL.Objetivo = Nothing)
            Dim añosLI As New List(Of Mvc.SelectListItem)
            Dim añoActual As Integer = DateTime.Today.Year

            For a As Integer = añoActual To añoActual + 4
                añosLI.Add(New SelectListItem With {.Value = a, .Text = a})
            Next

            If (objetivo IsNot Nothing AndAlso añosLI.Exists(Function(f) f.Value = objetivo.AñoObjetivo)) Then
                añosLI.First(Function(f) f.Value = objetivo.AñoObjetivo).Selected = True
            Else
                ' Seleccionamos por defecto el año actual que es el primero que cargamos
                añosLI.First().Selected = True
            End If

            ViewData("Años") = añosLI
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="ejercicio"></param>
        Private Sub CargarEjercicios(Optional ByVal ejercicio As Integer? = Nothing)
            ' Tenemos que recuperar los distintos ejercicios para los cuales hay objetivos
            Dim ejercicios As List(Of Integer) = BLL.ObjetivosBLL.ObtenerEjercicios(RolActual.IdPlanta)

            Dim ejerciciosLI As New List(Of Mvc.SelectListItem)
            ejercicios.ForEach(Sub(s) ejerciciosLI.Add(New SelectListItem With {.Value = s, .Text = s}))

            ejerciciosLI.Insert(0, New SelectListItem With {.Text = String.Format("<{0}>", Utils.Traducir("Todos")), .Value = CStr(Integer.MinValue)})

            If (ejercicio IsNot Nothing AndAlso ejerciciosLI.Exists(Function(f) f.Value = ejercicio)) Then
                ejerciciosLI.First(Function(f) f.Value = ejercicio).Selected = True
            End If

            ViewData("Ejercicios") = ejerciciosLI
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="objetivo"></param>
        Private Sub CargarTiposIndicadores(Optional ByVal objetivo As ELL.Objetivo = Nothing)
            Dim tiposIndicadores As List(Of ELL.TipoIndicador) = BLL.TiposIndicadoresBLL.CargarListado().OrderBy(Function(f) f.DescripcionCompleta).ToList()
            Dim tiposIndicadoresLI As List(Of Mvc.SelectListItem) = tiposIndicadores.Select(Function(f) New Mvc.SelectListItem With {.Text = f.DescripcionCompleta, .Value = f.Id}).OrderBy(Function(f) f.Text).ToList()

            If (objetivo IsNot Nothing AndAlso tiposIndicadoresLI.Exists(Function(f) f.Value = objetivo.IdTipoIndicador)) Then
                tiposIndicadoresLI.First(Function(f) f.Value = objetivo.IdTipoIndicador).Selected = True
            End If

            ViewData("TiposIndicadores") = tiposIndicadoresLI
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="objetivo"></param>
        Private Sub CargarPeriodicidad(Optional ByVal objetivo As ELL.Objetivo = Nothing)
            Dim periodicidadLI As New List(Of Mvc.SelectListItem)

            For Each per In [Enum].GetValues(GetType(ELL.Objetivo.TipoPeriodicidad))
                periodicidadLI.Add(New Mvc.SelectListItem With {.Text = Utils.Traducir(per.ToString()), .Value = CInt(per)})
            Next

            periodicidadLI.OrderBy(Function(f) f.Value).ToList()

            If (objetivo IsNot Nothing AndAlso periodicidadLI.Exists(Function(f) f.Value = objetivo.Periodicidad)) Then
                periodicidadLI.First(Function(f) f.Value = objetivo.Periodicidad).Selected = True
            End If

            ViewData("Periodicidad") = periodicidadLI
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="tipoAgrupacion"></param>
        Private Sub CargarTiposAgrupacion(Optional tipoAgrupacion As Integer? = Nothing)
            Dim tiposAgrupacionLI As New List(Of Mvc.SelectListItem)

            For Each agr In [Enum].GetValues(GetType(ELL.Objetivo.TipoAgrupacion))
                tiposAgrupacionLI.Add(New Mvc.SelectListItem With {.Text = Utils.Traducir(agr.ToString()), .Value = CInt(agr)})
            Next

            tiposAgrupacionLI.OrderBy(Function(f) f.Value).ToList()

            tiposAgrupacionLI.Insert(0, New SelectListItem With {.Text = String.Format("<{0}>", Utils.Traducir("Ninguno")), .Value = CStr(Integer.MinValue)})

            If (tipoAgrupacion IsNot Nothing AndAlso tiposAgrupacionLI.Exists(Function(f) f.Value = tipoAgrupacion)) Then
                tiposAgrupacionLI.First(Function(f) f.Value = tipoAgrupacion).Selected = True
            Else
                ' Por defecto seleccionamos el primer valor
                tiposAgrupacionLI.First().Selected = True
            End If

            ViewData("TiposAgrupacion") = tiposAgrupacionLI
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="descripcion"></param>
        ''' <param name="Retos"></param>
        ''' <param name="Procesos"></param>
        ''' <param name="Responsables"></param>
        ''' <param name="fechaObjetivo"></param>
        ''' <param name="indicador"></param>
        ''' <param name="descripcionIndicador"></param>
        ''' <param name="TiposIndicadores"></param>
        ''' <param name="valorInicial"></param>
        ''' <param name="valorObjetivo"></param>
        ''' <param name="Periodicidad"></param>
        ''' <param name="sentido"></param>
        ''' <returns></returns>
        <HttpPost>
        Function Agregar(ByVal descripcion As String, ByVal Retos As Integer, ByVal Procesos As Integer, ByVal Responsables As Integer,
                         ByVal fechaObjetivo As DateTime, ByVal indicador As String, ByVal descripcionIndicador As String, ByVal TiposIndicadores As Integer,
                         ByVal valorInicial As String, ByVal valorObjetivo As String, ByVal Periodicidad As Integer, ByVal sentido As Integer) As ActionResult
            Ejercicio = fechaObjetivo.Year

            Dim valorInicialDec As Decimal = Decimal.MinValue
            Dim valorObjetivoDec As Decimal = Decimal.MinValue
            Dim esNegativo As Boolean = False

            esNegativo = valorInicial.StartsWith("-")
            If (esNegativo) Then
                valorInicial = valorInicial.Replace("-", String.Empty)
            End If

            Decimal.TryParse(valorInicial, NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture(Ticket.Culture), valorInicialDec)

            If (Not String.IsNullOrEmpty(valorInicial) AndAlso valorInicial <> "0" AndAlso valorInicialDec = Decimal.Zero) Then
                MensajeAlerta(String.Format("{0}. {1}", Utils.Traducir("Valor inicial"), Utils.Traducir("Formato incorrecto")))
                Return Agregar()
            End If

            If (esNegativo) Then
                valorInicialDec = valorInicialDec * -1
            End If

            esNegativo = valorObjetivo.StartsWith("-")
            If (esNegativo) Then
                valorObjetivo = valorObjetivo.Replace("-", String.Empty)
            End If

            Decimal.TryParse(valorObjetivo, NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture(Ticket.Culture), valorObjetivoDec)

            If (Not String.IsNullOrEmpty(valorObjetivo) AndAlso valorObjetivo <> "0" AndAlso valorObjetivoDec = Decimal.Zero) Then
                MensajeAlerta(String.Format("{0}. {1}", Utils.Traducir("Valor objetivo"), Utils.Traducir("Formato incorrecto")))
                Return Agregar()
            End If

            If (esNegativo) Then
                valorObjetivoDec = valorObjetivoDec * -1
            End If

            ' Vamos a ver si la planta tiene padre y si hereda los retos. En ese caso al objetivo le vamos a añadir el IdPlanta ya que si no como el reto está
            ' realmento asociado a la planta padre, el objetivo se asociará a esta y no a la hija
            Dim planta As ELL.Planta = BLL.PlantasBLL.ObtenerPlanta(RolActual.IdPlanta)
            Dim idPlanta As Integer = Integer.MinValue
            If (planta.IdPlantaPadre <> Integer.MinValue AndAlso planta.HeredaRetos) Then
                idPlanta = RolActual.IdPlanta
            End If

            Dim objetivo As New ELL.Objetivo With {.Descripcion = descripcion, .IdReto = Retos, .IdProceso = Procesos, .IdResponsable = Responsables, .FechaObjetivo = fechaObjetivo,
                                                   .NombreIndicador = indicador, .DescripcionIndicador = descripcionIndicador, .IdTipoIndicador = TiposIndicadores,
                                                   .ValorInicial = valorInicialDec, .ValorObjetivo = valorObjetivoDec, .Periodicidad = Periodicidad, .IdUsuarioAlta = Ticket.IdUser,
                                                   .IdPlanta = idPlanta, .Sentido = sentido}
            Try
                BLL.ObjetivosBLL.Guardar(objetivo)
                MensajeInfo(Utils.Traducir("Datos guardados correctamente"))
                Return RedirectToAction("Editar", New With {.idObjetivo = objetivo.Id})
            Catch ex As Exception
                MensajeError(Utils.Traducir("Se ha producido un error al guardar los datos"))
                Return Agregar()
            End Try
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idObjetivo"></param>
        ''' <param name="descripcion"></param>
        ''' <param name="Retos"></param>
        ''' <param name="Procesos"></param>
        ''' <param name="Responsables"></param>
        ''' <param name="fechaObjetivo"></param>
        ''' <param name="indicador"></param>
        ''' <param name="descripcionIndicador"></param>
        ''' <param name="TiposIndicadores"></param>
        ''' <param name="valorInicial"></param>
        ''' <param name="valorObjetivo"></param>
        ''' <param name="Periodicidad"></param>
        ''' <param name="sentido"></param>
        ''' <returns></returns>
        <HttpPost>
        Function Editar(ByVal idObjetivo As Integer, ByVal descripcion As String, ByVal Retos As Integer, ByVal Procesos As Integer, ByVal Responsables As Integer,
                        ByVal fechaObjetivo As DateTime, ByVal indicador As String, ByVal descripcionIndicador As String, ByVal TiposIndicadores As Integer,
                        ByVal valorInicial As String, ByVal valorObjetivo As String, ByVal Periodicidad As Integer, ByVal sentido As Integer) As ActionResult
            Ejercicio = fechaObjetivo.Year
            Dim valorInicialDec As Decimal = Decimal.MinValue
            Dim valorObjetivoDec As Decimal = Decimal.MinValue
            Dim esNegativo As Boolean = False

            esNegativo = valorInicial.StartsWith("-")
            If (esNegativo) Then
                valorInicial = valorInicial.Replace("-", String.Empty)
            End If

            Decimal.TryParse(valorInicial, NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture(Ticket.Culture), valorInicialDec)

            If (Not String.IsNullOrEmpty(valorInicial) AndAlso valorInicial <> "0" AndAlso valorInicialDec = Decimal.Zero) Then
                MensajeAlerta(String.Format("{0}. {1}", Utils.Traducir("Valor inicial"), Utils.Traducir("Formato incorrecto")))
                Return Editar(idObjetivo)
            End If

            If (esNegativo) Then
                valorInicialDec = valorInicialDec * -1
            End If

            esNegativo = valorObjetivo.StartsWith("-")
            If (esNegativo) Then
                valorObjetivo = valorObjetivo.Replace("-", String.Empty)
            End If

            Decimal.TryParse(valorObjetivo, NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture(Ticket.Culture), valorObjetivoDec)

            If (Not String.IsNullOrEmpty(valorObjetivo) AndAlso valorObjetivo <> "0" AndAlso valorObjetivoDec = Decimal.Zero) Then
                MensajeAlerta(String.Format("{0}. {1}", Utils.Traducir("Valor objetivo"), Utils.Traducir("Formato incorrecto")))
                Return Editar(idObjetivo)
            End If

            If (esNegativo) Then
                valorObjetivoDec = valorObjetivoDec * -1
            End If

            Dim objetivo As ELL.Objetivo = BLL.ObjetivosBLL.ObtenerObjetivo(idObjetivo)
            objetivo.Id = idObjetivo
            objetivo.Descripcion = descripcion
            objetivo.IdReto = Retos
            objetivo.IdProceso = Procesos
            objetivo.IdResponsable = Responsables
            objetivo.FechaObjetivo = fechaObjetivo
            objetivo.NombreIndicador = indicador
            objetivo.DescripcionIndicador = descripcionIndicador
            objetivo.IdTipoIndicador = TiposIndicadores
            objetivo.ValorInicial = valorInicialDec
            objetivo.ValorObjetivo = valorObjetivoDec
            objetivo.Periodicidad = Periodicidad
            objetivo.Sentido = sentido

            Try
                BLL.ObjetivosBLL.Guardar(objetivo)
                MensajeInfo(Utils.Traducir("Datos guardados correctamente"))
                Return RedirectToAction("Editar", New With {.idObjetivo = objetivo.Id})
            Catch ex As Exception
                MensajeError(Utils.Traducir("Se ha producido un error al guardar los datos"))
                Return Editar(idObjetivo)
            End Try
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Ejercicios"></param>
        ''' <param name="TiposAgrupacion"></param>
        ''' <param name="Responsables"></param>
        ''' <returns></returns>
        <HttpPost>
        Function CuadroMando(ByVal Ejercicios As Integer, ByVal TiposAgrupacion As Integer, ByVal Responsables As Integer) As ActionResult
            Ejercicio = Ejercicios

            ViewData("ContainerFluid") = 1
            CargarEjercicios(ejercicio:=Ejercicios)
            CargarTiposAgrupacion(TiposAgrupacion)
            CargarResponsablesConObjetivos(objetivo:=New ELL.Objetivo With {.IdResponsable = Responsables}, cargarElementoVacio:=True, ejercicio:=Ejercicio)

            Dim idResponsable As Integer? = Nothing
            If (Responsables <> Integer.MinValue) Then
                idResponsable = Responsables
                ViewData("IdResponsable") = idResponsable
            End If

            CargarObjetivos(ejercicio:=Ejercicios, tipoAgrupacion:=TiposAgrupacion, idResponsable:=idResponsable)

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="hIdObjetivo"></param>
        ''' <returns></returns>
        Function Desplegar(ByVal hIdObjetivo As Integer) As ActionResult
            Dim plantasHijasAux As New List(Of Integer)
            Dim plantasHijas As List(Of Integer) = Nothing
            Dim split As String()

            If (TempData("plantasHijas") IsNot Nothing) Then
                plantasHijasAux = TempData("plantasHijas")
            Else
                For Each key In Request.Params.AllKeys.Where(Function(f) f.StartsWith("chkBox-"))
                    If (Request.Params(key).Contains("true")) Then
                        split = key.Split("-")
                        plantasHijasAux.Add(split(1))
                    End If
                Next
            End If

            CargarObjetivo(hIdObjetivo)
            Dim objetivo As ELL.Objetivo = CType(ViewData("Objetivo"), ELL.Objetivo)

            CargarRetosDespliegue(objetivo, plantasHijasAux.First)
            CargarProcesosDespliegue(plantasHijasAux.First, procesoDeBaja:=False)
            CargarResponsablesDespliegue(plantasHijasAux.First)
            CargarTiposIndicadores(objetivo)
            CargarPeriodicidad(objetivo)

            ViewData("IdPlantaDespliegue") = plantasHijasAux.First
            plantasHijasAux.Remove(plantasHijasAux.First)
            ViewData("PlantasDespliegue") = plantasHijasAux

            Return View("Desplegar")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="hIdObjetivo"></param>
        ''' <param name="descripcion"></param>
        ''' <param name="Retos"></param>
        ''' <param name="Procesos"></param>
        ''' <param name="Responsables"></param>
        ''' <param name="valorInicial"></param>
        ''' <param name="valorObjetivo"></param>
        ''' <returns></returns>
        Function DesplegarAPlanta(ByVal hIdObjetivo As Integer, ByVal hIdPlantaDestino As Integer, ByVal descripcion As String, ByVal Retos As Integer?,
                                  ByVal Procesos As Integer, ByVal Responsables As Integer, ByVal valorInicial As String, ByVal valorObjetivo As String) As ActionResult
            ' Cargamos el objetivo padre ya que ciertos datos se heredan de él
            Dim objetivoPadre As ELL.Objetivo = BLL.ObjetivosBLL.ObtenerObjetivo(hIdObjetivo)

            Ejercicio = objetivoPadre.FechaObjetivo.Year

            Dim retoAux As Integer = Integer.MinValue
            If (Retos Is Nothing) Then
                retoAux = objetivoPadre.IdReto
            Else
                retoAux = Retos
            End If

            Dim valorInicialDec As Decimal = Decimal.MinValue
            Dim valorObjetivoDec As Decimal = Decimal.MinValue
            Dim esNegativo As Boolean = False

            esNegativo = valorInicial.StartsWith("-")
            If (esNegativo) Then
                valorInicial = valorInicial.Replace("-", String.Empty)
            End If

            Decimal.TryParse(valorInicial, NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture(Ticket.Culture), valorInicialDec)

            If (Not String.IsNullOrEmpty(valorInicial) AndAlso valorInicial <> "0" AndAlso valorInicialDec = Decimal.Zero) Then
                MensajeAlerta(String.Format("{0}. {1}", Utils.Traducir("Valor inicial"), Utils.Traducir("Formato incorrecto")))
                'Return Agregar()
            End If

            If (esNegativo) Then
                valorInicialDec = valorInicialDec * -1
            End If

            esNegativo = valorObjetivo.StartsWith("-")
            If (esNegativo) Then
                valorObjetivo = valorObjetivo.Replace("-", String.Empty)
            End If

            Decimal.TryParse(valorObjetivo, NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture(Ticket.Culture), valorObjetivoDec)

            If (Not String.IsNullOrEmpty(valorObjetivo) AndAlso valorObjetivo <> "0" AndAlso valorObjetivoDec = Decimal.Zero) Then
                MensajeAlerta(String.Format("{0}. {1}", Utils.Traducir("Valor objetivo"), Utils.Traducir("Formato incorrecto")))
                'Return Agregar()
            End If

            If (esNegativo) Then
                valorObjetivoDec = valorObjetivoDec * -1
            End If

            ' Vamos a ver si la planta tiene padre y si hereda los retos. En ese caso al objetivo le vamos a añadir el IdPlanta ya que si no como el reto está
            ' realmento asociado a la planta padre, el objetivo se asociará a esta y no a la hija
            Dim planta As ELL.Planta = BLL.PlantasBLL.ObtenerPlanta(hIdPlantaDestino)
            Dim idPlanta As Integer = Integer.MinValue
            If (planta.IdPlantaPadre <> Integer.MinValue AndAlso planta.HeredaRetos) Then
                idPlanta = hIdPlantaDestino
            End If

            Dim objetivo As New ELL.Objetivo With {.Descripcion = descripcion, .IdReto = retoAux, .IdProceso = Procesos, .IdResponsable = Responsables, .FechaObjetivo = objetivoPadre.FechaObjetivo,
                                                   .NombreIndicador = objetivoPadre.NombreIndicador, .DescripcionIndicador = objetivoPadre.DescripcionIndicador, .IdTipoIndicador = objetivoPadre.IdTipoIndicador,
                                                   .ValorInicial = valorInicialDec, .ValorObjetivo = valorObjetivoDec, .Periodicidad = objetivoPadre.Periodicidad, .IdUsuarioAlta = Ticket.IdUser,
                                                   .IdPlanta = idPlanta, .Sentido = objetivoPadre.Sentido, .IdObjetivoPadre = hIdObjetivo}
            Try
                BLL.ObjetivosBLL.Guardar(objetivo)
                MensajeInfo(Utils.Traducir("Datos guardados correctamente"))

                'Aqui pueden darse dos situaciones. Que queden más plantas de las que desplegar o no
                Dim plantasHijas As New List(Of Integer)
                Dim split As String()

                For Each key In Request.Params.AllKeys.Where(Function(f) f.StartsWith("chkBox-"))
                    If (Request.Params(key).Contains("true")) Then
                        split = key.Split("-")
                        plantasHijas.Add(split(1))
                    End If
                Next

                If (plantasHijas.Count > 0) Then
                    TempData("plantasHijas") = plantasHijas
                    Return RedirectToAction("Desplegar", New With {.hIdObjetivo = hIdObjetivo})
                Else
                    Return RedirectToAction("Index", "Login")
                End If

                'Return RedirectToAction("Editar", New With {.idObjetivo = objetivo.Id})
            Catch ex As Exception
                MensajeError(Utils.Traducir("Se ha producido un error al guardar los datos"))
                'Return Agregar()
            End Try
        End Function

    End Class
End Namespace