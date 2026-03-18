@Imports CostCarriersLib

@code
    Dim idCabecera As Integer = CInt(ViewData("IdCabecera"))
    Dim cabecera As ELL.CabeceraCostCarrier = BLL.CabecerasCostCarrierBLL.Obtener(idCabecera, False)
    Dim idPlanta As Integer = CInt(ViewData("IdPlanta"))
    Dim beneficioHerramental As Decimal = CInt(ViewData("BeneficioHerramental"))
    Dim cargarSoloSesion As Boolean = False
    If (ViewData("CargarSoloSesion") IsNot Nothing) Then
        cargarSoloSesion = CBool(ViewData("CargarSoloSesion"))
    End If

    Dim soloLectura As Boolean = True
    If (ViewData("SoloLectura") IsNot Nothing) Then
        soloLectura = CBool(ViewData("SoloLectura"))
    End If

    ' Vamos a cargar todas las aditional info de tipo current
    Dim listaAdicInfoAux As List(Of ELL.ValidacionInfoAdicional) = BLL.ValidacionesInfoAdicionalBLL.CargarListado(idCabecera, idPlanta).Where(Function(f) f.Tipo = ELL.ValidacionInfoAdicional.TipoDato.Current_values).OrderBy(Function(f) f.FechaAlta).ToList()
    Dim listaAdicInfo As New List(Of ELL.ValidacionInfoAdicional)

    'De la lista listaAdicInfo eliminamos aquellas es las que haya algún paso rechazado
    ' Tenemos que ir a cada validación porque sacar el estado del step no nos vale.
    Dim validacionesLinea As List(Of ELL.ValidacionLinea)
    Dim historicoEstados As List(Of ELL.HistoricoEstadoLinea)
    Dim rejected As Boolean = False
    For Each adicInfo In listaAdicInfoAux
        rejected = False
        validacionesLinea = BLL.ValidacionesLineaBLL.CargarListadoPorValidacion(adicInfo.IdValidacion)

        ' Por cada validación linea tenemos que ir a sus valores en el historico para ver si se ha rechazado
        For Each validacionLinea In validacionesLinea
            historicoEstados = BLL.HistoricosEstadoLineaBLL.CargarListadoPorValidacionLinea(validacionLinea.Id)

            If (historicoEstados.Exists(Function(f) f.IdEstadoValidacion = ELL.Validacion.Estado.Rejected)) Then
                rejected = True
                Exit For
            End If
        Next

        If (Not rejected) Then
            listaAdicInfo.Add(adicInfo)
        End If
    Next

    ' Si es de sólo lectura indica que estamos visualizando los datos por lo tanto tenemos que quitar la columna de validación más reciente porque coincide con la actual
    ' Como la lista esta ordenada por fecha de alta ascendente eliminamos el último elemento
    If (soloLectura AndAlso listaAdicInfo IsNot Nothing AndAlso listaAdicInfo.Count > 0) Then
        listaAdicInfo.RemoveAt(listaAdicInfo.Count - 1)
    End If

    If (listaAdicInfo IsNot Nothing AndAlso listaAdicInfo.Count > 0) Then
        listaAdicInfoAux = listaAdicInfo.ToList()
        'Quitamos la validación más reciente
        listaAdicInfoAux.RemoveAt(listaAdicInfoAux.Count - 1)

        'Cogemos sólo la columna de validación de datos más recientes
        Dim adicInfo = listaAdicInfo.Last
        listaAdicInfo = New List(Of ELL.ValidacionInfoAdicional)
        listaAdicInfo.Add(adicInfo)
    End If

    Dim planta As ELL.Planta = BLL.PlantasBLL.CargarListadoPorIdPlantaSAB(idPlanta).FirstOrDefault(Function(f) f.IdCabecera = idCabecera)

    Dim culturaEsES As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture("es-ES")

    Dim etiquetaNetMagin As String = Utils.Traducir("Margen neto")
    Dim etiquetaEffectiveSales As String = Utils.Traducir("Ventas efectivas") & " (" & Utils.Traducir("total proyecto") & ")"
    If (cabecera.TipoProyPtksis = ELL.CabeceraCostCarrier.TIPO_PROY_ECO_BATZ) Then
        etiquetaNetMagin = "▲ " & Utils.Traducir("Beneficio precio pieza")
        etiquetaEffectiveSales = "▲ " & Utils.Traducir("Ventas efectivas") & " (" & Utils.Traducir("total proyecto") & ")"
    End If
