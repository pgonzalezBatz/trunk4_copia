Imports System.Globalization
Imports System.Web.Mvc

Namespace Controllers
    Public Class MetadataController
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
                roles.Add(ELL.Rol.TipoRol.Financiero)
                Return roles
            End Get
        End Property

#End Region

#Region "Métodos"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Empresas"></param>
        ''' <returns></returns>
        Function Index(ByVal Empresas As String, ByVal txtCodigo As String, ByVal numPage As Integer?) As ActionResult
            If (numPage Is Nothing) Then
                numPage = 1
            End If

            ViewData("ContainerFluid") = 1
            ViewData("PageSize") = Page_Size
            ViewData("PaginaActual") = numPage
            ViewData("Empresa") = Empresas
            ViewData("txtCodigo") = txtCodigo

            CargarComboEmpresas(Empresas)
            CargarCostCarriers(numPage - 1, Empresas, txtCodigo)

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="producto"></param>
        ''' <param name="estado"></param>
        ''' <returns></returns>
        Function Crear(Optional ByVal producto As String = "", Optional ByVal estado As String = "", Optional ByVal proyecto As String = "") As ActionResult
            ViewData("ContainerFluid") = 1

            CargarComboEmpresas()
            CargarComboTiposPlanta()
            CargarComboProductos(producto)
            CargarComboEstadosProyecto(estado)
            CargarComboProyectos(producto, proyecto)
            CargarComboActivos()
            CargarComboPropiedad()
            CargarListBoxPlantasProductivas()

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="portadorCoste"></param>
        ''' <param name="idStep"></param>
        ''' <param name="idValidacionLinea"></param>
        ''' <returns></returns>
        Function CrearDesdeFinanciero(ByVal portadorCoste As String, ByVal idStep As Integer, ByVal idValidacionLinea As Integer) As ActionResult
            ViewData("ContainerFluid") = 1

            Dim paso As ELL.Step = BLL.StepsBLL.Obtener(idStep)
            Dim validacionLineas As ELL.ValidacionLinea = BLL.ValidacionesLineaBLL.Obtener(idValidacionLinea)

            ' Obtenemos la planta del paso para cargar los datos de SOP y año serie
            ' Dim plantaPaso As ELL.Planta = BLL.PlantasBLL.Obtener(paso.IdPlanta)
            ' ViewData("PlantaPaso") = plantaPaso
            ' Ahora el SOP y los años serie vienen de la tabla VALIDACION_INFO_ADICIONAL. 
            ' Se cogen de los valores de la plantas el SOP más cercano a la fecha actual y de esa plant también su series years
            ' De las validaciones adicionales tenemos que coger las que NO sean de la planta de corporativo
            Dim idValidacion As Integer = BLL.ValidacionesLineaBLL.Obtener(idValidacionLinea).IdValidacion
            Dim validacionesInfoAdicionales As List(Of ELL.ValidacionInfoAdicional) = BLL.ValidacionesInfoAdicionalBLL.CargarListadoPorValidacion(idValidacion)
            Dim validacionInfoSeleccionada As ELL.ValidacionInfoAdicional = Nothing
            Dim diferencia As Long = Long.MaxValue
            If (validacionesInfoAdicionales IsNot Nothing AndAlso validacionesInfoAdicionales.Count > 0) Then
                For Each validacionInfo In validacionesInfoAdicionales.Where(Function(f) f.IdPlanta <> 0)
                    If (Math.Abs(DateTime.Today.Subtract(validacionInfo.SOP).Ticks) < diferencia) Then
                        validacionInfoSeleccionada = validacionInfo
                        diferencia = Math.Abs(DateTime.Today.Subtract(validacionInfo.SOP).Ticks)
                    End If
                Next
            End If

            ViewData("SOP") = DateTime.MinValue
            If (validacionInfoSeleccionada IsNot Nothing) Then
                ViewData("ValidacionInfoAdicional") = validacionInfoSeleccionada
            Else
                ' Puede ser que el SOP venga en la cabecera
                Dim cabecera As ELL.CabeceraCostCarrier = BLL.CabecerasCostCarrierBLL.Obtener(paso.IdCabecera, False)
                ViewData("SOP") = cabecera.SOP
                ViewData("AnyosSerie") = cabecera.SeriesYears
            End If

            Dim empresa As String = String.Empty
            Dim planta As String = "000"
            Dim tipoPlanta As String = String.Empty

            'Si la planta es 0 es corporativo pero aquí se va a mostrar como Igorre y tipo de planta Corporativo
            If (paso.IdPlantaSAB = 0) Then
                empresa = 1
                tipoPlanta = "C"
            Else
                If (paso.IdPlantaSAB = ELL.Planta.PLANTA_SAB_ZAMUDIO) Then
                    empresa = ELL.Planta.PLANTA_XPERT_ZAMUDIO
                Else
                    Dim plantasBLL As New SabLib.BLL.PlantasComponent()
                    empresa = plantasBLL.GetPlanta(paso.IdPlantaSAB).IdBrain
                End If
                tipoPlanta = "P"
            End If

            'CargarMetadata(empresa, planta, portadorCoste)
            Dim plantasProductivas As New List(Of ELL.BRAIN.CCProductionPlant)
            Dim listaPlantasBrain As List(Of Integer) = PlantasACargarBLL.ObtenerPlantasACargar(paso.Proyecto)

            If (listaPlantasBrain IsNot Nothing AndAlso listaPlantasBrain.Count > 0) Then
                listaPlantasBrain.ForEach(Sub(s) plantasProductivas.Add(New ELL.BRAIN.CCProductionPlant With {.IdPlantaSAB = s}))
            End If

            CargarComboEmpresas(empresa, True)
            CargarComboTiposPlanta(tipoPlanta)

            Dim producto As ELL.ProductoPtksis = BLL.ProductosPtksisBLL.ObtenerProducto(paso.Proyecto)
            CargarComboProductos(producto.Nombre)
            CargarComboEstadosProyecto(paso.Estado)
            CargarComboProyectos(producto.Nombre, paso.Proyecto)
            'TODO: Para el cálculo del activo hay que definir una lógica. Hablado con Silvia el 26/06/2019
            CargarComboActivos(If(validacionLineas.PaidByCustomer > 0 AndAlso paso.IdEstadoProyecto = ELL.EstadoProyecto.EstadoProyecto.Development, Integer.MinValue, ELL.Activo.Categoria.Non_material))
            CargarComboPropiedad(If(validacionLineas.PaidByCustomer > 0, "E", "I"))

            CargarListBoxPlantasProductivas(plantasProductivas)

            ViewData("PlantaProductivaCorporativo") = plantasProductivas.Select(Function(f) f.IdPlantaSAB).ToList.Exists(Function(f) f = 0)

            ' Cargamos el cost carrier para poder sacar el campo Datos en el popup de info
            ' Si la planta es Zamudio hay que ir a Navision para obtener los datos del cost carrier
            Dim costCarrier As ELL.BRAIN.CostCarrier
            If (paso.IdPlantaSAB = ELL.Planta.PLANTA_SAB_ZAMUDIO) Then
                costCarrier = BLL.Navision.CostCarriersBLL.Obtener(portadorCoste)
                ViewData("CostCarrier") = costCarrier
            Else
                costCarrier = BLL.BRAIN.CostCarriersBLL.Obtener(portadorCoste, empresa)
                ViewData("CostCarrier") = costCarrier
            End If

            ' Vamos a ver quien ha sido el solicitante
            Dim historicoEstado As ELL.HistoricoEstadoLinea = BLL.HistoricosEstadoLineaBLL.CargarListadoPorValidacionLinea(idValidacionLinea).OrderBy(Function(f) f.Fecha).FirstOrDefault()
            Dim usuariosBLL As New SabLib.BLL.UsuariosComponent
            Dim solicitante As SabLib.ELL.Usuario = usuariosBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = historicoEstado.IdUser})

            ViewData("Solicitante") = solicitante
            ViewData("IdStep") = idStep
            ViewData("IdValidacionLinea") = idValidacionLinea

            Return View("Crear")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="empresa"></param>
        ''' <param name="planta"></param>
        ''' <param name="codigo"></param>
        ''' <returns></returns>
        Function Editar(ByVal empresa As String, ByVal planta As String, ByVal codigo As String) As ActionResult
            ViewData("ContainerFluid") = 1

            Dim metadata As ELL.BRAIN.CCMetadata = CargarMetadata(empresa, planta, codigo)
            Dim plantasProductivas As List(Of ELL.BRAIN.CCProductionPlant) = BLL.BRAIN.CCProductionPlantBLL.CargarListado(empresa, planta, codigo)

            CargarComboEmpresas(empresa, True)
            CargarComboTiposPlanta(metadata.TipoPlanta)
            CargarComboProductos(metadata.Producto)
            CargarComboEstadosProyecto(metadata.EstadoProyecto)
            CargarComboProyectos(metadata.Producto, metadata.IdProyecto)
            CargarComboActivos(metadata.IdTipoActivo)
            CargarComboPropiedad(metadata.Propiedad)
            CargarListBoxPlantasProductivas(plantasProductivas)

            ' Cargamos el cost carrier para poder sacar el campo Datos en el popup de info
            Dim costCarrier As ELL.BRAIN.CostCarrier = BLL.BRAIN.CostCarriersBLL.Obtener(codigo, empresa)
            ViewData("CostCarrier") = costCarrier

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="empresa"></param>
        ''' <param name="codigo"></param>
        Private Sub CargarCostCarriers(ByVal numPage As Integer, Optional ByVal empresa As String = Nothing, Optional ByVal codigo As String = Nothing)
            Dim costCarriers As List(Of ELL.BRAIN.CCMetadata) = BLL.BRAIN.CCMetadataBLL.CargarListado(empresa:=empresa, codigo:=codigo)

            ViewData("NumeroDeFacturas") = costCarriers.Count
            ViewData("CostCarriers") = costCarriers.Skip(numPage * Page_Size).Take(Page_Size).ToList()
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="empresa"></param>
        ''' <param name="planta"></param>
        ''' <param name="codigo"></param>
        Private Function CargarMetadata(ByVal empresa As String, ByVal planta As String, ByVal codigo As String) As ELL.BRAIN.CCMetadata
            Dim metadata As ELL.BRAIN.CCMetadata = BLL.BRAIN.CCMetadataBLL.Obtener(empresa, planta, codigo)
            ViewData("Metadata") = metadata

            Return metadata
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="empresa"></param>
        ''' <param name="deshabilitar"></param>
        Private Sub CargarComboEmpresas(Optional ByVal empresa As String = Nothing, Optional ByVal deshabilitar As Boolean = False)
            Dim plantasBLL As New SabLib.BLL.PlantasComponent()
            Dim empresas As List(Of SabLib.ELL.Planta) = plantasBLL.GetPlantasBRAIN()

            ' Además tenemos que cargar la planta de Zamudio que no funciona con XPERT sino que funciona con Navision
            Dim plantaZamudio As SabLib.ELL.Planta = plantasBLL.GetPlanta(ELL.Planta.PLANTA_SAB_ZAMUDIO)
            If (plantaZamudio IsNot Nothing) Then
                plantaZamudio.IdBrain = ELL.Planta.PLANTA_XPERT_ZAMUDIO ' Le doy un código ficticio
                empresas.Add(plantaZamudio)
            End If

            Dim empresasLI As List(Of Mvc.SelectListItem) = empresas.Select(Function(f) New Mvc.SelectListItem With {.Text = f.Nombre, .Value = f.IdBrain, .Disabled = deshabilitar}).OrderBy(Function(f) f.Text).ToList()
            empresasLI.Insert(0, New SelectListItem With {.Text = String.Format("<{0}>", Utils.Traducir("Seleccione uno")), .Value = String.Empty, .Disabled = deshabilitar})

            If (Not String.IsNullOrEmpty(empresa) AndAlso empresasLI.Exists(Function(f) f.Value = empresa)) Then
                empresasLI.First(Function(f) f.Value = empresa).Selected = True

                If (deshabilitar) Then
                    empresasLI.First(Function(f) f.Value = empresa).Disabled = False
                End If
            End If

            ViewData("Empresas") = empresasLI
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="tipoPlanta"></param>
        Private Sub CargarComboTiposPlanta(Optional ByVal tipoPlanta As String = Nothing)
            Dim tipoPlantaLI As New List(Of Mvc.SelectListItem)

            tipoPlantaLI.Add(New SelectListItem With {.Text = Utils.Traducir("Corporativo"), .Value = "C"})
            tipoPlantaLI.Add(New SelectListItem With {.Text = Utils.Traducir("Planta"), .Value = "P"})

            If (Not String.IsNullOrEmpty(tipoPlanta) AndAlso tipoPlantaLI.Exists(Function(f) f.Value = tipoPlanta)) Then
                tipoPlantaLI.First(Function(f) f.Value = tipoPlanta).Selected = True
            End If

            ViewData("TiposPlanta") = tipoPlantaLI
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="producto"></param>
        Private Sub CargarComboProductos(Optional ByVal producto As String = Nothing)
            Dim productos As List(Of ELL.ProductoPtksis) = BLL.ProductosPtksisBLL.CargarListado()
            Dim productosLI As List(Of Mvc.SelectListItem) = productos.Select(Function(f) New Mvc.SelectListItem With {.Text = f.Nombre, .Value = f.Nombre}).OrderBy(Function(f) f.Text).ToList()

            productosLI.Insert(0, New SelectListItem With {.Text = String.Format("<{0}>", Utils.Traducir("Seleccione uno")), .Value = String.Empty})

            If (Not String.IsNullOrEmpty(producto) AndAlso productosLI.Exists(Function(f) f.Value = producto)) Then
                productosLI.First(Function(f) f.Value = producto).Selected = True
            End If

            ViewData("Productos") = productosLI
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="estado"></param>
        Private Sub CargarComboEstadosProyecto(Optional ByVal estado As String = Nothing)
            Dim estadosProyecto As List(Of ELL.EstadoBonos) = BLL.EstadosBonosBLL.CargarListadoCCMetadata()
            Dim estadoProyectoLI As List(Of Mvc.SelectListItem) = estadosProyecto.Select(Function(f) New Mvc.SelectListItem With {.Text = f.Nombre, .Value = f.Nombre}).OrderBy(Function(f) f.Text).ToList()

            If (Not String.IsNullOrEmpty(estado) AndAlso estadoProyectoLI.Exists(Function(f) f.Value = estado)) Then
                estadoProyectoLI.First(Function(f) f.Value = estado).Selected = True
            End If

            ViewData("EstadosProyecto") = estadoProyectoLI
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="activo"></param>
        Private Sub CargarComboActivos(Optional ByVal activo As Integer = Integer.MinValue)
            Dim activosBLL As New BLL.ActivosBLL()
            Dim activos As List(Of ELL.Activo) = activosBLL.CargarListado()
            Dim activosLI As List(Of Mvc.SelectListItem) = activos.Select(Function(f) New Mvc.SelectListItem With {.Text = f.Nombre, .Value = f.Id}).OrderBy(Function(f) f.Text).ToList()

            If (Not activo = Integer.MinValue AndAlso activosLI.Exists(Function(f) f.Value = activo)) Then
                activosLI.First(Function(f) f.Value = activo).Selected = True
                ViewData("hfActivo") = activosLI.First(Function(f) f.Value = activo).Text
            End If

            ViewData("Activos") = activosLI
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="empresasProductivas"></param>
        Private Sub CargarListBoxPlantasProductivas(Optional ByVal empresasProductivas As List(Of ELL.BRAIN.CCProductionPlant) = Nothing)
            Dim plantasBLL As New SabLib.BLL.PlantasComponent()
            Dim empresasAux As List(Of SabLib.ELL.Planta) = plantasBLL.GetPlantas()
            Dim empresas As New List(Of SabLib.ELL.Planta)
            Dim idSABPlantasMostrar As List(Of String) = ConfigurationManager.AppSettings("IdSABPlantasMostrar").Split(",").ToList()

            For Each idPlanta In idSABPlantasMostrar
                If (empresasAux.Exists(Function(f) f.Id = CInt(idPlanta))) Then
                    empresas.Add(empresasAux.FirstOrDefault(Function(f) f.Id = CInt(idPlanta)))
                End If
            Next

            Dim empresasLI As List(Of Mvc.SelectListItem) = empresas.Select(Function(f) New Mvc.SelectListItem With {.Text = f.Nombre, .Value = f.Id}).OrderBy(Function(f) f.Text).ToList()

            ' Si el Plant to charge es Batz Sistemas S.Coop se considera corporativo y no marcamos plantas productivas. En cualquier otro caso marcamos las plantas que vengan
            ' Para mi en el listado de empresas productivas vendrá como 0. No deberían venir plantas productivas y corporativo juntas. Sería un error.
            If (empresasProductivas IsNot Nothing AndAlso empresasProductivas.Count > 0) Then
                If (Not empresasProductivas.Select(Function(f) f.IdPlantaSAB).ToList.Exists(Function(f) f = 0)) Then
                    If (empresasProductivas IsNot Nothing AndAlso empresasProductivas.Count > 0) Then
                        For Each empresa In empresasProductivas
                            If (empresasLI.Exists(Function(f) f.Value = empresa.IdPlantaSAB)) Then
                                ' Me ha pasado de que que el CC tiene una planta que ya está obsoleta
                                empresasLI.First(Function(f) f.Value = empresa.IdPlantaSAB).Selected = True
                            End If
                        Next
                    End If
                End If
            End If

            ViewData("lbEmpresasProductivas") = empresasLI
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="propiedad"></param>
        Private Sub CargarComboPropiedad(Optional ByVal propiedad As String = Nothing)
            Dim propiedadLI As New List(Of Mvc.SelectListItem)

            propiedadLI.Add(New SelectListItem With {.Text = Utils.Traducir("Si"), .Value = "I"})
            propiedadLI.Add(New SelectListItem With {.Text = Utils.Traducir("No"), .Value = "E"})

            If (Not String.IsNullOrEmpty(propiedad) AndAlso propiedadLI.Exists(Function(f) f.Value = propiedad)) Then
                propiedadLI.First(Function(f) f.Value = propiedad).Selected = True
            End If

            ViewData("Propiedad") = propiedadLI
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="producto"></param>
        Function CargarProyectos(ByVal producto As String) As JsonResult
            Dim proyectos As List(Of ELL.ProyectoPtksis) = BLL.ProyectosPtksisBLL.CargarListado(producto)

            Return Json(proyectos, JsonRequestBehavior.AllowGet)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="producto"></param>
        ''' <param name="proyecto"></param>
        Private Sub CargarComboProyectos(ByVal producto As String, Optional ByVal proyecto As String = Nothing)
            Dim proyectosLI As New List(Of Mvc.SelectListItem)
            If (Not String.IsNullOrEmpty(producto)) Then
                Dim proyectos As List(Of ELL.ProyectoPtksis) = BLL.ProyectosPtksisBLL.CargarListado(producto)
                proyectosLI = proyectos.Select(Function(f) New Mvc.SelectListItem With {.Text = f.Nombre, .Value = f.Id}).OrderBy(Function(f) f.Text).ToList()

                If (Not String.IsNullOrEmpty(proyecto) AndAlso proyectosLI.Exists(Function(f) f.Value = proyecto)) Then
                    proyectosLI.First(Function(f) f.Value = proyecto).Selected = True
                End If
            End If

            ViewData("Proyectos") = proyectosLI
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="q"></param>
        ''' <param name="empresa"></param>
        ''' <returns></returns>
        Public Function BuscarCostCarriers(ByVal q As String, ByVal empresa As String) As JsonResult
            ' Si la empresa es Zamudio hay que ir a Navision
            Dim costCarriers As List(Of ELL.BRAIN.CostCarrier)
            If (empresa = ELL.Planta.PLANTA_XPERT_ZAMUDIO) Then
                costCarriers = BLL.Navision.CostCarriersBLL.CargarListado(q).OrderBy(Function(f) f.DescripcionCompleta).ToList()
            Else
                costCarriers = BLL.BRAIN.CostCarriersBLL.CargarListado(q, empresa).OrderBy(Function(f) f.DescripcionCompleta).ToList()
            End If

            Return Json(costCarriers, JsonRequestBehavior.AllowGet)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function BuscarUsuarios(ByVal q As String) As JsonResult
            Dim usuariosBLL As New SabLib.BLL.UsuariosComponent
            Dim plantasBLL As New SabLib.BLL.PlantasComponent
            Dim usuarios = (From users In usuariosBLL.GetUsuariosBusquedaSAB_Optimizado(q) Where ((users.FechaBaja = DateTime.MinValue OrElse users.FechaBaja > DateTime.Today) AndAlso Not String.IsNullOrEmpty(users.IdDirectorioActivo)) Select New With {.Id = users.Id, .NombreCompletoYPlanta = users.NombreCompletoYPlanta}).ToList()
            'Dim usuarios = (From users In usuariosBLL.GetUsuariosBusquedaSAB_Optimizado(q) Where (Not String.IsNullOrEmpty(users.IdDirectorioActivo)) Select New With {.Id = users.Id, .NombreCompleto = users.NombreCompleto}).ToList()

            Return Json(usuarios, JsonRequestBehavior.AllowGet)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function BuscarCodigoPresupuesto(ByVal q As String) As JsonResult
            Dim codigos As List(Of ELL.CodigoPrespuesto) = BLL.CodigosPresupuestoBLL.CargarListado(q)

            Return Json(codigos, JsonRequestBehavior.AllowGet)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        <HttpPost>
        Function Crear(ByVal Empresas As String, ByVal hfCodigo As String, ByVal txtCodigo As String, ByVal Lantegi As String, ByVal TiposPlanta As String, ByVal Denominacion As String, ByVal txtResponsable As String,
                       ByVal hfResponsable As Integer, ByVal fechaApertura As String, ByVal fechaCierre As DateTime?, ByVal fechaInicio As String, ByVal anyosSerie As String, ByVal Productos As String,
                       ByVal hfProyectos As String, ByVal Proyectos As String, ByVal EstadosProyecto As String, ByVal Activos As String, ByVal hfActivo As String, ByVal Propiedad As String, ByVal hfCodigoPresupuesto As String, ByVal txtCodigoPresupuesto As String,
                       ByVal lbEmpresasProductivas As IEnumerable(Of String), ByVal hfIdValidacionLinea As String, ByVal hfIdStep As String) As ActionResult
            Dim ccMetadata As New ELL.BRAIN.CCMetadata()

            If (BLL.BRAIN.CCMetadataBLL.Obtener(Request.Form("Empresas"), ccMetadata.Planta, Request.Form("hfCodigo")) IsNot Nothing) Then
                MensajeAlerta(Utils.Traducir("Ya existen esos metadatos para ese portador"))
                Return Crear(Productos, EstadosProyecto, Proyectos)
            End If

            Try
                ccMetadata.Empresa = Empresas
                ccMetadata.CodigoPortador = hfCodigo
                ccMetadata.Lantegi = Lantegi
                ccMetadata.TipoPlanta = TiposPlanta
                ccMetadata.DenomAmpliada = Denominacion
                ccMetadata.Responsable = txtResponsable
                ccMetadata.IdResponsableSAB = hfResponsable
                ccMetadata.FechaIni = DateTime.ParseExact(fechaApertura, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                If (hfIdValidacionLinea <> "0" AndAlso Not String.IsNullOrEmpty(hfIdStep)) Then
                    ccMetadata.Origen = ELL.BRAIN.CCMetadata.ORIGEN_AUTO
                Else
                    ccMetadata.Origen = ELL.BRAIN.CCMetadata.ORIGEN_MANUAL
                End If

                If (fechaCierre IsNot Nothing) Then
                    ccMetadata.FechaFin = fechaCierre
                End If
                If (Not String.IsNullOrEmpty(fechaInicio)) Then
                    ccMetadata.FechaEstimIni = fechaInicio
                End If
                If (Not String.IsNullOrEmpty(anyosSerie)) Then
                    ccMetadata.NumAnyosSerie = anyosSerie
                End If
                If (Not String.IsNullOrEmpty(Productos)) Then
                    ccMetadata.Producto = Productos
                End If
                If (Not String.IsNullOrEmpty(Proyectos)) Then
                    ccMetadata.IdProyecto = Proyectos
                End If
                If (Not String.IsNullOrEmpty(hfProyectos)) Then
                    ccMetadata.Proyecto = hfProyectos
                End If
                If (Not String.IsNullOrEmpty(EstadosProyecto)) Then
                    ccMetadata.EstadoProyecto = EstadosProyecto
                End If
                If (Not String.IsNullOrEmpty(Activos)) Then
                    ccMetadata.IdTipoActivo = Activos
                    ccMetadata.TipoActivo = hfActivo
                End If
                If (Not String.IsNullOrEmpty(Propiedad)) Then
                    ccMetadata.Propiedad = Propiedad
                End If
                If (Not String.IsNullOrEmpty(hfCodigoPresupuesto)) Then
                    ccMetadata.BudgetCode = hfCodigoPresupuesto
                    ccMetadata.DescBudgetCode = txtCodigoPresupuesto
                End If
                If (String.IsNullOrEmpty(hfActivo)) Then
                    ccMetadata.IdTipoActivo = Integer.MinValue
                End If

                '#Region "Valores decimales"

                '                If (Not String.IsNullOrEmpty(txtCantidadSolicitada)) Then
                '                    Dim cantidadSolicitadaDec As Decimal = Decimal.MinValue
                '                    Dim esNegativo As Boolean = False

                '                    esNegativo = txtCantidadSolicitada.StartsWith("-")
                '                    If (esNegativo) Then
                '                        txtCantidadSolicitada = txtCantidadSolicitada.Replace("-", String.Empty)
                '                    End If

                '                    Decimal.TryParse(txtCantidadSolicitada, NumberStyles.AllowDecimalPoint + NumberStyles.AllowThousands, CultureInfo.CreateSpecificCulture(Threading.Thread.CurrentThread.CurrentUICulture.Name), cantidadSolicitadaDec)

                '                    If (Not String.IsNullOrEmpty(txtCantidadSolicitada) AndAlso txtCantidadSolicitada <> "0" AndAlso txtCantidadSolicitada <> "0.00" AndAlso cantidadSolicitadaDec = Decimal.Zero) Then
                '                        MensajeAlerta(String.Format("{0}. {1}", Utils.Traducir("Cantidad solicitada"), Utils.Traducir("Formato incorrecto")))
                '                        Return Crear(Productos, EstadosProyecto, Proyectos)
                '                    End If

                '                    If (esNegativo) Then
                '                        cantidadSolicitadaDec = cantidadSolicitadaDec * -1
                '                    End If

                '                    ccMetadata.CantidadSolicitada = cantidadSolicitadaDec
                '                End If

                '#End Region
                Dim plantasBLL As New SabLib.BLL.PlantasComponent()
                Dim empresasProductivas As New List(Of ELL.BRAIN.CCProductionPlant)
                If (lbEmpresasProductivas IsNot Nothing AndAlso lbEmpresasProductivas.Count > 0) Then
                    For Each empresa In lbEmpresasProductivas
                        empresasProductivas.Add(New ELL.BRAIN.CCProductionPlant With {.IdPlantaSAB = empresa, .DescripcionPlanta = plantasBLL.GetPlanta(empresa).Nombre})
                    Next
                End If

                BLL.BRAIN.CCMetadataBLL.Guardar(ccMetadata, empresasProductivas)

                ' Si tenemos informado el campo hfIdValidacionLinea es que venimos desde financiero y tenemos que guardar tambien los valores anuales y cambiar el estado a opened
                If (hfIdValidacionLinea <> "0" AndAlso Not String.IsNullOrEmpty(hfIdStep)) Then
                    Dim validacionesAño As List(Of ELL.ValidacionAño) = BLL.ValidacionesAñoBLL.CargarListadoPorValidacionLinea(CInt(hfIdValidacionLinea))

                    ' Por cada año una entrada
                    Dim listaAños As List(Of Integer) = (From lstVal In validacionesAño
                                                         Group lstVal By lstVal.Año Into agrupacion = Group
                                                         Select Año).OrderBy(Function(f) f).ToList()

                    Dim paso As ELL.Step = BLL.StepsBLL.Obtener(CInt(hfIdStep))
                    Dim planta As ELL.Planta = BLL.PlantasBLL.Obtener(paso.IdPlanta)
                    Dim sumaGastos As Integer = Integer.MinValue

                    For Each año In listaAños
                        Dim costCarrierYear As New ELL.BRAIN.CCMetadataYear()
                        costCarrierYear.Empresa = Empresas
                        costCarrierYear.Planta = "000"
                        costCarrierYear.Anyo = año
                        costCarrierYear.CodigoPortador = hfCodigo
                        costCarrierYear.CodigoMoneda = planta.IdMoneda
                        costCarrierYear.Moneda = planta.Moneda

                        sumaGastos = validacionesAño.Where(Function(f) f.Año = año AndAlso f.IdColumna = ELL.Columna.Tipo.Year_expenses).Sum(Function(f) f.Valor)

                        If (paso.GastosAñoOrigenDatos = ELL.OrigenDatosStep.OrigenDatosStep.Planificacion) Then
                            ' Si el paso es de bonos es el sumatorio de gastos de ese año: working reports budget
                            costCarrierYear.PresupBonosPersona = sumaGastos
                        ElseIf (paso.IdCostGroupOT <> 17) Then
                            ' Si el cost group ot asociado al cost group y NO es TRAVEL COSTS sumatorio de gastos de ese año: invoices budget
                            ' Esta condición de la corrije Maite en un mail del 12/04/2019 a las 11:34 (RE: metadadtos)
                            costCarrierYear.PresupFacturas = sumaGastos
                        Else
                            ' Si no es de bonos ni de viajes sumatorio de gastos de ese año: travels budget
                            costCarrierYear.PresupViajes = sumaGastos
                        End If

                        ' Si no cargamos los valores no vamos a poder ver el PBC
                        paso.CargarValoresStep()

                        If (paso.PBC > 0) Then
                            ' Si NO es propiedad de BATZ sumatorio de ingresos de eses año: customer sales amount
                            costCarrierYear.ImporteVentaCliente = validacionesAño.Where(Function(f) f.Año = año AndAlso f.IdColumna = ELL.Columna.Tipo.Year_incomes).Sum(Function(f) f.Valor)
                        End If

                        Try
                            BLL.BRAIN.CCMetadataYearBLL.Guardar(costCarrierYear)
                        Catch ex As Exception
                            ' Eliminamos todos los datos anuales y los propios datos del cost carrier
                            BLL.BRAIN.CCMetadataYearBLL.Eliminar(ccMetadata.Empresa, ccMetadata.Planta, ccMetadata.CodigoPortador)
                            BLL.BRAIN.CCMetadataBLL.Eliminar(ccMetadata.Empresa, ccMetadata.Planta, ccMetadata.CodigoPortador)
                            Throw ex
                        End Try
                    Next

                    ' Marcamos el step como abierto y ponemos el codigo del portador al step
                    ' Además abrir el portador en el carrito
                    Try
                        BLL.ValidacionesLineaBLL.AñadirEstadoValidacion(CInt(hfIdValidacionLinea), Ticket.IdUser, String.Empty, ELL.Validacion.Estado.Opened, ELL.Validacion.Accion.Open, Integer.MinValue)
                        BLL.StepsBLL.CambiarCodigoPortador(CInt(hfIdStep), hfCodigo)

                        ' Los pasos de Zamudio no se abren en el carrito
                        If (Empresas <> ELL.Planta.PLANTA_XPERT_ZAMUDIO) Then
                            Try
                                Using servicio As New ServicioSoliciudesCompra.Service
                                    If (servicio.GuardarPortadorCoste(hfResponsable, hfCodigo, Empresas)) Then
                                        log.Info("Creado portador en carrito para empresa " & Empresas & ", portador " & hfCodigo & " y usuario SAB " & hfResponsable)
                                    Else
                                        log.Error("Error al crear portador en carrito para empresa " & Empresas & ", portador " & hfCodigo & " y usuario SAB " & hfResponsable)
                                    End If
                                End Using
                            Catch ex As Exception
                                log.Error("Error al crear portador en carrito para empresa " & Empresas & ", portador " & hfCodigo & " y usuario SAB " & hfResponsable, ex)
                            End Try
                        End If
                    Catch ex As Exception
                        ' Eliminamos todos los datos anuales y los propios datos del cost carrier
                        BLL.BRAIN.CCMetadataYearBLL.Eliminar(ccMetadata.Empresa, ccMetadata.Planta, ccMetadata.CodigoPortador)
                        BLL.BRAIN.CCMetadataBLL.Eliminar(ccMetadata.Empresa, ccMetadata.Planta, ccMetadata.CodigoPortador)
                        Throw ex
                    End Try

                    ' Mandamos el email al usuario y a el/los local project lead
                    Try
                        EnviarEmailPasoAbierto(paso, CInt(hfIdValidacionLinea))
                    Catch ex As Exception
                        log.Error("Error al enviar mail al usuario paso abierto")
                    End Try

                    MensajeInfo(Utils.Traducir("Datos guardados correctamente"))
                    Return RedirectToAction("DetallePasos", "Financiero", New With {.idCabecera = paso.IdCabecera})
                Else
                    MensajeInfo(Utils.Traducir("Datos guardados correctamente"))
                    Return RedirectToAction("Editar", New With {.empresa = ccMetadata.Empresa, .planta = ccMetadata.Planta, .codigo = ccMetadata.CodigoPortador})
                End If
            Catch ex As Exception
                MensajeError(Utils.Traducir("Se ha producido un error al guardar los datos"))
                If (hfIdValidacionLinea <> "0" AndAlso Not String.IsNullOrEmpty(hfIdStep)) Then
                    Return CrearDesdeFinanciero(hfCodigo, CInt(hfIdStep), CInt(hfIdValidacionLinea))
                Else
                    Return Crear(Productos, EstadosProyecto, Proyectos)
                End If
            End Try
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        <HttpPost>
        Function Editar(ByVal Empresas As String, ByVal hfCodigo As String, ByVal txtCodigo As String, ByVal Lantegi As String, ByVal TiposPlanta As String, ByVal Denominacion As String, ByVal txtResponsable As String,
                       ByVal hfResponsable As Integer, ByVal fechaApertura As String, ByVal fechaCierre As DateTime?, ByVal fechaInicio As String, ByVal anyosSerie As String, ByVal Productos As String,
                       ByVal hfProyectos As String, ByVal Proyectos As String, ByVal EstadosProyecto As String, ByVal Activos As String, ByVal hfActivo As String, ByVal Propiedad As String, ByVal hfCodigoPresupuesto As String, ByVal txtCodigoPresupuesto As String,
                       ByVal lbEmpresasProductivas As IEnumerable(Of String)) As ActionResult
            Dim ccMetadata As New ELL.BRAIN.CCMetadata()

            Try
                ccMetadata.Empresa = Empresas
                ccMetadata.CodigoPortador = hfCodigo
                ccMetadata.Lantegi = Lantegi
                ccMetadata.TipoPlanta = TiposPlanta
                ccMetadata.DenomAmpliada = Denominacion
                ccMetadata.Responsable = txtResponsable
                ccMetadata.IdResponsableSAB = hfResponsable
                ccMetadata.FechaIni = fechaApertura

                If (fechaCierre IsNot Nothing) Then
                    ccMetadata.FechaFin = fechaCierre
                End If
                If (Not String.IsNullOrEmpty(fechaInicio)) Then
                    ccMetadata.FechaEstimIni = fechaInicio
                End If
                If (Not String.IsNullOrEmpty(anyosSerie)) Then
                    ccMetadata.NumAnyosSerie = anyosSerie
                End If
                If (Not String.IsNullOrEmpty(Productos)) Then
                    ccMetadata.Producto = Productos
                End If
                If (Not String.IsNullOrEmpty(Proyectos)) Then
                    ccMetadata.IdProyecto = Proyectos
                End If
                If (Not String.IsNullOrEmpty(hfProyectos)) Then
                    ccMetadata.Proyecto = hfProyectos
                End If
                If (Not String.IsNullOrEmpty(EstadosProyecto)) Then
                    ccMetadata.EstadoProyecto = EstadosProyecto
                End If
                If (Not String.IsNullOrEmpty(Activos)) Then
                    ccMetadata.IdTipoActivo = Activos
                    ccMetadata.TipoActivo = hfActivo
                End If
                If (Not String.IsNullOrEmpty(Propiedad)) Then
                    ccMetadata.Propiedad = Propiedad
                End If
                If (Not String.IsNullOrEmpty(hfCodigoPresupuesto)) Then
                    ccMetadata.BudgetCode = hfCodigoPresupuesto
                    ccMetadata.DescBudgetCode = txtCodigoPresupuesto
                End If
                If (String.IsNullOrEmpty(hfActivo)) Then
                    ccMetadata.IdTipoActivo = Integer.MinValue
                End If

                '#Region "Valores decimales"

                '                If (Not String.IsNullOrEmpty(txtCantidadSolicitada)) Then
                '                    Dim cantidadSolicitadaDec As Decimal = Decimal.MinValue
                '                    Dim esNegativo As Boolean = False

                '                    esNegativo = txtCantidadSolicitada.StartsWith("-")
                '                    If (esNegativo) Then
                '                        txtCantidadSolicitada = txtCantidadSolicitada.Replace("-", String.Empty)
                '                    End If

                '                    Decimal.TryParse(txtCantidadSolicitada, NumberStyles.AllowDecimalPoint + NumberStyles.AllowThousands, CultureInfo.CreateSpecificCulture(Threading.Thread.CurrentThread.CurrentUICulture.Name), cantidadSolicitadaDec)

                '                    If (Not String.IsNullOrEmpty(txtCantidadSolicitada) AndAlso txtCantidadSolicitada <> "0" AndAlso txtCantidadSolicitada <> "0.00" AndAlso cantidadSolicitadaDec = Decimal.Zero) Then
                '                        MensajeAlerta(String.Format("{0}. {1}", Utils.Traducir("Cantidad solicitada"), Utils.Traducir("Formato incorrecto")))
                '                        Return Editar(Empresas, "000", hfCodigo)
                '                    End If

                '                    If (esNegativo) Then
                '                        cantidadSolicitadaDec = cantidadSolicitadaDec * -1
                '                    End If

                '                    ccMetadata.CantidadSolicitada = cantidadSolicitadaDec
                '                End If

                '#End Region
                Dim plantasBLL As New SabLib.BLL.PlantasComponent()
                Dim empresasProductivas As New List(Of ELL.BRAIN.CCProductionPlant)
                For Each empresa In lbEmpresasProductivas
                    empresasProductivas.Add(New ELL.BRAIN.CCProductionPlant With {.IdPlantaSAB = empresa, .DescripcionPlanta = plantasBLL.GetPlanta(empresa).Nombre})
                Next

                BLL.BRAIN.CCMetadataBLL.Guardar(ccMetadata, empresasProductivas)

                Try
                    Using servicio As New ServicioSoliciudesCompra.Service
                        If (servicio.GuardarPortadorCoste(hfResponsable, hfCodigo, Empresas)) Then
                            log.Info("Guardado portador en carrito para empresa " & Empresas & ", portador " & hfCodigo & " y usuario SAB " & hfResponsable)
                        Else
                            log.Error("Error al guardar portador en carrito para empresa " & Empresas & ", portador " & hfCodigo & " y usuario SAB " & hfResponsable)
                        End If
                    End Using
                Catch
                    log.Error("Error al guardar portador en carrito desde Editar Metadata para empresa " & Empresas & ", portador " & hfCodigo & " y usuario SAB " & hfResponsable)
                End Try

                MensajeInfo(Utils.Traducir("Datos guardados correctamente"))
                Return RedirectToAction("Editar", New With {.empresa = ccMetadata.Empresa, .planta = ccMetadata.Planta, .codigo = ccMetadata.CodigoPortador})
            Catch ex As Exception
                MensajeError(Utils.Traducir("Se ha producido un error al guardar los datos"))
                Return Editar(Empresas, "000", hfCodigo)
            End Try
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="empresa"></param>
        ''' <param name="planta"></param>
        ''' <param name="codigo"></param>
        ''' <returns></returns>
        Function Eliminar(ByVal empresa As String, ByVal planta As String, ByVal codigo As String) As ActionResult
            Try
                BLL.BRAIN.CCMetadataBLL.Eliminar(empresa, planta, codigo)
                MensajeInfo(Utils.Traducir("Metadatos eliminada correctamente"))
            Catch ex As Exception
                MensajeError(Utils.Traducir("Error al eliminar metadatos"))
                log.Error("Error al eliminar metadatos", ex)
            End Try

            Return RedirectToAction("Index")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="paso"></param>
        ''' <param name="idValidacionLinea"></param>
        Private Sub EnviarEmailPasoAbierto(ByVal paso As ELL.Step, ByVal idValidacionLinea As Integer)
            Try
                ' Para obtener el mail de usuario que realizó el envío tenemos que navegador por el histórico de estados y buscar el primer estado waiting for aproval
                Dim historicoEstado As ELL.HistoricoEstadoLinea = BLL.HistoricosEstadoLineaBLL.CargarListadoPorValidacionLinea(idValidacionLinea).OrderBy(Function(f) f.Fecha).FirstOrDefault()
                Dim uri = Url.Action("Index", "CostCarriers", Nothing, Request.Url.Scheme)
                Dim subject As String = Utils.Traducir("Paso abierto")
                Dim body As String = Utils.LeerFicheroTexto(Server.MapPath("~\Content\PlantillasMail\StepAbierto.html"))
                Dim mailFrom As String = ConfigurationManager.AppSettings("MailFrom")
                Dim mailto As String = historicoEstado.EmailUsuario
                Dim paramBLL As New SabLib.BLL.ParametrosBLL
                Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor
                Dim usuariosBLL As New SabLib.BLL.UsuariosComponent()
                Dim nombreUsuarioAbre As String = usuariosBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = Ticket.IdUser}).NombreCompleto
                Dim nombreProyecto As String = BLL.CabecerasCostCarrierBLL.Obtener(paso.IdCabecera, False).NombreProyecto
                Dim planta As String = BLL.PlantasBLL.Obtener(paso.IdPlanta).Planta
                Dim estado As String = BLL.EstadosBLL.Obtener(paso.IdEstado).Estado
                Dim costGroup As String = BLL.CostsGroupBLL.Obtener(paso.IdCostGroup).Descripcion

                ' Tenemos que avisar también a los local project lead
                Dim listaProjectLeads As List(Of ELL.Miembro) = BLL.MiembrosBLL.CargarLocalProjectLeads(paso.Proyecto)
                For Each miembro In listaProjectLeads
                    mailto += ";" & miembro.Email
                Next

                '***************************************            
                body = String.Format(body, nombreProyecto, nombreUsuarioAbre, planta, estado, costGroup, paso.Descripcion, uri, paso.Id)
                '***************************************

#If Not DEBUG Then
                Dim notificacionesActivas As Boolean = CBool(ConfigurationManager.AppSettings("NotificacionesActivas"))

                If (notificacionesActivas) Then
                    SabLib.BLL.Utils.EnviarEmail(mailFrom, mailto, subject, body, serverEmail)
                End If
#End If
            Catch ex As Exception
                log.Error("Se ha producido un error al enviar el mail de paso abierto", ex)
                Throw ex
            End Try
        End Sub

#End Region

    End Class
End Namespace