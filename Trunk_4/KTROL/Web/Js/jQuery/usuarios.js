
function initBusquedaUsuarios(text, hidden, div, urlBuscar, bDoPostback, controlPostback, divImage) {
    var UsuarioBuscar = urlBuscar + '?q=';

    $(document).ready(function () {
        var texto = $('#' + text);
        var inputUsuario = $('#' + hidden);
        var divUsuarios = $('#' + div);
        var imagenSeleccion = $('#' + divImage);

        var bindBehaviors = function (val) {
            $('#' + val.id).click(function () {
                texto.attr("value", val.nombre);
                inputUsuario.attr("value", val.id);
                divUsuarios.attr("style", "display:none;");

                //Imagen de usuario correcto o incorrecto
                if (imagenSeleccion && imagenSeleccion.hasClass('imagen-no-seleccionado')) {
                    imagenSeleccion.toggleClass("imagen-no-seleccionado");
                    imagenSeleccion.toggleClass("imagen-seleccionado");
                }

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
            //alert(id + ' - ' + user + ' ' + bDoPostback);
            if (bDoPostback) {
                var str = controlPostback.toString().replace(/_/gi, '$');
                __doPostBack(str, '');
                return false;
            } else {
                return false;
            }
        }

        //Convierte en mayusculas el nombre del link <a href=>NOMBRE</a>
        function explorador(content) {
            if (content.split(">").length > 1) {
                var posIni = content.indexOf(">");
                var posFin = content.indexOf("<", 1);
                var contentReemplazar = content.substring(posIni - 1, posFin + 1);
                return content.replace(contentReemplazar, contentReemplazar.toUpperCase());
            }
            else
                return content.toUpperCase();
        }

        //        //Para que al pulsar enter, no haga nada
        $(function () {
            $(':text').bind('keydown', function (e) { //on keydown for all textboxes     
                if (e.keyCode == 13) //if this is enter key    
                    e.preventDefault();
            });
        });
        //Para que al pulsar enter, no haga nada
        $(function () {
            $(':text').bind('keydown', function (e) { //on keydown for all textboxes     
                if (e.keyCode == 13) //if this is enter key    
                {
                    e.preventDefault();

                    var usuarios = divUsuarios.html().toLowerCase();
                    var userSplit = usuarios.split("<br>");
                    var id, name;
                    id = ""; name = "";
                    for (index = 0; index < userSplit.length - 1; index++) {
                        user = userSplit[index];
                        if (user.indexOf("style") != -1) //lo ha encontrado. Hay que conseguir el id y el user
                        {
                            for (var index = user.indexOf("id=") + 3; index < user.length - 1; index++) {
                                if (isFinite(user.charAt(index)))
                                    id = id + user.charAt(index);
                                else
                                    break;
                            }
                            name = user.split(">")[1].split("<")[0].toUpperCase();
                            addUser(id, name);
                            break;
                        }
                    }
                }
            });
        });

        $('#' + text).keyup(function (e) {
            //Capa de usuario
            if (this.value.length == 0 && imagenSeleccion) {
                if (imagenSeleccion.hasClass('imagen-seleccionado')) {
                    imagenSeleccion.toggleClass('imagen-seleccionado')
                    imagenSeleccion.toggleClass("imagen-no-seleccionado");
                }
                inputUsuario.attr("value", '');
            }

            divUsuarios.empty();

            if (this.value.length > 3) {
                if (e.keyCode == 40 || e.keyCode == 38) {  //flecha abajo, flecha arriba                    
                    var usuarios = divUsuarios.html().toLowerCase();
                    var userSplit = usuarios.split("<br>");
                    var user, textoConStyle, nuevoTexto;
                    var index, indexResaltar;
                    nuevoTexto = ''; indexResaltar = 0;
                    for (index = 0; index < userSplit.length - 1; index++) {
                        user = userSplit[index];
                        if (user.indexOf("style") != -1) //lo ha encontrado
                        {
                            if ((e.keyCode == 40 && index != userSplit.length - 2) || (e.keyCode == 38 && index != 0)) {
                                if (e.keyCode == 40)
                                    indexResaltar = index + 1;
                                if (e.keyCode == 38)
                                    indexResaltar = index - 1;

                                textoConStyle = user.replace("<a style=\"background-color: #ffff66\" ", "<a ");
                                nuevoTexto = nuevoTexto + explorador(textoConStyle) + "<br>";
                            }
                            else {
                                indexResaltar = index;  //Se le asigna al index actual para que cuando salga del bucle, no vuelva a pintar el primero como marcado
                                nuevoTexto = nuevoTexto + explorador(user) + "<br>";
                            }
                        }
                        else {
                            nuevoTexto = nuevoTexto + explorador(user) + "<br>";
                        }
                    }
                    //Seleccionamos el elemento. Si no habia seleccionado ninguno, se marca el primero
                    var todoElTextoSplit = nuevoTexto.split("<br>");
                    todoElTextoSplit[indexResaltar] = explorador(userSplit[indexResaltar].replace("<a ", "<a style=\"background-color: #ffff66\" "));   //Reemplazamos el texto del primero
                    nuevoTexto = todoElTextoSplit.join("<br>");

                    divUsuarios.empty();
                    divUsuarios.append(nuevoTexto);
                }
                else {
                    $.getJSON(UsuarioBuscar + this.value, function (json) {
                        divUsuarios.attr("style", "display:block;background-color:#EEE;border:1px solid #8FABFF;width:280px; height:150px; overflow:auto;position:absolute;");
                        divUsuarios.empty();
                        if (json.length == 0) {
                            divUsuarios.append('<h4>No se puedo encontrar ningun usuario</h4>');
                        }
                        else {
                            var index = 0;
                            $.each(json, function (key, val) {
                                if (e.keyCode == 13 && json.length == 1) //if this is enter key    
                                {
                                    //addUser(val.id, val.user);

                                    //Capa de usuarios
                                    if (imagenSeleccion && imagenSeleccion.hasClass('imagen-no-seleccionado')) {
                                        imagenSeleccion.toggleClass("imagen-no-seleccionado");
                                        imagenSeleccion.toggleClass("imagen-seleccionado");
                                    }
                                }
                                else {
                                    var style = '';
                                    if (index == 0)
                                        style = "style=\"background-color: #ffff66\"";
                                    divUsuarios.append("<a " + style + " id=" + val.id + " href=\"javascript:addUser(" + val.id + ",'" + val.nombre + "'," + bDoPostback + ",'" + controlPostback + "','" + text + "','" + hidden + "','" + div + "','" + divImage + "');\">" + val.nombre.toUpperCase() + "</a><br>");
                                }
                                index += 1;
                            });
                        };
                    });
                }
            }
            else {
                divUsuarios.empty();
                divUsuarios.attr("style", "display:none;");
                if (e.keyCode == 13) {  //si se pulsa el enter con menos de tres caracteres, se busca lo que haya
                    if (bDoPostback) {
                        var str = controlPostback.toString().replace(/_/gi, '$');
                        __doPostBack(str, '');
                        return false;
                    } else
                        return false;
                }
            };
        });
    });
}


//Añade el usuario
function addUser(id, user, doPostBack, controlPostback, text, hidden, div, divImage) {
    var texto = $('#' + text);
    var inputUsuario = $('#' + hidden);
    var divUsuarios = $('#' + div);

    var imagenSeleccion = $('#' + divImage);

    //Imagen de usuario correcto o incorrecto
    if (imagenSeleccion && imagenSeleccion.hasClass('imagen-no-seleccionado')) {
        imagenSeleccion.toggleClass("imagen-no-seleccionado");
        imagenSeleccion.toggleClass("imagen-seleccionado");
    }

    texto.attr("value", user);
    inputUsuario.attr("value", id);
    divUsuarios.attr("style", "display:none;");
    if (doPostBack) {
        var str = controlPostback.toString().replace(/_/gi, '$');
        __doPostBack(str, '');
    }


    //else {
    //        return true;
    //    }
}