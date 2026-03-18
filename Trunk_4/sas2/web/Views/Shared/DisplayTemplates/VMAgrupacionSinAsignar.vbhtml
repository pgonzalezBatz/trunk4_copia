@modeltype VMAgrupacion


<td rowspan="@(Model.ListOfMovimiento.Sum(Function(m) m.ListOfMarca.Count) + 1 )">
    @Html.EditorFor(Function(m) m.Seleccionado)
    @Html.HiddenFor(Function(m) m.Id)
    @Model.Id
</td>
<td rowspan="@(Model.ListOfMovimiento.Sum(Function(m) m.ListOfMarca.Count) + 1)">@Model.Peso</td>
@For Each a In Model.ListOfMovimiento
    @Html.DisplayFor(Function(m) a, "VMMovimientoFinalParaBulto")
Next
