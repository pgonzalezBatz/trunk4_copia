@imports web

@Html.ValidationSummary()
<form action="" method="post">
    <fieldset>
        <legend>@h.traducir("Añadir pregunta")</legend>
        <label>
            @h.traducir("Titulo")                            
            <br />
            @Html.TextBox("titulo")
        </label>
        <br />
        <label>
            @h.traducir("Descripción")
            <br />
            @Html.TextArea("descripcion")
        </label>
        <br />
        <label>
            @h.traducir("Tipo de pregunta")
            <br />
            @Html.DropDownList("tipopregunta")
        </label>
        <br />
        <label id="lblpeso">
            @h.traducir("Peso de la pregunta")
            <br />
            @Html.TextBox("peso")
        </label>
        <br />
    </fieldset>
    <input type="submit" value="@h.traducir("Guardar")" />
</form>
<script src="//intranet2.batz.es/baliabideorokorrak/jquery-1.5.1.min.js" type="text/javascript"></script>
<script type="text/javascript">
    function updateVisibililty() {
        if ($("#tipopregunta").attr('value') == "1") {
            $("#lblpeso").show();
        }
        else {
            $("#lblpeso").hide();
        };
    };
    $(document).ready(function () {
        updateVisibililty();
        $("#tipopregunta").change(function () {
            updateVisibililty();
        })
    });
</script>
