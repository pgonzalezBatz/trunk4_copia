@imports web
@ModelType Web.proveedor

@section title
    - @h.traducir("Detalle")
End section
@section menu1
    @Html.Partial("menu")
End Section




<div class="container-fluid">
    <div class="row">
        <div class="col-sm-12">
            @Html.ActionLink(h.traducir("Volver al listado"), "search", h.ToRouteValuesDelete(Request.QueryString, "id"))
        </div>
        
    </div>
    <div class="row">
        <div class="col-sm-6">
            <h3>@h.traducir("Datos del proveedor")</h3>
            <div class="display-label">
                <strong>@System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("cif"))</strong>
            </div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.cif)
            </div>
            <div class="display-label">
                <strong>@System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("codpro"))</strong>
            </div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.codpro)
            </div>
            <div class="display-label">
                <strong>@System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("Razon Social"))</strong>
            </div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.RazonSocial)
            </div>
            <div class="display-label">
                <strong>@System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("nombre"))</strong>
            </div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.nombre)
            </div>
        </div>
        <div class="col-sm-6">
            <h3>@h.traducir("Datos de la dirección")</h3>

            <div class="display-label">
                <strong>@System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("direccion"))</strong>
            </div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.direccion)
            </div>
            <div class="display-label">
                <strong>@System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("codigo Postal"))</strong>
            </div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.codigoPostal)
            </div>
            <div class="display-label">
                <strong>@System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("localidad"))</strong>
            </div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.localidad)
            </div>
            <div class="display-label">
                <strong>@System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("provincia"))</strong>
            </div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.provincia)
            </div>
            <div class="display-label">
                <strong>@System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("nombre Pais"))</strong>
            </div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.nombrePais)
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-6">
            <h3>@h.traducir("Datos de contacto")</h3>

            <div class="display-label">
                <strong>@System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("contacto"))</strong>
            </div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.contacto)
            </div>
            <div class="display-label">
                <strong>@System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("telefono"))</strong>
            </div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.telefono)
            </div>
            <div class="display-label">
                <strong>@System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("fax"))</strong>
            </div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.fax)
            </div>
            <div class="display-label">
                <strong>@System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("Usuario extranet"))</strong>
            </div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.nombreUsuario)
            </div>
            <div class="display-label">
                <strong>@System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("Email Pedidos"))</strong>
            </div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.email)
            </div>

            <div class="display-label">
                <strong>@System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("email 2"))</strong>
            </div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.email2)
            </div>

            <div class="display-label">
                <strong>@System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("email facturación"))</strong>
            </div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.EmailFacturacion)
            </div>
        </div>
        <div class="col-sm-6">
            <h3>@h.traducir("Otros datos")</h3>

            <div class="display-label">
                <strong>@System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("nombre F Pago"))</strong>
            </div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.nombreFPago)
            </div>
            <div class="display-label">
                <strong>@System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("moneda"))</strong>
            </div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.descMoneda)
            </div>


            @If Model.fechaAlta.HasValue Then
                @<div class="display-label">
                    <strong>@System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("fecha Alta"))</strong>
                </div>
                @<div class="display-field">
                    @Model.fechaAlta.Value.ToShortDateString
                </div>            End If

            @If Model.fechaBaja.HasValue Then
                @<div class="display-label">
                    <strong>@System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("fecha Baja"))</strong>
                </div>
                @<div class="display-field">
                    @Model.fechaBaja.Value.ToShortDateString
                </div>            End If

            <div class="display-label">
                <strong>@System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("id Planta"))</strong>
            </div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.idPlanta)
            </div>

            <div class="display-label">
                <strong>@System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("porte Troq"))</strong>
            </div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.porteTroq)
            </div>

            <div class="display-label">
                <strong>@System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("tipo Proveedor Sis"))</strong>
            </div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.tipoProveedorSis)
            </div>

            <div class="display-label">
                <strong>@System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("codigo Iva"))</strong>
            </div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.codigoIva)
            </div>

            <div class="display-label">
                <strong>@System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("numero Abreviado"))</strong>
            </div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.numeroAbreviado)
            </div>
            <div class="display-label">
                <strong>@System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("Creadora"))</strong>
            </div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.nombreCreador) @Html.DisplayFor(Function(model) model.apellido1Creador)
            </div>
            <div class="display-label">
                <strong>@System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("Notificado"))</strong>
            </div>
            <div class="display-field">
                @if Model.notificado Then
                    @h.Traducir("El proveedor ha sido notificado de la Extranet")
                Else
                    @h.Traducir("El proveedor NO ha sido notificado de la Extranet")
                End If
            </div>

        </div>
        </div>
    <div Class="row">
        <div Class="col-sm-6">
            @If Model.comentarios.Length > 0 Then
                @<div class="display-label">
                    <h3>@System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("comentarios"))</h3>
                </div>
                @<div class="display-field">
                    @Html.Raw(Model.comentarios.Replace(Environment.NewLine, "<br/>"))
                </div>
            End If

            @If ViewData("adjuntos").count > 0 Then
                @<div class="display-label">
                    <h3>@h.traducir("Documentos adjuntos")</h3>
                </div>
                @<div class="display-field">
                    @For Each a In ViewData("adjuntos")
                        @<a href="@Url.Action("adjunto", New With {.idEmpresa = Request("id"), .idAdjunto = a.id})">@a.nombre</a>
                    Next
                </div>
            End If
        </div>
        <div class="col-sm-6">
            <h3>@h.traducir("Homologaciones")</h3>

            <div class="display-label">
                <strong>@System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("Homologado"))</strong>
            </div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.homologado)
            </div>
            <div class="display-label">
                <strong>@System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("Clasificación"))</strong>
            </div>
            <div class="display-field">
                @Html.DisplayFor(Function(model) model.clasificacion)
            </div>
        </div>
        </div>
</div>
    
   
