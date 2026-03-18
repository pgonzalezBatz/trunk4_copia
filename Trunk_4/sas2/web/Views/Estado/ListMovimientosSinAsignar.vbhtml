@modeltype VMMovimientoSinAsignar
@Code
    Layout = "~/Views/Shared/RMaster.vbhtml"
End Code


<h3>@h.traducir("Listado de Movimientos Nuevos")
    <span class="label label-default">
        <span class="glyphicon glyphicon-info-sign"></span>
        Movimientos Por Marca
    </span>
    </h3>
<hr />
<div class="row">
    <div class="col-xs-3">
        <a class="btn btn-success" href="@Url.Action("AddMovimientoStep1")">
            <span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span>
            @h.traducir("Crear Nuevo Movimiento (Marcas)")
        </a>
    </div>
    <div class="col-xs-3">
        <a class="btn btn-success" href="@Url.Action("AddRecogidaCabecera")">
            <span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span>
            @h.traducir("Crear Nueva Recogida (OF-OP)")
        </a>
    </div>
    <div class="col-xs-3">
        <a class="btn btn-info" href="@Url.Action("ListMovimientosSinAsignarAgrupado", h.ToRouteValues(Request.QueryString, Nothing))">
            <span class="glyphicon glyphicon-compressed" aria-hidden="true"></span>
            @h.traducir("Ver Movimientos por fecha y OF-OP")
        </a>
    </div>

</div>
<br />
<div class="row">
    @If IsNumeric(Request("fecha")) Then
        @<div Class="alert alert-info" role="alert">
            @h.traducir("Filtro Fecha") @(New DateTime(Request("fecha")).ToShortDateString)
            <a href="@Url.Action("ListMovimientosRecogidasSinAsignar", h.ToRouteValuesDelete(Request.QueryString, "fecha"))" Class="btn btn-danger">@h.traducir("Quitar filtro Fecha")</a>
        </div>

    End If
</div>

<form action="@Url.Action("AgruparMovimiento", h.ToRouteValues(Request.QueryString, Nothing))" method="post">

    @If Model.LstMovimientoProveedorFecha Is Nothing Then
        @h.traducir("No se han encontrado movimientos de marcas pendientes")
    Else

        @<table class="table">
            <thead>
                <tr>
                    <th>@h.traducir("Fecha")</th>
                    <th>
                        @h.traducir("Origen")
                                        <span class="glyphicon glyphicon-chevron-right"></span>
                                        <span class="glyphicon glyphicon-send"></span>
                                        <span class="glyphicon glyphicon-chevron-right"></span>
                                        @h.traducir("Destino")
                                    </th>
                    <th>@h.traducir("OF-OP")</th>
                    <th>@h.traducir("Creado")</th>
                    <th>@h.traducir("Marca")</th>
                    <th>@h.traducir("Cantidad")</th>
                    <th>@h.traducir("Peso")</th>
                    <th>@h.traducir("Dimensiones")</th>
                    <th>@h.traducir("Observaciones")</th>
                    <th>@h.traducir("Articulo")</th>
                    <th>@h.traducir("Accion")</th>
                </tr>
            </thead>
            @Html.DisplayFor(Function(m) m.LstMovimientoProveedorFecha)
        </table>
    End If
    <input type="submit" class="btn btn-primary" value="@h.traducir("Agrupar")" />
</form>
