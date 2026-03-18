@modeltype  VMAgrupacionSinAsignar
@Code
    Layout = "~/Views/Shared/RMaster.vbhtml"
End Code


<h3>@h.traducir("Listado Bultos Nuevos")
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

<form action="@Url.Action("CrearAlbaran", h.ToRouteValues(Request.QueryString, Nothing))" method="post">

    @If Model Is Nothing Then
        @h.traducir("No se han encontrado movimientos de marcas pendientes")
    Else

        @<table class="table">
            <thead>
                <tr>
                    <th>@h.traducir("Nº bulto")</th>
                    <th>@h.traducir("Peso")</th>
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
                    <th>@h.traducir("Observación")</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                @Html.DisplayFor(Function(m) m.ListOfAgrupacion)
            </tbody>


        </table>
    End If
    <input type="submit" class="btn btn-primary" value="@h.traducir("Agrupar")" />
</form>
