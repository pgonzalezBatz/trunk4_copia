@imports web
@section title
    - @h.traducir("Busqueda")
End section
@section menu1
    @Html.Partial("menu")
End Section

<div class="row">
    <div class="col-sm-12">
        <a href="@Url.Action("busquedaConsulta")"> @h.traducir("Buscar proveedor en todas las plantas (solo para consulta)")</a>
    </div>
        <div class="col-sm-12">
            <h4> <a href="@Url.Action("create")"><span class="glyphicon glyphicon-plus"></span> @h.traducir("Relacionar nuevo proveedor global")</a></h4>
        </div>
    </div>
        <form action="" method="get">
            <div class="row">

                <div class="col-sm-12">
                    <div class="input-group">
                        @Html.TextBox("q", Nothing, New With {.class = "typeahead tt-input form-control", .autocomplete = "off", .spellcheck = "off", .placeholder = h.traducir("Buscar Nº Proveedor Global ")})
                        <div class="input-group-btn">
                            <button class="btn btn-primary" type="submit"><i class="glyphicon glyphicon-search"></i></button>
                        </div>
                    </div>
                </div>
            </div>
        </form>
        <br />

<div class="row">
    <div class="col-sm-12">
                    @If Model Is Nothing Then

                    Else
                        @<table class="table table-hover">
                            <thead>
                                <tr>
                                    <th>
                                        @System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("Nombre"))
                                    </th>
                                    <th>
                                        @System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("Cif"))
                                    </th>
                                    <th>
                                        @System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("Provincia"))
                                    </th>
                                    <th>
                                        @System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("Localidad"))
                                    </th>
                                    <th></th>
                                </tr>
                            </thead>
                            <tbody>
                                @For Each item In Model
                                    @<tr>
                                        <td>
                                            @item.nombre
                                        </td>
                                        <td>
                                            @item.cif
                                        </td>
                                        <td>
                                            @item.provincia
                                        </td>
                                        <td>
                                            @item.localidad
                                        </td>
                                <td>
                                    <a href="@Url.Action("edit", New With {.id = item.id})">@h.traducir("Editar")</a>
                                    @If item.lstEmpresas.count = 1 AndAlso item.lstEmpresas(0).idempresa Is DBNull.Value Then
                                        @Html.Encode("|")
                                        @<a href = "@Url.Action("delete", New With {.id = item.id})">@h.traducir("Eliminar")</a>
                                    End If
                                                        </td>
                                    </tr>
                                Next
                            </tbody>
                        </table>
                    End If         
                
    </div>
    
    </div>
@code
    Dim take = If(Request("take"), Controllers.corporativoController.TakeLimit)
    Dim skip = If(Request("skip"), Controllers.corporativoController.skipMin)
End Code

<span class="pagination"></span>
<ul class="pager">
    @If skip > Controllers.corporativoController.skipMin Then
        @<li>
            <a href="@Url.Action("index", h.ToRouteValues(Request.QueryString, New With {.skip = skip - take}))">@h.traducir("Menos")</a>
        </li>

    End If
    @If CType(Model, IEnumerable(Of Object)).Count = take Then
        @<li>
            <a href="@Url.Action("index", h.ToRouteValues(Request.QueryString, New With {.skip = skip + take}))">@h.traducir("Mas")</a>
        </li>
    End If
</ul>

     