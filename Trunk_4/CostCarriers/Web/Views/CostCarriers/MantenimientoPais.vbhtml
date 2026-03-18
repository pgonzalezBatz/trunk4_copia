@Imports CostCarriersLib
@ModelType List(Of String())
@Code


End Code
@section scripts
    <script src="~/Scripts/typeahead.bundle.min.js"></script>
    <script type="text/javascript">
        var paises = new Bloodhound({
            datumTokenizer: Bloodhound.tokenizers.obj.whitespace('Pais', 'Codigo'),
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            remote: {
                url: '@Url.Action("BuscarPais", "CostCarriers")?q=%QUERY',
                wildcard: '%QUERY'
            }
        });


        $('.typeahead').typeahead(null, {
            name: 'paises',
            limit: 10,
            source: paises,
            display: function (item) { return item.Pais },
            templates: {
                header: '<h4>@Utils.Traducir("Paises")</h4>'
            }
        }).on('typeahead:select', function (ev, suggestion) {
            $('#CodigoPais').val(suggestion.Codigo);
            $('#q').val(suggestion.Pais);
        });


    </script>
End Section

<h3><label>@Utils.Traducir("Mantenimiento - Países IVA")</label></h3>
<hr />

<div>
    <!-- buscador -->
    @*<form action="@Url.Action("BuscarPais", "CostCarriers"))" method="get">
        @Html.Hidden("CodigoPais")
        <div class="input-group">
            @Html.TextBox("q", Nothing, New With {.class = "typeahead tt-input form-control", .autocomplete = "off", .spellcheck = "off", .placeholder = "Buscar país"})
            <span class="input-group-btn">
                <button type="submit" class="btn btn-primary">
                    <i class="glyphicon glyphicon-search"></i>
                </button>
            </span>
        </div>
    </form>*@

    <div class="container">
        <form action="@Url.Action("AgregarPais")" method="post">
            <div style="display:inline-block">
                @Html.Hidden("CodigoPais")
                @Html.TextBox("q", Nothing, New With {.class = "typeahead tt-input form-control", .autocomplete = "off", .spellcheck = "off", .placeholder = Utils.Traducir("Buscar pais")})
            </div>
            <div class="input-group" style="display:inline-block">
                <input type="submit" value="@Utils.traducir("Añadir")" class="btn btn-primary" />
            </div>
        </form>
    </div>
    <br />
    @If Model IsNot Nothing AndAlso Model.Count > 0 Then
        @<div class="container tablaPaises">
            <table class="table table-condensed">
                <thead>
                    <tr>
                        <th>@Utils.Traducir("Nombre pais")</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    @For Each m In Model
                        @<tr>
                            <td>@m(1)</td>
                            <td>
                                <a href='@Url.Action("EliminarPais", "CostCarriers", New With {.codigo = m(0)})'>
                                    <span class="glyphicon glyphicon-remove boton-eliminar" aria-hidden="true" title="@Utils.Traducir("Eliminar país")"></span>
                                </a>
                            </td>
                        </tr>
                    Next
                </tbody>
            </table>
        </div>
    End If 
</div>