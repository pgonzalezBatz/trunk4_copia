function initBusquedaUsuarios(text, hidden, div, url) {
    $(function () {        
        var texto = $('#' + text);
        var inputUsuario = $('#' + hidden);
        var divUsuarios = $('#' + div);

        var bindBehaviors = function (val) {
            $('#' + val.Id + texto.attr('id')).click(function () {
                texto.val(val.NombreCompleto);
                inputUsuario.attr("value", val.Id);
                divUsuarios.attr("style", "display:none;");
                if (texto && texto.hasClass('auto-no-seleccionado')) {
                    texto.removeClass("auto-no-seleccionado");
                    texto.addClass("auto-seleccionado");
                }
                return false;
            });
        };

        //Añade el usuario
        function addUsuario(id, nombre) {
            texto.attr("value", nombre);
            inputUsuario.attr("value", id);
            divUsuarios.attr("style", "display:none;");
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
               inputUsuario.attr("value", '');
            }

            divUsuarios.empty();
            var text = this.value;

            $(this).data('timer', setTimeout(function () {
                if (text.length > 3) {                    
                    //encodeURIComponent lo utilizo para codificar símbolos especiales (á, é, ñ...)
                    $.getJSON(url + "?q=" + encodeURIComponent(text), function (json) {
                        divUsuarios.attr("style", "display:block;background-color:#EEE;border:1px solid #8FABFF;width:" + texto.width() + "px;height:150px; overflow:auto;position:absolute;z-index:1;");

                        if (json.length == 0) {
                            divUsuarios.append('<h4>No se encontró ningun registro</h4>');
                        }
                        else {
                            $.each(json, function (key, val) {
                                if (e.keyCode == 13 && json.length == 1) //if this is enter key    
                                {
                                    addUsuario(val.Id, val.NombreCompleto);
                                    if (texto && texto.hasClass('auto-no-seleccionado')) {
                                        texto.removeClass("auto-no-seleccionado");
                                        texto.addClass("auto-seleccionado");
                                    }
                                }
                                else {
                                    divUsuarios.append("<a style='display:inline;' id=" + val.Id + texto.attr('id') + " href=#>" + val.NombreCompleto + "</a>" + "<br/>");
                                    bindBehaviors(val)
                                }
                            });
                        };
                    });

               }
                else {
                    divUsuarios.attr("style", "display:none;");
                };
            }, 500));
        });
    });
}