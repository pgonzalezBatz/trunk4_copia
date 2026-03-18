@Imports CostCarriersLib

@Code
    Dim steps As List(Of ELL.Step) = CType(ViewData("Steps"), List(Of ELL.Step))
    Dim culturaEsES As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture("es-ES")
    Dim ticket As SabLib.ELL.Ticket = CType(Session("Ticket"), SabLib.ELL.Ticket)
    Dim mostrarbotones As Boolean = CBool(ViewData("MostrarBotones"))
    Dim cabeceraProyecto As ELL.CabeceraCostCarrier = CType(ViewData("CabeceraProyecto"), ELL.CabeceraCostCarrier)
    'Dim costGroup As ELL.CostGroup = BLL.CostsGroupBLL.Obtener(steps.First.IdCostGroup)
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
                return confirm('@Html.Raw(Utils.Traducir("¿Realmente desea aprobar los pasos seleccionados?"))');
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

        $("#chkSelectAll").change(function () {
            $(":checkbox[name^='chkStep-']").prop('checked', $(this).is(":checked"));
        });
    });

</script>

<h3><label>@ViewData("Titulo")</label></h3>
<hr />

@code
    Dim validacionLinea As ELL.ValidacionLinea = Nothing
    Dim marginSuma As String = String.Empty
    Dim BACV As Integer = 0
    Dim PBCV As Integer = 0
    Dim marginSumaV As String = String.Empty
    Dim marginSumaUltimoAprobado As String = String.Empty
    Dim delta As String = String.Empty
    @Html.ActionLink(Utils.Traducir("Volver a proyecto"), "DetalleProyecto", New With {.idCabecera = cabeceraProyecto.Id})
    @<br />
    @<br />

    If (steps.Count > 0) Then
        Dim stepsValidados As List(Of ELL.Step) = BLL.StepsBLL.CargarListadoValidados(steps.First.IdCostGroup, ticket.IdUser)
        mostrarbotones = steps.Where(Function(f) f.IdEstadoValidacion = ELL.Validacion.Estado.Waiting_for_approval).Select(Function(f) f.Id).Except(stepsValidados.Select(Function(f) f.Id)).Count > 0

        @Using Html.BeginForm("Aprobar", "Validaciones", FormMethod.Post)
            @Html.Hidden("hIdValidacionesLineaAprob")

            If (mostrarbotones) Then
                @<input type="submit" id="aprobar" value="@Utils.Traducir("Aprobar")" Class="btn btn-success" />
                @<input type="button" id="rechazar" value="@Utils.Traducir("Rechazar")" Class="btn btn-danger" />
                @<br />
                @<br />
            End If

            @<table id="tabla" class="table table-bordered table-condensed">
                <thead>
                    <tr>
                        <th colspan="2"></th>
                        <th colspan="2" style="text-align:center;font-weight:bold">@Utils.Traducir("Nueva petición")</th>
                        <th colspan="3" style="text-align:center;font-weight:bold;text-transform:uppercase; color:#ffffff;background-color:#000000;">@Utils.Traducir("Nuevo estado total")</th>
                        <th></th>
                        <th></th>
                    </tr>
                    <tr>
                        <th style="text-align:center;">@Html.CheckBox("chkSelectAll")</th>
                        <th Class="info" style="text-align:center;">@Utils.Traducir("Nombre")</th>
                        <th Class="danger" style="text-align:center;">@Utils.Traducir("Presupuesto aprobado")</th>
                        <th Class="info" style="text-align:center;">@Utils.Traducir("Dinero pagado por cliente")</th>
                        @*<th Class="info" style="text-align:center;">@Utils.Traducir("Margen") %</th>*@
                        <th Class="danger" style="text-align:center;">@Utils.Traducir("Presupuesto aprobado")</th>
                        <th Class="info" style="text-align:center;">@Utils.Traducir("Dinero pagado por cliente")</th>
                        <th Class="info" style="text-align:center;">@Utils.Traducir("Margen") %</th>
                        <th style="text-align:center;">@Utils.Traducir("Delta")</th>
                        <th Class="warning" style="text-align:center;">@Utils.Traducir("Estado validación")</th>
                    </tr>
                </thead>
                <tbody>
                    @For Each paso In steps
                        Dim nombre As String = BLL.ValidacionesLineaBLL.ObtenerPorStep(paso.Id).Nombre

                        ' Cargamos la validaciones línea
                        validacionLinea = BLL.ValidacionesLineaBLL.Obtener(paso.IdValidacionLinea)

                        If (validacionLinea.PaidByCustomer = 0) Then
                            marginSuma = "NA"
                        Else
                            marginSuma = (((validacionLinea.PaidByCustomer - validacionLinea.BudgetApproved) / validacionLinea.PaidByCustomer) * 100).ToString("N2", culturaEsES)
                        End If

                        Dim ultimoAprobado As ELL.ValidacionLinea = BLL.ValidacionesLineaBLL.ObtenerUltimoAprobado(paso.Id)

                        If (ultimoAprobado Is Nothing) Then
                            ultimoAprobado = New ELL.ValidacionLinea()
                        End If

                        If (ultimoAprobado IsNot Nothing) Then
                            BACV = validacionLinea.BudgetApproved - ultimoAprobado.BudgetApproved
                            PBCV = validacionLinea.PaidByCustomer - ultimoAprobado.PaidByCustomer
                        End If

                        If (PBCV = 0) Then
                            marginSumaV = "NA"
                        Else
                            marginSumaV = (((PBCV - BACV) / PBCV) * 100).ToString("N2", culturaEsES)
                        End If

                        If (ultimoAprobado.PaidByCustomer = 0) Then
                            marginSumaUltimoAprobado = "NA"
                        Else
                            marginSumaUltimoAprobado = String.Empty
                        End If

                        If (marginSuma <> "NA" AndAlso marginSumaUltimoAprobado <> "NA") Then
                            delta = ((((validacionLinea.PaidByCustomer - validacionLinea.BudgetApproved) / validacionLinea.PaidByCustomer) * 100) - (((ultimoAprobado.PaidByCustomer - ultimoAprobado.BudgetApproved) / ultimoAprobado.PaidByCustomer) * 100)).ToString("N2", culturaEsES)
                        Else
                            delta = "NA"
                        End If

                        @<tr>
    <td align="center">
        @code
            Dim otroValidador As String = String.Empty
            If (paso.IdEstadoValidacion = ELL.Validacion.Estado.Waiting_for_approval) Then
                If (Not stepsValidados.Exists(Function(f) f.Id = paso.Id)) Then
                    @Html.CheckBox("chkStep-" & CStr(paso.Id), New With {.class = "checkbox", .data_idvalidacionlinea = paso.IdValidacionLinea})
                Else
                    otroValidador = String.Format("{0}", Utils.Traducir("otro(s) validador(es)"))
                End If
            End If
        End code
    </td>

    <td>@nombre</td>
    <td align="right">@BACV.ToString("N0", culturaEsES)</td>
    <td align="right">@PBCV.ToString("N0", culturaEsES)</td>
    @*<td align="right">@marginSumaV</td>*@

    <td align="right">@validacionLinea.BudgetApproved.ToString("N0", culturaEsES)</td>
    <td align="right">@validacionLinea.PaidByCustomer.ToString("N0", culturaEsES)</td>
    <td align="right">@marginSuma</td>
    <td align="right">@delta</td>

    <td style="text-align:center;">
        @paso.EstadoValidacion
        @code If (Not String.IsNullOrEmpty(otroValidador)) Then
                @<span Class="label label-info">&nbsp;@otroValidador</span>
            End If
        end code
    </td>
</tr>
            Next
                </tbody>

            </table>


            End Using
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