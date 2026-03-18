@Imports TarjetasVisitaLib
@imports System.Globalization

@code
    Const telCentralita As String = "(+34)946 305 000"

    Dim ticket As SabLib.ELL.Ticket = CType(Session("Ticket"), SabLib.ELL.Ticket)
    Dim nombrePuesto As String = String.Empty
    Dim puesto As ELL.Puesto = CType(ViewData("Puesto"), ELL.Puesto)
    If (puesto IsNot Nothing AndAlso puesto.Nombre IsNot Nothing) Then
        nombrePuesto = puesto.Nombre
    End If
    Dim telefonos As ServicioTelefonia.Telephone = CType(ViewData("Telefonos"), ServicioTelefonia.Telephone)
    Dim movil As String = If(telefonos IsNot Nothing AndAlso Not String.IsNullOrEmpty(telefonos.Movil), telefonos.Movil, "")
    Dim origenDatosAlternativos As Integer = 0
    Dim datosAlternativos As ELL.DatosAlternativos = CType(ViewData("DatosAlternativos"), ELL.DatosAlternativos)
    If (datosAlternativos.Id <> Integer.MinValue) Then
        origenDatosAlternativos = 1
    End If
    'Dim telefonoFijo As String = If(String.IsNullOrEmpty(telefonos.Fijo), telCentralita, telefonos.Fijo)
    Dim telefonoFijo As String = telCentralita
    If telefonos IsNot Nothing AndAlso Not String.IsNullOrEmpty(telefonos.Fijo) Then
        telefonoFijo = telefonos.Fijo
    End If

    Dim idNegocio As String = CStr(ViewData("IdNegocio"))
    Dim imagenTarjeta As String = String.Empty
    Dim nombreVistaTarjeta As String = String.Empty

    Dim direccion As String = String.Empty
    If (idNegocio = ELL.Solicitud.NEGOCIO_SERVICIOS_GENERALES) Then
        imagenTarjeta = "tarjeta_group.jpg"
        nombreVistaTarjeta = "_TarjetaGroup.vbhtml"
        direccion = "Torrea, 2 - 48140 IGORRE"
    ElseIf (idNegocio = ELL.Solicitud.NEGOCIO_TROQUELERIA) Then
        imagenTarjeta = "tarjeta_tooling.jpg"
        nombreVistaTarjeta = "_TarjetaTroqSist.vbhtml"
        direccion = "Torrea, 2 - 48140 IGORRE - SPAIN"
    ElseIf (idNegocio = ELL.Solicitud.NEGOCIO_SISTEMAS) Then
        imagenTarjeta = "tarjeta_automotive.jpg"
        nombreVistaTarjeta = "_TarjetaTroqSist.vbhtml"
        direccion = "Torrea, 2 - 48140 IGORRE - SPAIN"
    End If
    ViewData("ImagenTarjeta") = imagenTarjeta
End Code

<h3>@String.Format("{0} - {1}", Utils.Traducir("Solicitud de tarjetas"), Utils.Traducir("Nueva"))</h3>
<hr />

