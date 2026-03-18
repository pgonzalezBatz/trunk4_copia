@imports web
@section title
    - @h.traducir("Listado de Loggs de proveedor")
End section
@section menu1
    @Html.Partial("menu")
End Section

<table class="table">
    <thead>
        <tr>
            <th>
                @System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("Responsable cambio"))
            </th>
            <th>
                @System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("Fecha"))
            </th>
            <th>
                @System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("nombre"))
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
                @System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("contacto"))
            </th>
            <th>
                @System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("Email"))
            </th>
            <th>
                @System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("Email2"))
            </th>
            <th>
                @System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("f Pago"))
            </th>
            <th>
                @System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("Localidad"))
            </th>
            <th>
                @System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("Provincia"))
            </th>
            <th>
                @System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("C. Postal"))
            </th>
        </tr>
    </thead>
    <tbody>
        @For Each l In Model
            @<tr>
                 <td>@l.nombreusuario @l.apellido1usuario</td>            
                <td>@l.fecha</td>
                 <td>@l.nombre</td>
                 <td>@l.cif</td>
                 <td>@l.telefono</td>
                 <td>@l.direccion</td>
                 <td>@l.contacto</td>
                 <td>@l.email</td>
                 <td>@l.email2</td>
                 <td>@l.idPago</td>
                 <td>@l.localidad</td>
                 <td>@l.provincia</td>
                 <td>@l.cpostal</td>
            </tr>
        Next
    </tbody>
    
</table>
