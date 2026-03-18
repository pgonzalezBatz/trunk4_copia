@Modeltype web.grupo
@imports web

@section header
    <title>@h.traducir("Administrar grupos")</title>
End section

@section  beforebody
    <strong><a href="@Url.Action("index")">@h.traducir("Volver")</a></strong>
    <br style="clear:both;" />
End Section

@Html.ValidationSummary()
<form action="" method="post" enctype="multipart/form-data">
    <fieldset>
        <legend>@h.Traducir("Añadir Grupo")  </legend>
        <label for="nombre">@h.Traducir("Nombre")  </label> <br />
        @Html.TextBoxFor(Function(m) m.Nombre)<br />
        <label for="imageupload">@h.Traducir("Imagen")  </label>
        <span style="font-size:9px; color:Red;">(@h.Traducir("Se recomienda que las imagenes sean de 115x115 pixels y en formato png"))</span><br />
        <input type="file" id="imageupload" name="imageupload" /> <br />
        <input type="submit" value="@h.Traducir(" guardar") " />
    </fieldset>
</form>

<table class="table1">
    <thead>
        <tr>
            <th>@h.Traducir("Grupo")</th>
            <th>@h.Traducir("Acciones")</th>
        </tr>
    </thead>
    <tbody>
        @For Each g As Grupo In ViewData("listofgroup")
        @<tr>
            <td><a href="@Url.Action("recurso", New With {.grupo = g.Nombre})">@g.Nombre </a></td>
            <td>
                <a href="@Url.Action("editargrupo", New With {.grupo = g.Nombre})">@h.Traducir("Cambiar imagen")</a>
                |
                <a href="@Url.Action("eliminargrupo", New With {.grupo = g.Nombre})">@h.Traducir("Eliminar grupo")  </a>
            </td>
        </tr>
        Next
    </tbody>
</table>