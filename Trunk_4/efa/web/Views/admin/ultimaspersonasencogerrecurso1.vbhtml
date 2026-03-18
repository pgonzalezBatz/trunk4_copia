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
        @For Each r In Model
        @<tr>
            <td>
                <a href="@Url.Action("ultimaspersonasencogerrecurso2", New With {.grupo = r.nombreGrupo, .recurso = r.id})">
                    @r.id
                </a>
            </td>
             <td>
                 @r.descripcion
             </td>
        </tr>
        Next
    </tbody>
</table>