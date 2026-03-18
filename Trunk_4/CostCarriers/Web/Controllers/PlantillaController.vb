Imports System.Web.Mvc

Namespace Controllers

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class PlantillaController
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
                roles.Add(ELL.Rol.TipoRol.Admin)
                Return roles
            End Get
        End Property

#End Region

#Region "Métodos"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Function Index(Optional ByVal idTipoProyecto As Integer? = Nothing) As ActionResult
            ViewData("idTipoProyecto") = idTipoProyecto

            CargarTiposProyecto()
            'CargarPlantas()
            'CargarEstados()

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Function CargarPlantillaArbol(ByVal idTipoProyecto As Integer) As JsonResult
            Dim tipoProyecto As String = BLL.TiposProyectoBLL.Obtener(idTipoProyecto).Descripcion

            Dim treeView As New TreeviewNode With {.text = "Tipo proyecto: " & tipoProyecto}

            ' Cargamos la plantilla por tipo de proyecto
            Dim plantilla As ELL.Plantilla = BLL.PlantillasBLL.ObtenerPorTipoProyecto(idTipoProyecto)

            If (plantilla IsNot Nothing) Then

                ' Cargamos las plantas de esa plantilla
                Dim plantasPlantillas As List(Of ELL.PlantaPlantilla) = BLL.PlantasPlantillaBLL.CargarListado(plantilla.Id)

                ' Vamos a ir componiendo la estructur del árbol
                ' Para cada planta añadimos un nodo
                For Each planta In plantasPlantillas
                    Dim treeViewNodePlanta As New TreeviewNode With {.text = String.Format("{0}: <b>{1}</b>", Utils.Traducir("Planta"), planta.Planta), .backColor = "#b6ced8", .dataAttributes = New DataAttribute With {.id = planta.Id, .idPlanta = planta.IdPlanta, .kind = "planta"}}

                    ' Cargamos los estados de esa planta 
                    Dim estadosPlantaPlantilla As List(Of ELL.EstadoPlantilla) = BLL.EstadosPlantillaBLL.CargarListado(planta.Id).OrderBy(Function(f) f.Estado).ToList()

                    ' Para cada estado añadimos un nodo
                    For Each estado In estadosPlantaPlantilla
                        Dim treeViewNodeEstado As New TreeviewNode With {.text = String.Format("{0}: <b>{1}</b>", Utils.Traducir("Estado"), estado.Estado), .backColor = "#dedede", .dataAttributes = New DataAttribute With {.id = estado.Id, .kind = "estado"}}

                        ' Cargamos los cost group de ese estado
                        Dim costGroups As List(Of ELL.CostGroupPlantilla) = BLL.CostsGroupPlantillaBLL.CargarListado(estado.Id)

                        ' Para cada cost group añadimos un nodo
                        For Each costGroup In costGroups.OrderBy(Function(f) f.Descripcion)
                            Dim treeViewCostGroup As New TreeviewNode With {.text = String.Format("{0}: <b>{1}</b>", Utils.Traducir("Grupo de coste"), costGroup.Descripcion), .backColor = "#b4d3b8", .dataAttributes = New DataAttribute With {.id = costGroup.Id, .kind = "costGroup"}}

                            ' Cargamos los steps de ese costgroup
                            Dim steps As List(Of ELL.StepPlantilla) = BLL.StepsPlantillaBLL.CargarListado(costGroup.Id)

                            ' Para cada step añadimos un nodo
                            For Each paso In steps
                                Dim treeViewStep As New TreeviewNode With {.text = String.Format("{0}: <b>{1}</b>", Utils.Traducir("Paso"), paso.Descripcion), .backColor = "#f3f2db", .dataAttributes = New DataAttribute With {.id = paso.Id, .kind = "step"}}

                                If (treeViewCostGroup.nodes Is Nothing) Then
                                    treeViewCostGroup.nodes = New List(Of TreeviewNode)()
                                End If
                                treeViewCostGroup.nodes.Add(treeViewStep)
                            Next

                            If (treeViewNodeEstado.nodes Is Nothing) Then
                                treeViewNodeEstado.nodes = New List(Of TreeviewNode)()
                            End If
                            treeViewNodeEstado.nodes.Add(treeViewCostGroup)
                        Next

                        If (treeViewNodePlanta.nodes Is Nothing) Then
                            treeViewNodePlanta.nodes = New List(Of TreeviewNode)()
                        End If
                        treeViewNodePlanta.nodes.Add(treeViewNodeEstado)
                    Next

                    If (treeView.nodes Is Nothing) Then
                        treeView.nodes = New List(Of TreeviewNode)()
                    End If
                    treeView.nodes.Add(treeViewNodePlanta)
                Next
            End If

            Return Json(treeView, JsonRequestBehavior.AllowGet)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        Private Sub CargarTiposProyecto()
            ViewData("TiposProyecto") = BLL.TiposProyectoBLL.CargarListado()
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idTipoProyecto"></param>
        ''' <returns></returns>
        Function CargarPlantas(ByVal idTipoProyecto As Integer) As JsonResult
            Dim plantasBLL As New SabLib.BLL.PlantasComponent()
            Dim plantas As New List(Of SabLib.ELL.Planta)

            '130924: Maite pide que cuando sea predevelopment, se puedan añadir todas las plantas
            If (idTipoProyecto = ELL.TipoProyecto.TipoProyecto.Industrialization OrElse idTipoProyecto = ELL.TipoProyecto.TipoProyecto.Predev) Then
                plantas = plantasBLL.GetPlantas()
            End If

            ' Para tipo de proyecto R-D y Predev sólo se puede añadir la planta de corporativo
            plantas.Add(New SabLib.ELL.Planta With {.Id = 0, .Nombre = Utils.Traducir("Corporativo")})

            ' Pide Maite el 14/10/2019 que para R-D también se puede seleccionar Igorre como planta
            ' Pide Maite el 08/04/2022 que para R-D también se puede seleccionar Zamudio como planta
            If (idTipoProyecto = ELL.TipoProyecto.TipoProyecto.R_D) Then
                Dim planta As SabLib.ELL.Planta = plantasBLL.GetPlanta(1)

                If (planta IsNot Nothing) Then
                    plantas.Add(New SabLib.ELL.Planta With {.Id = planta.Id, .Nombre = planta.Nombre})
                End If

                planta = plantasBLL.GetPlanta(47)

                If (planta IsNot Nothing) Then
                    plantas.Add(New SabLib.ELL.Planta With {.Id = planta.Id, .Nombre = planta.Nombre})
                End If
            End If

                Return Json(plantas, JsonRequestBehavior.AllowGet)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        Function CargarEstados(ByVal idTipoProyecto As Integer) As JsonResult
            Dim estados As List(Of ELL.EstadoProyecto) = BLL.EstadosProyectoBLL.CargarListado(idTipoProyecto).OrderBy(Function(f) f.Descripcion).ToList()

            Return Json(estados, JsonRequestBehavior.AllowGet)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idCostGroup"></param>
        ''' <returns></returns>
        Function CargarCostGroup(ByVal idCostGroup As Integer) As JsonResult
            Dim costGroup As ELL.CostGroupPlantilla = BLL.CostsGroupPlantillaBLL.Obtener(idCostGroup)

            Return Json(costGroup, JsonRequestBehavior.AllowGet)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idCostGroup"></param>
        ''' <returns></returns>
        Function CargarOrigenesDatos(ByVal idCostGroup As Integer) As JsonResult
            ' Si el cost group es Nuevo, es decir, lo mete el usuario y no lo coge de Bonos de sistemas no sacamos la opción de bonos
            Dim origenesDatos As List(Of ELL.OrigenDatosStep) = BLL.OrigenesDatosStepBLL.CargarListado()

            Dim costGroupPlantillas As ELL.CostGroupPlantilla = BLL.CostsGroupPlantillaBLL.Obtener(idCostGroup)
            If (costGroupPlantillas.IdBonos = Integer.MinValue) Then
                ' Eliminamos el Presupuesto de bonos de la lista
                If (origenesDatos IsNot Nothing AndAlso origenesDatos.Count > 0 AndAlso origenesDatos.Exists(Function(f) f.Id = ELL.OrigenDatosStep.OrigenDatosStep.Presupuesto_bonos)) Then
                    origenesDatos.Remove(origenesDatos.First(Function(f) f.Id = ELL.OrigenDatosStep.OrigenDatosStep.Presupuesto_bonos))
                End If
            End If

            origenesDatos.ForEach(Sub(s) s.Descripcion = Utils.Traducir(s.Descripcion))

            Return Json(origenesDatos, JsonRequestBehavior.AllowGet)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idCostGroup"></param>
        ''' <returns></returns>
        Function CargarOrigenesDatosGastos(ByVal idCostGroup As Integer) As JsonResult
            ' Como orígenes de datos de gastos anuales hay dos opciones:
            ' - Si el costgroup tiene bonos: planificación y manual
            ' - SI el costgroup no tiene bonos: manual
            Dim origenesDatos As List(Of ELL.OrigenDatosStep)

            Dim costGroupPlantillas As ELL.CostGroupPlantilla = BLL.CostsGroupPlantillaBLL.Obtener(idCostGroup)
            If (costGroupPlantillas.IdBonos <> Integer.MinValue) Then
                origenesDatos = BLL.OrigenesDatosStepBLL.CargarListado().Where(Function(f) f.Id = ELL.OrigenDatosStep.OrigenDatosStep.Planificacion OrElse f.Id = ELL.OrigenDatosStep.OrigenDatosStep.Manual).ToList()
            Else
                origenesDatos = BLL.OrigenesDatosStepBLL.CargarListado().Where(Function(f) f.Id = ELL.OrigenDatosStep.OrigenDatosStep.Manual).ToList()
            End If

            origenesDatos.ForEach(Sub(s) s.Descripcion = Utils.Traducir(s.Descripcion))

            Return Json(origenesDatos, JsonRequestBehavior.AllowGet)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        Function CargarVariablesFormula() As JsonResult
            Dim variables As List(Of ELL.VariableFormula) = BLL.VariablesFormulaBLL.CargarListado()
            Return Json(variables, JsonRequestBehavior.AllowGet)
        End Function

        ''' <summary>
        ''' Para borrar
        ''' </summary>
        Public Class CostGroupBonos

            Public Property Id As Integer
            Public Property Nombre As String

        End Class

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idPlanta"></param>
        ''' <returns></returns>
        Function CargarCostGroupBonos(ByVal idPlanta As Integer) As String
            Dim lista As List(Of ELL.EstadoBonos) = Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of ELL.EstadoBonos))(BLL.CostGroupBonosBLL.CargarListado(idPlanta))

            'If (idPlanta = 0) Then
            lista.Add(New ELL.EstadoBonos With {.Id = -1, .Nombre = Utils.Traducir("HR-MR")})
            'End If

            Return Newtonsoft.Json.JsonConvert.SerializeObject(lista)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Function CargarCostGroupOT() As JsonResult
            Dim costGroupsOT As List(Of ELL.CostGroupOT) = BLL.CostsGroupOTBLL.CargarListado()

            'Añadimos el elemento vacio
            costGroupsOT.Insert(0, New ELL.CostGroupOT With {.Id = -1, .Nombre = String.Format("<{0}>", Utils.Traducir("Seleccione uno"))})

            Return Json(costGroupsOT, JsonRequestBehavior.AllowGet)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idStep"></param>
        ''' <returns></returns>
        Function CargarStep(ByVal idStep As Integer) As JsonResult
            Dim [step] As ELL.StepPlantilla = BLL.StepsPlantillaBLL.Obtener(idStep)

            Return Json([step], JsonRequestBehavior.AllowGet)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="hTipoProyecto"></param>
        ''' <param name="selectPlantas"></param>
        Function AgregarPlanta(ByVal hTipoProyecto As String, ByVal selectPlantas As Integer) As JsonResult
            ' Hay que verificar que esa planta no esté ya añadida para ese tipo de proyecto
            ' Puede ser que no exista si la plantilla
            Try
                Dim plantilla As ELL.Plantilla = BLL.PlantillasBLL.ObtenerPorTipoProyecto(hTipoProyecto)

                If (plantilla Is Nothing) Then
                    plantilla = New ELL.Plantilla With {.IdTipoProyecto = hTipoProyecto, .IdUsuarioAlta = Ticket.IdUser}
                    BLL.PlantillasBLL.Guardar(plantilla)
                End If

                ' Ahora vamos a comprobar si existe la planta en la plantilla
                If (BLL.PlantasPlantillaBLL.CargarListado(plantilla.Id).Exists(Function(f) f.IdPlanta = selectPlantas)) Then
                    Return Json(New With {.messageType = "alerta", .message = Utils.Traducir("La planta ya existe para ese tipo de proyecto")})
                Else
                    BLL.PlantasPlantillaBLL.Guardar(New ELL.PlantaPlantilla With {.IdPlanta = selectPlantas, .IdPlantilla = plantilla.Id})
                End If
            Catch ex As Exception
                log.Error("Error al agregar la planta", ex)
                Return Json(New With {.messageType = "error", .message = Utils.Traducir("Error al agregar la planta")})
            End Try

            Return Json(New With {.messageType = "info", .message = Utils.Traducir("Planta agregada correctamente")})
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="hPlantaPlantilla"></param>
        ''' <param name="selectEstados"></param>
        Function AgregarEstado(ByVal hPlantaPlantilla As Integer, ByVal selectEstados As Integer) As JsonResult
            ' Hay que verificar que esa estado no esté ya añadido para esa planta
            Try
                If (BLL.EstadosPlantillaBLL.CargarListado(hPlantaPlantilla).Exists(Function(f) f.IdEstadoProyecto = selectEstados)) Then
                    Return Json(New With {.messageType = "alerta", .message = Utils.Traducir("El estado ya existe para esa planta")})
                Else
                    BLL.EstadosPlantillaBLL.Guardar(New ELL.EstadoPlantilla With {.IdPlantaPlantilla = hPlantaPlantilla, .IdEstadoProyecto = selectEstados})
                End If
            Catch ex As Exception
                log.Error("Error al agregar el estado", ex)
                Return Json(New With {.messageType = "error", .message = Utils.Traducir("Error al agregar el estado")})
            End Try

            Return Json(New With {.messageType = "info", .message = Utils.Traducir("Estado agregado correctamente")})
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="hEstadoPlantilla"></param>
        ''' <param name="origenCostGroupAgregar"></param>
        ''' <param name="txtAgregarDescripcionCostGroup"></param>
        ''' <param name="selectAgregarCostGroup"></param>
        ''' <param name="selectAgregarCostGroupOT"></param>
        ''' <returns></returns>
        Function AgregarCostGroup(ByVal hEstadoPlantilla As Integer, ByVal origenCostGroupAgregar As String, ByVal txtAgregarDescripcionCostGroup As String, ByVal selectAgregarCostGroup As Integer?, ByVal selectAgregarCostGroupOT As Integer) As JsonResult
            Try
                Dim costGroupPlantilla As New ELL.CostGroupPlantilla With {.IdEstadoPlantilla = hEstadoPlantilla, .IdCostGroupOT = selectAgregarCostGroupOT}

                If (origenCostGroupAgregar = 0) Then
                    ' Nuevo
                    costGroupPlantilla.IdBonos = Integer.MinValue
                    costGroupPlantilla.Descripcion = txtAgregarDescripcionCostGroup.ToUpper()
                ElseIf (origenCostGroupAgregar = 1) Then
                    ' Bonos
                    costGroupPlantilla.IdBonos = selectAgregarCostGroup
                    costGroupPlantilla.Descripcion = Nothing
                End If

                BLL.CostsGroupPlantillaBLL.Guardar(costGroupPlantilla)
            Catch ex As Exception
                log.Error("Error al guardar el cost group", ex)
                Return Json(New With {.messageType = "error", .message = Utils.Traducir("Error al guardar el grupo de coste")})
            End Try

            Return Json(New With {.messageType = "info", .message = Utils.Traducir("Grupo de coste guardado correctamente")})
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="hCostGroupPlantillaParaStep"></param>
        ''' <param name="txtAgregarDescripcionStep"></param>
        ''' <param name="selectAgregarOrigenDatosOBC"></param>
        ''' <param name="hFormulaAgregarTC"></param>
        ''' <param name="hFormulaAgregarTCCustomer"></param>
        ''' <param name="selectAgregarOrigenDatosGastosAño"></param>
        ''' <param name="selectAgregarOrigenDatoReal"></param>
        ''' <param name="infoGeneralStepAgregar"></param>
        ''' <returns></returns>
        Function AgregarStep(ByVal hCostGroupPlantillaParaStep As Integer, ByVal txtAgregarDescripcionStep As String,
                             ByVal selectAgregarOrigenDatosOBC As Integer, ByVal hFormulaAgregarTC As String, ByVal hFormulaAgregarTCCustomer As String,
                             ByVal selectAgregarOrigenDatosGastosAño As Integer, ByVal selectAgregarOrigenDatoReal As Integer, ByVal infoGeneralStepAgregar As Integer) As JsonResult
            Try
                Dim stepPlantilla As New ELL.StepPlantilla()
                If (infoGeneralStepAgregar = 0) Then
                    stepPlantilla = New ELL.StepPlantilla With {.Descripcion = txtAgregarDescripcionStep, .IdCostGroupPlantilla = hCostGroupPlantillaParaStep,
                                                                .OBCOrigenDatos = selectAgregarOrigenDatosOBC, .TCFormula = hFormulaAgregarTC, .TCFormulaCustomer = hFormulaAgregarTCCustomer,
                                                                .GastosAñoOrigenDatos = selectAgregarOrigenDatosGastosAño, .OrigenDatoReal = selectAgregarOrigenDatoReal}
                Else
                    stepPlantilla = New ELL.StepPlantilla With {.Descripcion = txtAgregarDescripcionStep, .IdCostGroupPlantilla = hCostGroupPlantillaParaStep,
                                                                .OBCOrigenDatos = ELL.OrigenDatosStep.OrigenDatosStep.Manual, .TCFormula = "0", .TCFormulaCustomer = "0",
                                                                .GastosAñoOrigenDatos = ELL.OrigenDatosStep.OrigenDatosStep.Manual, .OrigenDatoReal = 0, .EsInfoGeneral = True}
                End If

                BLL.StepsPlantillaBLL.Guardar(stepPlantilla)
            Catch ex As Exception
                log.Error("Error al guardar el step", ex)
                Return Json(New With {.messageType = "error", .message = Utils.Traducir("Error al guardar el paso")})
            End Try

            Return Json(New With {.messageType = "info", .message = Utils.Traducir("Paso guardado correctamente")})
        End Function

        '''' <summary>
        '''' 
        '''' </summary>
        '''' <param name="hTipoProyectoPlanta"></param>
        '''' <param name="selectPlantas"></param>
        'Function AgregarPlanta(ByVal hTipoProyectoPlanta As Integer, ByVal selectPlantas As Integer) As ActionResult
        '    ' Hay que verificar que esa planta no esté ya añadida para ese tipo de proyecto
        '    ' Puede ser que no exista si la plantilla
        '    Try
        '        Dim plantilla As ELL.Plantilla = BLL.PlantillasBLL.ObtenerPorTipoProyecto(hTipoProyectoPlanta)

        '        If (plantilla Is Nothing) Then
        '            plantilla = New ELL.Plantilla With {.IdTipoProyecto = hTipoProyectoPlanta, .IdUsuarioAlta = Ticket.IdUser}
        '            BLL.PlantillasBLL.Guardar(plantilla)
        '        End If

        '        ' Ahora vamos a comprobar si existe la planta en la plantilla
        '        If (BLL.PlantasPlantillaBLL.CargarListado(plantilla.Id).Exists(Function(f) f.IdPlanta = selectPlantas)) Then
        '            MensajeAlerta(Utils.Traducir("La planta ya existe para ese tipo de proyecto"))
        '        Else
        '            BLL.PlantasPlantillaBLL.Guardar(New ELL.PlantaPlantilla With {.IdPlanta = selectPlantas, .IdPlantilla = plantilla.Id})
        '            MensajeInfo(Utils.Traducir("Planta agregada correctamente"))
        '        End If
        '    Catch ex As Exception
        '        MensajeError(Utils.Traducir("Error al agregar la planta"))
        '        log.Error("Error al agregar la planta", ex)
        '    End Try

        '    Return RedirectToAction("Index", New With {.idTipoProyecto = hTipoProyectoPlanta})
        'End Function

        '''' <summary>
        '''' 
        '''' </summary>
        '''' <param name="hTipoProyectoEstado"></param>
        '''' <param name="hPlantaPlantilla"></param>
        '''' <param name="selectEstados"></param>
        'Function AgregarEstado(ByVal hTipoProyectoEstado As Integer, ByVal hPlantaPlantilla As Integer, ByVal selectEstados As Integer) As ActionResult
        '    ' Hay que verificar que esa estado no esté ya añadido para esa planta
        '    Try
        '        If (BLL.EstadosPlantillaBLL.CargarListado(hPlantaPlantilla).Exists(Function(f) f.IdEstadoProyecto = selectEstados)) Then
        '            MensajeAlerta(Utils.Traducir("El estado ya existe para esa planta"))
        '        Else
        '            BLL.EstadosPlantillaBLL.Guardar(New ELL.EstadoPlantilla With {.IdPlantaPlantilla = hPlantaPlantilla, .IdEstadoProyecto = selectEstados})
        '            MensajeInfo(Utils.Traducir("Estado agregado correctamente"))
        '        End If
        '    Catch ex As Exception
        '        MensajeError(Utils.Traducir("Error al agregar el estado"))
        '        log.Error("Error al agregar el estado", ex)
        '    End Try

        '    Return RedirectToAction("Index", New With {.idTipoProyecto = hTipoProyectoEstado})
        'End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        Function EliminarPlanta(ByVal id As Integer) As JsonResult
            Try
                BLL.PlantasPlantillaBLL.Eliminar(id)
            Catch ex As Exception
                log.Error("Error al eliminar la planta", ex)
                Return Json(New With {.messageType = "error", .message = Utils.Traducir("Error al eliminar la planta")})
            End Try

            Return Json(New With {.messageType = "info", .message = Utils.Traducir("Planta eliminada correctamente")})
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        Function EliminarEstado(ByVal id As Integer) As JsonResult
            Try
                BLL.EstadosPlantillaBLL.Eliminar(id)
            Catch ex As Exception
                log.Error("Error al eliminar el estado", ex)
                Return Json(New With {.messageType = "error", .message = Utils.Traducir("Error al eliminar el estado")})
            End Try

            Return Json(New With {.messageType = "info", .message = Utils.Traducir("Estado eliminado correctamente")})
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        Function EliminarCostGroup(ByVal id As Integer) As JsonResult
            Try
                BLL.CostsGroupPlantillaBLL.Eliminar(id)
            Catch ex As Exception
                log.Error("Error al eliminar el cost group", ex)
                Return Json(New With {.messageType = "error", .message = Utils.Traducir("Error al eliminar el grupo de coste")})
            End Try

            Return Json(New With {.messageType = "info", .message = Utils.Traducir("Grupo de coste eliminado correctamente")})
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        Function EliminarStep(ByVal id As Integer) As JsonResult
            Try
                BLL.StepsPlantillaBLL.Eliminar(id)
            Catch ex As Exception
                log.Error("Error al eliminar el step", ex)
                Return Json(New With {.messageType = "error", .message = Utils.Traducir("Error al eliminar el paso")})
            End Try

            Return Json(New With {.messageType = "info", .message = Utils.Traducir("Paso eliminado correctamente")})
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="ordenActual"></param>
        ''' <returns></returns>
        Function CambiarOrdenSteps(ByVal ordenActual() As Integer) As JsonResult
            Try
                BLL.StepsPlantillaBLL.CambiarOrdenSteps(ordenActual)
            Catch ex As Exception
                log.Error("Error al eliminar el step", ex)
                Return Json(New With {.messageType = "error", .message = Utils.Traducir("Error al cambiar de orden el paso")})
            End Try

            Return Json(New With {.messageType = "info", .message = Utils.Traducir("Paso cambiado de orden correctamente")})
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="hCostGroupPlantilla"></param>
        ''' <param name="origenCostGroupEditar"></param>
        ''' <param name="txtEditarDescripcionCostGroup"></param>
        ''' <param name="selectEditarCostGroup"></param>
        ''' <param name="selectEditarCostGroupOT"></param>
        ''' <returns></returns>
        Function EditarCostGroup(ByVal hCostGroupPlantilla As Integer, ByVal origenCostGroupEditar As String, ByVal txtEditarDescripcionCostGroup As String, ByVal selectEditarCostGroup As Integer?, ByVal selectEditarCostGroupOT As Integer) As JsonResult
            Try
                Dim costGroupPlantilla As New ELL.CostGroupPlantilla With {.Id = hCostGroupPlantilla, .IdCostGroupOT = selectEditarCostGroupOT}

                If (origenCostGroupEditar = 0) Then
                    ' Nuevo
                    costGroupPlantilla.IdBonos = Integer.MinValue
                    costGroupPlantilla.Descripcion = txtEditarDescripcionCostGroup.ToUpper()
                ElseIf (origenCostGroupEditar = 1) Then
                    ' Bonos
                    costGroupPlantilla.IdBonos = selectEditarCostGroup
                    costGroupPlantilla.Descripcion = Nothing
                End If

                BLL.CostsGroupPlantillaBLL.Guardar(costGroupPlantilla)
            Catch ex As Exception
                log.Error("Error al editar el cost group", ex)
                Return Json(New With {.messageType = "error", .message = Utils.Traducir("Error al editar el grupo de coste")})
            End Try

            Return Json(New With {.messageType = "info", .message = Utils.Traducir("Grupo de coste editado correctamente")})
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="hStepPlantilla"></param>
        ''' <param name="txtEditarDescripcionStep"></param>
        ''' <param name="selectEditarOrigenDatosOBC"></param>
        ''' <param name="hFormulaEditarTC"></param>
        ''' <param name="hFormulaEditarTCCustomer"></param>
        ''' <param name="selectEditarOrigenDatosGastosAño"></param>
        ''' <param name="selectEditarOrigenDatoReal"></param>
        ''' <param name="infoGeneralStepEditar"></param>
        ''' <returns></returns>
        Function EditarStep(ByVal hStepPlantilla As Integer, ByVal txtEditarDescripcionStep As String,
                             ByVal selectEditarOrigenDatosOBC As Integer, ByVal hFormulaEditarTC As String, ByVal hFormulaEditarTCCustomer As String,
                             ByVal selectEditarOrigenDatosGastosAño As Integer, ByVal selectEditarOrigenDatoReal As Integer, ByVal infoGeneralStepEditar As Integer) As JsonResult
            Try
                Dim stepPlantilla As ELL.StepPlantilla

                If (infoGeneralStepEditar = 0) Then
                    stepPlantilla = New ELL.StepPlantilla With {.Id = hStepPlantilla, .Descripcion = txtEditarDescripcionStep,
                                                                .OBCOrigenDatos = selectEditarOrigenDatosOBC, .TCFormula = hFormulaEditarTC, .TCFormulaCustomer = hFormulaEditarTCCustomer,
                                                                .GastosAñoOrigenDatos = selectEditarOrigenDatosGastosAño, .OrigenDatoReal = selectEditarOrigenDatoReal}
                Else
                    stepPlantilla = New ELL.StepPlantilla With {.Id = hStepPlantilla, .Descripcion = txtEditarDescripcionStep,
                                                                .OBCOrigenDatos = ELL.OrigenDatosStep.OrigenDatosStep.Manual, .TCFormula = "0", .TCFormulaCustomer = "0",
                                                                .GastosAñoOrigenDatos = ELL.OrigenDatosStep.OrigenDatosStep.Manual, .OrigenDatoReal = 0, .EsInfoGeneral = True}
                End If

                BLL.StepsPlantillaBLL.Guardar(stepPlantilla)
            Catch ex As Exception
                log.Error("Error al guardar el step", ex)
                Return Json(New With {.messageType = "error", .message = Utils.Traducir("Error al guardar el paso")})
            End Try

            Return Json(New With {.messageType = "info", .message = Utils.Traducir("Paso guardado correctamente")})
        End Function

#End Region

    End Class
End Namespace