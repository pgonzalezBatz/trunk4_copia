@ModelType BezerreSis.RECLAMACIONES
@Code
    ViewData("Title") = "Index"
    Dim oficialVisible
    If TempData.Peek("ReturnUrl").Equals("Index") Then
        oficialVisible = "none"
    Else
        oficialVisible = ""
    End If
    Dim oracleDatabase As New oracleDB
    Dim db As New Entities_BezerreSis
    Dim myDB As New myDb
    Dim aCookie As HttpCookie = Request.Cookies("BezerreSis_Filtro")
    Dim idPlantaSAB = aCookie.Values("IdPlanta")
    Dim idPlantaBRAIN = oracleDatabase.getIdPlantaBrainFromIdPlantaSab(idPlantaSAB)
    Dim procedenciaList As New List(Of SelectListItem)
    '''' todo: bezerresis.procedencias
    procedenciaList.Add(New SelectListItem With {.Value = 1, .Text = "Interna (Batz)"})
    procedenciaList.Add(New SelectListItem With {.Value = 3, .Text = "A planta Batz"})
    procedenciaList.Add(New SelectListItem With {.Value = 2, .Text = "A proveedor"})
    procedenciaList.Add(New SelectListItem With {.Value = 6, .Text = "Rechazada"})
    Dim usuariosList As New List(Of SelectListItem)
    usuariosList = oracleDatabase.getUsuariosCreadores().Where(Function(f) f.Value.ToString.Length > 2).Select(Function(f) New SelectListItem With {.Text = f.Text, .Value = f.Value.Substring(2)}).ToList
    Dim userId = oracleDatabase.getUserIdFromActiveDirectoryId(User.Identity.Name.ToString.ToUpper)
    Dim lockUsers = Not Configuration.ConfigurationManager.AppSettings("DevAdmins").Split(",").Contains(userId)
    Dim clasificacionList As New List(Of SelectListItem)
    clasificacionList.Add(New SelectListItem With {.Value = "1", .Text = "Planta cliente (Km.0)"})
    clasificacionList.Add(New SelectListItem With {.Value = "2", .Text = "Garantias"})
    clasificacionList.Add(New SelectListItem With {.Value = "3", .Text = "Planta filial"})
    Dim nivelimportanciaList As New List(Of SelectListItem)
    nivelimportanciaList = oracleDatabase.getEstructura(Configuration.ConfigurationManager.AppSettings("nivelImportanciaId"))
    Dim ncRepetitivaList As New List(Of SelectListItem)
    ncRepetitivaList = oracleDatabase.getEstructura(Configuration.ConfigurationManager.AppSettings("repetitivaId"))
    Dim reclamacionOficialList As New List(Of SelectListItem)
    reclamacionOficialList.Add(New SelectListItem With {.Text = "Sí", .Value = "1"})
    reclamacionOficialList.Add(New SelectListItem With {.Text = "No", .Value = "2"})
    Dim afectaIndicadoresList As New List(Of SelectListItem)
    afectaIndicadoresList.Add(New SelectListItem With {.Text = "Sí", .Value = "1"})
    afectaIndicadoresList.Add(New SelectListItem With {.Text = "No", .Value = "0"})
    Dim reclamacion = Model
    Dim editMode = False
    If reclamacion IsNot Nothing Then
        editMode = True
    End If
End Code

<br />
<h2>@(If(editMode, "Actualizar ", "Crear "))Reclamación</h2>
<br />
@Html.Hidden("EditMode", editMode)
@Using (Html.BeginForm())
    @Html.AntiForgeryToken()
    @<div class="form-horizontal">
        @Html.ValidationSummary(True, "", New With {.class = "text-danger"})

        <div class="form-group col-md-12" style="margin-bottom:0px;">
            @Html.LabelFor(Function(model) model.FECHACREACION, htmlAttributes:=New With {.class = "control-label col-sm-6"})
            <div class="col-sm-6">
                <div class="col-md-12">
                    <div class="form-group">
                        <div class='input-group date myDatePicker' id='datetimepicker1'>
                            @Html.TextBox("FECHACREACION", If(reclamacion Is Nothing, Date.Today.ToString("yyyy/MM/dd"), CDate(reclamacion.FECHACREACION).ToString("yyyy/MM/dd")), New With {.class = "form-control"})
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>
                @Html.ValidationMessageFor(Function(model) model.FECHACREACION, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group col-md-12">
            @Html.LabelFor(Function(model) model.CREADOR, htmlAttributes:=New With {.class = "control-label col-sm-6"})
            <div class="col-sm-6">
                @Html.DropDownList("CREADOR", usuariosList.AsEnumerable, "Seleccionar uno", htmlAttributes:=New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(model) model.CREADOR, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group col-md-12">
            @Html.LabelFor(Function(model) model.REFINTERNAPIEZA, htmlAttributes:=New With {.class = "control-label col-sm-6 aExportarLbl"})
            <div class="col-sm-6">
                @Html.EditorFor(Function(model) model.REFINTERNAPIEZA, New With {.htmlAttributes = New With {.class = "form-control aExportarTxt"}})
                @Html.ValidationMessageFor(Function(model) model.REFINTERNAPIEZA, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group col-md-12">
            @Html.LabelFor(Function(model) model.DENOMINACION, htmlAttributes:=New With {.class = "control-label col-sm-6 aExportarLbl"})
            <div class="col-sm-6">
                @Html.EditorFor(Function(model) model.DENOMINACION, New With {.htmlAttributes = New With {.class = "form-control aExportarTxt", .readonly = "readonly"}})
                @Html.ValidationMessageFor(Function(model) model.DENOMINACION, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group col-md-12">
            @Html.LabelFor(Function(model) model.PROYECTO, htmlAttributes:=New With {.class = "control-label col-sm-6 aExportarLbl"})
            <div class="col-sm-6">
                @Html.EditorFor(Function(model) model.PROYECTO, New With {.htmlAttributes = New With {.class = "form-control aExportarTxt", .readonly = "readonly"}})
                @Html.ValidationMessageFor(Function(model) model.PROYECTO, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group col-md-12">
            @Html.LabelFor(Function(model) model.REFCLIENTE, htmlAttributes:=New With {.class = "control-label col-sm-6"})
            <div class="col-sm-6">
                @Html.EditorFor(Function(model) model.REFCLIENTE, New With {.htmlAttributes = New With {.class = "form-control", .readonly = "readonly"}})
                @Html.ValidationMessageFor(Function(model) model.REFCLIENTE, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group col-md-12">
            @Html.LabelFor(Function(model) model.PRODUCTO, htmlAttributes:=New With {.class = "control-label col-sm-6"})
            <div class="col-sm-6">
                @Html.DropDownList("PRODUCTO", db.PRODUCTOS.Select(Function(f) New SelectListItem With {.Text = f.NOMBRE, .Value = f.ID}).OrderBy(Function(f) f.Text), "", htmlAttributes:=New With {.class = "form-control", .readonly = "readonly"})
                @Html.ValidationMessageFor(Function(model) model.PRODUCTO, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group col-md-12">
            @Html.LabelFor(Function(model) model.CLIENTE, htmlAttributes:=New With {.class = "control-label col-sm-6"})
            <div class="col-sm-6">
                @Html.DropDownList("CLIENTE", db.CLIENTES.Where(Function(o) o.PLANTAS.ID_BRAIN = idPlantaBRAIN).Select(Function(f) New SelectListItem With {.Text = f.NOMBRE, .Value = f.ID}).OrderBy(Function(f) f.Text), "Seleccionar uno", htmlAttributes:=New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(model) model.CLIENTE, "", New With {.Class = "text-danger"})
            </div>
        </div>

        <div class="form-group col-md-12">
            @Html.LabelFor(Function(model) model.CODXCLIENTE, htmlAttributes:=New With {.class = "control-label col-sm-6"})
            <div class="col-sm-6">
                @Html.EditorFor(Function(model) model.CODXCLIENTE, New With {.htmlAttributes = New With {.class = "form-control"}})
                @Html.ValidationMessageFor(Function(model) model.CODXCLIENTE, "", New With {.class = "text-danger"})
            </div>
        </div>

        <br />

        <div class="form-group col-md-12">
            @Html.LabelFor(Function(model) model.NUMPIEZASNOK, htmlAttributes:=New With {.class = "control-label col-sm-6 aExportarLbl"})
            <div class="col-sm-6">
                @Html.EditorFor(Function(model) model.NUMPIEZASNOK, New With {.htmlAttributes = New With {.class = "form-control aExportarTxt"}})
                @Html.ValidationMessageFor(Function(model) model.NUMPIEZASNOK, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group col-md-12">
            @Html.LabelFor(Function(model) model.DESCRIPCION, htmlAttributes:=New With {.class = "control-label col-sm-6 aExportarLbl"})
            <div class="col-sm-6">
                @Html.EditorFor(Function(model) model.DESCRIPCION, New With {.htmlAttributes = New With {.class = "form-control aExportarTxt", .rows = "4"}})
                @Html.ValidationMessageFor(Function(model) model.DESCRIPCION, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group col-md-12">
            @Html.LabelFor(Function(model) model.PROCEDENCIA, htmlAttributes:=New With {.class = "control-label col-sm-6"})
            <div class="col-sm-6">
                @Html.DropDownList("PROCEDENCIA", procedenciaList.AsEnumerable, "Seleccionar uno", htmlAttributes:=New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(model) model.PROCEDENCIA, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group col-md-12">
            @Html.LabelFor(Function(model) model.CLASIFICACION, htmlAttributes:=New With {.class = "control-label col-sm-6"})
            <div class="col-sm-6">
                @Html.DropDownList("CLASIFICACION", clasificacionList.AsEnumerable, "Seleccionar uno", htmlAttributes:=New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(model) model.CLASIFICACION, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group col-md-12">
            @Html.LabelFor(Function(model) model.NIVELIMPORTANCIA, htmlAttributes:=New With {.class = "control-label col-sm-6 aExportarLbl"})
            <div class="col-sm-6 nivelimportancia">
                <div style="display:flex">
                    @Html.DropDownList("NIVELIMPORTANCIA", nivelimportanciaList.AsEnumerable, "Seleccionar uno", htmlAttributes:=New With {.Class = "form-control aExportarTxt"})
                    <img src="~/Content/themes/base/Imagenes/info_mini32.png" title="<strong>ALTO:</strong> Impacto medio-alto en cliente final o característica de seguridad afectada<br /><strong>MEDIO:</strong> Impacto alto en tu planta o impacto bajo en cliente final<br /><strong>BAJO:</strong> Impacto bajo-medio en tu planta<br /><strong>SIN IMPACTO:</strong> El resto (Sin impacto en tu planta pero es necesario registrarla como NC)" data-toggle="tooltip" data-html="true" />
                </div>
                @Html.ValidationMessageFor(Function(model) model.NIVELIMPORTANCIA, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group col-md-12">
            @Html.LabelFor(Function(model) model.REPETITIVA, htmlAttributes:=New With {.class = "control-label col-sm-6 aExportarLbl"})
            <div class="col-sm-6">
                @Html.DropDownList("REPETITIVA", ncRepetitivaList.AsEnumerable, "Seleccionar uno", htmlAttributes:=New With {.Class = "form-control aExportarTxt"})
                @Html.ValidationMessageFor(Function(model) model.REPETITIVA, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group col-md-12" style="display:@oficialVisible">
            @Html.LabelFor(Function(model) model.RECLAMACIONOFICIAL, htmlAttributes:=New With {.class = "control-label col-sm-6"})
            <div class="col-sm-6">
                @Html.DropDownList("RECLAMACIONOFICIAL", reclamacionOficialList.AsEnumerable, "Seleccionar uno", htmlAttributes:=New With {.Class = "form-control"})
                @Html.ValidationMessageFor(Function(model) model.RECLAMACIONOFICIAL, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group col-md-12">
            @Html.LabelFor(Function(model) model.AFECTA_INDICADORES, htmlAttributes:=New With {.class = "control-label col-sm-6"})
            <div class="col-sm-6">
                @Html.DropDownList("AFECTA_INDICADORES", afectaIndicadoresList.AsEnumerable, "Seleccionar uno", htmlAttributes:=New With {.Class = "form-control"})
                @Html.ValidationMessageFor(Function(model) model.AFECTA_INDICADORES, "", New With {.class = "text-danger"})
            </div>
        </div>

        @Html.Hidden("PLANTACLIENTE", idPlantaBRAIN)
        @Html.Hidden("CREARENGTK", True)

        <div class="form-group">
            <div style="text-align:center">
                <input id="btn-submit" type="submit" value="@(If(editMode, " Actualizar", "Crear"))" Class="btn btn-info" style="font-size:18px;" />
            </div>
        </div>

        <div id="confirmBox">
            <div class="message"></div>
            <button class="yes">Si</button>
            <button class="no">No</button>
        </div>

    </div>
End Using

@section Scripts
    @Scripts.Render("~/bundles/jqueryval")
    <script type="text/javascript">
        // Inicializar tooltips
        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        });

        // Configuración del datepicker si el navegador no soporta input type=date
        if (!Modernizr.inputtypes.date) {
            $(function () {
                $.SeparadorDecimal(",");
                $('#datetimepicker1').datetimepicker({
                    format: 'YYYY/MM/DD',
                    defaultDate: '@Date.Today.ToString("yyyy/MM/dd")',
                    maxDate: moment()
                });
            });
        }

        // Autocomplete para REFINTERNAPIEZA
        $(function () {
            $('#REFINTERNAPIEZA').autocomplete({
                source: '@Url.Action("Suggest", New With {.emp = idPlantaBRAIN})',
                minLength: 3,
                select: function (evt, ui) {
                    $.ajax({
                        type: 'POST',
                        url: '@Url.Action("GetDataFromPieza", New With {.emp = idPlantaBRAIN})',
                        data: { input: ui.item.value },
                        success: function (result) {
                            document.getElementById("DENOMINACION").setAttribute("value", result[0]);
                            if (result[1] == '') {
                                result[1] = 'N/A'
                            }
                            if (result[2] == '') {
                                result[2] = 'N/A'
                            }
                            $('#PROYECTO').val(result[1]);
                            $('#REFCLIENTE').val(result[2]);
                            if(result[3] == null || result[3] == '' || result[3] == 0){
                                $('#PRODUCTO').val('');
                                $('#PRODUCTO > option').attr('disabled', true);
                            } else {
                                $('#PRODUCTO').val(result[3]);
                                $('#PRODUCTO > option:not(:selected)').attr('disabled', true);
                                $('#PRODUCTO > option:selected').attr('disabled', false);
                                $('#PRODUCTO-error').parent().empty();
                            }
                        },
                        error: function (ex) {
                            alert('Failed to retrieve states.' + ex);
                        }
                    });
                }
            });
        });

        // Control de cambio en CLIENTE
        $('#CLIENTE').change(function () {
            var text = this.options[this.selectedIndex].text;
            var value = this.options[this.selectedIndex].value;
            if (value == '' || value == null || value == 0) {
                $('#CLASIFICACION > option').attr('disabled', false);
                $('#CLASIFICACION').removeAttr('readonly');
                if ($('#CLASIFICACION').val() == null || $('#CLASIFICACION').val() == 3) {
                    $('#CLASIFICACION').val("");
                }
            } else if (text.indexOf("BATZ ") > -1 || text.indexOf("FPK BRASIL") > -1) {
                $('#CLASIFICACION').val(3);
                $('#CLASIFICACION').attr('readonly', 'readonly');
            } else {
                $('#CLASIFICACION > option[value="3"]').attr('disabled', true);
                if ($('#CLASIFICACION').val() == null || $('#CLASIFICACION').val() == 3) {
                    $('#CLASIFICACION').val("");
                }
                $('#CLASIFICACION').removeAttr('readonly');
            }
        });

        // Control de cambio en PROCEDENCIA
        $('#PROCEDENCIA').change(function () {
            var text = this.options[this.selectedIndex].text;
            var value = this.options[this.selectedIndex].value;
            if (value == '6' || value == 6) {
                $('#AFECTA_INDICADORES').val(0);
                $('#AFECTA_INDICADORES').attr('readonly', 'readonly');
            } else {
                $('#AFECTA_INDICADORES').val('');
                $('#AFECTA_INDICADORES').removeAttr('readonly');
            }
        });

        // Control de inicialización de la página
        window.onload = function () {
            var editMode = $('#EditMode').val();
            var exists = $('#CREADOR option[value="' + @userId + '"]').length > 0;
            if (exists && editMode == "False") {
                $('#CREADOR').val(@userId);
            } else if (!exists && editMode == "False") {
                $('#CREADOR').val("");
            } else {
                $('#CLIENTE').trigger("change");
            }
            if (@lockUsers.ToString().ToLower() == "true" || editMode == "True") {
                $('#CREADOR').attr("readonly","readonly");
            }
            if (editMode == "True") {
                $('#REFINTERNAPIEZA').attr("readonly","readonly");
            }
            $('#PRODUCTO > option:not(:selected)').attr('disabled', true);
            $('#PRODUCTO').css("background-color", "#eee");
        }

        var form;

        // Función para controlar el cambio en CLASIFICACION
        $(document).ready(function () {
            $('#CLASIFICACION').change(function () {
                var clasificacion = $(this).val();
                console.log("Clasificación seleccionada:", clasificacion); // Depuración

                if (clasificacion === "1") { // "Planta cliente (Km.0)"
                    // Mantener "Reclamación oficial" editable
                    $('#RECLAMACIONOFICIAL').prop('disabled', false);
                    console.log("Reclamación oficial habilitada para Planta cliente (Km.0)."); // Depuración
                } else {
                    // Mantener "Reclamación oficial" editable para otras clasificaciones
                    $('#RECLAMACIONOFICIAL').prop('disabled', false);
                    console.log("Reclamación oficial habilitada para otras clasificaciones."); // Depuración
                }
            });

            // Ejecutar el cambio de clasificación al cargar la página para establecer el estado inicial
            $('#CLASIFICACION').trigger('change');
        });

        // Evento de click para el botón de submit
        $(function () {
            $('#btn-submit').click(function (e) {
                var reclamacionOficial = $('#RECLAMACIONOFICIAL').val();
                var clasificacion = $('#CLASIFICACION').val();

                // Mostrar pop-up solo si la clasificación NO es "Garantías" (valor "2") y "Reclamación oficial" es "No"
                if (reclamacionOficial == 2 && clasificacion !== "2") {
                    e.preventDefault();
                    form = $(this).closest('form');
                    var dialog = $('<p>Estás creando una reclamación <u>no oficial</u>. Pese a ello, quieres crear una NC en GTK?</p>').dialog({ // jquery ui dialog
                        closeOnEscape: false,
                        title: "Atención",
                        buttons: {
                            "Si": function () {
                                $('#CREARENGTK').val(true);
                                dialog.dialog('close');
                                form.submit();
                            },
                            "No": function () {
                                $('#CREARENGTK').val(false);
                                dialog.dialog('close');
                                form.submit();
                            }
                        },
                        modal: true,
                        resizable: false,
                        draggable: false
                    });
                }
            });
        });
    </script>
End Section
