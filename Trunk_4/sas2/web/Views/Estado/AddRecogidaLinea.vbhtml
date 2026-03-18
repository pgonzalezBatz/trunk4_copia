@modeltype VMRecogidaFinal
@Code
    Layout = "~/Views/Shared/RMaster.vbhtml"
    Dim i = 0
End Code
@section header
    <link href="//intranet2.batz.es/baliabideorokorrak/jquery-ui-1.11.4.min.css" type="text/css" rel="stylesheet" />
    <link href="//intranet2.batz.es/baliabideorokorrak/bootstrap/css/typeahead.css" rel="stylesheet" />
    <style type="text/css">
        .twitter-typeahead {
            float: left;
            width: 100%
        }
    </style>
End Section
@section scripts
    <script src="//intranet2.batz.es/baliabideorokorrak/bootstrap/js/typeahead.bundle.min.js"></script>
    <script type="text/javascript">
        $(function () {
            var numord = new Bloodhound({
                datumTokenizer: Bloodhound.tokenizers.obj.whitespace(' '),
                queryTokenizer: Bloodhound.tokenizers.whitespace,
                remote: {
                    url: '@Url.Action("BuscarNumord", "json")?q=%QUERY',
                    wildcard: '%QUERY',
                    filter: function (list) {
                        total_results = list.length;
                        return list;
                    }
                }

            });

            $('#@Html.IdFor(Function(m) m.Linea.Numord)').typeahead(null, {
                autoselect: true,
                source: numord,
                display: function (item) { return item },
                hint: true,
                highlight: true,
                templates: {
                    footer: function (context) {
                        // calculate total hits here
                        if (total_results > context.suggestions.length) {
                            return '<h4 class="alert alert-info" role="alert"><span class="glyphicon glyphicon-info-sign"> </span> ' + (total_results - context.suggestions.length) + ' @h.traducir("OFs no mostradas")</h4>'
                        }
                    },
                    empty: ['<h4 class="alert alert-warning" role="alert"><span class="glyphicon glyphicon-warning-sign"></span> @h.traducir("No se han encontrado OFs")</h4>']
                },
                limit: 10
            });
            $('#@Html.IdFor(Function(m) m.Linea.Numord)').on('typeahead:selected', function (e, datum) {
                $.getJSON("@Url.Action("BuscarNumope", "json")?numord=" + $(this).val(), function (data) {
                    $('#@Html.IdFor(Function(m) m.Linea.Numope)').empty();
                    var items = [];
                    $.each(data, function (key, val) {
                        items.push("<option value='" + val.Numope + "'>" + val.Numope + ' - ' + val.Descripcion + "</option>");
                    });
                    $('#@Html.IdFor(Function(m) m.Linea.Numope)').append(items)

                });

            });
        });
    </script>
End Section
<h3>@h.traducir("Recogida. Añadir lineas")</h3>
<hr />
<h4>@Html.ActionLink(h.traducir("Volver"), "ListMovimientosRecogidasSinAsignar")</h4>
<form action="@Url.Action("InsertRecogidaLinea")" method="post">
    @Html.HiddenFor(Function(m) m.id)
    @Html.DisplayFor(Function(m) m.VectorRecogida)

    @Html.EditorFor(Function(m) m.Linea)
    <br />
    <input type="submit" value="@h.traducir("Añadir Linea")" Class="btn btn-primary" />
</form>

<h4>@h.traducir("Lineas de Recogida")</h4>
<table class="table">
    <thead>
        <tr>
            <th>
                @h.traducir("OF")
            </th>
            <th>
                @h.traducir("OP")
            </th>
            <th>
                @h.traducir("Peso")
            </th>
            <th>
                @h.traducir("Zona Entrega")
            </th>
            <th>
                @h.traducir("Accion")
            </th>
        </tr>
    </thead>
    <tbody>
        @For Each lr In Model.LstLinea
            @<tr>
                <td>
                    @lr.Numord
                </td>
                <td>
                    @lr.Numope
                </td>
                <td>
                    @lr.Peso
                </td>
                <td>
                    @lr.ZonaEntrega
                </td>
        <td>
            <form action="@Url.Action("DeleteRecogidaLinea")" method="post">
                @Html.HiddenFor(Function(m) m.id)
                @Html.Hidden("Numord", lr.Numord)
                @Html.Hidden("Numope", lr.Numope)
                @Html.Hidden("Peso", lr.Peso)
                @Html.Hidden("ZonaEntrega", lr.ZonaEntrega)
                <input type="submit" class="btn btn-danger" value="@h.traducir("Eliminar")" />
            </form>
        </td>
            </tr>
        Next
    </tbody>

</table>

