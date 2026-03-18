@imports web
@Code
    ViewBag.title = h.traducir("Definir y ejecutar asientos de nomina")
    Dim i = 0
    Dim totalLiquido = CType(Model, IEnumerable(Of Object)).Sum(Function(a) CType(a.l, IEnumerable(Of Object)).Sum(Function(e) If(e.key.nombreapunte.ToString.Contains("liquido"), CDec(e.total), 0.0)))
    Dim diferencuaTotales As Decimal = ViewData("PagosTOtal") + totalLiquido
End code
@Html.ValidationSummary()

<h3>
    @h.traducir("Total pagos =") 
    @ViewData("PagosTOtal")
    @h.traducir("Total liquido asiento =")
    @totalLiquido
</h3>
@If diferencuaTotales > 1 Then
    @<h2 style="color:red;">
    @h.traducir("Diferencias globales en la nomina ") @diferencuaTotales.ToString("###,###.##")
    </h2>
End If


    <form action="" method="post">
    @For Each a In Model
        @<table class="table3">
            <caption>@a.key</caption>
            <thead>
                <tr>
                    <th>Apunte</th>
                    <th>Cuenta</th>
                    <th>Lantegi</th>
                    <th>Devengo</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                @For Each e In a.l
                    @<tr>
                        <td>
                            @e.key.nombreapunte
                        </td>
                        <td>@e.key.cuenta</td>
                        <td>@e.key.lantegi</td>
                        <td>@CDec(e.total).ToString("###,###.##")</td>
<td>@e.count</td>
                    </tr>

                Next
            </tbody>
            <tfoot>
                <tr>
                    @If CDec(a.total) = 0 Then
                        @<th style="background-color: green; color:#FFF;">Nº documento</th>
                        @<th colspan="4" style="background-color: green; color:#FFF;">
                            @H.Traducir("Cuadra")
                        </th>
                    Else
                        @<th colspan="3" style="background-color: red; color: #FFF;">Diferencia</th>
                        @<th colspan="2" style="background-color: red; color: #FFF; ">@CDec(a.total).ToString("###,###.##")</th>
                    End If
                </tr>
            </tfoot>
        </table>
        @<br />
        i = i + 1
    Next
    @If CType(Model, IEnumerable(Of Object)).Sum(Function(a) a.total) = 0 Then
        @Html.Label("lndoc", h.traducir("Nº documento"))
                @<br />
        @Html.TextBox("ndoc")
                @<br />
        @Html.Label("fecharegistro", h.traducir("Fecha de registro"))
                @<br />
        @Html.TextBox("fregistro")
                @<br />
                @<input type="submit" value="@h.traducir("Registrar en Navision")" />

    Else
        @h.traducir("Es necesario que los asientos cuadren para hacer el traspaso a Navision")
    End If
</form>