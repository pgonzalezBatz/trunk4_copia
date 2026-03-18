@imports web
    
<label>
    @h.traducir("Usuario Baja")<br />
    @Html.TextBox("userbaja", Nothing, New With {.autocomplete = "off"})
    @Html.Hidden("idsabbaja")
    <div id="r_userbaja"></div>
    <br />
    <script src="//intranet2.batz.es/baliabideorokorrak/jquery-1.5.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="@Url.Content("~/Scripts/textbox_search.js")"></script>
    <script type="text/javascript">
        $(function () {
            var user = $("#userbaja");
            function fOneElement(val) {
                clickUser(val);
            };
            function fManyElements(div, val, f) {
                div.append('<a id="userbaja' + val.IdSab.toString() + '" href="#">' + val.Nombre + '</a><br/>');
                f($('#userbaja' + val.IdSab.toString()), function () { clickUser(val) });
            };
            function clickUser(val) {
                $('#idsabbaja').attr("value", val.IdSab);
                $.getJSON('@Url.action("elementosactivos", "ajax")?idsab=' + val.IdSab)
                    .done(function (d) {
                        $('#r_userbaja').empty();
                        $.each(d,function (i, e) {
                            $('#r_userbaja').append('<p><input type="checkbox" name="etiquetas" value="' + e.idEtiqueta + '"/>' + e.nombreModelo + '(Id Etiqueta:'+e.idEtiqueta+')</p>');
                        });
                    });
                user.attr("value", val.Nombre);
                user.attr("style", "color:green;font-weight:bold;");
            };
            textboxSearch('@Url.Action("buscarBaja", "ajax")?term=', user, fOneElement, fManyElements);
            //Edit cases
            if ($('#idsabbaja').attr("value").length > 0) {
                $.getJSON('@Url.Action("GetUser", "ajax")' + '?idsab=' + $('#idsabbaja').attr("value"), function (json) {
                    clickUser(json)
                })
            };
        });
    </script>
</label>
