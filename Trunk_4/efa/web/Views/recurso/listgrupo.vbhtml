@imports web

@section header
    <title>@h.traducir("Tipos de recursos")</title>
End section

@section  beforebody
  <a href="@Url.Action("accion")" style="float:left;">
    <img src="@Url.Content("~/Content/back.png") " alt="@h.traducir("Volver")  " />
</a>
<a href="@Url.Action("logout")" style="float:right;" >
    <img src="@Url.Content("~/Content/exit.png") " alt="@h.traducir("Cerrar sessión")  " />
</a>
<br style="clear:both;" />
End Section

<h3 class="touch">@h.Traducir("Seleccione el tipo de medio"):  </h3>
    @For Each g As Grupo In ViewData("recursos")
@<a href="coger?nombre=@g.Nombre" class="touch touch2">
    <h3>@g.Nombre</h3>
    <img src="@url.action("imagegrupo",new with{.nombregrupo=g.nombre}) "  alt="@h.Traducir("Imagen de " + g.Nombre)  " />
</a>
    Next
<a href="@Url.Action("cogertonerImpresora")" class="touch touch2">
    <h3>@h.traducir("Toner")</h3>
    <img src="@Url.Content("~/Content/toner.png")"  alt="@h.traducir("Imagen de toner")" />
</a>