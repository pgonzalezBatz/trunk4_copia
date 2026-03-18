@imports web
@Code
    ViewBag.title = "Inventario"
End Code
    <div id="notifications">
        @h.traducir("Histórico de activos para usuario") @ViewData("usuario").nombre
    </div>

@If Model.count = 0 Then
    @<strong>@h.traducir("El usuario no tiene activos asignados en el histórico")</strong>
Else
    @<table class="table3">
    <thead>
        <tr>
            <th>@h.traducir("Etiqueta")</th>
            <th>@h.traducir("Modelo")</th>
            <th>@h.traducir("Marca")</th>
            <th>@h.traducir("Tipo")</th>
            <th>@h.traducir("Precio")</th>
            <th>@h.traducir("Fecha asignación")</th>
            <th>@h.traducir("Fecha baja")</th>
            <th>@h.traducir("Microsoft OM")</th>
            <th>@h.traducir("Imputacion")</th>
        </tr>
    </thead>
    <tbody>
        @For Each a In Model
             @<tr>
                <td>@a.idEtiqueta</td>
                <td>@a.nombreModelo</td>
                <td>@a.nombreMarca</td>
                <td>@a.nombreTipo</td>
                <td>@a.precioModelo</td>
                <td>@a.fechaAlta.toshortdatestring</td>
                <td>
                    @If IsDate(a.fechaBaja) Then
                            @a.fechaBaja.toshortdatestring()
                        End If
                </td>
                 <td>@a.numeroSerie</td>
                <td>
                    @If a.EsDepartamento Then
                            @h.traducir("Departamento")
                        Else
                            @h.traducir("Usuario")
                        End If
                </td>
             </tr>
                    Next
    </tbody>
</table>
End If


<script type="text/javascript">
    document.getElementById("idetiqueta").focus();
</script>
