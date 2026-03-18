@ModelType List(Of String())

@Code
    ViewData("Title") = "Indicadores de Cliente Garantías"

    Dim pathPerfect = "../Content/img/cmpG.png"
    Dim pathLimit = "../Content/img/cmpY.png"
    Dim pathBad = "../Content/img/cmpR.png"
    Dim pathNone = "../Content/img/cmpX.png"
End Code

<br />
<h2>P3.3 Reclamaciones cliente Garantías</h2>
<br />
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
            <div class="col-md-6">
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
            <div class="col-md-6">
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
                @If ViewBag.MuestraMsg Then
                    @<span id="msgError" class="muestraMsg">Debes seleccionar todos los filtros</span>
                Else
                    @<span>&nbsp;</span>
                End If
            </div>
        </p>
        @*@<br />@<br />*@
        @*@<p class="reportes" style="text-align:center">
            <span>Reportes COGNOS: </span>
            <a href="https://cognos.batz.es/ibmcognos/bi/?objRef=i16B7B03452E5478792A60590BB54F0F1&ui_appbar=false&ui_navbar=false&format=spreadsheetML" onclick="return updateData()" target="_blank"><img src="~/Content/themes/base/Imagenes/excel.png" style="height:20px" /></a>
            <a href="https://cognos.batz.es/ibmcognos/bi/?objRef=i16B7B03452E5478792A60590BB54F0F1&ui_appbar=false&ui_navbar=false&format=PDF" onclick="return updateData()" target="_blank"><img src="~/Content/themes/base/Imagenes/pdf.png" style="height:20px" /></a>
        </p>*@

    End Using
</div>
<br />

@If ViewBag.ShowTable Then
    @<div id="resumenFiltros" class="resumenFiltros">
        <h4 style="margin-left:20px;color:#337ab7"><strong> Filtros seleccionados</strong></h4>
        <div style="margin-left:40px">
            <strong>CLIENTES: </strong>@ViewBag.clientesResumen
            <br />
            <strong>PRODUCTOS: </strong> @ViewBag.productosResumen
            <br />
        </div>
    </div>
    @<br />

    @<Table Class="table resultTable" id="tablaCalidad">
        <tr style="background-color:#337ab7;color:white">
            <th style="min-width:80px;">
                @Html.Label("FECHA")
            </th>
            <th>
                @Html.Label("Piezas Rechazadas")
            </th>
            <th>
                @Html.Label("Nº Incidencias")
            </th>
        </tr>

        @For Each item In Model
            ' Calculamos prrM antes de generar la fila
            Dim prrM As Integer = 0

            Integer.TryParse(item(1), prrM)

            @<tr>
                <td>@item(0)</td> <!-- Fecha (Año / Mes) -->
                <td>@item(2)</td> <!-- Piezas Rechazadas -->
                <td>@prrM</td> <!-- Nº Incidencias -->
            </tr>
        Next
    </Table>
    @<br />
End If

<div>
    <span> <i> Los cálculos del mes anterior al mes en curso sólo se mostrarán a partir del día @(Configuration.ConfigurationManager.AppSettings("CalculusDay") + 1)</i></span>
</div>

@section Scripts
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
        });

        $(document).ready(function () {
            var showTable = '@ViewBag.ShowTable';
            var muestraMsg = '@ViewBag.MuestraMsg';
            if (showTable == 'False' && muestraMsg == 'False') {
                seleccionarTodo();
            }
        });

        function updateData() {
            $.ajax({
                type: 'POST',
                url: '@Url.Action("UpdateData")',
                success: function (result) {
                    console.log('Data updated successfully!')
                },
                error: function (ex) {
                    alert('Failed to update data.' + ex);
                }
            });
        }
    </script>
End Section
