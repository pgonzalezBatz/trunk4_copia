function init(text, hidden, div, urlBuscar, bDoPostback, controlPostback, divImage, tableStyle, cellId, cellValue, pMinLength, maxDivHeight, opcion,soloActivos,idPlanta) {
    var UsuarioBuscar = urlBuscar;
    if (urlBuscar.indexOf('?') == -1)
        UsuarioBuscar = UsuarioBuscar + '?q=';
    else
        UsuarioBuscar = UsuarioBuscar + '&q=';
    if (pMinLength != null)
        minLength = pMinLength;

    $(document).ready(function () {
        var texto = $('#' + text);
        var inputText = $('#' + hidden);
        var divResult = $('#' + div);
        var imagenSeleccion = $('#' + divImage);

        var bindBehaviors = function (val) {
            $('#' + val.id).click(function () {
                var valueName = "obj." + cellValue;
                var idName = "obj." + cellId;
                texto.attr("value", eval(valueName));
                inputText.attr("value", eval(idName));
                divResult.attr("style", "display:none;");
                if (imagenSeleccion && imagenSeleccion.hasClass('imagen-no-seleccionado')) {
                    imagenSeleccion.removeClass("imagen-no-seleccionado");
                    imagenSeleccion.addClass("imagen-seleccionado");
                }
                if (bDoPostback == "True") {
                    var str = controlPostback.toString().replace(/_/gi, '$');
                    __doPostBack(str, '');
                    return false;
                } else {
                    return false;
                }
            });
        };

        //Añade el registro
        function addRegister(obj) {
            var valueName = "obj." + cellValue;
            var idName = "obj." + cellId;
            texto.attr("value", eval(valueName));
            inputText.attr("value", eval(idName));
            divResult.attr("style", "display:none;");
            if (bDoPostback == "True") {
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

        //Captura el click en la fila y añade el registro
        $('#myTable tr').live('click', function () {
            var id, value;
            var myTd = $(this).find("[scope='id']")[0]
            id = myTd.textContent || myTd.innerText;
            myTd = $(this).find("[scope='value']")[0];
            value = myTd.textContent || myTd.innerText;;
            texto.attr("value", value);
            inputText.attr("value", id);
            //Esto es necesario para cuando hay ajax
            document.getElementById(text).value = texto.val();
            document.getElementById(hidden).value = inputText.val();
            divResult.attr("style", "display:none;");
            if (imagenSeleccion && imagenSeleccion.hasClass('imagen-no-seleccionado')) {
                imagenSeleccion.removeClass("imagen-no-seleccionado");
                imagenSeleccion.addClass("imagen-seleccionado");
            }
            if (bDoPostback == "True") {
                var str = controlPostback.toString().replace(/_/gi, '$');
                __doPostBack(str, '');
                return false;
            } else {
                return false;
            }
        });

        $('#' + text).keyup(function (e) {
            clearTimeout($.data(this, 'timer'));
            var text = this.value;
            $(this).data('timer', setTimeout(function () {
                if (text.length < minLength && imagenSeleccion) {
                    if (imagenSeleccion.hasClass('imagen-seleccionado')) {
                        imagenSeleccion.removeClass('imagen-seleccionado')
                        imagenSeleccion.addClass("imagen-no-seleccionado");
                    }
                    inputText.attr("value", '');
                }

                if (text.length >= minLength) {
                    $.getJSON(UsuarioBuscar + text + '&o=' + opcion + '&sa=' + soloActivos + '&idP=' + idPlanta, function (json) {
                        divResult.attr("style", "display:block;border:1px solid #8FABFF;position:absolute;");
                        if (maxDivHeight != null) {
                            divResult.css("max-height", maxDivHeight + "px");
                            divResult.css("overflow-y", "auto");
                            divResult.css("overflow-x", "hidden");
                        }
                        divResult.empty();
                        if (json.length == 1) {  //Cabecera
                            divResult.append('<h4>No se pudo encontrar ningun registro</h4>');
                        }
                        else {
                            var mytable = document.createElement("table");
                            mytable.setAttribute("id", "myTable")
                            mytable.setAttribute("class", tableStyle);
                            mytable.setAttribute("border", "0")
                            var mycurrent_cell, mycurrent_label, mycurrent;
                            var mycurrent_row = document.createElement("tr");
                            var cabecera = json[0];
                            //Pinta la cabecera
                            for (var property in cabecera) {
                                mycurrent_cell = document.createElement("th");
                                mycurrent_label = document.createElement("span");
                                mycurrent_label.innerHTML = cabecera[property];
                                mycurrent_cell.appendChild(mycurrent_label);
                                mycurrent_row.appendChild(mycurrent_cell);
                            }
                            mytable.appendChild(mycurrent_row);
                            var index = 0;
                            $.each(json, function (key, val) {
                                //La primera fila es la cabecera
                                if (index > 0) {
                                    if (e.keyCode == 13 && json.length == 2) //if this is enter key. Es 2 porque tambien cuenta la cabecera 
                                    {
                                        addRegister(val);
                                        if (imagenSeleccion && imagenSeleccion.hasClass('imagen-no-seleccionado')) {
                                            imagenSeleccion.removeClass("imagen-no-seleccionado");
                                            imagenSeleccion.addClass("imagen-seleccionado");
                                        }
                                    }
                                    else {
                                        mycurrent_row = document.createElement("tr");
                                        if (index % 2 == 0) {
                                            mycurrent_row.setAttribute("class", "alt")
                                        }
                                        mycurrent_row.title = 'Click to select';
                                        for (var property in val) {                                            
                                            if (property == "e") {
                                                if (val[property] == "1") {  //Dado de baja              
                                                    mycurrent_row.setAttribute("style", "background-color:#ff3333;")
                                                    mycurrent_row.setAttribute("class", "")  //Sino se pone esto, no se muestra la columna en rojo
                                                }
                                            } else {
                                                mycurrent_cell = document.createElement("td");
                                                if (property == cellId) {
                                                    mycurrent_cell.setAttribute("scope", "id");
                                                }
                                                else if (property == cellValue) {
                                                    mycurrent_cell.setAttribute("scope", "value");
                                                }
                                                mycurrent_label = document.createElement("span");
                                                mycurrent_label.innerHTML = val[property];
                                                mycurrent_cell.appendChild(mycurrent_label);
                                                mycurrent_row.appendChild(mycurrent_cell);
                                            }                                           
                                        }
                                        mytable.appendChild(mycurrent_row);
                                        bindBehaviors(val);
                                    }
                                }
                                index = index + 1;
                            });
                            divResult.append(mytable.outerHTML);
                        };
                    });
                }
                else {
                    divResult.empty();
                    divResult.attr("style", "display:none;");
                };
            }, 400));
        });
    });

}
