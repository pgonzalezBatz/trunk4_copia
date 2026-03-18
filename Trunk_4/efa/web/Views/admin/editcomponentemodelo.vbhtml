@imports web

@section header
    <title>@h.traducir("Añadir o editar modelo de componente")</title>
End section
@section  beforebody
<a style="float:left;" href="@Url.Action("listcomponentemodelo", h.ToRouteValuesDelete(Request.QueryString, "idcomponentemodelo"))">@h.traducir("volver")</a>
<br style="clear:both;" />
End Section
<strong>@h.traducir("Modelo seleccionado")</strong><br />
    @ViewData("componente").nombre  @ViewData("componente").serie <br />
    @Html.ValidationSummary()
<form action="" method="post">
    <fieldset>
        <legend>@h.traducir("Introducir modelo de componente")</legend>
        <label>@h.traducir("Nombre")</label><br />
        @Html.TextBox("pn")<br />
        <label>@h.traducir("Descripcion")</label><br />
        @Html.TextBox("descripcion")<br />
        <label>@h.traducir("Stock actual")</label><br />
        @Html.TextBox("nElementos")<br />
        <label>@h.traducir("Hypervínculo")</label><br />
        @Html.TextBox("networkpath")<br />
        <input type="submit" value="@h.traducir(" guardar")" />
    </fieldset>
</form>