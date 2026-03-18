@Imports CostCarriersLib

@code
    Dim ticket As SabLib.ELL.Ticket = CType(Session("Ticket"), SabLib.ELL.Ticket)
    'Dim metadata As ELL.BRAIN.CCMetadata = CType(ViewData("Metadata"), ELL.BRAIN.CCMetadata)
    Dim costCarrier As ELL.BRAIN.CostCarrier = CType(ViewData("CostCarrier"), ELL.BRAIN.CostCarrier)
    Dim solicitante As SabLib.ELL.Usuario = CType(ViewData("Solicitante"), SabLib.ELL.Usuario)
    Dim idValidacionLinea As Integer = CInt(ViewData("IdValidacionLinea"))
    Dim idStep As Integer = CInt(ViewData("IdStep"))

    Dim sop As DateTime = DateTime.MinValue
    If (ViewData("SOP") IsNot Nothing) Then
        sop = CDate(ViewData("SOP"))
    End If

    Dim anyosSerie As Integer = Integer.MinValue
    If (ViewData("AnyosSerie") IsNot Nothing) Then
        anyosSerie = CInt(ViewData("AnyosSerie"))
    End If

    ' Este campo viene informado cuando se crea metadata desde financiero de cost carriers
    Dim validacionInfoAdicional As ELL.ValidacionInfoAdicional = CType(ViewData("ValidacionInfoAdicional"), ELL.ValidacionInfoAdicional)

    Dim plantaProductivaCorporativo As Boolean = False
    If (ViewData.Keys.ToList.Exists(Function(f) f = "PlantaProductivaCorporativo")) Then
        plantaProductivaCorporativo = CBool(ViewData("PlantaProductivaCorporativo"))
    End If
End Code

<script src="~/Scripts/costcarriers.js"></script>
<script src="~/Scripts/usuarios.js"></script>
<script src="~/Scripts/codigospresupuesto.js"></script>
<script src="~/Scripts/jquery.alphanum.js"></script>

