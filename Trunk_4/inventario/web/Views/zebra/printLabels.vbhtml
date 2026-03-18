@imports web
@Code
    ViewBag.title = "Busqueda"
End code
    <div id="notifications">
        Desde aqui se puede mandar una impresion de etiquetas autonumeradas a la Zebra. Para reimprimir etiquetas seguir <a href="@Url.Action("reprintLabels")">este link</a>
    </div>
    @Html.ValidationSummary()
    <form action="" method="post">
        <label>
            @h.Traducir("Nº de lineas a imprimir (Cada linea imprime 3 etiquetas)")<br />
            @Html.TextBox("lineas")
        </label><br />
        <br />
        <input type="submit" value="@h.Traducir("Imprimir")"  />
    </form>