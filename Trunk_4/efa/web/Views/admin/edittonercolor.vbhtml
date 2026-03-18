@imports web

@section header
    <title>@h.traducir("Añadir o editar Color de toner")</title>
End section
@section  beforebody
<a style="float:left;" href="@Url.Action("listtonercolor", New With {.idimpresora = ViewData("tonerimpresora").id})">@h.traducir("volver")</a>
<br style="clear:both;" />
End Section
<strong>@h.traducir("Impresora seleccionada")</strong><br />
    @ViewData("tonerimpresora").nombre @ViewData("tonerimpresora").serie
    @Html.ValidationSummary()
<form action="" method="post">
    <fieldset>
        <legend>@h.traducir("Introducir datos del color")</legend>
        <label>@h.traducir("Identificador color")</label><br />
        @Html.TextBox("idcolor")<br />
        <label>@h.traducir("Color")</label><br />
        @Html.TextBox("color")<br />
        <label>@h.traducir("Stock actual")</label><br />
        @Html.TextBox("stock")<br />
        <label>@h.traducir("Stock Minimo")</label><br />
        @Html.TextBox("stockMinimo")<br />
        <input type="submit" value="@h.traducir(" guardar")" />
    </fieldset>
</form>