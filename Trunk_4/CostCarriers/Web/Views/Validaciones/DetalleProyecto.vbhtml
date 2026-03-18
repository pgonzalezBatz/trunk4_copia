@Imports CostCarriersLib

@Code
    ' Cogemos de las validaciones la última
    Dim validacion As ELL.Validacion = CType(ViewData("Validaciones"), List(Of ELL.Validacion)).OrderByDescending(Function(f) f.Fecha).FirstOrDefault()
    Dim cabeceraProyecto As ELL.CabeceraCostCarrier = CType(ViewData("CabeceraProyecto"), ELL.CabeceraCostCarrier)
    Dim steps As List(Of ELL.Step) = CType(ViewData("Steps"), List(Of ELL.Step))
    Dim culturaEsES As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture("es-ES")
    Dim listaIdsPlantasUnicos As IEnumerable(Of Object) = (From lstSteps In steps
                                                           Group lstSteps By lstSteps.IdPlantaSAB, lstSteps.IdPlanta, lstSteps.Moneda Into agrupacion = Group
                                                           Select New With {.IdPlantaSAB = IdPlantaSAB, .IdPlanta = IdPlanta, .Moneda = Moneda}).ToList()
    Dim ticket As SabLib.ELL.Ticket = CType(Session("Ticket"), SabLib.ELL.Ticket)
End Code

