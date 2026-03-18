@Imports CostCarriersLib

@Code
    ' Cogemos de las validaciones la última
    Dim cabeceraProyecto As ELL.CabeceraCostCarrier = CType(ViewData("CabeceraProyecto"), ELL.CabeceraCostCarrier)
    Dim culturaEsES As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture("es-ES")
    Dim listaIdsPlantasUnicos As List(Of Integer) = CType(ViewData("IdsPlantasUnicos"), List(Of Integer))
End Code

<script type="text/javascript">

    $(function () {
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

<h3><label>@Utils.Traducir("Totales por proyecto") - @cabeceraProyecto.NombreProyecto</label></h3>
<hr />

@code
    Dim HorasSumaValidados As Integer = 0
    Dim BACGastosSumaValidados As Integer = 0
    Dim PBCSumaValidados As Integer = 0
    'Dim realSumaValidados As Integer = 0
    Dim HorasSumaPlanta As Integer = 0
    Dim BACGastosSumaPlanta As Integer = 0
    Dim PBCSumaPlanta As Integer = 0
    'Dim realSumaPlanta As Integer = 0
    Dim marginSumaPlanta As String = String.Empty
    Dim marginSuma As String = String.Empty
    Dim BACGastosSumaPlantaVentas As Integer = 0
    Dim PBCSumaPlantaVentas As Integer = 0
    Dim marginSumaPlantaVentas As String = String.Empty
    Dim listaCostGroups As List(Of ELL.CostGroup) = Nothing
    Dim validacionLineaValidada As ELL.ValidacionLinea = Nothing
    Dim usuariosRol As List(Of ELL.UsuarioRol) = CType(Session("RolesUsuario"), List(Of ELL.UsuarioRol))


    @Html.ActionLink(Utils.Traducir("Validaciones pendientes"), "Index", "Validaciones")
    @<br />
    @<br />

    If (listaIdsPlantasUnicos.Count > 0) Then
        @For Each idPlanta In listaIdsPlantasUnicos
            ' Quito esta condicion despues de mail de Maite del 18/05/2020 a las 10:23
            'If (usuariosRol.Select(Function(f) f.IdPlanta).Contains(idPlanta) OrElse idPlanta = 0) Then
            HorasSumaPlanta = 0
            BACGastosSumaPlanta = 0
            PBCSumaPlanta = 0
            'realSumaPlanta = 0
            BACGastosSumaPlantaVentas = 0
            PBCSumaPlantaVentas = 0
            marginSumaPlanta = String.Empty
            marginSumaPlantaVentas = String.Empty
            @<form class="form-horizontal">
                <Table id="tabla" Class="table table-bordered table-condensed">
                    <thead>
                        <tr>
                            <th Class="success" style="text-align:center;text-transform:uppercase" colspan="5">
                                @BLL.PlantasBLL.CargarListadoPorIdPlantaSAB(idPlanta).FirstOrDefault().Planta
                                @code
' Si la planta es CORPORATIVO no se muestran el botón. Me lo comenta así Maite porque son datos caldula
' Sólo tiene sentido para los proyectos de Industrialización
                                    @*If (idPlanta <> 0 AndAlso cabeceraProyecto.IdTipoProyecto = ELL.TipoProyecto.TipoProyecto.Industrialization) Then
                                        @<button type="button" Class="btn btn-default btn-xs btn-success btn-infoadicional" title='@Utils.Traducir("Info adicional")' data-idplanta="@idPlanta"><span class="glyphicon glyphicon-info-sign" aria-hidden="true"></span></button>
                                    End If*@
                                End code
                            </th>
                        </tr>
                        <tr>
                            <th></th>
                            <th Class="danger" style="text-align:center;">@Utils.Traducir("Horas")</th>
                            <th Class="danger" style="text-align:center;">@Utils.Traducir("Coste")</th>
                            <th Class="info" style="text-align:center;">@Utils.Traducir("Dinero pagado por cliente")</th>
                            <th Class="info" style="text-align:center;">@Utils.Traducir("Margen") %</th>
                            @*<th Class="info" style="text-align:center;">@Utils.Traducir("Dato real")</th>*@
                        </tr>
                    </thead>
                    <tbody>
                        @code
                            ' Vamos a obtener todos los costgroup de este proyecto para esta planta
                            listaCostGroups = BLL.CostsGroupBLL.CargarListadoPorCabeceraPlanta(cabeceraProyecto.Id, idPlanta)
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
                                HorasSumaValidados = 0
                                BACGastosSumaValidados = 0
                                PBCSumaValidados = 0
                                'realSumaValidados = 0

                                ' Cargamos las validaciones linea validados
                                validacionLineaValidada = BLL.ValidacionesLineaBLL.ObtenerValidadoByCostGroup(costGroup.Id)

                                If (validacionLineaValidada IsNot Nothing) Then

                                    HorasSumaValidados = validacionLineaValidada.Hours
                                    BACGastosSumaValidados = validacionLineaValidada.BudgetApproved
                                    PBCSumaValidados = validacionLineaValidada.PaidByCustomer
                                    HorasSumaPlanta += validacionLineaValidada.Hours
                                    BACGastosSumaPlanta += validacionLineaValidada.BudgetApproved
                                    PBCSumaPlanta += validacionLineaValidada.PaidByCustomer

                                    Dim validacionesLinea As List(Of ELL.ValidacionLinea) = BLL.ValidacionesLineaBLL.CargarListadoValidadosByCostGroup(costGroup.Id)

                                    ' Ahora está cogiendo todas las validaciones línea pero sólo tendría que coger las validadas.
                                    ' NO DEBERÍA SER ASÍ. SÓLO DEBERÍA COGER LAS APROBADAS

                                    For Each validacionLinea In validacionesLinea
                                        If (costGroup.IdAgrupacion = ELL.CostGroupOT.Agrupacion.Serial_tooling) Then
                                            If (validacionLinea.PaidByCustomer = 0) Then
                                                nombreCostGroup = CType(ELL.CostGroupOT.Agrupacion.Serial_tooling, ELL.CostGroupOT.Agrupacion).ToString().ToUpper().Replace("_", " ") & " BATZ" & " (" & costGroup.Estado & ")"
                                            ElseIf (validacionLinea.PaidByCustomer > 0) Then
                                                nombreCostGroup = CType(ELL.CostGroupOT.Agrupacion.Serial_tooling, ELL.CostGroupOT.Agrupacion).ToString().ToUpper().Replace("_", " ") & " CUSTOMER" & " (" & costGroup.Estado & ")"
                                            End If

                                            If (Not listaCostGroupsParaValidacion.Exists(Function(f) f.Nombre = nombreCostGroup)) Then
                                                costGroupsParaValidacion = New ELL.CostGroupParaValidacion With {.Nombre = nombreCostGroup}
                                                listaCostGroupsParaValidacion.Add(costGroupsParaValidacion)
                                            Else
                                                costGroupsParaValidacion = listaCostGroupsParaValidacion.FirstOrDefault(Function(f) f.Nombre = nombreCostGroup)
                                            End If

                                            costGroupsParaValidacion.HorasSumaValidados += validacionLinea.Hours
                                            costGroupsParaValidacion.BACGastosSumaValidados += validacionLinea.BudgetApproved
                                            costGroupsParaValidacion.PBCSumaValidados += validacionLinea.PaidByCustomer
                                            'costGroupsParaValidacion.RealSumaValidados += validacionLinea.RealDataInt_Ext
                                            'realSumaValidados = validacionLinea.RealDataInt_Ext
                                            'realSumaPlanta += validacionLinea.RealDataInt_Ext
                                        Else
                                            'realSumaValidados = validacionLinea.RealDataInt_Ext
                                            'realSumaPlanta += validacionLinea.RealDataInt_Ext
                                        End If
                                    Next

                                    'If (validacionLineaValidada.PaidByCustomer = 0) Then
                                    '    nombreCostGroup = CType(ELL.CostGroupOT.Agrupacion.Serial_tooling, ELL.CostGroupOT.Agrupacion).ToString().ToUpper().Replace("_", " ") & " BATZ" & " (" & costGroup.Estado & ")"
                                    'ElseIf (validacionLineaValidada.PaidByCustomer > 0) Then
                                    '    nombreCostGroup = CType(ELL.CostGroupOT.Agrupacion.Serial_tooling, ELL.CostGroupOT.Agrupacion).ToString().ToUpper().Replace("_", " ") & " CUSTOMER" & " (" & costGroup.Estado & ")"
                                    'End If
                                    'costGroupsParaValidacion = listaCostGroupsParaValidacion.FirstOrDefault(Function(f) f.Nombre = nombreCostGroup)
                                    'costGroupsParaValidacion.HorasSumaValidados = HorasSumaValidados
                                    'costGroupsParaValidacion.BACGastosSumaValidados = BACGastosSumaValidados
                                    'costGroupsParaValidacion.PBCSumaValidados = PBCSumaValidados
                                    'costGroupsParaValidacion.RealSumaValidados = realSumaValidados

                                End If

                                If (PBCSumaValidados = 0) Then
                                    marginSuma = "NA"
                                Else
                                    marginSuma = (((PBCSumaValidados - BACGastosSumaValidados) / PBCSumaValidados) * 100).ToString("N2", culturaEsES)
                                    BACGastosSumaPlantaVentas += BACGastosSumaValidados
                                    PBCSumaPlantaVentas += PBCSumaValidados
                                End If

                                If (costGroup.IdAgrupacion = Integer.MinValue OrElse costGroup.IdAgrupacion <> ELL.CostGroupOT.Agrupacion.Serial_tooling) Then
                                    @<tr>
                                        <td class="success">
                                            @String.Format("{0} ({1})", costGroup.Descripcion, costGroup.Estado)
                                        </td>
                                        @code
                                            @<td align="right">@HorasSumaValidados.ToString("N0", culturaEsES)</td>
                                            @<td align="right">@BACGastosSumaValidados.ToString("N0", culturaEsES)</td>
                                            @<td align="right">@PBCSumaValidados.ToString("N0", culturaEsES)</td>
                                        End code
                                        <td align="right">@marginSuma</td>
                                        @*<td align="right">@realSumaValidados.ToString("N0", culturaEsES)</td>*@
                                    </tr>
                                                End If
                                            Next
                                            For Each costGroupsParaValidacion In listaCostGroupsParaValidacion
                                                If (costGroupsParaValidacion.PBCSumaValidados = 0) Then
                                                    marginSuma = "NA"
                                                Else
                                                    marginSuma = (((costGroupsParaValidacion.PBCSumaValidados - costGroupsParaValidacion.BACGastosSumaValidados) / costGroupsParaValidacion.PBCSumaValidados) * 100).ToString("N2", culturaEsES)
                                                End If

                                @<tr>
                                    <td class="success">
                                        @costGroupsParaValidacion.Nombre
                                    </td>
                                    @code
                                        @<td align="right">@costGroupsParaValidacion.HorasSumaValidados.ToString("N0", culturaEsES)</td>
                                        @<td align="right">@costGroupsParaValidacion.BACGastosSumaValidados.ToString("N0", culturaEsES)</td>
                                        @<td align="right">@costGroupsParaValidacion.PBCSumaValidados.ToString("N0", culturaEsES)</td>
                                    End code
                                    <td align="right">@marginSuma</td>
                                    @*<td align="right">@costGroupsParaValidacion.RealSumaValidados.ToString("N0", culturaEsES)</td>*@
                                </tr>
                                        Next

                            @code
                                If (PBCSumaPlanta = 0) Then
                                    marginSumaPlanta = "NA"
                                Else
                                    marginSumaPlanta = (((PBCSumaPlanta - BACGastosSumaPlanta) / PBCSumaPlanta) * 100).ToString("N2", culturaEsES)
                                End If
                            End code

                            @<tr>
                                <td class="success">
                                    <b>
                                        @Utils.Traducir("Total").ToUpper()
                                    </b>
                                </td>
                                <td align="right"><b>@HorasSumaPlanta.ToString("N0", culturaEsES)</b></td>
                                <td align="right"><b>@BACGastosSumaPlanta.ToString("N0", culturaEsES)</b></td>
                                <td align="right"><b>@PBCSumaPlanta.ToString("N0", culturaEsES)</b></td>
                                <td align="right"><b>@marginSumaPlanta</b></td>
                                @*<td align="right"><b>@realSumaPlanta.ToString("N0", culturaEsES)</b></td>*@
                            </tr>

                            @code
                                If (PBCSumaPlantaVentas = 0) Then
                                    marginSumaPlantaVentas = "NA"
                                Else
                                    marginSumaPlantaVentas = (((PBCSumaPlantaVentas - BACGastosSumaPlantaVentas) / PBCSumaPlantaVentas) * 100).ToString("N2", culturaEsES)
                                End If
                            End code

                            @<tr>
                                <td class="success">
                                    <b>
                                        @Utils.Traducir("Total (sólo ventas)").ToUpper()
                                    </b>
                                </td>
                                <td></td>
                                <td align="right"><b>@BACGastosSumaPlantaVentas.ToString("N0", culturaEsES)</b></td>
                                <td align="right"><b>@PBCSumaPlantaVentas.ToString("N0", culturaEsES)</b></td>
                                <td align="right"><b>@marginSumaPlantaVentas</b></td>
                                @*<td></td>*@
                            </tr>


                                ' Si la planta es CORPORATIVO no se muestran el botón. Me lo comenta así Maite porque son datos calculados
                                ' Sólo tiene sentido para los proyectos de Industrialización
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


                        end code
                    </tbody>
                </Table>
            </form>
                                                'End if
                                            Next
    Else
        @Html.Label(String.Empty, Utils.Traducir("noExisteNingunRegistro"))
                                                    End If
End code

@*@code
    ' Estos datos son para el vista parcial _ValidacionAdicion.vbhtml
    Dim viewDataDictonary As New ViewDataDictionary()
    viewDataDictonary.Add("IdCabecera", cabeceraProyecto.Id)
    viewDataDictonary.Add("SoloLectura", True)
End Code

@Html.Partial("~/Views/Shared/_ValidacionAdicional.vbhtml", viewDataDictonary)*@