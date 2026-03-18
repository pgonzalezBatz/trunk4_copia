@ModelType web.Helbide
@imports web

@section title
@h.Traducir("Editar dirección")
End Section

<h3>@h.Traducir("Editar dirección")</h3>

 @Html.ValidationSummary()
<form method = "post" action="">
        <input type = "hidden" name="direccionRetorno" value="@(If(Request.UrlReferrer Is Nothing, " ", Request.UrlReferrer.AbsoluteUri))" />
        @Html.HiddenFor(Function(m) m.Id)
        <div Class="form-group">
            <Label>@h.traducir("Nº, calle")</Label>
            @Html.TextBoxFor(Function(m) m.Calle, Nothing, New With {.class = "form-control"})
        </div>
        <div class="form-group">
            <label>@h.traducir("Código postal")</label>
            @Html.TextBoxFor(Function(m) m.CodigoPostal, Nothing, New With {.class = "form-control"})
        </div>
        <div class="form-group">
            <label>@h.traducir("Población")</label>
            @Html.TextBoxFor(Function(m) m.Poblacion, Nothing, New With {.class = "form-control"})
        </div>
        <div class="form-group">
            <label>@h.traducir("Provincia")</label>
            @Html.TextBoxFor(Function(m) m.Provincia, Nothing, New With {.class = "form-control"})
        </div>
        <div class="form-group">
            <label>@h.traducir("Pais")</label>
            @Html.DropDownListFor(Function(m) m.Pais, CType(ViewData("listOfPais"), IEnumerable(Of SelectListItem)), h.traducir("Seleccionar país"), New With {.class = "form-control"})
        </div>

        <input type="submit" value="@h.traducir(" Guardar")" class="btn btn-primary" />
</form>