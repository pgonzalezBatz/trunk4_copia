@modeltype VMPuntoViaje


@If Not String.IsNullOrEmpty(Model.NoEmpresa) Then
        @<strong> @Html.DisplayFor(Function(m) m.NoEmpresa)</strong>
End If

@If Model.IdEmpresa.HasValue Then
        @<strong> @Html.DisplayFor(Function(m) m.TxtIdEmpresa)</strong>
        @Html.HiddenFor(Function(m) m.IdEmpresa)
End If

@If Model.IdHelbide Then
        @Html.DisplayFor(Function(m) m.txtIdHelbide)
        @Html.HiddenFor(Function(m) m.IdHelbide)
End If
