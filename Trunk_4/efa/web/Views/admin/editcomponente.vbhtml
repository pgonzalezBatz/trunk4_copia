@imports web

@section header
    <title>@h.traducir("Añadir o editar Componenter")</title>
End section
@section  beforebody
<a style="float:left;" href="@Url.Action("listcomponente")">@h.traducir("volver")</a>
<br style="clear:both;" />
End Section
@Html.ValidationSummary()
<form action="" method="post">
    <fieldset>
        <legend>@h.traducir("Introducir datos del componente")</legend>
        <label>@h.traducir("Nombre")</label><br />
        @Html.TextBox("nombre")<br />
        <label>@h.traducir("Serie")</label><br />
        @Html.TextBox("serie")<br />
        <input type="submit" value="@h.traducir(" guardar")" />
    </fieldset>
</form>