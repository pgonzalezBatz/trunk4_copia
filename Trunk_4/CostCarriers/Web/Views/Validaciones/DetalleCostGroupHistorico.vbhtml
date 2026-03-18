@Imports CostCarriersLib

@Code
    Dim validacionesLinea As List(Of ELL.ValidacionLinea) = CType(ViewData("ValidacionesLinea"), List(Of ELL.ValidacionLinea))
    Dim paso As ELL.Step = BLL.StepsBLL.Obtener(validacionesLinea.First.IdStep)
    Dim cabeceraProyecto As ELL.CabeceraCostCarrier = CType(ViewData("CabeceraProyecto"), ELL.CabeceraCostCarrier)
    Dim costGroup As ELL.CostGroup = BLL.CostsGroupBLL.Obtener(validacionesLinea.First.IdCostGroup)
    Dim culturaEsES As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture("es-ES")
End Code

<script type="text/javascript">

    $(function () {
    });

</script>

<h3><label>@ViewData("Titulo")</label></h3>
<hr />

@code
    Dim BACV As Integer = 0
    Dim PBCV As Integer = 0
    Dim marginSumaV As String = String.Empty
    Dim marginSumaUltimoAprobado As String = String.Empty
    Dim delta As String = String.Empty

    @Html.ActionLink(Utils.Traducir("Volver a proyecto"), "DetalleProyectoHistorico", New With {.idCabecera = cabeceraProyecto.Id, .idValidacion = validacionesLinea.First.IdValidacion})
    @<br />
    @<br />

    @<form>
        <table id="tabla" class="table table-bordered table-condensed">
            <thead>
                <tr>
                    <th></th>
                    <th colspan="2" style="text-align:center;font-weight:bold">@Utils.Traducir("Nueva petición")</th>
                    <th colspan="3" style="text-align:center;font-weight:bold;text-transform:uppercase; color:#ffffff;background-color:#000000;">@Utils.Traducir("Nuevo estado total")</th>
                    <th></th>
                    <th></th>
                </tr>
                <tr>
                    <th Class="info" style="text-align:center;">@Utils.Traducir("Nombre")</th>
                    <th Class="danger" style="text-align:center;">@Utils.Traducir("Presupuesto aprobado")</th>
                    <th Class="info" style="text-align:center;">@Utils.Traducir("Dinero pagado por cliente")</th>

                    <th Class="danger" style="text-align:center;">@Utils.Traducir("Presupuesto aprobado")</th>
                    <th Class="info" style="text-align:center;">@Utils.Traducir("Dinero pagado por cliente")</th>
                    <th Class="info" style="text-align:center;">@Utils.Traducir("Margen") %</th>
                    <th style="text-align:center;">@Utils.Traducir("Delta")</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                @For Each validacionLinea In validacionesLinea.OrderByDescending(Function(f) f.Id).ToList()
                    @code
                        ' Sacamos todos los estados por los que ha pasado ese paso en ese grupo de validación
                        Dim historicoEstados As List(Of ELL.HistoricoEstadoLinea) = BLL.HistoricosEstadoLineaBLL.CargarListadoPorValidacionLinea(validacionLinea.Id).OrderByDescending(Function(f) f.Fecha).ToList()

                        Dim marginSuma As String = String.Empty
                        If (validacionLinea.PaidByCustomer = 0) Then
                            marginSuma = "NA"
                        Else
                            marginSuma = (((validacionLinea.PaidByCustomer - validacionLinea.BudgetApproved) / validacionLinea.PaidByCustomer) * 100).ToString("N2", culturaEsES)
                        End If

                        ' Hay que obtener la anterior validación linea a la validación que estamos mirando
                        Dim valLineaAnterior As ELL.ValidacionLinea = BLL.ValidacionesLineaBLL.ObtenerAnteriorAprobada(validacionLinea)

                        If (valLineaAnterior IsNot Nothing) Then
                            BACV = validacionLinea.BudgetApproved - valLineaAnterior.BudgetApproved
                            PBCV = validacionLinea.PaidByCustomer - valLineaAnterior.PaidByCustomer
                        End If

                        If (PBCV = 0) Then
                            marginSumaV = "NA"
                        Else
                            marginSumaV = (((PBCV - BACV) / PBCV) * 100).ToString("N2", culturaEsES)
                        End If

                        If (valLineaAnterior.PaidByCustomer = 0) Then
                            marginSumaUltimoAprobado = "NA"
                        Else
                            marginSumaUltimoAprobado = String.Empty
                        End If

                        If (marginSuma <> "NA" AndAlso marginSumaUltimoAprobado <> "NA") Then
                            delta = ((((validacionLinea.PaidByCustomer - validacionLinea.BudgetApproved) / validacionLinea.PaidByCustomer) * 100) - (((valLineaAnterior.PaidByCustomer - valLineaAnterior.BudgetApproved) / valLineaAnterior.PaidByCustomer) * 100)).ToString("N2", culturaEsES)
                        Else
                            delta = "NA"
                        End If
                    End code
                    @<tr>
                        <td data-idStep="@validacionLinea.IdStep">@validacionLinea.Nombre</td>
                        <td align="right">@BACV.ToString("N0", culturaEsES)</td>
                        <td align="right">@PBCV.ToString("N0", culturaEsES)</td>

                        <td align="right">@validacionLinea.BudgetApproved.ToString("N0", culturaEsES)</td>
                        <td align="right">@validacionLinea.PaidByCustomer.ToString("N0", culturaEsES)</td>
                        <td align="right">@marginSuma</td>
                        <td align="right">@delta</td>
                        <td>
                            @code
                                If (historicoEstados IsNot Nothing AndAlso historicoEstados.Count > 0) Then
                                    @<table id="tabla" Class="table table-condensed table-hover">
                                        <thead>
                                            <tr>
                                                <th Class="warning" style="text-align:center;">@Utils.Traducir("Usuario")</th>
                                                <th Class="warning" style="text-align:center;">@Utils.Traducir("Fecha")</th>
                                                <th Class="warning" style="text-align:center;">@Utils.Traducir("Acción")</th>
                                                <th Class="warning" style="text-align:center;">@Utils.Traducir("Estado")</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            @For Each historico In historicoEstados.OrderByDescending(Function(f) f.Fecha).ToList()
                                    @<tr>
                                        <td>@historico.Usuario</td>
                                        <td>@historico.Fecha.ToString()</td>
                                        <td>@historico.AccionValidacion</td>
                                        <td>
                                            @historico.EstadoValdiacion
                                            @code

                                                If (historico.Equals(historicoEstados.First)) Then
                                                    Dim validador As String = String.Empty
                                                    If (historico.IdEstadoValidacion = ELL.Validacion.Estado.Waiting_for_approval) Then
                                                        Dim flujoAprob As List(Of ELL.FlujoAprobacion) = BLL.FlujosAprobacionBLL.CargarListadoPorValidacionLinea(historico.IdValidacionLinea).OrderBy(Function(f) f.Orden).ToList()

                                                        If (flujoAprob IsNot Nothing AndAlso flujoAprob.Count > 0) Then
                                                            validador = String.Format("{0} {1} {2}", flujoAprob(0).Nombre, flujoAprob(0).Apellido1, flujoAprob(0).Apellido2)
                                                            @<span Class="glyphicon glyphicon-info-sign text-alert" aria-hidden="true" title="@validador"></span>
                                                        End If
                                                    End If
                                                End If

                                                If (historico.IdEstadoValidacion = ELL.Validacion.Estado.Rejected) Then
                                                    @<span Class="glyphicon glyphicon-info-sign text-alert" aria-hidden="true" title="@historico.Comentarios"></span>
                                                End If
                                            End code

                                        </td>
                                    </tr>
                                                Next
                                        </tbody>
                                    </table>
                                                End If
                            End code
                        </td>
                    </tr>
                                                Next
            </tbody>
        </table>
    </form>
End code
