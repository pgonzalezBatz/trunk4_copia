@ModelType List(Of Object)
@Code
    ViewData("Title") = "Indicadores"
    'Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<br />
<h2>Indicadores automáticos</h2>
<br />
@Using (Html.BeginForm("Index_Filtro", "INDICADORES2", FormMethod.Get))
    @Html.AntiForgeryToken()
    @<div Class="row">
        <div Class="col-md-6">
            <h4 class="col-md-6">
                @Html.Label("Cliente", htmlAttributes:=New With {.Class = "control-label"})
            </h4>
            <p class="col-md-6">
                @Html.DropDownList("lClientes_ind", Nothing, "Seleccionar uno", htmlAttributes:=New With {.Class = "form-control", .onchange = "$(this).closest('form').trigger('submit');"})
            </p>
        </div>
        <div Class="col-md-6">
            <h4 class="col-md-6">
                @Html.Label("Productos", htmlAttributes:=New With {.Class = "control-label"})
            </h4>
            <p class="col-md-6">
                @Html.DropDownList("lProductos_ind", Nothing, "Seleccionar uno", htmlAttributes:=New With {.Class = "form-control", .onchange = "$(this).closest('form').trigger('submit');"})
            </p>
        </div>
    </div>
    @<div Class="row">
        <div Class="col-md-6">
            <h4 class="col-md-6">
                @Html.Label("Fecha desde", htmlAttributes:=New With {.class = "control-label"})
            </h4>
            <div class='input-group date myDatePicker' id='datetimepicker_1'>
                @Html.TextBox("FECHA_DESDE_ind", If(ViewBag.FECHA_DESDE_ind Is Nothing, Date.Today.AddDays(-Configuration.ConfigurationManager.AppSettings("CalculusDay") + 1).AddMonths(-Configuration.ConfigurationManager.AppSettings("DefaultMonthRange")).ToString("yyyy/MM"), Date.ParseExact(ViewBag.FECHA_DESDE_ind, "yyyy/MM", System.Globalization.DateTimeFormatInfo.InvariantInfo).ToString("yyyy/MM")), New With {.class = "form-control"})
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
                @Html.TextBox("FECHA_HASTA_ind", If(ViewBag.FECHA_HASTA_ind Is Nothing, Date.Today.AddMonths(-1).AddDays(-Configuration.ConfigurationManager.AppSettings("CalculusDay") + 1).ToString("yyyy/MM"), Date.ParseExact(ViewBag.FECHA_HASTA_ind, "yyyy/MM", System.Globalization.DateTimeFormatInfo.InvariantInfo).ToString("yyyy/MM")), New With {.class = "form-control"})
                <span class="input-group-addon">
                    <span class="glyphicon glyphicon-calendar"></span>
                </span>
            </div>
        </div>
    </div>
End Using
<br />
<table class="table">
    @*<tr style="background-color:#337ab7;color:white">
            <th rowspan="2">
                @Html.Label("FECHA")
            </th>
            <th rowspan="2">
                @Html.Label("ENVIADAS")
            </th>
            <th rowspan="2">
                @Html.Label("RECHAZADAS")
            </th>
            <th rowspan="2">
                @Html.Label("INCIDENCIAS")
            </th>
            <th rowspan="2">
                @Html.Label("Cliente")
            </th>
            <th rowspan="2">
                @Html.Label("Producto")
            </th>
            <th colspan="2">
                @Html.Label("Dato")
            </th>
            <th colspan="2">
                @Html.Label("Objetivo mensual")
            </th>
            <th colspan="2">
                @Html.Label("Objetivo anual")
            </th>
        </tr>
        <tr class="noHover" style="background-color:#337ab7;color:white">
            <th>PPM</th>
            <th>IPB</th>
            <th>PPM</th>
            <th>IPB</th>
            <th>PPM</th>
            <th>IPB</th>
        </tr>*@
    <tr style="background-color:#337ab7;color:white">
        <th>
            @Html.Label("FECHA")
        </th>
        <th>
            @Html.Label("ENVIADAS")
        </th>
        <th>
            @Html.Label("RECHAZADAS")
        </th>
        <th>
            @Html.Label("INCIDENCIAS")
        </th>
        <th>
            @Html.Label("Cliente")
        </th>
        <th>
            @Html.Label("Producto")
        </th>
    </tr>
    @For Each item In Model
        Dim oracleDb As New oracleDB
        Dim clientName = item(4).ToString
        Dim productName = oracleDb.getProductoNameFromProductoString(item(3).ToString, CInt(item(0)))
        'Dim objO = item(8).split(";")
        'Dim ppmOM = objO(0)
        'Dim prrOM = objO(1)
        'Dim ppmOA = objO(2)
        'Dim prrOA = objO(3)
        'Dim objD = item(9).split(";")
        'Dim ppm = objD(0)
        'Dim ipb = objD(1)
        @<tr>
            <td>
                @item(1) / @item(2)
            </td>
            <td>
                @item(5)
            </td>
            <td>
                @item(7)
            </td>
            <td>
                @item(6)
            </td>
            <td>
                @clientName
            </td>
            <td>
                @productName
            </td>
            @*<td>
                    @ppm
                </td>
                <td>
                    @ipb
                </td>
                <td>
                    @ppmOM
                </td>
                <td>
                    @prrOM
                </td>
                <td>
                    @ppmOA
                </td>
                <td>
                    @prrOA
                </td>*@
        </tr>
    Next
</table>

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
    <script>
        $('.myDatePicker').datetimepicker({
            viewMode: 'months',
            format: 'YYYY/MM',
            maxDate: moment().subtract(1, 'months'),
            useCurrent: false
    })
        .on('dp.change', function (e) {
            $(this).closest('form').trigger('submit');
        });
    </script>
End Section



@*,
         defaultDate: @Date.Today.AddMonths(-1).ToString("yyyy/MM")*@