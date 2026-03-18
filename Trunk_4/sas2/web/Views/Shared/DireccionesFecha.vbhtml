@ModelType  VMMovimientoStep1
<div class="form-group">
    <label>@h.traducir("Fecha Prevista Transporte")</label>
    @Html.EditorFor(Function(m) m.Fecha)
</div>
<div class="form-group">
    <label>@h.traducir("Origen del transporte")</label>
    <div class="input-group">
        <label class="input-group-addon">
            @Html.RadioButton("esOrigenProveedor", "True", True)
            @h.traducir("Seleccionar Empresa como origen")
        </label>
        <label class="input-group-addon">
            @Html.RadioButton("esOrigenProveedor", "False")
            @h.traducir("Seleccionar dirección")
        </label>
    </div>





    @Html.Hidden("Origen", 1)
    @Html.TextBox("TxtOrigen", Nothing, New With {.class = "form-control typeahead", .autocomplete = "off", .spellcheck = "off",
                                                                                       .placeholder = h.traducir("Nombre Empresa"),
                                                                                       .title = h.traducir("Origen de la mercancia")})




    <label>@h.traducir("Empresa Proveedora Origen")</label>
    @Html.TextBox("TxtIdEmpresaOrigen", New With {.class = "form-control typeahead", .autocomplete = "off", .spellcheck = "off",
                                                                                                               .placeholder = h.traducir("Nombre Empresa"),
                                                                                                               .title = h.traducir("Origen de la mercancia")})

    @Html.Hidden("id_helbide_origen")
    <label>@h.traducir("Empresa No Proveedora Origen") @h.traducir(" (Rellenar solo en casos en los que el origen no sea provedor de Batz)")</label>
    @Html.TextBox("TxtNoEmpresaOrigen", New With {.class = "form-control typeahead"})

    <label>@h.traducir("Empresa No proveedora origen (Rellenar solo en casos en los que el origen no sea provedor de Batz)")</label>
    @Html.TextBox("TxthelbideOrigen")
</div>
<br />
<br />
<div class="form-group">
    <label>@h.traducir("Destino del transporte")</label>
    <div class="input-group">
        <label class="input-group-addon">
            @Html.RadioButton("EsDestinoProveedor", "True", True)
            @h.traducir("Seleccionar Empresa como destino")
        </label>
        <label class="input-group-addon">
            @Html.RadioButton("EsDestinoProveedor", "False")
            @h.traducir("Seleccionar dirección")
        </label>
    </div>
    @Html.TextBox("TxtDestino", Nothing, New With {.class = "form-control typeahead", .autocomplete = "off", .spellcheck = "off",
                                                                                   .placeholder = h.traducir("Nombre Empresa"),
                                                                                   .autofocus = "autofocus",
                                                                                   .title = h.traducir("Destino de la mercancia")})
    @Html.Hidden("Destino")
</div>
