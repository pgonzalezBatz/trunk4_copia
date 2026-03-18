@modeltype VMVectorViaje



<div class="vector_viaje">
    @Html.DisplayFor(Function(m) m.PuntoOrigen)
    <span class="glyphicon glyphicon-chevron-right"></span>
    <span class="glyphicon glyphicon-send"></span>
    <span class="glyphicon glyphicon-chevron-right"></span>
    @Html.DisplayFor(Function(m) m.PuntoDestino)
</div>