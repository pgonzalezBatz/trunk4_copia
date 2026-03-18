@imports web

@section header
    <title>@h.traducir("Colores de toner para impresora")</title>
End section

@section  beforebody
  <a href="@Url.Action("listgrupo")" style="float:left;">
    <img src="@Url.Content("~/Content/back.png")" alt="@h.traducir("Volver")" />
</a>
<a href="@Url.Action("logout")" style="float:right;" >
    <img src="@Url.Content("~/Content/exit.png")" alt="@h.traducir("Cerrar sessión")" />
</a>
<br style="clear:both;" />
End Section
<br />
<a class="touch" href="@Url.Action("cogertonerimpresora")">
    @h.traducir("Por impresora")
</a>
<a class="touch" href="@Url.Action("cogertonercolor")">
    @h.traducir("Por color")
</a>
    