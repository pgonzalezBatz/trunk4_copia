@imports web
@section title
    - @h.traducir("Busqueda")
End section
@section menu1
    @Html.Partial("menu")
End Section
<div>
    @If SimpleRoleProvider.IsUserAuthorised(Web.roles.homologaciones) Then
        @<h3>@h.traducir("Aduntar archivos a proveedor")</h3>
        @<form action="@Url.Action("adjuntar", h.ToRouteValues(Request.QueryString, Nothing))" method="post" enctype="multipart/form-data">
            <fieldset>

                <input type="file" name="adjunto" accept="image/*,.pdf">
                <br />
                <input type="submit" value="@h.traducir("Guardar")" Class="btn btn-primary" />
            </fieldset>
        </form>
    End If
    <br />
    <a href="@Url.Action("search", h.ToRouteValuesDelete(Request.QueryString, "id"))">@h.traducir("Volver a la busqueda")</a>
    <h3>@h.traducir("Listado de adjuntos")</h3>
    @If Model.count > 0 Then
        @<table class="table">
            <thead>
                <tr>
                    <th>@h.traducir("Nombre adjunto")</th>
                    <th>@h.traducir("Acciones")</th>
                </tr>
            </thead>
            <tbody>
                @For Each a In Model
                    @<tr>
                        <td>
                            <a href="@Url.Action("adjunto", New With {.idempresa = Request("id"), .idadjunto = a.id})" target="_blank">
                                @a.nombre
                            </a>
                        </td>
                        <td>
                            @If SimpleRoleProvider.IsUserAuthorised(Web.roles.homologaciones) Then
                                @<form action="@Url.Action("eliminaradjunto", h.ToRouteValues(Request.QueryString, Nothing))" method="post">
                                    <input type="hidden" name="idadjunto" value="@a.id" />
                                    <input type="submit" value="@h.traducir("Eliminar")" onclick="return confirm('@h.traducir("¿Estas seguro de que quieres eliminar este adjunto?")')" Class="btn btn-primary" />
                                </form>
                            End If

                        </td>
                    </tr>
                Next
            </tbody>
        </table>
    End If
</div>