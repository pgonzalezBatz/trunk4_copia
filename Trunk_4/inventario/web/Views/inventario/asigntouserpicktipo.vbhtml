@imports web
@Code
    ViewBag.title = "Elegir Tipo"
End Code

<div id="notifications">@h.Traducir("Seleccionar tipo de recurso a asignar")</div> 
@h.Traducir("Usuario")<br /> <strong>@ViewData("usuario").nombre</strong>
<br />
@h.Traducir("Nº etiqueta")<br /> <strong>@Request("idetiqueta")</strong>
<br />
<br />
<ul>
    @For Each t In ViewData("listoftipo")
        @<li>
            <a href="@Url.Action("AsignToUserPickmarcaAndModelo", New With {.idsab = Request("idsab"), .idetiqueta = Request("idetiqueta"), .idtipo = t.key})" class="button">@t.value</a>
         </li>
    Next
</ul>