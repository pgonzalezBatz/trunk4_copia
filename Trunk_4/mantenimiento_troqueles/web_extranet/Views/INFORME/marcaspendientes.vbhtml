@ModelType IEnumerable(Of Object)

@Code
End Code
        <h3>@h.traducir("Marcas Pendientes de Informe")</h3>
    <hr />

    <table class="table table-hover">
        <thead>
            <tr>
                <th>@h.traducir("OF")</th>
                <th>@h.traducir("OP")</th>
                <th>@h.traducir("Marcas")</th>
            </tr>
        </thead>
    @For Each item In Model
        @If item.lstMarca.count > 0 Then
            @<tr>
                <td>
                    @item.numord
                </td>
                <td>
                    @item.numope
                </td>
                <td>
                    @Html.Encode("| ")
                    @For Each m In item.lstMarca
                        @m.marca
                        @Html.Encode(" | ")
                    Next
                </td>
            </tr>
        End If
    Next
    
    </table>
