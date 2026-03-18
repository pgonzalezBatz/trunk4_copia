@ModelType List(Of String())
@Code
    ViewData("Title") = "Proceso"
    Layout = "~/Views/Shared/_Layout.vbhtml"

    Dim pathPerfect = "../Content/img/cmpG.png"
    Dim pathLimit = "../Content/img/cmpY.png"
    Dim pathBad = "../Content/img/cmpR.png"
    Dim pathNone = "../Content/img/cmpX.png"
End Code

<br />
<h2>P3.1 Proceso fabricación</h2>
<br />

<div style="border:1px solid #ccc;border-radius:5px;background-color:#eee">
    <br />
    @Using (Html.BeginForm("Index_Filtro", "INDICADORES2", FormMethod.Get))
        @Html.AntiForgeryToken()
        @<div Class="row filters">
            <div Class="col-md-6">
                <h4 class="col-md-6">
                    @Html.Label("Cliente", htmlAttributes:=New With {.Class = "control-label"})
                </h4>
                <p class="col-md-6">
                    @Html.DropDownList("lClientes_ind", Nothing, htmlAttributes:=New With {.Class = "form-control multiDropdown", .multiple = "multiple"})
                </p>
            </div>
            <div Class="col-md-6">
                <h4 class="col-md-6">
                    @Html.Label("Productos", htmlAttributes:=New With {.Class = "control-label"})
                </h4>
                <p class="col-md-6">
                    @Html.DropDownList("lProductos_ind", Nothing, htmlAttributes:=New With {.Class = "form-control multiDropdown", .multiple = "multiple"})
                    @Html.Hidden("productosAll")
                </p>
            </div>
        </div>
        @<div Class="row filters">
            <div Class="col-md-6">
                <h4 class="col-md-6">
                    @Html.Label("Fecha desde", htmlAttributes:=New With {.class = "control-label"})
                </h4>
                <div class='input-group date myDatePicker' id='datetimepicker_1'>
                    @Html.TextBox("FECHA_DESDE_ind", If(ViewBag.FECHA_DESDE_ind Is Nothing, Date.Today.AddDays(-Configuration.ConfigurationManager.AppSettings("CalculusDay")).AddMonths(-Configuration.ConfigurationManager.AppSettings("DefaultMonthRange")).ToString("yyyy/MM"), Date.ParseExact(ViewBag.FECHA_DESDE_ind, "yyyy/MM", System.Globalization.DateTimeFormatInfo.InvariantInfo).ToString("yyyy/MM")), New With {.class = "form-control"})
                    <span class="input-group-addon">
                        <span class="glyphicon glyphicon-calendar"></span>
                    </span>
                </div>
            </div>
            <div Class="col-md-6">
                <h4 class="col-md-6">
                    @Html.Label("Fecha hasta", htmlAttributes:=New With {.class = "control-label"})
                </h4>
                <div class='input-group date myDatePicker' id='datetimepicker_2'>
                    @Html.TextBox("FECHA_HASTA_ind", If(ViewBag.FECHA_HASTA_ind Is Nothing, Date.Today.AddMonths(-1).AddDays(-Configuration.ConfigurationManager.AppSettings("CalculusDay")).ToString("yyyy/MM"), Date.ParseExact(ViewBag.FECHA_HASTA_ind, "yyyy/MM", System.Globalization.DateTimeFormatInfo.InvariantInfo).ToString("yyyy/MM")), New With {.class = "form-control"})
                    <span class="input-group-addon">
                        <span class="glyphicon glyphicon-calendar"></span>
                    </span>
                </div>
            </div>
        </div>
        @<div class="hidden-md"><br /></div>
        @<div style="text-align:center">
            <button type="submit" Class="btn btn-success" onclick="seleccionarTodo(); return false;">
                <i class="glyphicon glyphicon-ok" style="margin-right:10px;"></i>Seleccionar todo
            </button>
            <button type="submit" Class="btn btn-danger" onclick="resetearTodo(); return false;">
                <i class="glyphicon glyphicon-refresh" style="margin-right:10px;"></i>Borrar
            </button>
        </div>
        @<p>
            <div id="divBusqueda" style="text-align:center">
                <button type="submit" Class="btn btn-primary" onclick="return checkFilters('lProductos_ind');" style="font-size:18px">
                    <i class="glyphicon glyphicon-search" style="margin-right:10px;"></i>Buscar
                </button>
                @If ViewBag.MuestraMsg Then@<span id="msgError" class="muestraMsg">Debes seleccionar todos los filtros</span>
                Else
                    @<span>&nbsp;</span>
                End If
            </div>
        </p>
    End Using
</div>
<br />

