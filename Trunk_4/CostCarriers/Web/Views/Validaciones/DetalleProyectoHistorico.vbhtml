@Imports CostCarriersLib

@Code
    Dim cabeceraProyecto As ELL.CabeceraCostCarrier = CType(ViewData("CabeceraProyecto"), ELL.CabeceraCostCarrier)
    Dim validacion As ELL.Validacion = CType(ViewData("Validacion"), ELL.Validacion)
    Dim validacionesLinea As List(Of ELL.ValidacionLinea) = CType(ViewData("ValidacionesLinea"), List(Of ELL.ValidacionLinea))
    Dim listaIdsPlantasUnicos As New List(Of Integer)

    If (validacionesLinea IsNot Nothing AndAlso validacionesLinea.Count > 0) Then
        listaIdsPlantasUnicos = (From lstVL In validacionesLinea
                                 Group lstVL By lstVL.IdPlanta Into agrupacion = Group
                                 Select IdPlanta).ToList()
    End If
    Dim culturaEsES As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture("es-ES")
    Dim usuariosRol As List(Of ELL.UsuarioRol) = CType(Session("RolesUsuario"), List(Of ELL.UsuarioRol))
End Code

<script type="text/javascript">

    $(function () {
        $("#Tags").change(function () {
            var idCabecera = @cabeceraProyecto.Id;
            var idValidacion = $("#Tags").val();

            if (idValidacion == '') {
                window.location.href = '@Url.Action("DetalleProyectoHistorico", "Validaciones")' + '?IdCabecera=' + idCabecera;
            }
            else {
                window.location.href = '@Url.Action("DetalleProyectoHistorico", "Validaciones")' + '?IdCabecera=' + idCabecera + '&IdValidacion=' + idValidacion;
            }
        })

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

<h3><label>@Utils.Traducir("Estado actual e histórico") - @cabeceraProyecto.NombreProyecto</label></h3>
<hr />

@code
    Dim HorasSuma As Integer = 0
    Dim HorasSumaV As Integer = 0
    Dim BACGastosSuma As Integer = 0
    Dim BACGastosSumaV As Integer = 0
    Dim BACGastosSumaUltimoAprobado As Integer = 0
    Dim PBCSuma As Integer = 0
    Dim PBCSumaV As Integer = 0
    Dim PBCSumaUltimoAprobado As Integer = 0
    Dim marginSuma As String = String.Empty
    Dim marginSumaUltimoAprobado As String = String.Empty
    Dim delta As String = String.Empty

    'Dim realSuma As Integer = 0
    Dim listaIdsCostGroupsUnicos As List(Of Integer) = Nothing
    Dim costGroup As ELL.CostGroup = Nothing
    Dim validacionLinea As ELL.ValidacionLinea = Nothing
    Dim validacionesLineaAux As List(Of ELL.ValidacionLinea)

    @<span style="padding-right:15px;">@Html.ActionLink(Utils.Traducir("Volver"), "IndexHistorico")</span>
    @<span>@Html.ActionLink(Utils.Traducir("Ya validados"), "DetalleProyecto", "Totales", New With {.idCabecera = cabeceraProyecto.Id}, Nothing)</span>
    @<br />
    @<br />

    @<form class="form-horizontal">
        <div Class="form-group">
            <div class="col-sm-2">
                <label class="col-form-label">@Utils.Traducir("Denominación del presupuesto")</label>
            </div>
            <div class="col-sm-4">
                @Html.DropDownList("Tags", Nothing, New With {.class = "form-control"})
            </div>
        </div>
    </form>

    If (validacion IsNot Nothing) Then
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

        If (listaIdsPlantasUnicos.Count = 0) Then
            @<label class="text-danger">@Utils.Traducir("No hay ningún paso validado para ese grupo denominación")</label>
        End If

        @For Each idPlanta In listaIdsPlantasUnicos.OrderByDescending(Function(f) f).ToList()
            @code 
                Dim planta = BLL.PlantasBLL.CargarListadoPorIdPlantaSAB(idPlanta).FirstOrDefault()
            End code
                ' Quito esta condicion despues de mail de Maite del 18/05/2020 a las 10:23
                'If (usuariosRol.Select(Function(f) f.IdPlanta).Contains(idPlanta) OrElse idPlanta = 0) Then
            @<form Class="form-horizontal">
                @Html.Hidden("hfIdCostGroup")
                <div Class="form-group">
                    <label class="col-sm-2 label label-success text-uppercase">@planta.Planta</label>
                    <div Class="col-sm-10">
                        @code
' Si la planta es CORPORATIVO no se muestran el botón. Me lo comenta así Maite porque son datos caldula
' Sólo tiene sentido para los proyectos de Industrialización
                            @*If (idPlanta <> 0 AndAlso cabeceraProyecto.IdTipoProyecto = ELL.TipoProyecto.TipoProyecto.Industrialization) Then
                                    @<button type="button" Class="btn btn-default btn-xs btn-success btn-infoadicional" title='@Utils.Traducir("Info adicional")' data-idplanta="@idPlanta"><span class="glyphicon glyphicon-info-sign" aria-hidden="true"></span></button>
                                End If*@
                        End code
                    </div>
                </div>
                <div Class="form-group">
                    <label class="col-sm-2 col-form-label">@String.Format("{0} ({1})", Utils.Traducir("Resumen del presupuesto"), planta.Moneda)</label>
                    <div Class="col-sm-10">
                        <Table id="tabla" Class="table table-bordered table-condensed">
                            @code
                                If (idPlanta <> 0 AndAlso (cabeceraProyecto.TipoProyPtksis = ELL.CabeceraCostCarrier.TIPO_PROY_INDUSTRIALIZATION OrElse cabeceraProyecto.TipoProyPtksis = ELL.CabeceraCostCarrier.TIPO_PROY_ECO_BATZ)) Then
                                    @<tr>
                                        <td colspan="6">
                                            @code
                                                ' Estos datos son para el vista parcial _ValidacionAdicionalPlanta.vbhtml
                                                Dim viewDataDictonaryInfo As New ViewDataDictionary()
                                                viewDataDictonaryInfo = New ViewDataDictionary()
                                                viewDataDictonaryInfo.Add("IdCabecera", cabeceraProyecto.Id)
                                                viewDataDictonaryInfo.Add("IdPlanta", idPlanta)
                                                viewDataDictonaryInfo.Add("SoloLectura", True)
                                            End Code
                                            @Html.Partial("~/Views/Shared/_ValidacionAdicionalPlanta.vbhtml", viewDataDictonaryInfo)
                                        </td>
                                    </tr>
                                                End If
                            End code

                            <tr>
                                <td></td>
                                <td colspan="3" style="text-align:center;font-weight:bold">@Utils.Traducir("Nueva petición")</td>
                                <td colspan="4" style="text-align:center;font-weight:bold;text-transform:uppercase; color:#ffffff;background-color:#000000;">@Utils.Traducir("Nuevo estado total")</td>
                                <td></td>
                                <td></td>
                            </tr>


                            <tr>
                                <td></td>
                                <td Class="danger" style="text-align:center;font-weight:bold">@Utils.Traducir("Horas")</td>
                                <td Class="danger" style="text-align:center;font-weight:bold">@Utils.Traducir("Coste")</td>
                                <td Class="info" style="text-align:center;font-weight:bold">@Utils.Traducir("Dinero pagado por cliente")</td>
                                <td Class="danger" style="text-align:center;font-weight:bold;">@Utils.Traducir("Horas")</td>
                                <td Class="danger" style="text-align:center;font-weight:bold;">@Utils.Traducir("Coste")</td>
                                <td Class="info" style="text-align:center;font-weight:bold;">@Utils.Traducir("Dinero pagado por cliente")</td>
                                <td Class="info" style="text-align:center;font-weight:bold;">@Utils.Traducir("Margen") %</td>
                                <td Class="info" style="text-align:center;font-weight:bold">@Utils.Traducir("Delta")</td>
                                <td></td>
                            </tr>

                            <tbody>
                                @code
                                    Dim listaCostGroupsParaValidacion As New List(Of ELL.CostGroupParaValidacion)
                                    Dim costGroupParaValidacion As ELL.CostGroupParaValidacion
                                    Dim nombreCostGroup As String = String.Empty
                                    Dim tipoCostGroup As String = String.Empty

                                    ' Vamos a obtener los distintos cost group
                                    listaIdsCostGroupsUnicos = (From lstVL In validacionesLinea
                                                                Where lstVL.IdPlanta = idPlanta
                                                                Group lstVL By lstVL.IdCostGroup Into agrupacion = Group
                                                                Select IdCostGroup).ToList()

                                    For Each idCostGroup In listaIdsCostGroupsUnicos
                                        HorasSuma = 0
                                        HorasSumaV = 0
                                        BACGastosSuma = 0
                                        BACGastosSumaV = 0
                                        PBCSuma = 0
                                        PBCSumaV = 0
                                        'realSuma = 0

                                        costGroup = BLL.CostsGroupBLL.Obtener(idCostGroup)

                                        ' Tenemos que cargar las validaciones porque las que cargamos en el controlador viene agrupadas y no nos sirven
                                        validacionesLineaAux = BLL.ValidacionesLineaBLL.CargarListadoPorValidacion(validacion.Id).Where(Function(f) f.IdCostGroup = idCostGroup).ToList()

                                        If (costGroup.IdAgrupacion = ELL.CostGroupOT.Agrupacion.Serial_tooling) Then
                                            For Each validacionLinea In validacionesLineaAux
                                                If (validacionLinea.PaidByCustomer = 0) Then
                                                    nombreCostGroup = CType(ELL.CostGroupOT.Agrupacion.Serial_tooling, ELL.CostGroupOT.Agrupacion).ToString().ToUpper().Replace("_", " ") & " BATZ" & " (" & costGroup.Estado & ")"
                                                    tipoCostGroup = "batz"
                                                ElseIf (validacionLinea.PaidByCustomer > 0) Then
                                                    nombreCostGroup = CType(ELL.CostGroupOT.Agrupacion.Serial_tooling, ELL.CostGroupOT.Agrupacion).ToString().ToUpper().Replace("_", " ") & " CUSTOMER" & " (" & costGroup.Estado & ")"
                                                    tipoCostGroup = "customer"
                                                End If

                                                If (Not listaCostGroupsParaValidacion.Exists(Function(f) f.Nombre = nombreCostGroup)) Then
                                                    costGroupParaValidacion = New ELL.CostGroupParaValidacion With {.Nombre = nombreCostGroup}
                                                    listaCostGroupsParaValidacion.Add(costGroupParaValidacion)
                                                Else
                                                    costGroupParaValidacion = listaCostGroupsParaValidacion.FirstOrDefault(Function(f) f.Nombre = nombreCostGroup)
                                                End If

                                                costGroupParaValidacion.HorasSuma += validacionLinea.Hours
                                                costGroupParaValidacion.BACGastosSuma += validacionLinea.BudgetApproved
                                                costGroupParaValidacion.PBCSuma += validacionLinea.PaidByCustomer
                                                costGroupParaValidacion.Tipo = tipoCostGroup

                                                If (Not costGroupParaValidacion.IdsCostGroup.Contains(costGroup.Id)) Then
                                                    costGroupParaValidacion.IdsCostGroup.Add(costGroup.Id)
                                                End If

                                                ' Hay que obtener la anterior validación linea a la validación que estamos mirando
                                                Dim valLineaAnterior As ELL.ValidacionLinea = BLL.ValidacionesLineaBLL.ObtenerAnteriorAprobada(validacionLinea)

                                                costGroupParaValidacion.HorasSumaValidados += validacionLinea.Hours - valLineaAnterior.Hours
                                                costGroupParaValidacion.BACGastosSumaValidados += validacionLinea.BudgetApproved - valLineaAnterior.BudgetApproved
                                                costGroupParaValidacion.PBCSumaValidados += validacionLinea.PaidByCustomer - valLineaAnterior.PaidByCustomer

                                                costGroupParaValidacion.BACGastosSumaUltimoAprobado += valLineaAnterior.BudgetApproved
                                                costGroupParaValidacion.PBCSumaUltimoAprobado += valLineaAnterior.PaidByCustomer
                                            Next

                                            If (costGroupParaValidacion.PBCSuma = 0) Then
                                                marginSuma = "NA"
                                            Else
                                                marginSuma = (((costGroupParaValidacion.PBCSuma - costGroupParaValidacion.BACGastosSuma) / costGroupParaValidacion.PBCSuma) * 100).ToString("N2", culturaEsES)
                                            End If

                                            If (costGroupParaValidacion.PBCSumaUltimoAprobado = 0) Then
                                                marginSumaUltimoAprobado = "NA"
                                            Else
                                                marginSumaUltimoAprobado = String.Empty
                                            End If

                                            If (marginSuma <> "NA" AndAlso marginSumaUltimoAprobado <> "NA") Then
                                                delta = ((((costGroupParaValidacion.PBCSuma - costGroupParaValidacion.BACGastosSuma) / costGroupParaValidacion.PBCSuma) * 100) - (((costGroupParaValidacion.PBCSumaUltimoAprobado - costGroupParaValidacion.BACGastosSumaUltimoAprobado) / costGroupParaValidacion.PBCSumaUltimoAprobado) * 100)).ToString("N2", culturaEsES)
                                            Else
                                                delta = "NA"
                                            End If

                                        ElseIf (costGroup.IdAgrupacion <> ELL.CostGroupOT.Agrupacion.Serial_tooling) Then
                                            For Each validacionLinea In validacionesLineaAux
                                                HorasSuma += validacionLinea.Hours
                                                BACGastosSuma += validacionLinea.BudgetApproved
                                                PBCSuma += validacionLinea.PaidByCustomer
                                                'realSuma += validacionLinea.RealDataInt_Ext

                                                ' Hay que obtener la anterior validación linea a la validación que estamos mirando
                                                Dim valLineaAnterior As ELL.ValidacionLinea = BLL.ValidacionesLineaBLL.ObtenerAnteriorAprobada(validacionLinea)

                                                HorasSumaV += validacionLinea.Hours - valLineaAnterior.Hours
                                                BACGastosSumaV += validacionLinea.BudgetApproved - valLineaAnterior.BudgetApproved
                                                PBCSumaV += validacionLinea.PaidByCustomer - valLineaAnterior.PaidByCustomer

                                                BACGastosSumaUltimoAprobado += valLineaAnterior.BudgetApproved
                                                PBCSumaUltimoAprobado += valLineaAnterior.PaidByCustomer
                                            Next

                                            If (PBCSuma = 0) Then
                                                marginSuma = "NA"
                                            Else
                                                marginSuma = (((PBCSuma - BACGastosSuma) / PBCSuma) * 100).ToString("N2", culturaEsES)
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

                                            @<tr>
                                                <td class="success">
                                                    @String.Format("{0} ({1})", costGroup.Descripcion, costGroup.Estado)
                                                </td>                                                

                                                <td align="right">@HorasSumaV.ToString("N0", culturaEsES)</td>
                                                <td align="right">@BACGastosSumaV.ToString("N0", culturaEsES)</td>
                                                <td align="right">@PBCSumaV.ToString("N0", culturaEsES)</td>

                                                <td align="right">@HorasSuma.ToString("N0", culturaEsES)</td>
                                                <td align="right">@BACGastosSuma.ToString("N0", culturaEsES)</td>
                                                <td align="right">@PBCSuma.ToString("N0", culturaEsES)</td>
                                                <td align="right">@marginSuma</td>
                                                <td align="right">@delta</td>
                                                @*<td align="right">@realSuma.ToString("N0", culturaEsES)</td>*@
                                                <td style="text-align:center;">
                                                    @*@Html.ActionLink(Utils.Traducir("Ver detalle pasos"), "DetalleCostGroupHistorico", "Validaciones", New With {.idCostGroup = costGroup.Id, .idValidacion = validacion.Id}, Nothing)*@

                                                    <a href="@Url.Action("DetalleCostGroupHistorico", "Validaciones", New With {.idCostGroup = costGroup.Id, .idValidacion = validacion.Id}, Nothing)" class="btn btn-primary" title="@Utils.Traducir("Ver detalle pasos")">
                                                        <span class="glyphicon glyphicon-eye-open" aria-hidden="true"></span>
                                                    </a>
                                                </td>
                                            </tr>
                                        End If
                                    Next

                                    For Each costGroupParaValidacion In listaCostGroupsParaValidacion
                                        ' Componemos una cadena con todos los costgroup que contiene el agrupado separados por ;
                                        Dim costGroupsCadena As String = String.Empty
                                        costGroupParaValidacion.IdsCostGroup.ForEach(Sub(s) costGroupsCadena &= s & ";")

                                        If (costGroupParaValidacion.PBCSumaValidados = 0) Then
                                            marginSuma = "NA"
                                        Else
                                            marginSuma = (((costGroupParaValidacion.PBCSumaValidados - costGroupParaValidacion.BACGastosSumaValidados) / costGroupParaValidacion.PBCSumaValidados) * 100).ToString("N2", culturaEsES)
                                        End If

                                        @<tr>
                                            <td class="success">
                                                @costGroupParaValidacion.Nombre
                                            </td>

                                            <td align="right">@costGroupParaValidacion.HorasSumaValidados.ToString("N0", culturaEsES)</td>
                                            <td align="right">@costGroupParaValidacion.BACGastosSumaValidados.ToString("N0", culturaEsES)</td>
                                            <td align="right">@costGroupParaValidacion.PBCSumaValidados.ToString("N0", culturaEsES)</td>

                                            <td align="right">@costGroupParaValidacion.HorasSuma.ToString("N0", culturaEsES)</td>
                                            <td align="right">@costGroupParaValidacion.BACGastosSuma.ToString("N0", culturaEsES)</td>
                                            <td align="right">@costGroupParaValidacion.PBCSuma.ToString("N0", culturaEsES)</td>
                                            <td align="right">@marginSuma</td>
                                            <td align="right">@delta</td>
                                            @*<td align="right">@costGroupParaValidacion.RealSumaValidados.ToString("N0", culturaEsES)</td>*@
                                            <td style="text-align:center;">
                                                @*@Html.ActionLink(Utils.Traducir("Ver detalle pasos"), "DetalleCostGroupHistorico", "Validaciones", New With {.idCostGroup = costGroupsCadena & ";" & costGroupParaValidacion.Tipo, .idValidacion = validacion.Id}, Nothing)*@

                                                <a href="@Url.Action("DetalleCostGroupHistorico", "Validaciones", New With {.idCostGroup = costGroupsCadena & ";" & costGroupParaValidacion.Tipo, .idValidacion = validacion.Id}, Nothing)" class="btn btn-primary" title="@Utils.Traducir("Ver detalle pasos")">
                                                    <span class="glyphicon glyphicon-eye-open" aria-hidden="true"></span>
                                                </a>
                                            </td>
                                        </tr>
                                    Next
                                end code
                            </tbody>
                        </Table>
                    </div>
                </div>
            </form>
                                    'End if

            @<form class="form-horizontal">
                @For Each idPlanta1 In cabeceraProyecto.Plantas.Select(Function(f) f.IdPlanta).Except(listaIdsPlantasUnicos.Select(Function(f) f).ToList).OrderByDescending(Function(f) f).ToList()
                    ' Si la planta es CORPORATIVO no se muestran el botón. Me lo comenta así Maite porque son datos calcula
                    ' Sólo tiene sentido para los proyectos de Industrialización
                    If (idPlanta1 <> 0 AndAlso (cabeceraProyecto.TipoProyPtksis = ELL.CabeceraCostCarrier.TIPO_PROY_INDUSTRIALIZATION OrElse cabeceraProyecto.TipoProyPtksis = ELL.CabeceraCostCarrier.TIPO_PROY_ECO_BATZ) AndAlso usuariosRol.Select(Function(f) f.IdPlanta).Contains(idPlanta1)) Then
                        @<div Class="form-group">
                            <label Class="col-sm-2 label label-success text-uppercase">@BLL.PlantasBLL.CargarListadoPorIdPlantaSAB(idPlanta1).FirstOrDefault().Planta</label>
                            <div Class="col-sm-10">
                                @code
                                    @code
                                        ' Estos datos son para el vista parcial _ValidacionAdicionalPlanta.vbhtml
                                        Dim viewDataDictonaryInfo As New ViewDataDictionary()
                                        viewDataDictonaryInfo = New ViewDataDictionary()
                                        viewDataDictonaryInfo.Add("IdCabecera", cabeceraProyecto.Id)
                                        viewDataDictonaryInfo.Add("IdPlanta", idPlanta1)
                                        viewDataDictonaryInfo.Add("SoloLectura", True)
                                    End Code
                                    @Html.Partial("~/Views/Shared/_ValidacionAdicionalPlanta.vbhtml", viewDataDictonaryInfo)
                                End code
                            </div>
                        </div>
                                            End If
                                        Next
            </form>

                                        Next
    End if
End code

@*@code
        ' Estos datos son para el vista parcial _ValidacionAdicion.vbhtml
        Dim viewDataDictonary As New ViewDataDictionary()
        viewDataDictonary.Add("IdCabecera", cabeceraProyecto.Id)
        viewDataDictonary.Add("SoloLectura", True)
    End Code

    @Html.Partial("~/Views/Shared/_ValidacionAdicional.vbhtml", viewDataDictonary)*@
