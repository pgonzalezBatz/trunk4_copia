    @imports web
@Code
    ViewBag.title = "Evaluaciones y vencimientos"
End Code
@If SimpleRoleProvider.IsUserAuthorised(Role.rrhh) Then
    @<ul class="nav">
    @For Each tf In ViewData("tiposformulario")
            @<li class="nav-item">
    <a class="nav-link" href="@Url.Action("AdministrarFormularioList", New With {.idformulario = tf.value})">@h.traducir("Formulario") @tf.text</a>
    </li>
    Next
             <li class="nav-item">
                 <a class="nav-item nav-link" href="@url.action("editarnotificados")">@h.traducir("Editar vencimientos notificados")</a>
             </li>
    </ul>
End If

<h3 class="my-3">@H.Traducir("Listado de colaboradores")</h3>

<a class="btn btn-info" href="@Url.Action("HistoricoEvaluaciones")">@H.Traducir("Historico de últimas evaluaciones de cada colaborador")</a>
<table class="table">
    <thead class="thead-light">
        <tr>
            <th>@H.Traducir("Colaborador/Colaboradora")</th>
            <th>@H.Traducir("Tipo de notificación")</th>
            <th>@H.Traducir("Fecha evaluación")</th>
            <th>@H.Traducir("Acciones")</th>
            <th>@H.Traducir("Notificado a RRHH")</th>
        </tr>
    </thead>
    <tbody>
        @For Each cg In Model
            For Each c In cg
            @<tr>
                <td>
                    @c.nombre.ToString.ToUpper @Html.Encode(" ") @c.apellido1.ToString.ToUpper @Html.Encode(" ") @c.apellido2.ToString.ToUpper
                </td>
                <td>
                    @c.tipoVencimiento
                </td>
                <td>
                    @If IsDate(c.fechaVencimiento) Then
                        @c.fechaVencimiento.toshortdatestring
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
                            If CType(c.respuesta, List(Of web.Respuesta)).Where(Function(r) r.idFormulario = fe.idFormulario).Count = 0 Then
                                @*@<a href="@Url.Action("rellenarFormulario", New With {.idFormulario = fe.idFormulario, .idSabColaborador = c.idSab, .ticksVencimiento = CType(c.fechaVencimiento, DateTime).Ticks})">@H.Traducir("Rellenar evaluación")</a>*@
                                @<a href="@Url.Action("rellenarFormulario", New With {.idFormulario = fe.idFormulario, .idSabColaborador = c.idSab, .idTrabajador = c.idTrabajador, .ticksVencimiento = CType(c.fechaVencimiento, DateTime).Ticks})">@H.Traducir("Rellenar evaluación")</a>
                            Else
                                @<a href="@Url.Action("verFormulario", New With {.idFormulario = fe.idFormulario, .idSabColaborador = c.idSab, .ticksVencimiento = CType(c.fechaVencimiento, DateTime).Ticks})">@H.Traducir("Ver evaluación")</a>
                            End If
                            separador = True
                        End If

                    Next
                    @If ConfigurationManager.AppSettings("vencimiento").Split(";").ToList().Contains(c.idvencimiento) Then
                        If separador Then
                            @Html.Encode("|")
                        End If
                        @If c.propuestaContinuidad Is Nothing Then
                            @<a href="@Url.Action("addpropuestacontinuidad", New With {.idSabColaborador = c.idSab, .ticksVencimiento = CType(c.fechaVencimiento, DateTime).Ticks})">@H.Traducir("Nueva propuesta continuidad")</a>
                        Else
                            @If c.notificacionColaborador Is Nothing Then
                                @<a href="@Url.Action("editpropuestacontinuidad", New With {.id = c.propuestaContinuidad.id})">@H.Traducir("Editar propuesta continuidad")</a>
                            Else
                                @<a href="@Url.Action("verpropuestacontinuidad", New With {.id = c.propuestaContinuidad.id})">@H.Traducir("Ver propuesta continuidad")</a>
                            End If

                        End If
                    End If
                </td>
                <td>
                    @code
                        Dim mostrarOpcionDeNotificar = True
                    End Code
                    @For Each fe As web.FormularioVencimientos In ViewData("lstFormularioEvaluacionesEpsilon")

                        If fe.lstVencimiento.Contains(c.idvencimiento) AndAlso CType(c.respuesta, List(Of web.Respuesta)).Where(Function(r) r.idFormulario = fe.idFormulario).Count = 0 Then
                            mostrarOpcionDeNotificar = False
                        End If
                    Next
                    @If ConfigurationManager.AppSettings("vencimiento").Split(";").ToList().Contains(c.idvencimiento) AndAlso c.propuestaContinuidad Is Nothing Then
                        mostrarOpcionDeNotificar = False
                    End If
                    @If c.notificacionColaborador IsNot Nothing Then
                        @c.notificacionColaborador.fechaNotificacion
                    ElseIf mostrarOpcionDeNotificar Then
                        @<form action="@Url.Action("notificadocolaborador", New With {.idSabColaborador = c.idSab, .ticksVencimiento = CType(c.fechaVencimiento, DateTime).Ticks})" method="post">
                            <input type="submit" value="@H.Traducir("Notificar a RRHH")" class="btn btn-primary" />
                        </form>
                    End If

                </td>
            </tr>
                                Exit For
                            Next
                        Next
    </tbody>
</table>
<br />  
