@modeltype VMAgruparMovimiento
@Code
    Layout = "~/Views/Shared/RMaster.vbhtml"
End Code


<h3>@h.traducir("Agrupar Movimientos / Recogidas")    </h3>
<hr />


<form action="@Url.Action("AgruparMovimientoSave")" method="post">
    @Html.ValidationSummary()
    @If Model.LstMovimientoProveedorFecha Is Nothing Then
        @h.traducir("No se han encontrado movimientos de marcas pendientes")
    Else

        @<table class="table">
            <thead>
                <tr>
                    <th>@h.traducir("Fecha")</th>
                    <th>@h.traducir("Origen")</th>
                    <th>@h.traducir("Destino")</th>
                    <th>@h.traducir("OF-OP")</th>
                    <th>@h.traducir("Marca")</th>
                    <th>@h.traducir("Cantidad")</th>
                    <th>@h.traducir("Peso")</th>
                    <th>@h.traducir("Dimensiones")</th>
                    <th>@h.traducir("Observaciones")</th>
                    <th>@h.traducir("Articulo")</th>
                    <th>@h.traducir("Accion")</th>
                </tr>
            </thead>
            @Html.DisplayFor(Function(m) m.LstMovimientoProveedorFecha)
        </table>
    End If

    @Html.Label(h.traducir("Peso"))
    @Html.TextBoxFor(Function(m) m.Peso, New With {.class = "form-control"})

    <input type="submit" class="btn btn-primary" value="@h.traducir("Agrupar")" />
</form>