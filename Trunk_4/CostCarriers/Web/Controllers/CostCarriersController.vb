Imports System.Web.Mvc
Imports System.Web.Script.Serialization
Imports CostCarriersLib.BLL
Imports CostCarriersLib.BLL.BRAIN
Imports WebGrease.Css

Namespace Controllers
    Public Class CostCarriersController
        Inherits BaseController

#Region "Propiedades"

        ''' <summary>
        ''' Lista de ids de steps a validar
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        Public Property ListaIdStepsValidar() As List(Of Integer)
            Get
                If (Session("ListaIdStepsValidar") Is Nothing) Then
                    Return Nothing
                Else
                    Return CType(Session("ListaIdStepsValidar"), List(Of Integer))
                End If
            End Get
            Set(ByVal value As List(Of Integer))
                Session("ListaIdStepsValidar") = value
            End Set
        End Property

        ''' <summary>
        ''' Datos validacion
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        Public Property DatosValidacion As DatosValidacion
            Get
                If (Session("DatosValidacion") Is Nothing) Then
                    Return Nothing
                Else
                    Return CType(Session("DatosValidacion"), DatosValidacion)
                End If
            End Get
            Set(ByVal value As DatosValidacion)
                Session("DatosValidacion") = value
            End Set
        End Property

        ''' <summary>
        ''' Datos validacion
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        Public Property ValidacionesInfoAdicional As List(Of ELL.ValidacionInfoAdicional)
            Get
                If (Session("ValidacionesInfoAdicional") Is Nothing) Then
                    Return Nothing
                Else
                    Return CType(Session("ValidacionesInfoAdicional"), List(Of ELL.ValidacionInfoAdicional))
                End If
            End Get
            Set(ByVal value As List(Of ELL.ValidacionInfoAdicional))
                Session("ValidacionesInfoAdicional") = value
            End Set
        End Property

#End Region

#Region "Métodos"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Function Index() As ActionResult
            'Dim proyectosAfectados As List(Of ELL.ProjectAffected) = BLL.ProjectsAffectedBLL.ObtenerProyectosAfectados("16544.44568.50625.1150")
            'BLL.CabecerasCostCarrierBLL.Existe(proyectosAfectados.FirstOrDefault().Portador)
            'BLL.FlujosAprobacionBLL.ComponerFlujoAprobacion(BLL.CabecerasCostCarrierBLL.Obtener(233, False), BLL.ValidacionesLineaBLL.Obtener(4819))
            'BLL.FlujosAprobacionBLL.ComponerFlujoAprobacion(BLL.CabecerasCostCarrierBLL.Obtener(233, False), BLL.ValidacionesLineaBLL.Obtener(4818))
            'BLL.FlujosAprobacionBLL.ComponerFlujoAprobacion(BLL.CabecerasCostCarrierBLL.Obtener(211, False), BLL.ValidacionesLineaBLL.Obtener(3898))
            'BLL.FlujosAprobacionBLL.ComponerFlujoAprobacion(BLL.CabecerasCostCarrierBLL.Obtener(211, False), BLL.ValidacionesLineaBLL.Obtener(3899))
            'BLL.FlujosAprobacionBLL.ComponerFlujoAprobacion(BLL.CabecerasCostCarrierBLL.Obtener(211, False), BLL.ValidacionesLineaBLL.Obtener(3900))

            'Esto es para dar de alta metadatos de pasos ya abiertos
            'Dim validacionLinea As ELL.ValidacionLinea = BLL.ValidacionesLineaBLL.Obtener(2169)
            'Dim paso As ELL.Step = BLL.StepsBLL.Obtener(validacionLinea.IdStep)

            'Dim metadata As New ELL.BRAIN.CCMetadata With {.Empresa = "1",
            '                                        .Planta = "000",
            '                                        .CodigoPortador = "C20011",
            '                                        .FechaIni = DateTime.Now,
            '                                        .TipoPlanta = "C",
            '                                        .DenomAmpliada = "2020-023 STR EMP2 V37",
            '                                        .Negocio = "S",
            '                                        .Responsable = "Iker Llanos Zubizarreta",
            '                                        .IdResponsableSAB = 60205,
            '                                        .Producto = "STRUCTURAL PART",
            '                                        .IdProyecto = "16544.44568.38368.58293",
            '                                        .Proyecto = "STR EMP2 V37",
            '                                        .EstadoProyecto = "Offer",
            '                                        .Lantegi = "030",
            '                                        .Origen = "M"}

            'Dim ccprodfact As New ELL.BRAIN.CCProductionPlant With {.IdPlantaSAB = 47, .CodigoPortador = "C20011", .DescripcionPlanta = "Batz Zamudio", .Empresa = "1", .Planta = "0000"}
            'Dim listas As New List(Of ELL.BRAIN.CCProductionPlant)
            'listas.Add(ccprodfact)

            'Dim ccMetadataYear As New ELL.BRAIN.CCMetadataYear With {.Empresa = "1",
            '                                                .Planta = "000",
            '                                                .Anyo = DateTime.Now.Year,
            '                                                .CodigoPortador = "C20011",
            '                                                .CodigoMoneda = "90",
            '                                                .Moneda = "EUR",
            '                                                .PresupBonosPersona = 1996,
            '                                                .PresupFacturas = 0,
            '                                                .PresupViajes = 0}



            'BLL.BRAIN.CCMetadataBLL.Guardar(metadata, listas)
            'BLL.BRAIN.CCMetadataYearBLL.Guardar(ccMetadataYear)

            'BLL.ProjectsAffectedBLL.ObtenerProyectosAfectados("16544.44568.11920.8760")


            CargarTiposProyecto()
            CargarEstadosCostCarrierFiltro(Integer.MinValue)
            CargarProductosFiltro(String.Empty, Ticket.NombreUsuario)
            CargarCostCarriers(Integer.MinValue)

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="EstadoCostCarrier"></param>
        ''' <param name="Productos"></param>
        ''' <returns></returns>
        <HttpPost>
        Function Index(ByVal EstadoCostCarrier As Integer, ByVal Productos As String) As ActionResult
            CargarTiposProyecto()
            CargarEstadosCostCarrierFiltro(EstadoCostCarrier)
            CargarProductosFiltro(Productos, Ticket.NombreUsuario)

            CargarCostCarriers(EstadoCostCarrier, Productos)

            Return View()
        End Function

        Function MantenimientoPais() As ActionResult
            Dim listaPaises As List(Of String()) = ValoresStepBLL.CargarPaisesImportados()
            For Each pais In listaPaises
                pais(1) = Utils.Traducir(pais(1)).ToUpper()
            Next
            listaPaises = listaPaises.OrderBy(Function(o) o(1)).ToList()
            Return View(listaPaises)
        End Function

        Function BuscarPais(q As String) As JsonResult
            Dim listaPaises As List(Of String()) = ValoresStepBLL.CargarPaises("")
            For Each pais In listaPaises
                pais(0) = Utils.Traducir(pais(0)).ToUpper()
            Next
            listaPaises = listaPaises.OrderBy(Function(o) o(0)).ToList()
            listaPaises = listaPaises.Where(Function(o) o(0).ToString().ToLower().Contains(q.ToLower())).ToList()
            Dim lst = listaPaises.Select(Function(o) New With {.Pais = o(0), .Codigo = o(1)})
            Return Json(lst, JsonRequestBehavior.AllowGet)
        End Function

        Function AgregarPais(ByVal codigoPais As Integer, ByVal q As String) As ActionResult
            ''''agregar a bd
            ValoresStepBLL.AgregarPais(codigoPais)
            'Return View("MantenimientoPais")
            Return RedirectToAction("MantenimientoPais")
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="codigo"></param>
        ''' <returns></returns>
        Function EliminarPais(ByVal codigo As Integer) As ActionResult
            Try
                BLL.ValoresStepBLL.EliminarPais(codigo)
                MensajeInfo(Utils.Traducir("Pais eliminado correctamente"))
            Catch ex As Exception
                MensajeError(Utils.Traducir("Error al eliminar el pais"))
                log.Error("Error al eliminar pais", ex)
            End Try

            Return RedirectToAction("MantenimientoPais")
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idCabecera"></param>
        ''' <returns></returns>
        Function EnviarValidar(ByVal idCabecera As Integer) As ActionResult
#If DEBUG Then
            'ListaIdStepsValidar = New List(Of Integer)
            'ListaIdStepsValidar.Add(11724)
            'ListaIdStepsValidar.Add(11725)
            'ListaIdStepsValidar.Add(7850)
            'ListaIdStepsValidar.Add(7916)
            'ListaIdStepsValidar.Add(7855)
            'ListaIdStepsValidar.Add(7920)
#End If
            ' Si sólo tenemos un paso podría ser el paso de General info. Vamos a ver si es asi
            Dim esInfoGeneral As Boolean = False
            Dim listaSteps As New List(Of ELL.Step)
            If (ListaIdStepsValidar IsNot Nothing AndAlso ListaIdStepsValidar.Count = 1) Then
                Dim paso As ELL.Step = BLL.StepsBLL.Obtener(ListaIdStepsValidar.First)
                If (paso.EsInfoGeneral) Then
                    listaSteps.Add(paso)
                End If
                esInfoGeneral = paso.EsInfoGeneral
            End If

            If (Not esInfoGeneral) Then
                Dim listaStepsAux As New List(Of ELL.Step)
                ' Cargamos todos los steps
                For Each idStep In ListaIdStepsValidar
                    listaStepsAux.Add(BLL.StepsBLL.Obtener(idStep))
                Next

                Dim listaPresupuestosMoneda As New List(Of DatosPorMoneda)
                Dim datosPresupuesto As List(Of ELL.DatosDistribucion)
                Dim datosPresupViajes As List(Of ELL.DatosDistribucion)
                Dim datosPresupAnyos As List(Of ELL.DatosDistribucionAnyos)
                Dim jss As New JavaScriptSerializer()

                'Obtenemos los datos de presupuestos por cada moneda diferente
                Using cliente As New ServicioBonos.ServicioBonos
                    For Each idMoneda In listaStepsAux.Select(Function(f) f.IdMoneda).Distinct()
                        datosPresupuesto = jss.Deserialize(Of List(Of ELL.DatosDistribucion))(cliente.GetDatosDistribucion(listaStepsAux.FirstOrDefault.Proyecto, ELL.OrigenDatosStep.TipoDistribucion.Presupuesto, Integer.MinValue, idMoneda))
                        datosPresupViajes = jss.Deserialize(Of List(Of ELL.DatosDistribucion))(cliente.GetDatosDistribucion(listaStepsAux.FirstOrDefault.Proyecto, ELL.OrigenDatosStep.TipoDistribucion.Viaje, Integer.MinValue, idMoneda))
                        datosPresupAnyos = jss.Deserialize(Of List(Of ELL.DatosDistribucionAnyos))(cliente.GetDatosDistribucionPorAnyos(listaStepsAux.FirstOrDefault.Proyecto, ELL.OrigenDatosStep.TipoDistribucion.Planificacion, Integer.MinValue, idMoneda))

                        listaPresupuestosMoneda.Add(New DatosPorMoneda With {.IdMoneda = idMoneda, .DatosPresupuesto = datosPresupuesto, .DatosPresupViajes = datosPresupViajes, .DatosPresupAnyos = datosPresupAnyos})
                    Next
                End Using

                Dim datosPorMoneda As DatosPorMoneda
                If (ListaIdStepsValidar IsNot Nothing) Then
                    For Each paso In listaStepsAux
                        datosPorMoneda = listaPresupuestosMoneda.FirstOrDefault(Function(f) f.IdMoneda = paso.IdMoneda)

                        paso._datosPresupuesto = datosPorMoneda.DatosPresupuesto.FirstOrDefault(Function(f) f.Estado = paso.Estado AndAlso f.IdGrupoDistrib = paso.IdBonos)
                        paso._datosPresupViajes = datosPorMoneda.DatosPresupViajes.FirstOrDefault(Function(f) f.Estado = paso.Estado)
                        paso._datosPresupAnyos = datosPorMoneda.DatosPresupAnyos.Where(Function(f) f.Estado = paso.Estado AndAlso f.IdGrupoDistrib = paso.IdBonos).ToList()

                        paso.CargarValoresStep()

                        ' Sólo se envia a validar si el Budget approved no es 0 o si siendo 0 el PBC no es 0 (Maite Olivares 18/11/2020)
                        If ((paso.BACGastos <> Integer.MinValue AndAlso paso.BACGastos <> 0) OrElse ((paso.BACGastos = Integer.MinValue OrElse paso.BACGastos = 0) AndAlso (paso.PBC <> 0 OrElse paso.IdEstadoValidacion = ELL.Validacion.Estado.Opened))) Then
                            listaSteps.Add(paso)
                        End If
                    Next
                End If

                If (listaSteps.Count = 0) Then
                    MensajeAlerta(Utils.Traducir("No se puede enviar steps a validar con presupuesto aprobado a 0"))
                    Return Editar(idCabecera)
                End If
            End If

            Dim cabecera As ELL.CabeceraCostCarrier = BLL.CabecerasCostCarrierBLL.Obtener(listaSteps.FirstOrDefault.IdCabecera, False)

            ' Cargamos los valores de additional info sólo para los valores de awarding offer si no están ya cargados
            ' Si la planta es CORPORATIVO no es necesario
            ' Sólo tiene sentido para los proyectos de Industrialización 
            If (cabecera.IdTipoProyecto = ELL.TipoProyecto.TipoProyecto.Industrialization) Then
                'ValidacionesInfoAdicional = New List(Of ELL.ValidacionInfoAdicional)

                'Vamos a obtener todas las plantas que no sean corporativo a ver si tienen informados los valores de additional info
                Dim plantasAI As List(Of ELL.Planta) = cabecera.Plantas.Where(Function(f) f.IdPlanta <> 0).ToList()
                Dim valInfoAdicional As ELL.ValidacionInfoAdicional
                For Each plantaAI In plantasAI
                    If (Not ValidacionesInfoAdicional Is Nothing AndAlso Not ValidacionesInfoAdicional.Exists(Function(f) f.IdCabecera = cabecera.Id AndAlso f.IdPlanta = plantaAI.IdPlanta And f.Tipo = ELL.ValidacionInfoAdicional.TipoDato.Awarding_offer)) Then
                        valInfoAdicional = BLL.ValidacionesInfoAdicionalBLL.CargarListado(cabecera.Id, plantaAI.IdPlanta).FirstOrDefault(Function(f) f.Tipo = ELL.ValidacionInfoAdicional.TipoDato.Awarding_offer)

                        If (valInfoAdicional IsNot Nothing) Then
                            ValidacionesInfoAdicional.Add(valInfoAdicional)
                        End If
                    End If
                Next
            End If

            ViewData("UltimaValidacion") = BLL.ValidacionesBLL.ObtenerUltimoPorCabecera(idCabecera)
            ViewData("Steps") = listaSteps
            ViewData("ContainerFluid") = 1
            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idCabecera"></param>
        ''' <returns></returns>
        Function EnviarFacturar(ByVal idCabecera As Integer) As ActionResult
            ViewData("Pedidos") = BLL.PedidosBLL.CargarListado(idCabecera)
            ViewData("IdCabecera") = idCabecera

