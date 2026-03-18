@modeltype VMVectorViaje

<div class="">
    <h4>@h.traducir("Origen")</h4>
    @Html.EditorFor(Function(m) m.PuntoOrigen)
</div>
<div>
    <h4>@h.traducir("Destino")</h4>
    @Html.EditorFor(Function(m) m.PuntoDestino)
</div>