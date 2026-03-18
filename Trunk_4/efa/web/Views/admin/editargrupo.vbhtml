@Modeltype web.Grupo
@imports web

@section header
    <title>@h.traducir("Editar Grupo")</title>
End section
@section  beforebody
<a style="float:left;" href="@Url.Action("grupo")">@h.Traducir("volver")</a>
<br style="clear:both;" />
End Section

<h3>
    @h.Traducir("Cambiar imagen")
</h3>
<form method="post" enctype="multipart/form-data">
    <fieldset>
        <label>@h.Traducir("Imagen actual")  </label><br />
        <img src="@Url.Action("imagegrupo", "recurso", New With {.nombregrupo = Model.Nombre})" alt="@h.Traducir("Imagen del grupo") " /><br />
        @Html.HiddenFor(Function(m) m.Nombre)
        <label for="imageupload">@h.Traducir("Cambiar Imagen")  </label>
        <span style="font-size:9px; color:Red;">(@h.Traducir("Se recomienda que las imagenes sean de 115x115 pixels y en formato png"))</span><br />
        <input type="file" id="imageupload" name="imageupload" /> <br />
        <input type="submit" value="@h.traducir("guardar") " />
    </fieldset>
</form>