@Imports CostCarriersLib

@Code
    Dim pedido As ELL.Pedido = CType(ViewData("Pedido"), ELL.Pedido)
    Dim lineasPedido As List(Of ELL.LineaPedido) = CType(ViewData("LineasPedido"), List(Of ELL.LineaPedido))
    Dim culturaEsES As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture("es-ES")
    Dim plantasBLL As New SabLib.BLL.PlantasComponent
End Code

<script type="text/javascript">
    $(function () {
    })
</script>

<h3><label>@Utils.Traducir("Gestionar facturación pasos") - @lineasPedido.First.NombreProyecto - @lineasPedido.First.NumPedido </label></h3>
<hr />

@code
    Dim clase As String = String.Empty
    Dim texto As String = String.Empty
    Dim estado As String = String.Empty
    If (lineasPedido.Count > 0) Then
        @Using Html.BeginForm("Facturar", "Comercial", FormMethod.Post, New With {.class = "form-horizontal"})
            @Html.Hidden("hfIdPedido", lineasPedido.First.IdPedido)
            @Html.Hidden("hfIdCabecera", lineasPedido.First.IdCabecera)
            @code
                ' Sólo mostramos el botón de facturar si hay alguna linea de pedido sin numero de factura
                if (lineasPedido.Exists(Function(f) String.IsNullOrEmpty(f.NumFactura))) Then
                    @<input type="submit" id="submit" value="@Utils.Traducir("Facturar")" Class="btn btn-primary" />
                    @Html.Label(Utils.Traducir("Sólo las líneas de pedido con número de factura serán marcadas como facturadas"), New With {.style = "font-style: oblique;"})
                    @<br />
                    @<br />
                End If
            End code

            @<div class="row">
                <div class="col-sm-12">
                    <table id="tabla" class="table table-condensed table-striped table-hover">
                        <thead>
                            <tr>
                                <th>@Utils.Traducir("Planta")</th>
                                <th>@Utils.Traducir("Grupo de coste")</th>
                                <th>@Utils.Traducir("Portador")</th>
                                <th>@Utils.Traducir("Localización definitiva del medio")</th>
                                <th>@Utils.Traducir("Items")</th>
                                <th class="text-right">%</th>
                                <th class="text-right">@Utils.Traducir("Importe")</th>
                                <th class="text-center">@Utils.Traducir("Moneda")</th>
                                <th class="text-center">@Utils.Traducir("Estado")</th>
                                <th>@Utils.Traducir("Número factura")</th>
                            </tr>
                        </thead>
                        <tbody>
                            @For Each linea In lineasPedido
                                @<tr>
                                     <td>
                                         @code
                                             Dim nombrePlanta As String = String.Empty
                                             If (linea.IdPlantaSAB = 0) Then
                                                 nombrePlanta = Utils.Traducir("Corporativo")
                                             Else
                                                 nombrePlanta = plantasBLL.GetPlanta(linea.IdPlantaSAB).Nombre
                                             End If
                                             If linea.CodigoPais.Equals(0) Then
                                                 linea.Pais = "NOT APPLICABLE"
                                             End If
                                         End code
                                         @nombrePlanta
                                     </td>
                                    <td>@BLL.CostsGroupBLL.Obtener(BLL.StepsBLL.Obtener(linea.IdStep).IdCostGroup).Descripcion</td>
                                    <td>@linea.CostCarrier - @linea.NombreStep</td>
                                    <td>@Utils.Traducir(linea.Pais)</td>
                                    <td>@linea.Posiciones</td>
                                    <td class="text-right">@linea.Porcentaje</td>
                                    <td class="text-right">@linea.Importe.ToString("N2", culturaEsES)</td>
                                    <td class="text-center">@linea.MonedaFacturacion</td>
                                    <td class="text-center">@linea.EstadoFacturacion</td>
                                    @code
                                        If (String.IsNullOrEmpty(linea.NumFactura)) Then
                                            @<td>@Html.TextBox("txtNumFactura-" & linea.Id, linea.NumFactura, New With {.Class = "form-control", .maxlength = 20})</td>
                                        Else
                                            @<td>@linea.NumFactura</td>
                                        End If
                                    End code
                                </tr>

                                        Next
                        </tbody>
                    </table>
                </div>
            </div>
                                        End Using
                                        Else
        @Html.Label(String.Empty, Utils.Traducir("noExisteNingunRegistro"))
                                        End If


    @<div Class="row">
        <label class="col-sm-1 col-form-label">@Utils.Traducir("Instrucciones adicionales")</label>
        <div class="col-sm-11">
            @Html.TextArea("comentarios", pedido.Comentarios, New With {.maxlength = "1000", .rows = "5", .Class = "form-control"})
        </div>
    </div>
End code