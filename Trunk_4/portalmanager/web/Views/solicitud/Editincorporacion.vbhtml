@imports web
@Code
    ViewBag.title = "Editar incorporación"
End Code

<h3>
    @H.Traducir("Estas a punto de editar la solicitud Nº") @Request("idSolicitud")
</h3>

<form action="" method="post">
    <fieldset>
        <legend>@H.Traducir("Datos sobre la incorporación")</legend>
        @Html.Hidden("idSolicitud")
        <br />
        <strong>@H.Traducir("Descripción")</strong>
        <br />
        @Html.TextArea("datos_incorporacion")
        <br />

        <input type="submit" value="@H.Traducir("Guardar cambios")" />
        @Html.ActionLink(H.Traducir("Volver al listado"), "index")
    </fieldset>
</form>
