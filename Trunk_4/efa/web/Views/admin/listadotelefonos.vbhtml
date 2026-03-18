@imports web

@section header
    <title>@h.traducir("Listado de telefonos reservados")</title>
End section

@section  beforebody
    <strong><a href="@Url.Action("index")">@h.traducir("Volver")</a></strong>
    <br style="clear:both;" />
End Section

<table class="table1">
    <thead>
        <tr>
            <th>@h.Traducir("Número")  </th>
            <th>@h.Traducir("Id SAB")  </th>
            <th>@h.Traducir("Cogido el")  </th>
        </tr>
    </thead>
    @For Each r As RegistroDisplay In ViewData("registros")
    @<tr>
        <td>@r.Id</td>
        <td>
            @r.Nombre
            @r.Apellido1
            @r.Apellido2
        </td>
        <td>@r.Coger.ToShortDateString</td>

    </tr>
    Next
</table>