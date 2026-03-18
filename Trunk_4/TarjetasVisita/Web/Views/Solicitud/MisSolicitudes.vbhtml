@Imports TarjetasVisitaLib

@code
    Dim permiso As ELL.Permiso = CType(ViewData("Permiso"), ELL.Permiso)
    Dim solicitudes As List(Of ELL.Solicitud) = CType(ViewData("Solicitudes"), List(Of ELL.Solicitud))
End Code

<script type="text/javascript">
</script>

<h3>@Utils.Traducir("Mis solicitudes")</h3>
<hr />

@code
    If (permiso Is Nothing) Then
        ' No ha solicitado el permiso
        @<p>@Utils.Traducir("Debe solicitar la autorización a su responsable para solicitar tarjetas")</p>
        @<a href="@Url.Action("SolicitarPermiso")" Class="btn btn-info">
            <span Class="glyphicon glyphicon-user"></span> @Utils.Traducir("Solicitar permiso")
        </a>
    ElseIf (permiso IsNot Nothing AndAlso permiso.FechaRespuesta = DateTime.MinValue) Then
        ' Lo ha solicitado pero no hay respuesta
        @<p>@Utils.Traducir("La solicitud de autorización a su responsable esta pendiente de respuesta")</p>
    ElseIf (permiso IsNot Nothing AndAlso permiso.FechaRespuesta <> DateTime.MinValue AndAlso Not permiso.Autorizado) Then
        ' Lo ha solicitado y se lo han rechazado
        @<p>@Utils.Traducir("La solicitud de autorización a su responsable ha sido rechazada")</p>
        @<a href="@Url.Action("SolicitarPermiso")" Class="btn btn-info">
            <span Class="glyphicon glyphicon-user"></span> @Utils.Traducir("Solicitar permiso")
        </a>
    ElseIf (permiso IsNot Nothing AndAlso permiso.FechaRespuesta <> DateTime.MinValue AndAlso permiso.Autorizado) Then
        ' Lo ha solicitado y se lo han autorizado
        @<a href="@Url.Action("SeleccionarDatosAlternativos", "DatosAlternativos")" Class="btn btn-info">
            <span Class="glyphicon glyphicon-plus"></span> @Utils.Traducir("Nueva")
        </a>
        @<br />@<br />
        @If (solicitudes.Count = 0) Then
            @Html.Label(String.Empty, Utils.Traducir("noExisteNingunRegistro"))
        Else
            @<table Class="table table-striped table-hover table-condensed" style="margin-top:10px;">
                <thead>
                    <tr>
                        <th>@Utils.Traducir("Nombre")</th>
                        <th>@Utils.Traducir("Puesto de trabajo")</th>
                        <th>@Utils.Traducir("Movil")</th>
                        <th>@Utils.Traducir("Dirección")</th>
                        <th>@Utils.Traducir("Telefono")</th>
                        <th>@Utils.Traducir("Email")</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    @code
                        For Each solicitud In solicitudes
                            @<tr>
                                <td>@solicitud.Nombre</td>
                                <td>@solicitud.Puesto</td>
                                <td>@solicitud.Movil</td>
                                <td>@solicitud.Direccion</td>
                                <td>@solicitud.Fijo</td>
                                <td>@solicitud.Email</td>
                                <td>
                                    <a href='@Url.Action("Ver", "Solicitud", New With {.id = solicitud.Id})' class="">
                                        <span class="glyphicon glyphicon-search" aria-hidden="True" title="@Utils.Traducir("Ver solicitud")"></span>
                                    </a>
                                </td>
                            </tr>
                        Next
                    End Code
                </tbody>
            </table>
                        End If
                        End If
End Code