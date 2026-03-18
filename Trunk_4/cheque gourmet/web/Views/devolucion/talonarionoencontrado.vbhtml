@imports web

@section header
    <title>@h.traducir("Talonario no encontrado")</title>
End section

<h2>
    @h.traducir("No se ha encontrado ningun talonario distribuido, al que pertenezca el cheque seleccionado")
</h2>
<a href="@Url.Action("index")">
    @h.traducir("Volver")
</a>