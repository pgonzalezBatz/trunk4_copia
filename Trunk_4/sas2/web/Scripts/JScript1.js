
/*
Ajax-JSON texbox search. Displays a dinamyc list of matches from the textbox
Parameters:
    url= string where the ajax call will be made. JSON response it's expected
    searchBox= JQuery object of the box in which the search is typed
    onleElementToAppend= a function accepting one parameter (the return of the response) only called when one element returned
    manyElementsToAppend= a function accepting 3 parameters. The jquery div container, a value fron the JSON and a function required to call for element binding.
                           This function that is to be called, needs 2 parameters: The element to bind and a function to be executed when binding
    elementContainer= This parameter it's optional. Use it when the default container that's displayed when searching, needs to be changed
*/
function textboxSearch(url, searchBox, oneElementToAppend, manyElementsToAppend, elementContainer) {
    var fDisplay = function (sB) {
        var pos = sB.offset();
        if (typeof (elementContainer) == 'undefined') {
            elementContainer = '<div id="divavoidcolision" style="position:absolute;left:' + pos.left.toString() + 'px;top:' + (pos.top + sB.outerHeight()).toString() + 'px;display:block;width:48%;height:200px;background-color:Gray;overflow:auto;background-color:#EEE;border:1px solid #8FABFF;z-index:5;"></div>';
        };
        if ($('#' + $(elementContainer).attr('id')).length == 0) {
            $("body").append(elementContainer);
            //Avoid propagation when inside box click
            $('#' + $(elementContainer).attr('id')).click(function (event) {
                event.stopPropagation();
            });
        }
        else {
            $('#' + $(elementContainer).attr('id')).empty();
        };
    };
    var fHide = function () {
        $('#divavoidcolision').remove();
    };
    var bindProveedorBehaviors = function (element, f) {
        element.click(function () {
            f();
            fHide();
            return false;
        });
    };
    //prevent form submit
    searchBox.keydown(function (e) {
        if (e.keyCode == 13) {
            return false;
        };
    });

    searchBox.keyup(function (e) {
        clearTimeout($.data(this, 'timer'));
        var me = this;
        $(this).data('timer', setTimeout(function () {
            if (me.value.length > 2) {
                fDisplay(searchBox);
                $.getJSON(url + me.value, function (json) {
                    if (json.length == 0) {
                        $('#divavoidcolision').append("<h4>No elements found</h4>");
                    }
                    else {
                        if (json.length == 1 & e.keyCode == 13) {
                            oneElementToAppend(json[0]);
                            fHide();
                            return false;
                        }
                        else {
                            $.each(json, function (key, val) {
                                manyElementsToAppend($('#divavoidcolision'), val, bindProveedorBehaviors)
                            });
                        };
                    };
                });
            }
            else {
                fHide();
            };
        }, 500));
    });
    $('html').click(function () {
        if ($("#divavoidcolision").length > 0) {
            fHide();
        }
    });
}; 
function getValueForHidden(url, f) {
    $.getJSON(url, function (json) {
        f(json);
    });
};
function transportistas(url, searchBoxId, hiddenId, divHelperId) {
    searchBox = $('#' + searchBoxId);
    hidden = $('#' + hiddenId);
    divHelper = $('#' + divHelperId);
    $(document).ready(function () {
        var bind = function (val) {
            $('#tra' + val.Id).click(function () {
                hidden.attr("value", val.Id);
                searchBox.attr('value', val.Nombre);
                divHelper.empty();
                divHelper.attr("style", "display:none;");
                preventDefault(); //Prevent redirection
            });
        };
        searchBox.keyup(function (e) {
            if (this.value.length > 2) {
                $.getJSON(url + '?q=' + this.value, function (json) {
                    divHelper.attr("style", "display:block;width:50%;height:200px;background-color:Gray;position:absolute;overflow:auto;background-color:#EEE;border:1px solid #8FABFF;");
                    divHelper.empty();
                    if (json.length == 0) {
                        divHelper.append('<h4>No se puedo encontrar ningun transportista</h4>');
                    }
                    else {
                        $.each(json, function (key, val) {
                            divHelper.append('<a id="tra' + val.Id + '" href="#">' + val.Nombre + '</a><br/>');
                            bind(val)
                        });
                    };
                });
            }
            else {
                divHelper.empty();
                divHelper.attr("style", "display:none;");
            };
        });
    });
};

function movimientoMaterial(baseUrl, numordBoxId, numopeBoxId, marcaBoxId, divHelperId) {
    var numordBox = $('#' + numordBoxId);
    var numopeBox = $('#' + numopeBoxId);
    var marcaBox = $('#' + marcaBoxId);
    var divHelper = $('#' + divHelperId);
    var urlNumord = baseUrl + '/BuscarNumord?q=';
    var urlNumope = baseUrl + '/BuscarNumope?numord=';
    var urlMarca = baseUrl + '/BuscarMarca?numord=';

    $(document).ready(function () {
        numordBox.blur(function () {
            if (numordBox.val() > 0) {
                numope(urlNumope + numordBox.val(), numopeBox)
            };
        });
        numopeBox.focus(function () {
            divHelper.attr('style', 'display:none;');
        });
        numopeBox.change(function (e) {
            marcaBox.focus();
        });
        marcaBox.focus(function () {
            if (numordBox.val() > 0 & numopeBox.val() > 0) {
                marca(urlMarca + numordBox.val() + '&numope=' + numopeBox.val(), marcaBox, divHelper);
            }
            divHelper.attr("style", "display:block;");
        });
    });
};
function numord(jsonUrl, searchBox, divHelper, nextFocus) {
    var bindBehavior1 = function (val) {
        $('#numord' + val).click(function () {
            searchBox.attr("value", val);
            divHelper.attr("style", "display:none;");
            nextFocus.focus();
            return false;
        });
    };
    $.getJSON(jsonUrl, function (json) {
        divHelper.attr("style", "display:block;");
        divHelper.empty();
        if (json.length == 0) {
            divHelper.append('<h4>OF no existente</h4>');
        }
        else {
            $.each(json, function (key, val) {
                divHelper.append('<a id="numord' + val + '" href="#">' + val + '</a><br/>');
                bindBehavior1(val)
            });
        };
    });
};
function numope(jsonUrl, selectBox) {
    $.getJSON(jsonUrl, function (json) {
        selectBox.empty();
        selectBox.append('<option value="">- Seleccionar -</option>');
        $.each(json, function (key, val) {
            selectBox.append('<option value="' + val.Numope + '">' + val.Numope + ' - ' + val.Descripcion + '</option>');
        });
    });
};

function marca(jsonUrl, searchBox, divHelper) {
    divHelper.empty();
    var bindBehavior = function (val) {
        $('#mar' + val).click(function () {
            searchBox.attr("value", val);
            divHelper.attr('style', 'display:none;');
            return false;
        });
    };
    $.getJSON(jsonUrl, function (json) {
        $.each(json, function (key, val) {
            if (json.length > 0) {
                var m = $.trim(val.Marca);

                divHelper.append('<a id="mar' + m + '" href="#">' + m + '</a>');
                bindBehavior(m)
            }
            else {
                divHelper.append('<h3>No se han encontrado marcas</h3>');
            };
        });
    });
};