@modeltype VMRecogidaCabecera
@Code
    Layout = "~/Views/Shared/RMaster.vbhtml"
End Code

@*TODO: How to refactor the following two sections required by the partial? *@
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
    <script src='//intranet2.batz.es/baliabideorokorrak/jquery-ui-1.11.4.min.js' type="text/javascript"></script>
    <script src='//intranet2.batz.es/baliabideorokorrak/jquery.ui.datepicker-@(h.GetCulture().Split("-")(0)).js' type="text/javascript"></script>
    <script src="//intranet2.batz.es/baliabideorokorrak/bootstrap/js/typeahead.bundle.min.js"></script>
    <script type="text/javascript">
        $(function () {
            //Activate calendar. HTML code in partial view
            $('.calendar').datepicker($.datepicker.regional["@h.GetCulture().Split(" - ")(0)"]);
            //Origin logic. HTML code in partial view
            var total_results = 0;
             var companies = new Bloodhound({
                 datumTokenizer: Bloodhound.tokenizers.obj.whitespace('Id','Nombre'),
                 queryTokenizer: Bloodhound.tokenizers.whitespace,
                 remote: {
                     url: '@Url.Action("BuscarProveedor", "json")?q=%QUERY',
                     wildcard: '%QUERY',
                     filter: function (list) {
                         total_results=list.length;
                         return list;
                     }
                     }

            });
            var helbide = new Bloodhound({
                 datumTokenizer: Bloodhound.tokenizers.obj.whitespace('Id','Nombre'),
                 queryTokenizer: Bloodhound.tokenizers.whitespace,
                 remote: {
                     url: '@Url.Action("Buscar", "json")?q=%QUERY',
                     wildcard: '%QUERY',
                     filter: function (list) {
                         total_results=list.length;
                         return list;
                     }
                     }

            });

            var loadTypeaheadCompany =
                function (el) {
                    el.typeahead('destroy');
                    el.typeahead(null, {
                        autoselect: true,
                        key: 'Id',
                        source: companies,
                        displayKey: 'Nombre',
                        hint: true,
                        highlight: true,
                        templates: {
                            footer: function (context) {
                                // calculate total hits here
                                if (total_results > context.suggestions.length) {
                                    return '<h4 class="alert alert-info" role="alert"><span class="glyphicon glyphicon-info-sign"> </span><a href="@Url.Action("moreresults")?q=' + context.query + '"> ' + (total_results - context.suggestions.length) + ' @h.traducir("Empresas no mostradas")</a></h4>'
                                }
                            },
                            empty: ['<h4 class="alert alert-warning" role="alert"><span class="glyphicon glyphicon-warning-sign"></span> @h.traducir("No se han encontrado Empresas")</h4>']
                        },
                        limit: 10
                    });
                }
             var loadTypeaheadHelbide =
                 function (el) {
                     el.typeahead('destroy');
                     el.typeahead(null, {
                        autoselect: true,
                        source: helbide,
                        display: function (item) { return item.Calle + ' - ' + item.Poblacion },
                        hint: true,
                        highlight: true,
                        templates: {
                            footer: function (context) {
                                // calculate total hits here
                                if (total_results > context.suggestions.length) {
                                    return '<h4 class="alert alert-info" role="alert"><span class="glyphicon glyphicon-info-sign"> </span><a href="@Url.Action("moreresults")?q=' + context.query + '"> ' + (total_results - context.suggestions.length) + ' @h.traducir("Empresas no mostradas")</a></h4>'
                                }
                            },
                            empty: ['<h4 class="alert alert-warning" role="alert"><span class="glyphicon glyphicon-warning-sign"></span> @h.traducir("No se han encontrado Empresas")</h4>']
                        },
                        limit: 10
                    });

                }
            var txtIdEmpresaOrigen = $('#VectorRecogida_PuntoOrigen_TxtIdEmpresa');
            var txtIdHelbideOrigen = $('#VectorRecogida_PuntoOrigen_txtIdHelbide');
            var IdEmpresaorigen = $('#VectorRecogida_PuntoOrigen_IdEmpresa');
            var IdHelbideOrigen = $('#VectorRecogida_PuntoOrigen_IdHelbide');

            loadTypeaheadCompany(txtIdEmpresaOrigen);
            txtIdEmpresaOrigen.on('typeahead:selected', function (e, datum) {
                IdEmpresaorigen.val(datum.Id);
            });
            txtIdEmpresaOrigen.on('typeahead:change', function (e, datum) {
                if (datum.length == 0) {
                    IdEmpresaorigen.val("");
                };
            });
            loadTypeaheadHelbide(txtIdHelbideOrigen);
            txtIdHelbideOrigen.on('typeahead:selected', function (e, datum) {
                IdHelbideOrigen.val(datum.Id);
            });
            txtIdHelbideOrigen.on('typeahead:change', function (e, datum) {
                if (datum.length == 0) {
                    IdHelbideOrigen.val("");
                };
            });

            var txtIdEmpresaDestino = $('#VectorRecogida_PuntoDestino_TxtIdEmpresa');
            var txtIdHelbideDestino = $('#VectorRecogida_PuntoDestino_txtIdHelbide');
            var IdEmpresaDestino = $('#VectorRecogida_PuntoDestino_IdEmpresa');
            var IdHelbideDestino = $('#VectorRecogida_PuntoDestino_IdHelbide');

            loadTypeaheadCompany(txtIdEmpresaDestino);
            txtIdEmpresaDestino.on('typeahead:selected', function (e, datum) {
                IdEmpresaDestino.val(datum.Id);
            });
            txtIdEmpresaDestino.on('typeahead:change', function (e, datum) {
                if (datum.length == 0) {
                    IdEmpresaDestino.val("");
                };
            });
            loadTypeaheadHelbide(txtIdHelbideDestino);
            txtIdHelbideDestino.on('typeahead:selected', function (e, datum) {
                IdHelbideDestino.val(datum.Id);
            });
            txtIdHelbideDestino.on('typeahead:change', function (e, datum) {
                if (datum.length == 0) {
                    IdHelbideDestino.val("");
                };
            });
        });

    </script>

end section

<h3>@h.traducir("Añadir Movimientos de Marcas a Transportar - Paso 1")</h3>
<hr />
@Using Html.BeginForm("AddRecogidaLinea")
    @Html.HiddenFor(Function(m) m.id)
    If ViewData.ModelState.Keys.Any(Function(k) ViewData.ModelState(k).Errors.Any()) Then
        @<div Class="alert alert-danger">
            <Button Class="close" data-dismiss="alert" aria-hidden="true">&times;</Button>
            @Html.ValidationSummary(False, "Errors: ")
        </div>
    End If

    @Html.EditorFor(Function(m) m.VectorRecogida)
    @<br />@<br />
    @<label>@h.traducir("Fecha de Recogida")</label>
    @Html.EditorFor(Function(m) m.Fecha)

    @<label>@h.traducir("Observaciones")</label>
    @Html.TextAreaFor(Function(m) m.Observaciones, New With {.class = "form-control"})
    @<br />
    @<input type="submit" class="btn btn-primary" value="@h.traducir("Guardar")" />
End Using