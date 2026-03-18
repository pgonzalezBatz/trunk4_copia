@imports web

@section header
    <title>@h.traducir("Colores de toner")</title>
End section

@section  beforebody
    <strong><a style="float:left;" href="@Url.Action("listtonerimpresora")">@h.traducir("Volver")</a></strong>
    <br style="clear:both;" />
End Section

<strong>@h.traducir("Impresora seleccionada")</strong><br />
    @ViewData("tonerimpresora").nombre  @ViewData("tonerimpresora").serie <br />
<a href="@Url.Action("addtonercolor", New With {.idimpresora = ViewData("tonerimpresora").id})">@h.traducir("Añadir color de toner")</a>
<table class="table2">
    <thead>
        <tr>
            <th>@h.traducir("Identificador color")</th>
            <th>@h.traducir("color")</th>
            <th>@h.traducir("stock actual")</th>
            <th>@h.traducir("stock minimo")</th>
            <th colspan="2"></th>
        </tr>
    </thead>
    <tbody>
        @For Each i In Model
        @<tr>
            <td>@i.idcolor</td>
            <td>@i.color</td>
            <td>@i.stock</td>
            <td>@i.stockMinimo</td>
            <td>
                <a href="@Url.Action("edittonercolor", New With {.idimpresora = i.idImpresora, .idcolor = i.idcolor})">
                    @h.traducir("Editar")
                </a>
            </td>
            <td>
                <a href="@Url.Action("anulartonercolor", new with {.idimpresora=i.idImpresora, .idcolor=i.idcolor})">
                    @h.traducir("Eliminar")
                </a>
            </td>
        </tr>
        Next
    </tbody>
</table>