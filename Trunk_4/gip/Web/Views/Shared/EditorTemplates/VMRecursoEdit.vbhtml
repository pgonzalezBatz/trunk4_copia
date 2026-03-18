@imports web
@modeltype VMRecursoEdit

<div >
    @If Model.IsGrupoSabProveedores Then
        @Html.CheckBoxFor(Function(m) m.Seleccionado, New With {.disabled = "disabled"})
    Else
        @Html.CheckBoxFor(Function(m) m.Seleccionado)
    End If
    @Html.HiddenFor(Function(m) m.Grupo)
    @Html.DisplayFor(Function(m) m.NombreGrupo)
</div>