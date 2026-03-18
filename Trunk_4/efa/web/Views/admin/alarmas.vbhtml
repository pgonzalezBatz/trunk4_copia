@imports web

@section header
    <title>@h.traducir("Grupos")</title>
End section


@Html.ValidationSummary()
<table class="table1">
    <thead>
        <tr>
            <th>@h.traducir("Grupo")</th>
            <th>@h.traducir("Max. días")</th>
            <th>@h.traducir("Cambiar Max. días")</th>
            <th>@h.traducir("Excepciones")</th>
        </tr>
    </thead>
    <tbody>
        @For Each g As web.Grupo In ViewData("listofgroup")
        @<tr>
            <td>@g.Nombre </td>
            <td align="center">
                @If g.dias.HasValue Then
                @g.dias.Value
                Else
                @h.traducir("Sin asignar")
                End If
            </td>
            <td>
                <form action="@Url.Action("cambiaralarmas", New With {.nombre = g.Nombre})" method="post">
                    @Html.TextBox("dias")
                    <input type="submit" value="@h.traducir("cambiar")" />
                </form>
            </td>
            <td>
                <a href="@Url.Action("excepciones", New With {.nombre = g.Nombre})">@h.traducir("Ir a excepciones")  </a>
            </td>
        </tr>
        Next
    </tbody>
</table>