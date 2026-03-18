@imports web
@modeltype VMAreaRecursoEdit

<div class="form-group">
    <h4>@Model.NombreArea</h4>
    @Html.EditorFor(Function(m) m.ListOfRecurso)
</div>