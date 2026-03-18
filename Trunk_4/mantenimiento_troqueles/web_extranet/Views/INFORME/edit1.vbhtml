<h3>@h.traducir("Creación de informe")</h3>
<hr />

<h4>@h.traducir("Seleccionar Informe,  OF, Operación")</h4>

@Html.ValidationSummary(False, "", New With {.class = "alert alert-danger"})
<form action="" method="post">
    @Html.AntiForgeryToken()
    <div class="form-group">
        <label for="tipoSoldadura">@h.traducir("Tipo de informe")</label>
        @Html.DropDownList("tipoInforme", Nothing, New With {.class = "form-control"})
    </div>
    <div class="form-group">
        <label for="numord">@h.traducir("OF")</label>
        @Html.TextBox("numord", Nothing, New With {.class = "form-control"})
    </div>
    <div class="form-group">
        <label for="numord">@h.traducir("OP")</label>
        @Html.TextBox("numope", Nothing, New With {.class = "form-control"})
    </div>
    <div class="form-group">
        <input type="submit" value="@h.traducir("Continuar")" class="btn btn-primary " />
        <a href="@Url.Action("index", "INFORME", h.ToRouteValues(Request.QueryString, Nothing))">@h.traducir("Volver al listado")</a>
    </div>
</form>