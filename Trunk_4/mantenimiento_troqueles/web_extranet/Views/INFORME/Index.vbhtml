@ModelType IEnumerable(Of Object)

@Code
End Code
    @If ViewData("displayEntregadas") Then
        @<h3>@h.traducir("Informes Cerrados")</h3>
    Else
        @<h3>@h.traducir("Informes Abiertos")</h3>
    End If
    
    <hr />
<div class="row">
    <div class="col-sm-offset-2 col-sm-8">
        <form action="@Url.Action("index", h.ToRouteValues(Request.QueryString, Nothing))" method="get">
            <div class="input-group">
                @Html.Hidden("displayEntregadas")
                @Html.TextBox("q", Nothing, New With {.class = "typeahead tt-input form-control", .autocomplete = "off", .spellcheck = "off", .placeholder = h.traducir("Buscar OF, Proyecto o Cliente")})
                <span class="input-group-btn">
                    <button type="submit" class="btn btn-primary">
                        <i class="glyphicon glyphicon-search"></i>
                    </button>

                </span>
            </div>
        </form>
    </div>
</div>
    <p>
        <a><span class="glyphicon glyphicon-plus"></span></a>
        @Html.ActionLink(h.traducir("Crear informe"), "Create1") 
        
    </p>
    <table class="table table-hover">
        <thead>
            <tr>
                <th>@h.traducir("PDF")</th>
                <th>@h.traducir("Cliente")</th>
                <th>@h.traducir("Proyecto")</th>
                <th>@h.traducir("OF -OP")</th>
                <th>@h.traducir("Tipo informe")</th>
                <th>@h.traducir("Marcas")</th>
                <th></th>
            </tr>
        </thead>
    @For Each item In Model
        @<tr>
             <td>
                 <a href="@Url.Action("informePDF", New With {.id = item.IDINFORME})">
                    <span class="glyphicon glyphicon-file"></span>
                 </a>
             </td>
            <td>
                @item.cliente
            </td>
             <td>
                 @item.proyecto
             </td>
             <td>
                 @item.valorof - @item.valorop
             </td>
             <td>@item.TIPOINFORME.ToString.ToUpper</td>
             <td>
                 @if item.Marca.length > 30 Then
                    @item.Marca.ToString.Replace("|", ", ").Substring(0, 30)
                 Else
                    @item.Marca.Replace("|", ", ")
                 End If

                
            </td>
            <td>
                @If Not ViewData("displayEntregadas") Then
                    @Html.ActionLink(h.traducir("Editar informe"), "Edit3", New With {.idinforme = item.IDINFORME}) 
                    @Html.Encode(" | ")
                    @Html.ActionLink(h.traducir("Editar imagenes"), "EditImages", New With {.id = item.IDINFORME})
                    @Html.Encode(" | ")
                    @Html.ActionLink(h.traducir("Eliminar"), "Delete", New With {.idinforme = item.IDINFORME})
                End If
            </td>
        </tr>
    Next
    
    </table>
