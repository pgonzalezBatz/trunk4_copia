@imports web
@Code
    ViewBag.title = "Administrción de tipos y atributos"
End code
@Html.ActionLink(h.Traducir("Añadir tipo"),"addtipo")
    <br />
@If model.count > 0 Then
    @<table class="table3">
        <thead>
            <tr>
                <th>@h.Traducir("Id")</th>
                <th>@h.Traducir("nombre")</th>
                <th colspan="2"></th>
            </tr>
        </thead>
        <tbody>
            @For Each t In Model
                @<tr>   
                    <td>@t.key</td>
                    <td>@t.value</td>
                    <td>@Html.ActionLink(h.Traducir("Editar"), "edittipo", New With {.idtipo = t.key})</td>
                    <td>@Html.ActionLink(h.Traducir("Ver marcas"), "listmarca", New With {.idtipo = t.key})</td>
                 </tr>
            Next
        </tbody>
    </table>
Else
    @h.Traducir("No se han definido tipos")
End If