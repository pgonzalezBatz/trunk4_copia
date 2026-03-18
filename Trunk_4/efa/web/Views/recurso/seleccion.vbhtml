@Modeltype web.Recurso
@imports web

@section header
    <title>@h.traducir("Cormirmar selección")</title>
End section

@section  beforebody
  <a href="@Url.Action("coger", New With {.nombre = Model.nombreGrupo})" style="float:left;">
    <img src="@Url.Content("~/Content/back.png") " alt="@h.traducir("Volver")  " />
</a>
<a href="@Url.Action("logout")" style="float:right;" >
    <img src="@Url.Content("~/Content/exit.png") " alt="@h.traducir("Cerrar sessión")  " />
</a>
<br style="clear:both;" />
End Section

<h3 class="touch">
    @h.traducir("El recurso fisiko que esta a punto de coger es el siguiente"):
</h3>
<div style="margin-left:30px; font-size:18px;">
    <strong>@h.traducir("Grupo del recurso")</strong>:<br />
    @Html.DisplayTextFor(Function(m) m.NombreGrupo) <br />
    <strong>@h.traducir("Identificador del recurso")</strong>:<br />
    @Html.DisplayTextFor(Function(m) m.Id)<br />
    <br />
    <form method="post" class="touch">
        <input type="submit" value="@h.traducir("guardar")" />
    </form>
    <a class="touch" href="@url.Action("coger", new with{.nombre=Model.NombreGrupo}) ">Cancelar</a>
</div>