<script type="text/javascript">
    $(function () {
        var separadorEmail = '&nbsp;&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;&nbsp;';
        var separadorDireccion = '&nbsp;&nbsp;//&nbsp;&nbsp;';
        if (@origenDatosAlternativos == 1) {
            $("#divIdioma, #divNombreDosApellidos, #divMostrarMovil").hide();
            $("#nombre").val('@Html.Raw(datosAlternativos.Nombre)');
            $("#puesto").val('@Html.Raw(datosAlternativos.Puesto)');
            $("#movil").val('@datosAlternativos.Movil');
            $("#direccion").val('@Html.Raw(datosAlternativos.Direccion)');
            $("#fijo").val('@datosAlternativos.Fijo');
            $("#email").val('@datosAlternativos.Email');

            if (@idNegocio == @ELL.Solicitud.NEGOCIO_SERVICIOS_GENERALES) {
                $("#lblNombre").text('@Html.Raw(datosAlternativos.Nombre)');
                $("#lblPuesto").text('@Html.Raw(datosAlternativos.Puesto)');
                if ('@datosAlternativos.Movil' == '') {
                    $("#lblEmail").html('@datosAlternativos.Email');
                } else {
                    $("#lblEmail").html('@datosAlternativos.Email' + separadorEmail + '(+34) @datosAlternativos.Movil');
                }

                $("#lblDireccion").html('@Html.Raw(datosAlternativos.Direccion)' + separadorDireccion + 'T: (+34) @datosAlternativos.Fijo');
            } else {
                $("#lblNombre").text('@Html.Raw(datosAlternativos.Nombre)');
                $("#lblPuesto").text('@Html.Raw(datosAlternativos.Puesto)');
                $("#lblEmail").html('@datosAlternativos.Email');
                $("#lblMovil").html('@datosAlternativos.Movil');
                $("#lblFijo").html('@datosAlternativos.Fijo');
                $("#lblDireccion").html('@Html.Raw(datosAlternativos.Direccion)');
            }
        } else {
            $("#nombre").val('@Html.Raw(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(ticket.NombreCompleto.ToLower()))');
            $("#puesto").val('@Html.Raw(nombrePuesto.ToUpper())');
            $("#movil").val('@Html.Raw(movil)');
            $("#direccion").val('@Html.Raw(direccion)');
            $("#fijo").val('@telefonoFijo');
            $("#email").val('@ticket.email');

            if (@idNegocio == @ELL.Solicitud.NEGOCIO_SERVICIOS_GENERALES) {
                $("#lblNombre").text('@Html.Raw(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(ticket.NombreCompleto.ToLower()))');
                $("#lblPuesto").text('@Html.Raw(nombrePuesto.ToUpper())');
                $("#lblEmail").html('@ticket.email' + separadorEmail + '(+34) @movil');
                $("#lblDireccion").html('@Html.Raw(direccion)' + separadorDireccion + 'T: (+34) @telefonoFijo');
            } else {
                $("#lblNombre").text('@Html.Raw(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(ticket.NombreCompleto.ToLower()))');
                $("#lblPuesto").text('@Html.Raw(nombrePuesto.ToUpper())');
                $("#lblEmail").html('@ticket.email');
                $("#lblMovil").html('M: (+34) @movil');
                $("#lblFijo").html('T: (+34) @telefonoFijo');
                $("#lblDireccion").html('@Html.Raw(direccion)');
            }
        }

        $("#Idiomas").change(function () {
            $.ajax({
                url: '@Url.Action("CargarPuestoCultura", "DatosAlternativos")',
data: { idSab: @ticket.IdUser, cultura: $(this).val() },
                type: 'GET',
dataType: 'json',
success: function (d) {
                    $('#puesto').val('');
                    if (d) {
                        $('#puesto').val(d.Nombre.toUpperCase());
                        $("#lblPuesto").text(d.Nombre.toUpperCase());
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                }
            });
        })

        $("input[name=nombreDosApellidos]:radio").change(function () {
            if ($(this).val() == 1) {
                $("#nombre").val('@Html.Raw(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(ticket.NombreCompleto.ToLower()))');
                $("#lblNombre").text('@Html.Raw(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(ticket.NombreCompleto.ToLower()))');
            } else {
                $("#nombre").val('@Html.Raw(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(ticket.NombrePersona.ToLower()))' + ' ' + '@Html.Raw(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(ticket.Apellido1.ToLower()))');
                $("#lblNombre").text('@Html.Raw(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(ticket.NombrePersona.ToLower()))' + ' ' + '@Html.Raw(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(ticket.Apellido1.ToLower()))');
            }
        })

        $("input[name=mostrarMovil]:radio").change(function () {
            if ($(this).val() == 1) {
                $("#movil").val('@movil');
                if (@idNegocio == @ELL.Solicitud.NEGOCIO_SERVICIOS_GENERALES) {
                    $("#lblEmail").html('@ticket.email' + separadorEmail + '(+34) @movil');
                } else {
                    $("#lblMovil").html('M: (+34) @movil');
                }
            } else {
                $("#movil").val('');
                if (@idNegocio == @ELL.Solicitud.NEGOCIO_SERVICIOS_GENERALES) {
                    $("#lblEmail").html('@ticket.email');
                }
                else {
                    $("#lblMovil").html('');
                }
            }
        })

        $(".boton-enviar").click(function () {
            return confirm('@Html.Raw(Utils.Traducir("¿Desea enviar la solicitud a compras?"))');
        })
    });

</script>

@Using Html.BeginForm("Agregar", "Solicitud", FormMethod.Post, New With {.class = "form-horizontal"})
    @<div class="form-group">
         <div class="col-sm-6">
             <div id="divIdioma" Class="form-group">
                 <label Class="col-sm-4 control-label">@Utils.Traducir("Idioma")</label>
                 <div Class="col-sm-5">
                     @Html.DropDownList("Idiomas", Nothing, New With {.Class = "form-control required"})
                 </div>
             </div>
             <div Class="form-group">
                 <label class="col-sm-4 control-label">@Utils.Traducir("Nombre")</label>
                 <div class="col-sm-5">
                     @Html.TextBox("nombre", ticket.NombreCompleto, New With {.maxlength = "100", .required = "required", .class = "form-control", .readonly = "readonly"})
                 </div>
             </div>
             <div id="divNombreDosApellidos" Class="form-group">
                 <label class="col-sm-4 control-label">@Utils.Traducir("¿Nombre y dos apellidos?")</label>
                 <div class="col-sm-5">
                     @Html.RadioButton("nombreDosApellidos", 1, True, New With {.class = "form-check-input"})
                     &nbsp;<label class="form-check-label">@Utils.Traducir("Si")</label>
                     @Html.RadioButton("nombreDosApellidos", 0, False, New With {.class = "form-check-input"})
                     &nbsp;<label class="form-check-label">@Utils.Traducir("No")</label>
                 </div>
             </div>
             <div Class="form-group">
                 <label class="col-sm-4 control-label">@Utils.Traducir("Puesto de trabajo")</label>
                 <div class="col-sm-5">
                     @Html.TextBox("puesto", nombrePuesto, New With {.maxlength = "100", .required = "required", .class = "form-control", .readonly = "readonly"})
                 </div>
             </div>
             <div Class="form-group">
                 <label class="col-sm-4 control-label">@Utils.Traducir("Móvil")</label>
                 <div class="col-sm-5">
                     @Html.TextBox("movil", movil, New With {.maxlength = "20", .required = "required", .class = "form-control", .readonly = "readonly"})
                 </div>
             </div>
             <div id="divMostrarMovil" Class="form-group">
                 <label class="col-sm-4 control-label">@Utils.Traducir("¿Mostrar móvil?")</label>
                 <div class="col-sm-5">
                     @Html.RadioButton("mostrarMovil", 1, True, New With {.class = "form-check-input"})
                     &nbsp;<label class="form-check-label">@Utils.Traducir("Si")</label>
                     @Html.RadioButton("mostrarMovil", 0, False, New With {.class = "form-check-input"})
                     &nbsp;<label class="form-check-label">@Utils.Traducir("No")</label>
                 </div>
             </div>
             <div Class="form-group">
                 <label class="col-sm-4 control-label">@Utils.Traducir("Dirección")</label>
                 <div class="col-sm-5">
                     @Html.TextBox("direccion", direccion, New With {.maxlength = "250", .required = "required", .class = "form-control", .readonly = "readonly"})
                 </div>
             </div>
             <div Class="form-group">
                 <label class="col-sm-4 control-label">@Utils.Traducir("Teléfono")</label>
                 <div class="col-sm-5">
                     @Html.TextBox("fijo", If(String.IsNullOrEmpty(telefonoFijo), telCentralita, telefonoFijo), New With {.maxlength = "20", .required = "required", .class = "form-control", .readonly = "readonly"})
                 </div>
             </div>
             <div Class="form-group">
                 <label class="col-sm-4 control-label">@Utils.Traducir("Email")</label>
                 <div class="col-sm-5">
                     @Html.TextBox("email", ticket.email, New With {.maxlength = "100", .required = "required", .class = "form-control", .readonly = "readonly"})
                 </div>
             </div>
             <div Class="form-group">
                 <label class="col-sm-4 control-label">@Utils.Traducir("Nº cajas (200 uds/caja)")</label>
                 <div class="col-sm-5">
                     <select id="cajas" name="cajas" class="form-control required">
                         <option value="1" selected="selected">1</option>
                         <option value="2">2</option>
                     </select>
                 </div>
             </div>
             <div Class="form-group">
                 <div class="col-sm-offset-4 col-sm-5">
                     <input type="submit" id="submit" value="@Utils.Traducir("Enviar solicitud a compras")" class="btn btn-primary input-block-level form-control boton-enviar" />
                 </div>
             </div>
         </div>
         <div class="col-sm-6">
             @Html.Partial("~/Views/Shared/" & nombreVistaTarjeta, ViewData("ImagenTarjeta"))
         </div>
    </div>
End Using
