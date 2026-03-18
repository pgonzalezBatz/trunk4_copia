@imports web
@Code
    ViewBag.title = "Departamentos cuentas"
End code

<a href="@Url.Action("listtipocuenta", New With{.idAplicacion=Request("idAplicacion")})">@h.traducir("Volver")</a>
@If Model.count = 0 Then
    @<h3> @h.traducir("No se ha encontrado ningun elemento")</h3>
Else
    @<form action="" method="post">
    <table class="table3">
        <thead>
            <tr>
                <th>@h.traducir("Negocio")</th>
                <th>@h.traducir("Mano de obra")</th>
                <th>@h.traducir("Departamento")</th>
                @For Each tc In ViewData("lsttipocuenta")
                        @<th>@tc.nombre</th>
                    Next
                <th colspan="2"></th>
            </tr>
        </thead>
        <tbody>
            @For Each n In Model
                    Dim drawn = True
                    @For Each mo In n.listofmanoobra
                            Dim drawmo = True
                            @For Each d In mo.listOfDepartamento
                                    @<tr>
                                        @If drawn Then
                                            @<td rowspan="@n.rowspan "> @n.nombreNegocio</td>                
                                            End If
                                        @If drawmo Then
                                            @<td rowspan="@mo.listOfDepartamento.Count">@mo.nombreManoObra</td>        
                                            End If
                                        
                                        <td>@d.nombredepartamento</td>
                                        @For Each tc In d.listofcuentacuenta
                                            @<td>
                                                <select name="lstCuentaDepartamento">
                                                    <option value="">@h.traducir("Seleccionar")</option>
                                                @For Each c In tc
                                                        If c.selected Then
                                                            @<option value="@d.idDepartamento,@c.id" selected="selected">@c.nombre - @c.descripcion</option>
                                                Else
                                                    @<option value="@d.idDepartamento,@c.id">@c.nombre - @c.descripcion</option>
                                                End If
                                                     
                                                    Next
                                                </select>
                                             </td>
                                                    Next
                                        <td>
                                            
                                        </td>
                                     </tr>
                                drawn = False
                                drawmo = False
                                Next
                            Next
                    Next

        </tbody>
     </table>
        <input type="submit" value="@h.traducir("Guardar")" />
        </form>
    
End If

