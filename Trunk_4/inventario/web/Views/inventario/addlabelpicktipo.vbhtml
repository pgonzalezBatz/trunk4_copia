@imports web
@Code
    ViewBag.title = "Elegir Tipo"
End Code

<div id="notifications">
     @h.traducir("Asignar etiqueta a activo sin asignar usuario")
     
 </div>
    
@h.traducir("Etiqueta Nº")<br />  
<strong>@Request("idetiqueta")</strong><br />
<br />
<ul>
    @For Each t In ViewData("listoftipo")
        @<li>
            <a href="@Url.Action("addlabelPickmarcaAndModelo", New With {.idetiqueta = Request("idetiqueta"), .idtipo = t.key})" class="button">@t.value</a>
         </li>
    Next
</ul>
