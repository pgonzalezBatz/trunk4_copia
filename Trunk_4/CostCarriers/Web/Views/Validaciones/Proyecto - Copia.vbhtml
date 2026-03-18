@Imports CostCarriersLib

@Code
    Dim validaciones As List(Of ELL.Validacion) = CType(ViewData("Validaciones"), List(Of ELL.Validacion))
    Dim culturaEsES As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture("es-ES")
    Dim listaIdsPlantasUnicos As List(Of Integer) = (From lstVal In validaciones
                                                     Group lstVal By lstVal.IdPlanta Into agrupacion = Group
                                                     Select IdPlanta).ToList()
    Dim ticket As SabLib.ELL.Ticket = CType(Session("Ticket"), SabLib.ELL.Ticket)
    Dim listaValidacionesLinea As List(Of ELL.ValidacionLinea)
End Code

<script type="text/javascript">

    $(function () {
        $("#aprobar").click(function (e) {
            $("#hfIdValidacion").val($(this).data("idvalidacion"));
            $("#hfIdPlanta").val($(this).data("idplanta"));

            return confirm('@Html.Raw(Utils.Traducir("¿Desea aprobar los pasos seleccionados?"))');
        });

        $("#rechazar").click(function (e) {
            $("#hIdValidacionRechazar").val($(this).data("idvalidacion"));
            $("#hfIdPlantaRechazar").val($(this).data("idplantaRechazar"));
            $('#modalWindowRechazar').modal('show');
        });
    });

</script>

<h3>@Utils.Traducir("Gestionar validación pasos")</h3>
<hr />

