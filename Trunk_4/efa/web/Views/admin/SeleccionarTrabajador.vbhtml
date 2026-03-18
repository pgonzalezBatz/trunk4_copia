@imports web

@section header
    <title>@h.traducir("Trabajador")</title>
<style type="text/css">
    #contenido1 {text-align: left;}
</style>
End section

@section  beforebody
    <strong><a href="@Url.Action("Index")">@h.traducir("Volver")</a></strong>
    <br style="clear:both;" />
End Section


 @Html.ValidationSummary()
<span id="spanProveedor"></span>
<form action="" method="post">
    @Html.TextBox("box")
    @Html.Hidden("idSab")
    <input id="seleccionar" type="submit" value="Seleccionar" />
    <div id="helper"></div>
</form>

<script src="//intranet2.batz.es/baliabideorokorrak/jquery-1.5.1.min.js" type="text/javascript"></script>
<script src="../Scripts/JScript1.js" type="text/javascript"></script>
<script type="text/javascript">
    function HideHelper() {
        var divHelper = $('#helper');
        divHelper.empty();
        divHelper.attr("style", "display:none;");
    };
    function DisplayHelper() {
        var divHelper = $('#helper');
        divHelper.empty();
        divHelper.attr("style", "display:block;width:50%;height:200px;background-color:Gray;position:absolute;overflow:auto;background-color:#EEE;border:1px solid #8FABFF;");
    };
    function NoElemetsToAppend() {
        var divHelper = $('#helper');
        divHelper.empty();
        divHelper.attr("style", "display:block;width:50%;height:200px;background-color:Gray;position:absolute;overflow:auto;background-color:#EEE;border:1px solid #8FABFF;");
        divHelper.append('<h4>No se puedo encontrar ninguna empresa</h4>');
    };
    function OneElementToAppend(id, name) {
        f(id, name);
        $('#seleccionar').focus();
    };
    function ManyElementsToAppend(id, name, fun) {
        var divHelper = $('#helper');
        divHelper.append('<a id="tra' + id + '" href="#">' + name + '</a><br/>');
        fun(id, name, f);
    };
    function f(id, name) {
        var codtraBox = $('#idSab');
        var divHelper = $('#helper');
        var codNameTag = $('#spanProveedor');
        var searchBox = $('#box');
        codtraBox.attr("value", id);
        codNameTag.empty();
        codNameTag.attr('style', 'color:gray;');
        codNameTag.append(name);
        divHelper.empty();
        divHelper.attr("style", "display:none;");
        searchBox.attr('style', 'display:none');
    };
    proveedores('@Url.Content("~/json")', 'box', HideHelper, DisplayHelper, NoElemetsToAppend, OneElementToAppend, ManyElementsToAppend);
    //numord('<%=Url.Action("BuscarNumord","json") %>', 'Numord', 'divof','Numope');
    //numope('<%=Url.Action("BuscarNumope","json") %>', 'Numope','Numord')
</script>