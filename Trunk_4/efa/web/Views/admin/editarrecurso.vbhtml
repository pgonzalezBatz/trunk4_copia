@Modeltype web.recurso
@imports web

@section header
    <title>@h.traducir("Editar Recurso")</title>
End section
@section  beforebody
<a style="float:left;" href="@Url.Action("recurso", New With {.grupo = Model.NombreGrupo})">@h.Traducir("volver")</a>
<br style="clear:both;" />
End Section

<h3>
    @h.Traducir("Cambiar imagen")
</h3>
@Html.ValidationSummary()
<form method="post">
    <fieldset>
        <legend>@h.Traducir("Editar descripción")  </legend>
        @Html.HiddenFor(Function(m) m.NombreGrupo)
        <label>@h.Traducir("Identificador del recurso")  </label><br />
        @Html.HiddenFor(Function(m) m.Id)
        @Model.Id<br />
        <label>@h.Traducir("Identificador de Planta")  </label><br />
        @Html.HiddenFor(Function(m) m.Planta)
        @Model.Planta<br />
        <label for="descripcion">@h.Traducir("Descripción")  </label><br />
        @Html.TextArea("descripcion")<br />
        <input type="submit" value="@h.Traducir(" guardar")" />
    </fieldset>
</form>