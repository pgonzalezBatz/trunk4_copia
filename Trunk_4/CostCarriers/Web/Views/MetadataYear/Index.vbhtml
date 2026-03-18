@Imports CostCarriersLib

@code
    Dim listaCCYears As List(Of ELL.BRAIN.CCMetadataYear) = CType(ViewData("CostCarriersYears"), List(Of ELL.BRAIN.CCMetadataYear))
    Dim costCarrierMetadata As String = ViewData("CostCarrierMetadata")
    Dim numeroFacturas As Integer = CInt(ViewData("NumeroDeFacturas"))
    Dim culturaEsES As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture("es-ES")
End Code

<script type="text/javascript">
    $(function () {
        $(".boton-eliminar").click(function () {
            return confirm("@Html.Raw(Utils.Traducir("¿Desea eliminar el elemento seleccionado?"))");
        });
    });
</script>

<h3><label>@Utils.Traducir("Metadatos anuales del portador de coste")</label></h3>
<hr />

<a href="@Url.Action("Agregar", New With {.CostCarrierMetadata = costCarrierMetadata})" Class="btn btn-info">
    <span Class="glyphicon glyphicon-plus"></span> @Utils.Traducir("Nuevo")
</a>
<br /><br />

<div Class="panel panel-default">
    <div Class="panel-heading">
        <h3 Class="panel-title"><label>@Utils.Traducir("Filtros de búsqueda")</label></h3>
    </div>
    <div Class="panel-body">
        <div class="form-inline">
            <div Class="form-group">
                <label>@Utils.Traducir("Portador de coste")</label>
                @Html.DropDownList("CostCarriersMetadata", Nothing, New With {.class = "form-control", .disabled = "disabled"})
            </div>
        </div>
    </div>
</div>

@If (listaCCYears.Count = 0) Then
    @Html.Label(String.Empty, Utils.Traducir("noExisteNingunRegistro"))
Else
    @<table class="table table-striped table-hover table-condensed">
        <thead>
            <tr>
                <th>@Utils.Traducir("Empresa")</th>
                <th>@Utils.Traducir("Código")</th>
                <th>@Utils.Traducir("Año")</th>
                <th>@Utils.Traducir("Presupuesto total")</th>
                <th>@Utils.Traducir("Moneda")</th>
                <th></th>
                @*<th></th>*@
            </tr>
        </thead>
        <tbody>
            @code
                For Each ccYear In listaCCYears
                    Dim presupTotal As Decimal = ccYear.PresupBonosPersona + ccYear.PresupFacturas + ccYear.PresupViajes

                    @<tr>
                        <td>@ccYear.NombreEmpresa</td>
                        <td>@ccYear.CodigoPortador</td>
                        <td>@ccYear.Anyo</td>
                        <td style="text-align:right;">
                            @presupTotal.ToString("N2", culturaEsES)
                        </td>
                        <td>@ccYear.Moneda</td>
                        <td Class="text-center">
                            <a href='@Url.Action("Editar", "MetadataYear", New With {.empresa = ccYear.Empresa, .planta = ccYear.Planta, .anyo = ccYear.Anyo, .codigoPortador = ccYear.CodigoPortador})'>
                                <span class="glyphicon glyphicon-edit" aria-hidden="True" title="@Utils.Traducir("Editar")"></span>
                            </a>
                        </td>
                        @*<td Class="text-center">
                            <a href='@Url.Action("Eliminar", "MetadataYear", New With {.empresa = ccYear.Empresa, .planta = ccYear.Planta, .codigo = ccYear.CodigoPortador, .anyo = ccYear.Anyo})'>
                                <span class="glyphicon glyphicon-remove boton-eliminar" aria-hidden="True" title="@Utils.Traducir("Eliminar")"></span>
                            </a>
                        </td>*@
                    </tr>
                Next
            End Code
        </tbody>
    </table>
                End if


