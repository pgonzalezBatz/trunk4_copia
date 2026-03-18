@imports web
@Code
    ViewBag.title = h.traducir("Asientos mensuales")
End code

@If Model.count = 0 Then
    @<h3> @h.traducir("No se ha encontrado ningun elemento")</h3>
Else
    @<table class="table3">
        <thead>
            <tr>
                <th>@h.traducir("Id")</th>
                <th>@h.traducir("Nombre")</th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            @For Each e In Model
                @<tr>
                    <td>@e.id</td>
                    <td>@e.nombre</td>
                    <td>
                        <a href="@Url.Action("volcado", h.ToRouteValues(Request.QueryString, New With {.idasiento = e.id}))">@h.traducir("ver asiento")</a>
                    </td>
                </tr>
            Next
        </tbody>
    </table>
End If
