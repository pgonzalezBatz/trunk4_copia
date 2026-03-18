 @imports web

<table class="table">
    <thead>
        <tr>
            <th>@h.traducir("Colaborador")</th>
            <th>@h.traducir("Fecha")</th>
            @For Each i In Enumerable.Range(1, 13)
        @<th>Resp. @i.ToString</th>
            Next

            <th>@h.traducir("Total")</th>
            <th></th>
            <th>@h.traducir("Continua")</th>
            <th>@h.traducir("Indice")</th>
        </tr>
    </thead>
    <tbody>
        @For Each c In Model
            @<tr>
    <td>@c.nombre @c.apellido1 @c.apellido2</td>
    <td>@CType(c.lstevaluacion(0).fechaVencimiento, DateTime).ToShortDateString</td>
    @For Each p In c.lstEvaluacion
        @<td>
            @p.puntuacion
            @If p.idNotificadoVencimiento Is Nothing Or SimpleRoleProvider.IsUserAuthorised(Role.rrhh) Then
                @<a href="@Url.Action("cambiarPuntuacion", New With {.idformulario = p.idformulario, .idsab = p.idsab, .idrespuesta = p.idrespuesta})">@h.traducir("Editar")</a>
            End If
        </td>

    Next
    <th>
        @c.nota
    </th>
    <td>
        <a href="@Url.Action("viewrespuestatexto", New With {.idformulario = c.idformulario, .idsab = c.lstevaluacion(0).idsab, .ticksVencimiento = CType(c.lstevaluacion(0).fechaVencimiento, DateTime).Ticks})">@h.traducir("Ver otras respuestas")</a>
    </td>
    <td>
        @If c.continua IsNot Nothing Then
            @if c.continua Then
                @h.traducir("Si")
            Else
                @h.traducir("No")
            End If
        End If

    </td>
    <td>
        @If c.indice IsNot Nothing Then
           @Math.Round(c.indice, 3)
        End If

    </td>
</tr>
        Next
    </tbody>
</table>    