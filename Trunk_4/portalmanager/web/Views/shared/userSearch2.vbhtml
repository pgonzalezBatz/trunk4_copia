@imports web
    
<label id="lresponsable">
            @Html.Hidden("responsable")
            <span id="sresponsable">
                @html.display("nombreResponsable")
            </span>
            <a id="cambiarresponsable" href="#">@h.traducir("Cambiar responsable")</a>
        </label>
<script type="text/javascript" src="@Url.Content("~/Scripts/textbox_search.js")"></script>
<script type="text/javascript" src="@Url.Content("~/Scripts/query_string.js")"></script>
<script type="text/javascript">
    $(function () {
        var url2 = '@Url.Action("GetListOfUsuario", "ajax")'+'?term=';
        $('#cambiarresponsable').click(function () {
            $(this).hide();
            $('#lresponsable').append('<p>@h.traducir("Seleccionar nuevo responsable") <input type="text" id="tresponsable"/></p>');
            $('#tresponsable').focus();
            function oneElementToAppend(o) {
                var newUri = updateQueryStringParameter(document.URL, "idsab", o.IdSab.toString());
                window.location = newUri;
            };
            function manyElementsToAppend(container, o, f) {
                var newUri = updateQueryStringParameter(document.URL, "idsab", o.IdSab.toString());
                container.append('<a id="user' + o.IdSab.toString() + '" href="'+newUri+'">' + o.Nombre + '</a><br/>');
                f($('#user' + o.IdSab.toString()), function () { window.location = newUri });
            };
            
            textboxSearch(url2, $('#tresponsable'), oneElementToAppend, manyElementsToAppend);
        });
    });
    </script>