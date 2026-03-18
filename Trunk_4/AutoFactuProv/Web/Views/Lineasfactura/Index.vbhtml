@Imports AutoFactuProvLib

@code
    Layout = Nothing
    Dim lineas As IEnumerable(Of ELL.LineasFacturaProv) = CType(ViewData("LineasFactura"), IEnumerable(Of ELL.LineasFacturaProv))
End Code

<link href="~/Content/bootstrap.css" rel="stylesheet" />

@If (lineas.Count = 0) Then
    @Html.Label(Utils.Traducir("noExisteNingunRegistro"))
Else
        @<table class="table table-striped table-responsive table-hover table-condensed">
            <thead>
                <tr>
                    <th class="text-right">@Utils.Traducir("Albaran")</th>
                    <th class="text-right">@Utils.Traducir("Pedido")</th>
                    <th class="text-right">@Utils.Traducir("Linea")</th>
                    <th>@Utils.Traducir("Concepto")</th>
                    <th class="text-right">@Utils.Traducir("Cantidad recepcionada")</th>
                    <th class="text-right">@Utils.Traducir("Precio unitario")</th>
                    <th>@Utils.Traducir("Moneda")</th>
                </tr>
            </thead>
            <tbody>
                @For indice = 0 To lineas.Count - 1

                @<tr>
                    <td class="text-right">@lineas(indice).Albaran</td>
                    <td class="text-right">@lineas(indice).Pedido</td>
                    <td class="text-right">@lineas(indice).Linea</td>
                    <td>@lineas(indice).AlbaranBRAIN.Concepto</td>
                    <td class="text-right">@lineas(indice).AlbaranBRAIN.CantRecibida</td>
                    <td class="text-right">@lineas(indice).AlbaranBRAIN.PrecioUnitario</td>
                    <td>@lineas(indice).AlbaranBRAIN.Moneda</td>
                </tr>
                Next
            </tbody>
        </table>
End If