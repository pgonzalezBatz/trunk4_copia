@modeltype VMMovimientoStep1
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
            var numord = new Bloodhound({
                datumTokenizer: Bloodhound.tokenizers.obj.whitespace(' '),
                queryTokenizer: Bloodhound.tokenizers.whitespace,
                 remote: {
                     url: '@Url.Action("BuscarNumord", "json")?q=%QUERY',
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
            var txtIdEmpresaOrigen = $('#VectorMovimiento_PuntoOrigen_TxtIdEmpresa');
            var txtIdHelbideOrigen = $('#VectorMovimiento_PuntoOrigen_txtIdHelbide');
            var IdEmpresaorigen = $('#VectorMovimiento_PuntoOrigen_IdEmpresa');
            var IdHelbideOrigen = $('#VectorMovimiento_PuntoOrigen_IdHelbide');

            loadTypeaheadCompany(txtIdEmpresaOrigen);
            txtIdEmpresaOrigen.on('typeahead:selected', function (e, datum) {
                IdEmpresaorigen.val(datum.Id);
            });
            loadTypeaheadHelbide(txtIdHelbideOrigen);
            txtIdHelbideOrigen.on('typeahead:selected', function (e, datum) {
                IdHelbideOrigen.val(datum.Id);
            });

            var txtIdEmpresaDestino = $('#VectorMovimiento_PuntoDestino_TxtIdEmpresa');
            var txtIdHelbideDestino = $('#VectorMovimiento_PuntoDestino_txtIdHelbide');
            var IdEmpresaDestino = $('#VectorMovimiento_PuntoDestino_IdEmpresa');
            var IdHelbideDestino = $('#VectorMovimiento_PuntoDestino_IdHelbide');

            loadTypeaheadCompany(txtIdEmpresaDestino);
            txtIdEmpresaDestino.on('typeahead:selected', function (e, datum) {
                IdEmpresaDestino.val(datum.Id);
            });
            loadTypeaheadHelbide(txtIdHelbideDestino);
            txtIdHelbideDestino.on('typeahead:selected', function (e, datum) {
                IdHelbideDestino.val(datum.Id);
            });

        

          
            $('#Numord').typeahead(null, {
                        autoselect: true,
                        source: numord,
                        display: function (item) { return item },
                        hint: true,
                        highlight: true,
                        templates: {
                            footer: function (context) {
                                // calculate total hits here
                                if (total_results > context.suggestions.length) {
                                    return '<h4 class="alert alert-info" role="alert"><span class="glyphicon glyphicon-info-sign"> </span><a href="@Url.Action("moreresults")?q=' + context.query + '"> ' + (total_results - context.suggestions.length) + ' @h.traducir("OFs no mostradas")</a></h4>'
                                }
                            },
                            empty: ['<h4 class="alert alert-warning" role="alert"><span class="glyphicon glyphicon-warning-sign"></span> @h.traducir("No se han encontrado OFs")</h4>']
                        },
                        limit: 10
            });
            $('#Numord').on('typeahead:selected', function (e, datum) {
                $.getJSON("@Url.Action("BuscarNumope", "json")?numord=" + $(this).val(), function (data) {
                    $('#Numope').empty();
                    var items = [];
                    $.each(data, function (key, val) {
                        items.push("<option value='" + val.Numope + "'>" + val.Numope + ' - ' + val.Descripcion + "</option>");
                    });
                    $('#Numope').append(items)
                   
                });
                
            });
        });

    </script>

end section

<h3>@h.traducir("Añadir Movimientos de Marcas a Transportar - Paso 1")</h3>
<hr />
@Using Html.BeginForm()

    If ViewData.ModelState.Keys.Any(Function(k) ViewData.ModelState(k).Errors.Any()) Then
        @<div Class="alert alert-danger">
            <Button Class="close" data-dismiss="alert" aria-hidden="true">&times;</Button>
            @Html.ValidationSummary(False, "Errors: ")
        </div>
    End If




    @Html.EditorFor(Function(m) m.VectorMovimiento)
    @<br /> @<br />
    @<div class="form-group">
        <label>@h.traducir("OF")</label>
        @Html.TextBox("Numord", Nothing, New With {.class = "form-control typeahead", .autocomplete = "off", .spellcheck = "off",
                                                                     .placeholder = h.traducir("Buscar OF"),
                                                                     .title = h.traducir("OF")})
    </div>
    @<br />
    @<br />
            @<label>@h.traducir("Operación")</label>
            @Html.DropDownListFor(Function(m) m.Numope, Model.ListOfNumope, New With {.class = "form-control"})
            @<input type="submit" value="@h.traducir("Continuar")" class="btn btn-primary" />
End Using

