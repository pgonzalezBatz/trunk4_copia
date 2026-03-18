@imports web
@section header
    <title>@h.traducir("Anular Impresoras de toner")</title>
End section
@section  beforebody
    <a style="float:left;" href="@Url.Action("listtonerimpresora")">@h.traducir("volver")</a>
    <br style="clear:both;" />
End Section

<strong>@h.traducir("Eliminar la impresora Y todos los colores de esta?")</strong>
<form action="" method="post">
    <fieldset>
        <legend></legend>
        <label>@h.traducir("Nombre impresora")</label><br />
        @Html.Hidden("nombre")
        @Model.nombre<br />
        <label>@h.traducir("Serie")</label><br />
        @Html.Hidden("serie")
        @Model.serie<br />
        <a href="@Url.Action("listtonerimpresora")">@h.traducir("Volver sin eliminar")</a>
        <input type="submit" value="@h.traducir(" eliminar")" />
    </fieldset>
</form>