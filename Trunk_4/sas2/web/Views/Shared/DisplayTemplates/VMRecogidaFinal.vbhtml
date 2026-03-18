@modeltype VMRecogidaFinal

<tr>
    <td>
        @Html.EditorFor(Function(m) m.Seleccionado)
        @Html.HiddenFor(Function(m) m.id)
    </td>
    <td>
        <a href="@Url.Action("ListMovimientosRecogidasSinAsignar", h.ToRouteValues(Request.QueryString, New With {.fecha = Model.Fecha.Ticks}))">
            @Html.DisplayFor(Function(o) o.Fecha)
        </a>
</td>
    @Html.DisplayFor(Function(o) o.VectorRecogida)
    <td>@Html.DisplayFor(Function(o) o.Observaciones)</td>
    <td>@Html.DisplayFor(Function(o) o.LstLinea)</td>
    <td>
        <a class="btn btn-info" href="@Url.Action("EditCabeceraRecogida", New With {.id = Model.id})">
            <span class="glyphicon glyphicon-edit"></span>
            @h.traducir("Editar Cabecera Recogida")
        </a>
        <a class="btn btn-info" href="@Url.Action("AddRecogidaLinea", New With {.id = Model.id})">
            <span class="glyphicon glyphicon-edit"></span>
            @h.traducir("Editar Linea Recogida")
        </a>
    </td>
</tr>