@imports web
@Code
    ViewBag.title = "Elegir Modelo"
End Code
 <div id="notifications">
     @h.traducir("Asignar etiqueta a activo sin asignar usuario")
     
 </div>
@h.traducir("Etiqueta Nº")<br />  
<strong>@Request("idetiqueta")</strong><br />
@h.Traducir("Tipo")<br />
<strong>@ViewData("tipo").value</strong>
<br /><br />
@Html.ValidationSummary()
<ul class="nestedlist">
    @For Each m In ViewData("listofmarcamodelo")
        @<li>
            @m.nombreMarca
            <ul>
                @For Each mo In m.listOfModelo
                    @<li>
                        <form action="@Url.Action("addlabelpickmodelo", New With {.idetiqueta = Request("idetiqueta"), .idtipo = ViewData("tipo").key, .idmarca = m.idMarca})" method="post">
                            @Html.Hidden("idmodelo", mo.idModelo)

                            <input type="submit" value="@mo.nombreModelo - @mo.precioModelo €" />
                            @h.traducir("Microsoft OM"):
                                @Html.TextBox("numeroserie")
                            @h.traducir("Descripcion"):
                            @Html.TextArea("descripcion", New With {.style = "vertical-align:top;"})
                        </form>
                     </li>
                Next
            </ul>
            
         </li>
    Next

</ul>    
