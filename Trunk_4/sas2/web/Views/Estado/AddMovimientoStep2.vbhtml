@modeltype VMMovimientoFinal
@Code
    Layout = "~/Views/Shared/RMaster.vbhtml"
    Dim i = 0
End Code


<h3>@h.traducir("Añadir Movimientos de Marcas a Transportar - Paso 2")</h3>
<hr />

<form action="@Url.Action("AddMovimientoStep2")" method="post" class="form-inline">

    @If ViewData.ModelState.Keys.Any(Function(k) ViewData.ModelState(k).Errors.Any()) Then
        @<div Class="alert alert-danger">
            <Button Class="close" data-dismiss="alert" aria-hidden="true">&times;</Button>
            @Html.ValidationSummary(False, "Errors: ")
        </div>
    End If

    @Html.DisplayFor(Function(m) m.VectorMovimiento)


    <input type="hidden" value="@Model.Fecha.ToShortDateString" />
    @Html.HiddenFor(Function(m) m.Numope)
    @Html.HiddenFor(Function(m) m.Numord)
    <input type="submit" value="@h.traducir("Crear Movimientos")" Class="btn btn-primary" />
    <Table Class="table">
        <thead>
            <tr>
                <th> @h.traducir("Marca") </th>
                <th> @h.traducir("Cantidad")</th>
                <th> @h.traducir("Peso")</th>
                <th> @h.traducir("Ancho x Alto x Largo")</th>
                <th> @h.traducir("Diametro")</th>
                <th> @h.traducir("Observación")</th>
                <th> @h.traducir("Anteriores")</th>
            </tr>
        </thead>
        <tbody>
            @For Each m In Model.ListOfMarca
                @<tr>
                    <td>
                        <div Class="checkbox">
                            @Html.Hidden("listOfMarca[" + i.ToString + "].Marca", m.Marca)
                            @Html.Hidden("listOfMarca[" + i.ToString + "].Material", m.Material)
                            <Label>
                                @Html.CheckBox("listOfMarca[" + i.ToString + "].Seleccionado")
                                @m.Marca - @m.Material
                            </Label>
                        </div>
                    </td>
                    <td>
                        @Html.TextBox("listOfMarca[" + i.ToString + "].Cantidad", m.Cantidad, New With {.Class = "form-control", .style = "width:4em;"})
                    </td>
                    <td>
                        @Html.TextBox("listOfMarca[" + i.ToString + "].Peso", m.Peso, New With {.Class = "form-control", .style = "width:6em;"})
                    </td>
                    <td>
                        <div Class="form-group">
                            @Html.TextBox("listOfMarca[" + i.ToString + "].Ancho", m.Ancho, New With {.class = "form-control", .style = "width:6em;"})
                            X
                            @Html.TextBox("listOfMarca[" + i.ToString + "].Alto", m.Alto, New With {.class = "form-control", .style = "width:6em;"})
                            X
                            @Html.TextBox("listOfMarca[" + i.ToString + "].Largo", m.Largo, New With {.class = "form-control", .style = "width:6em;"})
                        </div>
                    </td>
                    <td>
                        @Html.TextBox("listOfMarca[" + i.ToString + "].Diametro", m.Diametro, New With {.Class = "form-control", .style = "width:6em;"})
                    </td>
                    <td>
                        @Html.TextArea("listOfMarca[" + i.ToString + "].Obervacion", m.Observacion, New With {.Class = "form-control"})
                    </td>
                    <td>
                        @m.salida
                        @Html.Hidden("listOfMarca[" + i.ToString + "].Obervacion.salida", m.salida)
                    </td>
                </tr>
                i = i + 1
            Next
        </tbody>
    </Table>
    <input type="submit" value="@h.traducir("Crear Movimientos")" Class="btn btn-primary" />
</form>