<script type="text/javascript">

    $(function () {
        //Con esto ocultamos lo que no queremos  solo info general
        if ("@steps.Exists(Function(f) f.EsInfoGeneral)" == "True") {
            $(".ocultar").hide();
        }

        $(".aprobar").click(function (e) {
            $("#hfIdCostGroup-" + $(this).data("idplanta")).val($(this).data("idcostgroup"));
            $("#hfIdPlanta-" + $(this).data("idplanta")).val('');

            return confirm('@Html.Raw(Utils.Traducir("¿Realmente desea aprobar los pasos sin validar del grupo de coste seleccionado?"))');
        });

        $(".rechazar").click(function (e) {
            $("#hIdCostGroupRechazar").val($(this).data("idcostgroup"));
            $("#hIdPlantaRechazar").val('');
            $('#modalWindowRechazar').modal('show');
        });

        $(".aprobar-todo").click(function (e) {
            $("#hfIdCostGroup-" + $(this).data("idplanta")).val('');
            $("#hfIdPlanta-" + $(this).data("idplanta")).val($(this).data("idplanta"));

            return confirm('@Html.Raw(Utils.Traducir("¿Realmente desea aprobar los pasos sin validar de la planta seleccionada?"))');
        });

        $(".rechazar-todo").click(function (e) {
            $("#hIdCostGroupRechazar").val('');
            $("#hIdPlantaRechazar").val($(this).data("idplanta"));
            $('#modalWindowRechazar').modal('show');
        });

        @*$(".btn-infoadicional").click(function () {
            var idPlanta = $(this).data("idplanta");

            $.ajax({
                url: '@Url.Action("CargarDatosValidacionInfoAdicional", "CostCarriers")',
                data: { idCabecera: @cabeceraProyecto.Id, idPlanta: idPlanta, idTipo: @CInt(ELL.ValidacionInfoAdicional.TipoDato.Awarding_offer)},
                type: 'GET',
                dataType: 'json',
                success: function (d) {
                    $("#txtMargenNetoAO").val('');
                    $("#txtVentasEfectivasAO").val('');
                    $("#txtBeneficioHerramentalAO").val('');
                    $("#txtPlantasClienteAO").val('');
                    $("#txtSOPAO").val('');
                    $("#txtVolumenMedioAO").val('');
                    $("#txtAnyosSerieAO").val('');
                    if (d) {
                        if (d.NetMargin != @Integer.MinValue) {
                            $("#txtMargenNetoAO").val(String(d.NetMargin).replace(".", ","));
                        }
                        if (d.EffectiveSales != @Integer.MinValue) {
                            $("#txtVentasEfectivasAO").val(d.EffectiveSales);
                        }
                        if (d.CustomerProperty != @Integer.MinValue) {
                            $("#txtBeneficioHerramentalAO").val(d.CustomerProperty);
                        }
                        if (d.CustomerPlants != '') {
                            $("#txtPlantasClienteAO").val(d.CustomerPlants);
                        }
                        if (d.SOP != '/Date(-62135596800000)/') {
                            $("#txtSOPAO").val(new Date(parseInt(d.SOP.substr(6))).toLocaleDateString("@Threading.Thread.CurrentThread.CurrentCulture.Name"));
                        }
                        if (d.AverageVolumen != @Integer.MinValue) {
                            $("#txtVolumenMedioAO").val(d.AverageVolumen);
                        }
                        if (d.SeriesYears != @Integer.MinValue) {
                            $("#txtAnyosSerieAO").val(d.AverageVolumen);
                        }
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                }
            });

            $.ajax({
                url: '@Url.Action("CargarDatosValidacionInfoAdicionalActual", "CostCarriers")',
                data: { idCabecera: @cabeceraProyecto.Id, idPlanta: idPlanta, idTipo: @CInt(ELL.ValidacionInfoAdicional.TipoDato.Current_values)},
                type: 'GET',
                dataType: 'json',
                success: function (d) {
                    $("#txtMargenNeto").val('');
                    $("#txtVentasEfectivas").val('');
                    $("#txtBeneficioHerramental").val('');
                    $("#txtPlantasCliente").val('');
                    $("#txtSOP").val('');
                    $("#txtVolumenMedio").val('');
                    $("#txtAnyosSerie").val('');
                    if (d) {
                        if (d.NetMargin != @Integer.MinValue) {
                            $("#txtMargenNeto").val(String(d.NetMargin).replace(".", ","));
                        }
                        if (d.EffectiveSales != @Integer.MinValue) {
                            $("#txtVentasEfectivas").val(d.EffectiveSales);
                        }
                        if (d.CustomerProperty != @Integer.MinValue) {
                            $("#txtBeneficioHerramental").val(d.CustomerProperty);
                        }
                        if (d.CustomerPlants != '') {
                            $("#txtPlantasCliente").val(d.CustomerPlants);
                        }
                        if (d.SOP != '/Date(-62135596800000)/') {
                            $("#txtSOP").val(new Date(parseInt(d.SOP.substr(6))).toLocaleDateString("@Threading.Thread.CurrentThread.CurrentCulture.Name"));
                        }
                        if (d.AverageVolumen != @Integer.MinValue) {
                            $("#txtVolumenMedio").val(d.AverageVolumen);
                        }
                        if (d.SeriesYears != @Integer.MinValue) {
                            $("#txtAnyosSerie").val(d.AverageVolumen);
                        }
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                }
            });

            $('#modalWindowInfoAdicional').modal('show');
        })*@
    });

</script>

<h3><label>@Utils.Traducir("Validaciones pendientes") - @cabeceraProyecto.NombreProyecto (@cabeceraProyecto.TipoProyPtksis)</label></h3>
<hr />

@code
    Dim HorasSuma As Integer = 0
    Dim HorasSumaPlanta As Integer = 0
    Dim BACGastosSuma As Integer = 0
    Dim BACGastosSumaPlanta As Integer = 0
    Dim BACGastosSumaUltimoAprobado As Integer = 0
    Dim PBCSuma As Integer = 0
    Dim PBCSumaUltimoAprobado As Integer = 0
    Dim PBCSumaPlanta As Integer = 0

    Dim HorasSumaV As Integer = 0
    Dim HorasSumaPlantaV As Integer = 0
    Dim BACGastosSumaV As Integer = 0
    Dim BACGastosSumaPlantaV As Integer = 0
    Dim PBCSumaV As Integer = 0
    Dim PBCSumaPlantaV As Integer = 0

    Dim marginSumaPlanta As String = String.Empty
    Dim marginSumaPlantaV As String = String.Empty
    Dim marginSuma As String = String.Empty
    Dim marginSumaV As String = String.Empty
    Dim marginSumaUltimoAprobado As String = String.Empty
    Dim BACGastosSumaPlantaVentas As Integer = 0
    Dim PBCSumaPlantaVentas As Integer = 0
    Dim marginSumaPlantaVentas As String = String.Empty

    Dim BACGastosSumaPlantaVentasV As Integer = 0
    Dim PBCSumaPlantaVentasV As Integer = 0
    Dim marginSumaPlantaVentasV As String = String.Empty

    Dim delta As String = String.Empty

    Dim listaCostGroups As List(Of ELL.CostGroup) = Nothing
    Dim validacionLineaValidada As ELL.ValidacionLinea = Nothing

    ' Con esto obtenemos el nombre del solicitante. Es raro pero podría darse el caso de que hubiera varios solicitantes ya que en esta pantalla
    ' se agrupan validaciones
    Dim historicoEstado As ELL.ValidacionLinea = BLL.ValidacionesLineaBLL.CargarListadoPorValidacion(validacion.Id).OrderBy(Function(f) f.Id).FirstOrDefault
    Dim nombreSolicitante As String = BLL.HistoricosEstadoLineaBLL.CargarListadoPorValidacionLinea(historicoEstado.Id).FirstOrDefault(Function(f) f.IdAccionValidacion = ELL.Validacion.Accion.Send_to_validate).Usuario
    Dim usuariosRol As List(Of ELL.UsuarioRol) = CType(Session("RolesUsuario"), List(Of ELL.UsuarioRol))

    @<span style="padding-right:15px;">@Html.ActionLink(Utils.Traducir("Volver"), "Index")</span>
    @<span>@Html.ActionLink(Utils.Traducir("Ya validados"), "DetalleProyecto", "Totales", New With {.idCabecera = cabeceraProyecto.Id}, Nothing)</span>
    @<br />
    @<br />

    If (listaIdsPlantasUnicos.Count > 0) Then
        @<form class="form-horizontal">
            <div Class="form-group">
                <div class="col-sm-2">
                    <label class="col-form-label">@Utils.Traducir("Solicitante")</label>
                </div>
                <div class="col-sm-10">
                    <label>@nombreSolicitante</label>
                </div>
            </div>
            <div Class="form-group">
                <div class="col-sm-2">
                    <label class="col-form-label">@Utils.Traducir("Denominación del presupuesto")</label>
                </div>
                <div class="col-sm-10">
                    <label>@validacion.Denominacion</label>
                </div>
            </div>
            <div Class="form-group">
                <div class="col-sm-2">
                    <label class="col-form-label">@Utils.Traducir("Descripción del presupuesto")</label>
                </div>
                <div class="col-sm-10">
                    @Html.TextArea("descripcion", validacion.Descripcion, New With {.maxlength = "1000", .rows = "5", .class = "form-control", .readonly = "readonly"})
                </div>
            </div>
            <div Class="form-group">
                <div class="col-sm-2">
                    <label class="col-form-label">@Utils.Traducir("Prevista en PG")</label>
                </div>
                <div class="col-sm-10">

                    @Html.RadioButton("previstoPg", 1, validacion.PrevistoPG, New With {.disabled = True})&nbsp;<label>@Utils.Traducir("Si")</label>
                    @Html.RadioButton("previstoPg", 0, Not validacion.PrevistoPG, New With {.disabled = True})&nbsp;<label>@Utils.Traducir("No")</label>
                </div>
            </div>
            @*@code
                ' Cogemos de los flujos de aprobación del usuario, los de el proyecto en concreto y aquellos que tengan un porcentaje informado
                Dim flujosAprobacion As List(Of ELL.FlujoAprobacion) = BLL.FlujosAprobacionBLL.CargarListadoPorUsuario(ticket.IdUser).Where(Function(f) f.IdCabecera = cabeceraProyecto.Id AndAlso f.Porcentaje <> Integer.MinValue).ToList()

                If (flujosAprobacion IsNot Nothing AndAlso flujosAprobacion.Count > 0) Then
                    @<div Class="form-group">
                         <div Class="col-sm-12">
                             <span class="label label-warning" style="font-size:x-large;">@String.Format("{0} ({1}%)", Utils.Traducir("Porcentaje aplicable al coste de los pasos del Corporativo"), flujosAprobacion.First.Porcentaje)</span>
                         </div>
                    </div>
                End If
                                end code*@


        </form>

        ' Al ordenar descending por IdPlantaSAB dejamos corporativo al final que es lo que quieren
        @For Each planta In listaIdsPlantasUnicos.OrderByDescending(Function(f) f.IdPlantaSAB).ToList()
            If (usuariosRol.Select(Function(f) f.IdPlanta).Contains(planta.IdPlantaSAB) OrElse planta.IdPlantaSAB = 0) Then
                HorasSumaPlanta = 0
                BACGastosSumaPlanta = 0
                PBCSumaPlanta = 0

                HorasSumaPlantaV = 0
                BACGastosSumaPlantaV = 0
                PBCSumaPlantaV = 0

                BACGastosSumaPlantaVentas = 0
                PBCSumaPlantaVentas = 0
                BACGastosSumaPlantaVentasV = 0
                PBCSumaPlantaVentasV = 0
                marginSumaPlanta = String.Empty
                marginSumaPlantaV = String.Empty
                marginSumaPlantaVentas = String.Empty
                marginSumaPlantaVentasV = String.Empty
                Dim nombrePlanta As String = BLL.PlantasBLL.CargarListadoPorIdPlantaSAB(planta.IdPlantaSAB).FirstOrDefault().Planta
                @Using Html.BeginForm("AprobarCostGroup", "Validaciones", FormMethod.Post, New With {.class = "form-horizontal"})
                    @Html.Hidden("hfIdCostGroup-" & planta.IdPlanta)
                    @Html.Hidden("hfIdPlanta-" & planta.IdPlanta)
                    @<div Class="form-group">
                        <label class="col-sm-2 label label-success text-uppercase">@nombrePlanta</label>
                        <div Class="col-sm-10">
                            <input type="submit" id="aprobarTodo" value="@Utils.Traducir("Aprobar todo")" Class="btn btn-success aprobar-todo" data-idplanta="@planta.IdPlanta" />
                            <input type="button" id="rechazarTodo" value="@Utils.Traducir("Rechazar todo")" Class="btn btn-danger rechazar-todo" data-idplanta="@planta.IdPlanta" />
                        </div>
                    </div>
                    @<div Class="form-group ocultar">
                        <div Class="col-sm-2">
                            <div><label class="col-form-label">@String.Format("{0} ({1})", Utils.Traducir("Resumen del presupuesto"), planta.Moneda)</label></div>
                            <div><span>@Html.ActionLink(String.Format("{0} ({1})", Utils.Traducir("Ya validados"), nombrePlanta), "DetalleProyecto", "Totales", New With {.idCabecera = cabeceraProyecto.Id, .idPlantaSAB = planta.IdPlantaSAB}, Nothing)</span></div>
                        </div>
                        <div Class="col-sm-10">
                            <Table id="tabla" Class="table table-bordered table-condensed">
                                @code
                                    ' Si la planta es CORPORATIVO no se muestran el botón. Me lo comenta así Maite porque son datos calculados
                                    ' Sólo tiene sentido para los proyectos de Industrialización
                                    If (planta.IdPlantaSAB <> 0 AndAlso (cabeceraProyecto.TipoProyPtksis = ELL.CabeceraCostCarrier.TIPO_PROY_INDUSTRIALIZATION OrElse cabeceraProyecto.TipoProyPtksis = ELL.CabeceraCostCarrier.TIPO_PROY_ECO_BATZ)) Then
                                        @<tr>
                                            <td colspan="11">
                                                @code
                                                    ' Estos datos son para el vista parcial _ValidacionAdicionalPlanta.vbhtml
                                                    Dim viewDataDictonaryInfo As New ViewDataDictionary()
                                                    viewDataDictonaryInfo = New ViewDataDictionary()
                                                    viewDataDictonaryInfo.Add("IdCabecera", cabeceraProyecto.Id)
                                                    viewDataDictonaryInfo.Add("IdPlanta", planta.IdPlantaSAB)
                                                    viewDataDictonaryInfo.Add("SoloLectura", True)
                                                End Code
                                                @Html.Partial("~/Views/Shared/_ValidacionAdicionalPlanta.vbhtml", viewDataDictonaryInfo)
                                            </td>
                                        </tr>
                                                    End If
                                End code

                                <tr>
                                <td></td>
                                    <td colspan = "3" style="text-align:center;font-weight:bold">@Utils.Traducir("Nueva petición")</td>
                                    <td colspan="4" style="text-align:center;font-weight:bold;text-transform:uppercase; color:#ffffff;background-color:#000000;">@Utils.Traducir("Nuevo estado total")</td>
                                    <td></td>
                                    <td></td>
                                </tr>

                                <tr>
                                <td></td>
                                    <td Class="danger" style="text-align:center;font-weight:bold">@Utils.Traducir("Horas")</td>
                                    <td Class="danger" style="text-align:center;font-weight:bold">@Utils.Traducir("Coste")</td>
                                    <td Class="info" style="text-align:center;font-weight:bold">@Utils.Traducir("Dinero pagado por cliente")</td>
                                    <td Class="danger" style="text-align:center;font-weight:bold">@Utils.Traducir("Horas")</td>
                                    <td Class="danger" style="text-align:center;font-weight:bold">@Utils.Traducir("Coste")</td>
                                    <td Class="info" style="text-align:center;font-weight:bold">@Utils.Traducir("Dinero pagado por cliente")</td>
                                    <td Class="info" style="text-align:center;font-weight:bold">@Utils.Traducir("Margen") %</td>
                                    <td Class="info" style="text-align:center;font-weight:bold">@Utils.Traducir("Delta")</td>
                                    <td></td>
                                </tr>

                                <tbody>
                                    @code
                                        ' Vamos a obtener los distintos cost group de los steps
                                        listaCostGroups = BLL.CostsGroupBLL.CargarListadoPorCabeceraPlanta(cabeceraProyecto.Id, planta.IdPlantaSAB)
                                        Dim nombreCostGroup As String = String.Empty
                                                                Dim tipoCostGroup As String = String.Empty
                                                                Dim listaCostGroupsParaValidacion As New List(Of ELL.CostGroupParaValidacion)
                                                                Dim costGroupsParaValidacion As ELL.CostGroupParaValidacion
                                                                Dim stepsCostGroup As List(Of ELL.Step)
                                                                Dim aprobar As Boolean = False

                                                                ' Como quieren agrupados los cost group de SERIAL TOOLING los vamos a tratar aparte. Primero creamos los grupos ficticios
                                                                For Each costGroup In listaCostGroups.Where(Function(f) f.IdAgrupacion = ELL.CostGroupOT.Agrupacion.Serial_tooling)
                                            stepsCostGroup = steps.Where(Function(f) f.IdCostGroup = costGroup.Id).ToList
                                            'stepsCostGroup.ForEach(Sub(s) s.CargarValoresStepValidacion())
                                            stepsCostGroup.ForEach(Sub(s) s.CargarValoresStep())

                                            For Each paso In stepsCostGroup
                                                nombreCostGroup = CType(ELL.CostGroupOT.Agrupacion.Serial_tooling, ELL.CostGroupOT.Agrupacion).ToString().ToUpper().Replace("_", " ") & " " & If(paso.PBC = 0, "BATZ", "CUSTOMER") & " (" & costGroup.Estado & ")"
                                                If (Not listaCostGroupsParaValidacion.Exists(Function(f) f.Nombre = nombreCostGroup)) Then
                                                    listaCostGroupsParaValidacion.Add(New ELL.CostGroupParaValidacion With {.Nombre = nombreCostGroup})
                                                End If
                                                                Next
                                                                Next

                                                                For Each costGroup In listaCostGroups.OrderBy(Function(f) f.Descripcion)
                                            HorasSuma = 0
                                            BACGastosSuma = 0
                                            PBCSuma = 0

                                            HorasSumaV = 0
                                            BACGastosSumaV = 0
                                            PBCSumaV = 0

                                            BACGastosSumaUltimoAprobado = 0
                                            PBCSumaUltimoAprobado = 0

                                            stepsCostGroup = steps.Where(Function(f) f.IdCostGroup = costGroup.Id).ToList()

                                            If (stepsCostGroup IsNot Nothing AndAlso stepsCostGroup.Count > 0) Then
                                                ' Cargamos los valores de validacion de los steps de cada costgroup
                                                stepsCostGroup.ForEach(Sub(s) s.CargarValoresStepValidacion())
                                                stepsCostGroup.ForEach(Sub(s) s.CargarValoresStep())

                                                For Each stepCG In stepsCostGroup
                                                    validacionLineaValidada = BLL.ValidacionesLineaBLL.Obtener(stepCG.IdValidacionLinea)

                                                    HorasSuma += validacionLineaValidada.Hours
                                                    HorasSumaPlanta += validacionLineaValidada.Hours
                                                    BACGastosSuma += validacionLineaValidada.BudgetApproved
                                                    BACGastosSumaPlanta += validacionLineaValidada.BudgetApproved
                                                    PBCSuma += validacionLineaValidada.PaidByCustomer
                                                    PBCSumaPlanta += validacionLineaValidada.PaidByCustomer

                                                    If (validacionLineaValidada.PaidByCustomer > 0) Then
                                                        BACGastosSumaPlantaVentas += validacionLineaValidada.BudgetApproved
                                                    End If

                                                                ' Cogemos los ultimos valores validados del step. Puede ser que no tenga
                                                                Dim ultimoAprobado As ELL.ValidacionLinea = BLL.ValidacionesLineaBLL.ObtenerUltimoAprobado(stepCG.Id)
                                                                If (ultimoAprobado Is Nothing) Then
                                                        ultimoAprobado = New ELL.ValidacionLinea()
                                                    End If

                                                    HorasSumaV += validacionLineaValidada.Hours - ultimoAprobado.Hours
                                                    HorasSumaPlantaV += validacionLineaValidada.Hours - ultimoAprobado.Hours
                                                    BACGastosSumaV += validacionLineaValidada.BudgetApproved - ultimoAprobado.BudgetApproved
                                                    BACGastosSumaPlantaV += validacionLineaValidada.BudgetApproved - ultimoAprobado.BudgetApproved
                                                    PBCSumaV += validacionLineaValidada.PaidByCustomer - ultimoAprobado.PaidByCustomer
                                                    PBCSumaPlantaV += validacionLineaValidada.PaidByCustomer - ultimoAprobado.PaidByCustomer

                                                    BACGastosSumaUltimoAprobado += ultimoAprobado.BudgetApproved
                                                    PBCSumaUltimoAprobado += ultimoAprobado.PaidByCustomer

                                                    If (validacionLineaValidada.PaidByCustomer > 0) Then
                                                        BACGastosSumaPlantaVentasV += validacionLineaValidada.BudgetApproved - ultimoAprobado.BudgetApproved
                                                    End If
                                                                Next

                                                                'Si usabamos estos valores estabamos mostrando lo ya validado en vez lo que queda por validar
                                                                'BACGastosSuma = stepsCostGroup.Sum(Function(f) f.BACGastosValidacion)
                                                                'PBCSuma = stepsCostGroup.Sum(Function(f) f.PBCValidacion)
                                                                'BACGastosSumaPlanta += stepsCostGroup.Sum(Function(f) f.BACGastosValidacion)
                                                                'PBCSumaPlanta += stepsCostGroup.Sum(Function(f) f.PBCValidacion)

                                                                If (PBCSuma = 0) Then
                                                    marginSuma = "NA"
                                                Else
                                                    marginSuma = (((PBCSuma - BACGastosSuma) / PBCSuma) * 100).ToString("N2", culturaEsES)
                                                    PBCSumaPlantaVentas += PBCSuma
                                                End If

                                                                'BACGastosSumaPlantaVentasV = BACGastosSumaPlantaVentas - BACGastosSumaPlantaVentasV

                                                                If (PBCSumaV = 0) Then
                                                    marginSumaV = "NA"
                                                Else
                                                    marginSumaV = (((PBCSumaV - BACGastosSumaV) / PBCSumaV) * 100).ToString("N2", culturaEsES)
                                                    PBCSumaPlantaVentasV += PBCSumaV
                                                End If

                                                                If (PBCSumaUltimoAprobado = 0) Then
                                                    marginSumaUltimoAprobado = "NA"
                                                Else
                                                    marginSumaUltimoAprobado = String.Empty
                                                End If

                                                                If (marginSuma <> "NA" AndAlso marginSumaUltimoAprobado <> "NA") Then
                                                    delta = ((((PBCSuma - BACGastosSuma) / PBCSuma) * 100) - (((PBCSumaUltimoAprobado - BACGastosSumaUltimoAprobado) / PBCSumaUltimoAprobado) * 100)).ToString("N2", culturaEsES)
                                                Else
                                                    delta = "NA"
                                                End If

                                                                If (costGroup.IdAgrupacion = Integer.MinValue OrElse costGroup.IdAgrupacion <> ELL.CostGroupOT.Agrupacion.Serial_tooling) Then
                                                    @<tr>
                                                        <td class="success">
                                                            @String.Format("{0} ({1})", costGroup.Descripcion, costGroup.Estado)
                                                        </td>
                                                        <td align="right">@HorasSumaV.ToString("N0", culturaEsES)</td>
                                                        <td align="right">@BACGastosSumaV.ToString("N0", culturaEsES)</td>
                                                        <td align="right">@PBCSumaV.ToString("N0", culturaEsES)</td>
                                                        @*<td align="right">@marginSumaV</td>*@

                                                        <td align="right">@HorasSuma.ToString("N0", culturaEsES)</td>
                                                        <td align="right">@BACGastosSuma.ToString("N0", culturaEsES)</td>
                                                        <td align="right">@PBCSuma.ToString("N0", culturaEsES)</td>
                                                        <td align="right">@marginSuma</td>
                                                        <td align="right">@delta</td>
                                                        <td style="text-align:center;">
                                                            <input type="submit" id="aprobar" value="@Utils.Traducir("Aprobar")" Class="btn btn-success aprobar" data-idcostgroup="@costGroup.Id" data-idplanta="@planta.IdPlanta" />
                                                            <input type="button" id="rechazar" value="@Utils.Traducir("Rechazar")" Class="btn btn-danger rechazar" data-idcostgroup="@costGroup.Id" data-idplanta="@planta.IdPlanta" />
                                                            @Html.Raw("&nbsp")
                                                            @*@Html.ActionLink(Utils.Traducir("Ver detalle pasos"), "DetalleCostGroup", "Validaciones", New With {.idCostGroup = costGroup.Id}, New With {.class = "glyphicon glyphicon-eye-open"})*@

                                                            <a href="@Url.Action("DetalleCostGroup", "Validaciones", New With {.idCostGroup = costGroup.Id})" class="btn btn-primary" title="@Utils.Traducir("Ver detalle pasos")">
                                                                <span class="glyphicon glyphicon-eye-open" aria-hidden="true" ></span>
                                                            </a>
                                                        </td>
                                                    </tr>
                                                ElseIf (costGroup.IdAgrupacion = ELL.CostGroupOT.Agrupacion.Serial_tooling) Then
                                                    For Each stepCG In stepsCostGroup
                                                        'PBCSuma = stepCG.PBC
                                                        'BACGastosSuma = stepCG.BACGastos
                                                        validacionLineaValidada = BLL.ValidacionesLineaBLL.Obtener(stepCG.IdValidacionLinea)

                                                        PBCSuma = validacionLineaValidada.PaidByCustomer
                                                        BACGastosSuma = validacionLineaValidada.BudgetApproved

                                                        If (PBCSuma = 0) Then
                                                            nombreCostGroup = CType(ELL.CostGroupOT.Agrupacion.Serial_tooling, ELL.CostGroupOT.Agrupacion).ToString().ToUpper().Replace("_", " ") & " BATZ" & " (" & costGroup.Estado & ")"
                                                            tipoCostGroup = "batz"
                                                        ElseIf (PBCSuma > 0) Then
                                                            nombreCostGroup = CType(ELL.CostGroupOT.Agrupacion.Serial_tooling, ELL.CostGroupOT.Agrupacion).ToString().ToUpper().Replace("_", " ") & " CUSTOMER" & " (" & costGroup.Estado & ")"
                                                            tipoCostGroup = "customer"
                                                        End If
                                                        costGroupsParaValidacion = listaCostGroupsParaValidacion.FirstOrDefault(Function(f) f.Nombre = nombreCostGroup)
                                                        costGroupsParaValidacion.HorasSuma += HorasSuma
                                                        costGroupsParaValidacion.BACGastosSuma += BACGastosSuma
                                                        costGroupsParaValidacion.PBCSuma += PBCSuma
                                                        costGroupsParaValidacion.Tipo = tipoCostGroup

                                                        ' Cogemos los ultimos valores validados del step. Puede ser que no tenga
                                                        Dim ultimoAprobado As ELL.ValidacionLinea = BLL.ValidacionesLineaBLL.ObtenerUltimoAprobado(stepCG.Id)
                                                        If (ultimoAprobado Is Nothing) Then
                                                            ultimoAprobado = New ELL.ValidacionLinea()
                                                        End If

                                                        costGroupsParaValidacion.HorasSumaValidados += HorasSuma - ultimoAprobado.Hours
                                                        costGroupsParaValidacion.BACGastosSumaValidados += BACGastosSuma - ultimoAprobado.BudgetApproved
                                                        costGroupsParaValidacion.PBCSumaValidados += PBCSuma - ultimoAprobado.PaidByCustomer

                                                        costGroupsParaValidacion.BACGastosSumaUltimoAprobado += ultimoAprobado.BudgetApproved
                                                        costGroupsParaValidacion.PBCSumaUltimoAprobado += ultimoAprobado.PaidByCustomer

                                                        If (Not costGroupsParaValidacion.IdsCostGroup.Contains(costGroup.Id)) Then
                                                            costGroupsParaValidacion.IdsCostGroup.Add(costGroup.Id)
                                                        End If
                                                    Next
                                                End If
                                            End If
                                        Next

                                        For Each costGroupsParaValidacion In listaCostGroupsParaValidacion
                                            ' Componemos una cadena con todos los costgroup que contiene el agrupado separados por ;
                                            Dim costGroupsCadena As String = String.Empty
                                            costGroupsParaValidacion.IdsCostGroup.ForEach(Sub(s) costGroupsCadena &= s & ";")

                                            If (costGroupsParaValidacion.PBCSuma = 0) Then
                                                marginSuma = "NA"
                                            Else
                                                marginSuma = (((costGroupsParaValidacion.PBCSuma - costGroupsParaValidacion.BACGastosSuma) / costGroupsParaValidacion.PBCSuma) * 100).ToString("N2", culturaEsES)
                                            End If

                                            If (costGroupsParaValidacion.PBCSumaValidados = 0) Then
                                                marginSumaV = "NA"
                                            Else
                                                marginSumaV = (((costGroupsParaValidacion.PBCSumaValidados - costGroupsParaValidacion.BACGastosSumaValidados) / costGroupsParaValidacion.PBCSumaValidados) * 100).ToString("N2", culturaEsES)
                                            End If

                                            If (costGroupsParaValidacion.PBCSumaUltimoAprobado = 0) Then
                                                marginSumaUltimoAprobado = "NA"
                                            Else
                                                marginSumaUltimoAprobado = String.Empty
                                            End If

                                            If (marginSuma <> "NA" AndAlso marginSumaUltimoAprobado <> "NA") Then
                                                delta = ((((costGroupsParaValidacion.PBCSuma - costGroupsParaValidacion.BACGastosSuma) / costGroupsParaValidacion.PBCSuma) * 100) - (((costGroupsParaValidacion.PBCSumaUltimoAprobado - costGroupsParaValidacion.BACGastosSumaUltimoAprobado) / costGroupsParaValidacion.PBCSumaUltimoAprobado) * 100)).ToString("N2", culturaEsES)
                                            Else
                                                delta = "NA"
                                            End If

                                            @<tr class='@costGroupsParaValidacion.EstiloFilaDestacada'>
                                                <td class="success">
                                                    @costGroupsParaValidacion.Nombre
                                                </td>
                                                <td align="right">@costGroupsParaValidacion.HorasSumaValidados.ToString("N0", culturaEsES)</td>
                                                <td align="right">@costGroupsParaValidacion.BACGastosSumaValidados.ToString("N0", culturaEsES)</td>
                                                <td align="right">@costGroupsParaValidacion.PBCSumaValidados.ToString("N0", culturaEsES)</td>
                                                @*<td align="right">@marginSumaV</td>*@

                                                <td align="right">@costGroupsParaValidacion.HorasSuma.ToString("N0", culturaEsES)</td>
                                                <td align="right">@costGroupsParaValidacion.BACGastosSuma.ToString("N0", culturaEsES)</td>
                                                <td align="right">@costGroupsParaValidacion.PBCSuma.ToString("N0", culturaEsES)</td>
                                                <td align="right">@marginSuma</td>
                                                <td align="right">@delta</td>
                                                <td style="text-align:center;">
                                                    <input type="submit" id="aprobar" value="@Utils.Traducir("Aprobar")" Class="btn btn-success aprobar" data-idcostgroup="@costGroupsCadena@costGroupsParaValidacion.Tipo" data-idplanta="@planta.IdPlanta" />
                                                    <input type="button" id="rechazar" value="@Utils.Traducir("Rechazar")" Class="btn btn-danger rechazar" data-idcostgroup="@costGroupsCadena@costGroupsParaValidacion.Tipo" data-idplanta="@planta.IdPlanta" />
                                                    @Html.Raw("&nbsp")
                                                    @*@Html.ActionLink(Utils.Traducir("Ver detalle pasos"), "DetalleCostGroup", "Validaciones", New With {.idCostGroup = costGroupsCadena & ";" & costGroupsParaValidacion.Tipo}, Nothing)*@

                                                    <a href="@Url.Action("DetalleCostGroup", "Validaciones", New With {.idCostGroup = costGroupsCadena & ";" & costGroupsParaValidacion.Tipo}, Nothing)" class="btn btn-primary" title="@Utils.Traducir("Ver detalle pasos")">
                                                        <span class="glyphicon glyphicon-eye-open" aria-hidden="true"></span>
                                                    </a>
                                                </td>
                                            </tr>

                                        Next

                                        @code
                                            If (PBCSumaPlanta = 0) Then
                                                marginSumaPlanta = "NA"
                                            Else
                                                marginSumaPlanta = (((PBCSumaPlanta - BACGastosSumaPlanta) / PBCSumaPlanta) * 100).ToString("N2", culturaEsES)
                                            End If

                                            If (PBCSumaPlantaV = 0) Then
                                                marginSumaPlantaV = "NA"
                                            Else
                                                marginSumaPlantaV = (((PBCSumaPlantaV - BACGastosSumaPlantaV) / PBCSumaPlantaV) * 100).ToString("N2", culturaEsES)
                                            End If

                                            If (marginSumaPlanta <> "NA" AndAlso marginSumaPlantaV <> "NA") Then
                                                delta = ((((PBCSumaPlanta - BACGastosSumaPlanta) / PBCSumaPlanta) * 100) - (((PBCSumaPlantaV - BACGastosSumaPlantaV) / PBCSumaPlantaV) * 100)).ToString("N2", culturaEsES)
                                            Else
                                                delta = "NA"
                                            End If
                                        End code

                                        @<tr>
                                            <td class="success">
                                                <b>
                                                    @Utils.Traducir("Total").ToUpper()
                                                </b>
                                            </td>
                                            <td align="right"><b>@HorasSumaPlantaV.ToString("N0", culturaEsES)</b></td>
                                            <td align="right"><b>@BACGastosSumaPlantaV.ToString("N0", culturaEsES)</b></td>
                                            <td align="right"><b>@PBCSumaPlantaV.ToString("N0", culturaEsES)</b></td>
                                            @*<td align="right"><b>@marginSumaPlantaV</b></td>*@

                                            <td align="right"><b>@HorasSumaPlanta.ToString("N0", culturaEsES)</b></td>
                                            <td align="right"><b>@BACGastosSumaPlanta.ToString("N0", culturaEsES)</b></td>
                                            <td align="right"><b>@PBCSumaPlanta.ToString("N0", culturaEsES)</b></td>
                                            <td align="right"><b>@marginSumaPlanta</b></td>
                                            <td align="right">@*<b>@delta</b>*@</td>
                                            <td></td>
                                        </tr>

                                        @code
                                            If (PBCSumaPlantaVentas = 0) Then
                                                marginSumaPlantaVentas = "NA"
                                            Else
                                                marginSumaPlantaVentas = (((PBCSumaPlantaVentas - BACGastosSumaPlantaVentas) / PBCSumaPlantaVentas) * 100).ToString("N2", culturaEsES)
                                            End If

                                            If (PBCSumaPlantaVentasV = 0) Then
                                                marginSumaPlantaVentasV = "NA"
                                            Else
                                                marginSumaPlantaVentasV = (((PBCSumaPlantaVentasV - BACGastosSumaPlantaVentasV) / PBCSumaPlantaVentasV) * 100).ToString("N2", culturaEsES)
                                            End If

                                            If (marginSumaPlantaVentas <> "NA" AndAlso marginSumaPlantaVentasV <> "NA") Then
                                                delta = ((((PBCSumaPlantaVentas - BACGastosSumaPlantaVentas) / PBCSumaPlantaVentas) * 100) - (((PBCSumaPlantaVentasV - BACGastosSumaPlantaVentasV) / PBCSumaPlantaVentasV) * 100)).ToString("N2", culturaEsES)
                                            Else
                                                delta = "NA"
                                            End If
                                        End code

                                        @<tr>
                                            <td class="success">
                                                <b>
                                                    @Utils.Traducir("Total (sólo ventas)").ToUpper()
                                                </b>
                                            </td>

                                            <td></td>
                                            <td align="right"><b>@BACGastosSumaPlantaVentasV.ToString("N0", culturaEsES)</b></td>
                                            <td align="right"><b>@PBCSumaPlantaVentasV.ToString("N0", culturaEsES)</b></td>
                                            @*<td align="right"><b>@marginSumaPlantaVentasV</b></td>*@

                                            <td></td>
                                            <td align="right"><b>@BACGastosSumaPlantaVentas.ToString("N0", culturaEsES)</b></td>
                                            <td align="right"><b>@PBCSumaPlantaVentas.ToString("N0", culturaEsES)</b></td>
                                            <td align="right"><b>@marginSumaPlantaVentas</b></td>
                                            <td align="right">@*<b>@delta</b>*@</td>
                                            <td></td>
                                        </tr>
                                    end code
                                </tbody>
                            </Table>
                        </div>
                    </div>




                                            End Using
            End if
        Next
        @*Tenemos que incluir la plantas para las cuales no se haya enviado ningún paso a validar porque necesitamos los datos de additional info*@
        @code
            @<form class="form-horizontal">

                @For Each idPlanta In cabeceraProyecto.Plantas.Select(Function(f) f.IdPlanta).Except(listaIdsPlantasUnicos.Select(Function(f) CInt(f.IdPlantaSAB)).ToList).OrderByDescending(Function(f) f).ToList()
                    ' Si la planta es CORPORATIVO no se muestran el botón. Me lo comenta así Maite porque son datos calcula
                    ' Sólo tiene sentido para los proyectos de Industrialización
                    If (idPlanta <> 0 AndAlso (cabeceraProyecto.TipoProyPtksis = ELL.CabeceraCostCarrier.TIPO_PROY_INDUSTRIALIZATION OrElse cabeceraProyecto.TipoProyPtksis = ELL.CabeceraCostCarrier.TIPO_PROY_ECO_BATZ) AndAlso usuariosRol.Select(Function(f) f.IdPlanta).Contains(idPlanta)) Then
                        @<div Class="form-group">
                            <label Class="col-sm-2 label label-success text-uppercase">@BLL.PlantasBLL.CargarListadoPorIdPlantaSAB(idPlanta).FirstOrDefault().Planta</label>
                            <div Class="col-sm-10">
                                @code
                                    @code
                                        ' Estos datos son para el vista parcial _ValidacionAdicionalPlanta.vbhtml
                                        Dim viewDataDictonaryInfo As New ViewDataDictionary()
                                        viewDataDictonaryInfo = New ViewDataDictionary()
                                        viewDataDictonaryInfo.Add("IdCabecera", cabeceraProyecto.Id)
                                        viewDataDictonaryInfo.Add("IdPlanta", idPlanta)
                                        viewDataDictonaryInfo.Add("SoloLectura", True)
                                    End Code
                                    @Html.Partial("~/Views/Shared/_ValidacionAdicionalPlanta.vbhtml", viewDataDictonaryInfo)
                                End code
                            </div>
                        </div>
                    End If
                Next

                @code
                    If (steps.Exists(Function(f) f.EsInfoGeneral)) Then
                        @<div Class="form-group">
                            <Label Class="col-sm-12 col-form-label text-danger text-uppercase text-center" style="font-size:larger">@Utils.Traducir("Se solicita validación de la información general (esta validación no contiene pasos)")</Label>
                        </div>
                    End If
                End code
            </form>
        End code

    Else
        @Html.Label(String.Empty, Utils.Traducir("noExisteNingunRegistro"))
    End If
End code

<div class="modal fade" id="modalWindowRechazar" tabindex="-1" role="dialog" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">@Utils.Traducir("Rechazar pasos")</h4>
            </div>
            <div class="modal-body">
                @Using Html.BeginForm("RechazarCostGroup", "Validaciones", FormMethod.Post)
                    @Html.Hidden("hIdCostGroupRechazar")
                    @Html.Hidden("hIdPlantaRechazar")
                    @<div Class="form-group">
                        <label class="control-label">@Utils.Traducir("Motivo del rechazo")</label>

                        @Html.TextArea("txtMotivo", String.Empty, New With {.class = "form-control", .required = "required", .maxlength = "200", .rows = "3"})

                    </div>
                    @<div Class="form-group">
                        <input type="submit" id="btnConfirmRechazo" value="@Utils.Traducir("Rechazar")" class="btn btn-primary input-block-level form-control" />
                    </div>
                End Using
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-dismiss="modal">@Utils.Traducir("Cerrar")</button>
            </div>
        </div>
    </div>
</div>

@*@code
        ' Estos datos son para el vista parcial _ValidacionAdicion.vbhtml
        Dim viewDataDictonary As New ViewDataDictionary()
        viewDataDictonary.Add("IdCabecera", cabeceraProyecto.Id)
        viewDataDictonary.Add("SoloLectura", True)
    End Code

    @Html.Partial("~/Views/Shared/_ValidacionAdicional.vbhtml", viewDataDictonary)*@

@*<div class="modal fade bd-example-modal-lg" id="modalWindowHistorico" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">@Utils.Traducir("Historico")</h4>
                </div>
                <div class="modal-body">
                    <Table id="tabla" Class="table table-condensed table-striped">
                        <thead>
                            <tr>
                                <th id="thDenom">@Utils.Traducir("Denominación del presupuesto")</th>
                                <th id="thDescri">@Utils.Traducir("Descripción del presupuesto")</th>
                                <th id="thRazon">@Utils.Traducir("Razones economicas")</th>
                                <th id="thPG">@Utils.Traducir("Prevista en PG")</th>
                                <th style="text-align:center;">@Utils.Traducir("Fecha")</th>
                            </tr>
                        </thead>
                        <tbody>
                            @code
                                ' Nos saltamos la primera que por orden de fecha descendente en la actual
                                For Each validacion In validaciones.Skip(1)
                                    @<tr>
                                        <td class="tdDenom">
                                            @validacion.Denominacion
                                        </td>
                                        <td class="tdDescri">
                                            @validacion.Descripcion
                                        </td>
                                        <td class="tdRazon">
                                            @validacion.RazonEconomica
                                        </td>
                                        <td class="tdPG">
                                            @If (validacion.PrevistoPG) Then
                                                previstoPG = Utils.Traducir("Si")
                                            Else
                                                previstoPG = Utils.Traducir("No")
                                            End If
                                            @previstoPG
                                        </td>
                                        <td style="text-align:center;">
                                            @validacion.Fecha.ToShortDateString()
                                        </td>
                                    </tr>
                                Next
                            End Code
                        </tbody>
                    </Table>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">@Utils.Traducir("Cerrar")</button>
                </div>
            </div>
        </div>
    </div>*@
