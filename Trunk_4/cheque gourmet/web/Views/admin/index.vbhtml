@imports web

@section header
    <title>@h.traducir("Administración")</title>
End section


<table class="table1">
    <thead>
        <tr>
            <th>@h.traducir("Descripción")</th>
            <th>@h.traducir("Precio")</th>
            <th>@h.traducir("Numero de cheques")</th>
            <th>@h.traducir("Acción")</th>
        </tr>
    </thead>
    <tbody>
        @For Each d As web.TipoDistribucion In Model
        @<tr>
            <td>@d.Nombre </td>
            <td>@d.Precio.ToString("C") </td>
            <td>@d.NumCheques</td>
            <td><a href="@(Url.Action("edit"))?id=@(d.id)">@h.traducir("Editar")</a></td>
        </tr>

        Next
    </tbody>
</table>
<a href="@Url.Action("add")">@h.traducir("Añadir nuevo")</a>