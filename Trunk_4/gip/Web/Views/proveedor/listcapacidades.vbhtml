@imports web
@section title
    - @h.traducir("Listado de capacidades Matrix")
End section
@section menu1
    @Html.Partial("menu")
End Section

<form method="post" action="@Url.Action("updateCapacidades", h.ToRouteValues(Request.QueryString, Nothing))">
    <fieldset>
        @For Each c In Model
            @If c.asignado Then
                @<input name="capacidades" type="checkbox" value="@c.idcapacidad" checked="checked" class="col-sm-1" /> @<span class="col-sm-3">@c.nombreCapacidad</span> @<a href="@Url.Action("editarcapacidad", New With {.idCapacidad = c.idCapacidad})" class="col-sm-1"><span class="glyphicon glyphicon-pencil" aria-hidden="True" title="@h.Traducir("Editar capacidad")"></span></a> @<a href="@Url.Action("borrarcapacidad", New With {.idCapacidad = c.idCapacidad})" class="col-sm-1" onclick="return confirm('Seguro que quieres borrar la capacidad?')"><span class="glyphicon glyphicon-trash" aria-hidden="True" title="@h.Traducir("Eliminar capacidad")"></span></a>
            Else
                @<input name="capacidades" type="checkbox" value="@c.idcapacidad" class="col-sm-1" /> @<span class="col-sm-3">@c.nombreCapacidad</span> @<a href="@Url.Action("editarcapacidad", New With {.idCapacidad = c.idCapacidad})" class="col-sm-1"><span class="glyphicon glyphicon-pencil" aria-hidden="True" title="@h.Traducir("Editar capacidad")"></span></a> @<a href="@Url.Action("borrarcapacidad", New With {.idCapacidad = c.idCapacidad})" class="col-sm-1" onclick="return confirm('Seguro que quieres borrar la capacidad?')"><span class="glyphicon glyphicon-trash" aria-hidden="True" title="@h.Traducir("Eliminar capacidad")"></span></a>
            End If

            @<br />
        Next
        <input type="submit" value="@h.Traducir("Guardar")" class="btn btn-primary" />
        @Html.Encode(" | ")
        <a href="@Url.Action("search", h.ToRouteValuesDelete(Request.QueryString, "id", "codpro"))">@h.Traducir("Volver al listado")</a>
        @Html.Encode(" | ")
        <a href="@Url.Action("nuevacapacidad", h.ToRouteValues(Request.QueryString, Nothing))">@h.Traducir("Nueva capacidad")</a>
    </fieldset>
</form>
