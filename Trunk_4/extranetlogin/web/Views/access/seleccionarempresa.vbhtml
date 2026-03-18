@imports web

<h4>
    @h.traducir("El sistema ha detectado que su usuario pertenece a varios proveedores. Seleccione el proveedor al que desea conectarse:")
</h4>
@For Each m In Model
    @<form action="" method="post">
    @Html.Hidden("idempresa", m.id)
    @Html.Hidden("nombreusuario", m.nombreusuario)
    <a class="btn btn-info" style="margin-bottom:10px;" href="@Url.Action("SeleccionarEmpresa2", "access", New With {.idempresa = m.id, .nombreusuario = m.nombreusuario, .tipo = m.tipo}, Nothing)">@String.Concat(h.traducir(m.nombre).ToUpper, " (", m.nombreplanta, ")")</a>
</form>
Next
