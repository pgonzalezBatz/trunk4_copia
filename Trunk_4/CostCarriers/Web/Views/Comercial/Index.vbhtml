@Imports CostCarriersLib

@Code
    Dim lineasPedido As List(Of ELL.LineaPedido) = CType(ViewData("LineasPedido"), List(Of ELL.LineaPedido))

    ' Tenemos que agrupas las lineas por proyecto
    Dim proyectos = From lp In lineasPedido
                    Group By IdCabecera = lp.IdCabecera, CodigoProyecto = lp.CodigoProyecto, NombreProyecto = lp.NombreProyecto, Cliente = lp.Cliente, Moneda = lp.MonedaFacturacion, Owner = lp.Owner
                    Into lineas = Group, Count()
End Code

<script type="text/javascript">
    $(function () {       
    })
</script> 

<h3><label>@Utils.Traducir("Gestionar facturación pasos")</label></h3>
<hr />

@code
    Dim clase As String = String.Empty
    Dim texto As String = String.Empty
    If (lineasPedido.Count > 0) Then
        @<div class="row">
            <div class="col-sm-11">
                <table id="tabla" class="table table-condensed table-striped table-hover">
                    <thead>
                        <tr>
                            <th>@Utils.Traducir("Código proyecto")</th>
                            <th>@Utils.Traducir("Proyecto")</th>
                            <th>@Utils.Traducir("Cliente")</th>
                            <th>@Utils.Traducir("Moneda")</th>
                            <th>@Utils.Traducir("Propietario")</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>                        
                        @For Each proyecto In proyectos.OrderByDescending(Function(f) f.lineas.Where(Function(g) g.IdEstadoFacturacion = ELL.Pedido.EstadoFacturacion.Sent_to_invoice).Count).ThenBy(Function(f) f.NombreProyecto)
                            @<tr>
                                <td>@proyecto.CodigoProyecto</td>
                                <td>@proyecto.NombreProyecto</td>
                                <td>@proyecto.Cliente</td>
                                <td>@proyecto.Moneda</td>
                                <td>@proyecto.Owner</td>
                                <td Class="text-center">
                                    @code
                                        If (proyecto.lineas.Where(Function(f) f.IdEstadoFacturacion = ELL.Pedido.EstadoFacturacion.Sent_to_invoice).Count > 0) Then
                                            clase = "text-danger"
                                            texto = "<strong>" & Utils.Traducir("Pasos para facturar") & "</strong>"
                                        Else
                                            clase = "glyphicon glyphicon-th-list"
                                            texto = String.Empty
                                        End If
                                    End code
                                    <a href='@Url.Action("DetalleProyecto", "Comercial", New With {.idCabecera = proyecto.IdCabecera})'>
                                        <span class="@clase" aria-hidden="true" title="@Utils.Traducir("Ir a detalle de proyecto")">@Html.Raw(texto)</span>
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