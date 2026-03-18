@imports web
@Code
    ViewBag.title = h.traducir("Asientos mensuales")
End code

@If Model.count = 0 Then
    @<h3> @h.traducir("No se ha encontrado ningun elemento")</h3>
Else
    @For Each tp In Model
             @<table class="table3">
        <thead>
            <tr>
                <th>@h.traducir("Departamento")</th>
                <th>@h.traducir("Cuenta")</th>
                <th>@h.traducir("Devengo")</th>
            </tr>
        </thead>
        <tbody>
            @For Each e In tp
                @For Each i In e.listOfManoObra
                        @For Each d In i.listOfDepartamento
                    @<tr>
                         <td>@d.nombreDepartamento</td>
                         <td>@d.descripcionCuenta</td>
                        <td>@d.devengo</td>
                    </tr>            
        Next
                    
                Next
                @<tr>
                    <th>@e.nombreNegocio</th>
                    <td></td>
                    <td >
                        @e.devengo
                    </td>
                </tr>
            Next
        </tbody>
    </table>
        Next
   
End If
