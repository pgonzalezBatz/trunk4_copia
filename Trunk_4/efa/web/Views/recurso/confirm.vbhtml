@imports web

@section header
    <title>@h.traducir("Confirmación")</title>
    <link href="@Url.Content("~/Content/Site.css")" rel="Stylesheet" type="text/css"  />
End section

@section  beforebody
    <a href="@Url.Action("accion")" style="float:left;">
    <img src="@Url.Content("~/Content/home.png") " alt="@h.traducir("Volver")" />
</a>
<a href="@Url.Action("logout")" style="float:right;" >
    <img src="@Url.Content("~/Content/exit.png")" alt="@h.traducir("Cerrar sessión")  " />
</a>
<br style="clear:both;" />
End Section

<div style="width:100%; text-align:center;">
    <h3>
        @h.Traducir("La operación se ha completado con éxito")
    </h3>
    <img src="@Url.Content("~/Content/ok.png") " alt="@h.Traducir("Imagen OK")  " />
    <h1 style="color:Red; font-size:2em;">
        @h.Traducir("No se olvide de cerrar sesión!")
    </h1>
</div>