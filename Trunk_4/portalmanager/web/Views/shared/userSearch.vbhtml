@imports web
    
<label id="lresponsable">
            @h.traducir("Responsable")
            <br />
            @Html.Hidden("responsable")
            <span id="sresponsable">
                @html.display("nombreResponsable")
            </span>
            <a id="cambiarresponsable" href="#">@h.traducir("Cambiar responsable")</a>
        </label>
<script type="text/javascript" src="@Url.Content("~/Scripts/textbox_search.js")"></script>
<script type="text/javascript">
    $(function () {
        var url2 = '@Url.Action("GetListOfUsuario", "ajax")'+'?term=';
        $('#cambiarresponsable').click(function () {
            $(this).hide();
            $('#lresponsable').append('<p>@h.traducir("Seleccionar nuevo responsable") <input type="text" id="tresponsable"/></p>');
            function oneElementToAppend (o) {
                clickUser(o);
            };
            function manyElementsToAppend (container, o, f) {
                container.append('<a id="user' + o.IdSab.toString() + '" href="#">' + o.Nombre + '</a><br/>');
                f($('#user' + o.IdSab.toString()), function () { clickUser(o) });
            };
            function clickUser(o) {
                $('#responsable').attr("value", o.IdSab);
                $('#sresponsable').empty();
                $('#sresponsable').append(o.Nombre);
                $('#lresponsable p').remove();
                $('#cambiarresponsable').show();
            }
            textboxSearch(url2, $('#tresponsable'), oneElementToAppend, manyElementsToAppend);
        });
    });
    </script>