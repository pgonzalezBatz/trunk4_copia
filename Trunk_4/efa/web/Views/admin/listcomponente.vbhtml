@imports web

@section header
    <title>@h.traducir("Repuestos")</title>
End section

@section  beforebody
    <strong><a style="float:left;" href="@Url.Action("index")">@h.traducir("Volver")</a></strong>
    <br style="clear:both;" />
End Section
<a href="@Url.Action("addcomponente")">@h.traducir("Añadir Component")</a>
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
                <a href="@Url.Action("listcomponentemodelo", New With {.idcomponente = i.id})">
                    @h.traducir("Ver modelo")
                </a>
            </td>
            <td>
                <a href="@Url.Action("editcomponente", New With {.idcomponente = i.id})">
                    @h.traducir("Editar")
                </a>
            </td>
            <td>
                <a href="@Url.Action("anularcomponente", New With {.idcomponente = i.id})">
                    @h.traducir("Eliminar")
                </a>
            </td>
        </tr>
        Next
    </tbody>
</table>