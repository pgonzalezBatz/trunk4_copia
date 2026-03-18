@imports web

@section header
    <title>@h.traducir("Exportar datos para la nomina")</title>
End section

<form action="@url.action("list")" method="get">
    <fieldset>
        <legend>@h.traducir("Datos para exportar")</legend>
        <strong>@h.traducir("Mes")</strong><br />
        @Html.DropDownList("yearmonth")<br />
        <strong>@h.traducir("Nº trabajador desde")</strong><br />
        @Html.TextBox("desde")<br />
        <strong>@h.traducir("Hasta")</strong><br />
        @Html.TextBox("hasta")<br />
        <input type="submit" value="@h.traducir("exportar")" />
    </fieldset>
</form>