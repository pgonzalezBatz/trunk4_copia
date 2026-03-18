@imports web
@Code
    ViewBag.title = "Elegir Marca"
End Code
<div id="notifications">@h.traducir("Seleccionar marca y modelo asignar")</div> 
@h.Traducir("Usuario")<br /> <strong>@ViewData("usuario").nombre</strong><br />
@h.traducir("Departamento")<br /> <strong>@ViewData("usuario").nombreDepartamento</strong><br />
@h.Traducir("Nº etiqueta")<br /> <strong>@Request("idetiqueta")</strong><br />
@h.Traducir("Tipo ") <br /><strong>@ViewData("tipo").value </strong><br />
<br />
<ul class="nestedlist">
    @For Each m In ViewData("listofmarcamodelo")
        @<li>
            @m.nombreMarca
            <ul>
                @For Each mo In m.listOfModelo
                    @<li>
                        <form action="@Url.Action("asignmodel", New With {.idsab = Request("idsab"), .idetiqueta = Request("idetiqueta"), .idtipo =  ViewData("tipo").key, .idmarca = m.idMarca})" method="post">
                            @Html.Hidden("idmodelo", mo.idModelo)
                            <input type="submit" value="@mo.nombreModelo - @mo.precioModelo €" />
                                @h.traducir("Numero de serie"):
                                @Html.TextBox("numeroserie")
                                @h.traducir("Descripcion"):
                                @Html.TextArea("descripcion", New With {.style = "vertical-align:top;"})
                                @h.traducir("Asignar al departamento"):
                                <input type="checkbox" name="departamento" value="departamento" />
                        </form>
                     </li>
                Next
            </ul>
            
         </li>
    Next

</ul>    
