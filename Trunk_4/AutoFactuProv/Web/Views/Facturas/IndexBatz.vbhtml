@Imports AutoFactuProvLib

@code
    Dim facturas As IEnumerable(Of ELL.Factura) = CType(ViewData("Facturas"), IEnumerable(Of ELL.Factura))
End Code

<h3>@Utils.Traducir("Facturas emitidas por Batz")</h3>
<hr />

@If (facturas.Count = 0) Then
    @Html.Label(Utils.Traducir("noExisteNingunRegistro"))
Else
    @<div class="row">
        <div class="col-sm-6">
            <table class="table table-striped table-responsive table-hover table-condensed">
                <thead>
                    <tr>
                        <th class="text-right">@Utils.Traducir("Factura")</th>
                        <th class="text-center">@Utils.Traducir("Fecha alta")</th>                        
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    @For indice = 0 To facturas.Count - 1
                        @<tr>
                            <td class="text-right">@facturas(indice).NumFactura</td>
                            <td class="text-center">@facturas(indice).FechaAlta.ToShortDateString()</td>                            
                            <td class="text-center">                                
                                <a href='@Url.Action("MostrarFactura", "Facturas", New With {.fichero = facturas(indice).NombreFicheroFactura})'><span class="glyphicon glyphicon-download-alt" style="cursor:pointer;" aria-hidden="true" title="@Utils.Traducir("Descargar factura")"></span></a>
                            </td>
                        </tr>
                    Next
                </tbody>
            </table>
        </div>
    </div>
End If