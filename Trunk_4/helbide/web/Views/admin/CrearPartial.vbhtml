@ModelType web.Helbide
@imports web
<form action="/helbide/admin/crear" method="post">
    @Html.ValidationSummary()
            <input type="hidden" name="direccionRetorno" value="@(If(Request.UrlReferrer Is Nothing, " ", Request.UrlReferrer.AbsoluteUri))" />
            <strong>@h.traducir("Nº, calle")</strong><br />
            @Html.EditorFor(Function(m) m.Calle)<br />
            <strong>@h.traducir("Código postal")</strong><br />
            @Html.EditorFor(Function(m) m.CodigoPostal)<br />
            <strong>@h.traducir("Población")</strong><br />
            @Html.EditorFor(Function(m) m.Poblacion)<br />
            <strong>@h.traducir("Provincia")</strong><br />
            @Html.EditorFor(Function(m) m.Provincia)<br />
            <strong>@h.traducir("Pais")</strong><br />
            @Html.DropDownListFor(Function(m) m.Pais, ViewData("listOfPais"))<br /><br />
            <input type="submit" value="@h.Traducir(" Guardar")" />
</form>
