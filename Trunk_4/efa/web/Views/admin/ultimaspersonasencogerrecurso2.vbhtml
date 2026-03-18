@imports web

@section header
    <title>@h.traducir("Listado de recursos reservados")</title>
End section

<table class="table1">
    <thead>
        <tr>
            <th>@h.Traducir("Id sab")  </th>
            <th>@h.Traducir("Nombre")  </th>
            <th>@h.Traducir("Apellido 1")  </th>
            <th>@h.Traducir("Apellido 2")  </th>
            <th>@h.Traducir("Fecha adquisición")  </th>
            <th>@h.Traducir("Fecha devolución")  </th>
        </tr>
    </thead>
    <tbody>
        @For Each u In Model
        @<tr>
            <td>@u.idSab</td>
            <td>@u.nombre</td>
            <td>@u.apellido1</td>
            <td>@u.apellido2</td>
            <td>@u.fechaCoger.toshortdatestring</td>
            <td>
                @If u.fechadejar.ToString.Length > 0 Then
                @u.fechaDejar
                Else
                @h.Traducir("Todavia no ha sido devuelto")
                End If
            </td>
        </tr>
        Next
    </tbody>
</table>