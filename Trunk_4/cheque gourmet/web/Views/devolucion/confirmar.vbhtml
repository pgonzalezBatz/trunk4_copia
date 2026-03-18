@imports web

@section header
    <title>@h.traducir("Confirmación de devolución")</title>
End section

<form action="@Url.Action("guardar")" method="post">
    <fieldset>
        <legend>@h.traducir("Confirmar devolución")</legend>

        <strong>@h.traducir("Código de trabajador")</strong><br />
        <input type="hidden" name="codtra" value="<%=Model.codtra%>" />
        @Model.codtra<br />
        <strong>@h.traducir("Nombre")</strong><br />
        @Model.nombre @Model.apellido1 @Model.apellido2<br />
        <strong>@h.traducir("Numero de cheques")</strong><br />
        <input type="hidden" name="numerocheques" value="@ViewData("numerocheques")" />
        @ViewData("numerocheques")<br />
        <strong>@h.traducir("Tipo de talonario")</strong><br />
        <input type="hidden" name="tipo" value="@model.tipo" />
        @Model.nombretipo<br />
        <input type="submit" value="@h.traducir("confirmar")" />
    </fieldset>
</form>