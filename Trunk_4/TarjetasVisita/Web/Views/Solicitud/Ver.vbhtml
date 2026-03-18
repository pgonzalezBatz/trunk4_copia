@Imports TarjetasVisitaLib
@imports System.Globalization

@code
    Dim solicitud As ELL.Solicitud = CType(ViewData("Solicitud"), ELL.Solicitud)
    Dim imagenTarjeta As String = String.Empty
    Dim nombreVistaTarjeta As String = String.Empty

    If (solicitud.IdNegocio = ELL.Solicitud.NEGOCIO_SERVICIOS_GENERALES) Then
        imagenTarjeta = "tarjeta_group.jpg"
        nombreVistaTarjeta = "_TarjetaGroup.vbhtml"
    ElseIf (solicitud.IdNegocio = ELL.Solicitud.NEGOCIO_TROQUELERIA) Then
        imagenTarjeta = "tarjeta_tooling.jpg"
        nombreVistaTarjeta = "_TarjetaTroqSist.vbhtml"
    ElseIf (solicitud.idNegocio = ELL.Solicitud.NEGOCIO_SISTEMAS) Then
        imagenTarjeta = "tarjeta_automotive.jpg"
        nombreVistaTarjeta = "_TarjetaTroqSist.vbhtml"
    End If
    ViewData("ImagenTarjeta") = imagenTarjeta
End Code

<h3>@String.Format("{0} - {1}", Utils.Traducir("Solicitud de tarjetas"), Utils.Traducir("Ver"))</h3>
<hr />

<script type="text/javascript">
    $(function () {
        var separadorEmail = '&nbsp;&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;&nbsp;';
        var separadorDireccion = '&nbsp;&nbsp;//&nbsp;&nbsp;';
        $("#nombre").val('@Html.Raw(solicitud.Nombre)');
        $("#puesto").val('@Html.Raw(solicitud.Puesto)');
        $("#movil").val('@solicitud.Movil');
        $("#direccion").val('@Html.Raw(solicitud.Direccion)');
        $("#fijo").val('@solicitud.Fijo');
        $("#email").val('@solicitud.Email');

        $("#lblNombre").text('@Html.Raw(solicitud.Nombre)');
        $("#lblPuesto").text('@Html.Raw(solicitud.Puesto)');

        if (@solicitud.IdNegocio == @ELL.Solicitud.NEGOCIO_SERVICIOS_GENERALES) {
            if (@solicitud.Movil != '') {
                $("#lblEmail").html('@solicitud.Email' + separadorEmail + '(+34) @solicitud.Movil');
            } else {
                $("#lblEmail").html('@solicitud.Email');
            }
            $("#lblDireccion").html('@Html.Raw(solicitud.Direccion)' + separadorDireccion + 'T: (+34) @solicitud.Fijo');
        } else {
            $("#lblEmail").html('@solicitud.Email');
             if (@solicitud.Movil != '') {
                $("#lblMovil").html('M: (+34) @solicitud.Movil');
            } else {
                $("#lblEmail").html('');
            }
            $("#lblFijo").html('T: (+34) @solicitud.Fijo');
            $("#lblDireccion").html('@Html.Raw(solicitud.Direccion)');
        }

        $("#cajas").val('@solicitud.Cajas');
    });

</script>

<form class = "form-horizontal">
    <div class="form-group">
         <div class="col-sm-6">            
             <div Class="form-group">
                 <label class="col-sm-4 control-label">@Utils.Traducir("Nombre")</label>
                 <div class="col-sm-5">
                     @Html.TextBox("nombre", solicitud.Nombre, New With {.class = "form-control", .readonly = "readonly"})
                 </div>
             </div>
             <div Class="form-group">
                 <label class="col-sm-4 control-label">@Utils.Traducir("Puesto de trabajo")</label>
                 <div class="col-sm-5">
                     @Html.TextBox("puesto", solicitud.Puesto, New With {.class = "form-control", .readonly = "readonly"})
                 </div>
             </div>
             <div Class="form-group">
                 <label class="col-sm-4 control-label">@Utils.Traducir("Móvil")</label>
                 <div class="col-sm-5">
                     @Html.TextBox("movil", solicitud.Movil, New With {.class = "form-control", .readonly = "readonly"})
                 </div>
             </div>
             <div Class="form-group">
                 <label class="col-sm-4 control-label">@Utils.Traducir("Dirección")</label>
                 <div class="col-sm-5">
                     @Html.TextBox("direccion", solicitud.Direccion, New With {.class = "form-control", .readonly = "readonly"})
                 </div>
             </div>
             <div Class="form-group">
                 <label class="col-sm-4 control-label">@Utils.Traducir("Teléfono")</label>
                 <div class="col-sm-5">
                     @Html.TextBox("fijo", solicitud.Fijo, New With {.class = "form-control", .readonly = "readonly"})
                 </div>
             </div>
             <div Class="form-group">
                 <label class="col-sm-4 control-label">@Utils.Traducir("Email")</label>
                 <div class="col-sm-5">
                     @Html.TextBox("email", solicitud.Email, New With {.class = "form-control", .readonly = "readonly"})
                 </div>
             </div>
             <div Class="form-group">
                 <label class="col-sm-4 control-label">@Utils.Traducir("Nº cajas (200 uds/caja)")</label>
                 <div class="col-sm-5">
                     @Html.TextBox("cajas", solicitud.Cajas, New With {.class = "form-control", .readonly = "readonly"})
                 </div>
             </div>             
         </div>
         <div class="col-sm-6">
             @Html.Partial("~/Views/Shared/" & nombreVistaTarjeta, ViewData("ImagenTarjeta"))
         </div>
    </div>  

    </form>
