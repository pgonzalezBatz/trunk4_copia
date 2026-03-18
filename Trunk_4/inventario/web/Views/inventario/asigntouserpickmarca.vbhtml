@imports web
@Code
    ViewBag.title = "Elegir Marca"
End Code
<div class="legend">@h.Traducir("Seleccionar marca a asignar")</div> 
@h.Traducir("Usuario")<br /> <strong>@ViewData("usuario").nombre</strong><br />
@h.Traducir("Nº etiqueta")<br /> <strong>@Request("idetiqueta")</strong><br />
@h.Traducir("Tipo ") <br /><strong>@ViewData("tipo").value </strong><br />
<br />
<ul>
    @For Each m In ViewData("listofmarca")
        @<li>
            <a href="@Url.Action("asigntouserpickmodelo", New With {.idsab = Request("idsab"), .idetiqueta = Request("idetiqueta"),.idtipo = ViewData("tipo").key, .idmarca = m.key})" class="button">@m.value</a>
         </li>
    Next

</ul>    
