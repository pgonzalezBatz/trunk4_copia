@imports web

@section header
    <title>@h.traducir("Listado de distribuciones")</title>
End section

<table class="table1">
    <thead>
        <tr>
            <th>@h.traducir("Hora")</th>
            <th>@h.traducir("Tipo")</th>
            <th>@h.traducir("Precio")</th>
            <th>@h.traducir("Nombre")</th>
            <th>@h.traducir("Cantidad")</th>
            <th>@h.traducir("Acción")</th>
        </tr>
    </thead>
    <tbody>
        @For Each d In Model
            Dim cantidad = d.hasta - d.desde + 1
        @<tr>
            <td>@d.hora</td>
            <td>@d.tipo</td>
            <td>@CDec(d.precio).ToString("C")</td>
            <td>@d.nombre @d.apellido1 @d.apellido2</td>
            <td>@cantidad</td>
            <td>
                @If Regex.IsMatch(d.tipo, "^Batz", RegularExpressions.RegexOptions.IgnoreCase) Then
                @<a href="@(Url.Action("delete"))?desde=@(d.desde)&hasta=@(d.hasta)&codtra=@(d.codtra)">@h.traducir("Eliminar")</a>
                End If
            </td>
        </tr>
        Next
    </tbody>
</table>