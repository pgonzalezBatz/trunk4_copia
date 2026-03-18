@imports web
@Code
    ViewBag.title = "Elegir Tipo"
End Code
<div class="legend">@h.Traducir("Seleccionar modelo a asignar")</div> 
@h.Traducir("Usuario")<br /> <strong>@ViewData("usuario").nombre</strong><br />
@h.Traducir("Nº etiqueta")<br /> <strong>@Request("idetiqueta")</strong><br />
@h.Traducir("Tipo ") <br /><strong>@ViewData("tipo").value </strong><br />
@h.Traducir("Marca ") <br /><strong> @ViewData("marca").value </strong><br />
<br />

<ul>
    @For Each m In ViewData("listofmodelo")
        @<li>
            <form action="@Url.Action("asignmodel", New With {.idsab = Request("idsab"), .idetiqueta = Request("idetiqueta"), .idtipo = m.id,.idmarca=ViewData("marca").key})" method="post">
                @Html.Hidden("idmodelo",m.id)
                <input type="submit" value="@m.nombre - @m.precio €" />
            </form>
         </li>
    Next
</ul>    
