@Imports CostCarriersLib

@Code
    Dim lineasPedido As List(Of ELL.LineaPedido) = CType(ViewData("LineasPedido"), List(Of ELL.LineaPedido))
    ' Tenemos que agrupas las lineas por proyecto
    Dim pedidos = From lp In lineasPedido
                  Group By IdPedido = lp.IdPedido, NumPedido = lp.NumPedido, ImporteTotal = lp.ImporteTotal, Moneda = lp.MonedaFacturacion
                  Into lineas = Group, Count()
    Dim culturaEsES As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture("es-ES")
End Code

<script type="text/javascript">
    $(function () {       
    })
</script> 

<h3><label>@Utils.Traducir("Gestionar facturación pasos") - @lineasPedido.First.NombreProyecto</label></h3>
<hr />

@code
    Dim clase As String = String.Empty
    Dim texto As String = String.Empty
    Dim estado As String = String.Empty
    Dim facturar As Boolean = False
    If (lineasPedido.Count > 0) Then
        @<div class="row">
            <div class="col-sm-10">
                <table id="tabla" class="table table-condensed table-striped table-hover">
                    <thead>
                        <tr>
                            <th>@Utils.Traducir("Pedido")</th>
                            <th>@Utils.Traducir("Estado")</th>
                            <th class="text-right">@Utils.Traducir("Importe total")</th>
                            <th class="text-right">@Utils.Traducir("Importe facturado")</th>
                            <th class="text-center">@Utils.Traducir("Moneda")</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>                        
                        @For Each pedido In pedidos
                            @code
                                ' Si hay algun paso para facturas se queda en estado Sent to invoice sino invoiced
                                if(lineasPedido.Where(Function(f) f.IdPedido = pedido.IdPedido).ToList().ToList().Exists(Function(f) f.IdEstadoFacturacion = ELL.Pedido.EstadoFacturacion.Sent_to_invoice)) Then
                                    estado = Utils.Traducir("Pasos para facturar")
                                    facturar = True
                                Else
                                    estado = Utils.Traducir("Facturado")
                                    facturar = False
                                End If
                            End code
                            @<tr>
                                <td>@pedido.NumPedido</td>
                                <td>@estado</td>
                                <td class="text-right">@pedido.ImporteTotal.ToString("N2", culturaEsES)</td>
                                <td class="text-right">@pedido.lineas.Where(Function(f) f.IdEstadoFacturacion = ELL.Pedido.EstadoFacturacion.Invoiced).Sum(Function(f) f.Importe).ToString("N2", culturaEsES)</td>
                                <td Class="text-center">@pedido.Moneda</td>
                                <td Class="text-center">
                                            @code
                                                If (facturar) Then
                                                    clase = "text-danger"
                                                    texto = "<strong>" & Utils.Traducir("Pasos para facturar") & "</strong>"
                                                Else
                                                    clase = "glyphicon glyphicon-th-list"
                                                    texto = String.Empty
                                                End If
                                            End code
                                            <a href='@Url.Action("DetallePedido", "Comercial", New With {.idPedido = pedido.IdPedido})'>
                                                <span class="@clase" aria-hidden="true" title="@Utils.Traducir("Ir a detalle de pedido")">@Html.Raw(texto)</span>
                                            </a>
                                        </td>
                            </tr>

                                                Next
                    </tbody>
                            </table>
                        </div>
                    </div>
                                        Else
                                    @Html.Label(String.Empty, Utils.Traducir("noExisteNingunRegistro"))
                                        End If
                                End code