@imports web
@Code
    ViewBag.title = "Inventario"
End code
<div>
    @Html.ActionLink(h.traducir("Usuario"), "asigntouser", Nothing, New With {.class = "button"})<br />
    @Html.ActionLink(h.traducir("Etiqueta"), "addlabel", Nothing, New With {.class = "button"})<br />
    @Html.ActionLink(h.Traducir("Administrar jerarquía"), "listtipo", "admin", Nothing, New With {.class = "button"})<br />
    @Html.ActionLink(h.Traducir("Imprimir etiqueta"), "printlabels", "zebra", Nothing, New With {.class = "button"})<br />
    @Html.ActionLink(h.traducir("Reasignar bajas"), "ReasignarContenidoUsuario1", Nothing, New With {.class = "button"})<br />

</div>