@ModelType IEnumerable(Of INFORMES)
@section scripts
    <script type="text/javascript">
        $(function () {
            $('#select_all').change(function () {
                if ($(this).prop('checked')) {
                    $('tbody tr td input[type="checkbox"]').each(function () {
                        $(this).prop('checked', true);
                    });
                } else {
                    $('tbody tr td input[type="checkbox"]').each(function () {
                        $(this).prop('checked', false);
                    });
                }
            });
        });
    </script>
End Section

<div class="row">
    <div class="col-sm-offset-2 col-sm-8">
        <form action="@Url.Action("index", h.ToRouteValues(Request.QueryString, Nothing))" method="get">
            <div class="input-group">
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
@If Model Is Nothing Then

Else
    @<form action="@Url.Action("downloadintercambio", h.ToRouteValues(Request.QueryString, Nothing))" method="post">
    <Table Class="table">
        <thead>
            <tr>
                <th><input type="checkbox" id="select_all" /></th>
                <th></th>
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
            </tr>
        </thead>
        <tbody>
            @For Each v In Model
                @<tr>
                    <td><input type="checkbox" name="lstInforme" value="@v.IDINFORME" /></td>
                    <td>
                        <a href="@Url.Action("sendemail", h.ToRouteValues(Request.QueryString, New With {.id = v.IDINFORME}))"><span class="glyphicon glyphicon-envelope"></span></a>
                    </td>
                    <td>
                        <a href="@Url.Action("informePDF", "INFORME", New With {.id = v.IDINFORME})">
                            <span class="glyphicon glyphicon-file"></span>
                        </a>
                    </td>
                    <td>@v.CLIENTE</td>
                    <td>@v.PROYECTO</td>
                    <td>@v.VALOROF - @v.VALOROP</td>
                    <td>@v.TIPOINFORME</td>
                    <td>
                        @If v.MARCA.Length > 30 Then
                            @v.MARCA.ToString.Replace("|", ", ").Substring(0, 30)
                        Else
                            @v.MARCA.Replace("|", ", ")
                        End If
                    </td>
                </tr>


            Next
        </tbody>
    </Table>
    <input type="submit" value="@h.traducir("Dejar en Intercambio")" class="btn btn-primary" />
</form>
End If