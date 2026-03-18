@ModelType IEnumerable(Of Object)


@section chunkmenuproveedor
    @mvc_helpers.proveedorLinks(Url)
End Section


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
                @Html.TextBox("q", Nothing, New With {.class = "typeahead tt-input form-control", .autocomplete = "off", .spellcheck = "off", .placeholder = h.traducir("Buscar OF, OF-OP, Proyecto, Cliente o Marca")})
                <span class="input-group-btn">
                    <button type="submit" class="btn btn-primary">
                        <i class="glyphicon glyphicon-search"></i>
                    </button>

                </span>
            </div>
        </form>
    </div>
</div>
    <table class="table table-hover">
        <thead>
            <tr>
                <th>@h.traducir("Validado")</th>
                <th>@h.traducir("PDF")</th>
                <th>@h.traducir("Cliente")</th>
                <th>@h.traducir("Proyecto")</th>
                <th>
                    @h.traducir("OF - OP")
                    <a href="@Url.Action("index", h.ToRouteValues(Request.QueryString, New With {.sortby = "ofop"}))"><span class="glyphicon glyphicon-sort-by-attributes"></span></a>
                </th>
                <th>@h.traducir("Tipo informe")
                    <a href="@Url.Action("index", h.ToRouteValues(Request.QueryString, New With {.sortby = "tipoinforme"}))"><span class="glyphicon glyphicon-sort-by-attributes"></span></a>
                </th>
                <th>@h.traducir("Marcas")</th>
                @If SimpleRoleProvider.IsUserAuthorised(web_intranet.Roles.validador) Then
                    @<th>@h.traducir("Validar")</th>
                End If
            </tr>
        </thead>
    @For Each item In Model
        @<tr>
            <td align="center">
                @If item.validado Then
                    @<span class="glyphicon glyphicon-ok text-success" title="@h.traducir("Informe validado para compensar")"></span>
                Else
                    @<span class="glyphicon glyphicon-remove text-danger" title="@h.traducir("Informe NO validado para compensar")"></span>
                End If
                                </td>
             <td>
                 <a href = "@Url.Action("informePDF", New With {.id = item.IDINFORME})">
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
        @If SimpleRoleProvider.IsUserAuthorised(web_intranet.Roles.validador) Then
                    @<td>
            <form action="@Url.Action("validarInforme", h.ToRouteValues(Request.QueryString, New With {.id = item.IDINFORME}))" method="post">
                <input type="submit" value="@h.traducir("Validar para compensación")"  Class="btn btn-primary" />
            </form>
        </td>
        End If
        </tr>
    Next
    
    </table>
