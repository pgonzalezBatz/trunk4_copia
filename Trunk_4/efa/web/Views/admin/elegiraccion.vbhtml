@imports web

@section header
    <title>@h.traducir("Acción")</title>
<style type="text/css">
    #contenido1 {text-align: left;}
    .name {margin: 2em 3em;display: block;}
</style>
End section
@section  beforebody
<strong><a href="@Url.Action("seleccionartrabajador")">@h.Traducir("Volver")</a></strong>
End Section

<strong class="name"> @Model.nombre  @Model.apellido1  @Model.apellido2 </strong>
<div>
    <a class="touch" href="@url.action("seleccionargrupo",new with{.idsab=Model.id}) ">@h.Traducir("Coger")  </a>
    <a class="touch" href="@url.action("seleccionarrecurso",new with{.idsab=Model.id}) ">@h.Traducir("Dejar")  </a>
</div>