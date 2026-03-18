@imports web
@Html.ValidationSummary()
<form action="" method="post">
        <label>
            @h.Traducir("Etiqueta")<br />
            @Html.TextBox("idetiqueta")
        </label>
        <br />
        <input type="submit" value="@h.Traducir("Guardar")" />
</form>
<script type="text/javascript">
    document.getElementById("idetiqueta").focus();
</script>