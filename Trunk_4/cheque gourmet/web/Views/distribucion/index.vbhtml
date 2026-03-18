@ModelType Distribucion
@imports web

@section header
    <title>@h.traducir("Distribución de cheques gourmet")</title>
End section


@Html.ValidationSummary()
<form action="@Url.Action("confirmardistribucion")" method="post">
    <strong>@h.traducir("Código de trabajador")</strong><br />
    @Html.TextBox("idTrabajador") <br />
    <strong>@h.traducir("Tipo de talonario")</strong><br />
    @For Each g In ViewData("listOfTipos")
         @<div class="dtipo">
        @Select Case g.key
            Case "S"
                        @<i>Ayudas Batz (Erabilera Mugatua)</i>
            Case "C"
                        @<i>A cuenta del trabajador</i>
        End Select
    <br />
    @For Each r In g
            @If Request("idtipo") IsNot Nothing AndAlso Request("idtipo") = r.id.ToString Then
                @<input type="radio" name="idtipo" value="@r.id" checked="checked" />
            Else
                @<input type="radio" name="idtipo" value="@r.id" />
            End If
            @r.nombre @html.Encode("(")@r.precio.ToString()@html.Encode("€)")
            @<br/>
        Next
    </div>
    Next
<br style="clear:left;" /><br />
    <strong>@h.traducir("Lectura primera página")</strong><br />
    @Html.TextBoxFor(Function(m) m.Lectura1, New With {.onKeyPress = "return disableEnterKey(event)", .class = "wide"})<br />
    <strong>@h.traducir("Lectura última página")</strong><br />
    @Html.TextBoxFor(Function(m) m.Lectura2, New With {.onKeyPress = "return disableEnterKey(event)", .class = "wide"}) <br />
    <input id="guardar" type="submit" value="@h.traducir("Guardar")" />
</form>
<script type="text/javascript">
    document.getElementById('idTrabajador').focus();
    function disableEnterKey(e) {
        var key;
        if (window.event)
            key = window.event.keyCode; //IE
        else
            key = e.which; //firefox
        if (key == 13) {
            if ((e.target || e.srcElement).id == "Lectura1") { //explorer e.srcElement, non rest of the world e.target
                document.getElementById('Lectura2').focus();
            };
            if ((e.target || e.srcElement).id == "Lectura2") {
                document.getElementById('guardar').focus();
            };
        };
        return (key != 13);
    }
</script>