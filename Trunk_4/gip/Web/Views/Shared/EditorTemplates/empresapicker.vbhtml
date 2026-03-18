@imports web
   @If Model Is Nothing Then
    @Html.Hidden("", Model)
    @Html.TextBox("txt", Nothing, New With {.autocomplete = "off"})
Else
    @code
            Dim u = db.GetProveedor(1, Model, ConfigurationManager.ConnectionStrings("oracle").ConnectionString)
    End Code
    @Html.Hidden("", Model)
    @Html.TextBox("txt", u.nombre, New With {.autocomplete = "off"})
End If

<script src="//intranet2.batz.es/baliabideorokorrak/jquery-1.11.0.min.js" type="text/javascript"></script>
<script src="//intranet2.batz.es/baliabideorokorrak/textbox_search.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        var user = $("#idEmpresa_txt");
        function fOneElement(val) {
            clickUser(val);
        };
        function fManyElements(div, val, f) {
            div.append('<a id="supplier' + val.id.toString() + '" href="#">' + val.nombre +  '</a><br/>');
            f($('#supplier' + val.id.toString()), function () { clickUser(val) });
        };
        function clickUser(val) {
            $('#idEmpresa').attr("value", val.id);
            user.val(val.nombre);
        };
        textboxSearch('@Url.Action("Search", "proveedor")?q=', user, fOneElement, fManyElements);
    });
</script>