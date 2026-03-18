Imports System.Web.Mvc

Namespace Controllers
    Public Class ValidacionesController
        Inherits BaseController

#Region "Propiedades"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overrides ReadOnly Property RolesAcceso As List(Of ELL.Rol.TipoRol)
            Get
                Dim roles As New List(Of ELL.Rol.TipoRol)
                roles.Add(ELL.Rol.TipoRol.Responsable_ingenieria_planta)
                roles.Add(ELL.Rol.TipoRol.Gerente_planta)
                roles.Add(ELL.Rol.TipoRol.Project_manager)
                roles.Add(ELL.Rol.TipoRol.Responsable_advance)
                roles.Add(ELL.Rol.TipoRol.Direccion_CMP)
                roles.Add(ELL.Rol.TipoRol.Direccion_operaciones)
                roles.Add(ELL.Rol.TipoRol.Product_manager)
                roles.Add(ELL.Rol.TipoRol.Direccion_industrial)
                roles.Add(ELL.Rol.TipoRol.Director_técnico)
                Return roles
            End Get
        End Property

#End Region

#Region "Métodos"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Function Index() As ActionResult
            CargarCabeceras()

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Function IndexHistorico() As ActionResult
            CargarEstadosCostCarrierFiltro(Integer.MinValue)
            CargarProductosFiltro(String.Empty)
            CargarCabecerasHistorico(Integer.MinValue)

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="EstadoCostCarrier"></param>
        ''' <param name="Productos"></param>
        ''' <returns></returns>
        <HttpPost>
        Function IndexHistorico(ByVal EstadoCostCarrier As Integer, ByVal Productos As String) As ActionResult
            CargarEstadosCostCarrierFiltro(EstadoCostCarrier)
            CargarProductosFiltro(Productos)
            CargarCabecerasHistorico(EstadoCostCarrier, Productos)

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idCabecera"></param>
        ''' <returns></returns>
        Function DetalleProyecto(ByVal idCabecera As Integer) As ActionResult
            CargarCabecera(idCabecera)
            CargarValidaciones(idCabecera)
            CargarStepsAGestionarPorCabecera(idCabecera)

            ViewData("ContainerFluid") = 1
            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idCabecera"></param>
        ''' <param name="idValidacion"></param>
        ''' <returns></returns>
        Function DetalleProyectoHistorico(ByVal idCabecera As Integer, Optional ByVal idValidacion As String = Nothing) As ActionResult
            CargarCabecera(idCabecera)
            CargarTags(idCabecera, idValidacion)

            If (Not String.IsNullOrEmpty(idValidacion)) Then
                CargarValidacion(idValidacion)
                ' Los datos están ya agrupados por costgroup. Para el tema del serial tooling no me sirve
                CargarValidacionesLineaAgrupados(idValidacion)
            End If

            ViewData("ContainerFluid") = 1
            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idCostGroup"></param>
        ''' <returns></returns>
        Function DetalleCostGroup(ByVal idCostGroup As String) As ActionResult
            Dim listaIdCostGroups As List(Of String) = idCostGroup.Split(";").Where(Function(f) Not String.IsNullOrEmpty(f)).ToList()

            ' Vamos a ver que tipo es si batz o customer
            Dim steps As New List(Of ELL.Step)
            Dim stepsAux As New List(Of ELL.Step)
            Dim res As Integer

            ' Con el tryparse evitamos los valores que no sean integer como las palabras "batz" o "customer"
            For Each idCostGroup In listaIdCostGroups.Where(Function(f) Int32.TryParse(f, res))
                CargarStepsAGestionar(CInt(idCostGroup))
                steps.AddRange(ViewData("Steps"))
            Next

            steps.ForEach(Sub(s) s.CargarValoresStep())

            Dim idCabecera As Integer = BLL.CostsGroupBLL.Obtener(idCostGroup).IdCabecera
            CargarCabecera(idCabecera)
            Dim cabecera As ELL.CabeceraCostCarrier = ViewData("CabeceraProyecto")
            Dim descripcionCostGroup As String = String.Empty

            If (listaIdCostGroups.Contains("batz")) Then
                steps = steps.Where(Function(f) f.PBC = 0).ToList()
                descripcionCostGroup = CType(ELL.CostGroupOT.Agrupacion.Serial_tooling, ELL.CostGroupOT.Agrupacion).ToString().ToUpper().Replace("_", " ") & " BATZ"
            ElseIf (listaIdCostGroups.Contains("customer")) Then
                steps = steps.Where(Function(f) f.PBC > 0).ToList()
                descripcionCostGroup = CType(ELL.CostGroupOT.Agrupacion.Serial_tooling, ELL.CostGroupOT.Agrupacion).ToString().ToUpper().Replace("_", " ") & " CUSTOMER"
            Else
                descripcionCostGroup = BLL.CostsGroupBLL.Obtener(steps.First.IdCostGroup).Descripcion
            End If

            ViewData("Steps") = steps
            ViewData("MostrarBotones") = steps.Exists(Function(f) f.IdEstadoValidacion = ELL.Validacion.Estado.Waiting_for_approval)

            ViewData("Titulo") = String.Format("{0} - {1} ({2} / {3} / {4})", Utils.Traducir("Pasos pendientes"), cabecera.NombreProyecto, steps.First().Planta, steps.First().Estado, descripcionCostGroup)

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idCostGroup"></param>
        ''' <param name="idPaso"></param>
        ''' <param name="idValidacion"></param>
        ''' <returns></returns>
        Function DetalleCostGroupHistorico(ByVal idCostGroup As String, Optional idPaso As Integer = Nothing, Optional ByVal idValidacion As Integer? = Nothing) As ActionResult
            Dim listaIdCostGroups As List(Of String) = idCostGroup.Split(";").Where(Function(f) Not String.IsNullOrEmpty(f)).ToList()
            Dim validacionesLinea As New List(Of ELL.ValidacionLinea)
            Dim res As Integer

            ' Con el tryparse evitamos los valores que no sean integer como las palabras "batz" o "customer"
            For Each idCostGroup In listaIdCostGroups.Where(Function(f) Int32.TryParse(f, res))
                If (idValidacion Is Nothing) Then
                    CargarValidacionesLineaPorCostGroup(idCostGroup, CInt(idPaso))
                Else
                    CargarValidacionesLinea(CInt(idValidacion), idCostGroup)
                End If

                validacionesLinea.AddRange(ViewData("ValidacionesLinea"))
            Next

            Dim idCabecera As Integer = BLL.CostsGroupBLL.Obtener(idCostGroup).IdCabecera
            CargarCabecera(idCabecera)
            Dim cabecera As ELL.CabeceraCostCarrier = ViewData("CabeceraProyecto")

            Dim descripcionCostGroup As String = String.Empty
            If (listaIdCostGroups.Contains("batz")) Then
                validacionesLinea = validacionesLinea.Where(Function(f) f.PaidByCustomer = 0).ToList()
                descripcionCostGroup = CType(ELL.CostGroupOT.Agrupacion.Serial_tooling, ELL.CostGroupOT.Agrupacion).ToString().ToUpper().Replace("_", " ") & " BATZ"
            ElseIf (listaIdCostGroups.Contains("customer")) Then
                validacionesLinea = validacionesLinea.Where(Function(f) f.PaidByCustomer > 0).ToList()
                descripcionCostGroup = CType(ELL.CostGroupOT.Agrupacion.Serial_tooling, ELL.CostGroupOT.Agrupacion).ToString().ToUpper().Replace("_", " ") & " CUSTOMER"
            Else
                descripcionCostGroup = BLL.CostsGroupBLL.Obtener(idCostGroup).Descripcion
            End If

            Dim paso As ELL.Step = BLL.StepsBLL.Obtener(validacionesLinea.First.IdStep)

            ViewData("ValidacionesLinea") = validacionesLinea
            ViewData("Titulo") = String.Format("{0} - {1} ({2} / {3} / {4}", Utils.Traducir("Estado actual e histórico pasos"), cabecera.NombreProyecto, Utils.Traducir(paso.Planta), Utils.Traducir(paso.Estado), descripcionCostGroup)

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="txtMotivo"></param>
        ''' <param name="hIdValidacionesLineaRechaz"></param>
        ''' <returns></returns>
        <HttpPost>
        Function Rechazar(ByVal txtMotivo As String, ByVal hIdValidacionesLineaRechaz As String) As ActionResult
            Dim idCostGroup As Integer = Integer.MinValue
            Try
                Dim listaIdValidacionesLinea As List(Of String) = hIdValidacionesLineaRechaz.Split("-").Where(Function(f) Not String.IsNullOrEmpty(f)).ToList()
                Dim validacionLinea As ELL.ValidacionLinea
                Dim paso As ELL.Step

                For Each idValidacionLinea In listaIdValidacionesLinea.Where(Function(f) Not String.IsNullOrEmpty(f))
                    BLL.ValidacionesLineaBLL.AñadirEstadoValidacion(idValidacionLinea, Ticket.IdUser, txtMotivo, ELL.Validacion.Estado.Rejected, ELL.Validacion.Accion.Reject, Integer.MinValue)

                    'Borramos el flujo de aprobación para esa validación linea
                    BLL.FlujosAprobacionBLL.Eliminar(idValidacionLinea)

                    'Se envía el mail al usuario para indicarle que su paso ha sido rechazado
                    validacionLinea = BLL.ValidacionesLineaBLL.Obtener(idValidacionLinea)
                    paso = BLL.StepsBLL.Obtener(validacionLinea.IdStep)

                    If (idCostGroup = Integer.MinValue) Then
                        idCostGroup = paso.IdCostGroup
                    End If

                    EnviarEmailUsuarioRechazo(paso, validacionLinea.Id, txtMotivo)
                Next

                MensajeInfo(Utils.Traducir("Paso/s rechazados correctamente"))

                ' Al rechazar tenemos que ver si para ese proyecto le queda algo por validar
                ' Si le queda algo volvemos al detalle del proyecto sino a la página inial de gestión de validaciones
                ' Cargamos los steps en estado pendiente de validar
                Dim algoParaValidar As Boolean = BLL.FlujosAprobacionBLL.CargarListadoPorUsuario(Ticket.IdUser).Where(Function(f) f.IdCabecera = paso.IdCabecera).Count > 0

                If (algoParaValidar) Then
                    Return RedirectToAction("DetalleProyecto", New With {.idCabecera = paso.IdCabecera})
                Else
                    Return RedirectToAction("Index")
                End If
            Catch ex As Exception
                log.Error("Error al rechazar el/los paso/s", ex)
                MensajeError(Utils.Traducir("Error al rechazar el/los paso/s"))
            End Try

            Return RedirectToAction("DetalleProyecto", New With {.idCostGroup = idCostGroup})
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="txtMotivo"></param>
        ''' <param name="hIdCostGroupRechazar"></param>
        ''' <param name="hIdPlantaRechazar"></param>
        ''' <returns></returns>
        <HttpPost>
        Function RechazarCostGroup(ByVal txtMotivo As String, ByVal hIdCostGroupRechazar As String, ByVal hIdPlantaRechazar As String) As ActionResult
            Dim idCabecera As Integer = Integer.MinValue
            Try
                Dim flujosAprobacion As List(Of ELL.FlujoAprobacion) = BLL.FlujosAprobacionBLL.CargarListadoPorUsuario(Ticket.IdUser)
                Dim flujosAprobacionAux As New List(Of ELL.FlujoAprobacion)

                If (Not String.IsNullOrEmpty(hIdCostGroupRechazar)) Then
                    Dim listaIdCostGroups As List(Of String) = hIdCostGroupRechazar.Split(";").Where(Function(f) Not String.IsNullOrEmpty(f)).ToList()

                    idCabecera = BLL.CostsGroupBLL.Obtener(listaIdCostGroups.First).IdCabecera
                    For Each idCostGroup In listaIdCostGroups
                        flujosAprobacionAux.AddRange(flujosAprobacion.Where(Function(f) f.IdCostGroup = idCostGroup))
                    Next
                ElseIf (Not String.IsNullOrEmpty(hIdPlantaRechazar)) Then
                    ' Con la planta vamos a cargar lost costgroups
                    Dim listCostGroups As List(Of ELL.CostGroup) = BLL.CostsGroupBLL.CargarListadoPorPlanta(hIdPlantaRechazar)

                    For Each costGroup In listCostGroups
                        idCabecera = costGroup.IdCabecera
                        flujosAprobacionAux.AddRange(flujosAprobacion.Where(Function(f) f.IdCostGroup = costGroup.Id))
                    Next
                End If

                For Each flujoAprobacion In flujosAprobacionAux
                    BLL.ValidacionesLineaBLL.AñadirEstadoValidacion(flujoAprobacion.IdValidacionLinea, Ticket.IdUser, txtMotivo, ELL.Validacion.Estado.Rejected, ELL.Validacion.Accion.Reject, Integer.MinValue)

                    'Borramos el flujo de aprobación para esa validación linea
                    BLL.FlujosAprobacionBLL.Eliminar(flujoAprobacion.IdValidacionLinea)

                    'Se envía el mail al usuario para indicarle que su paso ha sido rechazado
                    EnviarEmailUsuarioRechazo(BLL.StepsBLL.Obtener(flujoAprobacion.IdStep), flujoAprobacion.IdValidacionLinea, txtMotivo)
                Next

                MensajeInfo(Utils.Traducir("Paso/s rechazados correctamente"))

                ' Al rechazar tenemos que ver si para ese proyecto le queda algo por validar
                ' Si le queda algo volvemos al detalle del proyecto sino a la página inial de gestión de validaciones
                Dim algoParaValidar As Boolean = BLL.FlujosAprobacionBLL.CargarListadoPorUsuario(Ticket.IdUser).Where(Function(f) f.IdCabecera = idCabecera).Count > 0

                If (algoParaValidar) Then
                    Return RedirectToAction("DetalleProyecto", New With {.idCabecera = idCabecera})
                Else
                    Return RedirectToAction("Index")
                End If
            Catch ex As Exception
                log.Error("Error al rechazar el/los paso/s", ex)
                MensajeError(Utils.Traducir("Error al rechazar el/los paso/s"))
            End Try

            Return RedirectToAction("DetalleProyecto", New With {.idCabecera = idCabecera})
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="hIdValidacionesLineaAprob"></param>
        ''' <returns></returns>
        <HttpPost>
        Function Aprobar(ByVal hIdValidacionesLineaAprob As String) As ActionResult
            Dim idCostGroup As Integer = Integer.MinValue
            Try
                Dim listaIdValidacionesLinea As List(Of String) = hIdValidacionesLineaAprob.Split("-").Where(Function(f) Not String.IsNullOrEmpty(f)).ToList()
                Dim validacionLinea As ELL.ValidacionLinea = BLL.ValidacionesLineaBLL.Obtener(listaIdValidacionesLinea.FirstOrDefault(Function(f) Not String.IsNullOrEmpty(f)))
                Dim paso As ELL.Step = BLL.StepsBLL.Obtener(validacionLinea.IdStep)
                Dim cabecera As ELL.CabeceraCostCarrier = BLL.CabecerasCostCarrierBLL.Obtener(paso.IdCabecera, False)

                ' Tenemos que comprobar primero si de todos los pasos de un proyecto hay asignado cost carrier.
                ' Si existen algunos cogemos el más grande y le sumamos uno
                ' Si no existen cogemos el correlativo_cc más grande y le sumamos uno
                Dim stepsProyectos As New List(Of ELL.Step)

                '************************ RFC0155

                If (cabecera.TipoProyPtksis = ELL.CabeceraCostCarrier.TIPO_PROY_ECO_BATZ) Then
                    ' Si es un proyecto de tipo ECO
                    If (String.IsNullOrEmpty(cabecera.CodigoProyecto)) Then
                        ' Si no tiene código de proyecto
                        ' Comprobamos los project affected
                        Dim proyectosAfectados As List(Of ELL.ProjectAffected) = BLL.ProjectsAffectedBLL.ObtenerProyectosAfectados(cabecera.Proyecto)

                        If (proyectosAfectados.Count = 1 OrElse proyectosAfectados.Select(Function(f) f.Portador.ToUpper()).Distinct.Count = 1) Then
                            If (BLL.CabecerasCostCarrierBLL.Existe(proyectosAfectados.FirstOrDefault().Portador)) Then
                                ' Si el proyecto afectado existe en la aplicacion de cost carriers

                                ' Si sólo hay un project affected o si hay varios pero el código de proyecto es el mismo
                                ' Asignamos el código de proyecto
                                BLL.CabecerasCostCarrierBLL.CambiarCodigoProyecto(cabecera.Id, proyectosAfectados.First.Portador.ToUpper())
                                ' Miramos en todos los pasos de los proyectos cuyo codigo de proyecto coincida con el raiz p.e P162
                                stepsProyectos = BLL.StepsBLL.CargarListadoPorCodigoProyecto(cabecera.CodigoProyecto)
                            Else
                                ' Financiero tendrá que asignar un nuevo portador como hasta ahora
                                stepsProyectos = BLL.StepsBLL.CargarListadoPorCabecera(paso.IdCabecera)
                            End If
                        Else
                            ' Hay más de un project affected con códigos de proyecto diferentes. Financiero tendrá que asignar un nuevo portador como hasta ahora
                            stepsProyectos = BLL.StepsBLL.CargarListadoPorCabecera(paso.IdCabecera)
                        End If
                    Else
                        ' Si tiene código de proyecto 
                        ' Miramos en todos los pasos de los proyectos cuyo codigo de proyecto coincida con el raiz p.e P162
                        stepsProyectos = BLL.StepsBLL.CargarListadoPorCodigoProyecto(cabecera.CodigoProyecto)
                    End If
                Else
                    ' Si NO es un proyecto de tipo ECO
                    stepsProyectos = BLL.StepsBLL.CargarListadoPorCabecera(paso.IdCabecera)
                End If

                '********************************

                Dim correlativo As Integer = 0
                If (stepsProyectos.Exists(Function(f) Not String.IsNullOrEmpty(f.CostCarrier))) Then
                    ' Hay algún paso con cost carrier
                    ' Cogemos el más grande y le sumamos uno
                    correlativo = CInt(stepsProyectos.Max(Function(f) f.CostCarrier).ToUpper().Replace(cabecera.CodigoProyecto.ToUpper, String.Empty)) + 1

                    'Aqui puede darse el caso de que si haya ya cost carriers asignados pero tambien correlativos que no se hayan abierto.
                    'Habría que coger el máximo de los dos.
                    If stepsProyectos.Exists(Function(f) f.CorrelativoCC <> Integer.MinValue) Then
                        ' Buscamos sus códigos correlativos cogemos el más grande y le sumamos uno
                        Dim correlativoAux As Integer = stepsProyectos.Max(Function(f) f.CorrelativoCC) + 1

                        'Ahora tenemos que quedarnos con el más grande
                        correlativo = Math.Max(correlativo, correlativoAux)
                    End If
                ElseIf stepsProyectos.Exists(Function(f) f.CorrelativoCC <> Integer.MinValue) Then
                    ' No hay ningún paso con cost carrier
                    ' Buscamos sus códigos correlativos cogemos el más grande y le sumamos uno
                    correlativo = stepsProyectos.Max(Function(f) f.CorrelativoCC) + 1
                Else
                    ' Si no tiene cost carrier ni correlativos significa que es el primero y le damos el valor 1
                    correlativo = 1
                End If

                Dim listaPasosEnviarMailFinanciero As New List(Of ELL.Step)
                Dim listaPasosEnviarUsuario As New List(Of ELL.Step)
                Dim listaIdValidacionesLineaAprobados As New List(Of String)
                Dim flujosAprobacionCompleto As List(Of ELL.FlujoAprobacion)
                For Each idValidacionLinea In listaIdValidacionesLinea.Where(Function(f) Not String.IsNullOrEmpty(f))
                    validacionLinea = BLL.ValidacionesLineaBLL.Obtener(idValidacionLinea)
                    paso = BLL.StepsBLL.Obtener(validacionLinea.IdStep)

                    If (idCostGroup = Integer.MinValue) Then
                        idCostGroup = paso.IdCostGroup
                    End If

                    ' La aprobación final se realiza en N pasos
                    ' Si sólo queda una aprobación pasa a estado aprobado
                    ' Si queda más de una aprobación se queda en estado waiting for approval
                    flujosAprobacionCompleto = BLL.FlujosAprobacionBLL.CargarListadoPorIdCabecera(paso.IdCabecera).Where(Function(f) f.IdValidacionLinea = idValidacionLinea).ToList()

                    If (flujosAprobacionCompleto.Count > 1) Then
                        ' Si hay más de uno se queda en waiting for approval
                        BLL.ValidacionesLineaBLL.AñadirEstadoValidacion(idValidacionLinea, Ticket.IdUser, String.Empty, ELL.Validacion.Estado.Waiting_for_approval, ELL.Validacion.Accion.Approve, flujosAprobacionCompleto.OrderBy(Function(f) f.Orden).First.Orden)

                        ' Se envía un mail al siguiente validador
                        Try
                            EnviarEmailValidador(flujosAprobacionCompleto.OrderBy(Function(f) f.Orden)(1).Email)
                        Catch ex As Exception
                            log.Error("Error al enviar mail al validador")
                        End Try

                        listaPasosEnviarUsuario.Add(paso)
                    ElseIf (flujosAprobacionCompleto.Count = 1) Then
                        listaIdValidacionesLineaAprobados.Add(idValidacionLinea)

                        ' Si sólo queda uno pasa a estado approved
                        BLL.ValidacionesLineaBLL.AñadirEstadoValidacion(idValidacionLinea, Ticket.IdUser, String.Empty, ELL.Validacion.Estado.Approved, ELL.Validacion.Accion.Approve, flujosAprobacionCompleto.OrderBy(Function(f) f.Orden).First.Orden)

                        '' Si el paso es de Zamudio como no se van a abrir lo que hacemos es directamente marcarlo como abierto 
                        'If (paso.IdPlantaSAB = ELL.Planta.PLANTA_SAB_ZAMUDIO) Then
                        '    Dim usuarioRol As ELL.UsuarioRol = BLL.UsuariosRolBLL.CargarListadoPorPlanta(ELL.Planta.PLANTA_SAB_ZAMUDIO).FirstOrDefault(Function(f) f.IdRol = ELL.Rol.TipoRol.Financiero)

                        '    ' Aquí le vamos a pasar la fecha/hora con 10 minutos más porque como hemos añadido la acción de aprobación justo antes se no se nos solapa las fecha/hora y a la hora de seleccionar
                        '    ' el estado actual de un step este nos hace que falle 
                        '    BLL.ValidacionesLineaBLL.AñadirEstadoValidacion(idValidacionLinea, usuarioRol.IdSab, "Abierto automáticamente", ELL.Validacion.Estado.Opened, ELL.Validacion.Accion.Open, Integer.MinValue, DateTime.Now.AddMinutes(10))
                        'Else
                        ' Si el paso de info general no hay que enviar ni a financiero ni actualizar los datos de XPERT
                        If (Not paso.EsInfoGeneral) Then
                            If (String.IsNullOrEmpty(paso.CostCarrier)) Then
                                'Vamos a agrupar para que no se envíen demasiados mails
                                listaPasosEnviarMailFinanciero.Add(paso)
                                BLL.StepsBLL.GuardarCorrelativo(paso.Id, correlativo)
                                correlativo += 1
                            Else
                                ' Actualizar los metadatos del paso
                                Dim plantasBLL As New SabLib.BLL.PlantasComponent
                                Dim planta As SabLib.ELL.Planta = plantasBLL.GetPlanta(If(paso.IdPlantaSAB = 0, 1, paso.IdPlantaSAB))

                                Try
                                    BLL.BRAIN.CCMetadataBLL.Actualizar(planta.IdBrain, paso.CostCarrier, validacionLinea, paso)
                                Catch ex As Exception
                                    ' Si falla la actualización de los metadatos volvemos para atrás y eliminamos el estado de validación y damos de alta la entrada eliminada en el flujo
                                    BLL.ValidacionesLineaBLL.EliminarEstadoValidacion(idValidacionLinea, Ticket.IdUser, ELL.Validacion.Estado.Approved, ELL.Validacion.Accion.Approve, flujosAprobacionCompleto.OrderBy(Function(f) f.Orden).First.Orden, flujosAprobacionCompleto.OrderBy(Function(f) f.Orden).First.Porcentaje)
                                    log.Error("Error al actualizar los metadatos para la planta " & planta.IdBrain & ", costcarrier " & paso.CostCarrier & " y validación línea " & validacionLinea.Id, ex)
                                    Throw ex
                                End Try
                            End If
                        End If
                        'End If
                    End If
                Next

                MensajeInfo(Utils.Traducir("Paso/s aprobados correctamente"))

                If (listaPasosEnviarMailFinanciero.Count > 0) Then
                    EnviarEmailFinanciero(listaPasosEnviarMailFinanciero)

                    ' Para obtener el mail de usuario que realizó el envío tenemos que navegador por el histórico de estados y buscar el primer estado waiting for aproval
                    Dim historicoEstado As ELL.HistoricoEstadoLinea = BLL.HistoricosEstadoLineaBLL.CargarListadoPorValidacionLinea(listaIdValidacionesLinea(0)).OrderBy(Function(f) f.Fecha).FirstOrDefault()
                    If (historicoEstado IsNot Nothing) Then
                        EnviarEmailUsuario(listaPasosEnviarMailFinanciero, historicoEstado.EmailUsuario, False)
                    End If
                End If

                ' En cada aprobación quieren un envio al usuario. Aprobaciones parciales
                If (listaPasosEnviarUsuario.Count > 0) Then
                    ' Para obtener el mail de usuario que realizó el envío tenemos que navegador por el histórico de estados y buscar el primer estado waiting for aproval
                    Dim historicoEstado As ELL.HistoricoEstadoLinea = BLL.HistoricosEstadoLineaBLL.CargarListadoPorValidacionLinea(listaIdValidacionesLinea(0)).OrderBy(Function(f) f.Fecha).FirstOrDefault()
                    If (historicoEstado IsNot Nothing) Then
                        EnviarEmailUsuario(listaPasosEnviarUsuario, historicoEstado.EmailUsuario, True)
                    End If
                End If

                ' Al aprobar tenemos que ver si para ese proyecto le queda algo por validar
                ' Si le queda algo volvemos al detalle del proyecto sino a la página inial de gestión de validaciones
                Dim algoParaValidar As Boolean = BLL.FlujosAprobacionBLL.CargarListadoPorUsuario(Ticket.IdUser).Where(Function(f) f.IdCabecera = paso.IdCabecera).Count > 0

                If (algoParaValidar) Then
                    Return RedirectToAction("DetalleProyecto", New With {.idCabecera = paso.IdCabecera})
                Else
                    Return RedirectToAction("Index")
                End If
            Catch ex As Exception
                log.Error("Error al aprobar el/los paso/s. Validaciones línea: " & hIdValidacionesLineaAprob, ex)
                MensajeError(Utils.Traducir("Error al aprobar el/los paso/s"))
            End Try

            Return RedirectToAction("DetalleCostGroup", New With {.idCostGroup = idCostGroup})
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="email"></param>
        Private Sub EnviarEmailValidador(ByVal email As String)
            Try
                Dim body As String = Utils.LeerFicheroTexto(Server.MapPath("~\Content\PlantillasMail\StepsParaValidar.html"))
                Dim mailFrom As String = ConfigurationManager.AppSettings("MailFrom")
                Dim uri = Url.Action("Index", "Validaciones", Nothing, Request.Url.Scheme)
                Dim subject As String = Utils.Traducir("Nuevos pasos para validar")

                Dim paramBLL As New SabLib.BLL.ParametrosBLL
                Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor
                body = String.Format(body, uri)

#If Not DEBUG Then
                Dim notificacionesActivas As Boolean = CBool(ConfigurationManager.AppSettings("NotificacionesActivas"))

                If (notificacionesActivas) Then
                    If (Not String.IsNullOrEmpty(email)) Then
                        SabLib.BLL.Utils.EnviarEmail(mailFrom, email, subject, body, serverEmail)
                        log.Info("Enviado mail al validador " & email)
                    Else
                        log.Info("EnviarEmailValidador - Email vacio")
                    End If
                End If
#End If
            Catch ex As Exception
                log.Error("Se ha producido un error al enviar el mail al validador", ex)
                Throw ex
            End Try
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        <HttpPost>
        Function AprobarCostGroup() As ActionResult
            Dim idCabecera As Integer = Integer.MinValue
            Dim hfIdCostGroup As String = String.Empty
            Dim hfIdPlanta As String = String.Empty

            Try
                ' Recuperamos el id del cost group
                Dim key As String = Request.Params.AllKeys.FirstOrDefault(Function(f) f.StartsWith("hfIdCostGroup-"))
                hfIdCostGroup = Request.Params.Get(key)

                ' Recuperamos el id de la planta
                key = Request.Params.AllKeys.FirstOrDefault(Function(f) f.StartsWith("hfIdPlanta-"))
                hfIdPlanta = Request.Params.Get(key)

                Dim res As Integer
                Dim stepsPorValidar As New List(Of ELL.Step)
                Dim flujosAprobacion As List(Of ELL.FlujoAprobacion) = BLL.FlujosAprobacionBLL.CargarListadoPorUsuario(Ticket.IdUser)
                If (Not String.IsNullOrEmpty(hfIdCostGroup)) Then
                    Dim listaIdCostGroups As List(Of String) = hfIdCostGroup.Split(";").Where(Function(f) Not String.IsNullOrEmpty(f)).ToList()
                    ' Con el tryparse evitamos los valores que no sean integer como las palabras "batz" o "customer"
                    For Each idCostGroup In listaIdCostGroups.Where(Function(f) Int32.TryParse(f, res))
                        flujosAprobacion.Where(Function(f) f.IdCostGroup = idCostGroup).ToList().ForEach(Sub(s) stepsPorValidar.Add(BLL.StepsBLL.Obtener(s.IdStep)))
                        stepsPorValidar.ForEach(Sub(s) s.CargarValoresStep())
                    Next

                    If (listaIdCostGroups.Contains("batz")) Then
                        stepsPorValidar = stepsPorValidar.Where(Function(f) f.PBC = 0).ToList()
                    ElseIf (listaIdCostGroups.Contains("customer")) Then
                        stepsPorValidar = stepsPorValidar.Where(Function(f) f.PBC > 0).ToList()
                    End If
                ElseIf (Not String.IsNullOrEmpty(hfIdPlanta)) Then
                    ' Con la planta vamos a cargar lost costgroups
                    Dim listCostGroups As List(Of ELL.CostGroup) = BLL.CostsGroupBLL.CargarListadoPorPlanta(hfIdPlanta)

                    For Each costGroup In listCostGroups
                        flujosAprobacion.Where(Function(f) f.IdCostGroup = costGroup.Id).ToList().ForEach(Sub(s) stepsPorValidar.Add(BLL.StepsBLL.Obtener(s.IdStep)))
                    Next
                End If

                If (idCabecera = Integer.MinValue) Then
                    idCabecera = stepsPorValidar.FirstOrDefault.IdCabecera
                End If

                Dim cabecera As ELL.CabeceraCostCarrier = BLL.CabecerasCostCarrierBLL.Obtener(idCabecera, False)

                ' Tenemos que comprobar primero si de todos los pasos de un proyecto hay asignado cost carrier.
                ' Si existen algunos cogemos el más grande y le sumamos uno
                ' Si no existen cogemos el correlativo_cc más grande y le sumamos uno
                Dim stepsProyectos As New List(Of ELL.Step)

                '************************ RFC0155

                If (cabecera.TipoProyPtksis = ELL.CabeceraCostCarrier.TIPO_PROY_ECO_BATZ) Then
                    ' Si es un proyecto de tipo ECO
                    If (String.IsNullOrEmpty(cabecera.CodigoProyecto)) Then
                        ' Si no tiene código de proyecto
                        ' Comprobamos los project affected
                        Dim proyectosAfectados As List(Of ELL.ProjectAffected) = BLL.ProjectsAffectedBLL.ObtenerProyectosAfectados(cabecera.Proyecto)

                        If (proyectosAfectados.Count = 1 OrElse proyectosAfectados.Select(Function(f) f.Portador.ToUpper()).Distinct.Count = 1) Then
                            If (BLL.CabecerasCostCarrierBLL.Existe(proyectosAfectados.FirstOrDefault().Portador)) Then
                                ' Si el proyecto afectado existe en la aplicacion de cost carriers

                                ' Si sólo hay un project affected o si hay varios pero el código de proyecto es el mismo
                                ' Asignamos el código de proyecto
                                BLL.CabecerasCostCarrierBLL.CambiarCodigoProyecto(cabecera.Id, proyectosAfectados.First.Portador.ToUpper())
                                ' Miramos en todos los pasos de los proyectos cuyo codigo de proyecto coincida con el raiz p.e P162
                                stepsProyectos = BLL.StepsBLL.CargarListadoPorCodigoProyecto(cabecera.CodigoProyecto)
                            Else
                                ' Financiero tendrá que asignar un nuevo portador como hasta ahora
                                stepsProyectos = BLL.StepsBLL.CargarListadoPorCabecera(idCabecera)
                            End If
                        Else
                            ' Hay más de un project affected con códigos de proyecto diferentes. Financiero tendrá que asignar un nuevo portador como hasta ahora
                            stepsProyectos = BLL.StepsBLL.CargarListadoPorCabecera(idCabecera)
                        End If
                    Else
                        ' Si tiene código de proyecto 
                        ' Miramos en todos los pasos de los proyectos cuyo codigo de proyecto coincida con el raiz p.e P162
                        stepsProyectos = BLL.StepsBLL.CargarListadoPorCodigoProyecto(cabecera.CodigoProyecto)
                    End If
                Else
                    ' Si NO es un proyecto de tipo ECO
                    stepsProyectos = BLL.StepsBLL.CargarListadoPorCabecera(idCabecera)
                End If

                '********************************

                Dim correlativo As Integer = 0
                If (stepsProyectos.Exists(Function(f) Not String.IsNullOrEmpty(f.CostCarrier))) Then
                    ' Hay algún paso con cost carrier
                    ' Cogemos el más grande y le sumamos uno
                    correlativo = CInt(stepsProyectos.Max(Function(f) f.CostCarrier).ToUpper().Replace(cabecera.CodigoProyecto.ToUpper, String.Empty)) + 1

                    ' Se da el caso de que este no sea el correlativo más grande ya que puede haber pasos aprobaados, no abiertos con correlativo cc. Hay que coger el mas grande entre los dos
                    Dim correlativoCC As Integer = 1
                    If stepsProyectos.Exists(Function(f) f.CorrelativoCC <> Integer.MinValue) Then
                        correlativoCC = stepsProyectos.Max(Function(f) f.CorrelativoCC) + 1
                    End If

                    ' Ahora cogemos el mas grande entre correlativo y correlativo cc
                    correlativo = Math.Max(correlativo, correlativoCC)
                ElseIf stepsProyectos.Exists(Function(f) f.CorrelativoCC <> Integer.MinValue) Then
                    ' No hay ningún paso con cost carrier
                    ' Buscamos sus códigos correlativos cogemos el más grande y le sumamos uno
                    correlativo = stepsProyectos.Max(Function(f) f.CorrelativoCC) + 1
                Else
                    ' Si no tiene cost carrier ni correlativos significa que es el primero y le damos el valor 1
                    correlativo = 1
                End If

                ' La aprobación final se realiza en N pasos
                ' Si sólo queda una aprobación pasa a estado aprobado
                ' Si queda más de una aprobación se queda en estado waiting for approval
                Dim listaPasosEnviarMailFinanciero As New List(Of ELL.Step)
                Dim listaPasosEnviarUsuario As New List(Of ELL.Step)
                Dim flujosAprobacionCompleto As List(Of ELL.FlujoAprobacion) = BLL.FlujosAprobacionBLL.CargarListadoPorIdCabecera(idCabecera)
                Dim flujosAprobacionAux As List(Of ELL.FlujoAprobacion)
                For Each paso In stepsPorValidar.OrderBy(Function(f) f.IdPlantaSAB)
                    flujosAprobacionAux = flujosAprobacionCompleto.Where(Function(f) f.IdStep = paso.Id).ToList()

                    If (flujosAprobacionAux.Count > 1) Then
                        ' Si hay más de uno se queda en waiting for approval
                        BLL.ValidacionesLineaBLL.AñadirEstadoValidacion(paso.IdValidacionLinea, Ticket.IdUser, String.Empty, ELL.Validacion.Estado.Waiting_for_approval, ELL.Validacion.Accion.Approve, flujosAprobacionAux.OrderBy(Function(f) f.Orden).First.Orden)

                        ' Se envía un mail al siguiente validador
                        Try
                            EnviarEmailValidador(flujosAprobacionAux.OrderBy(Function(f) f.Orden)(1).Email)
                        Catch ex As Exception
                            log.Error("Error al enviar mail al usuario paso abierto")
                        End Try

                        listaPasosEnviarUsuario.Add(paso)
                    ElseIf (flujosAprobacionAux.Count = 1) Then
                        ' Si sólo queda uno pasa a estado approved
                        BLL.ValidacionesLineaBLL.AñadirEstadoValidacion(paso.IdValidacionLinea, Ticket.IdUser, String.Empty, ELL.Validacion.Estado.Approved, ELL.Validacion.Accion.Approve, flujosAprobacionAux.OrderBy(Function(f) f.Orden).First.Orden)

                        '' Si el paso es de Zamudio como no se van a abrir lo que hacemos es directamente marcarlo como abierto 
                        'If (paso.IdPlantaSAB = ELL.Planta.PLANTA_SAB_ZAMUDIO) Then
                        '    Dim usuarioRol As ELL.UsuarioRol = BLL.UsuariosRolBLL.CargarListadoPorPlanta(ELL.Planta.PLANTA_SAB_ZAMUDIO).FirstOrDefault(Function(f) f.IdRol = ELL.Rol.TipoRol.Financiero)
                        '    BLL.ValidacionesLineaBLL.AñadirEstadoValidacion(paso.IdValidacionLinea, usuarioRol.IdSab, "Abierto automáticamente", ELL.Validacion.Estado.Opened, ELL.Validacion.Accion.Open, Integer.MinValue, DateTime.Now.AddMinutes(10))
                        'Else

                        ' Si el paso de info general no hay que enviar ni a financiero ni actualizar los datos de XPERT
                        If (Not paso.EsInfoGeneral) Then
                            If (String.IsNullOrEmpty(paso.CostCarrier)) Then
                                ' Enviar mail a financiero para que abran el paso
                                'EnviarEmailFinanciero(paso)
                                listaPasosEnviarMailFinanciero.Add(paso)
                                BLL.StepsBLL.GuardarCorrelativo(paso.Id, correlativo)
                                correlativo += 1
                            Else
                                ' Actualizar los metadatos del paso
                                Dim plantasBLL As New SabLib.BLL.PlantasComponent

                                ' La planta 0 - corporativo no tiene reflejo real en los metadatos. Es la 1 - Igorre
                                Dim planta As SabLib.ELL.Planta = plantasBLL.GetPlanta(If(paso.IdPlantaSAB = 0, 1, paso.IdPlantaSAB))
                                Dim validacionLinea As ELL.ValidacionLinea = BLL.ValidacionesLineaBLL.Obtener(paso.IdValidacionLinea)

                                Try
                                    BLL.BRAIN.CCMetadataBLL.Actualizar(planta.IdBrain, paso.CostCarrier, validacionLinea, paso)
                                Catch ex As Exception
                                    ' Si falla la actualización de los metadatos volvemos para atrás y eliminamos el estado de validación y damos de alta la entrada eliminada en el flujo
                                    BLL.ValidacionesLineaBLL.EliminarEstadoValidacion(paso.IdValidacionLinea, Ticket.IdUser, ELL.Validacion.Estado.Approved, ELL.Validacion.Accion.Approve, flujosAprobacionAux.OrderBy(Function(f) f.Orden).First.Orden, flujosAprobacionAux.OrderBy(Function(f) f.Orden).First.Porcentaje)
                                    log.Error("Error al actualizar los metadatos para la planta " & planta.IdBrain & ", costcarrier " & paso.CostCarrier & " y validación línea " & validacionLinea.Id, ex)
                                    Throw ex
                                End Try
                            End If
                        End If
                        'End If
                    End If
                Next

                MensajeInfo(Utils.Traducir("Paso/s aprobados correctamente"))

                If (listaPasosEnviarMailFinanciero.Count > 0) Then
                    EnviarEmailFinanciero(listaPasosEnviarMailFinanciero)

                    ' Para obtener el mail de usuario que realizó el envío tenemos que navegador por el histórico de estados y buscar el primer estado waiting for aproval
                    Dim historicoEstado As ELL.HistoricoEstadoLinea = BLL.HistoricosEstadoLineaBLL.CargarListadoPorValidacionLinea(stepsPorValidar(0).IdValidacionLinea).OrderBy(Function(f) f.Fecha).FirstOrDefault()
                    If (historicoEstado IsNot Nothing) Then
                        EnviarEmailUsuario(listaPasosEnviarMailFinanciero, historicoEstado.EmailUsuario, False)
                    End If
                End If

                ' En cada aprobación quieren un envio al usuario. Aprobaciones parciales
                If (listaPasosEnviarUsuario.Count > 0) Then
                    ' Para obtener el mail de usuario que realizó el envío tenemos que navegador por el histórico de estados y buscar el primer estado waiting for aproval
                    Dim historicoEstado As ELL.HistoricoEstadoLinea = BLL.HistoricosEstadoLineaBLL.CargarListadoPorValidacionLinea(stepsPorValidar(0).IdValidacionLinea).OrderBy(Function(f) f.Fecha).FirstOrDefault()
                    If (historicoEstado IsNot Nothing) Then
                        EnviarEmailUsuario(listaPasosEnviarUsuario, historicoEstado.EmailUsuario, True)
                    End If
                End If

                ' Al aprobar tenemos que ver si para ese proyecto le queda algo por validar
                ' Si le queda algo volvemos al detalle del proyecto sino a la página inial de gestión de validaciones
                Dim algoParaValidar As Boolean = BLL.FlujosAprobacionBLL.CargarListadoPorUsuario(Ticket.IdUser).Where(Function(f) f.IdCabecera = idCabecera).Count > 0

                If (algoParaValidar) Then
                    Return RedirectToAction("DetalleProyecto", New With {.idCabecera = idCabecera})
                Else
                    Return RedirectToAction("Index")
                End If
            Catch ex As Exception
                log.Error("Error al aprobar el/los paso/s para el costgroup / planta: " & hfIdCostGroup & " - " & hfIdPlanta, ex)
                MensajeError(Utils.Traducir("Error al aprobar el/los paso/s"))
            End Try

            Return RedirectToAction("DetalleProyecto", New With {.idCabecera = idCabecera})
        End Function

        '''' <summary>
        '''' 
        '''' </summary>
        '''' <param name="idPlantaSAB"></param>
        '''' <param name="idCabecera"></param>
        '''' <returns></returns>
        'Private Function HayAlgoParaValidar(ByVal idPlantaSAB As Integer, ByVal idCabecera As Integer) As Boolean
        '    Dim algoParaValidar As Boolean = False
        '    Dim stepsAux As List(Of ELL.Step) = BLL.StepsBLL.CargarListadoPorPlantaConEstadoValidacion(idPlantaSAB).Where(Function(f) f.IdEstadoValidacion = ELL.Validacion.Estado.Waiting_for_approval).ToList()
        '    For Each paso In stepsAux.Where(Function(f) f.IdCabecera = idCabecera)
        '        ' Vamos a revisar su historico. Busco el usuario y una entrada en waiting for approval y accion approve
        '        If (Not BLL.HistoricosEstadoLineaBLL.CargarListadoPorValidacionLinea(paso.IdValidacionLinea).Exists(Function(f) f.IdUser = Ticket.IdUser AndAlso f.IdEstadoValidacion = ELL.Validacion.Estado.Waiting_for_approval AndAlso f.IdAccionValidacion = ELL.Validacion.Accion.Approve)) Then
        '            algoParaValidar = True
        '            Exit For
        '        End If
        '    Next

        '    Return algoParaValidar
        'End Function

        ''' <summary>
        ''' 
        ''' </summary>
        Private Sub CargarCabeceras()
            ' Tenemos que coger aquellas entradas del flujo de aprobación en las que seamos usuario validador cuyo orden sea el menor
            Dim flujosAprobacion As List(Of ELL.FlujoAprobacion) = BLL.FlujosAprobacionBLL.CargarListadoPorUsuario(Ticket.IdUser)
            Dim listaIdProyectos As List(Of String) = flujosAprobacion.Select(Function(f) f.Proyecto).Distinct().ToList()

            ' De cada proyecto sacamos sus datos de cabecera
            Dim listaCabeceras As New List(Of ELL.CabeceraCostCarrier)

            For Each idProyecto In listaIdProyectos
                listaCabeceras.Add(BLL.CabecerasCostCarrierBLL.ObtenerByProyecto(idProyecto))
            Next

            ViewData("CabecerasProyecto") = listaCabeceras
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        Private Sub CargarCabecerasHistorico(ByVal EstadoCostCarrier As Integer, Optional ByVal producto As String = Nothing)
            Dim usuariosRol As List(Of ELL.UsuarioRol) = BLL.UsuariosRolBLL.CargarListado(Ticket.IdUser).Where(Function(f) f.IdRol = ELL.Rol.TipoRol.Responsable_ingenieria_planta _
                                                                                                               OrElse f.IdRol = ELL.Rol.TipoRol.Gerente_planta _
                                                                                                               OrElse f.IdRol = ELL.Rol.TipoRol.Project_manager _
                                                                                                               OrElse f.IdRol = ELL.Rol.TipoRol.Responsable_advance _
                                                                                                               OrElse f.IdRol = ELL.Rol.TipoRol.Direccion_CMP _
                                                                                                               OrElse f.IdRol = ELL.Rol.TipoRol.Direccion_operaciones _
                                                                                                               OrElse f.IdRol = ELL.Rol.TipoRol.Product_manager).ToList()
            Dim steps As New List(Of ELL.Step)
            Dim historicoEstadoLinea As New List(Of ELL.HistoricoEstadoLinea)

            If (usuariosRol IsNot Nothing AndAlso usuariosRol.Count > 0) Then
                For Each usuarioRol In usuariosRol
                    ' Cargamos los steps 
                    steps.AddRange(BLL.StepsBLL.CargarListadoPorPlantaConEstadoValidacion(usuarioRol.IdPlanta))
                Next
            Else
                ' En caso de no ser director o gerente podría ser owner o coowner. Cargamos todos y vemos en los que es owner o coowner
                Dim stepsAux As List(Of ELL.Step) = BLL.StepsBLL.CargarListadoPorPlantaConEstadoValidacion()

                For Each paso In stepsAux
                    If (BLL.ProyectosBLL.EsOwner(paso.Proyecto, Ticket.NombreUsuario)) Then
                        steps.Add(paso)
                    End If
                Next
            End If

            ' De todos los steps sacamos los id proyecto difentes
            Dim listaIdProyectos As List(Of String) = (From lstStep In steps
                                                       Group lstStep By lstStep.Proyecto Into agrupacion = Group
                                                       Select Proyecto).ToList()

            ' De cada proyecto sacamos sus datos de cabecera
            Dim listaCabeceras As New List(Of ELL.CabeceraCostCarrier)

            For Each idProyecto In listaIdProyectos
                listaCabeceras.Add(BLL.CabecerasCostCarrierBLL.ObtenerByProyecto(idProyecto))
            Next

            If (EstadoCostCarrier <> Integer.MinValue) Then
                listaCabeceras = listaCabeceras.Where(Function(F) F.Abierto = EstadoCostCarrier).ToList()
            End If

            If (Not String.IsNullOrEmpty(producto)) Then
                listaCabeceras = listaCabeceras.Where(Function(F) F.Producto = producto).ToList()
            End If

            ViewData("CabecerasProyecto") = listaCabeceras
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idCabecera"></param>
        Private Sub CargarCabecera(ByVal idCabecera As Integer)
            ViewData("CabeceraProyecto") = BLL.CabecerasCostCarrierBLL.Obtener(idCabecera, False)
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idCabecera"></param>
        Private Sub CargarValidaciones(ByVal idCabecera As Integer)
            ViewData("Validaciones") = BLL.ValidacionesBLL.CargarListadoPorCabecera(idCabecera)
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idValidacion"></param>
        Private Sub CargarValidacionesLineaAgrupados(ByVal idValidacion As Integer)
            'Dim validacionesLinea As List(Of ELL.ValidacionLinea) = BLL.ValidacionesLineaBLL.CargarListadoValidadosPorValidacion(idValidacion)

            '' Si la validación no está validada en el listado no sacaba nada. Ahora quieren poder acceder al histórico a medida que se va avanzando y por
            '' eso hacemos la siguiente consulta
            'If (validacionesLinea Is Nothing OrElse validacionesLinea.Count = 0) Then
            '    validacionesLinea = BLL.ValidacionesLineaBLL.CargarListadoPorValidacion(idValidacion)
            'End If

            ' El 01/06/2020 pongo este listado porque CargarListadoValidadosPorValidacion no estaba cargando los estados waiting for approval
            ViewData("ValidacionesLinea") = BLL.ValidacionesLineaBLL.CargarListadoPorValidacion(idValidacion)
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idValidacion"></param>
        Private Sub CargarValidacionesLinea(ByVal idValidacion As Integer)
            ViewData("ValidacionesLinea") = BLL.ValidacionesLineaBLL.CargarListadoPorValidacion(idValidacion)
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idValidacion"></param>
        ''' <param name="idCostGroup"></param>
        Private Sub CargarValidacionesLinea(ByVal idValidacion As Integer, ByVal idCostGroup As Integer)
            ViewData("ValidacionesLinea") = BLL.ValidacionesLineaBLL.CargarListadoPorValidacion(idValidacion).Where(Function(f) f.IdCostGroup = idCostGroup).ToList()
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idCostGroup"></param>
        ''' <param name="idPaso"></param>
        Private Sub CargarValidacionesLineaPorCostGroup(ByVal idCostGroup As Integer, ByVal idPaso As Integer)
            ViewData("ValidacionesLinea") = BLL.ValidacionesLineaBLL.CargarListadoPorCostGroup(idCostGroup).Where(Function(f) f.IdStep = idPaso).ToList()
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idValidacion"></param>
        Private Sub CargarValidacion(ByVal idValidacion As Integer)
            ViewData("Validacion") = BLL.ValidacionesBLL.Obtener(idValidacion)
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idCabecera"></param>
        ''' <param name="idValidacion"></param>
        Private Sub CargarTags(ByVal idCabecera As Integer, Optional ByVal idValidacion As String = Nothing)
            Dim validaciones As List(Of ELL.Validacion) = BLL.ValidacionesBLL.CargarListadoPorCabecera(idCabecera).OrderBy(Function(f) f.Fecha).ToList()
            Dim validacionesLI As List(Of Mvc.SelectListItem) = validaciones.Select(Function(f) New Mvc.SelectListItem With {.Text = String.Format("{0} ({1})", f.Denominacion, f.Fecha.ToShortDateString), .Value = f.Id}).ToList()

            validacionesLI.Insert(0, New SelectListItem With {.Text = String.Format("<{0}>", Utils.Traducir("Seleccione uno")), .Value = String.Empty})

            If (Not idValidacion Is Nothing AndAlso validacionesLI.Exists(Function(f) f.Value = idValidacion)) Then
                validacionesLI.First(Function(f) f.Value = idValidacion).Selected = True
            End If

            ViewData("Tags") = validacionesLI
        End Sub

        '''' <summary>
        '''' 
        '''' </summary>
        'Private Sub CargarValidaciones()
        '    ' Primero necesito saber si soy director de ingeniería o gerente
        '    Dim usuariosRol As List(Of ELL.UsuarioRol) = BLL.UsuariosRolBLL.CargarListado(Ticket.IdUser).Where(Function(f) f.IdRol = ELL.Rol.TipoRol.Director_Ingenieria OrElse f.IdRol = ELL.Rol.TipoRol.Gerente).ToList()
        '    Dim validaciones As New List(Of ELL.Validacion)
        '    Dim validacionesAux As List(Of ELL.Validacion)

        '    For Each usuarioRol In usuariosRol
        '        validacionesAux = BLL.ValidacionesBLL.CargarListadoPorPlanta(usuarioRol.IdPlanta).Where(Function(f) f.IdEstadoValidacion = ELL.Validacion.Estado.Waiting_for_approval OrElse f.IdEstadoValidacion = ELL.Validacion.Estado.Rejected).ToList()

        '        'Cargamos las validaciones línea para cada validación
        '        validacionesAux.ForEach(Sub(s) s.ValidacionesLinea = BLL.ValidacionesLineaBLL.CargarListadoPorValidacion(s.Id).Where(Function(f) f.IdPlanta = usuarioRol.IdPlanta).ToList())

        '        validaciones.AddRange(validacionesAux)
        '    Next

        '    ViewData("Validaciones") = validaciones
        'End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idValidacion"></param>
        ''' <param name="idPlanta"></param>
        Private Sub CargarStepsAGestionar(ByVal idValidacion As Integer, ByVal idPlanta As Integer)
            Dim steps As List(Of ELL.Step) = BLL.StepsBLL.CargarListadoPorValidacion(idValidacion).Where(Function(f) f.IdPlantaSAB = idPlanta AndAlso (f.IdEstadoValidacion = ELL.Validacion.Estado.Waiting_for_approval OrElse f.IdEstadoValidacion = ELL.Validacion.Estado.Rejected)).ToList()

            ' Vamos a obtener los distintos steps
            steps = steps.GroupBy(Function(f) f.Id).Select(Function(o) o.First).ToList()

            ViewData("Steps") = steps
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idCabecera"></param>
        Private Sub CargarStepsAGestionarPorCabecera(ByVal idCabecera As Integer)
            Dim flujosAprobacion As List(Of ELL.FlujoAprobacion) = BLL.FlujosAprobacionBLL.CargarListadoPorUsuario(Ticket.IdUser)
            Dim steps As New List(Of ELL.Step)

            flujosAprobacion.ForEach(Sub(s) steps.Add(BLL.StepsBLL.Obtener(s.IdStep)))

            ' Tenemos que coger sólo los referentes a la cabecera
            steps = steps.Where(Function(f) f.IdCabecera = idCabecera).ToList()

            ViewData("Steps") = steps
        End Sub

        '''' <summary>
        '''' 
        '''' </summary>
        '''' <param name="idCabecera"></param>
        'Private Sub CargarStepsPorCabecera(ByVal idCabecera As Integer)
        '    'Dim usuariosRol As List(Of ELL.UsuarioRol) = BLL.UsuariosRolBLL.CargarListado(Ticket.IdUser).Where(Function(f) f.IdRol = ELL.Rol.TipoRol.Responsable_ingenieria_planta OrElse f.IdRol = ELL.Rol.TipoRol.Gerente_planta OrElse f.IdRol = ELL.Rol.TipoRol.Project_manager).ToList()
        '    Dim usuariosRol As List(Of ELL.UsuarioRol) = BLL.UsuariosRolBLL.CargarListado(Ticket.IdUser).Where(Function(f) f.IdRol = ELL.Rol.TipoRol.Responsable_ingenieria_planta _
        '                                                                                                       OrElse f.IdRol = ELL.Rol.TipoRol.Gerente_planta _
        '                                                                                                       OrElse f.IdRol = ELL.Rol.TipoRol.Project_manager _
        '                                                                                                       OrElse f.IdRol = ELL.Rol.TipoRol.Responsable_advance _
        '                                                                                                       OrElse f.IdRol = ELL.Rol.TipoRol.Direccion_CMP _
        '                                                                                                       OrElse f.IdRol = ELL.Rol.TipoRol.Direccion_operaciones _
        '                                                                                                       OrElse f.IdRol = ELL.Rol.TipoRol.Product_manager).ToList()


        '    Dim steps As New List(Of ELL.Step)

        '    For Each usuarioRol In usuariosRol
        '        ' Cargamos los steps aprobados, abiertos o cerrados
        '        steps = BLL.StepsBLL.CargarListadoPorPlantaConEstadoValidacion(usuarioRol.IdPlanta).Where(Function(f) f.IdEstadoValidacion = ELL.Validacion.Estado.Approved OrElse f.IdEstadoValidacion = ELL.Validacion.Estado.Opened OrElse f.IdEstadoValidacion = ELL.Validacion.Estado.Closed).ToList()
        '    Next

        '    ViewData("Steps") = steps
        'End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idCostGroup"></param>
        Private Sub CargarStepsAGestionar(ByVal idCostGroup As Integer)
            Dim flujosAprobacion As List(Of ELL.FlujoAprobacion) = BLL.FlujosAprobacionBLL.CargarListadoPorUsuario(Ticket.IdUser).Where(Function(f) f.IdCostGroup = idCostGroup).ToList()
            Dim steps As New List(Of ELL.Step)
            flujosAprobacion.ForEach(Sub(s) steps.Add(BLL.StepsBLL.Obtener(s.IdStep)))

            ViewData("Steps") = steps
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="paso"></param>
        ''' <param name="idValidacionLinea"></param>
        ''' <param name="motivoRechazo"></param>
        Private Sub EnviarEmailUsuarioRechazo(ByVal paso As ELL.Step, ByVal idValidacionLinea As Integer, ByVal motivoRechazo As String)
            Try
                ' Para obtener el mail de usuario que realizó el envío tenemos que navegador por el histórico de estados y buscar el primer estado waiting for aproval
                Dim historicoEstado As ELL.HistoricoEstadoLinea = BLL.HistoricosEstadoLineaBLL.CargarListadoPorValidacionLinea(idValidacionLinea).OrderBy(Function(f) f.Fecha).FirstOrDefault()
                Dim uri = Url.Action("Index", "CostCarriers", Nothing, Request.Url.Scheme)
                Dim subject As String = Utils.Traducir("Paso rechazado")
                Dim body As String = Utils.LeerFicheroTexto(Server.MapPath("~\Content\PlantillasMail\StepRechazado.html"))
                Dim mailFrom As String = ConfigurationManager.AppSettings("MailFrom")
                Dim mailto As String = historicoEstado.EmailUsuario
                Dim paramBLL As New SabLib.BLL.ParametrosBLL
                Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor
                Dim usuariosBLL As New SabLib.BLL.UsuariosComponent()
                Dim nombreUsuarioRechaza As String = usuariosBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = Ticket.IdUser}).NombreCompleto
                Dim nombreProyecto As String = BLL.CabecerasCostCarrierBLL.Obtener(paso.IdCabecera, False).NombreProyecto
                Dim planta As String = BLL.PlantasBLL.Obtener(paso.IdPlanta).Planta
                Dim estado As String = BLL.EstadosBLL.Obtener(paso.IdEstado).Estado
                Dim costGroup As String = BLL.CostsGroupBLL.Obtener(paso.IdCostGroup).Descripcion

                '***************************************            
                body = String.Format(body, nombreProyecto, nombreUsuarioRechaza, planta, estado, costGroup, paso.Descripcion, motivoRechazo, uri)

                '***************************************
#If Not DEBUG Then
                Dim notificacionesActivas As Boolean = CBool(ConfigurationManager.AppSettings("NotificacionesActivas"))

                If (notificacionesActivas) Then
                    SabLib.BLL.Utils.EnviarEmail(mailFrom, mailto, subject, body, serverEmail)
                    log.Info("Enviado mail a usuario rechazo" & mailto)
                End If
#End If
            Catch ex As Exception
                log.Error("Se ha producido un error al enviar el mail de paso rechazado", ex)
                Throw ex
            End Try
        End Sub

        '''' <summary>
        '''' 
        '''' </summary>
        '''' <param name="paso"></param>
        'Private Sub EnviarEmailFinanciero(ByVal paso As ELL.Step)
        '    Try
        '        Dim usuariosRol As List(Of ELL.UsuarioRol) = BLL.UsuariosRolBLL.CargarListadoPorPlanta(paso.IdPlantaSAB)

        '        Dim usuarioFinanciero As ELL.UsuarioRol = usuariosRol.FirstOrDefault(Function(f) f.IdRol = ELL.Rol.TipoRol.Financiero)
        '        If (usuarioFinanciero IsNot Nothing) Then
        '            Dim validadores As List(Of ELL.UsuarioRol) = BLL.UsuariosRolBLL.CargarListadoPorPlanta(paso.IdPlantaSAB).Where(Function(f) f.IdRol = ELL.Rol.TipoRol.Director_Ingenieria OrElse f.IdRol = ELL.Rol.TipoRol.Gerente).ToList()
        '            Dim nombreValidadores As String = String.Empty

        '            For Each validador In validadores
        '                If (validadores.FirstOrDefault.IdSab = validador.IdSab) Then
        '                    nombreValidadores = validador.NombreUsuario
        '                ElseIf (validadores.LastOrDefault.IdSab = validador.IdSab) Then
        '                    nombreValidadores &= " and " & validador.NombreUsuario
        '                Else
        '                    nombreValidadores &= ", " & validador.NombreUsuario
        '                End If
        '            Next

        '            Dim uri = Url.Action("Index", "Finaciero", Nothing, Request.Url.Scheme)
        '            Dim subject As String = Utils.Traducir("Paso aprobado")
        '            Dim body As String = Utils.LeerFicheroTexto(Server.MapPath("~\Content\PlantillasMail\StepsParaAbrir.html"))
        '            Dim mailFrom As String = ConfigurationManager.AppSettings("MailFrom")
        '            Dim mailto As String = usuarioFinanciero.Email
        '            Dim paramBLL As New SabLib.BLL.ParametrosBLL
        '            Dim oParams As SabLib.ELL.Parametros = paramBLL.consultar()
        '            Dim usuariosBLL As New SabLib.BLL.UsuariosComponent()
        '            Dim nombreProyecto As String = BLL.CabecerasCostCarrierBLL.Obtener(paso.IdCabecera, False).NombreProyecto
        '            Dim planta As String = BLL.PlantasBLL.Obtener(paso.IdPlanta).Planta
        '            Dim estado As String = BLL.EstadosBLL.Obtener(paso.IdEstado).Estado
        '            Dim costGroup As String = BLL.CostsGroupBLL.Obtener(paso.IdCostGroup).Descripcion

        '            '***************************************            
        '            body = String.Format(body, nombreValidadores, nombreProyecto, , planta, estado, costGroup, paso.Descripcion, uri)
        '            '***************************************

        '            'SabLib.BLL.Utils.EnviarEmail(mailFrom, mailto, subject, body, oParams.ServidorEmail)
        '        Else
        '            log.Info("No está definido el usuario financiero para la idPlantaSAB " & paso.IdPlantaSAB)
        '        End If
        '    Catch ex As Exception
        '        log.Error("Se ha producido un error al enviar el mail de paso rechazado", ex)
        '    End Try
        'End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="listaPasos"></param>
        Public Sub EnviarEmailFinanciero(ByVal listaPasos As List(Of ELL.Step))
            Try
                ' Primero tenemos que saber las diferentes plantas
                Dim idPlantasDiferentes As List(Of Integer) = listaPasos.Select(Function(f) f.IdPlantaSAB).Distinct.ToList()

                Dim nombreProyecto As String = BLL.CabecerasCostCarrierBLL.Obtener(listaPasos.First.IdCabecera, False).NombreProyecto

                ' Para cada planta diferentes agrupamos el envio de mail
                For Each idPlantaSab In idPlantasDiferentes
                    Dim usuariosRol As List(Of ELL.UsuarioRol) = BLL.UsuariosRolBLL.CargarListadoPorPlanta(idPlantaSab)

                    ' Puede haber más de un usuario con rol de financiero como pasa en Igorre
                    Dim usuariosFinanciero As List(Of ELL.UsuarioRol) = usuariosRol.Where(Function(f) f.IdRol = ELL.Rol.TipoRol.Financiero).ToList()
                    If (usuariosFinanciero IsNot Nothing AndAlso usuariosFinanciero.Count > 0) Then
                        '' Tenemos que sacar los nombres de los usuarios que han participado en la validacion
                        'Dim validadores As List(Of ELL.UsuarioRol) = BLL.UsuariosRolBLL.CargarListadoPorPlanta(idPlantaSab).Where(Function(f) f.IdRol = ELL.Rol.TipoRol.Responsable_ingenieria_planta OrElse f.IdRol = ELL.Rol.TipoRol.Gerente_planta).ToList()
                        'Dim nombreValidadores As String = String.Empty

                        'For Each validador In validadores
                        '    If (validadores.FirstOrDefault.IdSab = validador.IdSab) Then
                        '        nombreValidadores = validador.NombreUsuario
                        '    ElseIf (validadores.LastOrDefault.IdSab = validador.IdSab) Then
                        '        nombreValidadores &= " and " & validador.NombreUsuario
                        '    Else
                        '        nombreValidadores &= ", " & validador.NombreUsuario
                        '    End If
                        'Next

                        Dim uri = Url.Action("Index", "Financiero", Nothing, Request.Url.Scheme)
                        Dim subject As String = Utils.Traducir("Paso(s) aprobado(s)")
                        Dim body As String = Utils.LeerFicheroTexto(Server.MapPath("~\Content\PlantillasMail\StepsParaAbrir.html"))
                        Dim mailFrom As String = ConfigurationManager.AppSettings("MailFrom")
                        Dim mailto As String = String.Empty
                        usuariosFinanciero.ForEach(Sub(s) mailto &= s.Email & ";")
                        Dim paramBLL As New SabLib.BLL.ParametrosBLL
                        Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor
                        Dim usuariosBLL As New SabLib.BLL.UsuariosComponent()
                        Dim cuerpo As New StringBuilder

                        For Each paso In listaPasos.Where(Function(f) f.IdPlantaSAB = idPlantaSab)
                            cuerpo.Append("<tr>")
                            cuerpo.Append("<td>")
                            cuerpo.Append(paso.Planta)
                            cuerpo.Append("</td>")
                            cuerpo.Append("<td>")
                            cuerpo.Append(paso.Estado)
                            cuerpo.Append("</td>")
                            cuerpo.Append("<td>")
                            cuerpo.Append(BLL.CostsGroupBLL.Obtener(paso.IdCostGroup).Descripcion)
                            cuerpo.Append("</td>")
                            cuerpo.Append("<td>")
                            cuerpo.Append(paso.Descripcion)
                            cuerpo.Append("</td>")
                            cuerpo.Append("</tr>")
                        Next

                        '***************************************            
                        body = String.Format(body, nombreProyecto, cuerpo.ToString(), uri)
                        '***************************************
#If Not DEBUG Then
                        Dim notificacionesActivas As Boolean = CBool(ConfigurationManager.AppSettings("NotificacionesActivas"))

                        If (notificacionesActivas) Then
                            SabLib.BLL.Utils.EnviarEmail(mailFrom, mailto, subject, body, serverEmail)
                            log.Info("Enviado mail a financiero " & mailto)
                        End If
#End If
                    Else
                        log.Info("No está definido el usuario financiero para la idPlantaSAB " & idPlantaSab)
                    End If
                Next
            Catch ex As Exception
                log.Error("Se ha producido un error al enviar el mail a financiero", ex)
            End Try
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="listaPasos"></param>
        ''' <param name="email"></param>
        ''' <param name="aprobacionParcial"></param>
        Private Sub EnviarEmailUsuario(ByVal listaPasos As List(Of ELL.Step), ByVal email As String, ByVal aprobacionParcial As Boolean)
            Try
                Dim nombreProyecto As String = BLL.CabecerasCostCarrierBLL.Obtener(listaPasos.First.IdCabecera, False).NombreProyecto

                Dim uri = Url.Action("Index", "CostCarriers", Nothing, Request.Url.Scheme)
                Dim subject As String = Utils.Traducir("Paso(s) aprobado(s)")
                Dim body As String = String.Empty
                If (Not aprobacionParcial) Then
                    body = Utils.LeerFicheroTexto(Server.MapPath("~\Content\PlantillasMail\StepsAprobados.html"))
                Else
                    body = Utils.LeerFicheroTexto(Server.MapPath("~\Content\PlantillasMail\StepsAprobadosParcial.html"))
                End If
                Dim mailFrom As String = ConfigurationManager.AppSettings("MailFrom")
                Dim mailto As String = email
                Dim paramBLL As New SabLib.BLL.ParametrosBLL
                Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor
                Dim usuariosBLL As New SabLib.BLL.UsuariosComponent()
                Dim cuerpo As New StringBuilder

                For Each paso In listaPasos
                    cuerpo.Append("<tr>")
                    cuerpo.Append("<td>")
                    cuerpo.Append(paso.Planta)
                    cuerpo.Append("</td>")
                    cuerpo.Append("<td>")
                    cuerpo.Append(paso.Estado)
                    cuerpo.Append("</td>")
                    cuerpo.Append("<td>")
                    cuerpo.Append(BLL.CostsGroupBLL.Obtener(paso.IdCostGroup).Descripcion)
                    cuerpo.Append("</td>")
                    cuerpo.Append("<td>")
                    cuerpo.Append(paso.Descripcion)
                    cuerpo.Append("</td>")
                    cuerpo.Append("</tr>")
                Next

                '***************************************       
                If (Not aprobacionParcial) Then
                    body = String.Format(body, nombreProyecto, cuerpo.ToString(), uri)
                Else
                    body = String.Format(body, nombreProyecto, cuerpo.ToString(), uri, Ticket.NombreCompleto)
                End If
                '***************************************

#If Not DEBUG Then
                Dim notificacionesActivas As Boolean = CBool(ConfigurationManager.AppSettings("NotificacionesActivas"))

                If (notificacionesActivas) Then
                    SabLib.BLL.Utils.EnviarEmail(mailFrom, mailto, subject, body, serverEmail)
                    log.Info("Enviado mail a usuario " & mailto)
                End If
#End If
            Catch ex As Exception
                log.Error("Se ha producido un error al enviar el mail de paso aprobado", ex)
            End Try
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="productoSeleccionado"></param>
        ''' <returns></returns>
        Public Function CargarProductosFiltro(ByVal productoSeleccionado As String) As JsonResult
            ' Cargamos los productos para los 3 tipos de proyectos
            Dim productos As List(Of ELL.Producto) = BLL.ProductosBLL.CargarListado(ELL.TipoProyecto.TipoProyecto.Industrialization, String.Empty)
            productos.AddRange(BLL.ProductosBLL.CargarListado(ELL.TipoProyecto.TipoProyecto.R_D, String.Empty))
            productos.AddRange(BLL.ProductosBLL.CargarListado(ELL.TipoProyecto.TipoProyecto.Predev, String.Empty))

            Dim listaProductosUnicos As IEnumerable(Of Object) = (From lstProductos In productos
                                                                  Group lstProductos By lstProductos.Producto Into agrupacion = Group
                                                                  Select New With {.Producto = Producto}).ToList()

            Dim productosLI As List(Of Mvc.SelectListItem) = listaProductosUnicos.Select(Function(f) New Mvc.SelectListItem With {.Text = f.Producto, .Value = f.Producto}).OrderBy(Function(f) f.Text).ToList()
            productosLI.Insert(0, New SelectListItem With {.Text = String.Format("<{0}>", Utils.Traducir("Todos")), .Value = String.Empty})

            If (Not String.IsNullOrEmpty(productoSeleccionado) AndAlso productosLI.Exists(Function(f) f.Value = productoSeleccionado)) Then
                productosLI.First(Function(f) f.Value = productoSeleccionado).Selected = True
            End If

            ViewData("Productos") = productosLI
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="estado"></param>
        ''' <returns></returns>
        Public Function CargarEstadosCostCarrierFiltro(ByVal estado As Integer) As JsonResult
            Dim estadosLI As New List(Of Mvc.SelectListItem)
            estadosLI.Insert(0, New SelectListItem With {.Text = String.Format("<{0}>", Utils.Traducir("Todos")), .Value = Integer.MinValue})
            estadosLI.Insert(1, New SelectListItem With {.Text = Utils.Traducir("Abierto"), .Value = 1})
            estadosLI.Insert(2, New SelectListItem With {.Text = Utils.Traducir("Cerrado"), .Value = 0})
            estadosLI.Insert(3, New SelectListItem With {.Text = Utils.Traducir("Sin estado"), .Value = -1})

            If (Not String.IsNullOrEmpty(estado)) Then
                estadosLI.First(Function(f) f.Value = estado).Selected = True
            End If

            ViewData("EstadoCostCarrier") = estadosLI
        End Function

#End Region

    End Class
End Namespace