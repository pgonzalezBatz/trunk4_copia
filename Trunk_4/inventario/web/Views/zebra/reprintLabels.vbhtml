@imports web
@Code
    ViewBag.title = "Busqueda"
End code
    <div id="notifications">
        Esto reimprimira cualquier numero de etiqueta que se inserte en el recuadro. Aplicar este uso con mucho cuidado.
    </div>
    @Html.ValidationSummary()
    <form action="" method="post">
        <label>
            @h.Traducir("Nº de etiqueta a reimprimir")<br />
            @Html.TextBox("idetiqueta")
        </label><br />
        <br />
        <input type="submit" value="@h.Traducir("Imprimir")"  />
    </form>