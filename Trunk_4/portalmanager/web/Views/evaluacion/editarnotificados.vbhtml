    @imports web
@Code
    ViewBag.title = "Evaluaciones y vencimientos"
End Code

<h3 class="">@H.Traducir("Listado de colaboradores")</h3>
<table class="table">
    <thead class="thead-light">
        <tr>
            <th>@h.traducir("Fecha Notificación")</th>
            <th>@h.traducir("Colaborador/Colaboradora")</th>
            <th>@h.traducir("Tipo de notificación")</th>
            <th>@h.traducir("Fecha fin")</th>
            <th>@h.traducir("Acciones")</th>
        </tr>
    </thead>
    <tbody>
        @For Each cg In Model
    For Each c In cg
            @<tr>
                <td>
                    @CType(c.fechaVencimiento, DateTime).ToShortDateString
                </td>
                <td>
                    @c.nombre.ToString.ToUpper @Html.Encode(" ") @c.apellido1.ToString.ToUpper @Html.Encode(" ") @c.apellido2.ToString.ToUpper
                </td>
                <td>
                    @c.tipoVencimiento
                </td>
                <td>
                    @If IsDate(c.fechaProrroga) Then
                        @c.fechaProrroga.toshortdatestring
                    End If              
                </td>
                <td>
                    @code
                        Dim separador = False
                    End Code

                @For Each fe As web.FormularioVencimientos In ViewData("lstFormularioEvaluacionesEpsilon")
                    If fe.lstVencimiento.Contains(c.idvencimiento) Then
                        If separador Then
                        @Html.Encode("|")
                        End If
                            @<a href="@Url.Action("verFormulario", New With {.idFormulario = fe.idFormulario, .idSabColaborador = c.idSab, .ticksVencimiento = CType(c.fechaVencimiento, DateTime).Ticks})">@h.traducir("Ver evaluación")</a>
                        separador = True
                    End If

                Next
                @If ConfigurationManager.AppSettings("vencimiento").Split(";").ToList().Contains(c.idvencimiento) Then
                    If separador Then
                    @Html.Encode("|")
                    End If
                        @<a href="@Url.Action("verpropuestacontinuidad", New With {.id = c.propuestaContinuidad.id})">@h.traducir("Ver propuesta continuidad")</a>
                        @Html.Encode("|")
                        @<a href="@Url.Action("editpropuestacontinuidad", New With {.id = c.propuestaContinuidad.id})">@h.traducir("Editar propuesta continuidad")</a>
                End If
                </td>
            </tr>
                                Exit For
                            Next
                        Next
    </tbody>
</table>
<br />  