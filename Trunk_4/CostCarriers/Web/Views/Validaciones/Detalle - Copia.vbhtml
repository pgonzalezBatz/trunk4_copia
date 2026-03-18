@Imports CostCarriersLib

@Code
    Dim steps As List(Of ELL.Step) = CType(ViewData("Steps"), List(Of ELL.Step))
    Dim culturaEsES As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture("es-ES")
    Dim ticket As SabLib.ELL.Ticket = CType(Session("Ticket"), SabLib.ELL.Ticket)
    Dim mostrarbotones As Boolean = CBool(ViewData("MostrarBotones"))
End Code

<script type="text/javascript">

    $(function () {

        function ComprobarCheckboxes(e) {
            //Vamos a verificar que se han marcado steps para validar
            //Tenemos que comparar su valor con el sumatorio de todos los textbox con data-pbcvalor y mismo idStep
            var suma = 0
            $(".checkbox:checked" ).each(function () {
                suma = suma + 1;
            })

            if (suma == 0) {
                alert('@Html.Raw(Utils.Traducir("Debe marcar algún step para aprobar o rechazar"))');
                e.preventDefault();
                return false;
            }

            return true;
        }

        $("#aprobar").click(function (e) {
            if (ComprobarCheckboxes(e)) {
                $(".checkbox:checked").each(function () {
                    $('#hIdValidacionesLineaAprob').val($('#hIdValidacionesLineaAprob').val() + '-' + $(this).data('idvalidacionlinea'));
                })
                return confirm('@Html.Raw(Utils.Traducir("¿Desea aprobar los pasos seleccionados?"))');
            }
            else {
                $('#hIdValidacionesLineaAprob').val('');
                return false;
            }
        });

        $("#rechazar").click(function (e) {
            if (ComprobarCheckboxes(e)) {
                $(".checkbox:checked").each(function () {
                    $('#hIdValidacionesLineaRechaz').val($('#hIdValidacionesLineaRechaz').val() + '-' + $(this).data('idvalidacionlinea'))
                })

                $('#modalWindowRechazar').modal('show');
            }
        });
    });

</script>

<h3>@Utils.Traducir("Gestionar validación pasos - Detalle")</h3>
<hr />

@code
    @Using Html.BeginForm("Aprobar", "Validaciones", FormMethod.Post)
        @Html.Hidden("hIdValidacionesLineaAprob")

        If (steps.Count > 0) Then
            If (mostrarbotones) Then
                @<input type="submit" id="aprobar" value="@Utils.Traducir("Aprobar")" Class="btn btn-success" />
                @<input type="button" id="rechazar" value="@Utils.Traducir("Rechazar")" Class="btn btn-danger" />
                @<br />
                @<br />
            End If

            @<table id="tabla" class="table table-bordered table-condensed">
                <thead>
                    <tr>
                        <th></th>
                        <th Class="info" style="text-align:center;">@Utils.Traducir("Nombre")</th>
                        <th Class="danger" style="text-align:center;">@Utils.Traducir("Presupuesto aprobado")</th>
                        <th Class="info" style="text-align:center;">@Utils.Traducir("Dinero pagado por cliente")</th>
                        <th Class="info" style="text-align:center;">@Utils.Traducir("Margen") %</th>
                        <th Class="warning" style="text-align:center;">@Utils.Traducir("Estado")</th>
                    </tr>
                </thead>
                <tbody>
                    @For Each paso In steps
                        Dim nombre As String = BLL.ValidacionesLineaBLL.ObtenerPorStep(paso.Id).Nombre

                        paso.CargarValoresStepValidacion()

                        @<tr>
                            <td align="center">
                                @code
                                    If (paso.IdUsuarioValidador <> ticket.IdUser AndAlso paso.IdEstadoValidacion = ELL.Validacion.Estado.Waiting_for_approval) Then
                                        @Html.CheckBox("chkStep-" & CStr(paso.Id), New With {.class = "checkbox", .data_idvalidacionlinea = paso.IdValidacionLinea})
                                    End If
                                End code
                            </td>

                            <td><label class="control-label">@nombre</label></td>
                            <td align="right"><label class="control-label">@paso.BACGastosValidacion.ToString("N0", culturaEsES)</label></td>
                            <td align="right"><label class="control-label">@paso.PBCValidacion.ToString("N0", culturaEsES)</label></td>
                            <td align="right"><label class="control-label">@paso.MarginValidacion</label></td>
                            <td style="text-align:center;"><label class="control-label">@paso.EstadoValidacion</label></td>
                        </tr>
                                    Next
                    <tr><td colspan="6"></td></tr>
                </tbody>

            </table>
                                    Else
            @Html.Label(String.Empty, Utils.Traducir("noExisteNingunRegistro"))
                                        End If

                                    End Using

End code

<div class="modal fade" id="modalWindowRechazar" tabindex="-1" role="dialog" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">@Utils.Traducir("Rechazar pasos")</h4>
            </div>
            <div class="modal-body">
                @Using Html.BeginForm("Rechazar", "Validaciones", FormMethod.Post)
                    @Html.Hidden("hIdValidacionesLineaRechaz")
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