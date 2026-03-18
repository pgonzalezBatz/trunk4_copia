@Code
    Layout = Nothing
End Code

@Imports DOBLib

@code
    Dim evolucionObjetivos As List(Of ELL.EvolucionObjetivo) = CType(ViewData("EvolucionesObjetivo"), List(Of ELL.EvolucionObjetivo))
End Code

@If (evolucionObjetivos.Count = 0) Then
    @Html.Label(String.Empty, Utils.Traducir("noExisteNingunRegistro"))
Else
   @<form class="form-horizontal">
    @for each evolucion In evolucionObjetivos
        @<div Class="form-group">
            <label class="col-sm-6 control-label">@evolucion.Planta</label>
            <div class="col-sm-6">
                @Html.TextBox("valor", If(evolucion.ValorActual <> Integer.MinValue, evolucion.ValorActual, Utils.Traducir("Sin valor")), New With {.class = "form-control"})
            </div>
        </div>  Next
</form>
End If
