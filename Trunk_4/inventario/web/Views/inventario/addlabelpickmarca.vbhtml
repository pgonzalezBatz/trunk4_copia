@imports web
@Code
    ViewBag.title = "Elegir Tipo"
End Code
    
<h2>@h.Traducir("Elegir tipo para etiqueta ")  @Request("idetiqueta")</h2>
<h2>@h.Traducir("Tipo") @ViewData("tipo").value</h2>
<ul>
    @For Each ma In ViewData("listofmarca")
        @<li>
            <a href="@Url.Action("addlabelpickmodelo", New With {.idetiqueta = Request("idetiqueta"), .idtipo = ViewData("tipo").key, .idmarca = ma.key})">@ma.value</a>
         </li>
    Next
</ul>
