@Imports CostCarriersLib

@code
    Dim cabecera As ELL.CabeceraCostCarrier = CType(ViewData("Cabecera"), ELL.CabeceraCostCarrier)

    'Dim resumen As List(Of ResumenCambios) = CType(ViewData("ResumenCambios"), List(Of ResumenCambios))
    Dim valor As String = String.Empty
    Dim valorPopup As String = String.Empty
    Dim culturaEsES As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture("es-ES")
    Dim idCostGroupDesplegar As Integer = Integer.MinValue
    Dim idEstadoDesplegar As Integer = Integer.MinValue

    If (Request.QueryString.AllKeys.Contains("idCostGroup")) Then
        idCostGroupDesplegar = CInt(Request.QueryString("idCostGroup"))
        idEstadoDesplegar = BLL.CostsGroupBLL.Obtener(idCostGroupDesplegar).IdEstado
    End If

End Code

<script src="~/Scripts/jquery.alphanum.js"></script>

<script type="text/javascript">
    function CargarModalCambioPlantaStep(idStep, idPlanta, idCostGroupOT) {
        $.ajax({
            url: '@Url.Action("CargarDatosCambioEstadoStep", "CostCarriers")',
            data: { idPlanta: idPlanta, idCostGroupOT: idCostGroupOT  },
            type: 'GET',
            dataType: 'json',
            success: function (d) {
                $('#selectPlantaEstados').empty();
                if (d.length > 0) {
                    $("#hIdStep").val(idStep);
                    $.each(d, function (i, data) {
                        $('#selectPlantaEstados').append($('<option>', {
                            value: data.Id,
                            text : data.PlantaEstado
                        }));
                    });
                }
                else {
                    $('#selectPlantaEstados').append($('<option>', {
                        value: "",
                        text: "<@Utils.Traducir("No hay valores para mostrar")>"
                    }));
                }
                $('#modalWindowCambioPlantaStep').modal('show');
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    }

    function PlegarEstado(object, visible) {
        if (visible == true) {
            object.nextUntil("tr.info", "tr.active, tr.step").hide();
            object.nextUntil("tr.info", "tr.active").find("td span[data-idcostgroup]").removeClass("glyphicon-chevron-up");
            object.nextUntil("tr.info", "tr.active").find("td span[data-idcostgroup]").addClass("glyphicon-chevron-down");
        }
        else {
            object.nextUntil("tr.info", "tr.active").show();
        }
    }

    function PlegarCostGroup(object, visible) {
        if (visible == true) {
            object.nextUntil("tr.active", "tr.step").hide();
        }
        else {
            object.nextUntil("tr.active", "tr.step").show();
        }
    }

    //Si se borra o se añade un step se quiere que al recargar se despliegue su estado y su costgroup
    function GestionarStep(idEstado, idCostGroup)
    {
        if (idEstado != @Integer.MinValue){
            // Ponemos el icono del estado
            $("span[data-idestado='" + idEstado + "']").removeClass("glyphicon-chevron-down");
            $("span[data-idestado='" + idEstado + "']").addClass("glyphicon-chevron-up");
            $("tr.info[data-idestado='" + idEstado + "']").show();

            PlegarEstado($("tr.info[data-idestado='" + idEstado + "']"), false);

            // Ponemos el icono del costgroup
            $("span[data-idcostgroup='" + idCostGroup + "']").removeClass("glyphicon-chevron-down");
            $("span[data-idcostgroup='" + idCostGroup + "']").addClass("glyphicon-chevron-up");
            $("tr.info[data-idcostgroup='" + idCostGroup + "']").show();

            PlegarCostGroup($("tr.active[data-idcostgroup='" + idCostGroup + "']"), false);
        }
    }

    $(function () {
        $('.toolTp').tooltipster({
            side: 'right',
            theme: 'tooltipster-shadow',
            contentAsHTML: true,
            trigger: 'click',
            content: '@Utils.Traducir("Cargando")',
            // 'instance' is basically the tooltip. More details in the "Object-oriented Tooltipster" section.
            functionBefore: function (instance, helper) {
                var $origin = $(helper.origin);

                // we set a variable so the data is only loaded once via Ajax, not every time the tooltip opens
                if ($origin.data('loaded') !== true) {
                    var idStep = $(helper.origin).data("idstep")

                    $.get('MostrarLineasPedidoStep?idStep=' + idStep, function (data) {
                        // call the 'content' method to update the content of our tooltip with the returned data.
                        // note: this content update will trigger an update animation (see the updateAnimation option)
                        instance.content(data);

                        // to remember that the data has been loaded
                        $origin.data('loaded', true);
                    });
                }
            }
        });

        $("#anyosSerie").numeric({
            allowThouSep: false,
            allowDecSep: false
        });

        // Este if es para cuando se quiere crear un nuevo step pero previamente se han guardado los valores de la solicitud
        @*if ('@CStr(ViewData("IdCostGroupParaNuevoStep"))' != '') {
            $("#hCostGroupParaStep").val('@ViewData("IdCostGroupParaNuevoStep")');
            $('#modalWindowAgregarStep').modal('show');
        };*@

        $("#btnAñadrNuevoPaso").click(function () {
            var grupoSteps = $("#grupoSteps");
            var newId= 0
            var id = 0
            $("input[id^='txtAgregarDescripcionStep-']").each(function () {
                id = this.id.split("-")[1];

                if (parseInt(newId) <= parseInt(id)) {
                    newId = id;
                }
            })
            newId = parseInt(newId) + 1;

            grupoSteps.append("<div Class='form-group nuevo-step-dinamico'><div class='col-sm-offset-3 col-sm-7'><input type='text' class='form-control' name='txtAgregarDescripcionStep-" + newId + "' id='txtAgregarDescripcionStep-" + newId + "'></div></div>");
        });

        $("#request").submit(function (event) {
            if ($(this).find("input[type=submit]:focus")[0].id == 'validar' || $(this).find("input[type=submit]:focus")[0].id == 'facturar') {
                // Desactivamos los botones para que no se hagan más clicks
                //$("#validar").prop('disabled', true);
                //$("#facturar").prop('disabled', true);

                $(this).attr("target", "_blank");
            }
            else {
                $(this).attr("target", "_self");
            }
        });

        $("#btnConfirmCambiarAbreviatura").click(function () {
            if ($("#formCambiarAbreviatura").valid()) {
                if ($("#txtAbreviatura").val() != '') {
                    $("#contenedorCargando").show();
                    $.ajax({
                        url: '@Url.Action("CambiarAbreviatura", "CostCarriers")',
                        data: { hIdCabecera: $("#hIdCabecera").val(), txtAbreviatura: $("#txtAbreviatura").val()},
                        type: 'POST',
                        success: function (d) {
                            $("#titulo").text(d.titulo);
                            $("#hfAbreviatura").val(d.abreviatura);
                            @*$("#hfSOP").val(new Date(parseInt(d.sop.replace('/Date(', ''))).toLocaleDateString("@Threading.Thread.CurrentThread.CurrentCulture.Name"));
                            $("#hfAnyosSerie").val(d.anyosserie);*@
                            alert('@Html.Raw(Utils.Traducir("Datos cambiados correctamente"))');
                            $("#contenedorCargando").hide();
                            $("#modalWindowCambiarAbreviatura").modal("hide");
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                            $("#contenedorCargando").hide();
                            $("#modalWindowCambiarAbreviatura").modal("hide");
                        }
                    });
                }
            }

            return false;
        });

        $("#fechaSOP").datetimepicker({
            showClear: true,
            locale: '@Threading.Thread.CurrentThread.CurrentCulture.Name',
            format: '@Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper'
        });

        $("span.glyphicon.glyphicon-eye-open.text-danger").popover();

        $(window).keydown(function (event) {
            if (event.which == 13) {
                event.preventDefault();
                return false;
            }
        });

        $("#resumen").click(function () {
            var visible = $("#divResumen").is(":visible");
            if (visible == true) {
                $("#divResumen").hide("slow");
            }
            else {
                $("#divResumen").show("slow");
            }
        })

        $(".stepResumen").click(function () {
            var idEstado = $(this).data("idestado");
            var idCostGroup = $(this).data("idcostgroup");

            $("tr.info").each(function (f) {
                PlegarEstado($(this), true);
            });

            $("span[data-idestado]").removeClass("glyphicon-chevron-up");
            $("span[data-idestado]").addClass("glyphicon-chevron-down");
            $("span[data-idcostgroup]").removeClass("glyphicon-chevron-up");
            $("span[data-idcostgroup]").addClass("glyphicon-chevron-down");

            GestionarStep(idEstado, idCostGroup);

            //Deshabilitamos las filas de steps
            $("tr.step").each(function (f) {
                if ($(this).data("enabled") == 'True') {
                    $(this).prop("disabled", false);
                    $(this).find('input').prop("disabled", false);
                } else {
                    $(this).prop("disabled", true);
                    $(this).find('input').prop("disabled", true);
                }
            });
        })

        $(".cambio-planta").click(function () {
            var idStep = $(this).data("idstep");
            var idPlanta = $(this).data("idplanta");
            var idCostGroupOT = $(this).data("idcostgroupot");

            CargarModalCambioPlantaStep(idStep, idPlanta, idCostGroupOT);
        })

        function Editar(e) {
            @*//Vamos a recoger todos los label que tengan data-pbctotal. Ese atributo contiene el id del step
            //Tenemos que comparar su valor con el sumatorio de todos los textbox con data-pbcvalor y mismo idStep
            $("[data-pbctotal]").each(function () {
                var idStep = $(this).data('pbctotal');
                var stepname = $(this).data('stepname');
                var total = parseInt($(this).text().replace(/\./g, ""));
                var sumaTotal = 0;
                $("[data-pbcvalor='" + idStep + "']").each(function (f) {
                    //Vamos sumando los valores
                    if ($(this).val() != '' && $.isNumeric($(this).val())) {
                        sumaTotal = sumaTotal + parseInt($(this).val().replace(/\./g, ""));
                    }
                })

                //if (sumaTotal != '' && total != sumaTotal) {
                if (total != sumaTotal) {
                    var idTipoProyecto = $($("[data-tipoproyecto]")[0]).data("tipoproyecto");

                    if (idTipoProyecto == 1) {
                        alert('Step: ' + stepname + '\n' + '@Html.Raw(Utils.Traducir("Financiación externa")) ' + String(total) + ' @Html.Raw(Utils.Traducir("difiere de")) ' + '@Html.Raw(Utils.Traducir("suma de ingresos")) ' + String(sumaTotal));
                    } else {
                        alert('Step: ' + stepname + '\n' + '@Html.Raw(Utils.Traducir("Pagado por cliente")) ' + String(total) + ' @Html.Raw(Utils.Traducir("difiere de")) ' + '@Html.Raw(Utils.Traducir("suma de ingresos")) ' + String(sumaTotal));
                    }

                    e.preventDefault();
                    return false;
                }
            })

            $("[data-bactotal]").each(function () {
                var idStep = $(this).data('bactotal');
                var stepname = $(this).data('stepname');
                var total = parseInt($(this).text().replace(/\./g, ""));
                var sumaTotal = 0;
                $("[data-bacvalor='" + idStep + "']").each(function (f) {
                    //Vamos sumando los valores
                    if ($(this).val() != '' && $.isNumeric($(this).val())) {
                        sumaTotal = sumaTotal + parseInt($(this).val().replace(/\./g, ""));
                    }
                })

                //if (sumaTotal != '' && total != sumaTotal) {
                if (total != sumaTotal) {
                    var idTipoProyecto = $($("[data-tipoproyecto]")[0]).data("tipoproyecto");
                    alert('Step: ' + stepname + '\n' + '@Html.Raw(Utils.Traducir("Presupuesto aprobado")) ' + String(total) + ' @Html.Raw(Utils.Traducir("difiere de")) ' + '@Html.Raw(Utils.Traducir("suma de gastos")) ' + String(sumaTotal));

                    e.preventDefault();
                    return false;
                }
            })*@

            //Vamos a recoger todos los textboxes que tengan data-idagrupacion. Hay que mirarlos por planta, estado e idagrupacion.
            //Tiene que sumar 100 entre todos
            $("[data-idagrupacion]").each(function () {
                var idAgrupacion = $(this).data('idagrupacion');
                var idPlanta = $(this).data('idplanta');
                var idEstado = $(this).data('idestado');

                var sumaTotal = 0;
                $("[data-idagrupacion='" + idAgrupacion + "'][data-idplanta='" + idPlanta + "'][data-idestado='" + idEstado + "']").each(function (f) {
                    //Vamos sumando los valores
                    var valorCelda = $(this).val().replace(/\./g, "");
                    if (valorCelda != '' && $.isNumeric(valorCelda)) {
                        sumaTotal = sumaTotal + parseInt(valorCelda);
                    }
                })

                if (sumaTotal != 100) {
                    alert('@Html.Raw(Utils.Traducir("La suma total de los porcentajes por planta y estado de los pasos debe ser 100"))');
                    e.preventDefault();
                    return false;
                }
            })
        }

        function ValidarFacturar(e) {
            //Vamos a verificar que se han marcado steps para validar
            var suma = 0;
            var idplanta = -1;

            $(".checkbox:checked").each(function () {
                if (idplanta == -1) {
                    idplanta = $(this).data("idplanta");
                }

                if (idplanta != $(this).data("idplanta")) {
                    alert('@Html.Raw(Utils.Traducir("Los pasos para facturar no pueden pertenecer a diferentes plantas"))');
                    e.preventDefault();
                    return false;
                }

                if ($(this).data("costcarrier") != '') {
                    suma = suma + 1;
                } else {
                    // Lo desmarcamos para que no llegue al enviarse el valor
                    $(this).prop("checked", false);
                    var idCostGroup = $(this).data("idcostgroup");

                    //Desmarcamos el check padre del cost group
                    $("#chkCostGroup-" + idCostGroup).prop("checked", false);
                }
            })

            if (suma == 0) {
                alert('@Html.Raw(Utils.Traducir("Debe marcar algún paso abierto para enviar a facturar"))');
                e.preventDefault();
                return false;
            }
        }

        function Validar(e) {
            if ($("#hfAbreviatura").val() == '') {
                alert('@Html.Raw(Utils.Traducir("El proyecto no tiene informada la abreviatura"))');
                e.preventDefault();

                $("#txtAbreviatura").val('');

                $("#lblSOP").hide();
                $("#fechaSOP").hide();
                $("#SOP").prop("required", false);
                $("#SOP").val('');

                $("#lblAnyosSerie").hide();
                $("#anyosSerie").hide();
                $("#anyosSerie").prop("required", false);
                $("#anyosSerie").val('');

                @*if ('@cabecera.TipoProyPtksis' != '@ELL.CabeceraCostCarrier.TIPO_PROY_ECO_BATZ') {
                    $("#lblSOP").hide();
                    $("#fechaSOP").hide();
                    $("#SOP").prop("required", false);
                    $("#SOP").val('');

                    $("#lblAnyosSerie").hide();
                    $("#anyosSerie").hide();
                    $("#anyosSerie").prop("required", false);
                    $("#anyosSerie").val('');
                }
                else {
                    $("#lblSOP").show();
                    $("#fechaSOP").show();
                    $("#SOP").prop("required", true);
                    $("#SOP").val('');
                    if ('01/01/0001 00:00:00' != '01/01/0001 00:00:00') {
                        $("#SOP").val($("#hfSOP").val());
                    }

                    $("#lblAnyosSerie").show();
                    $("#anyosSerie").show();
                    $("#anyosSerie").prop("required", true);
                    $("#anyosSerie").val('');
                    if (-2147483648 != -2147483648) {
                        $("#anyosSerie").val($("#hfAnyosSerie").val());
                    }
                }*@

                $('#modalWindowCambiarAbreviatura').modal('show');
                return false;
            }

            //Vamos a verificar que se han marcado steps para validar
            //Tenemos que comparar su valor con el sumatorio de todos los textbox con data-pbcvalor y mismo idStep
            var suma = 0
            $(".checkbox:checked").each(function () {
                suma = suma + 1;
            })

            if (suma == 0) {
                alert('@Html.Raw(Utils.Traducir("Debe marcar algún paso para validar"))');
                e.preventDefault();
                return false;
            }

            //Solo vamos a validar para los pasos marcados para enviar a validar
            //Vamos a recoger todos los label que tengan data-pbctotal. Ese atributo contiene el id del step
            //Tenemos que comparar su valor con el sumatorio de todos los textbox con data-pbcvalor y mismo idStep
            $(".checkbox:checked").each(function () {
                var idStep = $(this).data('idstep');
                var stepname = $(this).data('stepname');
                var total = parseInt($("[data-pbctotal='" + idStep + "']").text().replace(/\./g, ""));

                if (!isNaN(total)){
                    var sumaTotal = 0;
                    $("[data-pbcvalor='" + idStep + "']").each(function (f) {
                        //Vamos sumando los valores
                        var valorCelda = $(this).val().replace(/\./g, "");
                        if (valorCelda != '' && $.isNumeric(valorCelda)) {
                            sumaTotal = sumaTotal + parseInt(valorCelda);
                        }
                    })

                    //if (sumaTotal != '' && total != sumaTotal) {
                    if (total != sumaTotal) {
                        var idTipoProyecto = $($("[data-tipoproyecto]")[0]).data("tipoproyecto");

                        if (idTipoProyecto == 1) {
                            alert('Step: ' + stepname + '\n' + '@Html.Raw(Utils.Traducir("Financiación externa")) ' + String(total) + ' @Html.Raw(Utils.Traducir("difiere de")) ' + '@Html.Raw(Utils.Traducir("suma de ingresos")) ' + String(sumaTotal));
                        } else {
                            alert('Step: ' + stepname + '\n' + '@Html.Raw(Utils.Traducir("Pagado por cliente")) ' + String(total) + ' @Html.Raw(Utils.Traducir("difiere de")) ' + '@Html.Raw(Utils.Traducir("suma de ingresos")) ' + String(sumaTotal));
                        }

                        e.preventDefault();
                        return false;
                    }
                }
            })

            //Solo vamos a validar para los pasos marcados para enviar a validar
            $(".checkbox:checked").each(function () {
                var idStep = $(this).data('idstep');
                var stepname = $(this).data('stepname');
                var total = parseInt($("[data-bactotal='" + idStep + "']").text().replace(/\./g, ""));
                var sumaTotal = NaN;
                $("[data-bacvalor='" + idStep + "']").each(function (f) {
                    //Vamos sumando los valores
                    var valorCelda = $(this).val().replace(/\./g, "");
                    if (valorCelda != '' && $.isNumeric(valorCelda)) {
                        if (isNaN(sumaTotal)) {
                            sumaTotal = 0;
                        }
                        sumaTotal = sumaTotal + parseInt(valorCelda);
                    }
                })

                //if (sumaTotal != '' && total != sumaTotal) {
                if (!isNaN(sumaTotal) && total != sumaTotal) {
                    var idTipoProyecto = $($("[data-tipoproyecto]")[0]).data("tipoproyecto");
                    alert('Step: ' + stepname + '\n' + '@Html.Raw(Utils.Traducir("Presupuesto aprobado")) ' + String(total) + ' @Html.Raw(Utils.Traducir("difiere de")) ' + '@Html.Raw(Utils.Traducir("suma de gastos")) ' + String(sumaTotal));

                    e.preventDefault();
                    return false;
                }
            })
        }

        $("#editar").click(function (e) {
            Editar(e)
        });

        $("#validar").click(function (e) {
            if (Validar(e) == false) {
                return
            }
            Editar(e);
        });

        $("#facturar").click(function (e) {
            if (ValidarFacturar(e) == false) {
                return;
            }
            Editar(e);
        });

        $('.step-description').click(function () {
            var input = $(this).siblings()[0];
            $(this).hide();
            $(input).val($(this).data("descripcion"));
            $(input).show();
            $(input).focus();
        });

        $('.step-description-input').on("blur", function (e) {
            var input = $(this);
            var costCarrier = $(this).data("portador");

            if (costCarrier != ''){
                costCarrier = ' (' + costCarrier + ')'
            }

            // Tenemos que guardar el nombre del step
            if ($(this).val() != '') {
                $.ajax({
                    url: '@Url.Action("CambiarDescripcionStep", "CostCarriers")',
                    data: { idStep: $(this).data('idstep'), descripcion: $(this).val()},
                    type: 'POST',
                    success: function () {
                        var label = $(input).prev()[0];
                        $(label).text($(input).val() + costCarrier);
                        $(label).data("descripcion", $(input).val());
                        $(input).hide();
                        $(label).show();
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });
            }

            return false;
        });

        $('.porcentaje').on("blur", function (e) {
            @*var idStep = $(this).data("idstep");
            var idAgrupacion = $(this).data("idagrupacion");
            var idCostGroup = $(this).data('idcostgroup');
            var porcentaje = parseInt($(this).val());
            var sumaTotal = 0;

            $("[data-idagrupacion='" + idAgrupacion + "']").each(function () {
                sumaTotal += parseInt($(this).val());
            });

            if (sumaTotal != 100) {
                alert('@Utils.Traducir("La suma de los porcentajes tiene que ser 100")');
                e.preventDefault();
                return false;
            }
            else {
                $.ajax({
                    url: '@Url.Action("CambiarPorcentajeStep", "CostCarriers")',
                    data: { idStep: idStep, porcentaje: porcentaje },
                    type: 'POST',
                    success: function () {
                        $("#contenedorCargando").show();
                        if (location.href.indexOf("&idCostGroup=") == -1) {
                            location.replace(location.href + "&idCostGroup=" + idCostGroup);
                        }
                        else {
                            location.replace(location.href.replace(/idCostGroup=\d+/, "idCostGroup=" + idCostGroup));
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });
            }*@
        });

        $("tr.danger").click(function (f) {
            //Pulsamos sobre una planta
            if (f.target.type == "button") {
                f.preventDefault();
                return false;
            }

            var visible = $(this).next().is(":visible");
            var idPlanta = $(this).data("idplanta");
            var span = $("span[data-idplanta='" + idPlanta + "']");

            span.toggleClass("glyphicon-chevron-up");
            span.toggleClass("glyphicon-chevron-down");

            if (visible == true) {
                //Ocultamos todas la filas estado, costgroup y step por debajo de la nuestra hasta la siguiente planta
                $(this).nextUntil("tr.danger", "tr.info, tr.active, tr.step").hide();
                //Quitamos el simbolo de plegar y ponemos el simbolo de desplegar para aquellas filas estado, costgroup por debajo de la nuestra hasta la siguiente planta
                $(this).nextUntil("tr.danger", "tr.info, tr.active").find("td span[data-idestado], td span[data-idcostgroup]").removeClass("glyphicon-chevron-up");
                $(this).nextUntil("tr.danger", "tr.info, tr.active").find("td span[data-idestado], td span[data-idcostgroup]").addClass("glyphicon-chevron-down");
            }
            else {
                $(this).nextUntil("tr.danger", "tr.info").show();
            }
        });

        $("tr.info").click(function (f) {
            //Pulsamos sobre un estado
            var visible = $(this).next().is(":visible");
            var idEstado = $(this).data("idestado");
            var span = $("span[data-idestado='" + idEstado + "']");

            span.toggleClass("glyphicon-chevron-up");
            span.toggleClass("glyphicon-chevron-down");

            PlegarEstado($(this), visible);
        });

        $("tr.active").click(function (f) {
            //Pulsamos sobre un cost group
            if (f.target.type == "button") {
                f.preventDefault();
                return false;
            }
            else if (f.target.type == "checkbox") {
                var idCostGroup = $(this).data("idcostgroup");
                $(":checkbox[data-idcostgroup='" + idCostGroup + "']").prop('checked', $(f.target).is(":checked"));
                return true;
            }

            var visible = $(this).next().is(":visible");
            var idCostGroup = $(this).data("idcostgroup");
            var span = $("span[data-idcostgroup='" + idCostGroup + "']");

            span.toggleClass("glyphicon-chevron-up");
            span.toggleClass("glyphicon-chevron-down");

            PlegarCostGroup($(this), visible);
        });

        $(".btn-agregarstep").click(function () {
            $("#hCostGroupParaStep").val($(this).data("idcostgroup"));
            $('#modalWindowAgregarStep').find('form').trigger('reset');
            $('.nuevo-step-dinamico').remove();
            $('#modalWindowAgregarStep').modal('show');
        })

        //$(".btn-cambiarsop").click(function () {
        //    var idPlanta = $(this).data("idplanta");
        //    var sop = $(this).data("sop");
        //    var anyos = $(this).data("anyos");
        //    var cambiarSOP = $(this).data("cambiarsop");

        //    $("#hIdPlanta").val(idPlanta);
        //    $("#SOP").val(sop);
        //    $("#anyosSerie").val(anyos);
        //    if (cambiarSOP == "True") {
        //        $("#btnConfirmCambiarSOP").show();
        //        $("#anyosSerie").prop("disabled", false);
        //        $("#SOP").prop("disabled", false);
        //    } else {
        //        $("#btnConfirmCambiarSOP").hide();
        //        $("#anyosSerie").prop("disabled", true);
        //        $("#SOP").prop("disabled", true);
        //    }

        //    $('#modalWindowCambiarSOP').modal('show');
        //})

        $(".btn-eliminarstep").click(function () {
            if (confirm('@Html.Raw(Utils.Traducir("¿Desea eliminar el paso seleccionado?"))')) {
                var idStep = $(this).data('idstep');
                var idCostGroup = $(this).data('idcostgroup');
                $.ajax({
                    url: '@Url.Action("EliminarStep", "CostCarriers")',
                    data: { id: idStep },
                    type: 'POST',
                    success: function (d) {
                        var estilo;
                        switch (d.messageType) {
                            case 'info':
                                estilo = "alert alert-success";
                                break;
                            case 'error':
                                estilo = "alert alert-danger";
                                break;
                            default:
                        }

                        //No vale con borrar la fila porque no recarga los totales
                        //$("tr.step[data-idstep='" + idStep + "']").remove();
                        //MostrarMensaje(d.message, estilo);
                        //location.reload();

                        $("#contenedorCargando").show();
                        if (location.href.indexOf("&idCostGroup=") == -1) {
                            location.replace(location.href + "&idCostGroup=" + idCostGroup);
                        }
                        else {
                            location.replace(location.href.replace(/idCostGroup=\d+/, "idCostGroup=" + idCostGroup));
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });
            }
        })

        //De inicio las columna de margin plegadas
        $(".shrink-margin:not(.margin-aux)").toggle();

        $("th.shrink-margin").click(function () {
            $(".shrink-margin").toggle();
        });

        //De inicio las columnas de años plegadas y plegado hasta el nivel de estado
        $("[data-anyo]:not(.anyoAux)").toggle();

        //Si la plantilla es de RD ocultamos las columnas Empresa, Presupuesto de oferta, Pagado por cliente, Objetivo y Margen
        $("[data-tipoProyecto=@CInt(ELL.TipoProyecto.TipoProyecto.R_D)]").hide();

        $("th.anyo, th.anyoAux").click(function () {
            var anyo = $(this).data("anyo");
            $("[data-anyo='" + anyo + "']").toggle();
        });

        $("tr.info").each(function (f) {
            PlegarEstado($(this), true);
        });

        $("span[data-idestado]").removeClass("glyphicon-chevron-up");
        $("span[data-idestado]").addClass("glyphicon-chevron-down");
        $("span[data-idcostgroup]").removeClass("glyphicon-chevron-up");
        $("span[data-idcostgroup]").addClass("glyphicon-chevron-down");

        GestionarStep(@idEstadoDesplegar, @idCostGroupDesplegar);

        //Deshabilitamos las filas de steps
        $("tr.step").each(function (f) {
            if ($(this).data("enabled") == 'True') {
                $(this).prop("disabled", false);
                $(this).find('input').prop("disabled", false);
            } else {
                $(this).prop("disabled", true);
                $(this).find('input').prop("disabled", true);
            }
        });

        $("#cambiarAbreviatura").click(function () {
            $("#txtAbreviatura").val($("#hfAbreviatura").val());
            @*if ('@cabecera.TipoProyPtksis' != '@ELL.CabeceraCostCarrier.TIPO_PROY_ECO_BATZ') {
                $("#lblSOP").hide();
                $("#fechaSOP").hide();
                $("#SOP").prop("required", false);
                $("#SOP").val('');

                $("#lblAnyosSerie").hide();
                $("#anyosSerie").hide();
                $("#anyosSerie").prop("required", false);
                $("#anyosSerie").val('');
            }
            else {
                $("#lblSOP").show();
                $("#fechaSOP").show();
                $("#SOP").prop("required", true);
                $("#SOP").val('');
                if ('@cabecera.SOP' != '@DateTime.MinValue') {
                    $("#SOP").val($("#hfSOP").val());
                }

                $("#lblAnyosSerie").show();
                $("#anyosSerie").show();
                $("#anyosSerie").prop("required", true);
                $("#anyosSerie").val('');
                if (@cabecera.SeriesYears != @Integer.MinValue) {
                    $("#anyosSerie").val($("#hfAnyosSerie").val());
                }
            }*@

            $('#modalWindowCambiarAbreviatura').modal('show');
        })
    })
</script>

<div id="contenedorCargando" Class="contenedor-cargando" style="display:none; z-index:99999;">
    <div Class="contenedor-imagen-cargando">
        <img src='@Url.Content("~/Content/Images/loading.gif")' />
    </div>
</div>

@code
    @Using Html.BeginForm("Editar", "CostCarriers", FormMethod.Post, New With {.id = "request"})
        @Html.Hidden("hIdCabecera", cabecera.Id)
        @Html.Hidden("hfAbreviatura", cabecera.Abreviatura)
        @*@Html.Hidden("hfSOP", cabecera.SOP.ToShortDateString())
        @Html.Hidden("hfAnyosSerie", cabecera.SeriesYears)*@
        @<h3>
            <label id="titulo">@ViewData("Titulo")</label>

            @code
                If (Not BLL.CabecerasCostCarrierBLL.ContienePasosAbiertos(cabecera.Id)) Then
                    @<button type="button" id="cambiarAbreviatura" Class="btn btn-default btn-xs btn-success" title='@Utils.Traducir("Cambiar datos cabecera")' style="margin-left:10px;margin-right:10px;"><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span></button>
                End If

                Dim steps As List(Of ELL.Step) = BLL.StepsBLL.CargarListadoPorCabecera(cabecera.Id)

                If (cabecera.TipoProyPtksis = ELL.CabeceraCostCarrier.TIPO_PROY_INDUSTRIALIZATION OrElse cabecera.TipoProyPtksis = ELL.CabeceraCostCarrier.TIPO_PROY_ECO_BATZ) Then
                    If (Not steps.Exists(Function(f) f.EsInfoGeneral)) Then
                        @<input type="submit" id="createGeneralInfo" name="submitButtonCreateGeneralInfo" value="@Utils.Traducir("Crear paso de información general")" Class="btn btn-primary" style="margin-left:50px;" />
                    Else
                        @<input type="submit" id="updateGeneralInfo" name="submitButtonUpdateGeneralInfo" value="@Utils.Traducir("Actualizar información general")" Class="btn btn-primary" style="margin-left:50px;" />
                    End If
                End If
            End code
        </h3>
        @<hr />
        @<input type="submit" id="editar" name="submitButtonSave" value="@Utils.Traducir("Guardar")" Class="btn btn-primary" />
        @<input type="submit" id="validar" name="submitButtonSendValidate" value="@Utils.Traducir("Enviar a validar")" Class="btn btn-primary" />

        @code
            If (Not cabecera.TipoProyPtksis.Equals(ELL.CabeceraCostCarrier.TIPO_PROY_R_D)) Then
                @<input type="submit" id="facturar" name="submitButtonSendInvoice" value="@Utils.Traducir("Enviar a facturar")" Class="btn btn-warning" style="margin-left:50px" />
            End If
        End code

        @*If (resumen IsNot Nothing AndAlso resumen.Count > 0) Thenfacturar
                @<input type="button" id="resumen" value="@Utils.Traducir("Resumen cambios")" Class="btn btn-primary btn-warning" />
            End If*@
        @<br />
        @<br />
        @<div id="divResumen" style="display:none;" class="col-sm-6">
            <table id="tablaResumen" class="table table-bordered table-condensed col-sm-6" style="font-size:x-small;">
                <thead>
                    <tr>
                        <th class="danger"><label Class="control-label">@Utils.Traducir("Planta")</label></th>
                        <th class="info"><label Class="control-label">@Utils.Traducir("Estado")</label></th>
                        <th class="active"><label Class="control-label">@Utils.Traducir("Grupo de coste")</label></th>
                        <th><label Class="control-label">@Utils.Traducir("Paso")</label></th>
                    </tr>
                </thead>
                <tbody>
                    @code
                        @*If (resumen IsNot Nothing AndAlso resumen.Count > 0) Then
                                For Each res In resumen
                                    @<tr>
                                        <td><label Class="control-label">@res.Planta</label></td>
                                        <td><label Class="control-label">@res.Estado</label></td>
                                        <td><label Class="control-label">@res.CostGroup</label></td>
                                        <td><label Class="control-label text-primary stepResumen" style="cursor:pointer;" data-idcostgroup='@res.IdCostGroup' data-idestado='@res.IdEstado'>@res.StepName </label></td>
                                    </tr>
                                Next
                            End If*@
                    End code
                </tbody>
            </table>
            <hr />
        </div>

        @<table id="tabla" class="table table-bordered" style="font-size:x-small;">
            <thead>
                <tr>
                    <th colspan="4" rowspan="3" class="success"><label Class="control-label">@String.Format("{0}/{1}/{2}/{3}", Utils.Traducir("Planta"), Utils.Traducir("Estado"), Utils.Traducir("Grupo de coste"), Utils.Traducir("Paso"))</label></th>
                    <th rowspan="3"></th>
                    @*<th rowspan="3"><label Class="control-label">@Utils.Traducir("Código")</label></th>*@
                    <th rowspan="3" data-tipoproyecto="@cabecera.IdTipoProyecto"><label Class="control-label">@Utils.Traducir("Empresa")</label></th>
                    <th rowspan="3" data-tipoproyecto="@cabecera.IdTipoProyecto"><label Class="control-label">@Utils.Traducir("Presupuesto de oferta")</label></th>
                    <th rowspan="3" data-tipoproyecto="@cabecera.IdTipoProyecto">
                        @code
                            If (cabecera.IdTipoProyecto = ELL.TipoProyecto.TipoProyecto.R_D) Then
                                @<label Class="control-label">@Utils.Traducir("Financiación externa")</label>
                            Else
                                @<label Class="control-label">@Utils.Traducir("Pagado por cliente")</label>
                            End If
                        End code
                    </th>
                    <th rowspan="3" data-tipoproyecto="@cabecera.IdTipoProyecto"><label Class="control-label">@Utils.Traducir("ObjetivoCC")</label></th>
                    <th rowspan="3"><label Class="control-label">@Utils.Traducir("Presupuesto aprobado")</label></th>
                    @For año As Integer = cabecera.Años.AnyoInicio To cabecera.Años.AnyoFin Step 1
                        @<th class="anyoAux col" data-anyo="@año" style="text-align:center;cursor:pointer;width:1%" rowspan="3">
                            <button type="button" class="btn btn-default btn-xs btn-success" title='@Utils.Traducir("Expandir año")'>
                                @año&nbsp;<span class="glyphicon glyphicon-zoom-in" aria-hidden="true"></span>
                            </button>
                        </th>
                        Dim colspan = 8
                        If (cabecera.IdTipoProyecto = ELL.TipoProyecto.TipoProyecto.R_D) Then
                            colspan = 4
                        End If

                        @<th colspan='@colspan' style="text-align:center;cursor:pointer;" class="anyo" data-anyo="@año">
                            <button type="button" class="btn btn-default btn-xs btn-info" title='@Utils.Traducir("Contraer año")'>
                                @año&nbsp;<span class="glyphicon glyphicon-zoom-out" aria-hidden="true"></span>
                            </button>
                        </th>
                    Next
                    @*<th rowspan="3" data-tipoproyecto="@cabecera.IdTipoProyecto"><label Class="control-label">@Utils.Traducir("Margen") % (@Utils.Traducir("Presupuesto aprobado"))</label></th>
                        <th rowspan="3"><label Class="control-label">@Utils.Traducir("Margen") % (@Utils.Traducir("Real"))</label></th>
                        <th rowspan="3"><label Class="control-label">@Utils.Traducir("Dato real interno")</label></th>
                        <th rowspan="3"><label Class="control-label">@Utils.Traducir("Dato real externo")</label></th>*@
                    <th rowspan="3" style="text-align:center;width:1%;" class="shrink-margin margin-aux">
                        @* <label Class="control-label">@Utils.Traducir("Margen") % (@Utils.Traducir("solo venta"))</label>*@
                        <button type="button" class="btn btn-default btn-xs btn-success" title='@Utils.Traducir("Expandir")'>
                            @Utils.Traducir("Margen") % <br /> (@Utils.Traducir("sólo ventas"))&nbsp;<span class="glyphicon glyphicon-zoom-in" aria-hidden="true"></span>
                        </button>
                    </th>
                    <th rowspan="2" style="text-align:center;" class="shrink-margin">
                        @* <label Class="control-label">@Utils.Traducir("Margen") % (@Utils.Traducir("solo venta"))</label>*@
                        <button type="button" class="btn btn-default btn-xs btn-info" title='@Utils.Traducir("Expandir")'>
                            @Utils.Traducir("Margen") % (@Utils.Traducir("sólo ventas"))&nbsp;<span class="glyphicon glyphicon-zoom-ot" aria-hidden="true"></span>
                        </button>
                    </th>
                    @*<th rowspan="2" colspan="2" style="text-align:center;"><label Class="control-label">@Utils.Traducir("Dato real")</label></th>*@
                </tr>
                <tr>
                    @For año As Integer = cabecera.Años.AnyoInicio To cabecera.Años.AnyoFin Step 1
                        @*@<th class="anyoAux" data-anyo="@año"></th>*@
                        @<th colspan="4" style="text-align:center; background-color: #fadd97;" data-anyo="@año"><label Class="control-label">@Utils.Traducir("Gastos")</label></th>
                        If (cabecera.IdTipoProyecto <> ELL.TipoProyecto.TipoProyecto.R_D) Then
                            @<th colspan="4" style="text-align:center; background-color: #d8ebd5" data-anyo="@año"><label Class="control-label">@Utils.Traducir("Ingresos")</label></th>
                        End If
                    Next
                </tr>
                <tr>
                    @For año As Integer = cabecera.Años.AnyoInicio To cabecera.Años.AnyoFin Step 1
                        @*@<th class="anyoAux" data-anyo="@año"></th>*@
                        @For cont As Integer = 1 To 2 Step 1
                            Dim color = String.Empty
                            If (cont = 1) Then
                                color = "#fadd97"
                            Else
                                color = "#d8ebd5"
                            End If

                            @For trimestre As Integer = 1 To 4 Step 1
                                If (cont = 1 OrElse (cont = 2 AndAlso cabecera.IdTipoProyecto <> ELL.TipoProyecto.TipoProyecto.R_D)) Then
                                    @<th style="text-align:center; background-color: @color;" data-anyo="@año"><label Class="control-label">@trimestre</label></th>
                                End If
                            Next
                        Next
                    Next
                    <th data-tipoproyecto="@cabecera.IdTipoProyecto" class="shrink-margin">@Utils.Traducir("Presupuesto aprobado")</th>
                    @*<th class="shrink-margin">@Utils.Traducir("Dato real")</th>
                        <th>@Utils.Traducir("INT")</th>
                        <th>@Utils.Traducir("EXT")</th>*@
                </tr>
            </thead>
            <tbody>
                @For Each planta In cabecera.Plantas.OrderBy(Function(f) f.IdPlanta)
                    @<tr class="danger" style="cursor: pointer;" data-idplanta='@planta.Id'>
                        <td colspan="4">
                            <span class="glyphicon glyphicon-chevron-up" aria-hidden="true" data-idplanta='@planta.Id'></span>
                            <label Class="control-label">@String.Format("{0} ({1}: {2})", planta.Planta, Utils.Traducir("Moneda"), planta.Moneda)</label>

                            @*@code
                                    Dim cambiarSOP As Boolean = False
                                    If (cabecera.IdOferta = Integer.MinValue AndAlso Not BLL.ProyectosPtksisBLL.Obtener(cabecera.Proyecto).Estado = "Predevelopment") Then
                                        cambiarSOP = True
                                    End If

                                    Dim sop As DateTime = planta.SOP
                                    Dim años As Integer = planta.AñosSerie
                                    Dim sopCadena As String = String.Empty
                                    Dim añosCadena As String = String.Empty

                                    If (sop <> DateTime.MinValue) Then
                                        sopCadena = sop.ToShortDateString()
                                    End If

                                    If (años <> Integer.MinValue) Then
                                        añosCadena = años
                                    End If

                                    @<button type="button" id="cambiarSOP" Class="btn btn-default btn-xs btn-success btn-cambiarsop" title='@Utils.Traducir("Cambiar SOP y años serie")' data-idplanta="@planta.Id" data-sop="@sopCadena" data-anyos="@añosCadena" data-cambiarSOP="@cambiarSOP" style="margin-left:10px;"><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span></button>
                                End code*@
                        </td>
                        <td></td>
                        <td data-tipoproyecto="@cabecera.IdTipoProyecto"></td>
                        <td align="right" data-tipoproyecto="@cabecera.IdTipoProyecto"><label Class="control-label">@planta.OBCSumatorio.ToString("N0", culturaEsES)</label></td>
                        <td align="right" data-tipoproyecto="@cabecera.IdTipoProyecto"><label Class="control-label">@planta.PBCSumatorio.ToString("N0", culturaEsES)</label></td>
                        <td align="right" data-tipoproyecto="@cabecera.IdTipoProyecto"><label Class="control-label">@planta.TCSumatorio.ToString("N0", culturaEsES)</label></td>
                        <td align="right"><label Class="control-label">@planta.BACGastosSumatorio.ToString("N0", culturaEsES)</label></td>

                        @For año As Integer = cabecera.Años.AnyoInicio To cabecera.Años.AnyoFin Step 1
                            @<td class="anyoAux" data-anyo="@año"></td>
                            @For cont As Integer = 1 To 2 Step 1
                                @For trimestre As Integer = 1 To 4 Step 1
                                    If (cont = 1) Then
                                        @<td align="right" data-anyo="@año" style="background-color: #fadd97;"><label class="control-label">@planta.GastosAñoSumatorio.Where(Function(f) f.Año = año AndAlso f.Trimestre = trimestre).Sum(Function(f) f.Valor).ToString("N0", culturaEsES)</label></td>
                                    ElseIf (cont = 2 AndAlso cabecera.IdTipoProyecto <> ELL.TipoProyecto.TipoProyecto.R_D) Then
                                        @<td align="right" data-anyo="@año" style="background-color: #d8ebd5"><label class="control-label">@planta.IngresosAñoSumatorio.Where(Function(f) f.Año = año AndAlso f.Trimestre = trimestre).Sum(Function(f) f.Valor).ToString("N0", culturaEsES)</label></td>
                                    End If
                                Next
                            Next
                        Next
                        <td align="right" data-tipoproyecto="@cabecera.IdTipoProyecto" class="shrink-margin"><label Class="control-label">@planta.MarginSumatorio</label></td>
                        @*<td align="right" data-tipoproyecto="@cabecera.IdTipoProyecto" class="shrink-margin"><label Class="control-label">@planta.MarginRealSumatorio</label></td>*@
                        <td class="shrink-margin margin-aux"></td>
                        @*<td align="right"><label Class="control-label">@planta.DatoRealInternoSumatorio.ToString("N0", culturaEsES)</label></td>
                            <td align="right"><label Class="control-label">@planta.DatoRealExternoSumatorio.ToString("N0", culturaEsES)</label></td>*@
                    </tr>
                    @For Each estado In planta.Estados
                        @<tr class="info" style="cursor: pointer;" data-idestado='@estado.Id'>
                            <td></td>
                            <td colspan="3">
                                <span class="glyphicon glyphicon-chevron-up" aria-hidden="true" data-idestado='@estado.Id'></span>
                                <label Class="control-label">@estado.Estado</label>
                            </td>
                            <td></td>
                            <td data-tipoproyecto="@cabecera.IdTipoProyecto"></td>
                            <td align="right" data-tipoproyecto="@cabecera.IdTipoProyecto"><label Class="control-label">@estado.OBCSumatorio.ToString("N0", culturaEsES)</label></td>
                            <td align="right" data-tipoproyecto="@cabecera.IdTipoProyecto"><label Class="control-label">@estado.PBCSumatorio.ToString("N0", culturaEsES)</label></td>
                            <td align="right" data-tipoproyecto="@cabecera.IdTipoProyecto"><label Class="control-label">@estado.TCSumatorio.ToString("N0", culturaEsES)</label></td>
                            <td align="right"><label Class="control-label">@estado.BACGastosSumatorio.ToString("N0", culturaEsES)</label></td>

                            @For año As Integer = cabecera.Años.AnyoInicio To cabecera.Años.AnyoFin Step 1
                                @<td class="anyoAux" data-anyo="@año"></td>
                                @For cont As Integer = 1 To 2 Step 1
                                    @For trimestre As Integer = 1 To 4 Step 1
                                        If (cont = 1) Then
                                            @<td align="right" data-anyo="@año" style="background-color: #fadd97;"><label class="control-label">@estado.GastosAñoSumatorio.Where(Function(f) f.Año = año AndAlso f.Trimestre = trimestre).Sum(Function(f) f.Valor).ToString("N0", culturaEsES)</label></td>
                                        ElseIf (cont = 2 AndAlso cabecera.IdTipoProyecto <> ELL.TipoProyecto.TipoProyecto.R_D) Then
                                            @<td align="right" data-anyo="@año" style="background-color: #d8ebd5"><label class="control-label">@estado.IngresosAñoSumatorio.Where(Function(f) f.Año = año AndAlso f.Trimestre = trimestre).Sum(Function(f) f.Valor).ToString("N0", culturaEsES)</label></td>
                                        End If
                                    Next
                                Next
                            Next
                            <td align="right" data-tipoproyecto="@cabecera.IdTipoProyecto" class="shrink-margin"><label Class="control-label">@estado.MarginSumatorio</label></td>
                            @*<td align="right" data-tipoproyecto="@cabecera.IdTipoProyecto" class="shrink-margin"><label Class="control-label">@estado.MarginRealSumatorio</label></td>*@
                            <td class="shrink-margin margin-aux"></td>
                            @*<td align="right"><label Class="control-label">@estado.DatoRealInternoSumatorio.ToString("N0", culturaEsES)</label></td>
                                <td align="right"><label Class="control-label">@estado.DatoRealExternoSumatorio.ToString("N0", culturaEsES)</label></td>*@
                        </tr>
                        For Each costGroup In estado.CostGroups
                            @<tr class="active" style="cursor: pointer;" data-idcostgroup='@costGroup.Id'>
                                <td></td>
                                <td></td>
                                <td align="center">@Html.CheckBox("chkCostGroup-" & CStr(costGroup.Id), New With {.class = "checkboxCostGroup"})</td>
                                <td>
                                    @If (costGroup.Steps.Count > 0) Then
                                        @<span Class="glyphicon glyphicon-chevron-up" aria-hidden="true" data-idcostgroup='@costGroup.Id'></span>
                                    End If
                                    <label class="control-label">@costGroup.Descripcion</label>
                                    @code
' Piden que se pueda meter un step manual en cualquier ocasión no sólo para los de oferta
'If (costGroup.IdCostGroupOT <> Integer.MinValue) Then
                                        @<button type="button" class="btn btn-default btn-xs btn-agregarstep" title='@Utils.Traducir("Agregar nuevo paso")' data-idcostgroup="@costGroup.Id">
                                            <span class="glyphicon glyphicon-plus" aria-hidden="true"></span>
                                        </button>
                                        'End If
                                    end code
                                </td>
                                <td></td>
                                <td data-tipoproyecto="@cabecera.IdTipoProyecto"></td>
                                <td align="right" data-tipoproyecto="@cabecera.IdTipoProyecto">
                                    @code
                                        If (costGroup.OBCSumatorio = 0) Then
                                            valor = String.Empty
                                        Else
                                            valor = costGroup.OBCSumatorio.ToString("N0", culturaEsES)
                                        End If

                                        @<label Class="control-label">@valor</label>
                                    End code
                                </td>
                                <td align="right" data-tipoproyecto="@cabecera.IdTipoProyecto">
                                    <label Class="control-label">@costGroup.PBCSumatorio.ToString("N0", culturaEsES)</label>
                                </td>
                                <td align="right" data-tipoproyecto="@cabecera.IdTipoProyecto"><label class="control-label">@costGroup.TCSumatorio.ToString("N0", culturaEsES)</label></td>
                                <td align="right"><label class="control-label">@costGroup.BACGastosSumatorio.ToString("N0", culturaEsES)</label></td>

                                @For año As Integer = cabecera.Años.AnyoInicio To cabecera.Años.AnyoFin Step 1
                                    @<td class="anyoAux" data-anyo="@año"></td>
                                    @For cont As Integer = 1 To 2 Step 1
                                        @For trimestre As Integer = 1 To 4 Step 1
                                            If (cont = 1) Then
                                                @<td align="right" data-anyo="@año" style="background-color: #fadd97;"><label class="control-label">@costGroup.GastosAñoSumatorio.Where(Function(f) f.Año = año AndAlso f.Trimestre = trimestre).Sum(Function(f) f.Valor).ToString("N0", culturaEsES)</label></td>
                                            ElseIf (cont = 2 AndAlso cabecera.IdTipoProyecto <> ELL.TipoProyecto.TipoProyecto.R_D) Then
                                                @<td align="right" data-anyo="@año" style="background-color: #d8ebd5"><label class="control-label">@costGroup.IngresosAñoSumatorio.Where(Function(f) f.Año = año AndAlso f.Trimestre = trimestre).Sum(Function(f) f.Valor).ToString("N0", culturaEsES)</label></td>
                                            End If
                                        Next
                                    Next
                                Next
                                <td align="right" data-tipoproyecto="@cabecera.IdTipoProyecto" class="shrink-margin"><label Class="control-label">@costGroup.MarginSumatorio</label></td>
                                @*<td align="right" data-tipoproyecto="@cabecera.IdTipoProyecto" class="shrink-margin"><label Class="control-label">@costGroup.MarginRealSumatorio</label></td>*@
                                <td class="shrink-margin margin-aux"></td>
                                @*<td align="right"><label Class="control-label">@costGroup.DatoRealInternoSumatorio.ToString("N0", culturaEsES)</label></td>
                                    <td align="right"><label Class="control-label">@costGroup.DatoRealExternoSumatorio.ToString("N0", culturaEsES)</label></td>*@
                            </tr>
                            @For Each paso In costGroup.Steps.Where(Function(f) f.Visible AndAlso Not f.EsInfoGeneral)
                                ' Forzamos la llamada por cada step a la carga de sus datos
                                @code
                                    'paso.CargarValoresStep()
                                    ' Obtenemos la validacion línea última para estados aprobado, abierto o cerrado
                                    Dim validacionLineaUltimoAprobado As ELL.ValidacionLinea = Nothing
                                    Dim valoresStepAnualesAprobados As List(Of ELL.ValidacionAño) = Nothing
                                    If (paso.IdEstadoValidacion <> Integer.MinValue) Then
                                        'paso.CargarValoresStepValidacion()
                                        validacionLineaUltimoAprobado = BLL.ValidacionesLineaBLL.ObtenerUltimoAprobado(paso.Id)

                                        ' Hay que sacar los valores anuales de la última validación aprobada
                                        If (validacionLineaUltimoAprobado IsNot Nothing) Then
                                            valoresStepAnualesAprobados = BLL.ValidacionesAñoBLL.CargarListadoPorValidacionLinea(validacionLineaUltimoAprobado.Id)
                                        End If
                                    End If
                                End code
                                @<tr class="step" data-idstep="@paso.Id" data-enabled='@paso.Cambiable'>
                                    <td></td>
                                    <td align="center">
                                        @code
                                            If (paso.IdEstadoValidacion = ELL.Validacion.Estado.Waiting_for_approval) Then
                                                @<span Class="glyphicon glyphicon-user text-warning" title='@Utils.Traducir("Pendiente de aprobación")'></span>
                                            ElseIf (paso.IdEstadoValidacion = ELL.Validacion.Estado.Approved) Then
                                                If (String.IsNullOrEmpty(paso.CostCarrier)) Then
                                                    @<span Class="glyphicon glyphicon-ok text-success" title='@Utils.Traducir("Aprobado")'></span>
                                                Else
                                                    @<span Class="glyphicon glyphicon-folder-open text-success" title='@Utils.Traducir("Abierto")'></span>
                                                End If
                                            ElseIf (paso.IdEstadoValidacion = ELL.Validacion.Estado.Rejected) Then
                                                @<span Class="glyphicon glyphicon-remove text-danger" title='@String.Format("{0}: {1}", Utils.Traducir("Rechazado"), paso.Comentarios)'></span>
                                            ElseIf (paso.IdEstadoValidacion = ELL.Validacion.Estado.Opened) Then
                                                @<span Class="glyphicon glyphicon-folder-open text-success" title='@Utils.Traducir("Abierto")'></span>
                                            ElseIf (paso.IdEstadoValidacion = ELL.Validacion.Estado.Closed) Then
                                                @<span Class="glyphicon glyphicon-folder-open" title='@Utils.Traducir("Cerrado")'></span>
                                            End If

                                            Dim lineasPedido As List(Of ELL.LineaPedido) = BLL.LineasPedidoBLL.CargarListadoPorPaso(paso.Id)
                                            If (Not String.IsNullOrEmpty(paso.CostCarrier) AndAlso lineasPedido IsNot Nothing AndAlso lineasPedido.Count > 0) Then
                                                Dim estilo As String = "text-success"

                                                If (lineasPedido.Exists(Function(f) f.IdEstadoFacturacion = ELL.Pedido.EstadoFacturacion.Sent_to_invoice)) Then
                                                    estilo = String.Empty
                                                End If

                                                @<span Class="toolTp glyphicon glyphicon glyphicon-list-alt @estilo" title='@Utils.Traducir("Listado facturacion")' style="margin-left:5px; cursor: pointer;" data-idstep="@paso.Id"></span>
                                            End If
                                        End code
                                    </td>
                                    @code
                                        @<td align="center">@Html.CheckBox("chkStep-" & CStr(paso.Id), New With {.class = "checkbox", .data_idstep = paso.Id, .data_stepname = paso.Descripcion, .data_idcostgroup = paso.IdCostGroup, .data_costcarrier = paso.CostCarrier, .data_idplanta = paso.IdPlantaSAB})</td>
                                    end code

                                    <td style="padding-left:20px;">
                                        @code
                                            Dim descripcionDelPaso As String = paso.Descripcion

                                            If (Not String.IsNullOrEmpty(paso.CostCarrier)) Then
                                                descripcionDelPaso &= " (" & paso.CostCarrier & ")"
                                            End If

                                            'Dejamos cambiar el nombre de los step cuyo cost group no sea ni de bonos ni de OT o
                                            'que aunque tenga oferta el costgroup el step no venga de oferta (los creados manualmente) o
                                            'los que sean de origen manual
                                            If ((costGroup.IdBonos = Integer.MinValue AndAlso costGroup.IdCostGroupOT = Integer.MinValue) OrElse (paso.Origen = ELL.Step.OrigenStep.DeSolicitud) OrElse (paso.OBCOrigenDatos = ELL.Step.OrigenStep.DeSolicitud)) Then
                                                @<label Class="control-label step-description" data-idstep="@paso.Id" data-descripcion="@paso.Descripcion" style="cursor: pointer; color:#0094ff;">@descripcionDelPaso</label>
                                                @<input Class="step-description-input" data-idstep="@paso.Id" data-portador="@paso.CostCarrier" style="display:none;width:100%;">
                                            Else
                                                @<label Class="control-label">@descripcionDelPaso</label>
                                            End If

                                            '- Si el costgroup del que viene el step viene de bonos y de oferta y el step viene de plantilla
                                            '- Si el costgroup de oferta del que viene el costgroup permite el cambio de porcentaje
                                            If (costGroup.IdBonos <> Integer.MinValue AndAlso costGroup.IdCostGroupOT <> Integer.MinValue AndAlso paso.Origen = ELL.Step.OrigenStep.Plantilla AndAlso paso.OBCOrigenDatos = ELL.OrigenDatosStep.OrigenDatosStep.Presupuesto_bonos) Then
                                                If (paso.CambioPorcentaje) Then
                                                    @Html.TextBox(String.Format("txtPorcentaje-{0}", paso.Id), If(paso.Porcentaje <> Integer.MinValue, paso.Porcentaje, 1), New With {.class = "numeric input-text porcentaje", .data_idagrupacion = paso.IdAgrupacion, .data_idstep = paso.Id, .data_idcostgroup = costGroup.Id, .data_idplanta = paso.IdPlanta, .data_idestado = paso.IdEstado})
                                                    @<label Class="control-label">%</label>
                                                End If
                                            End If

                                            'Si el step está marcado como manual dejamos borrarlo y si además no tiene estado de validación o si ha sido rechazado
                                            If ((paso.IdEstadoValidacion = Integer.MinValue OrElse paso.IdEstadoValidacion = ELL.Validacion.Estado.Rejected) AndAlso (paso.Origen = ELL.Step.OrigenStep.DeSolicitud OrElse paso.OBCOrigenDatos = ELL.OrigenDatosStep.OrigenDatosStep.Manual) AndAlso String.IsNullOrEmpty(paso.CostCarrier)) Then
                                                @<button type="button" Class="btn btn-default btn-xs btn-eliminarstep" title='@Utils.Traducir("Eliminar paso")' data-idstep="@paso.Id" data-idcostgroup="@paso.IdCostGroup">
                                                    <span class="glyphicon glyphicon-trash" aria-hidden="true"></span>
                                                </button>
                                            End If
                                        End code
                                    </td>
                                    <td align="center"><label class="control-label">@[Enum].GetName(GetType(ELL.StepPlantilla.DatoRealOrigen), paso.OrigenDatoReal).Replace("_", "/")</label></td>
                                    @*<td align="right">
                                            @code
                                                If (paso.Code = Integer.MinValue) Then
                                                    valor = String.Empty
                                                Else
                                                    valor = paso.Code
                                                End If
                                            End code

                                            <label class="control-label">@valor</label>
                                        </td>*@
                                    <td data-tipoproyecto="@cabecera.IdTipoProyecto">
                                        @code
                                            If (paso.CambiarPlanta AndAlso Not String.IsNullOrEmpty(paso.IdOferta) AndAlso paso.Cambiable) Then
                                                @<a href="#" class="cambio-planta" data-idstep="@paso.Id" data-idplanta="@paso.IdPlanta" data-idcostgroupot="@paso.IdCostGroupOT">@paso.Planta</a>
                                            Else
                                                @<label class="control-label">@paso.Planta</label>
                                            End If
                                        End code
                                    </td>
                                    <td align="right" data-tipoproyecto="@cabecera.IdTipoProyecto">
                                        @*Columna offer budget*@
                                        @code
                                            If (paso.ValoresStep.Exists(Function(f) f.IdColumna = ELL.Columna.Tipo.Offer_budget_cost)) Then
                                                valor = paso.ValoresStep.First(Function(f) f.IdColumna = ELL.Columna.Tipo.Offer_budget_cost).Valor.ToString("N0", culturaEsES)
                                            Else
                                                valor = String.Empty
                                            End If

                                            If (validacionLineaUltimoAprobado IsNot Nothing) Then
                                                If (paso.ValoresStep.Exists(Function(f) f.IdColumna = ELL.Columna.Tipo.Offer_budget_cost) AndAlso validacionLineaUltimoAprobado.OfferBudget <> paso.ValoresStep.First(Function(f) f.IdColumna = ELL.Columna.Tipo.Offer_budget_cost).Valor) Then
                                                    valorPopup = validacionLineaUltimoAprobado.OfferBudget.ToString("N0", culturaEsES)
                                                    @<span Class="glyphicon glyphicon-eye-open text-danger" aria-hidden="true" style="cursor:pointer;position: static;" data-content='@Utils.Traducir("Validado"): @valorPopup'></span>
                                                End If
                                            End If

                                            @If (paso.OBCOrigenDatos = ELL.OrigenDatosStep.OrigenDatosStep.Presupuesto_bonos OrElse paso.OBCOrigenDatos = ELL.OrigenDatosStep.OrigenDatosStep.Presupuesto_viajes OrElse (costGroup.IdCostGroupOT <> Integer.MinValue AndAlso paso.OBCOrigenDatos <> ELL.OrigenDatosStep.OrigenDatosStep.Manual)) Then
                                                @<label class="control-label">@valor</label>
                                            ElseIf (paso.OBCOrigenDatos = ELL.OrigenDatosStep.OrigenDatosStep.Manual) Then
                                                @Html.TextBox(String.Format("p-{0}-{1}-0-0", paso.Id, CInt(ELL.Columna.Tipo.Offer_budget_cost)), valor, New With {.class = "numeric input-text"})
                                            End If

                                        End code
                                    </td>
                                    <td align="right" data-tipoproyecto="@cabecera.IdTipoProyecto">
                                        <div class="contenido-celda">
                                            @*Columna paid by customer*@
                                            @code
                                                ' Vamos a tratar los valores
                                                valor = paso.PBC.ToString("N0", culturaEsES)

                                                ' Si dentro del grupo de steps hay uno invisible el resto de steps quieren que su PBC se muestre en blanco y no a 0
                                                If (costGroup.Steps.Exists(Function(f) Not f.Visible) AndAlso paso.PBC = 0) Then
                                                    valor = String.Empty
                                                End If

                                                If (Not String.IsNullOrEmpty(valor)) Then
                                                    If (validacionLineaUltimoAprobado IsNot Nothing) Then
                                                        valorPopup = validacionLineaUltimoAprobado.PaidByCustomer.ToString("N0", culturaEsES)
                                                        If (valor <> valorPopup) Then
                                                            @<span Class="glyphicon glyphicon-eye-open text-danger" aria-hidden="true" style="cursor:pointer;position: static;" data-content='@Utils.Traducir("Validado"): @valorPopup'></span>
                                                        End If
                                                    End If
                                                End If

                                                @<div>
                                                    @If (paso.IdCostGroupOT <> Integer.MinValue AndAlso paso.Origen = ELL.Step.OrigenStep.Oferta) Then
                                                        @<label Class="control-label" data-pbctotal='paso.Id' data-stepname='@paso.Descripcion'>@valor</label>
                                                    Else
                                                        @<label class="control-label">@valor</label>
                                                    End If
                                                </div>
                                            End code
                                        </div>
                                    </td>
                                    <td align="right" data-tipoproyecto="@cabecera.IdTipoProyecto">
                                        <div class="contenido-celda">
                                            @*Columna target*@
                                            @code
                                                ' Vamos a tratar los valores
                                                valor = paso.TC.ToString("N0", culturaEsES)

                                                If (validacionLineaUltimoAprobado IsNot Nothing) Then
                                                    ' Podemos tenemos un step que su target en interface sea una caja de texto
                                                    If (valoresStepAnualesAprobados IsNot Nothing AndAlso valoresStepAnualesAprobados.Exists(Function(f) f.IdColumna = ELL.Columna.Tipo.Target_cost)) Then
                                                        valorPopup = valoresStepAnualesAprobados.FirstOrDefault(Function(f) f.IdColumna = ELL.Columna.Tipo.Target_cost).Valor.ToString("N0", culturaEsES)
                                                    Else
                                                        valorPopup = paso.EvaluarFormulaTarget(validacionLineaUltimoAprobado.OfferBudget, validacionLineaUltimoAprobado.PaidByCustomer).ToString("N0", culturaEsES)
                                                    End If

                                                    If (valor <> valorPopup) Then
                                                        @<span Class="glyphicon glyphicon-eye-open text-danger" aria-hidden="true" style="cursor:pointer;position: static;" data-content='@Utils.Traducir("Validado"): @valorPopup'></span>
                                                    End If
                                                End If

                                                If (paso.Origen = ELL.Step.OrigenStep.DeSolicitud) Then
                                                    If (paso.IdCostGroupOT <> Integer.MinValue) Then
                                                        @<div>
                                                            <Label Class="control-label">@valor</Label>
                                                        </div>
                                                    Else
                                                        @<div>
                                                            @Html.TextBox(String.Format("p-{0}-{1}-0-0", paso.Id, CInt(ELL.Columna.Tipo.Target_cost)), valor, New With {.class = "numeric input-text"})
                                                        </div>
                                                    End If
                                                Else
                                                    @<div>
                                                        <Label Class="control-label">@valor</Label>
                                                    </div>
                                                End If
                                            End code
                                        </div>
                                    </td>
                                    @code
                                        @*Columna budget approved*@
                                    Dim estiloBA = String.Empty
                                    ' Los colores sólo los quieren para los proyectos de industrializacion
                                    If (cabecera.IdTipoProyecto = ELL.TipoProyecto.TipoProyecto.Industrialization) Then
                                        If (paso.BACGastos > paso.TC) Then
                                            estiloBA = "danger"
                                        Else
                                            estiloBA = "success"
                                        End If
                                    End If

                                    ' Vamos a tratar los valores
                                    valor = paso.BACGastos.ToString("N0", culturaEsES)
                                        @<td align="right" class="@estiloBA">
                                            <div class="contenido-celda">
                                                @If (validacionLineaUltimoAprobado IsNot Nothing) Then
                                                    valorPopup = validacionLineaUltimoAprobado.BudgetApproved.ToString("N0", culturaEsES)
                                                    If (valor <> valorPopup) Then
                                                        @<span Class="glyphicon glyphicon-eye-open text-danger" aria-hidden="true" style="cursor:pointer;position: static;" data-content='@Utils.Traducir("Validado"): @valorPopup'></span>
                                                    End If
                                                End If
                                                <div>
                                                    @If (paso.IdCostGroupOT <> Integer.MinValue AndAlso paso.Origen = ELL.Step.OrigenStep.Oferta) Then
                                                        @<label Class="control-label" data-bactotal='@paso.Id' data-stepname='@paso.Descripcion'>@valor</label>
                                                    Else
                                                        @<label class="control-label">@valor</label>
                                                    End If
                                                </div>
                                            </div>
                                        </td>
                                    end code

                                    @For año As Integer = cabecera.Años.AnyoInicio To cabecera.Años.AnyoFin Step 1
                                        @<td class="anyoAux" data-anyo="@año"></td>
                                        @For cont As Integer = 1 To 2 Step 1
                                            Dim tipoColumna As ELL.Columna.Tipo
                                            Dim colorCelda As String = String.Empty
                                            If (cont = 1) Then 'Gastos
                                                tipoColumna = ELL.Columna.Tipo.Year_expenses
                                                colorCelda = "#fadd97"
                                            ElseIf (cont = 2) Then 'Ingresos
                                                tipoColumna = ELL.Columna.Tipo.Year_incomes
                                                colorCelda = "#d8ebd5"
                                            End If
                                            @For trimestre As Integer = 1 To 4 Step 1
                                                If (cont = 1 OrElse (cont = 2 AndAlso cabecera.IdTipoProyecto <> ELL.TipoProyecto.TipoProyecto.R_D)) Then
                                                    @<td align="right" data-anyo="@año" style="background-color:@colorCelda;">
                                                        <div class="contenido-celda">
                                                            @code
                                                                ' Vamos a tratar los valores
                                                                If (paso.ValoresStep.Exists(Function(f) f.IdColumna = tipoColumna AndAlso f.Año = año AndAlso f.Trimestre = trimestre)) Then
                                                                    valor = paso.ValoresStep.First(Function(f) f.IdColumna = tipoColumna AndAlso f.Año = año AndAlso f.Trimestre = trimestre).Valor.ToString("N0", culturaEsES)

                                                                    If (valoresStepAnualesAprobados IsNot Nothing AndAlso valoresStepAnualesAprobados.Exists(Function(f) f.IdColumna = tipoColumna AndAlso f.Año = año AndAlso f.Trimestre = trimestre)) Then
                                                                        valorPopup = valoresStepAnualesAprobados.First(Function(f) f.IdColumna = tipoColumna AndAlso f.Año = año AndAlso f.Trimestre = trimestre).Valor.ToString("N0", culturaEsES)
                                                                    Else
                                                                        valorPopup = "0"
                                                                    End If
                                                                Else
                                                                    valor = String.Empty
                                                                    valorPopup = String.Empty
                                                                End If

                                                                If (valor <> valorPopup AndAlso valoresStepAnualesAprobados IsNot Nothing) Then
                                                                    @<span Class="glyphicon glyphicon-eye-open text-danger" aria-hidden="true" style="cursor:pointer;position: static;" data-content='@Utils.Traducir("Validado"): @valorPopup'></span>
                                                                End If

                                                                @<div>
                                                                    @if (tipoColumna = ELL.Columna.Tipo.Year_expenses) Then
                                                                        @If (paso.GastosAñoOrigenDatos = ELL.OrigenDatosStep.OrigenDatosStep.Planificacion) Then
                                                                            @<label Class="control-label">@valor</label>
                                                                        ElseIf (costGroup.IdCostGroupOT <> Integer.MinValue AndAlso paso.Origen = ELL.Step.OrigenStep.Oferta) Then
                                                                            ' Con esto identificamos los valores de ingresos de un step cuyo cost group es de OT
                                                                            @Html.TextBox(String.Format("p-{0}-{1}-{2}-{3}", paso.Id, CInt(tipoColumna), año, trimestre), valor, New With {.class = "numeric input-text", .data_bacvalor = paso.Id})
                                                                        ElseIf (paso.GastosAñoOrigenDatos = ELL.OrigenDatosStep.OrigenDatosStep.Manual OrElse paso.Origen = ELL.Step.OrigenStep.DeSolicitud) Then
                                                                            @Html.TextBox(String.Format("p-{0}-{1}-{2}-{3}", paso.Id, CInt(tipoColumna), año, trimestre), valor, New With {.class = "numeric input-text"})
                                                                        End If
                                                                    ElseIf (tipoColumna = ELL.Columna.Tipo.Year_incomes) Then
                                                                        @If (paso.IngresosAñoOrigenDatos = ELL.OrigenDatosStep.OrigenDatosStep.Planificacion) Then
                                                                            @<label Class="control-label">@valor</label>
                                                                        ElseIf (costGroup.IdCostGroupOT <> Integer.MinValue AndAlso paso.Origen = ELL.Step.OrigenStep.Oferta) Then
                                                                            @Html.TextBox(String.Format("p-{0}-{1}-{2}-{3}", paso.Id, CInt(tipoColumna), año, trimestre), valor, New With {.class = "numeric input-text", .data_pbcvalor = paso.Id})
                                                                        ElseIf (paso.IngresosAñoOrigenDatos = ELL.OrigenDatosStep.OrigenDatosStep.Manual OrElse paso.Origen = ELL.Step.OrigenStep.DeSolicitud) Then
                                                                            @Html.TextBox(String.Format("p-{0}-{1}-{2}-{3}", paso.Id, CInt(tipoColumna), año, trimestre), valor, New With {.class = "numeric input-text"})
                                                                        End If
                                                                    End If
                                                                </div>
                                                            End code
                                                        </div>
                                                    </td>
                                                End if
                                            Next
                                        Next
                                    Next
                                    <td align="right" data-tipoproyecto="@cabecera.IdTipoProyecto" class="shrink-margin"><label Class="control-label">@paso.Margin</label></td>
                                    @*<td align="right" data-tipoproyecto="@cabecera.IdTipoProyecto" class="shrink-margin"><label Class="control-label">@paso.MarginReal</label></td>*@
                                    <td class="shrink-margin margin-aux"></td>
                                    @*<td align="right"><label Class="control-label">@paso.DatoRealInterno.ToString("N0", culturaEsES)</label></td>
                                        <td align="right"><label Class="control-label">@paso.DatoRealExterno.ToString("N0", culturaEsES)</label></td>*@
                                </tr>
                            Next
                        Next
                    Next
                Next
            </tbody>
        </table>
    End Using
End Code

<div class="modal fade" id="modalWindowCambioPlantaStep" tabindex="-1" role="dialog" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">@Utils.Traducir("Cambiar planta paso")</h4>
            </div>
            <div class="modal-body">
                @Using Html.BeginForm("CambiarPlantaStep", "CostCarriers", FormMethod.Post, New With {.class = "form-horizontal"})
                    @Html.Hidden("hIdCabecera", cabecera.Id)
                    @Html.Hidden("hIdStep")
                    @<div Class="form-group">
                        <label class="col-sm-2 control-label">@Utils.Traducir("Planta\Estado")</label>
                        <div class="col-sm-10">
                            <select id="selectPlantaEstados" name="selectPlantaEstados" class="form-control" required="required"></select>
                        </div>
                    </div>
                    @<div Class="form-group">
                        <div class="col-sm-offset-2 col-sm-10">
                            <input type="submit" id="btnConfirmCambiarPlantaStep" value="@Utils.Traducir("Cambiar")" class="btn btn-primary input-block-level form-control" />
                        </div>
                    </div>
                End Using
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-dismiss="modal">@Utils.Traducir("Cerrar")</button>
            </div>
        </div>
    </div>
</div>

<div class="modal fade" id="modalWindowAgregarStep" tabindex="-1" role="dialog" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">@Utils.Traducir("Agregar nuevo paso")</h4>
            </div>
            <div class="modal-body">
                @Using Html.BeginForm("AgregarStep", "CostCarriers", FormMethod.Post, New With {.class = "form-horizontal", .id = "AgregarStepForm"})
                    @Html.Hidden("hIdCabecera", cabecera.Id)
                    @Html.Hidden("hCostGroupParaStep")
                    @<div id="grupoSteps">
                        <div Class="form-group">
                            <label class="col-sm-3 control-label">@Utils.Traducir("Descripción")</label>
                            <div class="col-sm-7">
                                @Html.TextBox("txtAgregarDescripcionStep-0", Nothing, New With {.class = "form-control", .required = "required", .maxlength = "100", .title = Utils.Traducir("Descripción") & " " & Utils.Traducir("campo obligatorio")})
                            </div>
                            <div class="col-sm-1">
                                <button type="button" id="btnAñadrNuevoPaso" class="btn btn-info" title="@Utils.Traducir("Añadir nuevo paso")">
                                    <span class="glyphicon glyphicon-plus"></span>
                                </button>
                            </div>
                        </div>
                    </div>
                    @<div Class="form-group">
                        <div class="col-sm-offset-3 col-sm-7">
                            <input type="submit" id="btnConfirmAgregarStep" value="@Utils.Traducir("Agregar")" class="btn btn-primary input-block-level form-control" />
                        </div>
                    </div>
                End Using
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-dismiss="modal">@Utils.Traducir("Cerrar")</button>
            </div>
        </div>
    </div>
</div>

@*<div class="modal fade" id="modalWindowCambiarPorcentaje" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">@Utils.Traducir("Agregar nuevo paso")</h4>
                </div>
                <div class="modal-body">
                    @Using Html.BeginForm("AgregarStep", "CostCarriers", FormMethod.Post, New With {.class = "form-horizontal", .id = "AgregarStepForm"})
                        @Html.Hidden("hIdCabecera", cabecera.Id)
                        @Html.Hidden("hCostGroupParaStep")
                        @<div Class="form-group">
                            <label class="col-sm-4 control-label">@Utils.Traducir("Descripción")</label>
                            <div class="col-sm-8">
                                @Html.TextBox("txtAgregarDescripcionStep", Nothing, New With {.class = "form-control", .required = "required", .maxlength = "100", .title = Utils.Traducir("Descripción") & " " & Utils.Traducir("campo obligatorio")})
                            </div>
                        </div>
                        @<div Class="form-group">
                            <div class="col-sm-offset-4 col-sm-8">
                                <input type="submit" id="btnConfirmAgregarStep" value="@Utils.Traducir("Agregar")" class="btn btn-primary input-block-level form-control" />
                            </div>
                        </div>
                    End Using
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">@Utils.Traducir("Cerrar")</button>
                </div>
            </div>
        </div>
    </div>*@

<div class="modal fade" id="modalWindowCambiarAbreviatura" tabindex="-1" role="dialog" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">@Utils.Traducir("Cambiar datos cabecera")</h4>
            </div>
            <div class="modal-body">
                <form Class="form-horizontal" id="formCambiarAbreviatura">
                    @Html.Hidden("hIdCabecera", cabecera.Id)
                    <div Class="form-group">
                        <label class="col-sm-4 control-label">@Utils.Traducir("Abreviatura")</label>
                        <div class="col-sm-8">
                            @Html.TextBox("txtAbreviatura", String.Empty, New With {.class = "form-control", .required = "required", .maxlength = "10", .style = "text-transform:uppercase;"})
                        </div>
                    </div>
                    @*<div Class="form-group">
                        <label id="lblSOP" class="col-sm-4 control-label">@Utils.Traducir("SOP")</label>
                        <div class="col-sm-8">
                            <div id="fechaSOP" class="input-group date">
                                @Html.TextBox("SOP", String.Empty, New With {.class = "form-control", .required = "required"})
                                <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                            </div>
                        </div>
                    </div>
                    <div Class="form-group">
                        <label id="lblAnyosSerie" class="col-sm-4 control-label">@Utils.Traducir("Años serie")</label>
                        <div class="col-sm-8">
                            @Html.TextBox("anyosSerie", String.Empty, New With {.class = "form-control", .required = "required"})
                        </div>
                    </div>*@
                    <div Class="form-group">
                        <div class="col-sm-offset-4 col-sm-8">
                            <input type="submit" id="btnConfirmCambiarAbreviatura" value="@Utils.Traducir("Guardar")" class="btn btn-primary input-block-level form-control" />
                        </div>
                    </div>
                </form>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-dismiss="modal">@Utils.Traducir("Cerrar")</button>
            </div>
        </div>
    </div>
</div>

@*<div class="modal fade" id="modalWindowCambiarSOP" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">@Utils.Traducir("Cambiar SOP y años serie")</h4>
                </div>
                <div class="modal-body">
                    @Using Html.BeginForm("CambiarSOP", "CostCarriers", FormMethod.Post, New With {.class = "form-horizontal"})
                        @Html.Hidden("hIdCabecera", cabecera.Id)
                        @Html.Hidden("hIdPlanta")
                        @<div Class="form-group">
                            <label class="col-sm-4 control-label">@Utils.Traducir("SOP")</label>
                            <div class="col-sm-8">
                                <div id="fechaSOP" class="input-group date">
                                    @Html.TextBox("SOP", String.Empty, New With {.class = "form-control", .required = "required"})
                                    <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                                </div>
                            </div>
                        </div>
                        @<div Class="form-group">
                            <label class="col-sm-4 control-label">@Utils.Traducir("Años serie")</label>
                            <div class="col-sm-8">
                                @Html.TextBox("anyosSerie", String.Empty, New With {.class = "form-control numeric input-text", .required = "required"})
                            </div>
                        </div>
                        @<div Class="form-group">
                            <div class="col-sm-offset-4 col-sm-8">
                                <input type="submit" id="btnConfirmCambiarSOP" value="@Utils.Traducir("Guardar")" class="btn btn-primary input-block-level form-control" />
                            </div>
                        </div>
                    End Using
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">@Utils.Traducir("Cerrar")</button>
                </div>
            </div>
        </div>
    </div>*@