<h3>@String.Format("{0} - {1}", Utils.Traducir("Datos alternativos de trabajadores"), Utils.Traducir("Nuevo"))</h3>
<hr />

<script src="~/Scripts/usuarios.js"></script>

<script type="text/javascript">
    function titleCase(string) {
        var sentenceArray = string.toLowerCase().split(" ");
        var sentence = '';
        for (var i = 0; i < sentenceArray.length; i++) {
            sentence += sentenceArray[i][0].toUpperCase() + sentenceArray[i].slice(1) + ' ';
        }
        return sentence;
    }

    $(function () {
        initBusquedaUsuarios("txtUsuario", "hfUsuario", "helperUsuario", "@Url.Action("BuscarUsuarios", "Login")");

        $(document).on("usuarioSeleccionado", function (event, idSab, idPlanta, email, nombreCompleto) {
            $("#nombre").val(titleCase(nombreCompleto));
            $("#email").val(email);
            $.ajax({
                url: '@Url.Action("CargarTelefonos", "DatosAlternativos")',
                data: { idSab: idSab, idPlanta: idPlanta },
                type: 'GET',
                dataType: 'json',
                success: function (d) {
                    $('#movil').val('');
                    $('#fijo').val('');
                    if (d) {
                        $('#movil').val(d.Movil);
                        $('#fijo').val(d.Fijo);
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                }
            });

            $.ajax({
                url: '@Url.Action("CargarPuesto", "DatosAlternativos")',
                data: { idSab: idSab },
                type: 'GET',
                dataType: 'json',
                success: function (d) {
                    $('#puesto').val('');
                    if (d) {
                        $('#puesto').val(d.Nombre);
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                }
            });
        });
    });

</script>

@Using Html.BeginForm("Agregar", "DatosAlternativos", FormMethod.Post, New With {.class = "form-horizontal"})
    @<div Class="form-group">
        <label class="col-sm-3 control-label">@Utils.Traducir("Descripción")</label>
        <div class="col-sm-4">
            @Html.TextBox("descripcion", String.Empty, New With {.maxlength = "150", .required = "required", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-3 control-label">@Utils.Traducir("Trabajador")</label>
        <div class="col-sm-4">
            @Html.TextBox("txtUsuario", String.Empty, New With {.class = "form-control auto-no-seleccionado", .style = "width:100%;"})
            @Html.Hidden("hfUsuario", String.Empty)
            <div id="helperUsuario" style="margin-top: -1px;">
            </div>
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-3 control-label">@Utils.Traducir("Nombre")</label>
        <div class="col-sm-4">
            @Html.TextBox("nombre", String.Empty, New With {.maxlength = "100", .required = "required", .class = "form-control", .style = "text-transform: capitalize;"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-3 control-label">@Utils.Traducir("Puesto de trabajo")</label>
        <div class="col-sm-4">
            @Html.TextBox("puesto", Nothing, New With {.maxlength = "100", .required = "required", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-3 control-label">@Utils.Traducir("Móvil")</label>
        <div class="col-sm-4">
            @Html.TextBox("movil", Nothing, New With {.maxlength = "20", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-3 control-label">@Utils.Traducir("Dirección")</label>
        <div class="col-sm-4">
            @Html.TextBox("direccion", Nothing, New With {.maxlength = "250", .required = "required", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-3 control-label">@Utils.Traducir("Telefono fijo")</label>
        <div class="col-sm-4">
            @Html.TextBox("fijo", Nothing, New With {.maxlength = "20", .required = "required", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-3 control-label">@Utils.Traducir("Email")</label>
        <div class="col-sm-4">
            @Html.TextBox("email", Nothing, New With {.maxlength = "100", .required = "required", .class = "form-control"})
        </div>
    </div>

    @<div Class="form-group">
        <div class="col-sm-offset-3 col-sm-4">
            <input type="submit" id="submit" value="@Utils.Traducir("Guardar")" class="btn btn-primary input-block-level form-control" />
        </div>
    </div>
End Using