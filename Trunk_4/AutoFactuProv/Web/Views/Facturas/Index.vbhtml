@Imports AutoFactuProvLib

@code
    Dim facturas As IEnumerable(Of ELL.FacturaProv) = CType(ViewData("Facturas"), IEnumerable(Of ELL.FacturaProv))
End Code

<script type="text/javascript">
    $(function () {
        $(".lineas-factura").click(function () {            
            var idFacturaProv = $(this).data('idfacturaprov');
            var numfactura = $(this).data('numfactura');
            $("#modalWindowFacturas").find('.modal-title').text('@Utils.Traducir("Lineas de la factura ")' + numfactura);
            $('#modalBodyFacturas').html('@Utils.Traducir("Cargando")...');
            $("#modalWindowFacturas").modal('show');
            $('#modalBodyFacturas').load('@Url.Action("Index", "LineasFactura")' + '?idFacturaProv=' + idFacturaProv);
        });
    })
</script>

<h3>@Utils.Traducir("Facturas emitidas por proveedor")</h3>
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
                        <th class="text-center">@Utils.Traducir("Líneas")</th>
                    </tr>
                </thead>
                <tbody>
                    @For indice = 0 To facturas.Count - 1
                        @<tr>
                            <td class="text-right">@facturas(indice).NumFactura</td>
                            <td class="text-center">@facturas(indice).FechaAlta.ToShortDateString()</td>                            
                            <td class="text-center"><span class="lineas-factura glyphicon glyphicon glyphicon-th-list" style="cursor:pointer;" aria-hidden="true" title="@Utils.Traducir("Lineas")" data-idfacturaprov="@facturas(indice).Id" data-numfactura="@facturas(indice).NumFactura"></span></td>
                        </tr>
                    Next
                </tbody>
            </table>
        </div>
    </div>
End If

<div class="modal fade bd-example-modal-lg" id="modalWindowFacturas" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-lg" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">Líneas</h4>
            </div>
            <div id="modalBodyFacturas" class="modal-body">
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-dismiss="modal">@Utils.Traducir("Cerrar")</button>
            </div>
        </div>
    </div>
</div>