#If DEBUG Then
            Session("ListaIdStepsValidar") = New List(Of Integer)({10179})
#End If
            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idPedido"></param>
        ''' <returns></returns>
        Function AsociarStepsPedido(ByVal idPedido As Integer) As ActionResult
            ViewData("Pedido") = BLL.PedidosBLL.Obtener(idPedido)
            ViewData("LineasPedido") = BLL.LineasPedidoBLL.CargarListado(idPedido)

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idCabecera"></param>
        ''' <param name="idPedido"></param>
        ''' <param name="comentarios"></param>
        ''' <returns></returns>
        <HttpPost>
        Function AsociarStepsPedido(ByVal idCabecera As Integer, ByVal idPedido As Integer, ByVal comentarios As String) As ActionResult
            Dim lineasPedido As New List(Of ELL.LineaPedido)
            Dim lineaPedido As ELL.LineaPedido
            Dim idStep As Integer
            Dim pedido As ELL.Pedido = BLL.PedidosBLL.Obtener(idPedido)

            For Each key In Request.Params.AllKeys.Where(Function(f) f.StartsWith("txtPosiciones-") OrElse f.StartsWith("txtPorcentaje-") OrElse f.StartsWith("txtImporteTotal-") OrElse f.StartsWith("ddlPais-"))
                idStep = key.Split("-")(1)
                lineaPedido = lineasPedido.FirstOrDefault(Function(f) f.IdStep = idStep)

                If (lineaPedido Is Nothing) Then
                    lineaPedido = New ELL.LineaPedido()
                    lineaPedido.IdPedido = idPedido
                    lineaPedido.IdStep = idStep
                    lineaPedido.IdCabecera = idCabecera
                    If (pedido IsNot Nothing) Then
                        lineaPedido.NumPedido = pedido.NumPedido
                    End If
                    lineasPedido.Add(lineaPedido)
                End If

                If (key.StartsWith("txtPosiciones-")) Then
                    lineaPedido.Posiciones = Request.Params(key)
                ElseIf (key.StartsWith("txtPorcentaje-")) Then
                    lineaPedido.Porcentaje = Request.Params(key)
                ElseIf (key.StartsWith("txtImporteTotal-")) Then
                    lineaPedido.Importe = Request.Params(key).Replace(",", ".")
                ElseIf (key.StartsWith("ddlPais-")) Then
                    Dim code = Request.Params(key)
                    If String.IsNullOrEmpty(code) Then
                        MensajeError(Utils.Traducir("Debes elegir un Mean location"))
                        Return AsociarStepsPedido(idPedido)
                    Else
                        lineaPedido.CodigoPais = CInt(code)
                    End If
                End If
            Next
            Try
                BLL.LineasPedidoBLL.Guardar(lineasPedido, ELL.Pedido.EstadoFacturacion.Sent_to_invoice, Ticket.IdUser)
                BLL.PedidosBLL.ActualizarComentarios(idPedido, comentarios)
                ListaIdStepsValidar.Clear()
            Catch ex As Exception
                MensajeError(Utils.Traducir("Error al enviar pasos enviados a facturar"))
                Return AsociarStepsPedido(idPedido)
            End Try

            Try
                ' Enviar mail a comercial/es de planta. No se pueden seleccioanr pasos de diferentes plantas
                Dim idPlanta As Integer = BLL.StepsBLL.Obtener(lineasPedido.First.IdStep).IdPlantaSAB
                Dim usuariosRoles As List(Of ELL.UsuarioRol) = BLL.UsuariosRolBLL.CargarListadoPorRol(ELL.Rol.TipoRol.Comercial).Where(Function(f) f.IdPlanta = idPlanta).ToList()

                If (usuariosRoles IsNot Nothing AndAlso usuariosRoles.Count > 0) Then
                    EnviarEmailComercial(usuariosRoles.Select(Function(f) f.Email).Distinct().ToList(), lineasPedido, comentarios)
                End If

                MensajeInfo(Utils.Traducir("Pasos enviados a facturar"))

                Return RedirectToAction("EnviarFacturar", New With {.idCabecera = idCabecera})
            Catch ex As Exception
                MensajeError(Utils.Traducir("Error al enviar notificación a comercial"))
                Return AsociarStepsPedido(idPedido)
            End Try

            Return AsociarStepsPedido(idPedido)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="producto"></param>
        Private Sub CargarCostCarriers(ByVal EstadoCostCarrier As Integer, Optional ByVal producto As String = Nothing)
            Dim prop As String = Ticket.NombreUsuario
            Dim tiposProyectoPTKSIS As New List(Of String)

            If (RolesUsuario.Exists(Function(f) f.IdRol = ELL.Rol.TipoRol.Project_manager)) Then
                prop = String.Empty
                tiposProyectoPTKSIS.Add(ELL.CabeceraCostCarrier.TIPO_PROY_INDUSTRIALIZATION)
            ElseIf (RolesUsuario.Exists(Function(f) f.IdRol = ELL.Rol.TipoRol.Responsable_advance)) Then
                prop = String.Empty
                tiposProyectoPTKSIS.Add(ELL.CabeceraCostCarrier.TIPO_PROY_R_D)
            ElseIf (RolesUsuario.Exists(Function(f) f.IdRol = ELL.Rol.TipoRol.Responsable_ingenieria_planta AndAlso f.IdPlanta = 0)) Then
                prop = String.Empty
                tiposProyectoPTKSIS.Add(ELL.CabeceraCostCarrier.TIPO_PROY_PREDEVELOPMENT)
            End If

            Dim costCarriers As IEnumerable(Of ELL.CabeceraCostCarrier) = BLL.CabecerasCostCarrierBLL.CargarListado(prop, tiposProyectoPTKSIS)

            If (EstadoCostCarrier <> Integer.MinValue) Then
                costCarriers = costCarriers.Where(Function(F) F.Abierto = EstadoCostCarrier)
            End If

            If (Not String.IsNullOrEmpty(producto)) Then
                costCarriers = costCarriers.Where(Function(F) F.Producto = producto)
            End If

            ViewData("CostCarriers") = costCarriers.ToList()
        End Sub

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
        ''' <param name="propietario"></param>
        ''' <returns></returns>
        Public Function CargarProductos(ByVal idTipoProyecto As Integer, ByVal propietario As String) As JsonResult
            Dim prop As String = propietario

            ' El Project Leader Manager puede crear y editar cualquier proyecto
            If (RolesUsuario.Exists(Function(f) f.IdRol = ELL.Rol.TipoRol.Project_manager)) Then
                prop = String.Empty
            End If

            Dim productos As List(Of ELL.Producto) = BLL.ProductosBLL.CargarListado(idTipoProyecto, prop).OrderBy(Function(f) f.Producto).ToList()

            Return Json(productos, JsonRequestBehavior.AllowGet)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="productoSeleccionado"></param>
        ''' <param name="propietario"></param>
        ''' <returns></returns>
        Public Function CargarProductosFiltro(ByVal productoSeleccionado As String, ByVal propietario As String) As JsonResult
            Dim prop As String = propietario

            ' El Project Leader Manager puede crear y editar cualquier proyecto
            If (RolesUsuario.Exists(Function(f) f.IdRol = ELL.Rol.TipoRol.Project_manager)) Then
                prop = String.Empty
            End If

            ' Cargamos los productos para los 3 tipos de proyectos
            Dim productos As List(Of ELL.Producto) = BLL.ProductosBLL.CargarListado(ELL.TipoProyecto.TipoProyecto.Industrialization, prop)
            productos.AddRange(BLL.ProductosBLL.CargarListado(ELL.TipoProyecto.TipoProyecto.R_D, prop))
            productos.AddRange(BLL.ProductosBLL.CargarListado(ELL.TipoProyecto.TipoProyecto.Predev, prop))

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

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="producto"></param>
        ''' <param name="idTipoProyecto"></param>
        ''' <param name="propietario"></param>
        ''' <returns></returns>
        Public Function CargarProyectos(ByVal producto As String, ByVal idTipoProyecto As Integer, ByVal propietario As String) As JsonResult
            Dim prop As String = propietario

            ' El Project Leader Manager puede crear y editar cualquier proyecto
            If (RolesUsuario.Exists(Function(f) f.IdRol = ELL.Rol.TipoRol.Project_manager)) Then
                prop = String.Empty
            End If

            Dim proyectos As List(Of ELL.Proyecto) = BLL.ProyectosBLL.CargarListado(producto, idTipoProyecto, prop).OrderBy(Function(f) f.Project).ToList()

            Return Json(proyectos, JsonRequestBehavior.AllowGet)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="tipoProyecto"></param>
        ''' <param name="selectAgregarProyecto"></param>
        ''' <param name="hIdOffer"></param> 
        ''' <returns></returns>
        Function AgregarCostCarrier(ByVal tipoProyecto As Integer, ByVal selectAgregarProyecto As String, ByVal hIdOffer As String) As JsonResult
            ' Hay que verificar para ese tipo de proyecto y proyecto no haya ya una entrada
            Dim cabecera As New ELL.CabeceraCostCarrier With {.IdTipoProyecto = tipoProyecto, .Proyecto = selectAgregarProyecto}
            Try
                If (BLL.CabecerasCostCarrierBLL.Existe(tipoProyecto, selectAgregarProyecto)) Then
                    Return Json(New With {.messageType = "alerta", .message = Utils.Traducir("Ya existe un portador de coste con ese tipo de proyecto y proyecto")})
                Else
                    Dim idOferta As Integer = Integer.MinValue
                    If (Not String.IsNullOrEmpty(hIdOffer)) Then
                        idOferta = hIdOffer
                    End If

                    BLL.CabecerasCostCarrierBLL.Guardar(cabecera, hIdOffer)

                    If (cabecera.Id = Integer.MinValue) Then
                        Return Json(New With {.messageType = "alerta", .message = Utils.Traducir("No se ha podido crear el portador de coste")})
                    End If
                End If
            Catch ex As Exception
                log.Error("Error al agregar el cost carrier", ex)
                Return Json(New With {.messageType = "error", .message = Utils.Traducir("Error al crear el portador de coste")})
            End Try

            Return Json(New With {.messageType = "info", .message = Utils.Traducir("Portador de coste creado correctamente"), .id = cabecera.Id})
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idCabecera"></param>
        ''' <returns></returns>
        Function Eliminar(ByVal idCabecera As Integer) As ActionResult
            Try
                BLL.CabecerasCostCarrierBLL.Eliminar(idCabecera)
                MensajeInfo(Utils.Traducir("Portador de coste eliminado correctamente"))
            Catch ex As Exception
                MensajeError(Utils.Traducir("Error al eliminar el portador de coste"))
                log.Error("Error al eliminar  cost carrier", ex)
            End Try

            Return RedirectToAction("Index")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idCabecera"></param>
        ''' <param name="recargarStepsOT"></param>
        ''' <returns></returns>
        Function Editar(ByVal idCabecera As Integer?, Optional ByVal recargarStepsOT As Boolean = False, Optional ByVal cargarResumenCambios As Boolean = False) As ActionResult
            If (idCabecera Is Nothing) Then
                Return RedirectToAction("Index")
            End If

            Dim cabecera As ELL.CabeceraCostCarrier = BLL.CabecerasCostCarrierBLL.Obtener(idCabecera, recargarStepsOT)

            'ViewData("ResumenCambios") = Nothing
            'If (cargarResumenCambios) Then
            '    ViewData("ResumenCambios") = ObtenerResumenCambios(cabecera)
            'End If

            ViewData("ContainerFluid") = 1
            ViewData("Titulo") = String.Format("{0} ({1} / {2}){3}", Utils.Traducir("Editar portador de coste"), cabecera.TipoProyecto, cabecera.NombreProyecto, If(Not String.IsNullOrEmpty(cabecera.Abreviatura), " (" & cabecera.Abreviatura & ")", String.Empty))
            ViewData("Cabecera") = cabecera

            Return View("Editar")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idCabecera"></param>
        ''' <param name="idPlanta"></param>
        ''' <param name="beneficioHerramental"></param>
        ''' <param name="soloLectura"></param>
        ''' <param name="cargarSoloSesion"></param>
        ''' <returns></returns>
        Function VerInfoAdicional(ByVal idCabecera As Integer, ByVal idPlanta As Integer, ByVal beneficioHerramental As Decimal, ByVal soloLectura As Boolean, ByVal cargarSoloSesion As Boolean) As ActionResult
            Dim viewDataDictonaryInfo As New ViewDataDictionary()
            viewDataDictonaryInfo.Add("IdCabecera", idCabecera)
            viewDataDictonaryInfo.Add("IdPlanta", idPlanta)
            viewDataDictonaryInfo.Add("BeneficioHerramental", beneficioHerramental)
            viewDataDictonaryInfo.Add("SoloLectura", soloLectura)
            viewDataDictonaryInfo.Add("CargarSoloSesion", cargarSoloSesion)

            ViewData("ViewDataDictionary") = viewDataDictonaryInfo

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="hIdCabecera"></param>
        ''' <param name="submitButtonSave"></param>
        ''' <param name="submitButtonSendValidate"></param>
        ''' <param name="submitButtonSendInvoice"></param>
        ''' <param name="submitButtonCreateGeneralInfo"></param>
        ''' <param name="submitButtonUpdateGeneralInfo"></param>
        ''' <returns></returns>
        <HttpPost>
        Function Editar(ByVal hIdCabecera As Integer, ByVal submitButtonSave As String, ByVal submitButtonSendValidate As String, ByVal submitButtonSendInvoice As String, ByVal submitButtonCreateGeneralInfo As String, ByVal submitButtonUpdateGeneralInfo As String) As ActionResult
            Dim split As String()
            If (Not String.IsNullOrEmpty(submitButtonSendInvoice)) Then
                ' Validar. Cogemos todos los checkboxes marcados
                ListaIdStepsValidar = New List(Of Integer)
                For Each key In Request.Params.AllKeys.Where(Function(f) f.StartsWith("chkStep-"))
                    If (Request.Params(key).Contains("true")) Then
                        split = key.Split("-")
                        ListaIdStepsValidar.Add(split(1))
                    End If
                Next

                If (ListaIdStepsValidar.Count = 0) Then
                    MensajeInfo(Utils.Traducir("Debe marcar algún paso abierto para enviar a facturar"))
                    Return RedirectToAction("Editar", New With {.idCabecera = hIdCabecera})
                Else
                    Return RedirectToAction("EnviarFacturar", New With {.idCabecera = hIdCabecera})
                End If
            ElseIf (Not String.IsNullOrEmpty(submitButtonCreateGeneralInfo)) Then
                ' Cargamos los costgroups de la planta 0 (corporativo)
                Dim costGroups As List(Of ELL.CostGroup) = BLL.CostsGroupBLL.CargarListadoPorCabeceraPlanta(hIdCabecera, 0)

                ' De los cost group cogemos para el estado development el de OTHERS DEV
                Dim cg As ELL.CostGroup = costGroups.FirstOrDefault(Function(f) f.Estado.ToLower() = ELL.EstadoBonos.Development.ToLower() AndAlso f.Descripcion.ToLower = "others dev")

                If (cg IsNot Nothing) Then
                    Dim paso As New ELL.Step With {.Descripcion = Utils.Traducir("Información general"), .IdCostGroup = cg.Id,
                                                   .OBCOrigenDatos = ELL.OrigenDatosStep.OrigenDatosStep.Manual, .TCFormula = "0", .TCFormulaCustomer = "0",
                                                   .GastosAñoOrigenDatos = ELL.OrigenDatosStep.OrigenDatosStep.Manual, .OrigenDatoReal = 0, .EsInfoGeneral = True}

                    Try
                        BLL.StepsBLL.Guardar(paso)
                    Catch ex As Exception
                        MensajeError(Utils.Traducir("Se ha producido un error al guardar el paso de información general"))
                        Return Editar(hIdCabecera, False)
                    End Try

                    Return RedirectToAction("Editar", New With {.idCabecera = hIdCabecera, .recargarStepsOT = True, .cargarResumenCambios = True})
                Else
                    MensajeError(Utils.Traducir("No se ha encontrado cost group OTHERS DEV en Corporativo -> Desarrollo"))
                    Return Editar(hIdCabecera, False)
                End If
            ElseIf (Not String.IsNullOrEmpty(submitButtonUpdateGeneralInfo)) Then
                ' Habra que verificar que ya no pasos pendientes de validar
                If (BLL.StepsBLL.CargarListadoPorCabecera(hIdCabecera).Where(Function(f) f.IdEstadoValidacion = ELL.Validacion.Estado.Waiting_for_approval).Count > 0) Then
                    MensajeInfo(Utils.Traducir("Enviar pasos a validar no está permitido hasta que el envío anterior esté finalizado"))
                    Return RedirectToAction("Editar", New With {.idCabecera = hIdCabecera})
                Else
                    ' Validar. Cogemos el paso de "General info"
                    ListaIdStepsValidar = New List(Of Integer)

                    Dim paso As ELL.Step = BLL.StepsBLL.CargarListadoPorCabecera(hIdCabecera).FirstOrDefault(Function(f) f.EsInfoGeneral)
                    If (paso IsNot Nothing) Then
                        ListaIdStepsValidar.Add(paso.Id)
                    End If

                    If (ListaIdStepsValidar.Count = 0) Then
                        MensajeInfo(Utils.Traducir("No existe ningún paso de información general"))
                        Return RedirectToAction("Editar", New With {.idCabecera = hIdCabecera})
                    Else
                        Return RedirectToAction("EnviarValidar", New With {.idCabecera = hIdCabecera})
                    End If
                End If
            Else
                Dim valoresSteps As New List(Of ELL.ValorStep)

                For Each key In Request.Params.AllKeys.Where(Function(f) f.StartsWith("p-"))
                    split = key.Split("-")
                    If (Not String.IsNullOrEmpty(Request.Params.Get(key))) Then
                        valoresSteps.Add(New ELL.ValorStep With {.IdStep = split(1), .IdColumna = split(2), .Año = split(3), .Trimestre = split(4), .Valor = Request.Params.Get(key).Replace(".", "")})
                    Else
                        valoresSteps.Add(New ELL.ValorStep With {.IdStep = split(1), .IdColumna = split(2), .Año = split(3), .Trimestre = split(4), .Valor = Integer.MinValue})
                    End If
                Next

                Dim porcentajesStep As New List(Of KeyValuePair(Of Integer, Integer))
                For Each key In Request.Params.AllKeys.Where(Function(f) f.StartsWith("txtPorcentaje-"))
                    split = key.Split("-")
                    If (Not String.IsNullOrEmpty(Request.Params.Get(key))) Then
                        porcentajesStep.Add(New KeyValuePair(Of Integer, Integer)(split(1), Request.Params.Get(key)))
                    End If
                Next

                Try
                    BLL.ValoresStepBLL.Guardar(valoresSteps, porcentajesStep)
                Catch ex As Exception
                    MensajeError(Utils.Traducir("Se ha producido un error al guardar los datos"))
                    Return Editar(hIdCabecera, False)
                End Try

                If (Not String.IsNullOrEmpty(submitButtonSave)) Then
                    ' Guardar
                    MensajeInfo(Utils.Traducir("Portador de coste guardado correctamente"))

                    Return RedirectToAction("Editar", New With {.idCabecera = hIdCabecera, .recargarStepsOT = True, .cargarResumenCambios = True})
                ElseIf (Not String.IsNullOrEmpty(submitButtonSendValidate)) Then
                    ' Pide Maite el 12/03/2021 que si hay algo pendiente de validar que no deje enviar más cosas
                    If (BLL.StepsBLL.CargarListadoPorCabecera(hIdCabecera).Where(Function(f) f.IdEstadoValidacion = ELL.Validacion.Estado.Waiting_for_approval).Count > 0) Then
                        MensajeInfo(Utils.Traducir("Enviar pasos a validar no está permitido hasta que el envío anterior esté finalizado"))
                        Return RedirectToAction("Editar", New With {.idCabecera = hIdCabecera})
                    End If


                    '**** Aqui metemos lo de los proyectos ECO- RFC0155
                    ' Se comprueba si en PTKSIS tiene relacionado algún proyecto de serie o industrialización en la pestaña “Project Affected”. 
                    ' Cargamos el proyecto para ver si es de tipo ECO
                    Dim cabecera As ELL.CabeceraCostCarrier = BLL.CabecerasCostCarrierBLL.Obtener(hIdCabecera, False)

                    If (cabecera IsNot Nothing AndAlso cabecera.TipoProyPtksis = ELL.CabeceraCostCarrier.TIPO_PROY_ECO_BATZ) Then
                        ' Aqui llamamos al servicio de bonos para ver tiene relacionado algún proyecto de serie o industrialización en la pestaña “Project Affected”

                        Dim proyectosAfectados As List(Of ELL.ProjectAffected) = BLL.ProjectsAffectedBLL.ObtenerProyectosAfectados(cabecera.Proyecto)
                        If (proyectosAfectados.Count = 0) Then
                            MensajeInfo(Utils.Traducir("Para continuar, es necesario informar los proyectos afectados en PTKSIS"))
                            Return RedirectToAction("Editar", New With {.idCabecera = hIdCabecera})
                        End If
                    End If
                    '**************************************************


                    ' Validar. Cogemos todos los checkboxes marcados
                    ListaIdStepsValidar = New List(Of Integer)
                    For Each key In Request.Params.AllKeys.Where(Function(f) f.StartsWith("chkStep-"))
                        If (Request.Params(key).Contains("true")) Then
                            split = key.Split("-")
                            ListaIdStepsValidar.Add(split(1))
                        End If
                    Next

                    If (ListaIdStepsValidar.Count = 0) Then
                        MensajeInfo(Utils.Traducir("Debe marcar algún paso para validar"))
                        Return RedirectToAction("Editar", New With {.idCabecera = hIdCabecera})
                    Else
                        Return RedirectToAction("EnviarValidar", New With {.idCabecera = hIdCabecera})
                    End If
                End If
            End If
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idPlanta"></param>
        ''' <param name="idCostGroupOT"></param>
        ''' <returns></returns>
        Function CargarDatosCambioEstadoStep(ByVal idPlanta As Integer, ByVal idCostGroupOT As Integer) As JsonResult
            Dim listaCostGroupCambioPlantaStep As List(Of ELL.CostGroupCambioPlantaStep) = BLL.CostsGroupBLL.CargarListadoCambioPlantaStep(idPlanta, idCostGroupOT)

            Return Json(listaCostGroupCambioPlantaStep, JsonRequestBehavior.AllowGet)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="cabecera"></param>
        ''' <returns></returns>
        Function ObtenerResumenCambios(ByVal cabecera As ELL.CabeceraCostCarrier) As List(Of ResumenCambios)
            Dim lista As New List(Of ResumenCambios)
            Dim valor As Decimal = Decimal.Zero
            Dim valorValidacion As Decimal = Decimal.Zero
            Dim agregado As Boolean = False
            For Each planta In cabecera.Plantas
                For Each estado In planta.Estados
                    For Each costGroup In estado.CostGroups
                        For Each paso In costGroup.Steps
                            agregado = False

                            ' Si el paso está en algún punto de la validación cargamos sus datos
                            If (paso.IdEstadoValidacion <> Integer.MinValue) Then
                                paso.CargarValoresStep()
                                paso.CargarValoresStepValidacion()

                                If (paso.PBC <> paso.PBCValidacion OrElse paso.TC <> paso.TCValidacion OrElse paso.BACGastos <> paso.BACGastosValidacion) Then
                                    lista.Add(New ResumenCambios With {.Planta = planta.Planta, .IdEstado = estado.Id, .Estado = estado.Estado, .IdCostGroup = costGroup.Id, .CostGroup = costGroup.Descripcion, .StepName = paso.Descripcion})
                                    agregado = True
                                End If

                                If (Not agregado) Then
                                    For año As Integer = cabecera.Años.AnyoInicio To cabecera.Años.AnyoFin Step 1
                                        For cont As Integer = 1 To 2 Step 1

                                            Dim tipoColumna As ELL.Columna.Tipo
                                            If (cont = 1) Then 'Gastos
                                                tipoColumna = ELL.Columna.Tipo.Year_expenses
                                            ElseIf (cont = 2) Then 'Ingresos
                                                tipoColumna = ELL.Columna.Tipo.Year_incomes
                                            End If
                                            For trimestre As Integer = 1 To 4 Step 1
                                                valor = Decimal.Zero
                                                valorValidacion = Decimal.Zero
                                                If (paso.ValoresStep.Exists(Function(f) f.IdColumna = tipoColumna AndAlso f.Año = año AndAlso f.Trimestre = trimestre)) Then
                                                    valor = paso.ValoresStep.First(Function(f) f.IdColumna = tipoColumna AndAlso f.Año = año AndAlso f.Trimestre = trimestre).Valor
                                                End If

                                                If (paso.ValoresStepValidacion.Exists(Function(f) f.IdColumna = tipoColumna AndAlso f.Año = año AndAlso f.Trimestre = trimestre)) Then
                                                    valorValidacion = paso.ValoresStepValidacion.First(Function(f) f.IdColumna = tipoColumna AndAlso f.Año = año AndAlso f.Trimestre = trimestre).Valor
                                                End If

                                                If (valor <> valorValidacion) Then
                                                    lista.Add(New ResumenCambios With {.Planta = planta.Planta, .IdEstado = estado.Id, .Estado = estado.Estado, .IdCostGroup = costGroup.Id, .CostGroup = costGroup.Descripcion, .StepName = paso.Descripcion})
                                                End If
                                            Next
                                        Next
                                    Next
                                End If
                            End If
                        Next
                    Next
                Next
            Next

            Return lista
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="hIdStep"></param>
        ''' <param name="selectPlantaEstados"></param>
        ''' <param name="hIdCabecera"></param>
        ''' <returns></returns>
        <HttpPost>
        Function CambiarPlantaStep(ByVal hIdStep As Integer, ByVal selectPlantaEstados As Integer, ByVal hIdCabecera As Integer) As ActionResult
            Try
                BLL.StepsBLL.CambiarPlanta(hIdStep, selectPlantaEstados)
                MensajeInfo(Utils.Traducir("Paso cambiado de planta correctametne"))
                Return RedirectToAction("Editar", New With {.idCabecera = hIdCabecera})
            Catch ex As Exception
                MensajeError(Utils.Traducir("Se ha producido un error al cambiar la planta del paso"))
                Return Editar(hIdCabecera, False)
            End Try
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idStep"></param>
        ''' <param name="descripcion"></param>
        ''' <returns></returns>
        Function CambiarDescripcionStep(ByVal idStep As Integer, ByVal descripcion As String) As JsonResult
            Try
                BLL.StepsBLL.CambiarDescripcionStep(idStep, descripcion)
            Catch ex As Exception
                log.Error("Error al cambiar la descrición del step", ex)
                Return Json(New With {.messageType = "error", .message = Utils.Traducir("Error al cambiar la descripción del paso")})
            End Try

            Return Json(New With {.messageType = "info", .message = Utils.Traducir("Descripción del paso cambiada correctamente")})
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idStep"></param>
        ''' <param name="porcentaje"></param>
        ''' <returns></returns>
        Function CambiarPorcentajeStep(ByVal idStep As Integer, ByVal porcentaje As Integer) As JsonResult
            Try
                BLL.StepsBLL.CambiarPorcentajeStep(idStep, porcentaje)
            Catch ex As Exception
                log.Error("Error al cambiar el porcentaje del step", ex)
                Return Json(New With {.messageType = "error", .message = Utils.Traducir("Error al cambiar el porcentaje del paso")})
            End Try

            Return Json(New With {.messageType = "info", .message = Utils.Traducir("Porcentaje del paso cambiado correctamente")})
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="hIdCabecera"></param>
        ''' <param name="hCostGroupParaStep"></param>
        ''' <returns></returns>
        <HttpPost>
        Function AgregarStep(ByVal hIdCabecera As Integer, ByVal hCostGroupParaStep As Integer) As ActionResult
            Try
                Dim listaSteps As New List(Of ELL.Step)
                For Each params In Request.Params.AllKeys.Where(Function(f) f.Contains("txtAgregarDescripcionStep-"))
                    ' Sólo recogemos los campos con valor
                    If (Not String.IsNullOrEmpty(Request.Params.Get(params))) Then
                        listaSteps.Add(New ELL.Step With {.Descripcion = Request.Params.Get(params), .IdCostGroup = hCostGroupParaStep,
                                                        .OBCOrigenDatos = ELL.OrigenDatosStep.OrigenDatosStep.Manual, .GastosAñoOrigenDatos = ELL.OrigenDatosStep.OrigenDatosStep.Manual,
                                                        .IngresosAñoOrigenDatos = ELL.OrigenDatosStep.OrigenDatosStep.Manual, .Origen = ELL.Step.OrigenStep.DeSolicitud, .OrigenDatoReal = ELL.StepPlantilla.DatoRealOrigen.EXT})
                    End If
                Next

                BLL.StepsBLL.GuardarLista(listaSteps)
            Catch ex As Exception
                log.Error("Error al guardar el step", ex)
                MensajeError(Utils.Traducir("Error al guardar el paso"))
            End Try

            Return RedirectToAction("Editar", New With {.idCabecera = hIdCabecera, .idCostGroup = hCostGroupParaStep, .cargarResumenCambios = True})
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        Function EliminarStep(ByVal id As Integer) As JsonResult
            Try
                BLL.StepsBLL.Eliminar(id)
            Catch ex As Exception
                log.Error("Error al eliminar el step", ex)
                Return Json(New With {.messageType = "error", .message = Utils.Traducir("Error al eliminar el paso")})
            End Try

            Return Json(New With {.messageType = "info", .message = Utils.Traducir("Paso eliminado correctamente")})
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="denominacion"></param>
        ''' <param name="descripcion"></param>
        ''' <param name="previstoPg"></param>
        Sub GuardarDatosValidacion(ByVal denominacion As String, ByVal descripcion As String, ByVal previstoPg As Integer)
            DatosValidacion = New DatosValidacion With {.Denominacion = denominacion, .Descripcion = descripcion, .PrevistoPG = previstoPg}
        End Sub


        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="denominacion"></param>
        ''' <param name="descripcion"></param>
        ''' <param name="previstoPg"></param>
        ''' <param name="submitButtonBack"></param>
        ''' <param name="submitButtonSendValidate"></param>
        ''' <returns></returns>
        <HttpPost>
        Function EnviarValidar(ByVal hfIdCabecera As String, ByVal denominacion As String, ByVal descripcion As String, ByVal previstoPg As Boolean, ByVal submitButtonBack As String, ByVal submitButtonSendValidate As String) As ActionResult
            If (Not String.IsNullOrEmpty(submitButtonBack)) Then
                DatosValidacion = New DatosValidacion With {.Denominacion = denominacion, .Descripcion = descripcion, .PrevistoPG = previstoPg}

                Return RedirectToAction("Editar", "CostCarriers", New With {.idCabecera = hfIdCabecera, .recargarStepsOT = True})
            ElseIf (Not String.IsNullOrEmpty(submitButtonSendValidate)) Then
                DatosValidacion = Nothing
                Try
                    Dim cabecera As ELL.CabeceraCostCarrier = BLL.CabecerasCostCarrierBLL.Obtener(CInt(hfIdCabecera), False)

                    If (String.IsNullOrEmpty(cabecera.Abreviatura)) Then
                        MensajeAlerta(Utils.Traducir("El proyecto no tiene informada la abreviatura"))
                        Return EnviarValidar(hfIdCabecera)
                    End If

                    ' Vamos a recoger los valores de info adicional de las plantas. Los acabados el PV no los cogemos que son los previous values
                    Dim valores As List(Of String)
                    Dim tipo As Integer
                    Dim infoAdic As ELL.ValidacionInfoAdicional
                    For Each params In Request.Params.AllKeys.Where(Function(f) Not f.Contains("PV") AndAlso (f.StartsWith("txtMargenNeto") OrElse f.StartsWith("txtVentasEfectivas") OrElse
                            f.StartsWith("txtBeneficioHerramental") OrElse f.StartsWith("txtPlantasCliente") OrElse f.StartsWith("txtSOP") OrElse
                            f.StartsWith("txtVolumenMedio") OrElse f.StartsWith("txtAnyosSerie")))
                        ' 0 - nombre del parametro / 1 - idCabecera / 2 - idPlanta
                        valores = params.Split("-").ToList()
                        tipo = If(valores(0).Contains("AO"), ELL.ValidacionInfoAdicional.TipoDato.Awarding_offer, ELL.ValidacionInfoAdicional.TipoDato.Current_values)

                        If (ValidacionesInfoAdicional Is Nothing) Then
                            ValidacionesInfoAdicional = New List(Of ELL.ValidacionInfoAdicional)
                            infoAdic = Nothing
                        Else
                            infoAdic = ValidacionesInfoAdicional.FirstOrDefault(Function(f) f.IdCabecera = valores(1) AndAlso f.IdPlanta = valores(2) AndAlso f.Tipo = tipo)
                        End If

                        If (infoAdic Is Nothing) Then
                            infoAdic = New ELL.ValidacionInfoAdicional With {.IdCabecera = valores(1), .IdPlanta = valores(2), .Tipo = tipo}
                            ValidacionesInfoAdicional.Add(infoAdic)
                        End If

                        Select Case True
                            Case valores(0).StartsWith("txtMargenNeto")
                                infoAdic.NetMargin = Request.Params.Get(params).Replace(".", String.Empty).Replace(",", ".")
                            Case valores(0).StartsWith("txtVentasEfectivas")
                                infoAdic.EffectiveSales = Request.Params.Get(params).Replace(".", String.Empty)
                            Case valores(0).StartsWith("txtBeneficioHerramental")
                                infoAdic.CustomerProperty = Request.Params.Get(params).Replace(".", String.Empty).Replace(",", ".")
                            Case valores(0).StartsWith("txtPlantasCliente")
                                infoAdic.CustomerPlants = Request.Params.Get(params)
                            Case valores(0).StartsWith("txtSOP")
                                infoAdic.SOP = CDate(Request.Params.Get(params).Replace("‎", String.Empty)) ' Hago este replace porque hay algún caracter extraño en la fecha
                            Case valores(0).StartsWith("txtVolumenMedio")
                                infoAdic.AverageVolumen = Request.Params.Get(params).Replace(".", String.Empty)
                            Case valores(0).StartsWith("txtAnyosSerie")
                                infoAdic.SeriesYears = Request.Params.Get(params).Replace(".", String.Empty)
                        End Select
                    Next

                    Dim validacion As New ELL.Validacion With {.IdUser = Ticket.IdUser, .Denominacion = denominacion, .Descripcion = descripcion,
                                                               .PrevistoPG = previstoPg, .IdCabecera = CInt(hfIdCabecera), .ValidacionesLinea = New List(Of ELL.ValidacionLinea)}

                    ' Por cada step una entrada en VALIDACION_LINEA
                    Dim validacionLinea As ELL.ValidacionLinea
                    Dim tipoColumna As ELL.Columna.Tipo
                    Dim valor As Integer
                    Dim nombre As String = String.Empty
                    Dim plantas As New List(Of Integer)
                    Dim idPlanta As Integer = Integer.MinValue
                    Dim horas As Integer = Decimal.Zero
                    Dim horasCostGroup As New List(Of KeyValuePair(Of Integer, Decimal))

                    Dim listaSteps As New List(Of ELL.Step)
                    ' Cargamos todos los steps
                    For Each idStep In ListaIdStepsValidar
                        listaSteps.Add(BLL.StepsBLL.Obtener(idStep))
                    Next

                    Dim listaPresupuestosMoneda As New List(Of DatosPorMoneda)
                    Dim datosPresupuesto As List(Of ELL.DatosDistribucion)
                    Dim datosPresupViajes As List(Of ELL.DatosDistribucion)
                    Dim datosPresupAnyos As List(Of ELL.DatosDistribucionAnyos)
                    Dim jss As New JavaScriptSerializer()

                    'Obtenemos los datos de presupuestos por cada moneda diferente
                    Using cliente As New ServicioBonos.ServicioBonos
                        For Each idMoneda In listaSteps.Select(Function(f) f.IdMoneda).Distinct()
                            datosPresupuesto = jss.Deserialize(Of List(Of ELL.DatosDistribucion))(cliente.GetDatosDistribucion(listaSteps.FirstOrDefault.Proyecto, ELL.OrigenDatosStep.TipoDistribucion.Presupuesto, Integer.MinValue, idMoneda))
                            datosPresupViajes = jss.Deserialize(Of List(Of ELL.DatosDistribucion))(cliente.GetDatosDistribucion(listaSteps.FirstOrDefault.Proyecto, ELL.OrigenDatosStep.TipoDistribucion.Viaje, Integer.MinValue, idMoneda))
                            datosPresupAnyos = jss.Deserialize(Of List(Of ELL.DatosDistribucionAnyos))(cliente.GetDatosDistribucionPorAnyos(listaSteps.FirstOrDefault.Proyecto, ELL.OrigenDatosStep.TipoDistribucion.Planificacion, Integer.MinValue, idMoneda))

                            listaPresupuestosMoneda.Add(New DatosPorMoneda With {.IdMoneda = idMoneda, .DatosPresupuesto = datosPresupuesto, .DatosPresupViajes = datosPresupViajes, .DatosPresupAnyos = datosPresupAnyos})
                        Next
                    End Using

                    Dim datosPorMoneda As DatosPorMoneda
                    For Each paso In listaSteps
                        horas = 0
                        datosPorMoneda = listaPresupuestosMoneda.FirstOrDefault(Function(f) f.IdMoneda = paso.IdMoneda)

                        paso._datosPresupuesto = datosPorMoneda.DatosPresupuesto.FirstOrDefault(Function(f) f.Estado = paso.Estado AndAlso f.IdGrupoDistrib = paso.IdBonos)
                        paso._datosPresupViajes = datosPorMoneda.DatosPresupViajes.FirstOrDefault(Function(f) f.Estado = paso.Estado)
                        paso._datosPresupAnyos = datosPorMoneda.DatosPresupAnyos.Where(Function(f) f.Estado = paso.Estado AndAlso f.IdGrupoDistrib = paso.IdBonos).ToList()

                        paso.CargarValoresStep()

                        ' Si es Offer budget es 0 no se envía a validar
                        'If (paso.BACGastos <> Integer.MinValue AndAlso paso.BACGastos <> 0) Then
                        If ((paso.BACGastos <> Integer.MinValue AndAlso paso.BACGastos <> 0) OrElse ((paso.BACGastos = Integer.MinValue OrElse paso.BACGastos = 0) AndAlso (paso.PBC <> 0 OrElse paso.IdEstadoValidacion = ELL.Validacion.Estado.Opened)) OrElse paso.EsInfoGeneral) Then
                            If (Request.Params.AllKeys.Contains(String.Format("{0}-{1}", "txtNombre", paso.Id))) Then
                                nombre = Request.Params.Get(String.Format("{0}-{1}", "txtNombre", paso.Id))
                            Else
                                If (paso.EsInfoGeneral) Then
                                    nombre = paso.Descripcion
                                Else
                                    nombre = String.Empty
                                End If
                            End If

                            If (paso.ValoresStep.Exists(Function(f) f.IdColumna = ELL.Columna.Tipo.Offer_budget_cost)) Then
                                'valor = paso.ValoresStep.First(Function(f) f.IdColumna = ELL.Columna.Tipo.Offer_budget_cost).Valor * If(paso.Porcentaje <> Integer.MinValue, paso.Porcentaje / 100, 1)
                                valor = paso.ValoresStep.First(Function(f) f.IdColumna = ELL.Columna.Tipo.Offer_budget_cost).Valor
                            Else
                                valor = Integer.MinValue
                            End If

                            ' Con esto evitamos multiples llamadas para el mismo costgroup
                            If (paso.OBCOrigenDatos = ELL.OrigenDatosStep.OrigenDatosStep.Presupuesto_bonos) Then
                                If (horasCostGroup.Exists(Function(f) f.Key = paso.IdCostGroup)) Then
                                    horas = horasCostGroup.FirstOrDefault(Function(f) f.Key = paso.IdCostGroup).Value
                                Else
                                    horas = BLL.CostsGroupBLL.Obtener(paso.IdCostGroup).Horas

                                    ' Si hay algun paso con porcentaje...
                                    If (paso.Porcentaje <> Integer.MinValue) Then
                                        horas = horas * paso.Porcentaje / 100
                                    End If

                                    horasCostGroup.Add(New KeyValuePair(Of Integer, Decimal)(paso.IdCostGroup, horas))
                                End If
                            End If

                            validacionLinea = New ELL.ValidacionLinea With {.IdStep = paso.Id, .OfferBudget = valor, .PaidByCustomer = paso.PBC, .BudgetApproved = paso.BACGastos,
                                                                        .Hours = horas, .Nombre = nombre, .ValidacionesAño = New List(Of ELL.ValidacionAño)}

                            ' Por cada validacion línea N entradas en VALIDACION_AÑO por año, tipo de columna y trimestre
                            For año As Integer = cabecera.Años.AnyoInicio To cabecera.Años.AnyoFin Step 1
                                For cont As Integer = 1 To 2 Step 1
                                    If (cont = 1) Then 'Gastos
                                        tipoColumna = ELL.Columna.Tipo.Year_expenses
                                    ElseIf (cont = 2) Then 'Ingresos
                                        tipoColumna = ELL.Columna.Tipo.Year_incomes
                                    End If
                                    For trimestre As Integer = 1 To 4 Step 1
                                        If (paso.ValoresStep.Exists(Function(f) f.IdColumna = tipoColumna AndAlso f.Año = año AndAlso f.Trimestre = trimestre)) Then
                                            valor = paso.ValoresStep.First(Function(f) f.IdColumna = tipoColumna AndAlso f.Año = año AndAlso f.Trimestre = trimestre).Valor
                                        Else
                                            valor = 0
                                        End If

                                        validacionLinea.ValidacionesAño.Add(New ELL.ValidacionAño With {.Año = año, .Trimestre = trimestre, .IdColumna = tipoColumna, .Valor = valor})
                                    Next
                                Next
                            Next

                            validacion.ValidacionesLinea.Add(validacionLinea)

                            ' Hay que añadir el valor del target que puede ser una caja de texto
                            If (paso.ValoresStep.Exists(Function(f) f.IdColumna = ELL.Columna.Tipo.Target_cost AndAlso f.Año = 0 AndAlso f.Trimestre = 0)) Then
                                validacionLinea.ValidacionesAño.Add(New ELL.ValidacionAño With {.Año = 0, .Trimestre = 0, .IdColumna = ELL.Columna.Tipo.Target_cost, .Valor = paso.ValoresStep.First(Function(f) f.IdColumna = ELL.Columna.Tipo.Target_cost AndAlso f.Año = 0 AndAlso f.Trimestre = 0).Valor})
                                'validacion.ValidacionesLinea.Add(validacionLinea)
                            End If

                            ' Sacamos las diferentes empresas a las que pertenecen los steps
                            idPlanta = BLL.PlantasBLL.Obtener(paso.IdPlanta).IdPlanta
                            If (Not plantas.Contains(idPlanta)) Then
                                plantas.Add(idPlanta)
                            End If
                        End If
                    Next

                    ' Añadimos los valores de info adicional
                    validacion.ValidacionesInfoAdicional = ValidacionesInfoAdicional
                    Dim listaFlujoAprobacion As List(Of ELL.FlujoAprobacion) = BLL.ValidacionesBLL.Guardar(validacion)
                    Try
                        EnviarEmailValidadores(listaFlujoAprobacion.Where(Function(f) f.Orden = 1).Select(Function(f) f.Email).Distinct().ToList())
                    Catch ex As Exception
                        MensajeError(Utils.Traducir("Error al enviar mensaje a los validadores"))
                    End Try

                    MensajeInfo(Utils.Traducir("Pasos enviados a validación correctamente"))
                    If (ListaIdStepsValidar IsNot Nothing) Then
                        ListaIdStepsValidar.Clear()
                    End If

                    If (ValidacionesInfoAdicional IsNot Nothing) Then
                        ValidacionesInfoAdicional.Clear()
                    End If
                Catch ex As Exception
                    log.Error("Error al guardar la validación", ex)
                    MensajeError(Utils.Traducir("Error al guardar la validación"))
                    Return EnviarValidar(hfIdCabecera)
                End Try

                Return RedirectToAction("Index", "CostCarriers")
            End If
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="listaEmails">Emails de los validadores</param>
        Private Sub EnviarEmailValidadores(ByVal listaEmails As List(Of String))
            Try
                Dim mailto As String = String.Empty
                listaEmails.ForEach(Sub(s) mailto &= s & ";")

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
                    If (Not String.IsNullOrEmpty(mailto)) Then
                        SabLib.BLL.Utils.EnviarEmail(mailFrom, mailto, subject, body, serverEmail)
                        log.Info("EnviarEmailValidadores " & mailto)
                    Else
                        log.Info("EnviarEmailValidadores - Email vacio")
                    End If
                End If
#End If
            Catch ex As Exception
                log.Error("Se ha producido un error al enviar el mail a los validadores", ex)
                Throw ex
            End Try
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="listaEmails"></param>
        ''' <param name="lineasPedido"></param>
        ''' <param name="comentarios"></param>
        Private Sub EnviarEmailComercial(ByVal listaEmails As List(Of String), ByVal lineasPedido As List(Of ELL.LineaPedido), ByVal comentarios As String)
            Try
                Dim mailto As String = String.Empty
                listaEmails.ForEach(Sub(s) mailto &= s & ";")

                Dim body As String = Utils.LeerFicheroTexto(Server.MapPath("~\Content\PlantillasMail\StepsParaFacturar.html"))
                Dim mailFrom As String = ConfigurationManager.AppSettings("MailFrom")
                Dim uri = Url.Action("Index", "Comercial", Nothing, Request.Url.Scheme)
                Dim subject As String = Utils.Traducir("Nuevos pasos para facturar") & ": "
                Dim cabecera As ELL.CabeceraCostCarrier = BLL.CabecerasCostCarrierBLL.Obtener(lineasPedido.First.IdCabecera, False)

                If (cabecera IsNot Nothing) Then
                    subject &= cabecera.CodigoProyecto
                End If

                Dim paso As ELL.Step
                Dim cuerpo As New StringBuilder
                For Each linea In lineasPedido
                    paso = BLL.StepsBLL.Obtener(linea.IdStep)

                    cuerpo.Append("<tr>")
                    cuerpo.Append("<td>")
                    cuerpo.Append(paso.Planta)
                    cuerpo.Append("</td>")
                    cuerpo.Append("<td>")
                    cuerpo.Append(BLL.CostsGroupBLL.Obtener(paso.IdCostGroup).Descripcion)
                    cuerpo.Append("</td>")
                    cuerpo.Append("<td>")
                    cuerpo.Append(paso.CostCarrier & " - " & paso.Descripcion)
                    cuerpo.Append("</td>")
                    cuerpo.Append("<td>")
                    cuerpo.Append(linea.Posiciones)
                    cuerpo.Append("</td>")
                    cuerpo.Append("<td style='text-align: right;'>")
                    cuerpo.Append(linea.Porcentaje)
                    cuerpo.Append("</td>")
                    cuerpo.Append("<td style='text-align: right;'>")
                    cuerpo.Append(linea.Importe.ToString("N2", System.Globalization.CultureInfo.CreateSpecificCulture("es-ES")))
                    cuerpo.Append("</td>")
                    cuerpo.Append("<td>")
                    cuerpo.Append(linea.NumPedido)
                    cuerpo.Append("</td>")
                    cuerpo.Append("</tr>")
                Next

                body = String.Format(body, uri, cuerpo, comentarios, Ticket.NombreUsuario)

                Dim paramBLL As New SabLib.BLL.ParametrosBLL
                Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor

#If Not DEBUG Then
                Dim notificacionesActivas As Boolean = CBool(ConfigurationManager.AppSettings("NotificacionesActivas"))

                If (notificacionesActivas) Then
                    SabLib.BLL.Utils.EnviarEmail(mailFrom, mailto, subject, body, serverEmail)
                End If
#End If
            Catch ex As Exception
                log.Error("Se ha producido un error al enviar el mail a los comerciales", ex)
                Throw ex
            End Try
        End Sub

        '        ''' <summary>
        '        ''' 
        '        ''' </summary>
        '        ''' <param name="idPlanta"></param>
        '        ''' <param name="cabecera"></param>
        '        Private Sub EnviarEmailValidadores(ByVal idPlanta As Integer, ByVal cabecera As ELL.CabeceraCostCarrier)
        '            Try
        '                ' Cargamos los datos de los validadores de la empresa
        '                Dim usuariosRoles As List(Of ELL.UsuarioRol) = BLL.UsuariosRolBLL.CargarListadoPorPlanta(idPlanta)
        '                ' Podría darse el caso que no hubiera director de ingenieria o gerente
        '                Dim emailDirectorIngenieria As String = String.Empty
        '                Dim emailGerente As String = String.Empty
        '                Dim emailProjectLeaderManager As String = String.Empty

        '                Dim usuarioRol As ELL.UsuarioRol = usuariosRoles.FirstOrDefault(Function(f) f.IdRol = ELL.Rol.TipoRol.Responsable_ingenieria_planta)
        '                If (usuarioRol IsNot Nothing) Then
        '                    emailDirectorIngenieria = usuarioRol.Email
        '                End If

        '                usuarioRol = usuariosRoles.FirstOrDefault(Function(f) f.IdRol = ELL.Rol.TipoRol.Gerente_planta)
        '                If (usuarioRol IsNot Nothing) Then
        '                    emailGerente = usuarioRol.Email
        '                End If

        '                usuarioRol = usuariosRoles.FirstOrDefault(Function(f) f.IdRol = ELL.Rol.TipoRol.Project_manager)
        '                If (usuarioRol IsNot Nothing) Then
        '                    emailProjectLeaderManager = usuarioRol.Email
        '                End If

        '                Dim plantasBLL As New SabLib.BLL.PlantasComponent()
        '                Dim planta As SabLib.ELL.Planta = Nothing
        '                Dim nombrePlanta As String = String.Empty
        '                If (idPlanta = 0) Then
        '                    nombrePlanta = "Corporativo"
        '                Else
        '                    nombrePlanta = plantasBLL.GetPlanta(idPlanta).Nombre
        '                End If

        '                If (String.IsNullOrEmpty(emailDirectorIngenieria)) Then
        '                    log.Info(String.Format("Director de ingenieria no definido para la planta {0}", nombrePlanta))
        '                    Return
        '                End If

        '                If (String.IsNullOrEmpty(emailGerente)) Then
        '                    log.Info(String.Format("Gerente no definido para la planta {0}", nombrePlanta))
        '                    Return
        '                End If

        '                If (String.IsNullOrEmpty(emailProjectLeaderManager)) Then
        '                    log.Info(String.Format("Project leader manager no definido para la planta {0}", nombrePlanta))
        '                    Return
        '                End If

        '                If (String.IsNullOrEmpty(emailDirectorIngenieria) AndAlso String.IsNullOrEmpty(emailGerente) AndAlso String.IsNullOrEmpty(emailProjectLeaderManager)) Then
        '                    Return
        '                End If

        '                Dim uri = Url.Action("Index", "Validaciones", Nothing, Request.Url.Scheme)

        '                Dim subject As String = Utils.Traducir("Nuevos pasos para validar")

        '                If (cabecera IsNot Nothing) Then
        '                    subject &= " - " & cabecera.NombreProyecto
        '                End If

        '                Dim body As String = Utils.LeerFicheroTexto(Server.MapPath("~\Content\PlantillasMail\StepsParaValidar.html"))
        '                Dim mailFrom As String = ConfigurationManager.AppSettings("MailFrom")
        '                Dim mailto As String = String.Empty

        '                'Si el proyecto es de tipo "INDUSTRIALIZATION" primero enviamos el mail de notificación al ProjectLeader Mananger
        '                If (cabecera.TipoProyPtksis = ELL.CabeceraCostCarrier.TIPO_PROY_INDUSTRIALIZATION) Then
        '                    mailto = emailProjectLeaderManager
        '                Else
        '                    mailto = String.Format("{0};{1}", emailDirectorIngenieria, emailGerente)
        '                End If

        '                Dim paramBLL As New SabLib.BLL.ParametrosBLL
        '                Dim oParams As SabLib.ELL.Parametros = paramBLL.consultar()

        '                '***************************************            
        '                body = String.Format(body, uri)
        '                '***************************************

        '#If Not DEBUG Then
        '                Dim notificacionesActivas As Boolean = CBool(ConfigurationManager.AppSettings("NotificacionesActivas"))

        '                If (notificacionesActivas) Then
        '                    SabLib.BLL.Utils.EnviarEmail(mailFrom, mailto, subject, body, oParams.ServidorEmail)
        '                End If
        '#End If
        '            Catch ex As Exception
        '                log.Error("Se ha producido un error al enviar el mail a los validadores", ex)
        '                Throw ex
        '            End Try
        '        End Sub

        '''' <summary>
        '''' 
        '''' </summary>
        'Private Sub EnviarEmailFinanciero(ByVal idEmpresaBrain As String)
        '    Try
        '        ' Cargamos los datos de los validadores de la empresa
        '        Dim validadores As ELL.Validador = BLL.ValidadoresBLL.Obtener(idEmpresaBrain, "000")

        '        Dim uri = Url.Action("Index", "Validaciones", Nothing, Request.Url.Scheme)

        '        Dim subject As String = Utils.Traducir("Nuevos pasos para abrir")
        '        Dim body As String = Utils.LeerFicheroTexto(Server.MapPath("~\Content\PlantillasMail\StepsParaAbrir.html"))
        '        Dim mailFrom As String = ConfigurationManager.AppSettings("MailFrom")
        '        Dim mailto As String = validadores.EmailFinanciero
        '        Dim paramBLL As New SabLib.BLL.ParametrosBLL
        '        Dim oParams As SabLib.ELL.Parametros = paramBLL.consultar()

        '        '***************************************            
        '        body = String.Format(body, uri)
        '        '***************************************

        '        SabLib.BLL.Utils.EnviarEmail(mailFrom, mailto, subject, body, oParams.ServidorEmail)
        '    Catch ex As Exception
        '        log.Error("Se ha producido un error al enviar el mail a financiero", ex)
        '        Throw ex
        '    End Try
        'End Sub

        '''' <summary>
        '''' 
        '''' </summary>
        '''' <param name="hIdCabecera"></param>
        '''' <param name="txtAbreviatura"></param>
        '''' <returns></returns>
        '<HttpPost>
        'Function CambiarAbreviatura(ByVal hIdCabecera As Integer, ByVal txtAbreviatura As String) As ActionResult
        '    Try
        '        BLL.CabecerasCostCarrierBLL.CambiarAbreviatura(hIdCabecera, txtAbreviatura.ToUpper())
        '        MensajeInfo(Utils.Traducir("Abreviatura cambiada correctamente"))
        '    Catch ex As Exception
        '        log.Error("Error al cambiar la abreviatura del proyecto", ex)
        '        MensajeError(Utils.Traducir("Error al cambiar la abreviatura del proyecto"))
        '    End Try

        '    Return RedirectToAction("Editar", New With {.idCabecera = hIdCabecera})
        'End Function

        '''' <summary>
        '''' 
        '''' </summary>
        '''' <param name="hIdCabecera"></param>
        '''' <param name="txtAbreviatura"></param>
        '''' <param name="SOP"></param>
        '''' <param name="anyosSerie"></param>
        '''' <returns></returns>
        'Function CambiarAbreviatura(ByVal hIdCabecera As Integer, ByVal txtAbreviatura As String, ByVal SOP As String, ByVal anyosSerie As String) As JsonResult
        '    Dim ret
        '    Try
        '        BLL.CabecerasCostCarrierBLL.CambiarAbreviatura(hIdCabecera, txtAbreviatura.ToUpper(), If(String.IsNullOrEmpty(SOP), DateTime.MinValue, CDate(SOP.Replace("‎", String.Empty))), If(String.IsNullOrEmpty(anyosSerie), Integer.MinValue, CInt(anyosSerie)))
        '        Dim cabecera As ELL.CabeceraCostCarrier = BLL.CabecerasCostCarrierBLL.Obtener(hIdCabecera, False)
        '        ret = New With {.titulo = String.Format("{0} ({1} / {2}){3}", Utils.Traducir("Editar portador de coste"), cabecera.TipoProyecto, cabecera.NombreProyecto, If(Not String.IsNullOrEmpty(cabecera.Abreviatura), " (" & cabecera.Abreviatura & ")", String.Empty)), .abreviatura = cabecera.Abreviatura, .sop = cabecera.SOP, .anyosserie = cabecera.SeriesYears}
        '    Catch ex As Exception
        '        log.Error("Error al guardar los datos de cabecera", ex)
        '        MensajeError(Utils.Traducir("Error al guardar los datos de cabecera"))
        '    End Try

        '    Return Json(ret, JsonRequestBehavior.AllowGet)
        'End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="hIdCabecera"></param>
        ''' <param name="txtAbreviatura"></param>
        ''' <returns></returns>
        Function CambiarAbreviatura(ByVal hIdCabecera As Integer, ByVal txtAbreviatura As String) As JsonResult
            Dim ret
            Try
                BLL.CabecerasCostCarrierBLL.CambiarAbreviatura(hIdCabecera, txtAbreviatura.ToUpper())
                Dim cabecera As ELL.CabeceraCostCarrier = BLL.CabecerasCostCarrierBLL.Obtener(hIdCabecera, False)
                ret = New With {.titulo = String.Format("{0} ({1} / {2}){3}", Utils.Traducir("Editar portador de coste"), cabecera.TipoProyecto, cabecera.NombreProyecto, If(Not String.IsNullOrEmpty(cabecera.Abreviatura), " (" & cabecera.Abreviatura & ")", String.Empty)), .abreviatura = cabecera.Abreviatura, .sop = cabecera.SOP, .anyosserie = cabecera.SeriesYears}
            Catch ex As Exception
                log.Error("Error al guardar los datos de cabecera", ex)
                MensajeError(Utils.Traducir("Error al guardar los datos de cabecera"))
            End Try

            Return Json(ret, JsonRequestBehavior.AllowGet)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="hIdPlanta"></param>
        ''' <param name="SOP"></param>
        ''' <param name="anyosSerie"></param>
        ''' <returns></returns>
        <HttpPost>
        Function CambiarSOP(ByVal hIdCabecera As Integer, ByVal hIdPlanta As Integer, ByVal SOP As DateTime, ByVal anyosSerie As String) As ActionResult
            Try
                BLL.PlantasBLL.CambiarSOPAñosSerie(hIdPlanta, SOP, anyosSerie)
                MensajeInfo(Utils.Traducir("SOP y años serie cambiados correctamente"))
            Catch ex As Exception
                log.Error("Error al cambiar SOP y años serie", ex)
                MensajeError(Utils.Traducir("Error al cambiar SOP y años serie"))
            End Try

            Return RedirectToAction("Editar", New With {.idCabecera = hIdCabecera})
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="hIdCabecera"></param>
        ''' <param name="txtNumPedido"></param>
        ''' <param name="txtImporteTotal"></param>
        ''' <param name="selectMoneda"></param>
        ''' <returns></returns>
        Function CrearPedido(ByVal hIdCabecera As Integer, ByVal txtNumPedido As String, ByVal txtImporteTotal As String, ByVal selectMoneda As String) As ActionResult
            Try
                Dim pedido As New ELL.Pedido With {.IdCabecera = hIdCabecera, .NumPedido = txtNumPedido, .ImporteTotal = txtImporteTotal.Replace(",", "."), .IdMoneda = selectMoneda}

                BLL.PedidosBLL.Guardar(pedido)
                MensajeInfo(Utils.Traducir("Pedido creado correctamente"))
            Catch ex As Exception
                log.Error("Error al crear pedido", ex)
                MensajeError(Utils.Traducir("Error al crear pedido"))
            End Try

            Return RedirectToAction("EnviarFacturar", New With {.idCabecera = hIdCabecera})
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idCabecera"></param>
        ''' <param name="idPlanta"></param>
        ''' <param name="idTipo"></param>
        ''' <param name="cargarSoloDeSesion">Cuando enviamos a validar solo queremos cargar los valores de sesión</param>
        ''' <returns></returns>
        Function CargarDatosValidacionInfoAdicional(ByVal idCabecera As Integer, ByVal idPlanta As Integer, ByVal idTipo As ELL.ValidacionInfoAdicional.TipoDato, Optional ByVal cargarSoloDeSesion As Boolean = False) As JsonResult
            Dim validacionInfoAdicional As ELL.ValidacionInfoAdicional = Nothing
            Try
                ' TODO: Cuando sea de oferta técnica lo valores no los mete el usuario. Me lo tiene que pasar Abel

                If (Not cargarSoloDeSesion) Then
                    validacionInfoAdicional = BLL.ValidacionesInfoAdicionalBLL.CargarListado(idCabecera, idPlanta).OrderByDescending(Function(f) f.FechaAlta).FirstOrDefault(Function(f) f.Tipo = idTipo)
                End If

                If (validacionInfoAdicional IsNot Nothing) Then
                    Return Json(validacionInfoAdicional, JsonRequestBehavior.AllowGet)
                Else
                    ' Puede ser que esté en sesion
                    If (ValidacionesInfoAdicional IsNot Nothing AndAlso ValidacionesInfoAdicional.Exists(Function(f) f.IdCabecera = idCabecera AndAlso f.IdPlanta = idPlanta AndAlso f.Tipo = idTipo)) Then
                        validacionInfoAdicional = ValidacionesInfoAdicional.FirstOrDefault(Function(f) f.IdCabecera = idCabecera AndAlso f.IdPlanta = idPlanta AndAlso f.Tipo = idTipo)
                        Return Json(validacionInfoAdicional, JsonRequestBehavior.AllowGet)
                    Else
                        Return Json(String.Empty, JsonRequestBehavior.AllowGet)
                    End If
                End If
            Catch ex As Exception
                log.Error("Error al obtener la información general de la validación", ex)
                MensajeError(Utils.Traducir("Error al obtener la información general de la validación"))
            End Try
        End Function

        '''' <summary>
        '''' Carga los datos de info adicional de sesión
        '''' </summary>
        '''' <param name="idCabecera"></param>
        '''' <param name="idPlanta"></param>
        '''' <returns></returns>
        'Function CargarDatosValidacionInfoAdicionalSesion(ByVal idCabecera As Integer, ByVal idPlanta As Integer) As JsonResult
        '    Dim validacionInfoAdicional As ELL.ValidacionInfoAdicional = Nothing

        '    If (ValidacionesInfoAdicional IsNot Nothing AndAlso ValidacionesInfoAdicional.Exists(Function(f) f.IdCabecera = idCabecera AndAlso f.IdPlanta = idPlanta AndAlso f.Tipo = ELL.ValidacionInfoAdicional.TipoDato.Current_values)) Then
        '        validacionInfoAdicional = ValidacionesInfoAdicional.FirstOrDefault(Function(f) f.IdCabecera = idCabecera AndAlso f.IdPlanta = idPlanta AndAlso f.Tipo = ELL.ValidacionInfoAdicional.TipoDato.Current_values)
        '        Return Json(validacionInfoAdicional, JsonRequestBehavior.AllowGet)
        '    Else
        '        Return Json(String.Empty, JsonRequestBehavior.AllowGet)
        '    End If
        'End Function

        ''' <summary>
        ''' Carga los datos de info adicional más reciente
        ''' </summary>
        ''' <param name="idCabecera"></param>
        ''' <param name="idPlanta"></param>
        ''' <param name="idTipo"></param>
        ''' <returns></returns>
        Function CargarDatosValidacionInfoAdicionalActual(ByVal idCabecera As Integer, ByVal idPlanta As Integer, ByVal idTipo As ELL.ValidacionInfoAdicional.TipoDato) As JsonResult
            Dim validacionInfoAdicional As ELL.ValidacionInfoAdicional = Nothing
            Try
                Dim listaValidacionInfoAdicional As List(Of ELL.ValidacionInfoAdicional) = BLL.ValidacionesInfoAdicionalBLL.CargarListado(idCabecera, idPlanta).Where(Function(f) f.Tipo = idTipo).ToList()

                If (listaValidacionInfoAdicional IsNot Nothing AndAlso listaValidacionInfoAdicional.Count > 0) Then
                    validacionInfoAdicional = BLL.ValidacionesInfoAdicionalBLL.CargarListado(idCabecera, idPlanta).OrderByDescending(Function(f) f.FechaAlta).First()
                    Return Json(validacionInfoAdicional, JsonRequestBehavior.AllowGet)
                Else
                    Return Json(String.Empty, JsonRequestBehavior.AllowGet)
                End If
            Catch ex As Exception
                log.Error("Error al obtener la información general de la validación", ex)
                MensajeError(Utils.Traducir("Error al obtener la información general de la validación"))
            End Try
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="hIdCabecera"></param>
        ''' <param name="hIdPlanta"></param>
        ''' <param name="txtMargenNeto"></param>
        ''' <param name="txtVentasEfectivas"></param>
        ''' <param name="txtBeneficioHerramental"></param>
        ''' <param name="txtPlantasCliente"></param>
        ''' <param name="txtSOP"></param>
        ''' <param name="txtVolumenMedio"></param>
        ''' <param name="txtAnyosSerie"></param>
        ''' <param name="txtMargenNetoAO"></param>
        ''' <param name="txtVentasEfectivasAO"></param>
        ''' <param name="txtBeneficioHerramentalAO"></param>
        ''' <param name="txtPlantasClienteAO"></param>
        ''' <param name="txtSOPAO"></param>
        ''' <param name="txtVolumenMedioAO"></param>
        ''' <param name="txtAnyosSerieAO"></param>
        ''' <returns></returns>
        <HttpPost>
        Function GuardarValidacionInfoAdicional(ByVal hIdCabecera As Integer, ByVal hIdPlanta As Integer, ByVal txtMargenNeto As String, ByVal txtVentasEfectivas As String,
                                                ByVal txtBeneficioHerramental As String, ByVal txtPlantasCliente As String, ByVal txtSOP As String,
                                                ByVal txtVolumenMedio As String, ByVal txtAnyosSerie As String, ByVal txtMargenNetoAO As String, ByVal txtVentasEfectivasAO As String,
                                                ByVal txtBeneficioHerramentalAO As String, ByVal txtPlantasClienteAO As String, ByVal txtSOPAO As String,
                                                ByVal txtVolumenMedioAO As String, ByVal txtAnyosSerieAO As String) As ActionResult
            If (ValidacionesInfoAdicional Is Nothing) Then
                ValidacionesInfoAdicional = New List(Of ELL.ValidacionInfoAdicional)
            End If

            Dim validacionInfoAdicional As New ELL.ValidacionInfoAdicional


            '**************** AWARDING OFFER ****************

            If (Not ValidacionesInfoAdicional.Exists(Function(f) f.IdCabecera = hIdCabecera AndAlso f.IdPlanta = hIdPlanta AndAlso f.Tipo = ELL.ValidacionInfoAdicional.TipoDato.Awarding_offer)) Then
                ValidacionesInfoAdicional.Add(New ELL.ValidacionInfoAdicional With {.IdCabecera = hIdCabecera, .IdPlanta = hIdPlanta, .Tipo = ELL.ValidacionInfoAdicional.TipoDato.Awarding_offer})
            End If

            validacionInfoAdicional = ValidacionesInfoAdicional.FirstOrDefault(Function(f) f.IdCabecera = hIdCabecera AndAlso f.IdPlanta = hIdPlanta AndAlso f.Tipo = ELL.ValidacionInfoAdicional.TipoDato.Awarding_offer)
            validacionInfoAdicional.NetMargin = If(String.IsNullOrEmpty(txtMargenNeto), Decimal.MinValue, CDec(txtMargenNetoAO.Replace(",", ".")))
            validacionInfoAdicional.EffectiveSales = If(String.IsNullOrEmpty(txtVentasEfectivasAO), Integer.MinValue, CInt(txtVentasEfectivasAO))
            validacionInfoAdicional.CustomerProperty = If(String.IsNullOrEmpty(txtBeneficioHerramentalAO), Integer.MinValue, CInt(txtBeneficioHerramentalAO))
            validacionInfoAdicional.CustomerPlants = txtPlantasClienteAO
            validacionInfoAdicional.SOP = If(String.IsNullOrEmpty(txtSOPAO), DateTime.MinValue, CDate(txtSOPAO.Replace("‎", String.Empty))) ' Hago este replace porque hay algún caracter extraño en la fecha
            validacionInfoAdicional.AverageVolumen = If(String.IsNullOrEmpty(txtVolumenMedioAO), Integer.MinValue, CInt(txtVolumenMedioAO))
            validacionInfoAdicional.SeriesYears = If(String.IsNullOrEmpty(txtAnyosSerieAO), Integer.MinValue, CInt(txtAnyosSerieAO))

            '**************** CURRENT VALUES ****************

            If (Not ValidacionesInfoAdicional.Exists(Function(f) f.IdCabecera = hIdCabecera AndAlso f.IdPlanta = hIdPlanta AndAlso f.Tipo = ELL.ValidacionInfoAdicional.TipoDato.Current_values)) Then
                ValidacionesInfoAdicional.Add(New ELL.ValidacionInfoAdicional With {.IdCabecera = hIdCabecera, .IdPlanta = hIdPlanta, .Tipo = ELL.ValidacionInfoAdicional.TipoDato.Current_values})
            End If

            validacionInfoAdicional = ValidacionesInfoAdicional.FirstOrDefault(Function(f) f.IdCabecera = hIdCabecera AndAlso f.IdPlanta = hIdPlanta AndAlso f.Tipo = ELL.ValidacionInfoAdicional.TipoDato.Current_values)
            validacionInfoAdicional.NetMargin = If(String.IsNullOrEmpty(txtMargenNeto), Decimal.MinValue, CDec(txtMargenNeto.Replace(",", ".")))
            validacionInfoAdicional.EffectiveSales = If(String.IsNullOrEmpty(txtVentasEfectivas), Integer.MinValue, CInt(txtVentasEfectivas))
            validacionInfoAdicional.CustomerProperty = If(String.IsNullOrEmpty(txtBeneficioHerramental), Integer.MinValue, CInt(txtBeneficioHerramental))
            validacionInfoAdicional.CustomerPlants = txtPlantasCliente
            validacionInfoAdicional.SOP = If(String.IsNullOrEmpty(txtSOP), DateTime.MinValue, CDate(txtSOP.Replace("‎", String.Empty))) ' Hago este replace porque hay algún caracter extraño en la fecha
            validacionInfoAdicional.AverageVolumen = If(String.IsNullOrEmpty(txtVolumenMedio), Integer.MinValue, CInt(txtVolumenMedio))
            validacionInfoAdicional.SeriesYears = If(String.IsNullOrEmpty(txtAnyosSerie), Integer.MinValue, CInt(txtAnyosSerie))

            Return RedirectToAction("EnviarValidar", New With {.idCabecera = hIdCabecera})
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idCabecera"></param>
        ''' <param name="idTipoProyecto"></param>
        ''' <param name="idProyecto"></param>
        ''' <returns></returns>
        Public Function CargarPlantasAgregar(ByVal idCabecera As Integer, ByVal idTipoProyecto As Integer, ByVal idProyecto As String) As JsonResult
            Dim listaPlantasMostrar As New List(Of SabLib.ELL.Planta)
            listaPlantasMostrar = BLL.PlantasBLL.CargarPlantasAgregar(idCabecera, idTipoProyecto, idProyecto)

            Return Json(listaPlantasMostrar, JsonRequestBehavior.AllowGet)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="selectAgregarPlanta"></param>
        ''' <param name="hIdCabecera"></param>
        ''' <param name="hIdTipoProyecto"></param>
        ''' <returns></returns>
        Public Function AgregarPlanta(ByVal selectAgregarPlanta As Integer, ByVal hIdCabecera As Integer, ByVal hIdTipoProyecto As Integer) As ActionResult
            Try
                BLL.CabecerasCostCarrierBLL.AgregarPlanta(hIdCabecera, selectAgregarPlanta, hIdTipoProyecto)
                MensajeInfo(Utils.Traducir("La planta de ha agregado correctamente al proyecto"))
            Catch ex As Exception
                log.Error("Error al agregar planta al proyecto", ex)
                MensajeError(Utils.Traducir("Error al agregar planta al proyecto"))
            End Try

            Return RedirectToAction("Index")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idStep"></param>
        ''' <returns></returns>
        Public Function MostrarLineasPedidoStep(ByVal idStep As Integer) As ActionResult
            ViewData("LineasPedido") = BLL.LineasPedidoBLL.CargarListadoPorPaso(idStep).OrderByDescending(Function(f) f.FechaAlta).ToList()

            Return View()
        End Function

#End Region
    End Class

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class DatosPorMoneda

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property IdMoneda As Integer

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property DatosPresupuesto As List(Of ELL.DatosDistribucion)

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property DatosPresupViajes As List(Of ELL.DatosDistribucion)

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property DatosPresupAnyos As List(Of ELL.DatosDistribucionAnyos)

    End Class

End Namespace