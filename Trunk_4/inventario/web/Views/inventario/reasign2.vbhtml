@imports web
@Code
    ViewBag.title = h.traducir("Reasignar elementos de usuari@ de baja a otr@")
    
End Code
    <h3>@h.traducir("Reasingar etiquetas")</h3>
<form action="ReasignarContenidoUsuario2" method="post">
    @For Each e In ViewData("etiquetas")
        @<input type="hidden" name="etiquetas" value="@e.idetiqueta" />
    @Html.Encode("*")
    @e.nombreModelo @Html.Encode(" ") @<i>(@e.fechaalta)</i>
    @<br />
    Next
    <br />

    @h.traducir("De")                                @Html.Encode(" ")
    @ViewData("usuarioBaja").nombre @Html.Encode(" ")
    @h.traducir("a")                             @Html.Encode(" ")
    @ViewData("usuario").nombre

    @Html.Hidden("idsab")
    @Html.Hidden("idsabbaja")
    <br />
    @Html.Hidden("liberar")
    <input type="submit" value="@h.traducir("Guardar")" />
</form>
<script type="text/javascript">
    document.getElementById("user").focus();
</script>
