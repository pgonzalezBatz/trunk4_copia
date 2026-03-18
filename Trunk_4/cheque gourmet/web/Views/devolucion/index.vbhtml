@imports web

@section header
    <title>@h.traducir("Reparto de devoluciones")</title>
End section

<div style="text-align:left; width:100%;">
    <a href="@Url.Action("devolver")">@h.traducir("Nueva devolución")</a>
</div>

<table class="table1">
    <thead>
        <tr>
            <th>@h.traducir("Nº trabajador")</th>
            <th>@h.traducir("Nombre")</th>
            <th>@h.traducir("Nº de cheques")</th>
            <th>@h.traducir("Fecha")</th>
            <th>@h.traducir("Tipo cheque")</th>
            <th>@h.traducir("Precio")</th>
            <th>@h.traducir("Acciones")</th>
        </tr>
    </thead>
    <tbody>
        @For Each m In Model
        @<tr>
            <td>@m.codtra</td>
            <td>@m.nombre @m.apellido1 @m.apellido2</td>
            <td>@m.numeroCheques</td>
            <td>@m.fecha</td>
            <td>@m.nombrecheque</td>
            <td>@m.preciocheque</td>
            <td>
                <a href="@Url.Action("confirmarreparto", New With {.id = m.id})">@h.traducir("Reparto")</a>
            </td>
        </tr>
        Next
    </tbody>
</table>
