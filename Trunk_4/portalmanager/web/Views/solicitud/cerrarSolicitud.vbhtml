@imports web
@Code
    ViewBag.title = "Cerrar solicitud"
End Code

<h3>
    @h.traducir("Estas a punto de cerrar la solicitud Nº") @Request("idSolicitud")
</h3>

<form action="" method="post">
    <fieldset>
        <legend>@h.traducir("Datos sobre la incorporación")</legend>
        @Html.Hidden("idSolicitud")
<br />
        <strong>@h.traducir("Descripción")</strong>
        <br />
        @Html.TextArea("datosIncorporacion")
        <br />
        
        <input type="submit" value="@h.traducir("Confirmar cierre")" />
        @Html.ActionLink(h.traducir("Volver al listado"), "index")
    </fieldset>
</form>
    