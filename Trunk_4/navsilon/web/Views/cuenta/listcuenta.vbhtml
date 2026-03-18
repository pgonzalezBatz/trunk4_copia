@imports web
@Code
    ViewBag.title = "Cuenta"
End code
@section top
    <a href="@Url.Action("listtipocuenta", h.ToRouteValuesDelete(Request.QueryString,"idTipoCuenta"))">@h.traducir("Volver")</a>
End Section
<a href="@Url.Action("createcuenta", New With {.idAplicacion = Request("idaplicacion"), .idtipocuenta = Request("idtipocuenta")})">@h.traducir("Crear nueva cuenta")</a>
@If Model.count = 0 Then
    @<h3> @h.traducir("No se ha encontrado ningun elemento")</h3>
Else
    @<table class="table3">
        <thead>
            <tr>
                <th>@h.traducir("Nombre")</th>
                <th>@h.traducir("Descripción")</th>
                <th>@h.traducir("Lantegi")</th>
                <th></th>
            </tr>
        </thead>
        <tbody>
                @For Each e In Model
                   @<tr>
                       <td>@e.nombre</td>        
                       <td>@e.descripcion</td>        
                        <td>@e.lantegi</td>        
                       <td>
                           <a href="@Url.Action("editcuenta", New With {.idAplicacion = Request("idaplicacion"), .idtipocuenta = Request("idtipocuenta"), .idcuenta = e.id})">@h.traducir("Editar cuenta")</a>
                       </td>
                    </tr>
                Next
        </tbody>
     </table>
End If

