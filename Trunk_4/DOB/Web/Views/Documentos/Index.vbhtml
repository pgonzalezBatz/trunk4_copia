@Code
    Layout = Nothing
End Code

@Imports DOBLib

@code
    Dim listaDocumentos As List(Of ELL.Documento) = CType(ViewData("Documentos"), List(Of ELL.Documento))
End Code

    @If (listaDocumentos.Count = 0) Then
        @Html.Label(String.Empty, Utils.Traducir("noExisteNingunRegistro"))
    Else

            @<table class="table table-striped table-hover table-condensed">
                <thead>
                    <tr>
                        <th>@Utils.Traducir("Fecha alta")</th>
                        <th>@Utils.Traducir("Nombre")</th>
                        <th>@Utils.Traducir("Subido por")</th>
                    </tr>
                </thead>
                <tbody>
                    @code
                        For Each documento In listaDocumentos.OrderByDescending(Function(f) f.FechaAlta)
                @<tr>
                    <td>
                        @documento.FechaAlta.ToShortDateString()
                    </td>
                    <td>
                        <a href='@Url.Action("Mostrar", "Documentos", New With {.idDocumento = documento.Id})'>
                            @documento.NombreFichero
                        </a>
                    </td>
                    <td>
                        @documento.NombreUsuario
                    </td>
                </tr>
                        Next
                    End Code
                </tbody>
            </table>
                        End If
