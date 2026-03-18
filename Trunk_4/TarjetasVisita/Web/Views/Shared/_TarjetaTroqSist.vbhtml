@Code
    Layout = Nothing
    Dim imagenTarjeta As String = CStr(ViewData("ImagenTarjeta"))
End Code

<div class="container-tarjeta-troq-sist">
    <img src="~/Content/Images/@imagenTarjeta" style="width:60%;border:1px solid black;">
    <div class="nombre-troq-sist">
        <label id="lblNombre">nombre</label>
    </div>
    <div class="puesto-troq-sist">
        <label id="lblPuesto">puesto</label>
    </div>
    <div class="email-troq-sist">
        <label id="lblEmail">email</label>
    </div>
    <div class="movil-troq-sist">
        <label id="lblMovil">email</label>
    </div>
    <div class="fijo-troq-sist">
        <label id="lblFijo">email</label>
    </div>
    <div class="direccion-troq-sist">
        <label id="lblDireccion">dirección</label>
    </div>
</div>
