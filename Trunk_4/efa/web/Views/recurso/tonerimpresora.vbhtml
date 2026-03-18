@imports web

@section header
    <title>@h.traducir("Tipos de impresoras")</title>
End section

@section  beforebody
  <a href="@Url.Action("listgrupo")" style="float:left;">
    <img src="@Url.Content("~/Content/back.png")" alt="@h.traducir("Volver")" />
</a>
<a href="@Url.Action("LogOut")" style="float:right;" >
    <img src="@Url.Content("~/Content/exit.png")" alt="@h.traducir("Cerrar sessión")" />
</a>
<br style="clear:both;" />
End Section
<h3 class="touch">
    @h.traducir("Seleccione la impresora"):
</h3>
        @For Each i In Model
@<a class="touch" href="@Url.Action("cogertonercolor", New With {.idimpresora = i.id})">
    @i.nombre  @i.serie
</a>
        Next
           