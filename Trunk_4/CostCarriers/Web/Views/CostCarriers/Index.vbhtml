@Imports CostCarriersLib

@Code
    Dim costCarriers As List(Of ELL.CabeceraCostCarrier) = CType(ViewData("CostCarriers"), List(Of ELL.CabeceraCostCarrier))
    Dim tiposProyecto As List(Of ELL.TipoProyecto) = CType(ViewData("TiposProyecto"), List(Of ELL.TipoProyecto))
    Dim ticket As SabLib.ELL.Ticket = CType(Session("Ticket"), SabLib.ELL.Ticket)
End Code

<h3><label>@Utils.Traducir("Portadores de coste solicitados")</label></h3>
<hr />

<style>
    form label.error {
        color: red;
        display: block;
        width: auto;
    }
</style>

<script type="text/javascript">
    $(function () {
        /******** VALIDACIONES ********/

        var validatorAgregarCostCarrier = $("#AgregarCostCarrierForm").validate({
			errorLabelContainer: $("#AgregarCostCarrierForm div.error")
		});

        $('#modalWindowCostCarriers').on('hidden.bs.modal', function (e) {
            /* Reseteamos el formulario modal al cerrarlo */
            validatorAgregarCostCarrier.resetForm();
        });

        $(".boton-eliminar").click(function () {
            return confirm("@Html.Raw(Utils.Traducir("¿Desea eliminar el portador de coste seleccionado?"))");
        });

        $("#btnAgregarCostCarrier").click(function(){
             $("input[name='tipoProyecto']").prop("checked",false);
             $('#selectAgregarProducto').empty();
             $('#selectAgregarProyecto').empty();
             $("#hIdOffer").val('');
             $('#modalWindowCostCarriers').modal('show');
        });

        $("input[name='tipoProyecto']").change(function () {
            var idTipoProyecto = $("input[name='tipoProyecto']:checked").val();
            CargarProductos(idTipoProyecto);
        });

        $('#selectAgregarProducto').change(function () {
            var idTipoProyecto = $("input[name='tipoProyecto']:checked").val();
            CargarProyectos($(this).val(), idTipoProyecto);
        });

        $('#selectAgregarProyecto').change(function () {
            $("#hIdOffer").val($(this).find(":selected").data("idoffer"));
        });

        $("#btnConfirmAgregarCostCarrier").click(function () {
            $("#contenedorCargando").show();
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
                                //$('#modalWindowCostCarriers').modal('hide');
                                estilo = "alert alert-success";
                                //window.location.href = "/CostCarriers/Editar?IdCabecera=" + d.id
                                window.location = '@Url.Action("Editar", "CostCarriers")?idCabecera=' + d.id + '&recargarStepsOT=true&cargarResumenCambios=true'
                                break;
                            case 'alerta':
                                estilo = "alert alert-warning";
                                break;
                            case 'error':
                                estilo = "alert alert-danger";
                                break;
                        }
                        $("#contenedorCargando").hide();
                        MostrarMensaje(d.message, estilo);
                    },
                    error: function (xhr, status, thrownError) {
                        $("#contenedorCargando").hide();
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });
            }

            return false;
        });

        function CargarProductos(idTipoProyecto) {
            $("#contenedorCargando").show();

            $.ajax({
                url: '@Url.Action("CargarProductos", "CostCarriers")',
                data: { idTipoProyecto: idTipoProyecto, propietario: '@ticket.NombreUsuario'},
                type: 'GET',
                dataType: 'json',
                success: function (d) {
                    $('#selectAgregarProducto').empty();
                    $('#selectAgregarProyecto').empty();
                    $("#hIdOffer").val('');
                    if(d.length > 0){
                        $.each(d, function (i, producto) {
                            $('#selectAgregarProducto').append($('<option>', {
                                value: producto.Producto,
                                text: producto.Producto
                            }));
                        });
                        CargarProyectos($('#selectAgregarProducto').val(), $("input[name='tipoProyecto']:checked").val());
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

        function CargarProyectos(producto, idTipoProyecto) {
            $.ajax({
                url: '@Url.Action("CargarProyectos", "CostCarriers")',
                data: { producto: producto, idTipoProyecto: idTipoProyecto, propietario: '@ticket.NombreUsuario'},
                type: 'GET',
                dataType: 'json',
                success: function (d) {
                    $('#selectAgregarProyecto').empty();
                    if(d.length > 0){
                        $.each(d, function (i, proyecto) {
                            var option = $('<option/>');
                            option.attr({ 'value': proyecto.IdProject, 'data-idoffer': proyecto.IdOffer}).text(proyecto.Project);
                            $('#selectAgregarProyecto').append(option);
                        });
                        $("#hIdOffer").val($('#selectAgregarProyecto').find(":selected").data("idoffer"));
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                }
            });
        }

        $(".agregar-planta").click(function () {
            var validator = $("#AgregarPlantaForm").validate();
            validator.resetForm();

            var idCabecera = $(this).data("idcabecera");
            var idTipoProyecto = $(this).data("idtipoproyecto");
            var idproyecto = $(this).data("idproyecto");

            $("#hIdCabecera").val(idCabecera);
            $("#hIdTipoProyecto").val(idTipoProyecto);

            $.ajax({
                url: '@Url.Action("CargarPlantasAgregar", "CostCarriers")',
                data: { idCabecera: idCabecera, idTipoProyecto: idTipoProyecto, idProyecto: idproyecto},
                type: 'GET',
                dataType: 'json',
                success: function (d) {
                    $('#selectAgregarPlanta').empty();
                    if(d.length > 0){
                        $.each(d, function (i, planta) {
                            var option = $('<option/>');
                            option.attr({ 'value': planta.Id }).text(planta.Nombre);
                            $('#selectAgregarPlanta').append(option);
                        });
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                }
            });

            $('#modalWindowAgregarPlanta').modal('show');
        })
    })
</script>

<div id="contenedorCargando" class="contenedor-cargando" style="display:none; z-index:99999;">
    <div class="contenedor-imagen-cargando">
        <img src='@Url.Content("~/Content/Images/loading.gif")' />
    </div>
</div>

<button type="button" id="btnAgregarCostCarrier" class="btn btn-info">
    <span class="glyphicon glyphicon-plus"></span>&nbsp;@Utils.Traducir("Petición de nuevo proyecto")
</button>

<br /><br />

@Using Html.BeginForm("Index", "CostCarriers", FormMethod.Post)
    @<div Class="panel panel-default">
        <div Class="panel-heading">
            <h3 Class="panel-title">@Utils.Traducir("Filtros de búsqueda")</h3>
        </div>
        <div Class="panel-body">
            <div class="form-horizontal">
                <div Class="form-group">
                    <Label class="col-sm-1 control-label">@Utils.Traducir("Producto")</Label>
                    <div class="col-sm-3">
                        @Html.DropDownList("Productos", Nothing, New With {.class = "form-control"})
                    </div>
                    <Label class="col-sm-2 control-label">@Utils.Traducir("Estado cost carrier")</Label>
                    <div class="col-sm-3">
                        @Html.DropDownList("EstadoCostCarrier", Nothing, New With {.class = "form-control"})
                    </div>
                    <div class="col-sm-3">
                        <input type="submit" id="submit" value="@Utils.Traducir("Buscar")" Class="btn btn-primary form-control" />
                    </div>
                </div>
            </div>
        </div>
    </div>
End Using

@If (costCarriers.Count = 0) Then
    @Html.Label(String.Empty, Utils.Traducir("noExisteNingunRegistro"))
Else

    @<div class="row">
        <div class="col-sm-12">
            <table class="table table-striped table-hover table-condensed">
                <thead>
                    <tr>
                        <th>@Utils.Traducir("Tipo de proyecto")</th>
                        <th>@Utils.Traducir("Producto")</th>
                        <th>@Utils.Traducir("Código")</th>
                        <th>@Utils.Traducir("Proyecto")</th>
                        <th></th>
                        <th></th>
                        <th></th>
                        <th></th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    @code
                        For Each costCarrier In costCarriers.OrderBy(Function(f) f.Producto).ThenBy(Function(f) f.NombreProyecto)
                            Dim contienePasosAprobados As Boolean = BLL.CabecerasCostCarrierBLL.ContienePasosAprobados(costCarrier.Id)

                            @<tr>
                                <td>@costCarrier.TipoProyecto</td>
                                <td>@costCarrier.Producto</td>
                                <td>@costCarrier.CodigoProyecto</td>
                                <td>@costCarrier.NombreProyecto</td>
                                <td Class="text-center">
                                    <a href='@Url.Action("Editar", "CostCarriers", New With {.idCabecera = costCarrier.Id, .recargarStepsOT = True, .cargarResumenCambios = True})'>
                                        <span class="glyphicon glyphicon-pencil" aria-hidden="True" title="@Utils.Traducir("Editar")"></span>
                                    </a>
                                </td>
                                <td Class="text-center">
                                    @If (Not contienePasosAprobados) Then
                                        @<a href='@Url.Action("Eliminar", "CostCarriers", New With {.idCabecera = costCarrier.Id})'>
                                            <span class="glyphicon glyphicon-remove boton-eliminar" aria-hidden="true" title="@Utils.Traducir("Eliminar")"></span>
                                        </a>
                                    End If
                                </td>
                                <td>
                                    @code
                                        Dim listaPlantas As List(Of SabLib.ELL.Planta) = BLL.PlantasBLL.CargarPlantasAgregar(costCarrier.Id, costCarrier.IdTipoProyecto, costCarrier.Proyecto)

                                        If (listaPlantas IsNot Nothing AndAlso listaPlantas.Count > 0) Then
                                                                                @<a style = "cursor: pointer;" >
                                        <span class="glyphicon glyphicon-globe agregar-planta" aria-hidden="true" title="@Utils.Traducir("Agregar planta")" data-idcabecera="@costCarrier.Id" data-idtipoproyecto="@costCarrier.IdTipoProyecto" data-idproyecto="@costCarrier.Proyecto"></span>
                                                                                                        </a>

                                        End If

                                            End code
                                </td>
                                <td Class="text-center">
                                    <a href='@Url.Action("DetalleProyectoHistorico", "Validaciones", New With {.idCabecera = costCarrier.Id})'>
                                        <span class="glyphicon glyphicon-option-horizontal text-danger" aria-hidden="true" title="@Utils.Traducir("Estado actual e histórico")"></span>
                                    </a>
                                </td>
                                <td Class="text-center">
                                    <a href='@Url.Action("DetalleProyecto", "Totales", New With {.idCabecera = costCarrier.Id})'>
                                        <span class="glyphicon glyphicon-ok-circle text-success" aria-hidden="true" title="@Utils.Traducir("Totales por proyecto")"></span>
                                    </a>
                                </td>
                            </tr>
                        Next
                    End Code
                </tbody>
            </table>
        </div>
    </div>
End If

<div class="modal fade" id="modalWindowCostCarriers" tabindex="-1" role="dialog" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">@Utils.Traducir("Crear nuevo portador de coste")</h4>
            </div>
            <div class="modal-body">
                @Using Html.BeginForm("AgregarCostCarrier", "CostCarriers", Nothing, New With {.class = "form-horizontal", .id = "AgregarCostCarrierForm"})
                    @<div Class="form-group">
                        <span class="col-sm-12 label" style="font-size:larger;background-color:deeppink;">* @Utils.Traducir("Deberías ser propietario o co-propietario")</span>
                    </div>
                    @<div Class="form-group">
                        <span class="col-sm-12 label" style="font-size:larger;background-color:deeppink;">* @Utils.Traducir("Deberías tener la planificación del proyecto finalizada en la aplicación de bonos")</span>
                    </div>
                    @<div Class="form-group">
                        <label class="col-sm-4 control-label">@Utils.Traducir("Tipo de proyecto")</label>
                        <div class="col-sm-8">
                            @code
                                For Each tipoProyecto In tiposProyecto
                                    @<div>
                                        @Html.RadioButton("tipoProyecto", tipoProyecto.Id, New With {.required = "required", .title = Utils.Traducir("Tipo de proyecto") & " " & Utils.Traducir("campo obligatorio")})
                                        <label Class="control-label">@tipoProyecto.Descripcion</label>
                                    </div>
                                Next
                            End Code
                        </div>
                    </div>
                    @<div Class="form-group">
                        <label class="col-sm-4 control-label">@Utils.Traducir("Producto")</label>
                        <div class="col-sm-8">
                            <select id="selectAgregarProducto" name="selectAgregarProducto" class="form-control" required="required" title="@Utils.Traducir("Producto") @Utils.Traducir("campo obligatorio")"></select>
                        </div>
                    </div>
                    @<div Class="form-group">
                        <label class="col-sm-4 control-label">@Utils.Traducir("Proyecto")</label>
                        <div class="col-sm-8">
                            @Html.Hidden("hIdOffer")
                            <select id="selectAgregarProyecto" name="selectAgregarProyecto" class="form-control" required="required" title="@Utils.Traducir("Proyecto") @Utils.Traducir("campo obligatorio")"></select>
                        </div>
                    </div>
                    @<div Class="form-group">
                        <div class="col-sm-offset-4 col-sm-8 error"></div>
                    </div>
                    @<div Class="form-group">
                        <div class="col-sm-offset-4 col-sm-8">
                            <input type="submit" id="btnConfirmAgregarCostCarrier" value="@Utils.Traducir("Crear")" class="btn btn-primary input-block-level form-control" />
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

<div class="modal fade" id="modalWindowAgregarPlanta" tabindex="-1" role="dialog" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">@Utils.Traducir("Agregar planta a proyecto")</h4>
            </div>
            <div class="modal-body">
                @Using Html.BeginForm("AgregarPlanta", "CostCarriers", Nothing, New With {.class = "form-horizontal", .id = "AgregarPlantaForm"})
                    @<div Class="form-group">
                        <label class="col-sm-4 control-label">@Utils.Traducir("Planta")</label>
                        <div class="col-sm-8">
                            @Html.Hidden("hIdCabecera")
                            @Html.Hidden("hIdTipoProyecto")
                            <select id="selectAgregarPlanta" name="selectAgregarPlanta" class="form-control" required="required" title="@Utils.Traducir("Planta") @Utils.Traducir("campo obligatorio")"></select>
                        </div>
                    </div>
                    @<div Class="form-group">
                        <div class="col-sm-offset-4 col-sm-8">
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
