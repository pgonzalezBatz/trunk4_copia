@Imports CostCarriersLib

@Code
    Dim cabeceras As List(Of ELL.CabeceraCostCarrier) = CType(ViewData("CabecerasProyecto"), List(Of ELL.CabeceraCostCarrier))
End Code

<h3><label>@Utils.Traducir("Validaciones pendientes")</label></h3>
<hr />

@code
    If (cabeceras.Count > 0) Then
        @<div class="row">
            <div class="col-sm-7">
                <table id="tabla" class="table table-condensed table-striped table-hover">
                    <thead>
                        <tr>
                            <th>@Utils.Traducir("Proyecto")</th>
                            <th></th>     
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        @For Each cabecera In cabeceras
                            @<tr>
                                <td>@cabecera.NombreProyecto</td>
                                <td Class="text-center">
                                     <a href='@Url.Action("DetalleProyecto", "Validaciones", New With {.idCabecera = cabecera.Id})'>
                                         <span class="glyphicon glyphicon-option-horizontal text-danger" aria-hidden="true" title="@Utils.Traducir("Validaciones pendientes")"></span>
                                     </a>
                                 </td>       
                                <td Class="text-center">
                                    <a href='@Url.Action("DetalleProyecto", "Totales", New With {.idCabecera = cabecera.Id})'>
                                        <span class="glyphicon glyphicon-ok-circle text-success" aria-hidden="true" title="@Utils.Traducir("Totales por proyecto")"></span>
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
