@imports web
@Code
    ViewBag.title = h.traducir("Relaciones de departamentos RRHH Administracion") 
End code

@If Model.count = 0 Then
    @<h3> @h.traducir("No se ha encontrado ningun elemento")</h3>
Else
    @<form action="" method="post">
    <table class="table3">
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
                            <select name="lstMapeo">
                                <option value="" style="background-color:#FF8080;"> @h.traducir("Seleccionar")</option>
                                @For Each n In ViewData("lstnegocio")
                                        For Each mo In n.lstManoObra
                                                    @<optgroup label="@n.nombrenegocio - @mo.nombreManoObra">
                                                    @For Each d In mo.lstDepartamento
                                                            If e.relacion IsNot Nothing AndAlso e.relacion.idadministracion = d.id Then
                                                                @<option value="@e.id,@d.id" selected="selected">@d.nombre</option>        
                                                                Else
                                                                @<option value="@e.id,@d.id">@d.nombre</option>        
                                                                End If
                                                    
                                                Next
                                                </optgroup>    
                                            
                                Next
                            Next
                            </select>
                       </td>
                    </tr>
                                Next
        </tbody>
     </table>
        <input type="submit" value="@h.traducir("Guardar")" />
        </form>
End If

