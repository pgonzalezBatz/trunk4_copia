@modeltype VMMarca

<tr>
    <td>
        @Html.EditorFor(Function(m) m.Seleccionado)
        @Html.HiddenFor(Function(m) m.Id)
        @Html.HiddenFor(Function(m) m.Peso)
        @Html.HiddenFor(Function(m) m.Alto)
        @Html.HiddenFor(Function(m) m.Ancho)
        @Html.HiddenFor(Function(m) m.Largo)
        @Html.HiddenFor(Function(m) m.Marca)
        @Html.HiddenFor(Function(m) m.Material)
        @Html.HiddenFor(Function(m) m.Observacion)
        @Html.HiddenFor(Function(m) m.Cantidad)
        @Model.Marca
    </td>
    <td>@Model.Cantidad</td>
    <td>@Model.Peso</td>
    <td>
        @Model.Largo X         @Model.Ancho X         @Model.Alto
    </td>
    <td>@Model.Observacion</td>
    <td>@Model.Articulo</td>
    <td>@Model.Material</td>
</tr>