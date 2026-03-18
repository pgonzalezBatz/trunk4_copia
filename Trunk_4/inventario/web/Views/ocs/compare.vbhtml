@imports web
@Code
    ViewBag.title = "Comparar activos asignados con OCS"
End code

<strong>@h.traducir("Usuarios que segun el OCS no estan etiquetados")</strong>

<table class="table3">
    <thead>
        <tr>
            <th>@h.traducir("Usuario")</th>
            <th>@h.traducir("Nombre")</th>
            <th>@h.traducir("Apellido1")</th>
            <th>@h.traducir("Apellido2")</th>
            <th>@h.traducir("Departamento")</th>
        </tr>
    </thead>
    <tbody>
        @For Each d In Model
            @<tr>
                <td>@d.userName</td>
                <td>@d.nombre</td>
                <td>@d.apellido1</td>
                <td>@d.apellido2</td>
                <td>@d.nombredepartamento</td>
             </tr>
        Next
    </tbody>
</table>
