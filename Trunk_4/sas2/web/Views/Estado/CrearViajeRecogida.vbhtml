@modeltype VMRecogidaViaje
@Code
    Layout = "~/Views/Shared/RMaster.vbhtml"
End Code


<h3>@h.traducir("Agrupar Movimientos / Recogidas")    </h3>
<hr />


<form action="@Url.Action("RecogidaViajeSave")" method="post">
    @Html.ValidationSummary()

    @Html.Label(h.traducir("Transportista"))
    @Html.DropDownListFor(Function(m) m.Transportista, Model.LstTransportista, New With {.class = "form-control"})

    @Html.Label(h.traducir("Matricula 1"))
    @Html.TextBoxFor(Function(m) m.Matricula1, New With {.class = "form-control"})

    @Html.Label(h.traducir("Matricula 2"))
    @Html.TextBoxFor(Function(m) m.Matricula2, New With {.class = "form-control"})
    <br />
    <input type="submit" class="btn btn-primary" value="@h.traducir("Crear viaje")" />



    @If Model.LstRecogida Is Nothing Then
        @h.traducir("No se han encontrado movimientos de marcas pendientes")
    Else

        @<table class="table">
            <thead>
                <tr>
                    <th></th>
                    <th>@h.traducir("Fecha")</th>
                    <th>@h.traducir("Origen")</th>
                    <th>@h.traducir("Destino")</th>
                    <th>@h.traducir("Observaciones")</th>
                    <th>@h.traducir("OFs")</th>
                    <th>@h.traducir("Accion")</th>
                </tr>
            </thead>
            @Html.DisplayFor(Function(m) m.LstRecogida, New With {.InsideTable = True, .grouped = True})
        </table>
    End If


</form>