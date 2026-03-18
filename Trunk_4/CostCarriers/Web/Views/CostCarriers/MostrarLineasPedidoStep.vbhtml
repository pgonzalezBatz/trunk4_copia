@Imports CostCarriersLib

@Code
    Layout = Nothing
    Dim lineasPedido As List(Of ELL.LineaPedido) = CType(ViewData("LineasPedido"), List(Of ELL.LineaPedido))
    Dim culturaEsES As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture("es-ES")
End code

<h3><label>@Utils.Traducir("Líneas de pedido") - @Utils.Traducir("Portador"): @lineasPedido.First.CostCarrier - @lineasPedido.First.NombreStep</label></h3>
<hr />


@code
    If (lineasPedido IsNot Nothing AndAlso lineasPedido.Count > 0) Then
        @<table id="tabla" class="table table-condensed table-striped table-hover">
            <thead>
                <tr>
                    <th>@Utils.Traducir("Pedido")</th>
                    <th>@Utils.Traducir("Items")</th>
                    <th class="text-right">%</th>
                    <th class="text-right">@Utils.Traducir("Importe")</th>
                    <th class="text-center">@Utils.Traducir("Estado")</th>
                    <th>@Utils.Traducir("Factura")</th>
                </tr>
            </thead>
            <tbody>
                @For Each linea In lineasPedido
                    @<tr>
                        <td>@linea.NumPedido</td>
                        <td>@linea.Posiciones</td>
                        <td class="text-right">@linea.Porcentaje</td>
                        <td class="text-right">@linea.Importe.ToString("N2", culturaEsES)</td>
                        <td class="text-center">@Utils.Traducir(linea.EstadoFacturacion)</td>
                        <td>@linea.NumFactura</td>
                    </tr>
                Next
            </tbody>
        </table>
    End If

End code
