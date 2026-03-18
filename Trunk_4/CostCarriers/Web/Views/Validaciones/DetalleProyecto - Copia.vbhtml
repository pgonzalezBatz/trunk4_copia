@Imports CostCarriersLib

@Code
    ' Cogemos de las validaciones la última
    Dim validacion As ELL.Validacion = CType(ViewData("Validaciones"), List(Of ELL.Validacion)).OrderByDescending(Function(f) f.Fecha).FirstOrDefault()
    Dim cabeceraProyecto As ELL.CabeceraCostCarrier = CType(ViewData("CabeceraProyecto"), ELL.CabeceraCostCarrier)
    Dim steps As List(Of ELL.Step) = CType(ViewData("Steps"), List(Of ELL.Step))
    Dim culturaEsES As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture("es-ES")
    Dim listaIdsPlantasUnicos As IEnumerable(Of Object) = (From lstSteps In steps
                                                           Group lstSteps By lstSteps.IdPlantaSAB, lstSteps.IdPlanta Into agrupacion = Group
                                                           Select New With {.IdPlantaSAB = IdPlantaSAB, .IdPlanta = IdPlanta}).ToList()
End Code

<script type="text/javascript">

    $(function () {
        $(".aprobar").click(function (e) {
            $("#hfIdCostGroup").val($(this).data("idcostgroup"));
            $("#hfIdPlanta").val(22);

            return confirm('@Html.Raw(Utils.Traducir("¿Desea aprobar los pasos sin validar del grupo de coste seleccionado?"))');
        });

        $(".rechazar").click(function (e) {
            $("#hIdCostGroupRechazar").val($(this).data("idcostgroup"));
            $("#hIdPlantaRechazar").val('');
            $('#modalWindowRechazar').modal('show');
        });

        $(".aprobar-todo").click(function (e) {
            $("#hfIdCostGroup").val('');
            $("#hfIdPlanta").val($(this).data("idplanta"));

            return confirm('@Html.Raw(Utils.Traducir("¿Desea aprobar los pasos sin validar de la planta seleccionada?"))');
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

<h3><label>@Utils.Traducir("Validaciones pendientes") - @cabeceraProyecto.NombreProyecto</label></h3>
<hr />

@code
    Dim HorasSuma As Integer = 0
    Dim BACGastosSuma As Integer = 0
    Dim PBCSuma As Integer = 0
    Dim HorasSumaPlanta As Integer = 0
    Dim BACGastosSumaPlanta As Integer = 0
    Dim PBCSumaPlanta As Integer = 0
    Dim marginSuma As String = String.Empty
    Dim listaCostGroups As List(Of ELL.CostGroup) = Nothing
    Dim validacionLineaValidada As ELL.ValidacionLinea = Nothing

    @<span style="padding-right:15px;">@Html.ActionLink(Utils.Traducir("Volver"), "Index")</span>
    @<span>@Html.ActionLink(Utils.Traducir("Ya validados"), "DetalleProyecto", "Totales", New With {.idCabecera = cabeceraProyecto.Id}, Nothing)</span>
    @<br />
    @<br />

    If (listaIdsPlantasUnicos.Count > 0) Then
        @<form class="form-horizontal">
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
                    <Label>@validacion.Descripcion</Label>
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
        </form>

        ' Al ordenar descending por IdPlantaSAB dejamos corporativo al final que es lo que quieren
        @For Each planta In listaIdsPlantasUnicos.OrderByDescending(Function(f) f.IdPlantaSAB).ToList()
            @Using Html.BeginForm("AprobarCostGroup", "Validaciones", FormMethod.Post, New With {.class = "form-horizontal"})
                @Html.Hidden("hfIdCostGroup")
                @Html.Hidden("hfIdPlanta")
                @<div Class="form-group">
                    <label class="col-sm-2 label label-success text-uppercase">@BLL.PlantasBLL.CargarListadoPorIdPlantaSAB(planta.IdPlantaSAB).FirstOrDefault().Planta</label>
                    <div Class="col-sm-10">
                        <input type="submit" id="aprobarTodo" value="@Utils.Traducir("Aprobar todo")" Class="btn btn-success aprobar-todo" data-idplanta="@planta.IdPlanta" />
                        <input type="button" id="rechazarTodo" value="@Utils.Traducir("Rechazar todo")" Class="btn btn-danger rechazar-todo" data-idplanta="@planta.IdPlanta" />
                    </div>
                </div>
                @<div Class="form-group">
                    <label class="col-sm-2 col-form-label">@Utils.Traducir("Resumen del presupuesto")</label>
                    <div Class="col-sm-10">
                        <Table id="tabla" Class="table table-bordered table-condensed">
                            @code
                                ' Si la planta es CORPORATIVO no se muestran el botón. Me lo comenta así Maite porque son datos calculados
                                ' Sólo tiene sentido para los proyectos de Industrialización
                                If (planta.IdPlantaSAB <> 0 AndAlso cabeceraProyecto.IdTipoProyecto = ELL.TipoProyecto.TipoProyecto.Industrialization) Then
                                    @<tr>
                                        <td colspan="6">
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
                                <td Class="danger" style="text-align:center;font-weight:bold">@Utils.Traducir("Horas")</td>
                                <td Class="danger" style="text-align:center;font-weight:bold">@Utils.Traducir("Coste")</td>
                                <td Class="info" style="text-align:center;font-weight:bold">@Utils.Traducir("Dinero pagado por cliente")</td>
                                <td Class="info" style="text-align:center;font-weight:bold">@Utils.Traducir("Margen") %</td>
                                <td></td>
                            </tr>

                            <tbody>
                                @code
                                    ' Vamos a obtener los distintos cost group de los steps
                                    listaCostGroups = BLL.CostsGroupBLL.CargarListadoPorCabeceraPlanta(cabeceraProyecto.Id, planta.IdPlantaSAB)
                                    Dim stepsCostGroup As List(Of ELL.Step)
                                    Dim aprobar As Boolean = False

                                    For Each costGroup In listaCostGroups.OrderBy(Function(f) f.Descripcion)
                                        HorasSuma = 0
                                        BACGastosSuma = 0
                                        PBCSuma = 0

                                        stepsCostGroup = steps.Where(Function(f) f.IdCostGroup = costGroup.Id).ToList()

                                        If (stepsCostGroup IsNot Nothing AndAlso stepsCostGroup.Count > 0) Then
                                            ' Cargamos los valores de validacion de los steps de cada costgroup
                                            stepsCostGroup.ForEach(Sub(s) s.CargarValoresStepValidacion())

                                            For Each stepCG In stepsCostGroup
                                                validacionLineaValidada = BLL.ValidacionesLineaBLL.Obtener(stepCG.IdValidacionLinea)

                                                HorasSuma += validacionLineaValidada.Hours
                                                HorasSumaPlanta += validacionLineaValidada.Hours
                                            Next

                                            BACGastosSuma = stepsCostGroup.Sum(Function(f) f.BACGastosValidacion)
                                            PBCSuma = stepsCostGroup.Sum(Function(f) f.PBCValidacion)
                                            BACGastosSumaPlanta += stepsCostGroup.Sum(Function(f) f.BACGastosValidacion)
                                            PBCSumaPlanta += stepsCostGroup.Sum(Function(f) f.PBCValidacion)

                                            If (PBCSuma = 0) Then
                                                marginSuma = "NA"
                                            Else
                                                marginSuma = (((PBCSuma - BACGastosSuma) / PBCSuma) * 100).ToString("N2", culturaEsES)
                                            End If

                                            @<tr>
                                                <td class="success">
                                                    @String.Format("{0} ({1})", costGroup.Descripcion, costGroup.Estado)
                                                </td>
                                                <td align="right">@HorasSuma.ToString("N0", culturaEsES)</td>
                                                <td align="right">@BACGastosSuma.ToString("N0", culturaEsES)</td>
                                                <td align="right">@PBCSuma.ToString("N0", culturaEsES)</td>
                                                <td align="right">@marginSuma</td>
                                                <td style="text-align:center;">
                                                    <input type="submit" id="aprobar" value="@Utils.Traducir("Aprobar")" Class="btn btn-success aprobar" data-idcostgroup="@costGroup.Id" />
                                                    <input type="button" id="rechazar" value="@Utils.Traducir("Rechazar")" Class="btn btn-danger rechazar" data-idcostgroup="@costGroup.Id" />
                                                    @Html.Raw("&nbsp")
                                                    @Html.ActionLink(Utils.Traducir("Ver detalle pasos"), "DetalleCostGroup", "Validaciones", New With {.idCostGroup = costGroup.Id}, Nothing)
                                                </td>
                                            </tr>
                                        End If
                                    Next
                                    @<tr>
                                        <td class="success">
                                            <b>
                                                @Utils.Traducir("Total").ToUpper()
                                            </b>
                                        </td>
                                        <td align="right"><b>@HorasSumaPlanta.ToString("N0", culturaEsES)</b></td>
                                        <td align="right"><b>@BACGastosSumaPlanta.ToString("N0", culturaEsES)</b></td>
                                        <td align="right"><b>@PBCSumaPlanta.ToString("N0", culturaEsES)</b></td>
                                        <td></td>
                                        <td></td>
                                    </tr>


                                end code
                            </tbody>
                        </Table>
                    </div>
                </div>




                                    End Using

                                    Next
        @*Tenemos que incluir la plantas para las cuales no se haya enviado ningún paso a validar porque necesitamos los datos de additional info*@
        @code
@<form class="form-horizontal">
            @For Each idPlanta In cabeceraProyecto.Plantas.Select(Function(f) f.IdPlanta).Except(listaIdsPlantasUnicos.Select(Function(f) CInt(f.IdPlantaSAB)).ToList).OrderByDescending(Function(f) f).ToList()
                ' Si la planta es CORPORATIVO no se muestran el botón. Me lo comenta así Maite porque son datos calcula
                ' Sólo tiene sentido para los proyectos de Industrialización
                If (idPlanta <> 0 AndAlso cabeceraProyecto.IdTipoProyecto = ELL.TipoProyecto.TipoProyecto.Industrialization) Then
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
