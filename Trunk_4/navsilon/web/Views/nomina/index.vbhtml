@imports web
@Code
    ViewBag.title = h.traducir("Definir y ejecutar asientos de nomina")
End code

&#8226;<strong><a>@h.traducir("Ejecutar los traspasos de un mes")</a>(@h.traducir("tienen que estar definidos los asientos y relacionados con columnas"))</strong>
<br />
@For Each d In ViewData("fechas")
    @<a href="@Url.Action("tipoNomina", New With {.year = d.year, .month = d.month})" style="margin-right:1.5em;">
        @d.year/@d.month
    </a>
Next
@If Date.UtcNow.Month >= 7 Then
    @<a href="@Url.Action("tipoNomina", New With {.year = Date.UtcNow.Year, .month = 7, .paga = 14})" style="margin-right:1.5em;">
        @H.Traducir("Paga de verano")
    </a>
End If
@If Date.UtcNow.Month >= 1 And Date.UtcNow.Month <= 4 Then

    @<a href="@Url.Action("tipoNomina", New With {.year = Date.UtcNow.Year - 1, .month = 6, .paga = 14})" style="margin-right:1.5em;">
        @H.Traducir("Paga de verano")
    </a>

    @<a href="@Url.Action("tipoNomina", New With {.year = Date.UtcNow.Year - 1, .month = 12, .paga = 13})" style="margin-right:1.5em;">
        @H.Traducir("Paga de navidad")
    </a>
End If
@If Date.UtcNow.Month = 12 Then
    @<a href="@Url.Action("tipoNomina", New With {.year = Date.UtcNow.Year, .month = 12, .paga = 13})" style="margin-right:1.5em;">
        @H.Traducir("Paga de navidad")
    </a>
End If
<br />
<a href="@Url.Action("definirasiento")"></a>
&#8226;<strong>@h.traducir("Definir los diferentes asientos")</strong>
<a If@h.traducir("Nuevo asiento")</a>
<br />
@For Each a In ViewData("listofasiento")
    @<a href="@Url.Action("definirasiento", New With {.idAsiento = a.id})">@h.traducir("Definir") @Html.Encode(" ")@a.nombre</a>
    @<br />
Next
&#8226;<strong>@h.traducir("Definir las relaciones de los asientos con los conceptos de RRHH")</strong>
<table class="table3">
    <thead>
        <tr>
            <th>@h.traducir("Id")</th>
            <th>@h.traducir("Nombre")</th>
            <th></th>
            <th></th>
        </tr>
    </thead>
    <tbody>
        @For Each tc In ViewData("tiposcuenta")
            @<tr>
                <td>@tc.key.id</td>
                <td>@tc.key.nombre</td>
                <td>
                    <a href="@Url.Action("editcolumnas", New With {.idTipoCuenta = tc.key.id})">@h.traducir("Editar")</a>
                </td>
                <td>
                    @For Each c In tc.listofcolumns
                        @c.nombre
                    Next
                </td>
            </tr>

        Next
    </tbody>
</table>