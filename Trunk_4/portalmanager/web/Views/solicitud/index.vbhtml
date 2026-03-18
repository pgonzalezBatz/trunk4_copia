@imports web

<h3 class="my-3">
    @h.traducir("Listado de solicitudes de personal")
</h3>

    <a class="btn btn-success" href="@Url.Action("createestructural")">@h.traducir("Nueva solicitud")</a>
    <a class="btn btn-success" href="@Url.Action("createbecaria")">@h.traducir("Nueva solicitud de becari@")</a>
    @If String.IsNullOrEmpty(Request("cerradas")) Then
        @<a class="btn btn-info" href="@Url.Action("index", New With {.cerradas = True})">@h.traducir("Mostrar solicitudes cerradas")</a>
    Else
        @<a class="btn btn-info" href="@Url.Action("index")">@h.traducir("Mostrar solicitudes abiertas")</a>
    End If


@If ViewData("coberturapuesto").count = 0 Then
    @<h3>@h.traducir("No se encontraton solicitudes de personal")</h3>
    @<br />
Else

    @<table class="table mt-1">
        <thead class="thead-light">
            <tr>
                <th>@h.traducir("id")</th>
                <th>@h.traducir("Nº personas")</th>
                <th>@h.traducir("Responsable")</th>
                <th>@h.traducir("Negocio")</th>
                <th>@h.traducir("Departamento")</th>
                <th>@h.traducir("PG")</th>
                <th>@h.traducir("Puesto")</th>
                <th>@h.traducir("Descripción")</th>
                <th>@h.traducir("Creado")</th>
                <th>@h.traducir("Fecha prevista inicio")</th>
                <th>@h.traducir("Validacion")</th>
                @If Not String.IsNullOrEmpty(Request("cerradas")) Then
                    @<th>@h.traducir("Incorporación")</th>
                End If
                <th>@h.traducir("Acciones")</th>
            </tr>
        </thead>
        <tbody>
            @For Each c As coberturaPuesto In ViewData("coberturapuesto")
                @<tr>
    <td><a href="@Url.Action("detailestructural", h.ToRouteValues(Request.QueryString, New With {.idSolicitud = c.id}))">@c.id</a></td>
    <td>@c.nPersonas</td>
    <td>@c.nombreResponsable @c.apellido1Responsable</td>
    <td>@c.nombreNegocio</td>
    <td>@c.nombreDepartamento</td>
    <td>
        @If c.pgestion.Value Then
            @H.Traducir("Si")
        Else
            @H.Traducir("No")        End If
    </td>
    <td>@c.puesto</td>
    <td>@c.descripcion</td>
    <td>@c.FechaCreacion.ToShortDateString</td>
    <td>@c.fecha.ToShortDateString</td>
    <td>
        @If c.FechaIncorporacion.HasValue Then
            @<span style="color:red;">@H.Traducir("Cerrada")</span>
            @<br />
            @c.ResponsableCierre
            @<br />
            @<a href="@Url.Action("validacionesEstructural", H.ToRouteValues(Request.QueryString, New With {.idsolicitud = c.id}))">@H.Traducir("Ver validaciones")</a>
        Else

            @If c.ListOfValidacion.Count > 0 Then
                @code
                    Dim lastValidation = c.ListOfValidacion.OrderBy(Function(v) v.orden).Last
                End Code

                @If lastValidation Is Nothing Then
                    @H.Traducir("No necesita validaciones")
                ElseIf lastValidation.fechaValidacion.HasValue Then
                    @H.Traducir("Validada")
                    @<br />
                    @c.UltimaValidacion.Value.ToShortDateString
                    @If SimpleRoleProvider.IsUserAuthorised(Role.rrhh) Then
                        @<br />
                        @Html.ActionLink(H.Traducir("Cerrar"), "cerrarSolicitud", New With {.idSolicitud = c.id})
                        @<br />
                        @<a href="@Url.Action("validacionesEstructural", New With {.idsolicitud = c.id})">@H.Traducir("Ver validaciones")</a>
                    End If
                    'ElseIf lastValidation.fechaRechazo.HasValue Then
                elseif c.ListOfValidacion.Any(Function(v) v.fechaRechazo.HasValue) Then
                    @H.Traducir("Rechazada")
                    @If SimpleRoleProvider.IsUserAuthorised(Role.rrhh) Then
                        @<br>
                        @Html.ActionLink(H.Traducir("Cerrar"), "cerrarSolicitud", New With {.idSolicitud = c.id}) End If
                Else
                    @<a href="@Url.Action("validacionesEstructural", New With {.idsolicitud = c.id})">@H.Traducir("Pendiente de validar")</a>
                End If
            Else
                @H.Traducir("No necesita validaciones")
                    End If
        End If

    </td>
    @If Not String.IsNullOrEmpty(Request("cerradas")) Then
        @<td>@c.DatosIncorporacion</td>
    End If
         <td>
             @If Not c.UltimaValidacion.HasValue Or SimpleRoleProvider.IsUserAuthorised(web.Role.rrhh) Then
                 @<a href="@Url.Action("editEstructural", New With {.idsolicitud = c.id, .abiertas = String.IsNullOrEmpty(Request("cerradas"))})">@H.Traducir("Editar")</a>
                 @<br />
                 @If Not String.IsNullOrEmpty(Request("cerradas")) Then
                     @<a href="@Url.Action("editIncorporacion", New With {.idsolicitud = c.id})">@H.Traducir("Editar incorporación")</a>
                     @<br />
                 End If
             End If
             @If SimpleRoleProvider.IsUserAuthorised(web.Role.rrhh) Then
                 @<a href="@Url.Action("deleteEstructural", H.ToRouteValues(Request.QueryString, New With {.idsolicitud = c.id}))">@H.Traducir("Eliminar")</a>
             End If
         </td>
    </tr>
            Next
        </tbody>
    </table>
