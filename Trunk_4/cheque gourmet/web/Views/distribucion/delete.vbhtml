@imports web
@section header
    <title>@h.traducir("Confirmación para eliminar")</title>
End section

<strong>@h.traducir("Confirmas que quieres eliminar la distribución que se muestra a continuación?")</strong><br />
<form method="post" action="@url.action("deletepost")">
    <fieldset>
        <legend>@h.traducir("Distribución")</legend>
        <strong>@h.traducir("Nombre"):</strong><br />
        @Model.Nombre @Model.apellido1 @Model.apellido2<br />
        <strong>@h.traducir("Hora"):</strong><br />
        @Model.hora<br />
        <strong>@h.traducir("Tipo"):</strong><br />
        @Model.tipo<br />
        <strong>@h.traducir("Precio"):</strong><br />
        @CDec(Model.precio).ToString("C")<br />
        @Html.Hidden("desde", Model.desde)
        @Html.Hidden("hasta", Model.desde)
        @Html.Hidden("codtra", Model.desde)
        <input type="submit" value="@h.traducir("eliminar")" />
    </fieldset>
</form>