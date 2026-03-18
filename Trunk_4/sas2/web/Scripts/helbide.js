function init(baseUrl,text,  div,  path) {
    var urlHelbideBuscar = baseUrl+'/buscar?q=';
    var urlHelbideCrear = '//intranet2.batz.es/helbide/admin/crear';
    $(document).ready(function () {
        var divDirecciones = $('#' + div);

        $('#' + text).keyup(function (e) {
            if (this.value.length > 3) {
                $.getJSON(urlHelbideBuscar + this.value, function (json) {
                    console.log(json);
                    divDirecciones.attr("style", "display:block;background-color:#EEE;border:1px solid #8FABFF;position:absolute;width:400px; height:200px; overflow:auto;");
                    divDirecciones.empty();
                    if (json.length == 0) {
                        divDirecciones.append('<h4>No se puedo encontrar ninguna dirección</h4><a href="' + urlHelbideCrear + '" target="_blank">crear nueva dirección</a>');
                    }
                    else {
                        console.log(json);
                        $.each(json, function (key, val) {
                            divDirecciones.append('&raquo;<a href="' + path + val.Id + '">' + val.Calle + ', ' + val.CodigoPostal + ', ' + val.Poblacion + '</a><br/>');
                        });
                    };
                });
            }
            else {
                divDirecciones.empty();
                divDirecciones.attr("style", "display:none;");
            };
        });
    });
}
