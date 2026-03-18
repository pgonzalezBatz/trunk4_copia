@imports web

@section header
    <title>@h.traducir("Acción a realizar")</title>
End section

@section  beforebody
    <a style="float:left;" href="@Url.Action("logout")">
        <img src="@Url.Content("~/Content/exit.png")" alt="@h.Traducir("Cerrar sessión")" />
    </a>
    <br style="clear:both;" />
End Section

<h3 class="touch">@h.Traducir("Indique la acción a realizar")  </h3>
<a href="listgrupo" class="touch">
    @h.Traducir("Coger")
</a>
<a href="dejar" class="touch">
    @h.Traducir("Dejar")
</a>