End If
@If ViewData("becaria").count = 0 Then
    @<h3>@h.traducir("No se encontraton solicitudes de becari@")</h3>
Else
    @<h3 class="my-3">
        @h.traducir("Solicitudes de becari@s")
    </h3>
    @<table class="table">
        <thead>
            <tr>
                <th>@h.traducir("id")</th>
                <th>@h.traducir("Nº personas")</th>
                <th>@h.traducir("Responsable")</th>
                <th>@h.traducir("Negocio")</th>
                <th>@h.traducir("Departamento")</th>
                <th>@h.traducir("Descripción")</th>
                <th>@h.traducir("Creación")</th>
                <th>@h.traducir("Incorporación")</th>
                <th>@h.traducir("Validacion")</th>
                @If Not String.IsNullOrEmpty(Request("cerradas")) Then
                    @<th>@h.traducir("Incorporación")</th>
                End If
                <th>@h.traducir("Acciones")</th>
            </tr>
        </thead>
        <tbody>
            @For Each c As becaria In ViewData("becaria")
                @<tr>
                    <td><a href="@Url.Action("detailbecaria", h.ToRouteValues(Request.QueryString, New With {.idSolicitud = c.id}))">@c.id</a></td>
                    <td>@c.nPersonas</td>
                    <td>@c.nombreResponsable @c.apellido1Responsable</td>
                    <td>@c.nombreNegocio</td>
                    <td>@c.nombreDepartamento</td>
                    <td>@c.descripcion</td>
                    <td>@c.fechaCreacion.ToShortDateString</td>
                    <td>@c.fecha.ToShortDateString</td>
                    <td>
                        @If c.fechaIncorporacion.HasValue Then
                            @<span style="color:red;">@H.Traducir("Cerrada")</span>
                        Else

                            @If c.ListOfValidacion.Count > 0 Then
                                @code
                                    Dim lastValidation = c.ListOfValidacion.OrderBy(Function(v) v.orden).Last
                                End Code
                                @If lastValidation Is Nothing Then
                                    @H.Traducir("No necesita validaciones")
                                ElseIf lastValidation.fechaValidacion.HasValue Then
                                    @H.Traducir("Validada")
                                    @<br />
                                    @c.UltimaValidacion.Value.ToShortDateString
                                    @If SimpleRoleProvider.IsUserAuthorised(Role.rrhh) Then
                                        @<br />
                                        @Html.ActionLink(H.Traducir("Cerrar"), "cerrarSolicitud", New With {.idSolicitud = c.id})
                                        @<br />
                                        @c.ResponsableCierre
                                        @<br />
                                        @<a href="@Url.Action("validacionesbecaria", New With {.idsolicitud = c.id})">@H.Traducir("Ver validaciones")</a>
                                    End If
                                    'ElseIf lastValidation.fechaRechazo.HasValue Then
                                elseif c.ListOfValidacion.Any(Function(v) v.fechaRechazo.HasValue) Then
                                    @H.Traducir("Rechazada")
                                    @If SimpleRoleProvider.IsUserAuthorised(Role.rrhh) Then
                                        @<br />
                                        @<a href="@Url.Action("validacionesbecaria", New With {.idsolicitud = c.id})">@H.Traducir("Ver validaciones")</a>
                                        @<br />
                                        @Html.ActionLink(H.Traducir("Cerrar"), "cerrarSolicitud", New With {.idSolicitud = c.id})                                End If
                                Else
                                    @<a href="@Url.Action("validacionesbecaria", New With {.idsolicitud = c.id})">@H.Traducir("Pendiente de validar")</a>
                                End If
                            Else
                                @H.Traducir("No necesita validaciones")
                            End If

                        End If

                    </td>
                    @If Not String.IsNullOrEmpty(Request("cerradas")) Then

                        @<td>@c.datosIncorporacion</td>
                    End If
                    <td>
                        @If Not c.ultimaValidacion.HasValue Or SimpleRoleProvider.IsUserAuthorised(web.Role.rrhh) Then
                            @<a href="@Url.Action("editbecaria", New With {.idsolicitud = c.id})">@h.traducir("Editar")</a>
                            @<br />

                        End If
                        @If SimpleRoleProvider.IsUserAuthorised(Role.rrhh) Then
                            @<a href="@Url.Action("deleteBecaria", New With {.idsolicitud = c.id})">@h.traducir("Eliminar")</a>
                        End If
                    </td>
                </tr>
            Next
        </tbody>
    </table>
End If
