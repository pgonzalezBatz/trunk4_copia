@imports web

@section header
    <title>@h.traducir("Listado de posesiones")</title>
<style type="text/css">
    .table1 ul {list-style: none;  padding: 0;}
</style>
End section

<h3>@h.traducir("Listado de personas que actualmente tienen asignado algún teléfono") </h3>
<table class="table1">
    <thead>
        <tr>
            <th>@h.traducir("Nombre")</th>
            <th>@h.traducir("Apellido 1")</th>
            <th>@h.traducir("Apellido 2")</th>
            <th>@h.traducir("Numero")</th>
        </tr>
    </thead>
    <tbody>
        @For Each u In Model
        @<tr>
            <td>@u.nombre</td>
            <td>@u.apellido1</td>
            <td>@u.apellido2</td>
            <td>
                <ul>
                    @For Each r In u.listOfRecurso
                    @<li>
                        @r.id  <strong><a href="@Url.Action("reasignar", New With {.id = r.id})">@h.traducir("Reasignar")</a></strong>
                    </li>
                    Next
                </ul>
            </td>
        </tr>
        Next
    </tbody>
</table>