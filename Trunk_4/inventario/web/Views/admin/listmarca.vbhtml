@imports web
@Code
    ViewBag.title = "Administrción marcas"
End code
@Html.ActionLink(h.Traducir("Añadir marca"),"addmarca",New With{.idtipo=Request("idtipo")})  |
@Html.ActionLink(h.Traducir("Volver a tipos"),"listtipo",New With{.idtipo=Request("idtipo")})
    <br />
@If Model.count > 0 Then
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
                    <td>@Html.ActionLink(h.Traducir("Editar"), "editmarca", New With {.idtipo = Request("idtipo"), .idmarca = t.key})</td>
                    <td>@Html.ActionLink(h.Traducir("Ver modelos"), "listmodelo", New With {.idtipo = Request("idtipo"), .idmarca = t.key})</td>
                 </tr>
            Next
        </tbody>
    </table>
Else
    @h.Traducir("No se han definido tipos")
End If
