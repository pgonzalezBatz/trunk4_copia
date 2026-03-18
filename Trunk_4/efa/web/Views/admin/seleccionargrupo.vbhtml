@imports web

@section header
    <title>@h.traducir("Grupos")</title>
End section

@For Each g In Model
@<a class="touch" href="@Url.Action("seleccionarrecurso", New With {.idsab = Request("idsab"), .grupo = g.nombre})">
    @g.Nombre
</a>
Next