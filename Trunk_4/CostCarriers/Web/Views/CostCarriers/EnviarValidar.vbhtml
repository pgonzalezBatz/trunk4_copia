@Imports CostCarriersLib

<h3><label>@Utils.Traducir("Envío validación pasos")</label></h3>
<hr />

@Code
    Dim steps As List(Of ELL.Step) = CType(ViewData("Steps"), List(Of ELL.Step))

    Dim cabecera As ELL.CabeceraCostCarrier = BLL.CabecerasCostCarrierBLL.Obtener(steps(0).IdCabecera, False)
    Dim ultimaValidacion As ELL.Validacion = Nothing
    Dim previstoPG As Boolean = BLL.ProyectosPtksisBLL.EstaEnPG(cabecera.Proyecto, DateTime.Today.Year)

    If (Session("DatosValidacion") IsNot Nothing) Then
        Dim datosValidacion As DatosValidacion = CType(Session("DatosValidacion"), DatosValidacion)
        ultimaValidacion = New ELL.Validacion With {.Denominacion = datosValidacion.Denominacion, .Descripcion = datosValidacion.Descripcion, .PrevistoPG = datosValidacion.PrevistoPG}
    ElseIf (ViewData("UltimaValidacion") IsNot Nothing) Then
        ultimaValidacion = CType(ViewData("UltimaValidacion"), ELL.Validacion)
    End If

    Dim culturaEsES As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture("es-ES")
    Dim viewDataDictonaryInfo As New ViewDataDictionary()
End code

