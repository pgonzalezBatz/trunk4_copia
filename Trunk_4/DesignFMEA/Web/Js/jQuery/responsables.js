function init(text, hidden, div,urlBuscar,bDoPostback,controlPostback) {
 
    //var UsuarioBuscar = urlBuscar + '?q=';
    var UsuarioBuscar = urlBuscar;
    if (urlBuscar.indexOf('?') == -1)
        UsuarioBuscar = UsuarioBuscar + '?q=';
    else
        UsuarioBuscar = UsuarioBuscar + '&q=';

    $(document).ready(function () {
        var texto = $('#' + text);
        var inputUsuario = $('#' + hidden);
        var divUsuarios = $('#' + div);

        var bindBehaviors = function (val) {
            $('#' + val.id).click(function () {
                texto.attr("value", val.user);
                inputUsuario.attr("value", val.id);
                divUsuarios.attr("style", "display:none;");
                if (bDoPostback) {
                    var str = controlPostback.toString().replace(/_/gi, '$');
                    __doPostBack(str, '');
                    return false;
                } else {
                    return false;
                }
            });
        };

        //Añade el usuario
        function addUser(id, user) {
            texto.attr("value", user);
            inputUsuario.attr("value", id);
            divUsuarios.attr("style", "display:none;");
            if (bDoPostback) {
                var str = controlPostback.toString().replace(/_/gi, '$');
                __doPostBack(str, '');
                return false;
            } else {
                return false;
            }
        }

        //Para que al pulsar enter, no haga nada
        $(function () {
            $(':text').bind('keydown', function (e) { //on keydown for all textboxes     
                if (e.keyCode == 13) //if this is enter key    
                    e.preventDefault();
            });
        });

        $('#' + text).keyup(function (e) {
            clearTimeout($.data(this, 'timer'));
            var text = this.value;
            $(this).data('timer', setTimeout(function () {
                if (text.length > 3) {
                    $.getJSON(UsuarioBuscar + text, function (json) {
                        divUsuarios.attr("style", "display:block;background-color:#EEE;border:1px solid #8FABFF;width:380px; height:150px; overflow:auto;position:absolute;");
                        divUsuarios.empty();
                        if (json.length == 0) {
                            divUsuarios.append('<h4>No se puedo encontrar ningun usuario</h4>');
                        }
                        else {
                            $.each(json, function (key, val) {
                                if (e.keyCode == 13 && json.length == 1) //if this is enter key    
                                {
                                    addUser(val.id, val.user);
                                }
                                else {
                                    divUsuarios.append("<a id=" + val.id + " href=#>" + val.user + "</a>" + "<br/>");
                                    bindBehaviors(val)
                                }

                            });
                        };
                    });
                }
                else {
                    divUsuarios.empty();
                    divUsuarios.attr("style", "display:none;");
                };
            }, 400));
        });
    });
}
