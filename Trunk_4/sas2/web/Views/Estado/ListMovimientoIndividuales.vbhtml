@modeltype VMMovimientoFinal
@Code
    Layout = "~/Views/Shared/RMaster.vbhtml"
End Code


<h3>@h.traducir("Listado de Movimientos Individuales")</h3>
<hr />


@If Model.ListOfMarca Is Nothing Then
@h.traducir("No se han encontrado movimientos de marcas pendientes")
Else
@<table class="table">
    <thead>
        <tr>
            <th>@h.traducir("Marca")</th>
            <th>@h.traducir("Cantidad")</th>
            <th>@h.traducir("Peso")</th>
            <th>@h.traducir("Dimensiones")</th>
            <th>@h.traducir("Observación")</th>
            <th>@h.traducir("Articulo")</th>
            <th>@h.traducir("Material")</th>
        </tr>
    </thead>
    <tbody>
        @Html.DisplayFor(Function(m) m.ListOfMarca)
    </tbody>
</table>
End If



