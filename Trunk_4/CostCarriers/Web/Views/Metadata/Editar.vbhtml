@Imports CostCarriersLib

@code
    Dim ticket As SabLib.ELL.Ticket = CType(Session("Ticket"), SabLib.ELL.Ticket)
    Dim metadata As ELL.BRAIN.CCMetadata = CType(ViewData("Metadata"), ELL.BRAIN.CCMetadata)
    Dim costCarrier As ELL.BRAIN.CostCarrier = CType(ViewData("CostCarrier"), ELL.BRAIN.CostCarrier)
End Code

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

            if ($("#lbEmpresasProductivas :selected").length == 0 && $("#hfCodigo").val() != '' && $("#TiposPlanta").val() != 'C' && ($("#hfCodigo").val().lastIndexOf("P", 0) == 0 || $("#hfCodigo").val().lastIndexOf("C", 0) == 0)) {
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
            //OcultarCampos()
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

        OcultarCampos();
        QuitarRequerimientoCampos();

        GestionarCampos('@metadata.CodigoPortador');

        if ('@metadata.CodigoPortador' != '') {
            $("#txtCodigo").removeClass("auto-no-seleccionado");
            $("#txtCodigo").addClass("auto-seleccionado");
        }

        if ('@metadata.IdResponsableSAB' != '') {
            $("#txtResponsable").removeClass("auto-no-seleccionado");
            $("#txtResponsable").addClass("auto-seleccionado");
        }

        if ('@metadata.BudgetCode' != '') {
            $("#txtCodigoPresupuesto").removeClass("auto-no-seleccionado");
            $("#txtCodigoPresupuesto").addClass("auto-seleccionado");
        }

        initBusquedaUsuarios("txtResponsable", "hfResponsable", "helperResponsable", "@Url.Action("BuscarUsuarios", "Metadata")");
        initBusquedaCodigosPresupuesto("txtCodigoPresupuesto", "hfCodigoPresupuesto", "helperCodigoPresupuesto", "@Url.Action("BuscarCodigoPresupuesto", "Metadata")");

        //if ($("#Productos").val() != '') {
        //    CargarProyectos($("#Productos").val(), $("#Proyectos").val(), $("#EstadosProyecto").val());
        //}

        /**********************************/
        $('#infoDatos').popover({
            title: '@Html.Raw(Utils.Traducir("Datos"))',
            trigger: 'focus',
            content: '@costCarrier.Datos.Replace(vbCrLf, "\r\n")',
            placement: 'bottom',
            container: 'body'
        });

        $("#fechaAp, #fechaCi, #fechaInic").datetimepicker({
            showClear: true,
            locale: '@Threading.Thread.CurrentThread.CurrentCulture.Name',
            format: '@Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper'
        });

        $("#Productos").change(function () {
            CargarProyectos($("#Productos").val());
        });

        //$("#EstadosProyecto").change(function () {
        //    CargarProyectos($("#Productos").val());
        //});

        $("#Propiedad").change(function () {
            GestionarPropiedad();
        })

        $("#Proyectos").change(function () {
            $('#hfProyectos').val($('#Proyectos option:selected').text());
        })

        $("#Activos").change(function () {
            $('#hfActivo').val($('#Activos option:selected').text());
        })

        $(document).on("codigoBorrado", function (event) {
            $('#lblLantegiObs').hide();
            $('#Lantegi').val('');
            $('#fechaAp').val('');
        });
    });

</script>

<h3><label>@Utils.Traducir("Editar metadatos del portador de coste")</label></h3>
<hr />

