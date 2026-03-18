@imports web

@section header
    <title>@h.traducir("Impresoras de toner")</title>
End section

@section  beforebody
    <strong><a style="float:left;" href="@Url.Action("index")">@h.traducir("Volver")</a></strong>
    <br style="clear:both;" />
End Section
<a href="@Url.Action("addtonerimpresora")">@h.traducir("Add Impresora")</a>
<table class="table2">
    <thead>
        <tr>
            <th>@h.traducir("Id")</th>
            <th>@h.traducir("Nombre")</th>
            <th>@h.traducir("Serie")</th>
            <th colspan="3"></th>
        </tr>
    </thead>
    <tbody>
        @For Each i In Model
        @<tr>
            <td>@i.id</td>
            <td>@i.Nombre</td>
            <td>@i.serie</td>
            <td>
                <a href="@Url.Action("listtonercolor", New With {.idimpresora = i.id})">
                    @h.traducir("Ver colores")
                </a>
            </td>
            <td>
                <a href="@Url.Action("edittonerimpresora", new with {.idimpresora=i.id})">
                    @h.traducir("Editar")
                </a>
            </td>
            <td>
                <a href="@Url.Action("anulartonerimpresora", new with {.idimpresora=i.id})">
                    @h.traducir("Eliminar")
                </a>
            </td>
        </tr>
        Next
    </tbody>
</table>