@imports web
@section title
    - @h.traducir("Exportar proveedor a plantas de sistemas")
End section

@Html.ValidationSummary()
@If Model Is Nothing Then

Else
    @<table>
        <thead>
            <tr>
                <th>
                    @h.traducir("Planta")
                </th>
                <th>
                    @h.traducir("Codigo proveedor")
                </th>
            </tr>
        </thead>
        <tbody>
            @For Each item In Model
                @<tr>
                    <td>
                        @item.nombre
                    </td>
                    <td>
                        @If item.existe Then
                            @item.codigoProveedor
                        Else
                            @<form action="" method="post">
                                @Html.Hidden("idPlantaDestino", item.idPlanta)
                                <input type="submit" value="@h.traducir("Exportar")" />
                            </form>
                        End If
                    </td>   
                </tr>
            Next
        </tbody>
    </table>
End If
