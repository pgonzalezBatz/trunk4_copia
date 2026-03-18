@imports web
@Code
    ViewBag.title = "Manos de obra"
End code

<a href="@Url.Action("createmanoobraadministracion")">@h.traducir("Crear nueva mano de obra")</a> @h.traducir("para el negocio de") <strong>@ViewData("negocio").nombre</strong>
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
                @For Each mo In Model
                   @<tr>
                       <td>@mo.id</td>
                       <td>@mo.nombre</td>        
                       <td>
                           <a href="@Url.Action("listdepartamentosadministracion", New With {.idnegocio = Request("idnegocio"), .idmanoobra = mo.id})">@h.traducir("ver departamentos") @mo.nombre</a>        
                       </td>
                    </tr>
                Next
        </tbody>
     </table>
End If

