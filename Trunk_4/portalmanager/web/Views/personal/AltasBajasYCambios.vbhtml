@imports web
@ModelType IEnumerable(Of object)
@Code
    ViewBag.title = "Bajas Altas y cambios de secuencias"
End Code
@section header
    <link href="//intranet2.batz.es/baliabideorokorrak/jquery-ui-1.11.4.min.css" rel="Stylesheet" type="text/css" />
End Section
@Html.Partial("menu2")

<form action="" method="post" class="mb-3">
    <div class="form-row mb-3">
        <div class="col-2">
            <label class="text-capitalize">@H.Traducir("Desde")</label>
            @Html.TextBox("fromDate", Nothing, New With {.class = "calendar form-control "})
        </div>
        <div class="col-1">
            
        </div>
        <div class="col-2">
            <label class="text-capitalize">@H.Traducir("Hasta")</label>
            @Html.TextBox("toDate", Nothing, New With {.class = "calendar form-control"})
        </div>
    </div>
            <input class="btn btn-primary" type="submit" value="@H.Traducir("Mostrar datos")" />
</form>

@If ViewData("bajas") IsNot Nothing Then
    @<h4>@H.Traducir("Bajas")</h4>
    @<table class="table small">
        <thead class="thead-light">
            <tr>
                <th>@H.Traducir("Id trabajador")</th>
                <th>@H.Traducir("Secuencia")</th>
                <th>@H.Traducir("Nombre Completo")</th>
                <th>@H.Traducir("Convenio")</th>
                <th>@H.Traducir("Categoria")</th>
                <th>@H.Traducir("Baja")</th>
                <th>@H.Traducir("Departamento")</th>
                <th>N5</th>
                <th>N6</th>
                <th>N7</th>
                <th>@H.Traducir("Fecha baja")</th>
            </tr>
        </thead>
        @For Each p In ViewData("bajas")
            @<tr>
                <td><a href="@Url.Action("busquedacompleta", New With {.s = p.idTrabajador})">@p.idTrabajador</a></td>
                <td>@p.idSecuencia</td>
                <td>@p.nombre @Html.Encode(" ") @p.apellido1 @Html.Encode(" ") @p.apellido2</td>
                <td>@p.convenio</td>
                <td>@p.categoria</td>
                <td>@p.baja</td>
                <td>@p.dN4</td>
                <td>@p.dN5</td>
                <td>@p.dN6</td>
                <td>@p.dN7</td>
                <td>@p.fBaja.toshortdatestring()</td>
            </tr>   Next
    </table>
End If


@If ViewData("altas") IsNot Nothing Then
    @<h4>@h.traducir("Altas")</h4>
    @<table class="table small">
    <thead class="thead-light">
            <tr>
                <th>@h.traducir("Id trabajador")</th>
                <th>@h.traducir("Secuencia")</th>
                <th>@h.traducir("Nombre Completo")</th>
                <th>@h.traducir("Convenio")</th>
                <th>@h.traducir("Categoria")</th>
                <th>@h.traducir("Departamento")</th>
                <th>N5</th>
                <th>N6</th>
                <th>N7</th>
                <th>@h.traducir("Fecha alta")</th>
            </tr>
        </thead>
        @For Each p In ViewData("altas")
            @<tr>
                <td><a href="@Url.Action("busquedacompleta", New With {.s = p.idTrabajador})">@p.idTrabajador</a></td>
                <td>@p.idSecuencia</td>
                <td>@p.nombre @Html.Encode(" ") @p.apellido1 @Html.Encode(" ") @p.apellido2</td>
                <td>@p.convenio</td>
                <td>@p.categoria</td>
                <td>@p.dN4</td>
                <td>@p.dN5</td>
                <td>@p.dN6</td>
                <td>@p.dN7</td>
                <td>@p.fAlta.toshortdatestring()</td>
            </tr>
        Next
    </table>
end if

@If ViewData("cambios") IsNot Nothing Then
    @<h4>@h.traducir("Cambios de puesto")</h4>
    @<table class="table">
        <thead class="thead-light">
            <tr>
                <th>@h.traducir("Id trabajador")</th>
                <th>@h.traducir("Nombre Completo")</th>
                <th>@h.traducir("Puesto origen")</th>
                <th>@h.traducir("F. Puesto Origen ")</th>
                <th>@h.traducir("Puesto Destino")</th>
                <th>@h.traducir("F. Puesto Destino ")</th>
            </tr>
        </thead>
        @For Each p In ViewData("cambios")
            @<tr>
                <td><a href="@Url.Action("busquedacompleta", New With {.s = p.idTrabajador})">@p.idTrabajador</a></td>
                <td>@p.nombre @Html.Encode(" ") @p.apellido1 @Html.Encode(" ") @p.apellido2</td>
                <td>
                    @p.dN4_1 @Html.Encode(" | ") @p.dN5_1 @Html.Encode(" | ") @p.dN6_1 @Html.Encode(" | ") @p.dN7_1

                </td>
                <td>@p.fAlta1.toshortdatestring=>@p.fBaja1.toshortdatestring</td>
                <td>@p.dN4_2 @Html.Encode(" | ") @p.dN5_2 @Html.Encode(" | ") @p.dN6_2 @Html.Encode(" | ") @p.dN7_2</td>
                <td>@p.fAlta2.toshortdatestring=>@p.fBaja2</td>
            </tr>
        Next
    </table>
End If



<script src='//intranet2.batz.es/baliabideorokorrak/jquery-1.11.2.min.js' type="text/javascript"></script>
<script src='//intranet2.batz.es/baliabideorokorrak/jquery-ui-1.11.4.min.js' type="text/javascript"></script>
<script src='//intranet2.batz.es/baliabideorokorrak/jquery.ui.datepicker-@(h.GetCulture().Split("-")(0)).js' type="text/javascript"></script>
<script type="text/javascript">
    $(document).ready(function () {
        $('.calendar').datepicker($.datepicker.regional["@h.GetCulture().Split("-".ToCharArray, StringSplitOptions.RemoveEmptyEntries)(0)"]);
    });
</script>