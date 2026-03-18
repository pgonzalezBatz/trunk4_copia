@imports web
@section header
    <title>@h.traducir("Impresoras de toner")</title>
End section
@section  beforebody
    <a style="float:left;" href="@Url.Action("listtonercolor", New With {.idimpresora = ViewData("tonerimpresora").id})">@h.traducir("volver")</a>
    <br style="clear:both;" />
End Section

<strong>@h.traducir("Impresora seleccionada")</strong><br />
    @ViewData("tonerimpresora").nombre @ViewData("tonerimpresora").serie<br />
<strong>@h.traducir("Eliminar el color de toner?")</strong><br />
<form action="" method="post">
    <fieldset>
        <legend></legend>
        <label>@h.traducir("Identificador color")</label><br />
        @Html.Hidden("idcolor")
        @Model.idcolor<br />
        <label>@h.traducir("Color")</label><br />
        @Html.Hidden("color")
        @Model.color<br />
        <label>@h.traducir("Stock actual")</label><br />
        @Html.Hidden("stock")
        @Model.stock<br />
        <label>@h.traducir("Stock minimo")</label><br />
        @Html.Hidden("stockminimo")
        @Model.stockminimo<br />
        <a href="@Url.Action("listtonercolor", New With {.idimpresora = ViewData("tonerimpresora").id})"> @h.traducir("Volver sin eliminar")</a>
        <input type="submit" value="@h.traducir(" eliminar")" />
    </fieldset>
</form>