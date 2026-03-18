@Imports CostCarriersLib

@Code
    Dim tiposProyecto As List(Of ELL.TipoProyecto) = CType(ViewData("TiposProyecto"), List(Of ELL.TipoProyecto))
    Dim idTipoProyecto As Integer? = ViewData("idTipoProyecto")
End Code

<h3><label>@Utils.Traducir("Plantillas")</label></h3>
<hr />

<script src="~/Scripts/bootstrap-treeview.js"></script>
<script src="~/Scripts/BootstrapMenu.min.js"></script>
<script src="~/Scripts/jquery.alphanum.js"></script>

<script type="text/javascript">

    var menu = new BootstrapMenu('[data-kind="planta"]', {
        fetchElementData: function($element) {
            var id = $element.data('id');
            $("#hNodeIdActual").val($element.data('nodeid'));
            return id;
        },
        actions: [
            {
                name: '@Utils.Traducir("Agregar nuevo estado")',
                iconClass: 'glyphicon glyphicon-plus',
                onClick: function(id) {
                    $("#hPlantaPlantilla").val(id);
                    var idTipoProyecto = $("input[name='tipoProyecto']:checked").val();
                    CargarModalEstados(idTipoProyecto);
                }
            },
            {
                name: '@Utils.Traducir("Eliminar planta")',
                iconClass: 'glyphicon glyphicon-trash',
                onClick: function(id) {
                    var bRet = confirm('@Html.Raw(Utils.Traducir("¿Desea eliminar la planta seleccionada?"))');
                    if (bRet == true) {
                        EliminarPlanta(id);
                    }
                }
            }
        ]
    });

    var menu = new BootstrapMenu('[data-kind="estado"]', {
        fetchElementData: function($element) {
            var id = $element.data('id');
            $("#hNodeIdActual").val($element.data('nodeid'));
            return id;
        },
        actions: [
            {
                name: '@Utils.Traducir("Agregar nuevo grupo de coste")',
                iconClass: 'glyphicon glyphicon-plus',
                onClick: function(id) {
                    $("#hEstadoPlantilla").val(id);
                    $("#txtAgregarDescripcionCostGroup").val('');
                    // Con esto sacamos el padre que es la planta. Necesario para obtener los cost groups de bonos
                    var nodoPadre = $('#tree').treeview('getParent', parseInt($("#hNodeIdActual").val()));
                    CargarModalAgregarCostGroup(nodoPadre.dataAttributes.idPlanta);
                }
            },
            {
                name: '@Utils.Traducir("Eliminar estado")',
                iconClass: 'glyphicon glyphicon-trash',
                onClick: function(id) {
                    var bRet = confirm('@Html.Raw(Utils.Traducir("¿Desea eliminar el estado seleccionado?"))');
                    if (bRet == true) {
                        EliminarEstado(id);
                    }
                }
            }
        ]
    });

    var menu = new BootstrapMenu('[data-kind="costGroup"]', {
        fetchElementData: function($element) {
            var id = $element.data('id');
            $("#hNodeIdActual").val($element.data('nodeid'));
            return id;
        },
        actions: [
            {
                name: '@Utils.Traducir("Agregar nuevo paso")',
                iconClass: 'glyphicon glyphicon-plus',
                onClick: function(id) {
                    $("#hCostGroupPlantillaParaStep").val(id);
                    $("#txtAgregarDescripcionStep").val('');
                    $("#hFormulaAgregarTC, #hFormulaAgregarTCCustomer").val('');
                    $("#txtAgregarOperandoTC, #txtAgregarOperandoTCCustomer").val('');
                    $("#txtFormulaAgregarTC, #txtFormulaAgregarTCCustomer").val('');
                    var nodoActual = $('#tree').treeview('getNode', parseInt($("#hNodeIdActual").val()));
                    CargarModalAgregarStep(nodoActual.dataAttributes.id);
                }
            },
            {
                name: '@Utils.Traducir("Editar grupo de coste")',
                iconClass: 'glyphicon glyphicon-pencil',
                onClick: function(id) {
                    $("#hCostGroupPlantilla").val(id);
                    $("#txtEditarDescripcionCostGroup").val('');
                    // Con esto sacamos el padre del padre que es la planta. Necesario para obtener los cost groups de bonos
                    var nodoPadre = $('#tree').treeview('getParent', parseInt($("#hNodeIdActual").val()));
                    var nodoPadre = $('#tree').treeview('getParent', nodoPadre.nodeId);
                    CargarModalEditarCostGroup(id, nodoPadre.dataAttributes.idPlanta);
                }
            },
            {
                name: '@Utils.Traducir("Eliminar grupo de coste")',
                iconClass: 'glyphicon glyphicon-trash',
                onClick: function(id) {
                    var bRet = confirm('@Html.Raw(Utils.Traducir("¿Desea eliminar el grupo de coste seleccionado?"))');
                    if (bRet == true) {
                        EliminarCostGroup(id);
                    }
                }
            }
        ]
    });

    var menu = new BootstrapMenu('[data-kind="step"]', {
        fetchElementData: function($element) {
            var id = $element.data('id');
            $("#hNodeIdActual").val($element.data('nodeid'));
            return id;
        },
        actions: [
            {
                name: '@Utils.Traducir("Editar paso")',
                iconClass: 'glyphicon glyphicon-pencil',
                onClick: function(id) {
                    $("#hStepPlantilla").val(id);
                    var nodoPadre = $('#tree').treeview('getParent', parseInt($("#hNodeIdActual").val()));
                    CargarModalEditarStep(id, nodoPadre.dataAttributes.id);
                }
            },
            {
                name: '@Utils.Traducir("Eliminar paso")',
                iconClass: 'glyphicon glyphicon-trash',
                onClick: function(id) {
                    var bRet = confirm('@Html.Raw(Utils.Traducir("¿Desea eliminar el paso seleccionado?"))');
                    if (bRet == true) {
                        EliminarStep(id);
                    }
                }
            }
        ]
    });

    function CargarModalPlantas(idTipoProyecto){
        $.ajax({
            url: '@Url.Action("CargarPlantas", "Plantilla")',
            data: { idTipoProyecto: idTipoProyecto },
            type: 'GET',
            dataType: 'json',
            success: function (d) {
                if(d.length > 0){
                    $('#selectPlantas').empty();
                    $.each(d, function (i, planta) {
                        $('#selectPlantas').append($('<option>', {
                            value: planta.Id,
                            text : planta.Nombre
                        }));
                    });
                    $('#modalWindowPlantas').modal('show');
                }
                else{
                    // No hay nodos o porque no existe la plantilla o porque no existen plantas asociadas así que tenemos que dar la opción de añadir
                    $("#arbol").hide();
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    }

    function CargarModalEstados(idTipoProyecto) {        
        $.ajax({
            url: '@Url.Action("CargarEstados", "Plantilla")',
            data: { idTipoProyecto: idTipoProyecto },
            type: 'GET',
            dataType: 'json',
            success: function (d) {
                if(d.length > 0){
                    $('#selectEstados').empty();
                    $.each(d, function (i, planta) {
                        $('#selectEstados').append($('<option>', {
                            value: planta.Id,
                            text : planta.Descripcion
                        }));
                    });
                    $('#modalWindowEstados').modal('show');
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    }

    function CargarModalAgregarCostGroup(idPlanta){
        $.ajax({
            url: '@Url.Action("CargarCostGroupBonos", "Plantilla")',
            data: { idPlanta: idPlanta },
            type: 'GET',
            dataType: 'json',
            success: function (d) {
                $("input[name='origenCostGroupAgregar'][value='0']").prop("checked", true);
                $("input[name='origenCostGroupAgregar'][value='1']").prop("checked", false);
                $('#txtAgregarDescripcionCostGroup').prop('required', 'required');
                $('#txtAgregarDescripcionCostGroup').prop('disabled', false);

                if(d.length > 0){
                    $('#selectAgregarCostGroup').prop('disabled', 'disabled');
                    $('#selectAgregarCostGroup').empty();
                    $.each(d, function (i, origenDato) {
                        $('#selectAgregarCostGroup').append($('<option>', {
                            value: origenDato.Id,
                            text : origenDato.Nombre
                        }));
                    });
                }

                $.ajax({
                    url: '@Url.Action("CargarCostGroupOT", "Plantilla")',
                    type: 'GET',
                    dataType: 'json',
                    success: function (d) {
                        if(d.length > 0){
                            $('#selectAgregarCostGroupOT').empty();
                            $.each(d, function (i, origenDato) {
                                $('#selectAgregarCostGroupOT').append($('<option>', {
                                    value: origenDato.Id,
                                    text : origenDato.Nombre
                                }));
                            });
                        }
                        $('#modalWindowAgregarCostGroup').modal('show');
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    }

    function CargarModalEditarCostGroup(idCostGroup, idPlanta){
        $.ajax({
            url: '@Url.Action("CargarCostGroupBonos", "Plantilla")',
            data: { idPlanta: idPlanta },
            type: 'GET',
            dataType: 'json',
            success: function (d) {
                if(d.length > 0){
                    $('#selectEditarCostGroup').empty();
                    $.each(d, function (i, origenDato) {
                        $('#selectEditarCostGroup').append($('<option>', {
                            value: origenDato.Id,
                            text : origenDato.Nombre
                        }));
                    });
                }

                $.ajax({
                    url: '@Url.Action("CargarCostGroupOT", "Plantilla")',
                    type: 'GET',
                    dataType: 'json',
                    success: function (d) {
                        if(d.length > 0){
                            $('#selectEditarCostGroupOT').empty();
                            $.each(d, function (i, origenDato) {
                                $('#selectEditarCostGroupOT').append($('<option>', {
                                    value: origenDato.Id,
                                    text : origenDato.Nombre
                                }));
                            });
                        }

                        $.ajax({
                            url: '@Url.Action("CargarCostGroup", "Plantilla")',
                            data: { idCostGroup: idCostGroup },
                            type: 'GET',
                            dataType: 'json',
                            success: function (d) {
                                if (d.IdCostGroupOT != @Integer.MinValue){
                                    $('#selectEditarCostGroupOT').val(d.IdCostGroupOT);
                                }

                                if (d.IdBonos == @Integer.MinValue){
                                    $("#txtEditarDescripcionCostGroup").val(d.DescripcionGuardada);
                                    if (d.IdCostGroupOT == @Integer.MinValue){
                                        $('#txtEditarDescripcionCostGroup').prop('required', 'required');
                                        $("input[name='origenCostGroupEditar'][value='0']").prop("checked", true)
                                    }
                                    else
                                    {
                                        $('#txtEditarDescripcionCostGroup').prop('required', false);
                                        $("input[name='origenCostGroupEditar'][value='1']").prop("checked", true)
                                    }

                                    $('#txtEditarDescripcionCostGroup').prop('disabled', false);
                                    $('#selectEditarCostGroup').prop('disabled', 'disabled');
                                    //$("input[name='origenCostGroupEditar'][value='0']").prop("checked",true)
                                }
                                else{
                                    $('#selectEditarCostGroup').val(d.IdBonos);
                                    $('#txtEditarDescripcionCostGroup').prop('required', false);
                                    $('#txtEditarDescripcionCostGroup').prop('disabled', 'disabled');
                                    $('#selectEditarCostGroup').prop('disabled', false);
                                    $("input[name='origenCostGroupEditar'][value='1']").prop("checked",true)
                                }

                                $('#modalWindowEditarCostGroup').modal('show');
                            },
                            error: function (xhr, ajaxOptions, thrownError) {
                                alert(xhr.status);
                                alert(thrownError);
                            }
                        });
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    }

    function CargarModalEditarStep(idStep, idCostGroup) {        
        $.ajax({
            url: '@Url.Action("CargarOrigenesDatos", "Plantilla")',
            data: { idCostGroup: idCostGroup },
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (d) {
                if (d.length > 0) {
                    $('#selectEditarOrigenDatosOBC').empty();
                    $.each(d, function (i, origenDato) {
                        if (origenDato.Id != @CInt(ELL.OrigenDatosStep.OrigenDatosStep.Planificacion)){
                            $('#selectEditarOrigenDatosOBC').append($('<option>', {
                                value: origenDato.Id,
                                text: origenDato.Descripcion
                            }));
                        }
                    });
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
                return false;
            }
        });

        $.ajax({
            url: '@Url.Action("CargarOrigenesDatosGastos", "Plantilla")',
            data: { idCostGroup: idCostGroup },
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (d) {
                if (d.length > 0) {
                    $('#selectEditarOrigenDatosGastosAño').empty();
                    $.each(d, function (i, origenDato) {
                        $('#selectEditarOrigenDatosGastosAño').append($('<option>', {
                            value: origenDato.Id,
                            text: origenDato.Descripcion
                        }));
                    });
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
                return false;
            }
        });

        $.ajax({
            url: '@Url.Action("CargarVariablesFormula", "Plantilla")',
            data: { idCostGroup: idCostGroup },
            type: 'GET',
            dataType: 'json',
            success: function (d) {
                if(d.length > 0){
                    $('#selectEditarVariableTC, #selectEditarVariableTCCustomer').empty();
                    $.each(d, function (i, origenDato) {
                        $('#selectEditarVariableTC, #selectEditarVariableTCCustomer').append($('<option>', {
                            value: origenDato.Id,
                            text : origenDato.Nombre
                        }));
                    });
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
                return false;
            }
        });

        $.ajax({
            url: '@Url.Action("CargarStep", "Plantilla")',
            data: { idStep: idStep },
            type: 'GET',
            dataType: 'json',
            success: function (d) {
                $("#txtEditarDescripcionStep").val(d.Descripcion);
                $('#selectEditarOrigenDatosOBC').val(d.OBCOrigenDatos);
                $('#hFormulaEditarTC').val(d.TCFormula);
                $('#txtEditarOperandoTC').val('');
                $('#txtFormulaEditarTC').val(d.TCFormulaDecodificada);
                $('#hFormulaEditarTCCustomer').val(d.TCFormulaCustomer);
                $('#txtEditarOperandoTCCustomer').val('');
                $('#txtFormulaEditarTCCustomer').val(d.TCFormulaCustomerDecodificada);
                $('#selectEditarOrigenDatosGastosAño').val(d.GastosAñoOrigenDatos);
                $('#selectEditarOrigenDatoReal').val(d.OrigenDatoReal);

                if (d.EsInfoGeneral == 1) {
                    $("input[name='infoGeneralStepEditar'][value='1']").prop("checked", true);
                    $('#divEditarStep').hide();
                } else {
                    $("input[name='infoGeneralStepEditar'][value='0']").prop("checked", true);
                    $('#divEditarStep').show();
                }

                $('#modalWindowEditarStep').modal('show');
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
                return false;
            }
        });
    }

    function CargarModalAgregarStep(idCostGroup) {
        $.ajax({
            url: '@Url.Action("CargarOrigenesDatos", "Plantilla")',
            data: { idCostGroup: idCostGroup },
            type: 'GET',
            dataType: 'json',
            success: function (d) {
                if (d.length > 0) {
                    $('#selectAgregarOrigenDatosOBC').empty();
                    $.each(d, function (i, origenDato) {
                        if (origenDato.Id != @CInt(ELL.OrigenDatosStep.OrigenDatosStep.Planificacion)){
                            $('#selectAgregarOrigenDatosOBC').append($('<option>', {
                                value: origenDato.Id,
                                text: origenDato.Descripcion
                            }));
                        }
                    });
                    //Por defecto MANUAL                    
                    $('#selectAgregarOrigenDatosOBC').val(@CInt(ELL.OrigenDatosStep.OrigenDatosStep.Manual));
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
                return false;
            }
        });

        $.ajax({
            url: '@Url.Action("CargarOrigenesDatosGastos", "Plantilla")',
            data: { idCostGroup: idCostGroup },
            type: 'GET',
            dataType: 'json',
            success: function (d) {
                if (d.length > 0) {
                    $('#selectAgregarOrigenDatosGastosAño').empty();
                    $.each(d, function (i, origenDato) {
                        $('#selectAgregarOrigenDatosGastosAño').append($('<option>', {
                            value: origenDato.Id,
                            text: origenDato.Descripcion
                        }));
                    });
                }
                //Por defecto MANUAL                    
                $('#selectAgregarOrigenDatosGastosAño').val(@CInt(ELL.OrigenDatosStep.OrigenDatosStep.Manual));
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
                return false;
            }
        });

        $.ajax({
            url: '@Url.Action("CargarVariablesFormula", "Plantilla")',
            data: { idCostGroup: idCostGroup },
            type: 'GET',
            dataType: 'json',
            success: function (d) {
                if(d.length > 0){
                    $('#selectAgregarVariableTC, #selectAgregarVariableTCCustomer').empty();
                    $.each(d, function (i, origenDato) {
                        $('#selectAgregarVariableTC, #selectAgregarVariableTCCustomer').append($('<option>', {
                            value: origenDato.Id,
                            text : origenDato.Nombre
                        }));
                    });
                    //Por defecto PBC                    
                    $('#selectAgregarVariableTCCustomer').val(@CInt(ELL.VariableFormula.Tipo.Paid_by_customer));
                    $('#modalWindowAgregarStep').modal('show');
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    }

    function CargarArbol(idTipoProyecto, nodeIdExpandir) {
        $("#botonAñadirPlanta").show();
        $("#contenedorCargando").show();

        $.ajax({
            url: '@Url.Action("CargarPlantillaArbol", "Plantilla")',
            data: { idTipoProyecto: idTipoProyecto },
            type: 'GET',
            dataType: 'json',
            success: function (d) {
                // Cogemos d.nodes porque es donde empiezan las plantas dependientes del tipo de proyecto
                if (d.nodes == null || d.nodes.length > 0) {
                    $("#arbol").show();
                    $('#tree').treeview({
                        data: d.nodes
                    });

                    if(nodeIdExpandir){
                        $('#tree').treeview('revealNode', parseInt(nodeIdExpandir), {silent: true});
                        $('#tree').treeview('expandNode', parseInt(nodeIdExpandir), {silent: true});
                    }

                    //$('.list-group').sortable();
                    //$('.list-group').disableSelection();
                }
                else{
                    // No hay nodos o porque no existe la plantilla o porque no existen plantas asociadas así que tenemos que dar la opción de añadir
                    $("#arbol").hide();
                }
                $("#contenedorCargando").hide();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                $("#contenedorCargando").hide();
                alert(xhr.status);
                alert(thrownError);
            }
        });
    }

    function EliminarPlanta(idPlantaPlantilla){
        $.ajax({
            url: '@Url.Action("EliminarPlanta", "Plantilla")',
            data: { id: idPlantaPlantilla },
            type: "POST",
            success: function (d){
                var estilo;
                switch(d.messageType) {
                    case 'info':
                        var idTipoProyecto = $("input[name='tipoProyecto']:checked").val();
                        CargarArbol(idTipoProyecto);
                        estilo = "alert alert-success";
                        break;
                    case 'alerta':
                        estilo = "alert alert-warning";
                        break;
                    case 'error':
                        estilo = "alert alert-danger";
                        break;
                    default:
                }

                MostrarMensaje(d.message, estilo);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    }

    function EliminarEstado(idEstadoPlantilla){
        $.ajax({
            url: '@Url.Action("EliminarEstado", "Plantilla")',
            data: { id: idEstadoPlantilla },
            type: "POST",
            success: function (d){
                var estilo;
                switch(d.messageType) {
                    case 'info':
                        var idTipoProyecto = $("input[name='tipoProyecto']:checked").val();
                        var nodoPadre = $('#tree').treeview('getParent', parseInt($("#hNodeIdActual").val()));
                        nodoIdExpandir = nodoPadre.nodeId;
                        CargarArbol(idTipoProyecto, nodoIdExpandir);
                        estilo = "alert alert-success";
                        break;
                    case 'alerta':
                        estilo = "alert alert-warning";
                        break;
                    case 'error':
                        estilo = "alert alert-danger";
                        break;
                    default:
                }

                MostrarMensaje(d.message, estilo);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    }

    function EliminarCostGroup(idCostGroup){
        $.ajax({
            url: '@Url.Action("EliminarCostGroup", "Plantilla")',
            data: { id: idCostGroup },
            type: "POST",
            success: function (d){
                var estilo;
                switch(d.messageType) {
                    case 'info':
                        var idTipoProyecto = $("input[name='tipoProyecto']:checked").val();
                        var nodoPadre = $('#tree').treeview('getParent', parseInt($("#hNodeIdActual").val()));
                        nodoIdExpandir = nodoPadre.nodeId;
                        CargarArbol(idTipoProyecto, nodoIdExpandir);
                        estilo = "alert alert-success";
                        break;
                    case 'alerta':
                        estilo = "alert alert-warning";
                        break;
                    case 'error':
                        estilo = "alert alert-danger";
                        break;
                    default:
                }

                MostrarMensaje(d.message, estilo);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    }

    function EliminarStep(idStep){
        $.ajax({
            url: '@Url.Action("EliminarStep", "Plantilla")',
            data: { id: idStep },
            type: "POST",
            success: function (d){
                var estilo;
                switch(d.messageType) {
                    case 'info':
                        var idTipoProyecto = $("input[name='tipoProyecto']:checked").val();
                        var nodoPadre = $('#tree').treeview('getParent', parseInt($("#hNodeIdActual").val()));
                        nodoIdExpandir = nodoPadre.nodeId;
                        CargarArbol(idTipoProyecto, nodoIdExpandir);
                        estilo = "alert alert-success";
                        break;
                    case 'alerta':
                        estilo = "alert alert-warning";
                        break;
                    case 'error':
                        estilo = "alert alert-danger";
                        break;
                    default:
                }

                MostrarMensaje(d.message, estilo);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    }

    function CambiarOrdenSteps(currentOrder){
        $.ajax({
            url: '@Url.Action("CambiarOrdenSteps", "Plantilla")',
            data: { ordenActual: currentOrder},
            type: 'POST',
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    }

    $(function () {
        /******** VALIDACIONES ********/

        var validatorAgregarCostGroup = $("#AgregarCostGroupForm").validate({
			errorLabelContainer: $("#AgregarCostGroupForm div.error")
		});
        var validatorEditarCostGroup = $("#EditarCostGroupForm").validate({
			errorLabelContainer: $("#EditarCostGroupForm div.error")
		});
        var validatorAgregarStep = $("#AgregarStepForm").validate({
			errorLabelContainer: $("#AgregarStepForm div.error")
		});
        var validatorEditarStep = $("#EditarStepForm").validate({
			errorLabelContainer: $("#EditarStepForm div.error")
		});

        $('#modalWindowAgregarCostGroup').on('hidden.bs.modal', function (e) {
            /* Reseteamos el formulario modal al cerrarlo */
            validatorAgregarCostGroup.resetForm();
        });
        $('#modalWindowEditarCostGroup').on('hidden.bs.modal', function (e) {
            /* Reseteamos el formulario modal al cerrarlo */
            validatorEditarCostGroup.resetForm();
        });
        $('#modalWindowAgregarStep').on('hidden.bs.modal', function (e) {
            /* Reseteamos el formulario modal al cerrarlo */
            validatorAgregarStep.resetForm();
        });
        $('#modalWindowEditarStep').on('hidden.bs.modal', function (e) {
            /* Reseteamos el formulario modal al cerrarlo */
            validatorEditarStep.resetForm();
        });

        /******************************/
        $("input[name='infoGeneralStepAgregar']").change(function () {
            var infoGeneral = $("input[name='infoGeneralStepAgregar']:checked").val();
            if (infoGeneral == 1) {
                $("#divAgregarStep").hide();
                $("#txtAgregarDescripcionStep").val('@Utils.Traducir("Información general")');
            } else if (infoGeneral == 0) {
                $("#divAgregarStep").show();
                $("#txtAgregarDescripcionStep").val("");
            }
        });

        $("input[name='tipoProyecto']").change(function () {
            var idTipoProyecto = $("input[name='tipoProyecto']:checked").val();
            CargarArbol(idTipoProyecto);
        });

        $("input[name='origenCostGroupAgregar']").change(function () {
            var origenCostGroup = $("input[name='origenCostGroupAgregar']:checked").val();

            validatorAgregarCostGroup.resetForm();
            $('#txtAgregarDescripcionCostGroup').val('');
            if(origenCostGroup == 0){
                //Nuevo cost group
                if ($('#selectAgregarCostGroupOT').val() == '-1'){
                    $('#txtAgregarDescripcionCostGroup').prop('required', 'required');
                }
                else {
                    $('#txtAgregarDescripcionCostGroup').prop('required', false);
                }

                $('#txtAgregarDescripcionCostGroup').prop('disabled', false);
                $('#selectAgregarCostGroup').prop('disabled', 'disabled');
            }
            else{
                //Cost group de bonos
                $('#txtAgregarDescripcionCostGroup').prop('required', false);
                $('#txtAgregarDescripcionCostGroup').prop('disabled', 'disabled');
                $('#selectAgregarCostGroup').prop('disabled', false);
            }
        });

        $("input[name='origenCostGroupEditar']").change(function () {
            var origenCostGroup = $("input[name='origenCostGroupEditar']:checked").val();

            validatorEditarCostGroup.resetForm();
            $('#txtEditarDescripcionCostGroup').val('');
            if(origenCostGroup == 0){
                //Nuevo cost group
                if ($('#selectEditarCostGroupOT').val() == '-1') {
                    $('#txtEditarDescripcionCostGroup').prop('required', 'required');
                }
                else {
                    $('#txtEditarDescripcionCostGroup').prop('required', false);
                }

                $('#txtEditarDescripcionCostGroup').prop('disabled', false);
                $('#selectEditarCostGroup').prop('disabled', 'disabled');
            }
            else{
                //Cost group de bonos
                $('#txtEditarDescripcionCostGroup').prop('required', false);
                $('#txtEditarDescripcionCostGroup').prop('disabled', 'disabled');
                $('#selectEditarCostGroup').prop('disabled', false);
            }
        });

        $("#btnAgregarPlanta").click(function () {
            var idTipoProyecto = $("input[name='tipoProyecto']:checked").val();
            $('#hTipoProyecto').val(idTipoProyecto);
            CargarModalPlantas(idTipoProyecto);
        });

        $("#btnConfirmAgregarPlanta, #btnConfirmAgregarEstado, #btnConfirmAgregarCostGroup, #btnConfirmEditarCostGroup, #btnConfirmAgregarStep, #btnConfirmEditarStep").click(function () {            
            var buttonId = this.id;
            var $form = $(this).parents('form');

            if($form.valid()){
                $.ajax({
                    url: $form.attr('action'),
                    data: $form.serialize(),
                    type: "POST",
                    success: function(d) {
                        var estilo;
                        switch(d.messageType) {
                            case 'info':
                                estilo = "alert alert-success";
                                var nodoIdExpandir;
                                switch(buttonId) {
                                    case 'btnConfirmAgregarPlanta':
                                        $('#modalWindowPlantas').modal('hide');
                                        break;
                                    case 'btnConfirmAgregarEstado':
                                        $('#modalWindowEstados').modal('hide');
                                        nodoIdExpandir = $("#hNodeIdActual").val();
                                        break;
                                    case 'btnConfirmAgregarCostGroup':
                                        $('#modalWindowAgregarCostGroup').modal('hide');
                                        nodoIdExpandir = $("#hNodeIdActual").val();
                                        break;
                                    case 'btnConfirmEditarCostGroup':
                                        $('#modalWindowEditarCostGroup').modal('hide');
                                        var nodoPadre = $('#tree').treeview('getParent', parseInt($("#hNodeIdActual").val()));
                                        nodoIdExpandir = nodoPadre.nodeId;
                                        break;
                                    case 'btnConfirmAgregarStep':
                                        $('#modalWindowAgregarStep').modal('hide');
                                        nodoIdExpandir = $("#hNodeIdActual").val();
                                        break;
                                    case 'btnConfirmEditarStep':
                                        $('#modalWindowEditarStep').modal('hide');
                                        var nodoPadre = $('#tree').treeview('getParent', parseInt($("#hNodeIdActual").val()));
                                        nodoIdExpandir = nodoPadre.nodeId;
                                        break;
                                }

                                var idTipoProyecto = $("input[name='tipoProyecto']:checked").val();
                                CargarArbol(idTipoProyecto, nodoIdExpandir);
                                break;
                            case 'alerta':
                                estilo = "alert alert-warning";
                                break;
                            case 'error':
                                estilo = "alert alert-danger";
                                break;
                        }
                        MostrarMensaje(d.message, estilo);
                    },
                    error: function (xhr, status, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });
            }

            return false;
        });

        $("#txtAgregarOperandoTC, #txtEditarOperandoTC").alphanum({
            allow              : '+ - * / ( ) . ,',    // Allow extra characters
	        allowLatin         : false,  // a-z A-Z
	        allowOtherCharSets : false  // eg é, Á, Arabic, Chinese etc
        });

        $("#btnAgregarOperandoTC").click(function(){
            $("#txtFormulaAgregarTC").val($("#txtFormulaAgregarTC").val() + $("#txtAgregarOperandoTC").val());
            $("#hFormulaAgregarTC").val($("#hFormulaAgregarTC").val() + $("#txtAgregarOperandoTC").val());
        });

        $("#btnAgregarOperandoTCCustomer").click(function () {
            $("#txtFormulaAgregarTCCustomer").val($("#txtFormulaAgregarTCCustomer").val() + $("#txtAgregarOperandoTCCustomer").val());
            $("#hFormulaAgregarTCCustomer").val($("#hFormulaAgregarTCCustomer").val() + $("#txtAgregarOperandoTCCustomer").val());
        });

        $("#btnAgregarVariableTC").click(function(){
            $("#txtFormulaAgregarTC").val($("#txtFormulaAgregarTC").val() + '[' + $("#selectAgregarVariableTC option:selected").text() + "]");
            $("#hFormulaAgregarTC").val($("#hFormulaAgregarTC").val() + '[' + $("#selectAgregarVariableTC option:selected").val() + "]");
        });

        $("#btnAgregarVariableTCCustomer").click(function () {
            $("#txtFormulaAgregarTCCustomer").val($("#txtFormulaAgregarTCCustomer").val() + '[' + $("#selectAgregarVariableTCCustomer option:selected").text() + "]");
            $("#hFormulaAgregarTCCustomer").val($("#hFormulaAgregarTCCustomer").val() + '[' + $("#selectAgregarVariableTCCustomer option:selected").val() + "]");
        });

        $("#btnLimpiarAgregarFormulaTC").click(function(){
            $("#txtFormulaAgregarTC").val('');
            $("#hFormulaAgregarTC").val('');
        });

        $("#btnLimpiarAgregarFormulaTCCustomer").click(function () {
            $("#txtFormulaAgregarTCCustomer").val('');
            $("#hFormulaAgregarTCCustomer").val('');
        });

        $("#btnEditarOperandoTC").click(function(){
            $("#txtFormulaEditarTC").val($("#txtFormulaEditarTC").val() + $("#txtEditarOperandoTC").val());
            $("#hFormulaEditarTC").val($("#hFormulaEditarTC").val() + $("#txtEditarOperandoTC").val());
        });

        $("#btnEditarOperandoTCCustomer").click(function () {
            $("#txtFormulaEditarTCCustomer").val($("#txtFormulaEditarTCCustomer").val() + $("#txtEditarOperandoTCCustomer").val());
            $("#hFormulaEditarTCCustomer").val($("#hFormulaEditarTCCustomer").val() + $("#txtEditarOperandoTCCustomer").val());
        });

        $("#btnEditarVariableTC").click(function(){
            $("#txtFormulaEditarTC").val($("#txtFormulaEditarTC").val() + '[' + $("#selectEditarVariableTC option:selected").text() + "]");
            $("#hFormulaEditarTC").val($("#hFormulaEditarTC").val() + '[' + $("#selectEditarVariableTC option:selected").val() + "]");
        });

        $("#btnEditarVariableTCCustomer").click(function () {
            $("#txtFormulaEditarTCCustomer").val($("#txtFormulaEditarTCCustomer").val() + '[' + $("#selectEditarVariableTCCustomer option:selected").text() + "]");
            $("#hFormulaEditarTCCustomer").val($("#hFormulaEditarTCCustomer").val() + '[' + $("#selectEditarVariableTCCustomer option:selected").val() + "]");
        });

        $("#btnLimpiarEditarFormulaTC").click(function(){
            $("#txtFormulaEditarTC").val('');
            $("#hFormulaEditarTC").val('');
        });

        $("#btnLimpiarEditarFormulaTCCustomer").click(function () {
            $("#txtFormulaEditarTCCustomer").val('');
            $("#hFormulaEditarTCCustomer").val('');
        });

        $("#selectAgregarCostGroupOT").change(function () {
            validatorAgregarCostGroup.resetForm();
            var origenCostGroup = $("input[name='origenCostGroupAgregar']:checked").val();

            if ($(this).val() == "-1") {
                if (origenCostGroup == 0) {
                    $('#txtAgregarDescripcionCostGroup').prop('required', 'required');
                }
                else
                {
                    $('#txtAgregarDescripcionCostGroup').prop('required', false);
                }
            }
            else {
                $('#txtAgregarDescripcionCostGroup').prop('required', false);
            }
        });

        $("#selectEditarCostGroupOT").change(function () {
            validatorEditarCostGroup.resetForm();
            var origenCostGroup = $("input[name='origenCostGroupEditar']:checked").val();

            if ($(this).val() == "-1") {
                if (origenCostGroup == 0) {
                    $('#txtEditarDescripcionCostGroup').prop('required', 'required');
                }
                else {
                    $('#txtEditarDescripcionCostGroup').prop('required', false);
                }
            }
            else {
                $('#txtEditarDescripcionCostGroup').prop('required', false);
            }
        });

        // En la funcion Tree.prototype.buildTree = function (nodes, level) del bootstrap-treeview.js se ha añadido un trigger para que nos demos cuenta cuando se ha
        // generado el árbol porque al desplegar un nodo se regenera el árbol y no lo pone como sortable        
        $(document).on("treeBuilt", function (event) {            
            $('.list-group').sortable({
                items: "[data-kind='step']",
                update: function (event, ui) {                    
                    var currentOrder = $(this).sortable('toArray', { attribute: 'data-id' });
                    CambiarOrdenSteps(currentOrder);
                }
            });
            $('.list-group').disableSelection();
        });
    });
</script>

@Html.Hidden("hNodeIdActual")

<div id="contenedorCargando" class="contenedor-cargando" style="display:none; z-index:99999;">
    <div class="contenedor-imagen-cargando">
        <img src='@Url.Content("~/Content/Images/loading.gif")' />
    </div>
</div>

@Using Html.BeginForm("Editar", "Plantilla", FormMethod.Post, New With {.class = "form-horizontal"})
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Tipo de proyecto")</label>
        @code
            For Each tipoProyecto In tiposProyecto
    @<div class="col-sm-3">
        @If (idTipoProyecto IsNot Nothing AndAlso idTipoProyecto = tipoProyecto.Id) Then
    @Html.RadioButton("tipoProyecto", tipoProyecto.Id, True)
        Else
    @Html.RadioButton("tipoProyecto", tipoProyecto.Id)
        End If
        <label Class="control-label">@tipoProyecto.Descripcion</label>

    </div>
            Next
        End Code
    </div>
    @<div id="botonAñadirPlanta" Class="form-group" style="display:none;">
        @*<input type="button" id="btnAgregarPlanta" value="@Utils.Traducir("Agregar planta")" class="col-sm-offset-2 col-sm-2 btn btn-primary glyphicon glyphicon-plus" data-toggle="modal" />*@
        <button type="button" id="btnAgregarPlanta" class="col-sm-offset-2 col-sm-2 btn btn-info">
            <span class="glyphicon glyphicon-plus"></span>&nbsp;@Utils.Traducir("Agregar planta")
        </button>
    </div>

    @<div id="arbol" Class="form-group">
        <div id="tree" class="col-sm-offset-2 col-sm-6 "></div>
    </div>
            End Using

        <div class="modal fade" id="modalWindowPlantas" tabindex="-1" role="dialog" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">@Utils.Traducir("Agregar nueva planta")</h4>
                    </div>
                    <div class="modal-body">
                        @Using Html.BeginForm("AgregarPlanta", "Plantilla", Nothing, New With {.class = "form-horizontal"})
                @Html.Hidden("hTipoProyecto")
                @<div Class="form-group">
                    <label class="col-sm-2 control-label">@Utils.Traducir("Planta")</label>
                    <div class="col-sm-10">
                        <select id="selectPlantas" name="selectPlantas" class="form-control"></select>
                    </div>
                </div>
                @<div Class="form-group">
                    <div class="col-sm-offset-2 col-sm-10">
                        <input type="submit" id="btnConfirmAgregarPlanta" value="@Utils.Traducir("Agregar")" class="btn btn-primary input-block-level form-control" />
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

        <div class="modal fade" id="modalWindowEstados" tabindex="-1" role="dialog" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">@Utils.Traducir("Agregar nuevo estado")</h4>
                    </div>
                    <div class="modal-body">
                        @Using Html.BeginForm("AgregarEstado", "Plantilla", FormMethod.Post, New With {.class = "form-horizontal"})
                @Html.Hidden("hPlantaPlantilla")
                @<div Class="form-group">
                    <label class="col-sm-2 control-label">@Utils.Traducir("Estado")</label>
                    <div class="col-sm-10">
                        <select id="selectEstados" name="selectEstados" class="form-control"></select>
                    </div>
                </div>
                @<div Class="form-group">
                    <div class="col-sm-offset-2 col-sm-10">
                        <input type="submit" id="btnConfirmAgregarEstado" value="@Utils.Traducir("Agregar")" class="btn btn-primary input-block-level form-control" />
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

        <div class="modal fade" id="modalWindowAgregarCostGroup" tabindex="-1" role="dialog" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">@Utils.Traducir("Agregar nuevo grupo de coste")</h4>
                    </div>
                    <div class="modal-body">
                        @Using Html.BeginForm("AgregarCostGroup", "Plantilla", FormMethod.Post, New With {.class = "form-horizontal", .id = "AgregarCostGroupForm"})
                @Html.Hidden("hEstadoPlantilla")
                @<div Class="form-group col-sm-12">
                    @Html.RadioButton("origenCostGroupAgregar", 0, True)
                    <label class="control-label">@Utils.Traducir("Nuevo")</label>
                </div>
                @<div Class="form-group">
                    <label class="col-sm-3 control-label">@Utils.Traducir("Descripción")</label>
                    <div class="col-sm-9">
                        @Html.TextBox("txtAgregarDescripcionCostGroup", Nothing, New With {.class = "form-control", .style = "text-transform:uppercase", .required = "required", .maxlength = "50", .title = Utils.Traducir("Descripción") & " " & Utils.Traducir("campo obligatorio")})
                    </div>
                </div>
                @<div Class="form-group col-sm-12">
                    @Html.RadioButton("origenCostGroupAgregar", 1)
                    <label class="control-label">@Utils.Traducir("Bonos")</label>
                </div>
                @<div Class="form-group">
                    <label class="col-sm-3 control-label">@Utils.Traducir("Grupo de coste de bonos")</label>
                    <div class="col-sm-9">
                        <select id="selectAgregarCostGroup" name="selectAgregarCostGroup" class="form-control" disabled="disabled"></select>
                    </div>
                </div>
                @<hr />
                @<div Class="form-group">
                    <label class="col-sm-3 control-label">@Utils.Traducir("Grupo de coste OT")</label>
                    <div class="col-sm-9">
                        <select id="selectAgregarCostGroupOT" name="selectAgregarCostGroupOT" class="form-control"></select>
                    </div>
                </div>
                @<div Class="form-group">
                    <div class="col-sm-offset-3 col-sm-9 error"></div>
                </div>
                @<div Class="form-group">
                    <div class="col-sm-offset-3 col-sm-9" style="color:#f3f2db">
                        <input type="submit" id="btnConfirmAgregarCostGroup" value="@Utils.Traducir("Agregar")" class="btn btn-primary input-block-level form-control" />
                    </div>
                </div>
                        End Using
                    </div>
                    <div Class="modal-footer">
                        <Button type="button" Class="btn btn-secondary" data-dismiss="modal">@Utils.Traducir("Cerrar")</Button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modalWindowEditarCostGroup" tabindex="-1" role="dialog" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">@Utils.Traducir("Editar grupo de coste")</h4>
                    </div>
                    <div class="modal-body">
                        @Using Html.BeginForm("EditarCostGroup", "Plantilla", FormMethod.Post, New With {.class = "form-horizontal", .id = "EditarCostGroupForm"})
                @Html.Hidden("hCostGroupPlantilla")
                @<div Class="form-group col-sm-12">
                    @Html.RadioButton("origenCostGroupEditar", 0)
                    <label class="control-label">@Utils.Traducir("Nuevo")</label>
                </div>
                @<div Class="form-group">
                    <label class="col-sm-3 control-label">@Utils.Traducir("Descripción")</label>
                    <div class="col-sm-9">
                        @Html.TextBox("txtEditarDescripcionCostGroup", Nothing, New With {.class = "form-control", .style = "text-transform:uppercase", .required = "required", .maxlength = "50", .title = Utils.Traducir("Descripción") & " " & Utils.Traducir("campo obligatorio")})
                    </div>
                </div>
                @<div Class="form-group col-sm-12">
                    @Html.RadioButton("origenCostGroupEditar", 1)
                    <label class="control-label">@Utils.Traducir("Bonos")</label>
                </div>
                @<div Class="form-group">
                    <label class="col-sm-3 control-label">@Utils.Traducir("Grupo de coste de bonos")</label>
                    <div class="col-sm-9">
                        <select id="selectEditarCostGroup" name="selectEditarCostGroup" class="form-control"></select>
                    </div>
                </div>
                @<hr />
                @<div Class="form-group">
                    <label class="col-sm-3 control-label">@Utils.Traducir("Grupo de coste OT")</label>
                    <div class="col-sm-9">
                        <select id="selectEditarCostGroupOT" name="selectEditarCostGroupOT" class="form-control"></select>
                    </div>
                </div>
                @<div Class="form-group">
                    <div class="col-sm-offset-3 col-sm-9 error"></div>
                </div>
                @<div Class="form-group">
                    <div class="col-sm-offset-3 col-sm-9">
                        <input type="submit" id="btnConfirmEditarCostGroup" value="@Utils.Traducir("Guardar")" class="btn btn-primary input-block-level form-control" />
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
            <div class="modal-dialog modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">@Utils.Traducir("Agregar nuevo paso")</h4>
                    </div>
                    <div class="modal-body">
                        @Using Html.BeginForm("AgregarStep", "Plantilla", FormMethod.Post, New With {.class = "form-horizontal", .id = "AgregarStepForm"})
                    @Html.Hidden("hCostGroupPlantillaParaStep")
                    @<div Class="form-group">
                        <label class="col-sm-4 control-label">@Utils.Traducir("Descripción")</label>
                        <div class="col-sm-8">
                            @Html.TextBox("txtAgregarDescripcionStep", Nothing, New With {.class = "form-control", .required = "required", .maxlength = "100", .title = Utils.Traducir("Descripción") & " " & Utils.Traducir("campo obligatorio")})
                        </div>
                    </div>
                    @<div Class="form-group">
                        <label class="col-sm-4 control-label">@Utils.Traducir("Es información general?")</label>
                         <div class="col-sm-8">
                            @Html.RadioButton("infoGeneralStepAgregar", 1)
                            <label class="control-label">@Utils.Traducir("Si")</label>
                            @Html.RadioButton("infoGeneralStepAgregar", 0, True)
                            <label class="control-label">@Utils.Traducir("No")</label>
                        </div>
                    </div>
                    @<div id="divAgregarStep">
                    <div Class="form-group">
                        <label class="col-sm-4 control-label">@Utils.Traducir("Coste presupuestado origen")</label>
                        <div class="col-sm-8">
                            <select id="selectAgregarOrigenDatosOBC" name="selectAgregarOrigenDatosOBC" class="form-control"></select>
                        </div>
                    </div>
                    <div Class="form-group" style="padding-top:10px; background-color:#dedede;">
                        @Html.Hidden("hFormulaAgregarTC")
                        <label class="col-sm-4 control-label">@Utils.Traducir("Coste objetivo formula BATZ")</label>
                        <div class="col-sm-8">
                            <div Class="form-row">
                                <div class="form-group col-md-5">
                                    <label class="col-form-label">@Utils.Traducir("Operando")</label>
                                    @Html.TextBox("txtAgregarOperandoTC", String.Empty, New With {.class = "form-control"})
                                </div>
                                <div class="form-group col-md-2">
                                    <label class="col-form-label">&nbsp;</label>
                                    <button type="button" id="btnAgregarOperandoTC" class="btn btn-primary form-control">
                                        <span class="glyphicon glyphicon-plus"></span>
                                    </button>
                                </div>
                                <div class="form-group col-md-5">
                                    <label class="col-form-label">@Utils.Traducir("Variable")</label>
                                    <select id="selectAgregarVariableTC" name="selectAgregarVariableTC" class="form-control"></select>
                                </div>
                                <div class="form-group col-md-2">
                                    <label class="col-form-label">&nbsp;</label>
                                    <button type="button" id="btnAgregarVariableTC" class="btn btn-primary form-control">
                                        <span class="glyphicon glyphicon-plus"></span>
                                    </button>
                                </div>
                                <div Class="form-group col-md-11">
                                    <Label Class="col-form-label">@Utils.Traducir("Fórmula")</Label>
                                    @Html.TextBox("txtFormulaAgregarTC", Nothing, New With {.class = "form-control", .required = "required", .maxlength = "500", .readonly = "readonly", .title = Utils.Traducir("Coste objetivo formula BATZ") & " " & Utils.Traducir("campo obligatorio")})
                                </div>
                                <div class="form-group col-md-2">
                                    <label class="col-form-label">&nbsp;</label>
                                    <button type="button" id="btnLimpiarAgregarFormulaTC" class="btn btn-primary form-control">
                                        <span class="glyphicon glyphicon-trash"></span>
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div Class="form-group" style="padding-top:10px; background-color:#dedede;">
                        @Html.Hidden("hFormulaAgregarTCCustomer")
                        <label class="col-sm-4 control-label">@Utils.Traducir("Coste objetivo formula cliente")</label>
                        <div class="col-sm-8">
                            <div Class="form-row">
                                <div class="form-group col-md-5">
                                    <label class="col-form-label">@Utils.Traducir("Operando")</label>
                                    @Html.TextBox("txtAgregarOperandoTCCustomer", String.Empty, New With {.class = "form-control"})
                                </div>
                                <div class="form-group col-md-2">
                                    <label class="col-form-label">&nbsp;</label>
                                    <button type="button" id="btnAgregarOperandoTCCustomer" class="btn btn-primary form-control">
                                        <span class="glyphicon glyphicon-plus"></span>
                                    </button>
                                </div>
                                <div class="form-group col-md-5">
                                    <label class="col-form-label">@Utils.Traducir("Variable")</label>
                                    <select id="selectAgregarVariableTCCustomer" name="selectAgregarVariableTCCustomer" class="form-control"></select>
                                </div>
                                <div class="form-group col-md-2">
                                    <label class="col-form-label">&nbsp;</label>
                                    <button type="button" id="btnAgregarVariableTCCustomer" class="btn btn-primary form-control">
                                        <span class="glyphicon glyphicon-plus"></span>
                                    </button>
                                </div>
                                <div Class="form-group col-md-11">
                                    <Label Class="col-form-label">@Utils.Traducir("Fórmula")</Label>
                                    @Html.TextBox("txtFormulaAgregarTCCustomer", Nothing, New With {.class = "form-control", .required = "required", .maxlength = "500", .readonly = "readonly", .title = Utils.Traducir("Coste objetivo formula cliente") & " " & Utils.Traducir("campo obligatorio")})
                                </div>
                                <div class="form-group col-md-2">
                                    <label class="col-form-label">&nbsp;</label>
                                    <button type="button" id="btnLimpiarAgregarFormulaTCCustomer" class="btn btn-primary form-control">
                                        <span class="glyphicon glyphicon-trash"></span>
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div Class="form-group">
                        <label class="col-sm-4 control-label">@Utils.Traducir("Presupuesto aprobado origen")</label>
                        <div class="col-sm-8">
                            <select id="selectAgregarOrigenDatosGastosAño" name="selectAgregarOrigenDatosGastosAño" class="form-control"></select>
                        </div>
                    </div>
                    <div Class="form-group">
                        <label class="col-sm-4 control-label">@Utils.Traducir("Origen dato real")</label>
                        <div class="col-sm-8">
                            <select id="selectAgregarOrigenDatoReal" name="selectAgregarOrigenDatoReal" class="form-control">
                                <option value="0" selected="selected">EXT</option>
                                <option value="1">INT</option>
                                <option value="2">INT/EXT</option>
                            </select>
                        </div>
                    </div>
                    </div>
                    @<div Class="form-group">
                        <div class="col-sm-offset-4 col-sm-8 error"></div>
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
        </div>

        <div class="modal fade" id="modalWindowEditarStep" tabindex="-1" role="dialog" aria-hidden="true">
            <div class="modal-dialog modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">@Utils.Traducir("Editar paso")</h4>
                    </div>
                    <div class="modal-body">
                        @Using Html.BeginForm("EditarStep", "Plantilla", FormMethod.Post, New With {.class = "form-horizontal", .id = "EditarStepForm"})
                    @Html.Hidden("hStepPlantilla")
                    @<div Class="form-group">
                        <label class="col-sm-4 control-label">@Utils.Traducir("Descripción")</label>
                        <div class="col-sm-8">
                            @Html.TextBox("txtEditarDescripcionStep", Nothing, New With {.class = "form-control", .required = "required", .maxlength = "100", .title = Utils.Traducir("Descripción") & " " & Utils.Traducir("campo obligatorio")})
                        </div>
                    </div>
                    @<div Class="form-group">
                        <label class="col-sm-4 control-label">@Utils.Traducir("Es información general?")</label>
                         <div class="col-sm-8">
                            @Html.RadioButton("infoGeneralStepEditar", 1)
                            <label class="control-label">@Utils.Traducir("Si")</label>
                            @Html.RadioButton("infoGeneralStepEditar", 0)
                            <label class="control-label">@Utils.Traducir("No")</label>
                        </div>
                    </div>
                    @<div id="divEditarStep">
                    <div Class="form-group">
                        <label class="col-sm-4 control-label">@Utils.Traducir("Coste presupuestado origen")</label>
                        <div class="col-sm-8">
                            <select id="selectEditarOrigenDatosOBC" name="selectEditarOrigenDatosOBC" class="form-control"></select>
                        </div>
                    </div>
                    <div Class="form-group" style="padding-top:10px; background-color:#dedede;">
                        @Html.Hidden("hFormulaEditarTC")
                        <label class="col-sm-4 control-label">@Utils.Traducir("Coste objetivo formula BATZ")</label>
                        <div class="col-sm-8">
                            <div Class="form-row">
                                <div class="form-group col-md-5">
                                    <label class="col-form-label">@Utils.Traducir("Operando")</label>
                                    @Html.TextBox("txtEditarOperandoTC", String.Empty, New With {.class = "form-control"})
                                </div>
                                <div class="form-group col-md-2">
                                    <label class="col-form-label">&nbsp;</label>
                                    <button type="button" id="btnEditarOperandoTC" class="btn btn-primary form-control">
                                        <span class="glyphicon glyphicon-plus"></span>
                                    </button>
                                </div>
                                <div class="form-group col-md-5">
                                    <label class="col-form-label">@Utils.Traducir("Variable")</label>
                                    <select id="selectEditarVariableTC" name="selectEditarVariableTC" class="form-control"></select>
                                </div>
                                <div class="form-group col-md-2">
                                    <label class="col-form-label">&nbsp;</label>
                                    <button type="button" id="btnEditarVariableTC" class="btn btn-primary form-control">
                                        <span class="glyphicon glyphicon-plus"></span>
                                    </button>
                                </div>
                                <div Class="form-group col-md-11">
                                    <Label Class="col-form-label">@Utils.Traducir("Fórmula")</Label>
                                    @Html.TextBox("txtFormulaEditarTC", Nothing, New With {.class = "form-control", .required = "required", .maxlength = "500", .readonly = "readonly", .title = Utils.Traducir("Coste objetivo formula BATZ") & " " & Utils.Traducir("campo obligatorio")})
                                </div>
                                <div class="form-group col-md-2">
                                    <label class="col-form-label">&nbsp;</label>
                                    <button type="button" id="btnLimpiarEditarFormulaTC" class="btn btn-primary form-control">
                                        <span class="glyphicon glyphicon-trash"></span>
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div Class="form-group" style="padding-top:10px; background-color:#dedede;">
                        @Html.Hidden("hFormulaEditarTCCustomer")
                        <label class="col-sm-4 control-label">@Utils.Traducir("Coste objetivo formula cliente")</label>
                        <div class="col-sm-8">
                            <div Class="form-row">
                                <div class="form-group col-md-5">
                                    <label class="col-form-label">@Utils.Traducir("Operando")</label>
                                    @Html.TextBox("txtEditarOperandoTCCustomer", String.Empty, New With {.class = "form-control"})
                                </div>
                                <div class="form-group col-md-2">
                                    <label class="col-form-label">&nbsp;</label>
                                    <button type="button" id="btnEditarOperandoTCCustomer" class="btn btn-primary form-control">
                                        <span class="glyphicon glyphicon-plus"></span>
                                    </button>
                                </div>
                                <div class="form-group col-md-5">
                                    <label class="col-form-label">@Utils.Traducir("Variable")</label>
                                    <select id="selectEditarVariableTCCustomer" name="selectEditarVariableTCCustomer" class="form-control"></select>
                                </div>
                                <div class="form-group col-md-2">
                                    <label class="col-form-label">&nbsp;</label>
                                    <button type="button" id="btnEditarVariableTCCustomer" class="btn btn-primary form-control">
                                        <span class="glyphicon glyphicon-plus"></span>
                                    </button>
                                </div>
                                <div Class="form-group col-md-11">
                                    <Label Class="col-form-label">@Utils.Traducir("Fórmula")</Label>
                                    @Html.TextBox("txtFormulaEditarTCCustomer", Nothing, New With {.class = "form-control", .required = "required", .maxlength = "500", .readonly = "readonly", .title = Utils.Traducir("Coste objetivo formula cliente") & " " & Utils.Traducir("campo obligatorio")})
                                </div>
                                <div class="form-group col-md-2">
                                    <label class="col-form-label">&nbsp;</label>
                                    <button type="button" id="btnLimpiarEditarFormulaTCCustomer" class="btn btn-primary form-control">
                                        <span class="glyphicon glyphicon-trash"></span>
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div Class="form-group">
                        <label class="col-sm-4 control-label">@Utils.Traducir("Presupuesto aprobado origen")</label>
                        <div class="col-sm-8">
                            <select id="selectEditarOrigenDatosGastosAño" name="selectEditarOrigenDatosGastosAño" class="form-control"></select>
                        </div>
                    </div>
                    <div Class="form-group">
                        <label class="col-sm-4 control-label">@Utils.Traducir("Origen dato real")</label>
                        <div class="col-sm-8">
                            <select id="selectEditarOrigenDatoReal" name="selectEditarOrigenDatoReal" class="form-control">
                                <option value="0">EXT</option>
                                <option value="1">INT</option>
                                <option value="2">INT/EXT</option>
                            </select>
                        </div>
                    </div>
                        </div>
                    @<div Class="form-group">
                        <div class="col-sm-offset-4 col-sm-8 error"></div>
                    </div>
                    @<div Class="form-group">
                        <div class="col-sm-offset-4 col-sm-8">
                            <input type="submit" id="btnConfirmEditarStep" value="@Utils.Traducir("Guardar")" class="btn btn-primary input-block-level form-control" />
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



