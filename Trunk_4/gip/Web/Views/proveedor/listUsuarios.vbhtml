@imports web
@section title
    - @h.Traducir("Listado de usuarios del proveedor")
End section
@section menu1
    @Html.Partial("menu")
End Section
@*@Code
    Dim strCnOracle As String = ConfigurationManager.ConnectionStrings("oracle").ConnectionString
    Dim isGlobal As Boolean = Db.EsProveedorGlobal(ViewData("IdEmpresa"), strCnOracle)
End Code*@
<table class="table">
    <thead>
        <tr>
            <th>
                @System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.Traducir("Id"))
            </th>
            <th>
                @System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.Traducir("Nombre"))
            </th>
            <th>
                @System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.Traducir("Origen"))
            </th>
            <th>
                @System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.Traducir("Planta"))
            </th>
            <th>
                @System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.Traducir("Tipo Usuario"))
            </th>
            <th>
                @System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.Traducir("Grupos"))
            </th>
        </tr>
    </thead>
    <tbody>
        @For Each l In Model
            @<tr  class="@(If(IsDate(l.fechabaja), "danger", ""))">
                 <td>@l.id</td>            
                <td>@l.nombre</td>
                <td>
                    @If l.usuarioEmpresa Then
                        @h.Traducir("GIP")
                    Else
                        @h.Traducir("SAB Proveedores")
                    End If
                                    </td>
        <td>
            @l.nombrePlanta
        </td>
        <td>
            @If l.idUsuarioAdministrador = l.id Then
              @h.Traducir("Administrador")
            End If
        </td>
        <td>
            @Html.Raw(Html.Encode(l.grupos).Replace("|", "<br />"))

        </td>
            </tr>
        Next
    </tbody>
    
</table>
