@imports web
@ModelType IEnumerable(Of Web.proveedor)
@section title
    - @h.traducir("Busqueda")
End section
@section menu1
    @Html.Partial("menu")
End Section

<form action="" method="get">
        @If SimpleRoleProvider.IsUserAuthorised(Web.roles.editar) Then
            @<div Class="row">
                <div Class="col-sm-12">
                    <h4>@Html.ActionLink(h.Traducir("Crear Nuevo proveedor"), "Create")</h4>
                </div>
            </div>
        End If

        <div class="row">
            <div class="col-sm-12">
                <label class="radio-inline">@Html.RadioButton("st", CInt(proveedorController.searchtype.standard), True) @h.traducir("Buscar CIF, Código de proveedor y Nombre")</label>
                <label class="radio-inline">@Html.RadioButton("st", CInt(proveedorController.searchtype.direccion)) @h.traducir("Dirección")</label>
                <label class="radio-inline">@Html.RadioButton("st", CInt(proveedorController.searchtype.codigoPostal)) @h.traducir("Código postal")</label>
                <label class="radio-inline">@Html.RadioButton("st", CInt(proveedorController.searchtype.localidad)) @h.traducir("Localidad")</label>
                <label class="radio-inline">@Html.RadioButton("st", CInt(proveedorController.searchtype.provincia)) @h.traducir("Provincia")</label>
                <label class="radio-inline">@Html.RadioButton("st", CInt(proveedorController.searchtype.pais)) @h.traducir("Pais")</label>
            </div>

        </div>
        <div class="row">

            <div class="col-sm-9">
                <div class="input-group">
                    @Html.TextBox("q", Nothing, New With {.autofocus = "", .class = "form-control"})
                    <div class="input-group-btn">
                        <button class="btn btn-primary" type="submit"><i class="glyphicon glyphicon-search"></i></button>
                    </div>
                </div>
            </div>
            <div class="col-sm-3">
                @Html.DropDownList("idplanta", Nothing, New With {.class = "form-control"})
            </div>
        </div>
</form>
<br />
@If Model Is Nothing Then

