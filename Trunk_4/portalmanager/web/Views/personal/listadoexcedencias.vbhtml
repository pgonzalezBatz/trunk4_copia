@imports web
@ModelType IEnumerable(Of object)


@Html.Partial("menu2")
@If Model IsNot Nothing Then
    @<table class="table">
        <thead class="thead-light">
            <tr>
                <th>@h.traducir("Nº Trabajador")</th>
                <th>@h.traducir("Nombre")</th>
                <th>@h.traducir("Tipo de excedencia")</th>
                <th>@h.traducir("Fecha Baja")</th>
                <th>@h.traducir("Fecha inicio excedencia")</th>
                <th>@h.traducir("Fecha fin excedencia")</th>
            </tr>
        </thead>
        @For Each p In Model
            @<tr>
                <td>@p.idTrabajador</td>
                <td>@p.nombre @p.apellido1 @p.apellido2</td>
                <td>@p.TipoBaja</td>
                <td>@p.FechaBaja.toshortdatestring</td>
                <td>@p.FechaInicio.toshortdatestring</td>
                <td>@p.FechaFin.toshortdatestring</td>

            </tr>   Next
    </table>
End If