End Code

<script src="~/Scripts/jquery.alphanum.js"></script>

<script type="text/javascript">
    $(function () {
        if ("@soloLectura.ToString().ToLower()" == "true") {
            $("#@String.Format("txtMargenNetoAO-{0}-{1}", idCabecera, idPlanta)").attr("readonly", "readonly");
            $("#@String.Format("txtMargenNeto-{0}-{1}", idCabecera, idPlanta)").attr("readonly", "readonly");
            $("#@String.Format("txtVentasEfectivasAO-{0}-{1}", idCabecera, idPlanta)").attr("readonly", "readonly");
            $("#@String.Format("txtVentasEfectivas-{0}-{1}", idCabecera, idPlanta)").attr("readonly", "readonly");
            $("#@String.Format("txtBeneficioHerramentalAO-{0}-{1}", idCabecera, idPlanta)").attr("readonly", "readonly");
            $("#@String.Format("txtBeneficioHerramental-{0}-{1}", idCabecera, idPlanta)").attr("readonly", "readonly");
            $("#@String.Format("txtPlantasClienteAO-{0}-{1}", idCabecera, idPlanta)").attr("readonly", "readonly");
            $("#@String.Format("txtPlantasCliente-{0}-{1}", idCabecera, idPlanta)").attr("readonly", "readonly");
            $("#@String.Format("txtSOPAO-{0}-{1}", idCabecera, idPlanta)").attr("readonly", "readonly");
            $("#@String.Format("txtSOP-{0}-{1}", idCabecera, idPlanta)").attr("readonly", "readonly");
            $("#@String.Format("txtVolumenMedioAO-{0}-{1}", idCabecera, idPlanta)").attr("readonly", "readonly");
            $("#@String.Format("txtVolumenMedio-{0}-{1}", idCabecera, idPlanta)").attr("readonly", "readonly");
            $("#@String.Format("txtAnyosSerieAO-{0}-{1}", idCabecera, idPlanta)").attr("readonly", "readonly");
            $("#@String.Format("txtAnyosSerie-{0}-{1}", idCabecera, idPlanta)").attr("readonly", "readonly");
        }
        else {
            $("#@String.Format("txtMargenNetoAO-{0}-{1}", idCabecera, idPlanta)").removeAttr("readonly");
            $("#@String.Format("txtMargenNeto-{0}-{1}", idCabecera, idPlanta)").removeAttr("readonly");
            $("#@String.Format("txtVentasEfectivasAO-{0}-{1}", idCabecera, idPlanta)").removeAttr("readonly");
            $("#@String.Format("txtVentasEfectivas-{0}-{1}", idCabecera, idPlanta)").removeAttr("readonly");
            $("#@String.Format("txtBeneficioHerramentalAO-{0}-{1}", idCabecera, idPlanta)").removeAttr("readonly");
            $("#@String.Format("txtBeneficioHerramental-{0}-{1}", idCabecera, idPlanta)").removeAttr("readonly");
            $("#@String.Format("txtPlantasClienteAO-{0}-{1}", idCabecera, idPlanta)").removeAttr("readonly");
            $("#@String.Format("txtPlantasCliente-{0}-{1}", idCabecera, idPlanta)").removeAttr("readonly");
            $("#@String.Format("txtSOPAO-{0}-{1}", idCabecera, idPlanta)").removeAttr("readonly");
            $("#@String.Format("txtSOP-{0}-{1}", idCabecera, idPlanta)").removeAttr("readonly");
            $("#@String.Format("txtVolumenMedioAO-{0}-{1}", idCabecera, idPlanta)").removeAttr("readonly");
            $("#@String.Format("txtVolumenMedio-{0}-{1}", idCabecera, idPlanta)").removeAttr("readonly");
            $("#@String.Format("txtAnyosSerieAO-{0}-{1}", idCabecera, idPlanta)").removeAttr("readonly");
            $("#@String.Format("txtAnyosSerie-{0}-{1}", idCabecera, idPlanta)").removeAttr("readonly");
        }

        $.fn.alphanum.setNumericSeparators({
            thousandsSeparator: ".",
            decimalSeparator: ","
        });

        $("#@String.Format("txtVentasEfectivasAO-{0}-{1}", idCabecera, idPlanta), " +
            "#@String.Format("txtVolumenMedioAO-{0}-{1}", idCabecera, idPlanta), " +
            "#@String.Format("txtAnyosSerieAO-{0}-{1}", idCabecera, idPlanta), "+
            "#@String.Format("txtVentasEfectivas-{0}-{1}", idCabecera, idPlanta), " +
            "#@String.Format("txtVolumenMedio-{0}-{1}", idCabecera, idPlanta), " +
            "#@String.Format("txtAnyosSerie-{0}-{1}", idCabecera, idPlanta)").numeric({
            allowThouSep: false,
            allowDecSep: false
        });

        $("#@String.Format("txtMargenNetoAO-{0}-{1}", idCabecera, idPlanta), " +
            "#@String.Format("txtMargenNeto-{0}-{1}", idCabecera, idPlanta), " +
            "#@String.Format("txtBeneficioHerramentalAO-{0}-{1}", idCabecera, idPlanta), " +
            "#@String.Format("txtBeneficioHerramental-{0}-{1}", idCabecera, idPlanta)").numeric({
            allowMinus: true,
            allowThouSep: false,
            maxDecimalPlaces: 3
        });

        $("#@String.Format("SOP-{0}-{1}", idCabecera, idPlanta), #@String.Format("SOPAO-{0}-{1}", idCabecera, idPlanta)").datetimepicker({
            showClear: true,
            locale: '@Threading.Thread.CurrentThread.CurrentCulture.Name',
            format: '@Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper'
        });

        $(document).ready(function () {
            var denominacion = $("#denominacion").val();
            var descripcion = $("#descripcion").val();
            var previstoPg = $('#previstoPg').val();

            $.ajax({
                url: '@Url.Action("GuardarDatosValidacion", "CostCarriers")',
                data: { denominacion: denominacion, descripcion: descripcion, previstoPg: previstoPg },
                type: 'GET',
                dataType: 'json'
            });

            $.ajax({
                url: '@Url.Action("CargarDatosValidacionInfoAdicional", "CostCarriers")',
                data: { idCabecera: @idCabecera, idPlanta: @idPlanta, idTipo: @CInt(ELL.ValidacionInfoAdicional.TipoDato.Awarding_offer)},
                type: 'GET',
                dataType: 'json',
                success: function (d) {
                    $("#@String.Format("txtMargenNetoAO-{0}-{1}", idCabecera, idPlanta)").val('');
                    $("#@String.Format("txtVentasEfectivasAO-{0}-{1}", idCabecera, idPlanta)").val('');
                    $("#@String.Format("txtBeneficioHerramentalAO-{0}-{1}", idCabecera, idPlanta)").val('');
                    $("#@String.Format("txtPlantasClienteAO-{0}-{1}", idCabecera, idPlanta)").val('');
                    $("#@String.Format("txtSOPAO-{0}-{1}", idCabecera, idPlanta)").val('');
                    $("#@String.Format("txtVolumenMedioAO-{0}-{1}", idCabecera, idPlanta)").val('');
                    $("#@String.Format("txtAnyosSerieAO-{0}-{1}", idCabecera, idPlanta)").val('');
                    if (d) {
                        if (d.NetMargin != @Integer.MinValue) {
                            $("#@String.Format("txtMargenNetoAO-{0}-{1}", idCabecera, idPlanta)").val(d.NetMargin.toLocaleString('es-ES'));
                        }
                        if (d.EffectiveSales != @Integer.MinValue) {
                            $("#@String.Format("txtVentasEfectivasAO-{0}-{1}", idCabecera, idPlanta)").val(d.EffectiveSales.toLocaleString('es-ES'));
                        }
                        if (d.CustomerProperty != @Integer.MinValue) {
                            $("#@String.Format("txtBeneficioHerramentalAO-{0}-{1}", idCabecera, idPlanta)").val(d.CustomerProperty.toLocaleString('es-ES'));
                        }
                        if (d.CustomerPlants != '') {
                            $("#@String.Format("txtPlantasClienteAO-{0}-{1}", idCabecera, idPlanta)").val(d.CustomerPlants);
                        }
                        if (d.SOP != '/Date(-62135596800000)/') {
                            $("#@String.Format("txtSOPAO-{0}-{1}", idCabecera, idPlanta)").val(new Date(parseInt(d.SOP.substr(6))).toLocaleDateString("@Threading.Thread.CurrentThread.CurrentCulture.Name"));
                        }
                        if (d.AverageVolumen != @Integer.MinValue) {
                            $("#@String.Format("txtVolumenMedioAO-{0}-{1}", idCabecera, idPlanta)").val(d.AverageVolumen.toLocaleString('es-ES'));
                        }
                        if (d.SeriesYears != @Integer.MinValue) {
                            $("#@String.Format("txtAnyosSerieAO-{0}-{1}", idCabecera, idPlanta)").val(d.SeriesYears.toLocaleString('es-ES'));
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
                data: { idCabecera: @idCabecera, idPlanta: @idPlanta, idTipo:  @CInt(ELL.ValidacionInfoAdicional.TipoDato.Current_values), cargarSoloDeSesion: '@cargarSoloSesion' },
                type: 'GET',
                dataType: 'json',
                success: function (d) {
                    $("#@String.Format("txtMargenNeto-{0}-{1}", idCabecera, idPlanta)").val('');
                    $("#@String.Format("txtVentasEfectivas-{0}-{1}", idCabecera, idPlanta)").val('');
                    @*$("#@String.Format("txtBeneficioHerramental-{0}-{1}", idCabecera, idPlanta)").val('@beneficioHerramental.ToString("N3", culturaEsES)');*@
                    $("#@String.Format("txtBeneficioHerramental-{0}-{1}", idCabecera, idPlanta)").val('');
                    $("#@String.Format("txtPlantasCliente-{0}-{1}", idCabecera, idPlanta)").val('');
                    $("#@String.Format("txtSOP-{0}-{1}", idCabecera, idPlanta)").val('');
                    $("#@String.Format("txtVolumenMedio-{0}-{1}", idCabecera, idPlanta)").val('');
                    $("#@String.Format("txtAnyosSerie-{0}-{1}", idCabecera, idPlanta)").val('');
                    $("#@String.Format("lblCurrent-{0}-{1}", idCabecera, idPlanta)").text('@Utils.Traducir("Actual")');
                    if (d) {
                        $("#@String.Format("lblCurrent-{0}-{1}", idCabecera, idPlanta)").text('@Utils.Traducir("Actual")' + ' (' + new Date(parseInt(d.FechaAlta.substr(6))).toLocaleDateString("@Threading.Thread.CurrentThread.CurrentCulture.Name") + ')');

                        if (d.NetMargin != @Integer.MinValue) {
                            $("#@String.Format("txtMargenNeto-{0}-{1}", idCabecera, idPlanta)").val(d.NetMargin.toLocaleString('es-ES'));
                        }
                        if (d.EffectiveSales != @Integer.MinValue) {
                            $("#@String.Format("txtVentasEfectivas-{0}-{1}", idCabecera, idPlanta)").val(d.EffectiveSales.toLocaleString('es-ES'));
                        }
                        if (d.CustomerProperty != @Integer.MinValue) {
                            $("#@String.Format("txtBeneficioHerramental-{0}-{1}", idCabecera, idPlanta)").val(d.CustomerProperty.toLocaleString('es-ES'));
                        }
                        if (d.CustomerPlants != '') {
                            $("#@String.Format("txtPlantasCliente-{0}-{1}", idCabecera, idPlanta)").val(d.CustomerPlants);
                        }
                        if (d.SOP != '/Date(-62135596800000)/') {
                            $("#@String.Format("txtSOP-{0}-{1}", idCabecera, idPlanta)").val(new Date(parseInt(d.SOP.substr(6))).toLocaleDateString("@Threading.Thread.CurrentThread.CurrentCulture.Name"));
                        }
                        if (d.AverageVolumen != @Integer.MinValue) {
                            $("#@String.Format("txtVolumenMedio-{0}-{1}", idCabecera, idPlanta)").val(d.AverageVolumen.toLocaleString('es-ES'));
                        }
                        if (d.SeriesYears != @Integer.MinValue) {
                            $("#@String.Format("txtAnyosSerie-{0}-{1}", idCabecera, idPlanta)").val(d.SeriesYears.toLocaleString('es-ES'));
                        }
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                }
            });

            $('#@String.Format("masvalidaciones-{0}", idPlanta)').click(function () {
                $('#@String.Format("modalWindowMasValidaciones-{0}", idPlanta)').modal('show');
            });
        })
    })
</script>

<table id="tabla" Class="table table-bordered table-condensed">
    <thead>
        <tr>
            @*<th Class="info"><label>@String.Format("{0} ({1})", Utils.Traducir("Información general"), planta.Moneda)</label></th>*@
            <th Class="info"><label>@Utils.Traducir("Información general")</label></th>
            <th Class="info" style="text-align:center"><label id="@String.Format("lblCurrent-{0}-{1}", idCabecera, idPlanta)"></label></th>
            <th Class="info" style="text-align:center"><label>@Utils.Traducir("Oferta de adjudicación")</label></th>
            @code
                For Each adicInfo In listaAdicInfo
                    @<th Class="info" style="text-align:center"><label>@String.Format("{0} ({1})", Utils.Traducir("Validacion"), adicInfo.FechaAlta.ToShortDateString())</label></th>
                Next
            End code
        </tr>
    </thead>
    <tbody>
        <tr>
            <td class="success"><label>@etiquetaNetMagin %</label></td>
            <td>@Html.TextBox(String.Format("txtMargenNeto-{0}-{1}", idCabecera, idPlanta), String.Empty, New With {.class = "form-control numeric input-text", .required = "required", .readonly = "", .style = "font-weight: bold;"})</td>
            <td>@Html.TextBox(String.Format("txtMargenNetoAO-{0}-{1}", idCabecera, idPlanta), String.Empty, New With {.class = "form-control numeric", .required = "required", .readonly = "", .style = "font-weight: bold;"})</td>
            @code
                For Each adicInfo In listaAdicInfo
                    @<td>@Html.TextBox(String.Format("txtMargenNetoPV-{0}", adicInfo.Id), adicInfo.NetMargin.ToString("N3", culturaEsES), New With {.class = "form-control numeric input-text", .readonly = "readonly", .style = "font-weight: bold;"})</td>
                Next
            End code
        </tr>
        <tr>
            <td class="success"><label>@etiquetaEffectiveSales</label></td>
            <td>@Html.TextBox(String.Format("txtVentasEfectivas-{0}-{1}", idCabecera, idPlanta), String.Empty, New With {.class = "form-control numeric input-text", .required = "required", .readonly = "", .style = "font-weight: bold;"})</td>
            <td>@Html.TextBox(String.Format("txtVentasEfectivasAO-{0}-{1}", idCabecera, idPlanta), String.Empty, New With {.class = "form-control numeric input-text", .required = "required", .readonly = "", .style = "font-weight: bold;"})</td>
            @code
                For Each adicInfo In listaAdicInfo
                    @<td>@Html.TextBox(String.Format("txtVentasEfectivasPV-{0}", adicInfo.Id), adicInfo.EffectiveSales.ToString("N0", culturaEsES), New With {.class = "form-control numeric input-text", .readonly = "readonly", .style = "font-weight: bold;"})</td>
                Next
            End code
        </tr>
        <tr>
            <td class="success"><label>@Utils.Traducir("Beneficio herramental propiedad cliente") %</label></td>
            <td>@Html.TextBox(String.Format("txtBeneficioHerramental-{0}-{1}", idCabecera, idPlanta), String.Empty, New With {.class = "form-control numeric input-text", .required = "required", .readonly = "", .style = "font-weight: bold;"})</td>
            <td>@Html.TextBox(String.Format("txtBeneficioHerramentalAO-{0}-{1}", idCabecera, idPlanta), String.Empty, New With {.class = "form-control numeric input-text", .required = "required", .readonly = "", .style = "font-weight: bold;"})</td>
            @code
                For Each adicInfo In listaAdicInfo
                    @<td>@Html.TextBox(String.Format("txtBeneficioHerramentalPV-{0}", adicInfo.Id), adicInfo.CustomerProperty.ToString("N3", culturaEsES), New With {.class = "form-control numeric input-text", .readonly = "readonly", .style = "font-weight: bold;"})</td>
                Next
            End code
        </tr>
        @code
            If (cabecera.TipoProyPtksis <> ELL.CabeceraCostCarrier.TIPO_PROY_ECO_BATZ) Then
                @<tr>
                    <td Class="success"><label>@Utils.Traducir("Plantas del cliente")</label></td>
                    <td>@Html.TextBox(String.Format("txtPlantasCliente-{0}-{1}", idCabecera, idPlanta), String.Empty, New With {.class = "form-control", .maxLength = 100, .required = "required", .readonly = "", .style = "font-weight: bold;"})</td>
                    <td>@Html.TextBox(String.Format("txtPlantasClienteAO-{0}-{1}", idCabecera, idPlanta), String.Empty, New With {.class = "form-control", .maxLength = 100, .required = "required", .readonly = "", .style = "font-weight: bold;"})</td>
                    @code
                        For Each adicInfo In listaAdicInfo
                            @<td>@Html.TextBox(String.Format("txtPlantasClientePV-{0}", adicInfo.Id), adicInfo.CustomerPlants, New With {.class = "form-control", .maxLength = 100, .readonly = "readonly", .style = "font-weight: bold;"})</td>
                        Next
                    End code
                </tr>
            End if
        end code
        <tr>
            <td class="success"><label>@Utils.Traducir("SOP")</label></td>
            <td>
                <div class="input-group date" id="@String.Format("SOP-{0}-{1}", idCabecera, idPlanta)">
                    @Html.TextBox(String.Format("txtSOP-{0}-{1}", idCabecera, idPlanta), String.Empty, New With {.class = "form-control", .required = "required", .readonly = "", .style = "font-weight: bold;"})
                    <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                </div>
            </td>
            <td>
                <div class="input-group date" id="@String.Format("SOPAO-{0}-{1}", idCabecera, idPlanta)">
                    @Html.TextBox(String.Format("txtSOPAO-{0}-{1}", idCabecera, idPlanta), String.Empty, New With {.class = "form-control", .required = "required", .readonly = "", .style = "font-weight: bold;"})
                    <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                </div>
            </td>
            @code
                For Each adicInfo In listaAdicInfo
                    @<td>@Html.TextBox(String.Format("txtSOPPV-{0}", adicInfo.Id), adicInfo.SOP.ToShortDateString(), New With {.class = "form-control", .readonly = "readonly", .style = "font-weight: bold;"})</td>
                Next
            End code
        </tr>
        <tr>
            <td class="success"><label>@Utils.Traducir("Volumen medio anual")</label></td>
            <td>@Html.TextBox(String.Format("txtVolumenMedio-{0}-{1}", idCabecera, idPlanta), String.Empty, New With {.class = "form-control numeric input-text", .required = "required", .readonly = "", .style = "font-weight: bold;"})</td>
            <td>@Html.TextBox(String.Format("txtVolumenMedioAO-{0}-{1}", idCabecera, idPlanta), String.Empty, New With {.class = "form-control numeric input-text", .required = "required", .readonly = "", .style = "font-weight: bold;"})</td>
            @code
                For Each adicInfo In listaAdicInfo
                    @<td>@Html.TextBox(String.Format("txtVolumenMedioPV-{0}", adicInfo.Id), adicInfo.AverageVolumen.ToString("N0", culturaEsES), New With {.class = "form-control numeric input-text", .readonly = "readonly", .style = "font-weight: bold;"})</td>
                Next
            End code
        </tr>
        <tr>
            <td class="success"><label>@Utils.Traducir("Años serie")</label></td>
            <td>@Html.TextBox(String.Format("txtAnyosSerie-{0}-{1}", idCabecera, idPlanta), String.Empty, New With {.class = "form-control numeric input-text", .required = "required", .readonly = "", .style = "font-weight: bold;"})</td>
            <td>@Html.TextBox(String.Format("txtAnyosSerieAO-{0}-{1}", idCabecera, idPlanta), String.Empty, New With {.class = "form-control numeric input-text", .required = "required", .readonly = "", .style = "font-weight: bold;"})</td>
            @code
                For Each adicInfo In listaAdicInfo
                    @<td>@Html.TextBox(String.Format("txtAnyosSeriePV-{0}", adicInfo.Id), adicInfo.SeriesYears.ToString("N0", culturaEsES), New With {.class = "form-control numeric input-text", .readonly = "readonly", .style = "font-weight: bold;"})</td>
                Next
            End code
        </tr>
    </tbody>
</table>

@code
    If (listaAdicInfoAux.Count > 0) Then
        @<input type = "button" id="@String.Format("masvalidaciones-{0}", idPlanta)" value="@Utils.Traducir("Ver más")" Class="btn" />
    End If
End Code


<div class="modal fade" id="@String.Format("modalWindowMasValidaciones-{0}", idPlanta)" tabindex="-1" role="dialog" aria-hidden="true">
    <div class="modal-dialog modal-lg" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">@Utils.Traducir("Más validaciones")</h4>
            </div>
            <div class="modal-body table-responsive">
                <table Class="table table-bordered">
                    <thead>
                        <tr>
                            <th style="min-width:150px;"></th>
                            @code
                                For Each adicInfo In listaAdicInfoAux
                                    @<th Class="info" style="text-align:center; min-width:200px;"><label>@String.Format("{0} ({1})", Utils.Traducir("Validacion"), adicInfo.FechaAlta.ToShortDateString())</label></th>
                                Next
                            End code
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td class="success"><label>@etiquetaNetMagin %</label></td>
                            @code
                                For Each adicInfo In listaAdicInfoAux
                                    @<td>@Html.TextBox(String.Format("txtMargenNetoPV-{0}", adicInfo.Id), adicInfo.NetMargin.ToString("N3", culturaEsES), New With {.class = "form-control numeric input-text", .readonly = "readonly", .style = "font-weight: bold;"})</td>
                                Next
                            End code
                        </tr>
                        <tr>
                            <td class="success"><label>@etiquetaEffectiveSales</label></td>
                            @code
                                For Each adicInfo In listaAdicInfoAux
                                    @<td>@Html.TextBox(String.Format("txtVentasEfectivasPV-{0}", adicInfo.Id), adicInfo.EffectiveSales.ToString("N0", culturaEsES), New With {.class = "form-control numeric input-text", .readonly = "readonly", .style = "font-weight: bold;"})</td>
                                Next
                            End code
                        </tr>
                        <tr>
                            <td class="success"><label>@Utils.Traducir("Beneficio herramental propiedad cliente") %</label></td>
                            @code
                                For Each adicInfo In listaAdicInfoAux
                                    @<td>@Html.TextBox(String.Format("txtBeneficioHerramentalPV-{0}", adicInfo.Id), adicInfo.CustomerProperty.ToString("N3", culturaEsES), New With {.class = "form-control numeric input-text", .readonly = "readonly", .style = "font-weight: bold;"})</td>
                                Next
                            End code
                        </tr>
                        @code
                            If (cabecera.TipoProyPtksis <> ELL.CabeceraCostCarrier.TIPO_PROY_ECO_BATZ) Then
                                @<tr>
                                    <td Class="success"><label>@Utils.Traducir("Plantas del cliente")</label></td>
                                    @code
                                        For Each adicInfo In listaAdicInfoAux
                                            @<td>@Html.TextBox(String.Format("txtPlantasClientePV-{0}", adicInfo.Id), adicInfo.CustomerPlants, New With {.class = "form-control", .maxLength = 100, .readonly = "readonly", .style = "font-weight: bold;"})</td>
                                        Next
                                    End code
                                </tr>
                            End if
                        end code
                        <tr>
                            <td class="success"><label>@Utils.Traducir("SOP")</label></td>
                            @code
                                For Each adicInfo In listaAdicInfoAux
                                    @<td>@Html.TextBox(String.Format("txtSOPPV-{0}", adicInfo.Id), adicInfo.SOP.ToShortDateString(), New With {.class = "form-control", .readonly = "readonly", .style = "font-weight: bold;"})</td>
                                Next
                            End code
                        </tr>
                        <tr>
                            <td class="success"><label>@Utils.Traducir("Volumen medio anual")</label></td>
                            @code
                                For Each adicInfo In listaAdicInfoAux
                                    @<td>@Html.TextBox(String.Format("txtVolumenMedioPV-{0}", adicInfo.Id), adicInfo.AverageVolumen.ToString("N0", culturaEsES), New With {.class = "form-control numeric input-text", .readonly = "readonly", .style = "font-weight: bold;"})</td>
                                Next
                            End code
                        </tr>
                        <tr>
                            <td class="success"><label>@Utils.Traducir("Años serie")</label></td>
                            @code
                                For Each adicInfo In listaAdicInfoAux
                                    @<td>@Html.TextBox(String.Format("txtAnyosSeriePV-{0}", adicInfo.Id), adicInfo.SeriesYears.ToString("N0", culturaEsES), New With {.class = "form-control numeric input-text", .readonly = "readonly", .style = "font-weight: bold;"})</td>
                                Next
                            End code
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-dismiss="modal">@Utils.Traducir("Cerrar")</button>
            </div>
        </div>
    </div>
</div>