@imports web


<h3 class="my-3">@H.Traducir("Historico de evaluaciones")</h3>

<form action="" method="get">
    @Html.DropDownList("TipoFormulario", Nothing, New With {.Class = "form-control", .onchange = "this.form.submit();"})
</form>
@If Model.count > 0 Then
@<Table Class="table">
    <thead Class="thead-light">
        <tr>
            <th>@H.Traducir("Nombre")</th>
            <th>@H.Traducir("Apellido 1")</th>
            <th>@H.Traducir("Apellido 2")</th>
            <th>@H.Traducir("Fecha evaluación")</th>
            <th>@H.Traducir("Fecha fin contrato")</th>
            <th>@H.Traducir("Fecha realización evaluacion")</th>
            <th>@H.Traducir("Tipo evaluación")</th>
            <th>@H.Traducir("Total")</th>
            <th>@H.Traducir("Continua")</th>
            <th>@H.Traducir("Indice")</th>
            @For Each i In Enumerable.Range(1, CType(Model, List(Of Object)).Select(Of Integer)(Function(o) o.lstPuntuaciones.count()).Max())
                @<th>@H.Traducir("Resp") @i</th>
            Next
            <th></th>
        </tr>
    </thead>
    <tbody>
        @For Each e In Model
            Dim b As Nullable(Of Boolean) = e.continua
            @<tr>
    <td>@e.nombre</td>
    <td>@e.apellido1</td>
    <td>@e.apellido2</td>
    <td>@e.fechaVencimiento.toshortdatestring</td>
    <td>@e.fechaFinContrato</td>
    <td>@e.fecha.toshortdatestring</td>
    <td>@e.tipoEvaluacion</td>

    <th>@e.PuntuacionTotal</th>
    <td>

        @If b.HasValue() And b Then
            @H.Traducir("Si")
        ElseIf b.HasValue() And Not b Then
            @H.Traducir("No")
        Else
            @Html.Encode("?")End If
    </td>
    <td>@e.indice</td>

    @For Each p In e.lstPuntuaciones
        @<td>
            @p.Puntuacion
        </td>       Next
    <td>
        <a class="" href="@Url.Action("Viewrespuestatexto", New With {.idsab = e.idSab, .ticksVencimiento = CDate(e.fechaVencimiento).Ticks, .fecha1 = e.fechaVencimiento.ToShortDateString, .fecha2 = e.fechaFinContrato, .fecha3 = e.fecha.ToShortDateString, .tipoEv = e.tipoEvaluacion, .descEv = e.descEvaluacion})"> Mas</a>
    </td>


</tr>

        Next
    </tbody>
</table>
end if