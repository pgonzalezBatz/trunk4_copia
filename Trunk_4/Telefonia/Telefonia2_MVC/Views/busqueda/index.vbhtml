@Code
    ViewData("Title") = "index"
End Code
@section header
<link href="//intranet2.batz.es/baliabideorokorrak/bootstrap/css/typeahead.css" rel="stylesheet" />
End Section
@section scripts
<script src="//intranet2.batz.es/baliabideorokorrak/bootstrap/js/typeahead.bundle.min.js"></script>
<script type="text/javascript">
    var total_resuts, total_resuts_deps, total_results_otros
    var names = new Bloodhound({
        datumTokenizer: Bloodhound.tokenizers.obj.whitespace('nombre', 'apellido1', 'apellido2'),
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        remote: {
            url: '@Url.Action("getnames")?q=%QUERY',
            wildcard: '%QUERY',
            filter: function (list) {
                //$('.tt-dataset').append(list.length);
                total_resuts=list.length;
                return list;
            }
        }

    });
    var deps = new Bloodhound({
        datumTokenizer: Bloodhound.tokenizers.obj.whitespace('planta', 'nombre'),
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        remote: {
            url: '@Url.Action("getdeps")?q=%QUERY',
            wildcard: '%QUERY',
            filter: function (list) {
                //$('.tt-dataset').append(list.length);
                total_resuts_deps = list.length;
                return list;
            }
        }

    });
    var otros = new Bloodhound({
        datumTokenizer: Bloodhound.tokenizers.obj.whitespace('extensionOtro', 'nombre','numero','planta'),
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        remote: {
            url: '@Url.Action("getotros")?q=%QUERY',
            wildcard: '%QUERY',
            filter: function (list) {
                total_results_otros = list.length;
                return list;
            }
        }

    });

    $('.typeahead').typeahead(null, {
        autoselect: true,
        name: 'names',
        source: names,
        hint: true,
        highlight: true,
        display: function (item) {
            var r = "";
            if (typeof item.extensionInterna === 'string' && item.extensionInterna.length) {
                r = r + 'Ext. Interna: ' + item.extensionInterna + ' '
            };
            if (typeof item.extensionExterna === 'string' && item.extensionExterna.length) {
                r = r + 'Ext. Externa: ' + item.extensionExterna + ' '
            };
            if (typeof item.numero === 'string' && item.numero.length) {
                r = r + 'Numero: ' + item.numero + ' '
            };
            if (typeof item.extensionOtro === 'string' && item.extensionOtro.length) {
                r = r + 'Otros: ' + item.nombre +  ', Extension (otros): ' + item.extensionOtro + ', Número (otros): ' + item.numero
            };
            return r + item.nombre + ' ' + item.apellido1 + ' ' + item.apellido2
        },
        templates: {
            header: '<h4>@h.traducir("Trabajadores")</h4>',
            footer: function (context) {
                // calculate total hits here
                if (total_resuts > context.suggestions.length) {
                    return '<h4 class="alert alert-info" role="alert"><span class="glyphicon glyphicon-info-sign"> </span><a href="@Url.Action("moreresults")?q=' + context.query + '"> ' + (total_resuts - context.suggestions.length) + ' @h.traducir("Trabajadores o extensiones no mostrados")</a></h4>'
                }
            },
            empty: ['<h4 class="alert alert-warning" role="alert"><span class="glyphicon glyphicon-warning-sign"></span> @h.traducir("No se han encontrado trabajadores o extensiones")</h4>']
        },
        limit: 10

    },
    {
        name: 'deps', source: deps,
        display: function (item) {
            return item.nombre;
        },
        templates: {
            header: '<h4>@h.Traducir("Departamentos")</h4>',
            footer: function (context) {
                // calculate total hits here
                if (total_resuts_deps > context.suggestions.length) {
                    return '<h4 class="alert alert-info" role="alert"><span class="glyphicon glyphicon-info-sign"> </span> ' + (total_resuts_deps - context.suggestions.length) + ' @h.Traducir("Departamentos no mostrados")</h4>'
                }
            },
            empty: ['<h4 class="alert alert-warning" role="alert"><span class="glyphicon glyphicon-warning-sign"></span> @h.Traducir("No se han encontrado departamentos")</h4>']
        }
    },
    {
        name: 'otros', source: otros,
        display: function (item) {
            return item.nombre;
        },
        templates: {
            header: '<h4>@h.Traducir("Otros")</h4>',
            footer: function (context) {
                // calculate total hits here
                if (total_results_otros > context.suggestions.length) {
                    return '<h4 class="alert alert-info" role="alert"><span class="glyphicon glyphicon-info-sign"> </span> ' + (total_results_otros - context.suggestions.length) + ' @h.Traducir("Otros no mostrados")</h4>'
                }
            },
            empty: ['<h4 class="alert alert-warning" role="alert"><span class="glyphicon glyphicon-warning-sign"></span> @h.Traducir("No se han encontrado 'otros'")</h4>']
        }
    }
        );

    

    var f_load_values = function (ev, suggestion) {
        if (typeof suggestion.planta === 'number') {
            $.ajax({
                type: "GET",
                url: "@Url.Action("detailDepartamento")",
                data: { id: suggestion.id, planta : suggestion.planta, nombreDepartamento:suggestion.nombre },
                success: function (viewHTML) {
                    $("#result").html(viewHTML);
                    },
                error: function (errorData) { onError(errorData); }
            });
        }
        //else if (typeof suggestion.extensiones === 'string') {
        else if (typeof suggestion.extensionOtro === 'number') {
             $.ajax({
                type: "GET",
                 url: "@Url.Action("detailOtros")",
                 //data: { extensionOtro: suggestion.extensionOtro, nombre: suggestion.nombre },
                 data: { nombre: suggestion.nombre },
                success: function (viewHTML) {
                    $("#result").html(viewHTML);
                    },
                error: function (errorData) { onError(errorData); }
            });
        }
        else {
            $.ajax({
                type: "GET",
                url: "@Url.Action("detail")",
                data: { id: suggestion.id },
                success: function (viewHTML) {
                    $("#result").html(viewHTML);
                },
                error: function (errorData) { onError(errorData); }
            });
        };
    };

    $('.typeahead').bind('typeahead:select', f_load_values);
    $('.typeahead').bind("typeahead:autocompleted", f_load_values);
</script>  
    <style type="text/css">
        .twitter-typeahead{ float:left; width:100%}
    </style> 
End Section
<div class="hidden-xs">
    <h3>@h.traducir("Búsqueda de teléfonos y extensiones")</h3>
    <hr />
</div>
<form class="form-horizontal">
    <div class="form-group">
        <div class="col-sm-12">
            <span id="search-total"></span>
            @Html.TextBox("q", Nothing, New With {.class = "form-control typeahead", .autocomplete = "off", .spellcheck = "off",
                                                        .placeholder = h.traducir("Nombre, apellidos, extensión o departamento"), .autofocus = "autofocus", .title = h.traducir("Nombre o apellidos del trabajador")})
        </div>
        </div>
   <div id="result">
       @If Request("id") IsNot Nothing Then
        @Html.Action("detail", New With {.id = Request("id")})
       End If
       @If Request("idDepartamento") IsNot Nothing AndAlso Request("idplanta") IsNot Nothing Then
        @Html.Action("DetailDepartamento", New With {.id = Request("idDepartamento"), .planta = Request("idplanta")})
       End If
                   </div>
</form>