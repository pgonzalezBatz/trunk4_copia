@modeltype VMMovimientoFinal

@If ViewData("grouped") Then
@<tr>
    <td>
        <a href="@Url.Action("ListMovimientosRecogidasSinAsignarAgrupado", h.ToRouteValues(Request.QueryString, New With {.fecha = Model.Fecha.Ticks}))">
            @Html.DisplayFor(Function(o) o.Fecha)
        </a>
    </td>
    @Html.DisplayFor(Function(o) o.VectorMovimiento, New With {.InsideTable = True})
    <td>@Model.Numord - @Model.Numope</td>
    <td>@Model.CalculateCantidad()</td>
    <td>@Model.CalculatePeso()</td>
    <td>
        <a Class="btn btn-info" href="@Url.Action("ListMovimientoIndividuales", New With {.fechaTicks = Model.Fecha.Ticks, .Numord = Model.Numord, .Numope = Model.Numope,
.IdEmpresaOrigen = Model.VectorMovimiento.PuntoOrigen.IdEmpresa,
.IdEmpresaDestino = Model.VectorMovimiento.PuntoDestino.IdEmpresa,
.IdHelbideOrigen = Model.VectorMovimiento.PuntoOrigen.IdHelbide,
.IdHelbideDestino = Model.VectorMovimiento.PuntoDestino.IdHelbide})">
            <span Class="glyphicon glyphicon-eye-open" aria-hidden="true"></span>
            @h.traducir("Ver")
        </a>
    </td>
</tr>


Else


    @<tr>
    <td rowspan="@(Model.ListOfMarca.Count() + 1 )">@Html.DisplayFor(Function(m) m.Fecha)</td>
    <td rowspan="@(Model.ListOfMarca.Count() + 1 )">@Html.DisplayFor(Function(o) o.VectorMovimiento, New With {.InsideTable = True})    </td>
    <td rowspan="@(Model.ListOfMarca.Count() + 1 )">@Model.Numord - @Model.Numope</td>
    <td rowspan="@(Model.ListOfMarca.Count() + 1 )">@Html.DisplayFor(Function(m) m.Creador)</td>
    <td colspan = "7" >
        @Html.HiddenFor(Function(m) m.Fecha)
        @Html.HiddenFor(Function(m) m.Numord)
        @Html.HiddenFor(Function(m) m.Numope)
    </td>
</tr>
        @Html.DisplayFor(Function(m) m.ListOfMarca)
End If
