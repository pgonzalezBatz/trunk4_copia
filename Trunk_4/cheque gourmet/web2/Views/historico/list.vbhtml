@imports web2
@Code
    ViewBag.title = "Histórico"
End Code

<h4>@h.traducir("Histórico de todos los talonarios recibidos")</h4>
<table class="table1">
    <thead>
        <tr>
            <th>@h.traducir("Fecha corte")</th>
            <th>@h.traducir("Tipo")</th>
            <th>@h.traducir("Precio cheque")</th>
            <th>@h.traducir("Precio talonario")</th>
        </tr>
    </thead>
    <tbody>
        @For Each g In Model
            @code
            Dim mes As Boolean = True
            End Code
            @For Each el In g.list
                @<tr>
                    @If mes Then
                        @<td rowspan="@g.count">
                            @g.key.toshortdatestring()
                        </td>
                        @code
                        mes = False
                        End Code
                        @<td>@el.tipo</td>
                        @<td>@CDec(el.precio).ToString("###,###.00")€</td>
                        @<td>@CDec(el.precio * el.numeroCheques).ToString("###,###.00") </td>
                    End If
                </tr>
            Next
                Next
    </tbody>
</table>