<script type="text/javascript">

    $(function () {
        $("form").submit(function () {
            if ($("#hfCodigo").val() == "") {
                alert("@Html.Raw(Utils.Traducir("Código campo obligatorio"))");
                return false;
            }

            if ($("#hfResponsable").val() == "") {
                alert("@Html.Raw(Utils.Traducir("Responsable campo obligatorio"))");
                return false;
            }

            // Si el paso tiene planta productiva corporativo no es obligatorio
            if ($("#lbEmpresasProductivas :selected").length == 0 && $("#hfCodigo").val() != '' && '@plantaProductivaCorporativo.ToString' == 'False' && ($("#hfCodigo").val().lastIndexOf("P", 0) == 0 || $("#hfCodigo").val().lastIndexOf("C", 0) == 0)) {
                alert("@Html.Raw(Utils.Traducir("Debe seleccionar al menos una empresa productiva"))");
                return false;
            }

            // Silvia dice que no sea obligatorio
            @*if ($("#hfCodigoPresupuesto").val() == '' && ($("#hfCodigo").val().lastIndexOf("D", 0) == 0 || $("#hfCodigo").val().lastIndexOf("Y", 0) == 0)) {
                alert("@Html.Raw(Utils.Traducir("Código presupuesto campo obligatorio"))");
                return false;
            }*@
        });

        $("#anyosSerie").alphanum({
            allowPlus: false,
            allowMinus: true,
            allowThouSep: false,
            allowDecSep: false
        });

        function OcultarCampos() {
            $(".ocultar").each(function () {
                if (!$(this).hasClass("hidden")) {
                    $(this).addClass("hidden");
                }
            })
        }

        function LimpiarCampos(limpiarCodigo) {
            if (limpiarCodigo == true) {
                $("#txtCodigo").val('');
                $("#hfCodigo").val('');
                $("#txtCodigo").removeClass("auto-seleccionado");
                $("#txtCodigo").addClass("auto-no-seleccionado");
            }
            $('#Lantegi').val('');
            $('#lblLantegiObs').hide();
            $("#fechaInicio").val('');
            $("#anyosSerie").val('');
            $('#Productos option:eq(0)').prop('selected', true);
            $('#Proyectos').empty();
            $('#EstadosProyecto option:eq(0)').prop('selected', true);
            $('#txtCodigoPresupuesto').val('');
            $('#hfCodigoPresupuesto').val('');
            $("#txtCodigoPresupuesto").removeClass('auto-seleccionado');
            $("#txtCodigoPresupuesto").addClass('auto-no-seleccionado');            
            $('#lbEmpresasProductivas').val([]);
        }

        function QuitarRequerimientoCampos() {
            $(".requerir").each(function () {
                $(this).prop("required", false);
            })
        }

        function GestionarPropiedad() {
            if ($("#Propiedad").val() == "I") {
                $("#lblActivos").removeClass("hidden");
                $("#divActivos").removeClass("hidden");
                $("#Activos").prop("required", true);
            }
            else {
                $("#lblActivos").addClass("hidden");
                $("#divActivos").addClass("hidden");
                $("#Activos").prop("required", false);
                $('#hfActivo').val('');
            }
        }

        function GestionarCampos(codigo) {
            OcultarCampos();
            QuitarRequerimientoCampos();

            $("#TiposPlanta option").each(function (index) {
                $(this).prop('disabled', true);
            });

            $("#EstadosProyecto option").each(function (index) {
                $(this).prop('disabled', true);
            });

            if (codigo.lastIndexOf("AP", 0) == 0) {
                $('#TiposPlanta option:eq(0)').prop('selected', true);
                $('#TiposPlanta option:eq(0)').prop('disabled', false);
            }
            else if (codigo.lastIndexOf("B", 0) == 0) {
                $('#TiposPlanta option:eq(0)').prop('selected', true);
                $('#TiposPlanta option:eq(0)').prop('disabled', false);
            }
            else if (codigo.lastIndexOf("D", 0) == 0) {
                $('#TiposPlanta option:eq(0)').prop('selected', true);
                $('#TiposPlanta option:eq(0)').prop('disabled', false);

                $("#EstadosProyecto option[value='@ELL.EstadoBonos.R_D']").prop('disabled', false)
                $('#EstadosProyecto').val("@ELL.EstadoBonos.R_D");

                $("#lblProductos").removeClass("hidden");
                $("#Productos").removeClass("hidden");
                $("#lblProyectos").removeClass("hidden");
                $("#Proyectos").removeClass("hidden");
                $("#EstadosProyecto").removeClass("hidden");

                $("#Productos").prop("required", true);
                $("#Proyectos").prop("required", true);
            }
            else if (codigo.lastIndexOf("C", 0) == 0 || codigo.lastIndexOf("I", 0) == 0) {
                $('#TiposPlanta option:eq(0)').prop('selected', true);
                $('#TiposPlanta option:eq(0)').prop('disabled', false);

                $("#EstadosProyecto option[value='@ELL.EstadoBonos.Offer']").prop('disabled', false);
                $("#EstadosProyecto option[value='@ELL.EstadoBonos.RFQ']").prop('disabled', false);
                $("#EstadosProyecto option[value='@ELL.EstadoBonos.RFI']").prop('disabled', false);
                $('#EstadosProyecto').val("@ELL.EstadoBonos.Offer");

                $("#lblProductos").removeClass("hidden");
                $("#Productos").removeClass("hidden");
                $("#lblProyectos").removeClass("hidden");
                $("#Proyectos").removeClass("hidden");
                $("#EstadosProyecto").removeClass("hidden");
                $("#lblEmpresasProductivas").removeClass("hidden");
                $("#divEmpresasProductivas").removeClass("hidden");

                $("#Productos").prop("required", true);
                $("#Proyectos").prop("required", true);
            }
            else if (codigo.lastIndexOf("G", 0) == 0) {
                $('#TiposPlanta option:eq(0)').prop('selected', true);
                $('#TiposPlanta option:eq(0)').prop('disabled', false);

                $("#EstadosProyecto option[value='@ELL.EstadoBonos.Offer_RFI']").prop('disabled', false);
                $("#EstadosProyecto option[value='@ELL.EstadoBonos.RFQ']").prop('disabled', false);
                $("#EstadosProyecto option[value='@ELL.EstadoBonos.RFI']").prop('disabled', false);
                $('#EstadosProyecto').val("@ELL.EstadoBonos.Offer_RFI");

                $("#lblProductos").removeClass("hidden");
                $("#Productos").removeClass("hidden");
                $("#lblProyectos").removeClass("hidden");
                $("#Proyectos").removeClass("hidden");
                $("#EstadosProyecto").removeClass("hidden");

                $("#Productos").prop("required", true);
                $("#Proyectos").prop("required", true);
            }
            else if (codigo.lastIndexOf("P", 0) == 0) {
                $("#TiposPlanta option").each(function (index) {
                    $(this).prop('disabled', false);
                });

                $("#EstadosProyecto option[value='@ELL.EstadoBonos.Development']").prop('disabled', false);
                $("#EstadosProyecto option[value='@ELL.EstadoBonos.Industrialization']").prop('disabled', false);
                $('#EstadosProyecto').val("@ELL.EstadoBonos.Development");

                $("#lblFechaInicio").removeClass("hidden");
                $("#divFechaInicio").removeClass("hidden");
                $("#lblAnyosSerie").removeClass("hidden");
                $("#divAnyosSerie").removeClass("hidden");
                $("#lblProductos").removeClass("hidden");
                $("#Productos").removeClass("hidden");
                $("#lblProyectos").removeClass("hidden");
                $("#Proyectos").removeClass("hidden");
                $("#EstadosProyecto").removeClass("hidden");
                $("#lblActivos").removeClass("hidden");
                $("#divActivos").removeClass("hidden");
                $("#lblPropiedad").removeClass("hidden");
                $("#divPropiedad").removeClass("hidden");
                $("#lblEmpresasProductivas").removeClass("hidden");
                $("#divEmpresasProductivas").removeClass("hidden");

                // Hablando con Maite el 04/09/2019 decidimos que este campo no sea obligatorio
                //$("#fechaInicio").prop("required", true);
                // Me pide Maite el 13/05/2019 que quite la obligatoriedad de este campo
                //$("#anyosSerie").prop("required", true);
                $("#Productos").prop("required", true);
                $("#Proyectos").prop("required", true);
                $("#Propiedad").prop("required", true);
                $("#Activos").prop("required", true);

                GestionarPropiedad();
            }
            else if (codigo.lastIndexOf("N", 0) == 0) {
                $('#TiposPlanta option:eq(0)').prop('selected', true);
                $('#TiposPlanta option:eq(0)').prop('disabled', false);

                $("#EstadosProyecto option[value='@ELL.EstadoBonos.Development']").prop('disabled', false);
                $("#EstadosProyecto option[value='@ELL.EstadoBonos.Industrialization']").prop('disabled', false);
                $('#EstadosProyecto').val("@ELL.EstadoBonos.Development");

                $("#lblProductos").removeClass("hidden");
                $("#Productos").removeClass("hidden");
                $("#lblProyectos").removeClass("hidden");
                $("#Proyectos").removeClass("hidden");
                $("#EstadosProyecto").removeClass("hidden");

                $("#Productos").prop("required", true);
                $("#Proyectos").prop("required", true);
            }
            else if (codigo.lastIndexOf("RP", 0) == 0) {
                $('#TiposPlanta option:eq(1)').prop('selected', true);
                $('#TiposPlanta option:eq(1)').prop('disabled', false);

                $("#lblProductos").removeClass("hidden");
                $("#Productos").removeClass("hidden");
                $("#lblProyectos").removeClass("hidden");
                $("#Proyectos").removeClass("hidden");
                $("#EstadosProyecto").removeClass("hidden");

                $("#Productos").prop("required", true);
                $("#Proyectos").prop("required", true);
            }
            else if (codigo.lastIndexOf("T", 0) == 0 || codigo.lastIndexOf("U", 0) == 0 || codigo.lastIndexOf("UJ", 0) == 0 || codigo.lastIndexOf("UM", 0) == 0 || codigo.lastIndexOf("UT", 0) == 0 || codigo.lastIndexOf("W", 0) == 0 || codigo.lastIndexOf("UW", 0) == 0) {
                $('#TiposPlanta option:eq(1)').prop('selected', true);
                $('#TiposPlanta option:eq(1)').prop('disabled', false);

                $("#lblProductos").removeClass("hidden");
                $("#Productos").removeClass("hidden");
                $("#lblProyectos").removeClass("hidden");
                $("#Proyectos").removeClass("hidden");
                $("#EstadosProyecto").removeClass("hidden");

                $("#Productos").prop("required", true);
                $("#Proyectos").prop("required", true);
            }
            else if (codigo.lastIndexOf("Y", 0) == 0) {
                $("#TiposPlanta option").each(function (index) {
                    $(this).prop('disabled', false);
                });

                $("#lblFechaInicio").removeClass("hidden");
                $("#divFechaInicio").removeClass("hidden");
                $("#lblActivos").removeClass("hidden");
                $("#divActivos").removeClass("hidden");
                $("#lblPropiedad").removeClass("hidden");
                $("#divPropiedad").removeClass("hidden");
                $("#lblCodigoPresupuesto").removeClass("hidden");
                $("#divCodigoPresupuesto").removeClass("hidden");                

                $("#fechaInicio").prop("required", true);
                $("#Propiedad").prop("required", true);
                $("#Activos").prop("required", true);

                GestionarPropiedad();
            }
        }

        function CargarProyectos(producto, proyecto) {
            if (producto == '') {
                $('#Proyectos').empty();
                $('#Proyectos').prop('disabled', true);
                $('#hfProyectos').val('');
            }
            else {
                $('#Proyectos').prop('disabled', false);
                $.ajax({
                    url: '@Url.Action("CargarProyectos", "Metadata")',
                    data: { producto: producto },
                    type: 'GET',
                    dataType: 'json',
                    success: function (d) {
                        if (d.length > 0) {
                            $('#Proyectos').empty();
                            $.each(d, function (i, proyecto) {
                                $('#Proyectos').append($('<option>', {
                                    value: proyecto.Id,
                                    text: proyecto.Nombre
                                }));
                            });

                            if (proyecto) {
                                $('#Proyectos').val(proyecto);
                            }

                            $('#hfProyectos').val($('#Proyectos option:selected').text());
                        }
                        else {
                            $('#Proyectos').empty();
                            $('#hfProyectos').val('');
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });
            }
        }

        /********* INICIALIZACIÓN *********/

        $('#infoDatos').popover({
            title: '@Html.Raw(Utils.Traducir("Datos"))',
            content: '@Html.Raw(Utils.Traducir("Sin datos"))',
            trigger: 'focus',
            placement: 'bottom',
            container: 'body'
        });

        OcultarCampos();
        QuitarRequerimientoCampos();
        if ($("#Empresas").val() != '') {
            $("#txtCodigo").prop('disabled', $("#Empresas").val() == '@Integer.MinValue');

            initBusquedaCostCarriers("txtCodigo", "hfCodigo", "helperCodigo", $("#Empresas").val(), "@Url.Action("BuscarCostCarriers", "Metadata")");
        }

        if ($("#hfCodigo").val() != '') {
            $("#txtCodigo").removeClass("auto-no-seleccionado");
            $("#txtCodigo").addClass("auto-seleccionado");

            GestionarCampos($("#hfCodigo").val())
        }

        if ($("#hfResponsable").val() != '') {
            $("#txtResponsable").removeClass("auto-no-seleccionado");
            $("#txtResponsable").addClass("auto-seleccionado");
        }

        if ($("#hfCodigoPresupuesto").val() != '') {
            $("#txtCodigoPresupuesto").removeClass("auto-no-seleccionado");
            $("#txtCodigoPresupuesto").addClass("auto-seleccionado");
        }

        initBusquedaUsuarios("txtResponsable", "hfResponsable", "helperResponsable", "@Url.Action("BuscarUsuarios", "Metadata")");
        initBusquedaCodigosPresupuesto("txtCodigoPresupuesto", "hfCodigoPresupuesto", "helperCodigoPresupuesto", "@Url.Action("BuscarCodigoPresupuesto", "Metadata")");

        if ($("#Productos").val() != '') {
            CargarProyectos($("#Productos").val(), $("#Proyectos").val());
        }

        /**********************************/

        $("#Activos").change(function () {
            $('#hfActivo').val($('#Activos option:selected').text());
        })

        $("#fechaAp, #fechaCi, #fechaInic").datetimepicker({
            showClear: true,
            locale: '@Threading.Thread.CurrentThread.CurrentCulture.Name',
            format: '@Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper'
        });

        $("#Empresas").change(function () {
            var valor = $(this).val()

            LimpiarCampos(true);

            $("#txtCodigo").prop('disabled', valor == '@Integer.MinValue');

            //Si la empresa seleccionada es Zamudio habría que ir a buscar a Navision
            initBusquedaCostCarriers("txtCodigo", "hfCodigo", "helperCodigo", valor, "@Url.Action("BuscarCostCarriers", "Metadata")");

            if (valor == '@ELL.Planta.PLANTA_XPERT_ZAMUDIO') {
                $("#fechaApertura").prop("readonly", false);
            } else {
                $("#fechaApertura").prop("readonly", true);
            }
        });

        if ($("#Empresas").val() == '@ELL.Planta.PLANTA_XPERT_ZAMUDIO') {
            $("#fechaApertura").prop("readonly", false);
        }

        $("#Productos").change(function () {
            CargarProyectos($("#Productos").val());
        });

        //$("#EstadosProyecto").change(function () {
        //    CargarProyectos($("#Productos").val());
        //});

        $("#Propiedad").change(function () {
            //if ($("#Propiedad").val() == "I") {
            //    $("#lblActivos").removeClass("hidden");
            //    $("#divActivos").removeClass("hidden");
            //    $("#Activos").prop("required", true);
            //    $('#hfActivo').val('');
            //}
            //else {
            //    $("#lblActivos").addClass("hidden");
            //    $("#divActivos").addClass("hidden");
            //    $("#Activos").prop("required", false);
            //}
            GestionarPropiedad();
        })

        $("#Proyectos").change(function () {
            $('#hfProyectos').val($('#Proyectos option:selected').text());
        })

        $(document).on("codigoSeleccionado", function (event, codigo, lantegi, fechaApertura, datos, descripcionCompleta) {        
            $('#txtCodigo').val(descripcionCompleta);
            $('#hfCodigo').val(codigo);
            if ($('#txtCodigo') && $('#txtCodigo').hasClass('auto-no-seleccionado')) {
                $('#txtCodigo').removeClass("auto-no-seleccionado");
                $('#txtCodigo').addClass("auto-seleccionado");
            }

            //LimpiarCampos(false)
            GestionarCampos(codigo);

            // Mostramos un mensaje si el portador está obsoleto
            if (lantegi.toUpperCase() == 'OBS' || lantegi == '') {
                $('#lblLantegiObs').show();
            }
            else {
                $('#lblLantegiObs').hide();
            }
            $('#Lantegi').val(lantegi);

            var anyo = parseInt(fechaApertura.substring(0, 4));
            var mes = parseInt(fechaApertura.substring(4, 6)) -1; //El indice del mes empieza en 0 en javascript
            var dia = parseInt(fechaApertura.substring(6, 8));
            $('#fechaAp').data("DateTimePicker").date(new Date(anyo, mes, dia));

            if (datos == '')
            {
                $('#infoDatos').data('bs.popover').options.content = '@Html.Raw(Utils.Traducir("Sin datos"))';
            }
            else {
                $('#infoDatos').data('bs.popover').options.content = datos;
            }
        });

        $(document).on("responsableSeleccionado", function (event, id, nombreCompleto) {
            $('#txtResponsable').val(nombreCompleto);
            $('#hfResponsable').val(id);
            if ($('#txtResponsable') && $('#txtResponsable').hasClass('auto-no-seleccionado')) {
                $('#txtResponsable').removeClass("auto-no-seleccionado");
                $('#txtResponsable').addClass("auto-seleccionado");
            }
        })

        // Estas líneas están para cuando venimos de financiero
        @code
            If (costCarrier IsNot Nothing) Then
                @<text>       
                    $(document).trigger("codigoSeleccionado", ["@costCarrier.Valor", "@costCarrier.Lantegi", "@costCarrier.FechaAperturaCadena", "@costCarrier.Datos", "@Html.Raw(costCarrier.DescripcionCompleta)"]);
                </text>
            End If

            If (solicitante IsNot Nothing) Then
                @<text>
                    $(document).trigger("responsableSeleccionado", ["@solicitante.Id", "@Html.Raw(solicitante.NombreCompletoYPlanta)"]);
                </text>
            End If
                        End code

        $(document).on("codigoBorrado", function (event) {            
            $('#lblLantegiObs').hide();
            $('#Lantegi').val('');
            $('#fechaAp').val('');

            $('#infoDatos').data('bs.popover').options.content = '@Html.Raw(Utils.Traducir("Sin datos"))';
        });
    });

</script>

<h3><label>@Utils.Traducir("Crear metadatos del portador de coste")</label></h3>
<hr />

@Using Html.BeginForm("Crear", "Metadata", FormMethod.Post, New With {.class = "form-horizontal"})
    @Html.Hidden("hfIdValidacionLinea", idValidacionLinea)
    @Html.Hidden("hfIdStep", idStep)
    @<div Class="form-group row">
         <div class="col-sm-1">
             @code
                 If (costCarrier IsNot Nothing) Then
                     @<input type = "submit" id="submit" value="@Utils.Traducir("Guardar y marcar paso como abierto")" Class="btn btn-primary" />             
                 Else
                     @<input type = "submit" id="submit" value="@Utils.Traducir("Guardar")" Class="btn btn-primary" />             
                 End If
                        End Code
         </div>
    </div>
    @<div Class="form-group row">
        <label class="col-sm-1 col-form-label">@Utils.Traducir("Empresa")</label>
        <div class="col-sm-2">
            @Html.DropDownList("Empresas", Nothing, New With {.required = "required", .class = "form-control"})
        </div>
        <label class="col-sm-1 col-form-label">@Utils.Traducir("Código")</label>
        <div class="col-sm-3">
            @Html.TextBox("txtCodigo", Nothing, New With {.class = "form-control auto-no-seleccionado", .style = "width:100%;"})             
            @Html.Hidden("hfCodigo")            
            <div id="helperCodigo" style="margin-top: -1px;">
            </div>
        </div>
         <div class="col-sm-1">
             @Html.TextBox("Lantegi", Nothing, New With {.class = "form-control", .readonly = "readonly"})             
         </div>
         <span id="lblLantegiObs" class="label label-danger" style="display:none;">@Utils.Traducir("Portador de coste inactivo: pendiente de validación de inversión")</span>
    </div>
    @<div Class="form-group row">
        <label class="col-sm-1 col-form-label">@Utils.Traducir("Tipo planta")</label>
         <div class="col-sm-2">
             @Html.DropDownList("TiposPlanta", Nothing, New With {.required = "required", .class = "form-control"})
         </div>
         <label class="col-sm-1 col-form-label">
             @Utils.Traducir("Descripción ampliada")    
             <a id="infoDatos" href="#"><span class="glyphicon glyphicon glyphicon-info-sign" aria-hidden="true"></span></a>
         </label>
         <div class="col-sm-2">
            @Html.TextArea("Denominacion", Nothing, New With {.class = "form-control", .maxLength = 200, .rows = 4, .width = "100%"})
         </div>
         <label class="col-sm-1 col-form-label">@Utils.Traducir("Responsable")</label>
         <div class="col-sm-3">             
             @Html.TextBox("txtResponsable", Nothing, New With {.class = "form-control auto-no-seleccionado", .style = "width:100%;"})
             @Html.Hidden("hfResponsable")
             <div id="helperResponsable" style="margin-top: -1px;">
             </div>
         </div>
    </div>
    @<div Class="form-group row">
        <label class="col-sm-1 col-form-label">@Utils.Traducir("Fecha apertura código")</label>
        <div class="col-sm-2">
            <div class="input-group date" id="fechaAp">
                @Html.TextBox("fechaApertura", String.Empty, New With {.required = "required", .class = "form-control", .readonly = "readonly"})
                <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
            </div>
        </div>         
         <label id="lblFechaInicio" class="col-sm-1 col-form-label ocultar">@Utils.Traducir("SOP")</label>
         <div id="divFechaInicio" class="col-sm-2 ocultar">
             <div class="input-group date" id="fechaInic">
                 @Html.TextBox("fechaInicio", If(validacionInfoAdicional Is Nothing OrElse validacionInfoAdicional.SOP = DateTime.MinValue, If(sop = DateTime.MinValue, String.Empty, sop.ToShortDateString()), validacionInfoAdicional.SOP.ToShortDateString()), New With {.class = "form-control requerir"})
                 <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
             </div>
         </div>
         <label class="col-sm-1 col-form-label">@Utils.Traducir("Fecha cierre")</label>
         <div class="col-sm-2">
             <div class="input-group date" id="fechaCi">
                 @Html.TextBox("fechaCierre", String.Empty, New With {.class = "form-control"})
                 <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
             </div>
         </div>
    </div>
    @<div Class="form-group row">
        <label id="lblAnyosSerie" class="col-sm-1 col-form-label ocultar">@Utils.Traducir("Nº años serie")</label>
        <div id="divAnyosSerie" class="col-sm-2 ocultar">            
            @Html.TextBox("anyosSerie", If(validacionInfoAdicional Is Nothing OrElse validacionInfoAdicional.SeriesYears = Integer.MinValue, If(anyosSerie = Integer.MinValue, String.Empty, anyosSerie), validacionInfoAdicional.SeriesYears), New With {.type = "number", .class = "form-control text-right requerir"})            
        </div>
        <label id="lblProductos" class="col-sm-1 col-form-label ocultar">@Utils.Traducir("Producto")</label>
        <div class="col-sm-2">
            @Html.DropDownList("Productos", Nothing, New With {.class = "form-control ocultar requerir"})
        </div>
        <label id="lblProyectos" class="col-sm-1 col-form-label ocultar">@Utils.Traducir("Estado - Proyecto")</label>
        <div class="col-sm-2">
            @Html.Hidden("hfProyectos")
            @Html.DropDownList("EstadosProyecto", Nothing, New With {.class = "form-control ocultar"})
            @Html.DropDownList("Proyectos", Nothing, New With {.disabled = "disabled", .class = "form-control ocultar"})
        </div>        
    </div>
    @<div Class="form-group row">
         <label id="lblPropiedad" class="col-sm-1 col-form-label ocultar">@Utils.Traducir("¿Propiedad de BATZ?")</label>
         <div id="divPropiedad" class="col-sm-2 ocultar">
             @Html.DropDownList("Propiedad", Nothing, New With {.class = "form-control"})
         </div>
         <label id="lblActivos" class="col-sm-1 col-form-label ocultar">@Utils.Traducir("Activo")</label>
         <div id="divActivos" class="col-sm-2 ocultar">
             @Html.DropDownList("Activos", Nothing, New With {.class = "form-control"})
             @Html.Hidden("hfActivo")
         </div>
         <label id="lblCodigoPresupuesto" class="col-sm-1 col-form-label ocultar">@Utils.Traducir("Código presupuesto")</label>
         <div id="divCodigoPresupuesto" class="col-sm-3 ocultar">
             @Html.TextBox("txtCodigoPresupuesto", Nothing, New With {.class = "form-control auto-no-seleccionado", .style = "width:100%;"})
             @Html.Hidden("hfCodigoPresupuesto")
             <div id="helperCodigoPresupuesto" style="margin-top: -1px;">
             </div>
         </div>
    </div>
    @<div Class="form-group row">         
         <label id="lblEmpresasProductivas" class="col-sm-1 col-form-label ocultar">@Utils.Traducir("Plantas productivas")</label>
         <div id="divEmpresasProductivas" class="col-sm-2 ocultar">
             @Html.ListBox("lbEmpresasProductivas", Nothing, New With {.class = "form-control"})
         </div>
    </div>

                 End Using
