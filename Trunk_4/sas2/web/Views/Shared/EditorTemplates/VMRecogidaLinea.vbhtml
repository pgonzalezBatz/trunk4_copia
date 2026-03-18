@modeltype VMRecogidaLinea

<div class="form-group">
    <label>@h.traducir("OF - OP")</label>
    @Html.TextBox("Numord", Nothing, New With {.class = "form-control typeahead", .autocomplete = "off", .spellcheck = "off",
                                                                                                                                                                                                                .placeholder = h.traducir("Buscar OF"),
                                                                                                                                                                                                                .title = h.traducir("OF")})

    @Html.DropDownListFor(Function(m) m.Numope, Model.ListOfNumope, New With {.class = "form-control"})
</div>
    <div Class="form-group" style="width:100%;">
        <Label>@h.traducir("Peso")</Label>
        @Html.TextBoxFor(Function(m) m.Peso, New With {.class = "form-control"})
    </div>

    <div class="form-group">
        <label>@h.traducir("Zona de Entrega")</label>
        @Html.DropDownListFor(Function(m) m.ZonaEntrega, Model.ListOfZonaEntrega, New With {.class = "form-control"})
    </div>