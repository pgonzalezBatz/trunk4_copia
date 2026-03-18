@Modeltype web.Grupo
@imports web

@section header
    <title>@h.traducir("Eliminar Grupo")</title>
End section
@section  beforebody
<strong><a href="@Url.Action("grupo")">@h.Traducir("Volver")</a></strong>
<br style="clear:both;" />
End Section

@Html.ValidationSummary()
<h3>Estas seguro de que quieres eliminar esto?</h3>
<form action="@Url.Action("eliminargrupo")" method="post">
    <fieldset>
        <legend>Datos del recurso</legend>

        <label>@h.Traducir("Nombre de grupo")  </label>
        @Html.HiddenFor(Function(m) m.Nombre)
        @Html.DisplayTextFor(Function(m) m.Nombre)<br />
    </fieldset>
    <span class="cancelanchor">@Html.ActionLink("Volver", "grupo")</span>
    <input type="submit" value="@h.Traducir("eliminar")  " />
</form>