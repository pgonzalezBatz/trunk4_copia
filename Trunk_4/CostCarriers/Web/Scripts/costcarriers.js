function initBusquedaCostCarriers(text, hidden, div, empresa, url) {
    $(function () {        
        var texto = $('#' + text);
        var hid = $('#' + hidden);
        var divAux = $('#' + div);        

        var bindBehaviors = function (val) {
            $('#' + val.Valor + texto.attr('id')).click(function () {
                texto.val(val.DescripcionCompleta);
                hid.attr("value", val.Valor);
                divAux.attr("style", "display:none;");
                if (texto && texto.hasClass('auto-no-seleccionado')) {
                    texto.removeClass("auto-no-seleccionado");
                    texto.addClass("auto-seleccionado");
                }
                                
                $(document).trigger("codigoSeleccionado", [val.Valor, val.Lantegi, val.FechaAperturaCadena, val.Datos, val.DescripcionCompleta]);

                return false;
            });
        };

        //Añade el usuario
        function addUsuario(id, valor) {
            texto.attr("value", valor);
            hid.attr("value", id);
            divAux.attr("style", "display:none;");
            return false;
        }

        //Para que al pulsar enter, no haga nada
        $(function () {
            texto.bind('keydown', function (e) { //on keydown for all textboxes     
                if (e.keyCode == 13) //if this is enter key    
                    e.preventDefault();
            });
        });

        texto.keyup(function (e) {            
            clearTimeout($.data(this, 'timer'));

           if (texto) {
               if (texto.hasClass('auto-seleccionado')) {
                   texto.removeClass('auto-seleccionado')
                   texto.addClass("auto-no-seleccionado");
               }
               hid.attr("value", '');
            }

            divAux.empty();
            var text = this.value;

            $(document).trigger("codigoBorrado");

            $(this).data('timer', setTimeout(function () {
                if (text.length > 3) {                    
                    //encodeURIComponent lo utilizo para codificar símbolos especiales (á, é, ñ...)
                    $.getJSON(url + "?q=" + encodeURIComponent(text) + "&empresa=" + empresa, function (json) {
                        divAux.attr("style", "display:block;background-color:#EEE;border:1px solid #8FABFF;width:" + texto.width() + "px;height:150px; overflow:auto;position:absolute;z-index:999;");

                        if (json.length == 0) {
                            divAux.append('<h4>No se encontró ningun registro</h4>');                            
                        }
                        else {
                            $.each(json, function (key, val) {
                                if (e.keyCode == 13 && json.length == 1) //if this is enter key    
                                {
                                    addUsuario(val.Valor, val.DescripcionCompleta);
                                    if (texto && texto.hasClass('auto-no-seleccionado')) {
                                        texto.removeClass("auto-no-seleccionado");
                                        texto.addClass("auto-seleccionado");
                                    }
                                }
                                else {
                                    divAux.append("<a style='display:inline;' id=" + val.Valor + texto.attr('id') + " href=#>" + val.DescripcionCompleta + "</a>" + "<br/>");
                                    bindBehaviors(val)
                                }
                            });
                        };
                    });

               }
                else {
                    divAux.attr("style", "display:none;");
                };
            }, 500));
        });
    });
}