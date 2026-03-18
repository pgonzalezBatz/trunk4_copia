@Imports TarjetasVisitaLib

@code
    Dim permisos As List(Of ELL.Permiso) = CType(ViewData("Permisos"), List(Of ELL.Permiso))
End Code

<script type="text/javascript">
</script>

<h3>@Utils.Traducir("Histórico solicitudes de permiso")</h3>
<hr />

@code

    If (permisos.Count = 0) Then
            @Html.Label(String.Empty, Utils.Traducir("noExisteNingunRegistro"))
    Else
            @<table Class="table table-striped table-hover table-condensed" style="margin-top:10px;">
                <thead>
                    <tr>
                        <th>@Utils.Traducir("Solicitante")</th>
                        <th class="text-center">@Utils.Traducir("Fecha solicitud")</th>
                        <th class="text-center">@Utils.Traducir("¿Autorizado?")</th>
                        <th class="text-center">@Utils.Traducir("Fecha autorizacion/rechazo")</th>
                        <th></th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    @code
                        For Each permiso In permisos
                            @<tr>
    <td>@permiso.NombreCompleto</td>
    <td class="text-center">@permiso.FechaSolicitud.ToShortDateString()</td>

    @code
        If (permiso.FechaRespuesta <> DateTime.MinValue) Then
            If (permiso.Autorizado) Then
                @<td Class="text-center">@Utils.Traducir("Si")</td>
            Else
                @<td Class="text-center">@Utils.Traducir("No")</td>
            End If
        Else
            @<td Class="text-center"></td>
        End If

        If (permiso.FechaRespuesta <> DateTime.MinValue) Then
            @<td Class="text-center">@permiso.FechaRespuesta.ToShortDateString()</td>
        Else
            @<td Class="text-center"></td>
        End If
    end code

    <td>
        @code
            If (permiso.FechaRespuesta = DateTime.MinValue) Then
                @<a href='@Url.Action("Autorizar", "Permiso", New With {.id = permiso.Id, .autorizado = True})' class="autorizar">
                    <span class="glyphicon glyphicon-ok text-success" aria-hidden="True" title="@Utils.Traducir(" Autorizar")"></span>
                </a>
            End If
        end code


    </td>
    <td>
        @code
            If (permiso.FechaRespuesta = DateTime.MinValue) Then
                @<a href='@Url.Action("Autorizar", "Permiso", New With {.id = permiso.Id, .autorizado = False})' class="rechazar">
                    <span class="glyphicon glyphicon-remove text-danger" aria-hidden="True" title="@Utils.Traducir("Rechazar")"></span>
                </a>
            End If
        end code
    </td>
</tr>
            Next
                    End Code
                </tbody>
            </table>
                        End If
End Code