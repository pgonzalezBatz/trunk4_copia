@imports web
    
<label>
    @h.Traducir("Usuario")<br />
    @Html.TextBox("user",Nothing,New With{.autocomplete="off"})
    @Html.Hidden("idsab")
    <br />
    <script src="//intranet2.batz.es/baliabideorokorrak/jquery-1.5.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="@Url.Content("~/Scripts/textbox_search.js")"></script>
    <script type="text/javascript">
        $(function () {
            var user = $("#user");
            function fOneElement(val) {
                clickUser(val);
            };
            function fManyElements(div, val, f) {
                div.append('<a id="user' + val.IdSab.toString() + '" href="#">' + val.Nombre + '</a><br/>');
                f($('#user' + val.IdSab.toString()), function () { clickUser(val) });
            };
            function clickUser(val) {
                $('#idsab').attr("value", val.IdSab);
                user.attr("value", val.Nombre);
                user.attr("style", "color:green;font-weight:bold;");
            };
            textboxSearch('@Url.Action("buscar", "ajax")?term=', user, fOneElement, fManyElements);
            //Edit cases
            if ($('#idsab').attr("value").length > 0) {
                $.getJSON('@Url.Action("GetUser", "ajax")' + '?idsab=' + $('#idsab').attr("value"), function (json) {
                    clickUser(json)
                })
            };
        });
    </script>
</label>
