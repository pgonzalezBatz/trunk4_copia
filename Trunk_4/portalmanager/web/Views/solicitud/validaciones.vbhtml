@imports web
@Code
    ViewBag.title = "Validaciones de solicitud de personal"
End Code

<div class="sublink">
    <a href="@Url.Action("index")">@h.traducir("Volver")</a>
</div>

<h3>
    @h.traducir("Solicitud")
</h3>
<strong>@h.traducir("Nº de solicitud")</strong>: @ViewData("solicitud").id

<h3>
    @h.traducir("Listado de validaciones")
</h3>
<table class="table">
    <thead class="thead-light">
        <tr>
            <th>@h.traducir("Orden de validacion")</th>
            <th>@h.traducir("Nombre")</th>
            <th>@h.traducir("Apellido 1")</th>
            <th>@h.traducir("Apellido 2")</th>
            <th>@h.traducir("Validación")</th>
        </tr>
    </thead>
    <tbody>
        @For Each v As Validacion In ViewData("validaciones")
            @<tr>
                <td>@v.orden</td>
                <td>@v.nombre</td>
                <td>@v.apellido1</td>
                <td>@v.apellido2</td>
                <td>
                    @If v.fechaValidacion.HasValue Then
                       @h.traducir("Validada el")@Html.Encode(" ") @v.fechaValidacion.Value
                    ElseIf v.fechaRechazo.HasValue Then
                        @h.traducir("Rechazada el ")@Html.Encode(" ")@v.fechaRechazo.Value
                    Else
                    If v.idSab = SimpleRoleProvider.GetId() Or SimpleRoleProvider.IsUserAuthorised(Role.rrhh) Then
                    @<form action="" method="post">
                            @Html.Hidden("idsabvalidador", v.idSab)
                            <input type="submit" value="@h.traducir("Validar")" name="validar" />
                            <input type="submit" value="@h.traducir("Rechazar")" name="rechazar" />
                     </form>
                    Else
                        @h.traducir("Sin validar")
                    End If
                    End If
                </td>
             </tr>
                    Next
    </tbody>
</table>
