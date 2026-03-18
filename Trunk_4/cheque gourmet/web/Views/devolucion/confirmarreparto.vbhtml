@imports web

@section header
    <title>@h.traducir("Reparto de devoluciones")</title>
End section

<form action="@Url.Action("guardarreparto", New With {.id = Request("id")})" method="post">
    <fieldset>
        <legend>@h.traducir("Confirmar reparto")</legend>
        <strong>@h.traducir("Código de trabajador")</strong><br />
        @Model.codtra<br />
        <strong>@h.traducir("Nombre de trabajador")</strong><br />
        @Model.nombre @Model.apellido1 @Model.apellido2<br />
        <strong>@h.traducir("Numero de cheques")</strong><br />
        @Model.numerocheques<br />
        <strong>@h.traducir("Fecha de devolución")</strong><br />
        @Model.fecha<br />
        <strong>@h.traducir("Confirma el reparto de cheques")</strong><br />
        <input type="submit" value="@h.traducir("confirmar devolución")" />
    </fieldset>
</form>