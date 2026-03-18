@imports web

@section header
    <title>@h.traducir("Confirmación")</title>
<style type="text/css">
    #contenido1 {
        text-align: left;
    }
</style>
End section
@section  beforebody
<strong><a href="@Url.Action("elegiraccion", New With {.idsab = Request("idSab")})">@h.traducir("Volver") </a></strong>
End Section

<br />
<strong>@h.traducir("La acción se ha efectuado con éxito") </strong><br /><br />
<a href="@Url.Action("elegiraccion", New With {.idsab = Request("idSab")})" class="touch">@h.traducir("Coger/Dejar más recursos")</a>
<a href="@Url.Action("index")" class="touch">@h.traducir("Volver al inicio")</a>