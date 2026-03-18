@imports web
@Code
    ViewBag.title = "Elegir Tipo"
End Code
    
<h2>@h.Traducir("Elegir tipo para etiqueta ")  @Request("idetiqueta")</h2>
<h2>@h.Traducir("Tipo") @ViewData("tipo").value</h2>
<h2>@h.Traducir("Marca") @ViewData("marca").value</h2>
<ul>
    @For Each m In ViewData("listofmodelo")
        @<li>
            <form action="" method="post">
                @Html.Hidden("idmodelo",m.id)
                <input type="submit" value="@m.nombre - @m.precio €" />
            </form>
         </li>
    Next
</ul>
