@imports web
@Code
    ViewBag.title = "Negocios"
End code

<a href="@Url.Action("createnegocioadministracion")">@h.traducir("Crear nuevo negocio")</a> |
<a href="@Url.Action("createmanoobraadministracion")">@h.traducir("Crear nueva mano de obra")</a>
@If Model.count = 0 Then
    @<h3> @h.traducir("No se ha encontrado ningun elemento")</h3>
Else
   @<table class="table3">
    <thead>
        <tr>
            <th colspan="2">@h.traducir("Organigrama")</th>
            <th>@h.traducir("Lantegi")</th>
            <th>@h.traducir("Nº personas segun mapeos")</th>
            <th>@h.traducir("Acciones")</th>
        </tr>
    </thead>
        <tbody>
            @For Each n In Model
                 @code 
                     Dim colspan = n.ndepartamentos + n.lstMO.count + 1
                 End code   
                
                @For Each mo In n.lstMO
                        @For Each d In mo.lstDepartamento
                                    @<tr>
                                        @If colspan > 0 Then
@<th rowspan="@colspan">@n.nombreNegocio</th>                                                
                                            colspan=0
                                            End If
                                        <td>@html.Encode(" ")@Html.Encode(" ") @d.nombre</td>
                                         <td>@d.lantegi</td>
                                         <td>@d.nPersonas</td>
                                         <td>
                                             <a href="@Url.Action("editdepartamentoadministracion", New With {.idnegocio = n.idNegocio, .idmanoobra = mo.idMO, .iddepartamento = d.id})">@h.traducir("Editar")</a>
                                         </td>
                                    </tr>    
                        Next
                        @<tr>
                             @If colspan > 0 Then
                                 @<th rowspan="@colspan">@n.nombreNegocio</th>
                                 colspan = 0
                             End If
                            <th >@Html.Encode(" ")@mo.nombreMO</th>
                             <th></th>
                             <th>@mo.npersonas</th>
                             <th>
                                 <a href="@Url.Action("createDepartamentoAdministracion", New With {.idNegocio = n.idNegocio, .idManoObra = mo.idMO})">@h.traducir("Crear departamento")</a>
                             </th>
                        </tr>
                Next
                @<tr>
                     <th>@h.traducir("TOTAL")</th>
                             <th></th>
                             <th>@n.npersonas</th>
                             <th></th>
                        </tr>
            Next
            <tr>
                <th>Batz S.COOP.</th>
                <th>@h.traducir("TOTAL")</th>
                <th></th>
                <th>@ViewData("npersonas")</th>
                <th></th>
            </tr>
        </tbody>
    </table>
    
End If

