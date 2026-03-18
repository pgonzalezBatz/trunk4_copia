@Imports CostCarriersLib

@code
    Dim listaCostCarriers As List(Of ELL.BRAIN.CCMetadata) = CType(ViewData("CostCarriers"), List(Of ELL.BRAIN.CCMetadata))
    Dim pageSize As Integer = CInt(ViewData("PageSize"))
    Dim paginaActual As Integer = CInt(ViewData("PaginaActual"))
    Dim numeroElementos As Integer = CInt(ViewData("NumeroDeFacturas"))
    Dim empresa As String = String.Empty
    Dim codigo As String = String.Empty

    If (Not String.IsNullOrEmpty(ViewData("Empresa"))) Then
        empresa = ViewData("Empresa")
    End If

    If (Not String.IsNullOrEmpty(ViewData("txtCodigo"))) Then
        codigo = ViewData("txtCodigo")
    End If

End Code

<script type="text/javascript">
    $(function () {
        $(".boton-eliminar").click(function () {
            return confirm("@Html.Raw(Utils.Traducir("¿Desea eliminar el elemento seleccionado?"))");
        });
    });
</script>

<h3><label>@Utils.Traducir("Metadatos de portadores de coste")</label></h3>
<hr />

<a href="@Url.Action("Crear")" Class="btn btn-info">
    <span Class="glyphicon glyphicon-plus"></span> @Utils.Traducir("Nuevo")
</a>
<br /><br />

@Using Html.BeginForm("Index", "Metadata", FormMethod.Post)
    @<div Class="panel panel-default">
        <div Class="panel-heading">
            <h3 Class="panel-title">@Utils.Traducir("Filtros de búsqueda")</h3>
        </div>
        <div Class="panel-body">
            <div class="form-inline">
                <div Class="form-group">
                    <label>@Utils.Traducir("Empresa")</label>
                    @Html.DropDownList("Empresas", Nothing, New With {.class = "form-control"})
                </div>
                <div Class="form-group">
                    <label>@Utils.Traducir("Código")</label>
                    @Html.TextBox("txtCodigo", Nothing, New With {.class = "form-control"})
                </div>
                <input type="submit" id="submit" value="@Utils.Traducir("Buscar")" Class="btn btn-primary form-control" />
            </div>
        </div>
    </div>  End Using

@If (listaCostCarriers.Count = 0) Then
    @Html.Label(String.Empty, Utils.Traducir("noExisteNingunRegistro"))
Else
    @<table class="table table-striped table-hover table-condensed">
        <thead>
            <tr>
                <th>@Utils.Traducir("Empresa")</th>
                <th>@Utils.Traducir("Código")</th>
                <th>@Utils.Traducir("Lantegi")</th>
                <th>@Utils.Traducir("Tipo planta")</th>
                <th>@Utils.Traducir("Responsable")</th>
                <th class="text-center">@Utils.Traducir("Fecha apertura")</th>
                <th class="text-center">@Utils.Traducir("Fecha cierre")</th>
                <th></th>
                @*<th></th>*@
                <th></th>
            </tr>
        </thead>
        <tbody>
            @code
                For Each costcarrier In listaCostCarriers
                    Dim contieneDatosAños As Boolean = BLL.BRAIN.CCMetadataYearBLL.CargarListado(costcarrier.Empresa, costcarrier.Planta, costcarrier.CodigoPortador).Count > 0
                    Dim estiloIconoAños As String = String.Empty

                    If (contieneDatosAños) Then
                        estiloIconoAños = "text-success"
                    Else
                        estiloIconoAños = "text-muted"
                    End If

                    Dim fechaFin As String = If(costcarrier.FechaFin = DateTime.MinValue, String.Empty, costcarrier.FechaFin.ToShortDateString())

                    @<tr>
                        <td>@costcarrier.NombreEmpresa</td>
                        <td>@costcarrier.DescripcionCompleta</td>
                        <td>@costcarrier.Lantegi</td>
                        @select Case costcarrier.TipoPlanta
                            Case "C"
                                @<td>@Utils.Traducir("Corporativo")</td>
                            Case "P"
                                                            @<td>@Utils.Traducir("Planta")</td>
                        End Select
                        <td>@costcarrier.Responsable</td>
                        <td class="text-center">@costcarrier.FechaIni.ToShortDateString()</td>
                        <td class="text-center">@fechaFin</td>
                        <td Class="text-center">
                            <a href='@Url.Action("Editar", "Metadata", New With {.empresa = costcarrier.Empresa, .planta = costcarrier.Planta, .codigo = costcarrier.CodigoPortador})'>
                                <span class="glyphicon glyphicon-edit" aria-hidden="True" title="@Utils.Traducir("Editar")"></span>
                            </a>
                        </td>
                        @*<td Class="text-center">
                            <a href='@Url.Action("Eliminar", "Metadata", New With {.empresa = costcarrier.Empresa, .planta = costcarrier.Planta, .codigo = costcarrier.CodigoPortador})'>
                                <span class="glyphicon glyphicon-remove boton-eliminar" aria-hidden="True" title="@Utils.Traducir("Eliminar")"></span>
                            </a>
                        </td>*@
                        <td Class="text-center">
                            <a href='@Url.Action("Index", "MetadataYear", New With {.CostCarriersMetadata = String.Format("{0}-{1}-{2}", costcarrier.Empresa, costcarrier.Planta, costcarrier.CodigoPortador)})'>
                                <span class="glyphicon glyphicon-calendar @estiloIconoAños" aria-hidden="True" title="@Utils.Traducir("Datos anuales")"></span>
                            </a>
                        </td>
                    </tr>
                Next
            End Code
        </tbody>
    </table>
    @code
        If (Math.Ceiling(numeroElementos / pageSize) > 1) Then
            @<nav aria-label="Page navigation example">
                <ul Class="pagination">
                    @code
                        Dim numPagina As Integer = 1
                        Dim estilo = "active"
                        For iteracion = numPagina To Math.Ceiling(numeroElementos / pageSize) Step 1
                            If (iteracion = paginaActual) Then
                                estilo = "page-item active"
                            Else
                                estilo = "page-item"
                            End If
                            @<li Class="@estilo">@Html.ActionLink(iteracion, "Index", "Metadata", New With {.Empresas = empresa, .txtCodigo = codigo, .numPage = iteracion}, Nothing)</li>
                        Next
                    End Code
                </ul>
            </nav>
                        End If
    End Code
                        End if