@If ViewBag.ShowTable Then
    @<div id="resumenFiltros" class="resumenFiltros" >
        <h4 style="margin-left:20px;color:#337ab7"><strong> Filtros seleccionados</strong></h4>
        <div style="margin-left:40px">
            <strong>CLIENTES: </strong>@ViewBag.clientesResumen
            <br />
            <strong>PRODUCTOS: </strong> @ViewBag.productosResumen
            <br />
        </div>
    </div>
    @<br />

    @<Table Class="table resultTable table-bordered" id="tablaProceso">
        <tr style="background-color:#337ab7;color:white">
            <th rowspan="2" style="min-width:80px;">@Html.Label("AÑO/MES")</th>
            <th colspan="2">@Html.Label("Nº Incidencias")</th>
            <th colspan="3">@Html.Label("Repetitivas")</th>
            <th colspan="3">@Html.Label("Tiempo medio acciones inmediatas")</th>
            <th colspan="3">@Html.Label("Tiempo medio acciones correctivas")</th>
        </tr>
        <tr style="background-color:#337ab7;color:white">
            <th>@Html.Label("Mensual")</th>
            <th>@Html.Label("Acumulado")</th>
            <th>@Html.Label("Mensual Nº (%)")</th>
            <th>@Html.Label("Acumulado Nº (%)")</th>
            <th>@Html.Label("Objetivo (%)")</th>
            <th>@Html.Label("Mensual (Días)")</th>
            <th>@Html.Label("Acumulado Anual (Días)")</th>
            <th>@Html.Label("Objetivo (Días)")</th>
            <th>@Html.Label("Mensual (Días)")</th>
            <th>@Html.Label("Acumulado Anual (Días)")</th>
            <th>@Html.Label("Objetivo (Días)")</th>
        </tr>
        @For Each item In Model
            Dim percMes As String
            percMes = If(item(2).Equals("0"), "-", Math.Round(CInt(item(4)) / CInt(item(2)) * 100, 0, MidpointRounding.AwayFromZero))
            Dim percAno As String
            percAno = If(item(3).Equals("0"), "-", Math.Round(CInt(item(5)) / CInt(item(3)) * 100, 0, MidpointRounding.AwayFromZero))
            Dim repIconPath = If(Not item(3).Equals("0") AndAlso Not item(10).Equals("-"), If(Double.Parse(percAno) > Double.Parse(item(10)), pathBad, If(Double.Parse(percAno) = Double.Parse(item(10)), pathLimit, pathPerfect)), pathNone)
            Dim d14IconPath = If(Not item(3).Equals("0") AndAlso Not item(11).Equals("-"), If(helpers.UpRoundedDouble(item(7)) > Double.Parse(item(11)), pathBad, If(helpers.UpRoundedDouble(item(7)) = Double.Parse(item(11)), pathLimit, pathPerfect)), pathNone)
            Dim d56IconPath = If(Not item(3).Equals("0") AndAlso Not item(12).Equals("-"), If(helpers.UpRoundedDouble(item(9)) > Double.Parse(item(12)), pathBad, If(helpers.UpRoundedDouble(item(9)) = Double.Parse(item(12)), pathLimit, pathPerfect)), pathNone)
            Dim objRep = If(item(10).Equals("-"), "-", item(10) & "%")
            @<tr>
    <td>@item(0) / @item(1)</td>
    <td>@item(2)</td>
    <td>@item(3)</td>
    <td>@item(4)  @If Not percMes.Equals("-") Then@<span>@Html.Raw("(" & percMes & "%)")</span>End If</td>
    <td><img src="@repIconPath" style="padding:5px" />@item(5)  @If Not percAno.Equals("-") Then@<span>@Html.Raw("(" & percAno & "%)")</span>End If</td>
    <td>@objRep</td>
    <td>@helpers.UpRoundedDouble(item(6))</td>
    <td><img src="@d14IconPath" style="padding:5px" />@helpers.UpRoundedDouble(item(7))</td>
    <td>@item(11)</td>
    <td>@helpers.UpRoundedDouble(item(8))</td>
    <td><img src="@d56IconPath" style="padding:5px" />@helpers.UpRoundedDouble(item(9))</td>
    <td>@item(12)</td>
</tr>
        Next
    </Table>
    @<br />
End If
<div>
    <span> <i> Los cálculos del mes anterior al mes en curso sólo se mostrarán a partir del día @(Configuration.ConfigurationManager.AppSettings("CalculusDay") + 1)</i></span>
</div>

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
    @Scripts.Render("~/bundles/filters")
    <script>
        $('.myDatePicker').datetimepicker({
            viewMode: 'months',
            format: 'YYYY/MM',
            maxDate: moment().subtract(1, 'months').subtract(@Configuration.ConfigurationManager.AppSettings("CalculusDay"), 'days'),
            useCurrent: false
        });


        $(function () {
            $.validator.addMethod('date',
                function (value, element) {
                    if (this.optional(element)) {
                        return true;
                    }
                    var valid = true;
                    try {
                        $.datepicker.parseDate('dd/mm/yy', value);
                    }
                    catch (err) {
                        valid = false;
                    }
                    return valid;
                });
            //$(".datetype").datepicker({ dateFormat: 'dd/mm/yy' });
        });

        $(document).ready(function () {
            var showTable = '@ViewBag.ShowTable';
            var muestraMsg = '@ViewBag.MuestraMsg';
            if (showTable == 'False' && muestraMsg == 'False') {
                seleccionarTodo();
            }
        })
    </script>
End Section



