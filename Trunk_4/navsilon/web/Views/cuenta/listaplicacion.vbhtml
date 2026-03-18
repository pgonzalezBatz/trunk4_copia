@imports web
@Code
    ViewBag.title = "Aplicacion"
End code

<a href="@Url.Action("createaplicacion")">@h.traducir("Crear nueva aplicacion")</a>
@If Model.count = 0 Then
    @<h3> @h.traducir("No se ha encontrado ningun elemento")</h3>
Else
    @<table class="table3">
        <thead>
            <tr>
                <th>@h.traducir("Id")</th>
                <th>@h.traducir("Nombre")</th>
                <th></th>
            </tr>
        </thead>
        <tbody>
                @For Each e In Model
                   @<tr>
                       <td>@e.id</td>
                       <td>@e.nombre</td>        
                       <td>
                        <a href="@Url.Action("listtipocuenta", New With {.idaplicacion = e.id})">@h.traducir("ver/crear tipos cuenta")</a>        
                       </td>
                    </tr>
                Next
        </tbody>
     </table>
End If

