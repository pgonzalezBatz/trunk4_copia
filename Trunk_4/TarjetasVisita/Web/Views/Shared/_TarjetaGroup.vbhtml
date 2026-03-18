@Code
    Layout = Nothing
    Dim imagenTarjeta As String = CStr(ViewData("ImagenTarjeta"))
End Code

<div class="container-tarjeta-group">
    <img src="~/Content/Images/@imagenTarjeta" style="width:60%;border:1px solid black;">
    <div class="nombre-group">
        <label id="lblNombre">nombre</label>
    </div>
    <div class="puesto-group">
        <label id="lblPuesto">puesto</label>
    </div>
    <div class="email-group">
        <label id="lblEmail">email</label>
    </div>
    <div class="direccion-group">
        <label id="lblDireccion">dirección</label>
    </div>
</div>
