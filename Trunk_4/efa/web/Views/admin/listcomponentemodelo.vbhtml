@imports web

@section header
    <title>@h.traducir("Modelos de componente")</title>
End section

@section  beforebody
    <strong><a style="float:left;" href="@Url.Action("listcomponente")">@h.traducir("Volver")</a></strong>
    <br style="clear:both;" />
End Section

<strong>@h.traducir("Componente seleccionado")</strong><br />
    @ViewData("componente").nombre  @ViewData("componente").serie <br />
<a href="@Url.Action("addcomponentemodelo", New With {.idComponente = ViewData("componente").id})">@h.traducir("Añadir modelo de componente")</a>
<table class="table2">
    <thead>
        <tr>
            <th>@h.traducir("nombre")</th>
            <th>@h.traducir("comentarios")</th>
            <th>@h.traducir("stock actual")</th>
            <th>@h.traducir("hypervinculo")</th>
            <th colspan="2"></th>
        </tr>
    </thead>
    <tbody>
        @For Each i In Model
        @<tr>
            <td>@i.pn</td>
            <td>@i.descripcion</td>
            <td>@i.nelementos</td>
            <td>

            <a href="file:///@i.networkpath.trim(" ")">@i.networkpath
                
                </a>
            </td>
            <td>
                <a href="@Url.Action("editcomponentemodelo", New With {.idcomponente = ViewData("componente").id, .idcomponentemodelo = i.id})">
                    @h.traducir("Editar")
                </a>
            </td>
            <td>
                <a href="@Url.Action("anularcomponentemodelo", New With {.idcomponente = ViewData("componente").id, .idcomponentemodelo = i.id})">
                    @h.traducir("Eliminar")
                </a>
            </td>
        </tr>
        Next
    </tbody>
</table>