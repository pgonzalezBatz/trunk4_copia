@modeltype VMPuntoViaje


@If Not String.IsNullOrEmpty(Model.NoEmpresa) Then
    @If ViewData("InsideTable") Is Nothing Then
        @<div Class="form-group">
            <Label>@h.traducir("Nombre Empresa (En caso de que no sea proveedor)")</Label>
            @Html.DisplayFor(Function(m) m.NoEmpresa)
        </div>
    Else
        @<p><strong> @Html.DisplayFor(Function(m) m.NoEmpresa)</strong></p>
    End If

End If


@If Model.IdEmpresa.HasValue Then
    @If ViewData("InsideTable") Is Nothing Then
        @<div Class="form-group">
            <Label>@h.traducir("Empresa Proveedora")</Label>
            <div class="row">
                <div class="col-xs-12">
                    @Html.HiddenFor(Function(m) m.IdEmpresa)
                    @Html.DisplayFor(Function(m) m.TxtIdEmpresa)
                </div>
            </div>
        </div>
    Else
        @<p><strong> @Html.DisplayFor(Function(m) m.TxtIdEmpresa)</strong></p>
        @Html.HiddenFor(Function(m) m.IdEmpresa)
    End If
End If
@If Model.IdHelbide Then
    @If ViewData("InsideTable") Is Nothing Then
        @<div Class="input-group" style="width:100%;">
            <Label>@h.traducir("Dirección (En caso de que la dirección sea diferente a la del proveedor o no haya proveedor)")</Label>
            <div class="row">
                <div class="col-xs-12">
                    @Html.HiddenFor(Function(m) m.IdHelbide)
                    @Html.DisplayFor(Function(m) m.txtIdHelbide)
                </div>
            </div>
        </div>
    Else
        @<p>@Html.DisplayFor(Function(m) m.txtIdHelbide)</p>
        @Html.HiddenFor(Function(m) m.IdHelbide)
    End If

End If
