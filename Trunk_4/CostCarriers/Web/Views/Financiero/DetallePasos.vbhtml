@Imports CostCarriersLib

@Code
    Dim steps As List(Of ELL.Step) = CType(ViewData("Steps"), List(Of ELL.Step))
    Dim culturaEsES As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture("es-ES")
    Dim listaIdsPlantasUnicos As List(Of Integer) = (From lstStep In steps
                                                     Group lstStep By lstStep.IdPlanta Into agrupacion = Group
                                                     Select IdPlanta).ToList()
    Dim ticket As SabLib.ELL.Ticket = CType(Session("Ticket"), SabLib.ELL.Ticket)
    Dim cabeceraProyecto As ELL.CabeceraCostCarrier = CType(ViewData("CabeceraProyecto"), ELL.CabeceraCostCarrier)
End Code

<script type="text/javascript">

    $(function () {
    });

</script>

<h3><label>@Utils.Traducir("Gestionar apertura pasos") - @cabeceraProyecto.NombreProyecto</label></h3>
<hr />

@code
    Dim plantas As New List(Of ELL.Planta)
    listaIdsPlantasUnicos.ForEach(Sub(s) plantas.Add(BLL.PlantasBLL.Obtener(s)))

    @Using Html.BeginForm("Abrir", "Financiero", FormMethod.Post)
        If (steps.Count > 0) Then
            @Html.ActionLink(Utils.Traducir("Volver a listado proyectos"), "Index")
            @<br />
            @<br />
            @<table id="tabla" class="table table-bordered table-condensed">    

    @For Each planta In plantas.OrderBy(Function(f) f.IdPlanta)
        @<thead>
            <tr>
                <th Class="success" style="text-align:center;text-transform:uppercase" colspan="6">@planta.Planta</th>
            </tr>
            <tr>
                <th Class="info" style="text-align:center;">@Utils.Traducir("Portador de coste")</th>
                <th Class="info" style="text-align:center;">@Utils.Traducir("Nombre")</th>
                <th Class="info" style="text-align:center;">@Utils.Traducir("Estado")</th>
                <th Class="info" style="text-align:center;">@Utils.Traducir("Grupo de coste")</th>
                <th Class="warning" style="text-align:center;">@Utils.Traducir("Estado validación")</th>
                <th></th>
            </tr>
        </thead>
        @<tbody>
            @For Each paso In steps.Where(Function(f) f.IdPlanta = planta.Id).OrderBy(Function(f) If(Not String.IsNullOrEmpty(f.CostCarrier), f.CostCarrier, cabeceraProyecto.CodigoProyecto & CStr(f.CorrelativoCC).PadLeft(4, "0")))
                Dim nombre As String = BLL.ValidacionesLineaBLL.ObtenerPorStep(paso.Id).Nombre

                paso.CargarValoresStepValidacion()
                Dim costGroup As ELL.CostGroup = BLL.CostsGroupBLL.Obtener(paso.IdCostGroup)
                Dim codigo As String = String.Empty

                @<tr>
                    <td>
                        @code
                            Dim estiloCodigo As String = String.Empty
                            Dim textoValidacionEstado As String = String.Empty
                            ' Esto indica que en álgún momento ya se ha abierto
                            If (Not String.IsNullOrEmpty(paso.CostCarrier)) Then
                                codigo = paso.CostCarrier
                                estiloCodigo = "text-success"
                                If (paso.IdEstadoValidacion <> ELL.Validacion.Estado.Opened) Then
                                    textoValidacionEstado = String.Format("({0})", Utils.Traducir("ya abierto"))
                                End If
                            Else
                                If (paso.IdEstadoValidacion = ELL.Validacion.Estado.Opened AndAlso Not String.IsNullOrEmpty(paso.Comentarios)) Then
                                    textoValidacionEstado = Utils.Traducir(paso.Comentarios)
                                End If

                                If (Not String.IsNullOrEmpty(cabeceraProyecto.CodigoProyecto)) Then
                                    If (paso.CorrelativoCC = Integer.MinValue) Then
                                        codigo = cabeceraProyecto.CodigoProyecto & "_??"
                                    Else
                                        codigo = cabeceraProyecto.CodigoProyecto & CStr(paso.CorrelativoCC).PadLeft(4, "0")
                                    End If
                                Else
                                    codigo = Utils.Traducir("Sin código de proyecto")
                                End If
                            End If
                        End code
                        <label Class="control-label @estiloCodigo">@codigo</label>
                    </td>
                    <td><label class="control-label text-danger">@cabeceraProyecto.Abreviatura</label>_<label class="control-label">@nombre</label></td>
                    <td><label class="control-label">@paso.Estado</label></td>
                    <td><label class="control-label">@costGroup.Descripcion</label></td>
                    <td style="text-align:center;"><label class="control-label">@String.Format("{0} {1}", paso.EstadoValidacion, textoValidacionEstado)</label></td>
                    <td style="text-align:center">
                        @code
                            ' Un paso puede estar aprobador pero ya haber sido abierto anteriormente con lo cual no hay que abrirlo otra vez
                            If (String.IsNullOrEmpty(paso.CostCarrier) AndAlso paso.IdEstadoValidacion = ELL.Validacion.Estado.Approved AndAlso Not String.IsNullOrEmpty(cabeceraProyecto.CodigoProyecto)) Then
                            @<a href='@Url.Action("CrearDesdeFinanciero", "Metadata", New With {.portadorCoste = codigo, .idStep = paso.Id, .idValidacionLinea = paso.IdValidacionLinea})'>
                                <span class="glyphicon glyphicon-plus" aria-hidden="true" title="@Utils.Traducir("Crear metadatos del portador de coste")"></span>
                            </a>
                            End If
                            @<a href='@Url.Action("DetalleCostGroupHistorico", "Validaciones", New With {.idCostGroup = costGroup.Id, .idPaso = paso.Id, .idValidacion = Nothing})' target="_blank">
                                <span class="glyphicon glyphicon-option-horizontal text-danger" aria-hidden="true" title="@Utils.Traducir("Ver detalle")"></span>
                            </a>
                            '@Html.ActionLink(Utils.Traducir("Ver detalle"), "DetalleCostGroupHistorico", "Validaciones", New With {.idCostGroup = costGroup.Id, .idPaso = paso.Id, .idValidacion = Nothing}, Nothing)
                        End code
                    </td>
                </tr>
                            Next
            <tr><td colspan="6"></td></tr>
        </tbody>
                            Next
</table>
                            Else
            @Html.Label(String.Empty, Utils.Traducir("noExisteNingunRegistro"))
                                            End If
                                    End Using
                                    End code