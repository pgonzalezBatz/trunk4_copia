@imports web
@Code
    ViewBag.title = "Tipos cuenta"
End code

<a href="@Url.Action("createtipocuenta",New With{.idAplicacion=Request("idaplicacion")})">@h.traducir("Crear nuevo tipo cuenta")</a>
@If Model.count = 0 Then
    @<h3> @h.traducir("No se ha encontrado ningun elemento")</h3>
Else
    @<table class="table3">
        <thead>
            <tr>
                <th>@h.traducir("Id")</th>
                <th>@h.traducir("Nombre")</th>
                <th>@h.traducir("Nº Cuentas sin asignar")</th>
                <th>@h.traducir("Nº departamentos sin asignar")</th>
                <th></th>
                <th></th>
            </tr>
        </thead>
        <tbody>
                @For Each e In Model
                   @<tr>
                       <td>@e.id</td>
                       <td>@e.nombre</td>        
                        <td>@e.cuentassinAsignar</td>        
                        <td>@e.departamentosSinAsignar</td>        
                       <td>
                        <a href="@Url.Action("listcuenta", New With {.idAplicacion = Request("idaplicacion"), .idtipocuenta = e.id})">@h.traducir("ver cuentas")</a>        
                       </td>
                        <td>
                            <a href="@Url.Action("listDepartamentosCuenta", New With {.idAplicacion = Request("idaplicacion"), .idtipocuenta = e.id})">@h.traducir("ver asignaciones")</a>
                        </td>
                    </tr>
                Next
        </tbody>
     </table>
End If