@code
    Dim listaIdsCostGroupsUnicos As List(Of Integer) = Nothing
    Dim BACGastosSuma As Integer = Integer.MinValue
    Dim PBCSuma As Integer = Integer.MinValue
    Dim marginSuma As String = String.Empty
    Dim costGroup As ELL.CostGroup

    If (validaciones.Count > 0) Then
        @For Each idPlanta In listaIdsPlantasUnicos
            @Using Html.BeginForm("AprobarValidacion", "Validaciones", FormMethod.Post, New With {.class = "form-horizontal"})
                @Html.Hidden("hfIdValidacion")
                @Html.Hidden("hfIdPlanta")
                @<div Class="form-group">
                    <label class="col-sm-2 label label-success text-uppercase">@validaciones.FirstOrDefault(Function(f) f.IdPlanta = idPlanta).Planta</label>
                </div>
                @For Each validacion In validaciones.Where(Function(f) f.IdPlanta = idPlanta)
                    @<div Class="form-group">
                        <label class="col-sm-2 control-label">@Utils.Traducir("Denominación del presupuesto")</label>
                        @code
                            ' Miramos si dentro de una validación queda algún estp por validar o rechazar
                            listaValidacionesLinea = BLL.ValidacionesLineaBLL.CargarListadoUltimoPorValidacion(validacion.Id).Where(Function(f) f.IdEstadoValidacion = ELL.Validacion.Estado.Waiting_for_approval AndAlso f.IdUser <> ticket.IdUser AndAlso f.IdPlanta = idPlanta).ToList()
                            If (listaValidacionesLinea IsNot Nothing AndAlso listaValidacionesLinea.Count > 0) Then
                                @<input type="submit" id="aprobar" value="@Utils.Traducir("Aprobar")" Class="btn btn-success aprobar" data-idvalidacion="@validacion.Id" data-idplanta="@validacion.IdPlanta">
                                @<input type="button" id="rechazar" value="@Utils.Traducir("Rechazar")" Class="btn btn-danger rechazar" data-idvalidacion="@validacion.Id" data-idplanta="@validacion.IdPlanta" />
                            End If
                        End code
                        <div class="col-sm-4">
                            <label>@validacion.Denominacion</label>
                        </div>
                    </div>
                    @<div Class="form-group">
                        <label class="col-sm-2 control-label">@Utils.Traducir("Descripción y objeto del presupuesto")</label>
                        <div class="col-sm-4">
                            <label>@validacion.Descripcion</label>
                        </div>
                    </div>
                    @<div Class="form-group">
                        <label class="col-sm-2 control-label">@Utils.Traducir("Razones economicas")</label>
                        <div class="col-sm-4">
                            <label>@validacion.RazonEconomica</label>
                        </div>
                    </div>
                    @<div Class="form-group">
                        <label class="col-sm-2 control-label">@Utils.Traducir("Prevista en PG")</label>
                        <div class="col-sm-4">
                            @Html.RadioButton("previstoPg", 1, validacion.PrevistoPG, New With {.disabled = True})&nbsp;<label>@Utils.Traducir("Si")</label>
                            @Html.RadioButton("previstoPg", 0, Not validacion.PrevistoPG, New With {.disabled = True})&nbsp;<label>@Utils.Traducir("No")</label>
                        </div>
                    </div>
                    @<div Class="form-group">
                        <label class="col-sm-2 control-label">@Utils.Traducir("Resumen del presupuesto")</label>
                        <div Class="form-group">
                            <div Class="col-sm-5">
                                <Table id="tabla" Class="table table-bordered table-condensed">
                                    <thead>
                                        <tr>
                                            <th>@Html.ActionLink("Ver detalle pasos", "Detalle", "Validaciones", New With {.idValidacion = validacion.Id, .idPlanta = validacion.IdPlanta}, Nothing)</th>
                                            <th Class="danger" style="text-align:center;">@Utils.Traducir("Horas")</th>
                                            <th Class="danger" style="text-align:center;">@Utils.Traducir("Coste")</th>
                                            <th Class="info" style="text-align:center;">@Utils.Traducir("Pagado por cliente")</th>
                                            <th Class="info" style="text-align:center;">@Utils.Traducir("Margen") %</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        @code
                                            ' Vamos a obtener los distintos cost group de los steps
                                            listaIdsCostGroupsUnicos = (From lstVal In validacion.ValidacionesLinea
                                                                        Group lstVal By lstVal.IdCostGroup Into agrupacion = Group
                                                                        Select IdCostGroup).ToList()

                                            For Each idCostGroup In listaIdsCostGroupsUnicos
                                                BACGastosSuma = validacion.ValidacionesLinea.Where(Function(f) f.IdCostGroup = idCostGroup).Sum(Function(f) f.BudgetApproved)
                                                PBCSuma = validacion.ValidacionesLinea.Where(Function(f) f.IdCostGroup = idCostGroup).Sum(Function(f) f.PaidByCustomer)

                                                If (BACGastosSuma = 0) Then
                                                    marginSuma = CInt(100).ToString("N2", System.Globalization.CultureInfo.CreateSpecificCulture("es-ES"))
                                                ElseIf (PBCSuma = 0) Then
                                                    marginSuma = "NA"
                                                Else
                                                    marginSuma = (BACGastosSuma / PBCSuma).ToString("N2", System.Globalization.CultureInfo.CreateSpecificCulture("es-ES"))
                                                End If

                                                @<tr>
                                                    <td Class="active">
                                                        @code
                                                            costGroup = BLL.CostsGroupBLL.Obtener(idCostGroup)
                                                        End code
                                                        @String.Format("{0} ({1})", costGroup.Descripcion, costGroup.Estado)
                                                    </td>
                                                    <td align="right">0</td>
                                                    <td align="right">@BACGastosSuma.ToString("N0", culturaEsES)</td>
                                                    <td align="right">@PBCSuma.ToString("N0", culturaEsES)</td>
                                                    <td align="right">@marginSuma</td>
                                                </tr>
                                                            Next
                                        end code
                                        <tr>
                                            <td colspan="5"></td>
                                        </tr>
                                    </tbody>
                                </Table>
                            </div>
                        </div>


                    </div>
                    @<hr />
                                                            Next

                                                            End Using

                                                            Next
    Else
        @Html.Label(String.Empty, Utils.Traducir("noExisteNingunRegistro"))
                                                            End If

End code

<div class="modal fade" id="modalWindowRechazar" tabindex="-1" role="dialog" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">@Utils.Traducir("Rechazar pasos")</h4>
            </div>
            <div class="modal-body">
                @Using Html.BeginForm("RechazarValidacion", "Validaciones", FormMethod.Post)
            @Html.Hidden("hIdValidacionRechazar")
            @Html.Hidden("hIdPlantaRechazar")
            @<div Class="form-group">
                <label class="control-label">@Utils.Traducir("Motivo del rechazo")</label>

                @Html.TextArea("txtMotivo", String.Empty, New With {.class = "form-control", .required = "required", .maxlength = "200", .rows = "3"})

            </div>
            @<div Class="form-group">
                <input type="submit" id="btnConfirmRechazo" value="@Utils.Traducir("Rechazar")" class="btn btn-primary input-block-level form-control" />
            </div>
                End Using
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-dismiss="modal">@Utils.Traducir("Cerrar")</button>
            </div>
        </div>
    </div>
</div>