@Using Html.BeginForm("Editar", "Metadata", FormMethod.Post, New With {.class = "form-horizontal"})
    @<div Class="form-group row">
        <div class="col-sm-2">
            <input type="submit" id="submit" value="@Utils.Traducir("Guardar")" Class="btn btn-primary" />&nbsp;
            <a href='@Url.Action("Index", "MetadataYear", New With {.CostCarriersMetadata = String.Format("{0}-{1}-{2}", metadata.Empresa, metadata.Planta, metadata.CodigoPortador)})'>
                @Utils.Traducir("Ir a datos anuales")
            </a>
        </div>
    </div>
    @<div Class="form-group row">
        <label class="col-sm-1 col-form-label">@Utils.Traducir("Empresa")</label>
        <div class="col-sm-2">
            @Html.DropDownList("Empresas", Nothing, New With {.required = "required", .class = "form-control"})
        </div>
        <label class="col-sm-1 col-form-label">@Utils.Traducir("Código")</label>
        <div class="col-sm-3">
            @Html.TextBox("txtCodigo", metadata.DescripcionCompleta, New With {.class = "form-control auto-no-seleccionado", .style = "width:100%;", .readonly = "readonly"})
            @Html.Hidden("hfCodigo", metadata.CodigoPortador)
            <div id="helperCodigo" style="margin-top: -1px;">
            </div>
        </div>
        <div class="col-sm-1">
            @Html.TextBox("Lantegi", metadata.Lantegi, New With {.class = "form-control", .readonly = "readonly"})
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
            @Html.TextArea("Denominacion", metadata.DenomAmpliada, New With {.class = "form-control", .maxLength = 200, .rows = 4})
        </div>
        <label class="col-sm-1 col-form-label">@Utils.Traducir("Responsable")</label>
        <div class="col-sm-3">
            @Html.TextBox("txtResponsable", metadata.Responsable, New With {.class = "form-control auto-no-seleccionado", .style = "width:100%;"})
            @Html.Hidden("hfResponsable", metadata.IdResponsableSAB)
            <div id="helperResponsable" style="margin-top: -1px;">
            </div>
        </div>
    </div>
    @<div Class="form-group row">
        <label class="col-sm-1 col-form-label">@Utils.Traducir("Fecha apertura código")</label>
        <div class="col-sm-2">
            <div class="input-group date" id="fechaAp">
                @Html.TextBox("fechaApertura", metadata.FechaIni.ToShortDateString(), New With {.required = "required", .class = "form-control", .readonly = "readonly"})
                <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
            </div>
        </div>
        <label id="lblFechaInicio" class="col-sm-1 col-form-label ocultar">@Utils.Traducir("SOP")</label>
        <div id="divFechaInicio" class="col-sm-2 ocultar">
            <div class="input-group date" id="fechaInic">
                @Html.TextBox("fechaInicio", If(metadata.FechaEstimIni = DateTime.MinValue, String.Empty, metadata.FechaEstimIni.ToShortDateString()), New With {.class = "form-control requerir"})
                <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
            </div>
        </div>
        <label class="col-sm-1 col-form-label">@Utils.Traducir("Fecha cierre")</label>
        <div class="col-sm-2">
            <div class="input-group date" id="fechaCi">
                @Html.TextBox("fechaCierre", If(metadata.FechaFin = DateTime.MinValue, String.Empty, metadata.FechaFin.ToShortDateString()), New With {.class = "form-control"})
                <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
            </div>
        </div>
    </div>
    @<div Class="form-group row">
        <label id="lblAnyosSerie" class="col-sm-1 col-form-label ocultar">@Utils.Traducir("Nº años serie")</label>
        <div id="divAnyosSerie" class="col-sm-2 ocultar">
            @Html.TextBox("anyosSerie", If(metadata.NumAnyosSerie = Integer.MinValue, String.Empty, metadata.NumAnyosSerie), New With {.type = "number", .class = "form-control text-right requerir"})
        </div>
        <label id="lblProductos" class="col-sm-1 col-form-label ocultar">@Utils.Traducir("Producto")</label>
        <div class="col-sm-2">
            @Html.DropDownList("Productos", Nothing, New With {.class = "form-control ocultar requerir"})
        </div>
        <label id="lblProyectos" class="col-sm-1 col-form-label ocultar">@Utils.Traducir("Estado - Proyecto")</label>
        <div class="col-sm-2">
            @Html.Hidden("hfProyectos", metadata.Proyecto)
            @Html.DropDownList("EstadosProyecto", Nothing, New With {.class = "form-control ocultar"})
            @Html.DropDownList("Proyectos", Nothing, New With {.class = "form-control ocultar"})
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
            @Html.Hidden("hfActivo", ViewData("hfActivo"))
        </div>
        <label id="lblCodigoPresupuesto" class="col-sm-1 col-form-label ocultar">@Utils.Traducir("Código presupuesto")</label>
        <div id="divCodigoPresupuesto" class="col-sm-3 ocultar">
            @Html.TextBox("txtCodigoPresupuesto", metadata.DescBudgetCode, New With {.class = "form-control auto-no-seleccionado", .style = "width:100%;"})
            @Html.Hidden("hfCodigoPresupuesto", metadata.BudgetCode)
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