function proveedores(baseUrl, searchBoxId, hideHelper, displayHelper, noElementsToAppend, oneElementToAppend, manyElementsToAppend) {
    var urlProveedor = baseUrl + '/BuscarTrabajador?q=';
    var searchBox = $('#' + searchBoxId);
    $(document).ready(function () {
        //anchor click
        var bindTrabajadorBehaviors = function (id, name, f) {
            $('#tra' + id).click(function () {
                f(id, name);
            });
        };
        //prevent form submit
        searchBox.keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
            };
        });
        
        //search trabajadores via JSON
        searchBox.keyup(function (e) {
            clearTimeout($.data(this, 'timer'));
            var a = this.value;
            $(this).data('timer',setTimeout(function () {
                if (a.length > 2) {
                    $.getJSON(urlProveedor + a, function (json) {
                        if (json.length == 0) {
                            noElementsToAppend();
                        }
                        else {
                            if (json.length == 1 & e.keyCode == 13) {
                                OneElementToAppend(json[0].id, json[0].nombre + ' ' + json[0].apellido1 + ' ' + json[0].apellido2);
                                e.preventDefault();
                            }
                            else {
                                displayHelper();
                                $.each(json, function (key, val) {
                                    ManyElementsToAppend(val.id, val.nombre + ' ' + val.apellido1 + ' ' + val.apellido2, bindTrabajadorBehaviors)
                                });
                            };
                        };
                    });
                }
                else {
                    hideHelper();
                };
            }, 700));
        });
    });
};

