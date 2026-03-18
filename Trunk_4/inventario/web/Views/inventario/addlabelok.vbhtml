@imports web
@Code
    ViewBag.title = "Inventario"
End Code
<div>
    <h2 style="color:green;">@h.Traducir("Accion realizada con éxito")!</h2>
    <br />
    @Html.ActionLink(h.Traducir("Añadir otra etiqueta"), "addlabel", Nothing, New With {.class = "button"})
    <br />
    @Html.ActionLink(h.Traducir("Volver a la página inicial"), "index", Nothing, New With {.class = "button"})
</div>