<script type="text/javascript">
    $(function () {
        //Con esto ocultamos lo que no queremos al enviar a validar solo info general
        if ("@steps.Exists(Function(f) f.EsInfoGeneral)" == "True") {
            $(".ocultar").hide();
        }

        @*$(".btn-infoadicional").click(function () {
            var denominacion = $("#denominacion").val();
            var descripcion = $("#descripcion").val();
            var previstoPg = $('#previstoPg').val();

            $.ajax({
                url: '@Url.Action("GuardarDatosValidacion", "CostCarriers")',
                data: { denominacion: denominacion, descripcion: descripcion, previstoPg: previstoPg},
                type: 'GET',
                dataType: 'json'
            });

            var idPlanta = $(this).data("idplanta");
            $("#hIdPlanta").val(idPlanta);
            var BACGastosSumaPlanta = parseInt($("#lblBACGastosSumaPlanta-" + idPlanta).text().replace(/\./g, ""));
            var PBCSumaplanta = parseInt($("#lblPBCSumaplanta-" + idPlanta).text().replace(/\./g, ""));
            $.ajax({
                url: '@Url.Action("CargarDatosValidacionInfoAdicional", "CostCarriers")',
                data: { idCabecera: @cabecera.Id, idPlanta: idPlanta, idTipo: @CInt(ELL.ValidacionInfoAdicional.TipoDato.Awarding_offer)},
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
                            $("#txtBeneficioHerramentalAO").val(d.CustomerProperty).replace(".", ","));
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
                            $("#txtAnyosSerieAO").val(d.SeriesYears);
                        }
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                }
            });

            $.ajax({
                url: '@Url.Action("CargarDatosValidacionInfoAdicional", "CostCarriers")',
                data: { idCabecera: @cabecera.Id, idPlanta: idPlanta, idTipo: @CInt(ELL.ValidacionInfoAdicional.TipoDato.Current_values), cargarSoloDeSesion: true},
                type: 'GET',
                dataType: 'json',
                success: function (d) {
                    $("#txtMargenNeto").val('');
                    $("#txtVentasEfectivas").val('');
                    $("#txtBeneficioHerramental").val(PBCSumaplanta - BACGastosSumaPlanta);
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
                            $("#txtBeneficioHerramental").val(d.CustomerProperty).replace(".", ","));
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
                            $("#txtAnyosSerie").val(d.SeriesYears);
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
    })
</script>

@Using Html.BeginForm("EnviarValidar", "CostCarriers", FormMethod.Post, New With {.class = "form-horizontal"})
    @Html.Hidden("hfIdCabecera", cabecera.Id)
    @*@<input type="submit" id="volver" name="submitButtonBack" value="@Utils.Traducir("Volver a solicitud")" Class="btn btn-primary" />*@
    @<input type="submit" id="validar" name="submitButtonSendValidate" value="@Utils.Traducir("Enviar a validar")" Class="btn btn-primary" />
    @<br />
    @<br />
    @<div Class="form-group">
        <label class="col-sm-2 col-form-label">@Utils.Traducir("Proyecto")</label>
        <label class="col-sm-10">@cabecera.NombreProyecto</label>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 col-form-label">@Utils.Traducir("Denominación del presupuesto")</label>
        <div class="col-sm-10">
            @Html.TextBox("denominacion", If(ultimaValidacion IsNot Nothing, ultimaValidacion.Denominacion, String.Empty), New With {.maxlength = "500", .required = "required", .Class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 col-form-label">@Utils.Traducir("Descripción del presupuesto")</label>
        <div class="col-sm-10">
            @Html.TextArea("descripcion", If(ultimaValidacion IsNot Nothing, ultimaValidacion.Descripcion, String.Empty), New With {.maxlength = "1000", .rows = "5", .required = "required", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 col-form-label">@Utils.Traducir("Prevista en PG")</label>
        <div class="col-sm-10">
            @code
                If (previstoPG) Then
                    @<label>@Utils.Traducir("Si")</label>
                Else
                    @<label>@Utils.Traducir("No")</label>
                End If
                @Html.Hidden("previstoPg", previstoPG)
            End Code
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 col-form-label">@Utils.Traducir("Resumen del presupuesto")</label>

        @code
            ' Vamos a obtener las distintas plantas de los steps
            Dim listaIdsPlantasUnicos As List(Of Integer) = (From lstStep In steps
                                                             Group lstStep By lstStep.IdPlantaSAB Into agrupacion = Group
                                                             Select IdPlantaSAB).ToList()
            Dim listaCostGroups As List(Of ELL.CostGroup) = Nothing
            Dim HorasSuma As Integer = Integer.MinValue
            Dim BACGastosSuma As Integer = Integer.MinValue
            Dim PBCSuma As Integer = Integer.MinValue
            Dim marginSuma As String = String.Empty
            Dim HorasSumaValidados As Integer = Integer.MinValue
            Dim BACGastosSumaValidados As Integer = Integer.MinValue
            Dim PBCSumaValidados As Integer = Integer.MinValue
            Dim HorasSumaPlanta As Integer = Integer.MinValue
            Dim BACGastosSumaPlanta As Integer = Integer.MinValue
            Dim PBCSumaplanta As Integer = Integer.MinValue
            Dim marginSumaPlanta As String = String.Empty
            Dim BACGastosSumaPlantaVentas As Integer = Integer.MinValue
            Dim PBCSumaplantaVentas As Integer = Integer.MinValue
            Dim marginSumaPlantaVentas As String = String.Empty
            Dim estiloFilaDestacada As String = String.Empty
            Dim validacionLineaValidada As ELL.ValidacionLinea = Nothing
            Dim stepAux As ELL.Step = Nothing

            If (listaIdsPlantasUnicos.Count > 0) Then
                @<div Class="col-sm-10">
                    <table id="tabla" Class="table table-bordered table-condensed">
                        @For Each idPlanta In listaIdsPlantasUnicos
                            HorasSumaPlanta = 0
                            BACGastosSumaPlanta = 0
                            PBCSumaplanta = 0
                            BACGastosSumaPlantaVentas = 0
                            PBCSumaplantaVentas = 0
                            @<thead class="ocultar">
                                <tr>
                                    <th Class="success" style="text-align:center;text-transform:uppercase" colspan="5">
                                        @code
                                            Dim paso = steps.FirstOrDefault(Function(f) f.IdPlantaSAB = idPlanta)
                                        End code

                                        @String.Format("{0} ({1})", paso.Planta, paso.Moneda)
                                    </th>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th Class="danger" style="text-align:center;">@Utils.Traducir("Horas")</th>
                                    <th Class="danger" style="text-align:center;">@Utils.Traducir("Coste")</th>
                                    <th Class="info" style="text-align:center;">@Utils.Traducir("Dinero pagado por cliente")</th>
                                    <th Class="info" style="text-align:center;">@Utils.Traducir("Margen") %</th>
                                </tr>
                            </thead>
                            @<tbody class="ocultar">
                                @code
                                    ' Vamos a obtener todos los costgroup de este proyecto para esta planta
                                    listaCostGroups = BLL.CostsGroupBLL.CargarListadoPorCabeceraPlanta(steps(0).IdCabecera, idPlanta)
                                    Dim nombreCostGroup As String = String.Empty
                                                        Dim listaCostGroupsParaValidacion As New List(Of ELL.CostGroupParaValidacion)
                                                        Dim costGroupsParaValidacion As ELL.CostGroupParaValidacion

                                                        ' Como quieren agrupados los cost group de SERIAL TOOLING los vamos a tratar aparte. Primero creamos los grupos ficticios
                                                        For Each costGroup In listaCostGroups.Where(Function(f) f.IdAgrupacion = ELL.CostGroupOT.Agrupacion.Serial_tooling)
                                                        For i = 0 To 1
                                            nombreCostGroup = CType(ELL.CostGroupOT.Agrupacion.Serial_tooling, ELL.CostGroupOT.Agrupacion).ToString().ToUpper().Replace("_", " ") & " " & If(i = 0, "BATZ", "CUSTOMER") & " (" & costGroup.Estado & ")"
                                            If (Not listaCostGroupsParaValidacion.Exists(Function(f) f.Nombre = nombreCostGroup)) Then
                                                listaCostGroupsParaValidacion.Add(New ELL.CostGroupParaValidacion With {.Nombre = nombreCostGroup})
                                            End If
                                                        Next
                                                        Next

                                                        For Each costGroup In listaCostGroups.OrderBy(Function(f) f.Descripcion)
                                        HorasSuma = 0
                                        BACGastosSuma = 0
                                        PBCSuma = 0
                                        HorasSumaValidados = 0
                                        BACGastosSumaValidados = 0
                                        PBCSumaValidados = 0
                                        estiloFilaDestacada = String.Empty

                                        ' Cargamos las validaciones linea validados
                                        validacionLineaValidada = BLL.ValidacionesLineaBLL.ObtenerValidadoByCostGroup(costGroup.Id)

                                        If (validacionLineaValidada IsNot Nothing) Then
                                            HorasSumaValidados = validacionLineaValidada.Hours
                                            BACGastosSumaValidados = validacionLineaValidada.BudgetApproved
                                            PBCSumaValidados = validacionLineaValidada.PaidByCustomer
                                        End If

                                                        For Each paso In costGroup.Steps
                                                        If (paso.IdEstadoValidacion = ELL.Validacion.Estado.Approved OrElse paso.IdEstadoValidacion = ELL.Validacion.Estado.Opened) Then
                                                BACGastosSuma += paso.BACGastosValidacion
                                                PBCSuma += paso.PBCValidacion
                                            End If

                                                        ' Si se los pasos del costgroup alguno es de los que se ha enviado a validar se trata
                                                        If (steps.Exists(Function(f) f.Id = paso.Id)) Then
                                                stepAux = steps.FirstOrDefault(Function(f) f.Id = paso.Id)
                                                If (stepAux.OBCOrigenDatos = ELL.OrigenDatosStep.OrigenDatosStep.Presupuesto_bonos) Then

                                                        ' Si hay algun paso con porcentaje...
                                                        If (stepAux.Porcentaje <> Integer.MinValue) Then
                                                        HorasSuma = costGroup.Horas * stepAux.Porcentaje / 100
                                                        HorasSumaPlanta += costGroup.Horas * stepAux.Porcentaje / 100
                                                    Else
                                                        HorasSuma = costGroup.Horas
                                                        HorasSumaPlanta += costGroup.Horas
                                                    End If
                                                        End If

                                                        If (stepAux IsNot Nothing) Then
                                                    BACGastosSuma += stepAux.BACGastos
                                                    PBCSuma += stepAux.PBC
                                                    BACGastosSumaPlanta += stepAux.BACGastos
                                                    PBCSumaplanta += stepAux.PBC
                                                End If

                                                estiloFilaDestacada = "warning text-danger"

                                                If (costGroup.IdAgrupacion = ELL.CostGroupOT.Agrupacion.Serial_tooling) Then
                                                        If (stepAux.PBC = 0) Then
                                                        nombreCostGroup = CType(ELL.CostGroupOT.Agrupacion.Serial_tooling, ELL.CostGroupOT.Agrupacion).ToString().ToUpper().Replace("_", " ") & " BATZ" & " (" & costGroup.Estado & ")"
                                                    ElseIf (stepAux.PBC > 0) Then
                                                        nombreCostGroup = CType(ELL.CostGroupOT.Agrupacion.Serial_tooling, ELL.CostGroupOT.Agrupacion).ToString().ToUpper().Replace("_", " ") & " CUSTOMER" & " (" & costGroup.Estado & ")"
                                                    End If
                                                    costGroupsParaValidacion = listaCostGroupsParaValidacion.FirstOrDefault(Function(f) f.Nombre = nombreCostGroup)
                                                    costGroupsParaValidacion.HorasSumaValidados = HorasSumaValidados
                                                    costGroupsParaValidacion.BACGastosSumaValidados = BACGastosSumaValidados
                                                    costGroupsParaValidacion.PBCSumaValidados = PBCSumaValidados
                                                    costGroupsParaValidacion.HorasSuma += HorasSuma
                                                    costGroupsParaValidacion.BACGastosSuma += stepAux.BACGastos
                                                    costGroupsParaValidacion.PBCSuma += stepAux.PBC
                                                    costGroupsParaValidacion.EstiloFilaDestacada = estiloFilaDestacada
                                                End If
                                                        End If
                                                        Next

                                                        If (PBCSuma = 0) Then
                                            marginSuma = "NA"
                                        Else
                                            marginSuma = (((PBCSuma - BACGastosSuma) / PBCSuma) * 100).ToString("N2", culturaEsES)
                                            BACGastosSumaPlantaVentas += BACGastosSuma
                                            PBCSumaplantaVentas += PBCSuma
                                        End If

                                                        If (costGroup.IdAgrupacion = Integer.MinValue OrElse costGroup.IdAgrupacion <> ELL.CostGroupOT.Agrupacion.Serial_tooling) Then
                                            @<tr class='@estiloFilaDestacada'>
                                                <td class="success">
                                                    @String.Format("{0} ({1})", costGroup.Descripcion, costGroup.Estado)
                                                </td>
                                                <td align="right" title='@String.Format("{0}: {1}", Utils.Traducir("Validado"), HorasSumaValidados.ToString("N0", culturaEsES))'>@HorasSuma.ToString("N0", culturaEsES)</td>
                                                <td align="right" title='@String.Format("{0}: {1}", Utils.Traducir("Validado"), BACGastosSumaValidados.ToString("N0", culturaEsES))'>@BACGastosSuma.ToString("N0", culturaEsES)</td>
                                                <td align="right" title='@String.Format("{0}: {1}", Utils.Traducir("Validado"), PBCSumaValidados.ToString("N0", culturaEsES))'>@PBCSuma.ToString("N0", culturaEsES)</td>
                                                <td align="right">@marginSuma</td>
                                            </tr> End If
                                    Next
                                                            For Each costGroupsParaValidacion In listaCostGroupsParaValidacion
                                                            If (costGroupsParaValidacion.PBCSuma = 0) Then
                                            marginSuma = "NA"
                                        Else
                                            marginSuma = (((costGroupsParaValidacion.PBCSuma - costGroupsParaValidacion.BACGastosSuma) / costGroupsParaValidacion.PBCSuma) * 100).ToString("N2", culturaEsES)
                                        End If

                                        @<tr class='@costGroupsParaValidacion.EstiloFilaDestacada'>
                                            <td class="success">
                                                @costGroupsParaValidacion.Nombre
                                            </td>
                                            <td align="right" title='@String.Format("{0}: {1}", Utils.Traducir("Validado"), costGroupsParaValidacion.HorasSumaValidados.ToString("N0", culturaEsES))'>@costGroupsParaValidacion.HorasSuma.ToString("N0", culturaEsES)</td>
                                            <td align="right" title='@String.Format("{0}: {1}", Utils.Traducir("Validado"), costGroupsParaValidacion.BACGastosSumaValidados.ToString("N0", culturaEsES))'>@costGroupsParaValidacion.BACGastosSuma.ToString("N0", culturaEsES)</td>
                                            <td align="right" title='@String.Format("{0}: {1}", Utils.Traducir("Validado"), costGroupsParaValidacion.PBCSumaValidados.ToString("N0", culturaEsES))'>@costGroupsParaValidacion.PBCSuma.ToString("N0", culturaEsES)</td>
                                            <td align="right">@marginSuma</td>
                                        </tr>
                                    Next

                                    @code
                                        If (PBCSumaplanta = 0) Then
                                            marginSumaPlanta = "NA"
                                        Else
                                            marginSumaPlanta = (((PBCSumaplanta - BACGastosSumaPlanta) / PBCSumaplanta) * 100).ToString("N2", culturaEsES)
                                        End If
                                                                End code

                                    @<tr>
                                        <td class="success">
                                            <b>
                                                @Utils.Traducir("Total").ToUpper()
                                            </b>
                                        </td>
                                        <td align="right"><b>@HorasSumaPlanta.ToString("N0", culturaEsES)</b></td>
                                        <td align="right"><b><label id="@String.Format("lblBACGastosSumaPlanta-{0}", idPlanta)">@BACGastosSumaPlanta.ToString("N0", culturaEsES)</label></b></td>
                                        <td align="right"><b><label id="@String.Format("lblPBCSumaplanta-{0}", idPlanta)">@PBCSumaplanta.ToString("N0", culturaEsES)</label></b></td>
                                        <td align="right"><b>@marginSumaPlanta</b></td>
                                    </tr>

                                    @code
                                        If (PBCSumaplantaVentas = 0) Then
                                            marginSumaPlantaVentas = "NA"
                                        Else
                                            marginSumaPlantaVentas = (((PBCSumaplantaVentas - BACGastosSumaPlantaVentas) / PBCSumaplantaVentas) * 100).ToString("N2", culturaEsES)
                                        End If
                                                                    End code

                                    @<tr>
                                        <td class="success">
                                            <b>
                                                @Utils.Traducir("Total (sólo ventas)").ToUpper()
                                            </b>
                                        </td>
                                        <td></td>
                                        <td align="right"><b><label>@BACGastosSumaPlantaVentas.ToString("N0", culturaEsES)</label></b></td>
                                        <td align="right"><b><label>@PBCSumaplantaVentas.ToString("N0", culturaEsES)</label></b></td>
                                        <td align="right"><b>@marginSumaPlantaVentas</b></td>
                                    </tr>

                                        ' Si la planta es CORPORATIVO no se muestran el botón. Me lo comenta así Maite porque son datos calculados
                                        ' Sólo tiene sentido para los proyectos de Industrialización
                                        If (idPlanta <> 0 AndAlso (cabecera.TipoProyPtksis = ELL.CabeceraCostCarrier.TIPO_PROY_INDUSTRIALIZATION OrElse cabecera.TipoProyPtksis = ELL.CabeceraCostCarrier.TIPO_PROY_ECO_BATZ)) Then
                                        @<tr>
                                            <td colspan="5">
                                                @code
                                                    ' Estos datos son para el vista parcial _ValidacionAdicionalPlanta.vbhtml
                                                    viewDataDictonaryInfo = New ViewDataDictionary()
                                                    viewDataDictonaryInfo.Add("IdCabecera", cabecera.Id)
                                                    viewDataDictonaryInfo.Add("IdPlanta", idPlanta)
                                                    Dim beneficioHeramental As Decimal = Decimal.Zero
                                                                        If (PBCSumaplanta <> 0) Then
                                                        beneficioHeramental = CDec((PBCSumaplanta - BACGastosSumaPlanta) / PBCSumaplanta)
                                                    End If
                                                    viewDataDictonaryInfo.Add("BeneficioHerramental", beneficioHeramental)
                                                    viewDataDictonaryInfo.Add("SoloLectura", False)
                                                    viewDataDictonaryInfo.Add("CargarSoloSesion", True)
                                                end code
                                                @Html.Partial("~/Views/Shared/_ValidacionAdicionalPlanta.vbhtml", viewDataDictonaryInfo)                                                
                                            </td>
                                            @*<td>
                                                @Html.ActionLink("asdasdsadasdasdasdsadaa", "VerInfoAdicional", New With {.idCabecera = cabecera.Id, .idPlanta = idPlanta, .beneficioHerramental = beneficioHeramental, .soloLectura = True, .cargarSoloSesion = True})
                                            </td>*@
                                        </tr>
                                                    End If

                                    @<tr>
                                        <td colspan="5"></td>
                                    </tr>



                                end code
                            </tbody>

                                                    Next
                        @*Tenemos que incluir la plantas para las cuales no se haya enviado ningún paso a validar porque necesitamos los datos de additional info*@
                        @code
                            For Each idPlantaAux In cabecera.Plantas.Select(Function(f) f.IdPlanta).Except(listaIdsPlantasUnicos)
                                ' Si la planta es CORPORATIVO no se muestran el botón. Me lo comenta así Maite porque son datos calculados
                                ' Sólo tiene sentido para los proyectos de Industrialización
                                If (idPlantaAux <> 0 AndAlso (cabecera.TipoProyPtksis = ELL.CabeceraCostCarrier.TIPO_PROY_INDUSTRIALIZATION OrElse cabecera.TipoProyPtksis = ELL.CabeceraCostCarrier.TIPO_PROY_ECO_BATZ)) Then
                                    @<tr>
                                        <td Class="success" style="text-align:center;text-transform:uppercase" colspan="5">
                                            @code
                                                Dim planta = cabecera.Plantas.FirstOrDefault(Function(f) f.IdPlanta = idPlantaAux)
                                            End code

                                            <label>@String.Format("{0} ({1})", planta.Planta, planta.Moneda)</label>
                                            @*<button type="button" Class="btn btn-default btn-xs btn-success btn-infoadicional" title='@Utils.Traducir("Cambiar info adicional")' data-idplanta="@idPlanta" style="margin-left:10px;"><span class="glyphicon glyphicon-info-sign" aria-hidden="true"></span></button>*@
                                        </td>
                                    </tr>
                                    @<tr>
                                        <td colspan="5">
                                            @code
                                                ' Estos datos son para el vista parcial _ValidacionAdicionalPlanta.vbhtml
                                                viewDataDictonaryInfo = New ViewDataDictionary()
                                                viewDataDictonaryInfo.Add("IdCabecera", cabecera.Id)
                                                viewDataDictonaryInfo.Add("IdPlanta", idPlantaAux)
                                                Dim beneficioHeramental As Decimal = Decimal.Zero
                                                If (PBCSumaplanta <> 0) Then
                                                    beneficioHeramental = CDec((PBCSumaplanta - BACGastosSumaPlanta) / PBCSumaplanta)
                                                End If
                                                viewDataDictonaryInfo.Add("BeneficioHerramental", beneficioHeramental)
                                                viewDataDictonaryInfo.Add("SoloLectura", False)
                                                viewDataDictonaryInfo.Add("CargarSoloSesion", True)
                                            End Code
                                            @Html.Partial("~/Views/Shared/_ValidacionAdicionalPlanta.vbhtml", viewDataDictonaryInfo)
                                        </td>
                                         @*<td>
                                                @Html.ActionLink("Ver todo", "VerInfoAdicional", New With {.idCabecera = cabecera.Id, .idPlanta = idPlantaAux, .beneficioHerramental = beneficioHeramental, .soloLectura = True, .cargarSoloSesion = True}, New With {.target = "blank"})
                                            </td>*@
                                    </tr>
                                                    End If
                                                Next
                        End code
                    </table>


                </div>
                                                End If
        End code
    </div>

    @code
        If (Not steps.Exists(Function(f) f.EsInfoGeneral)) Then
            @<div Class="form-group">
                <label class="col-sm-2 col-form-label">@Utils.Traducir("Pasos a validar")</label>
                    <div Class="form-group">
                        <div Class="col-sm-6">
                            <Table id="tabla" Class="table table-bordered table-condensed">
                                <thead>
                                    <tr>
                                        <th Class="info" style="text-align:center;">@String.Format("{0} ({1})", Utils.Traducir("Nombre"), cabecera.Abreviatura)</th>
                                        <th Class="info" style="text-align:center;">@Utils.Traducir("Presupuesto aprobado")</th>
                                        <th Class="info" style="text-align:center;">@Utils.Traducir("Dinero pagado por cliente")</th>
                                        <th Class="info" style="text-align:center;">@Utils.Traducir("Margen") %</th>
                                        <th Class="info" style="text-align:center;">@Utils.Traducir("Empresa")</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    @For Each paso In steps.OrderBy(Function(f) f.IdPlanta)
                                        ' La longitud en XPERT es de 40 pero le quitamos uno más porque quieren meter un guión bajo entre la abreviatura y el nombre del paso
                                        Dim longitudMaxima As Integer = 40 - cabecera.Abreviatura.Length - 1
                                        Dim descripcion As String = If(paso.Descripcion.Length > longitudMaxima, paso.Descripcion.Substring(0, longitudMaxima), paso.Descripcion)
                                        paso.CargarValoresStep()

                                        @<tr>
                                            <td>@Html.TextBox(String.Format("txtNombre-{0}", paso.Id), descripcion, New With {.maxlength = longitudMaxima, .style = "width: 325px;", .required = "required", .class = "form-control"})</td>
                                            <td align="right">@paso.BACGastos.ToString("N0", culturaEsES)</td>
                                            <td align="right">@paso.PBC.ToString("N0", culturaEsES)</td>
                                            <td align="right">@paso.Margin</td>
                                            <td>@String.Format("{0} ({1})", paso.Planta, paso.Moneda)</td>
                                        </tr>
                                    Next
                                </tbody>
                            </Table>
                        </div>
                    </div>
            </div>
        Else
            @<div Class="form-group">
                <label class="col-sm-12 col-form-label text-danger text-uppercase text-center" style="font-size:larger">@Utils.Traducir("Sólo se enviará a validar la información general")</label>
            </div>
        End If
    End code

        End Using
@*@code
        ' Estos datos son para el vista parcial _ValidacionAdicional.vbhtml
        Dim viewDataDictonary As New ViewDataDictionary()
        viewDataDictonary.Add("IdCabecera", cabecera.Id)
        viewDataDictonary.Add("SoloLectura", False)
    End Code

    @Html.Partial("~/Views/Shared/_ValidacionAdicional.vbhtml", viewDataDictonary)*@