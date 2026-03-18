@imports web
@section header
    <title>@h.traducir("Anular Componente")</title>
End section
@section  beforebody
    <a style="float:left;" href="@Url.Action("listcomponentemodelo", h.ToRouteValuesDelete(Request.QueryString, "idcomponentemodelo"))">@h.traducir("volver")</a>
    <br style="clear:both;" />
End Section

<strong>@h.traducir("Eliminar El modelo del componente") @ViewData("componente").nombre  @ViewData("componente").serie @Html.Encode("?")</strong>
<form action="" method="post">
    <fieldset>
        <legend></legend>
        <label>@h.traducir("Nombre")</label><br />
        @Html.Hidden("pn")
        @Model.pn<br />
        <label>@h.traducir("Descripcion")</label><br />
        @Html.Hidden("descripcion")
        @Model.descripcion<br />
        <label>@h.traducir("Stock")</label><br />
        @Html.Hidden("nElementos")
        @Model.nElementos<br />
        <a href="@Url.Action("listcomponentemodelo", h.ToRouteValuesDelete(Request.QueryString, "idcomponentemodelo"))">@h.traducir("Volver sin eliminar")</a>
        <input type="submit" value="@h.traducir(" eliminar")" />
    </fieldset>
</form>