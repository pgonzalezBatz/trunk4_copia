@imports web

@section header
    <title>@h.traducir("Listado de recursos")</title>
End section

<table class="table1">
    <thead>
        <tr>
            <th>@h.Traducir("Nombre de grupo")</th>
        </tr>
    </thead>
    <tbody>
        @For Each g In Model
        @<tr>
            <td>
                <a href="@Url.Action("ultimaspersonasencogerrecurso1", new with {.grupo=g.nombre})">
                    @g.nombre
                </a>
            </td>
        </tr>
        Next
    </tbody>
</table>