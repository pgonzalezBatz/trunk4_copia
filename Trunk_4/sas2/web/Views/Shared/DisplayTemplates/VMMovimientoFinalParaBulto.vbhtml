@modeltype VMMovimientoFinal




<td rowspan="@(Model.ListOfMarca.Count() + 1 )">@Html.DisplayFor(Function(m) m.Fecha)</td>
<td rowspan="@(Model.ListOfMarca.Count() + 1 )">@Html.DisplayFor(Function(o) o.VectorMovimiento, New With {.InsideTable = True, .grouped = True})    </td>
<td rowspan="@(Model.ListOfMarca.Count() + 1 )">@Model.Numord - @Model.Numope</td>
<td rowspan="@(Model.ListOfMarca.Count() + 1 )">@Html.DisplayFor(Function(m) m.Creador)</td>
<td colspan="7">
    @Html.HiddenFor(Function(m) m.Fecha)
    @Html.HiddenFor(Function(m) m.Numord)
    @Html.HiddenFor(Function(m) m.Numope)
</td>
@For Each e In Model.ListOfMarca
    @Html.DisplayFor(Function(m) e, "VMMarcaParaBulto")
Next
