@modeltype VMPuntoViaje

<div class="input-group" style="width:100%;">
    <label>@h.traducir("Empresa Proveedora")</label>
    @Html.HiddenFor(Function(m) m.IdEmpresa)
    @Html.TextBoxFor(Function(m) m.TxtIdEmpresa, New With {.class = "form-control typeahead"})
</div>
<div class="input-group" style="width:100%;">
    <label>@h.traducir("Dirección (En caso de que la dirección sea diferente a la del proveedor o no haya proveedor)")</label>
    @Html.HiddenFor(Function(m) m.IdHelbide)
    @Html.TextBoxFor(Function(m) m.txtIdHelbide, New With {.class = "form-control typeahead"})
</div>
<div class="input-group" style="width:100%;">
    <label>@h.traducir("Nombre Empresa (En caso de que no sea proveedor)")</label>
    @Html.TextBoxFor(Function(m) m.NoEmpresa, New With {.class = "form-control"})
</div>