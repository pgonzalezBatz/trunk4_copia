function init(text, hidden, div, urlBuscar, bDoPostback, controlPostback, divImage, tableStyle, cellId, cellValue, pMinLength, maxDivHeight, opcion,param1,param2,hiddenFactory,cellFactory) {    
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
        var inputFactoryText = $('#' + hiddenFactory);
        var divResult = $('#' + div);
        var imagenSeleccion = $('#' + divImage);

        //Añade el registro
        function addRegister(obj) {
            var valueName = "obj." + cellValue;
            var idName = "obj." + cellId;
            var idFactory = "obj." + cellFactory;
            texto.attr("value", eval(valueName));
            inputText.attr("value", eval(idName));            
            inputFactoryText.attr("value", eval(idFactory));
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
        $('#myTable_' + text + ' tr').live('click', function (e) {
            var id, value, factory;           
            //debugger;
            //var myTd_old = $(this).find("[scope='id']")[0];
            //var myTd_old2 = $(this).find("[scope=\"id\"]")[0];
            //var myTd_old3 = $(this).find()[0];
            //var myTd_old4 = $(this)[0];
            //var myTd_old5 = $(this).find('[scope="id"]')[0];
            var myTd = $(this).children(0);

            id = myTd[0].textContent; //|| myTd[0].innerText;
            //myTd = $(this).find("[scope='value']")[0];
            value = myTd[1].textContent; //|| myTd(1).innerText;;
            texto.attr("value", value);
            inputText.attr("value", id);            
            //myTd = $(this).find("[scope='factory']")[0];
            factory = myTd[2].textContent; //|| myTd(2).innerText;;
            inputFactoryText.attr("value", factory);
            //Esto es necesario para cuando hay ajax
            document.getElementById(text).value = texto.val();
            document.getElementById(hidden).value = inputText.val();
            document.getElementById(hiddenFactory).value = inputFactoryText.val();
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
                return true;
            }
        });

        /*$('#' + text).focusout(function () {
            divResult.empty();
            divResult.attr("style", "display:none;");
        });*/

        $('#' + text).keyup(function (e) {
            clearTimeout($.data(this, 'timer'));
            var text1 = this.value;
            if (e.keyCode == 27)  //Scape
                text1 = ""; //Lo ponemos a blanco para que se oculte
            $(this).data('timer', setTimeout(function () {
                if (text1.length < minLength && imagenSeleccion) {
                    if (imagenSeleccion.hasClass('imagen-seleccionado')) {
                        imagenSeleccion.removeClass('imagen-seleccionado')
                        imagenSeleccion.addClass("imagen-no-seleccionado");
                    }
                    inputText.attr("value", '');
                    inputFactoryText.attr("value", '');
                }

                if (text1.length >= minLength) {
                    $.getJSON(UsuarioBuscar + text1 + '&o=' + opcion + '&p1=' + param1 + '&p2=' + param2, function (json) {
                        divResult.attr("style", "display:block;border:1px solid #8FABFF;position:absolute;");     
                        if (maxDivHeight != null) {
                            divResult.css("max-height", maxDivHeight + "px");
                            divResult.css("overflow-y", "auto");
                            divResult.css("overflow-x", "hidden");
                        }
                        divResult.empty();
                        if (json.length == 1) {  //Cabecera
                            divResult.empty();
                            divResult.attr("style", "display:none;");
                            //divResult.append('<h4>No se pudo encontrar ningun registro</h4>');
                        }
                        else {
                            var mytable = document.createElement("table");
                            mytable.setAttribute("id", "myTable_" + text)
                            mytable.setAttribute("class", tableStyle);
                            mytable.setAttribute("border", "0")
                            var mycurrent_cell, mycurrent_label, mycurrent;
                            var mycurrent_row = document.createElement("tr");
                            var cabecera = json[0];
                            //Pinta la cabecera
                            var colVisible;
                            for (var property in cabecera) {
                                colVisible = 1;
                                var infoCab = property.split("&");
                                if (infoCab.length > 1)
                                    if (infoCab[1] = "inv")
                                        colVisible = 0;
                                
                                mycurrent_cell = document.createElement("th");
                                if (colVisible == 0) {
                                    mycurrent_cell.setAttribute("style", "visibility:hidden;");
                                } else {
                                    mycurrent_label = document.createElement("span");
                                    mycurrent_label.innerHTML = cabecera[infoCab[0]];
                                    mycurrent_cell.appendChild(mycurrent_label);
                                }
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
                                        var columnVisible;
                                        for (var property in val) {                                            
                                            columnVisible = 1;
                                            var infoRow = property.split("&");
                                            if (infoRow.length > 1)
                                                if (infoRow[1] = "inv")
                                                    columnVisible = 0;
                                            
                                            if (infoRow[0] == "e") {
                                                if (val[infoRow[0]] == "1") {  //Dado de baja              
                                                    mycurrent_row.setAttribute("style", "background-color:#ff3333;")
                                                    mycurrent_row.setAttribute("class", "")  //Sino se pone esto, no se muestra la columna en rojo
                                                }
                                            } else {
                                                mycurrent_cell = document.createElement("td");
                                                if (columnVisible == 0)
                                                    mycurrent_cell.setAttribute("style", "visibility:hidden;");

                                                if (infoRow[0] == cellId) {
                                                    mycurrent_cell.setAttribute("scope", "id");
                                                }
                                                else if (infoRow[0] == cellValue) {
                                                    mycurrent_cell.setAttribute("scope", "value");
                                                }
                                                else if (infoRow[0] == cellFactory) {
                                                    mycurrent_cell.setAttribute("scope", "factory");
                                                }
                                                mycurrent_label = document.createElement("span");
                                                mycurrent_label.innerHTML = val[property];
                                                mycurrent_cell.appendChild(mycurrent_label);
                                                mycurrent_row.appendChild(mycurrent_cell);
                                            }
                                        }
                                        mytable.appendChild(mycurrent_row);     
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
                    //divResult.append('<h4>No se pudo encontrar ningun registro</h4>');                    
                };
            }, 400));
        });
    });

}
