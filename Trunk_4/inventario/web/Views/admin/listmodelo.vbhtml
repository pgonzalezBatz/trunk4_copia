@imports web
@Code
    ViewBag.title = "Administrción de modelos"
End code
@Html.ActionLink(h.Traducir("Añadir modelo"),"addmodelo",New With{.idtipo=Request("idtipo"),.idmarca=Request("idmarca")})  |
@Html.ActionLink(h.Traducir("Volver a marcas"),"listmarca",New With{.idtipo=Request("idtipo")})
    <br />
@If Model.count > 0 Then
    @<table class="table3">
        <thead>
            <tr>
                <th>@h.Traducir("Id")</th>
                <th>@h.Traducir("nombre")</th>
                <th colspan="3"></th>
            </tr>
        </thead>
        <tbody>
            @For Each t In Model
                @<tr>   
                    <td>@t.id</td>
                    <td>@t.nombre</td>
                    <td>@t.precio</td>
                    <td>@Html.ActionLink(h.Traducir("Editar"), "editmodelo", New With {.idtipo = Request("idtipo"), .idmarca = Request("idmarca"), .idmodelo = t.id})</td>
                     <td>@Html.ActionLink(h.traducir("Ver asignaciones"), "listAsignaciones", New With {.idtipo = Request("idtipo"), .idmarca = Request("idmarca"), .idmodelo = t.id})</td>
                 </tr>
            Next
        </tbody>
    </table>
Else
    @h.Traducir("No se han definido tipos")
End If