Else
    @<span>  @h.traducir("Nº de resultados") </span>
    @<span Class="badge">@ViewBag.count @Html.Encode(" ")</span>
    @<table class="table table-responsive">
        <thead>
            <tr>
                <th>
                    @System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("Código"))
                </th>
                <th>
                    @System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("nombre"))
                </th>
                <th>
                    @System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("R.S."))
                </th>
                <th>
                    @System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("cif"))
                </th>
                <th>
                    @System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("telefono"))
                </th>
                <th>
                    @System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("Dirección"))
                </th>
                <th>
                    @System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("contacto / email"))
                </th>
                <th>
                    @System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("f Pago"))
                </th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            @For Each item In Model
                Dim proveedorAdok = Not item.fechaBaja.HasValue AndAlso String.IsNullOrEmpty(item.codpro)
                Dim currentItem = item
                Dim cl As String
                If currentItem.fechaBaja.HasValue Then
                    cl = "baja"
                Else
                    cl = "alta"
                End If
                @<tr class="@cl">
                    <td>
                        @If proveedorAdok Then
                            @<span class="glyphicon glyphicon-info-sign alert alert-info" title="@h.traducir("Proveedor ADOK")"> </span>
                        Else @<strong>@Html.DisplayFor(Function(modelItem) currentItem.codpro)</strong>
                        End If
                    </td>
                    <td>
                        <strong>@Html.DisplayFor(Function(modelItem) currentItem.nombre)</strong>
                    </td>
                    <td>
                        <strong>@Html.DisplayFor(Function(modelItem) currentItem.RazonSocial)</strong>
                    </td>
                    <td>
                        @Html.DisplayFor(Function(modelItem) currentItem.cif)
                    </td>
                    <td>
                        @Html.DisplayFor(Function(modelItem) currentItem.telefono)
                        @If currentItem.numeroAbreviado.Length > 0 Then
                            @<br />
                            @<span style="color:red;">
                                @h.traducir("Directo")
                                @currentItem.numeroAbreviado
                            </span>

                        End If
                    </td>
                    <td>
                        @Html.DisplayFor(Function(modelItem) currentItem.direccion)
                        @Html.DisplayFor(Function(modelItem) currentItem.codigoPostal)
                        @Html.DisplayFor(Function(modelItem) currentItem.localidad)
                        @Html.DisplayFor(Function(modelItem) currentItem.provincia)
                        @Html.DisplayFor(Function(modelItem) currentItem.nombrePais)
                    </td>
                    <td>
                        @Html.DisplayFor(Function(modelItem) currentItem.contacto)
                        @Html.DisplayFor(Function(modelItem) currentItem.email)
                    </td>
                    <td>
                        @Html.DisplayFor(Function(modelItem) currentItem.nombreFPago)
                    </td>
                    <td>
                        @If Not proveedorAdok Then
                            @Html.ActionLink(h.traducir("Detalle"), "Details", h.ToRouteValues(Request.QueryString, New With {.id = currentItem.id}))

                            @If SimpleRoleProvider.IsUserAuthorised(Web.roles.editar) Then
                                @Html.Encode(" | ")
                                @Html.ActionLink(h.traducir("Editar"), "Edit", h.ToRouteValues(Request.QueryString, New With {.id = currentItem.id}))
                            End If
                        Else
                            @If SimpleRoleProvider.IsUserAuthorised(Web.roles.editar) Then
                                @Html.ActionLink(h.traducir("Asignar numero"), "EditAdok", h.ToRouteValues(Request.QueryString, New With {.id = currentItem.id}))
                            End If
                        End If
                        <br />

                        @Html.ActionLink(h.traducir("Adjuntos"), "listadjunto", h.ToRouteValues(Request.QueryString, New With {.id = currentItem.id}))
                        @If Not IsNothing(ViewData("user")) AndAlso ((Not IsNothing(ViewData("user").n2) AndAlso Not IsDBNull(ViewData("user").n2) AndAlso ViewData("user").n2.ToString().ToLower().Contains("sis")) OrElse (Not IsNothing(ViewData("user").idTrabajador) AndAlso Not IsDBNull(ViewData("user").idTrabajador) AndAlso IsNumeric(ViewData("user").idTrabajador) AndAlso CInt(ViewData("user").idTrabajador) = 3164)) Then
                            @Html.Encode(" | ")
                            @Html.ActionLink(h.Traducir("Exportar a plantas sistemas"), "exportsistemas", h.ToRouteValues(Request.QueryString, New With {.id = currentItem.id}))
                        End If
                        @If SimpleRoleProvider.IsUserAuthorised(Web.roles.editarRecursos) Then
                            @Html.Encode(" | ")
                            @Html.ActionLink(h.Traducir("Recursos"), "listrecursos", h.ToRouteValues(Request.QueryString, New With {.id = currentItem.id}))
                        End If
                        @If SimpleRoleProvider.IsUserAuthorised(Web.roles.editar) Then
                            @Html.Encode(" | ")
                            @Html.ActionLink(h.Traducir("Logs"), "listlogs", h.ToRouteValues(Request.QueryString, New With {.id = currentItem.id}))
                            @Html.Encode(" | ")
                            @Html.ActionLink(h.traducir("Usuarios"), "listusuarios", h.ToRouteValues(Request.QueryString, New With {.id = currentItem.id}))
                        End If
                        @If SimpleRoleProvider.IsUserAuthorised(Web.roles.AdministrarPotencialesYCapacidades) Then
                            @Html.Encode(" | ")
                            @Html.ActionLink(h.traducir("Capacidades"), "listcapacidades", h.ToRouteValues(Request.QueryString, New With {.id = currentItem.id, .codpro = currentItem.codpro}))
                        End If
                    </td>
                </tr>
            Next
        </tbody>
    </table>
End If
@code
    Dim take = If(Request("take"), proveedorController.TakeLimit)
    Dim skip = If(Request("skip"), proveedorController.skipMin)
End Code

<span class="pagination"></span>
<ul class="pager">
    @If skip > proveedorController.skipMin Then
    @<li>
        <a href="@Url.Action("search", h.ToRouteValues(Request.QueryString, New With {.skip = skip - take}))">@h.traducir("Menos")</a>
    </li>

    End If
    @If ViewBag.count > take Then
    @<li>
        <a href="@Url.Action("search", h.ToRouteValues(Request.QueryString, New With {.skip = skip + take}))">@h.traducir("Mas")</a>
    </li>
    End If
</ul>