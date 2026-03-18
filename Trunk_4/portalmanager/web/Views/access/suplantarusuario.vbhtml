@imports web
@Code
    ViewBag.title = "Suplantar usuario"
End Code

<h3 class="mb-2">Suplantar Usuario</h3>
<form method="post" action="">
        <div id="dspnewidsab">

        </div>
        @Html.Hidden("newidsab")
        @Html.TextBox("txtnewidsab", Nothing, New With {.autocomplete = "off", .class = "form-control"})
        <input type="submit" value="Suplantar" class="btn btn-primary"/>

</form>
<script src="//intranet2.batz.es/baliabideorokorrak/jquery-1.5.1.min.js" type="text/javascript"></script>
<script type="text/javascript" src="@Url.Content("~/Scripts/textbox_search.js")"></script>
<script type="text/javascript">
    $(function () {
        var url2 = '@Url.Action("GetListOfUsuario", "ajax")' + '?term=';
        function oneElementToAppend(o) {
            clickUser(o);
        };
        function manyElementsToAppend(container, o, f) {
            container.append('<a id="user' + o.IdSab.toString() + '" href="#">' + o.Nombre + '</a><br/>');
            f($('#user' + o.IdSab.toString()), function () { clickUser(o) });
        };
        function clickUser(o) {
            $('#newidsab').attr("value", o.IdSab);
            $('#dspnewidsab').empty();
            $('#dspnewidsab').append(o.Nombre);
        }
        textboxSearch(url2, $('#txtnewidsab'), oneElementToAppend, manyElementsToAppend);
    });
</script>