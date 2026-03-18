@imports web
@Code
    ViewBag.title = "Administrción de modelos"
End code
@Html.ActionLink(h.traducir("Volver"), "listmodelo", New With {.idtipo = Request("idtipo"), .idmarca = Request("idmarca"), .idmodelo = Request("idmodelo")})
    <br />
@If Model.count > 0 Then
    @<table class="table3">
        <thead>
            <tr>
                <th>@h.traducir("nombre")</th>
                <th>@h.traducir("nombre")</th>
                <th>@h.traducir("nombre")</th>
                <th>@h.traducir("Tipo de asignación")</th>
                <th>@h.traducir("Nº etiqueta")</th>
            </tr>
        </thead>
        <tbody>
            @For Each t In Model
                @<tr>   
                    <td>@t.nombre</td>
                    <td>@t.apellido1</td>
                    <td>@t.apellido2</td>
                     <td>
                         @if t.departamento Then
                             @h.traducir("Departamental")
                         Else
                            @h.traducir("Individual")
                         End If
            </td>
                     <td>@t.idEtiqueta</td>
                 </tr>
            Next
        </tbody>
    </table>
Else
    @h.traducir("No se han encontrado asignaciones a este modelo")
End If
