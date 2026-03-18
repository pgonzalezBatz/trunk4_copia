@imports web
@ModelType Web.proveedor

@section title
    - create
End section
@section header
<link href="//intranet2.batz.es/baliabideorokorrak/jquery-ui-1.11.4.min.css" rel="Stylesheet" type="text/css" />
End Section
@section scripts
    <script src='//intranet2.batz.es/baliabideorokorrak/jquery-ui-1.11.4.min.js' type="text/javascript"></script>
    <script src='//intranet2.batz.es/baliabideorokorrak/jquery.ui.datepicker-@(h.GetCulture().Split("-")(0)).js' type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('.calendar').datepicker($.datepicker.regional["@h.GetCulture().Split(" - ")(0)"]);
        });
        $('form').on('submit', function () {
            $('#btnCrear').prop('disabled', true);
        });
    </script>
End Section
<div class="container-fluid">
    <h3>@h.traducir("Datos del proveedor")</h3>
    <form action="" method="post" class="form-horizontal">
        @Html.AntiForgeryToken()

        @Html.ValidationSummary()
        <div class="form-group">
            <div class="col-sm-2">
                <label for="cif">@h.traducir("cif")</label>
                @Html.TextBoxFor(Function(model) model.cif, New With {.maxlength = 15, .class = "form-control"})
                @Html.ValidationMessageFor(Function(model) model.cif)
            </div>
            <div class="col-sm-4">
                <label for="cif">@h.traducir("Razon Social")</label>
                @Html.TextBoxFor(Function(model) model.RazonSocial, New With {.maxlength = 70, .class = "form-control"})
                @Html.ValidationMessageFor(Function(model) model.RazonSocial)
            </div>
            <div class="col-sm-6">
                <label for="nombre">@h.traducir("nombre")</label>
                @Html.TextBoxFor(Function(model) model.nombre, New With {.maxlength = 100, .class = "form-control"})
                @Html.ValidationMessageFor(Function(model) model.nombre)
            </div>

        </div>

        <h4>@h.traducir("Datos de la dirección")</h4>
        <div class="form-group">
            <div class="col-sm-6">
                <label for="direccion">@h.traducir("direccion")</label>
                <div>
                    @Html.TextBoxFor(Function(model) model.direccion, New With {.maxlength = 35, .class = "form-control"})
                    @Html.ValidationMessageFor(Function(model) model.direccion)
                </div>
            </div>
            <div class="col-sm-6">
                <label for="codigoPostal">@h.traducir("codigoPostal")</label>
                @Html.TextBoxFor(Function(model) model.codigoPostal, New With {.maxlength = 10, .class = "form-control"})
                @Html.ValidationMessageFor(Function(model) model.codigoPostal)
            </div>
        </div>

        <div class="form-group">
            <div class="col-sm-6">
                <label for="localidad">@h.traducir("localidad")</label>
                @Html.TextBoxFor(Function(model) model.localidad, New With {.maxlength = 35, .class = "form-control"})
                @Html.ValidationMessageFor(Function(model) model.localidad)
            </div>
            <div class="col-sm-6">
                <label for="provincia">@h.traducir("provincia")</label>
                @Html.TextBoxFor(Function(model) model.provincia, New With {.maxlength = 35, .class = "form-control"})
                @Html.ValidationMessageFor(Function(model) model.provincia)
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-6">
                <label for="pais">@h.traducir("pais")</label>
                @Html.DropDownList("pais", Nothing, New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(model) model.pais)
            </div>
        </div>
        <h3>@h.traducir("Datos de contacto")</h3>
        <div class="form-group">
            <div class="col-sm-6">
                <label for="contacto">@h.traducir("contacto")</label>
                @Html.TextBoxFor(Function(model) model.contacto, New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(model) model.contacto)
            </div>
            <div class="col-sm-6">
                <label for="telefono">@h.traducir("telefono")</label>
                @Html.TextBoxFor(Function(model) model.telefono, New With {.maxlength = 20, .class = "form-control"})
                @Html.ValidationMessageFor(Function(model) model.telefono)
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-6">
                <label for="fax">@h.traducir("fax")</label>
                @Html.TextBoxFor(Function(model) model.fax, New With {.maxlength = 20, .class = "form-control"})
                @Html.ValidationMessageFor(Function(model) model.fax)
            </div>
            <div class="col-sm-6">
                <label for="email" class="">@h.Traducir("Usuario Extranet (email)")</label>

                <div class="input-group">
                    <span class="input-group-addon">@@</span>
                    @Html.TextBoxFor(Function(model) model.nombreUsuario, New With {.class = "form-control"})
                </div>
                @Html.ValidationMessageFor(Function(model) model.nombreUsuario)
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-6">
                <label for="email">@h.traducir("Email Pedidos")</label>
                <div class="input-group">
                    <span class="input-group-addon">@@</span>
                    @Html.TextBoxFor(Function(model) model.email, New With {.class = "form-control"})
                </div>
                @Html.ValidationMessageFor(Function(model) model.email)
            </div>
            <div class="col-sm-6">
                <label for="email2">@h.traducir("email") 2</label>
                <div class="input-group">
                    <span class="input-group-addon">@@</span>
                    @Html.TextBoxFor(Function(model) model.email2, New With {.class = "form-control"})
                </div>
                @Html.ValidationMessageFor(Function(model) model.email2)
            </div>
        </div>


        <div class="form-group">
            <div class="col-sm-6">
                <label for="email">@h.traducir("Email Facturación")</label>
                <div class="input-group">
                    <span class="input-group-addon">@@</span>
                    @Html.TextBoxFor(Function(model) model.EmailFacturacion, New With {.class = "form-control"})
                </div>
                @Html.ValidationMessageFor(Function(model) model.EmailFacturacion)
            </div>
        </div>


        <h3>@h.traducir("Otros datos")</h3>
        <div class="form-group">
            <div class="col-sm-6">
                <label for="fPago">@h.traducir("Forma de pago")</label>
                @Html.DropDownListFor(Function(model) model.fPago, CType(ViewBag.formapago, IEnumerable(Of SelectListItem)), htmlAttributes:=New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(model) model.fPago)
            </div>
            <div class="col-sm-6">
                <label for="modeda">@h.traducir("moneda")</label>
                @Html.DropDownListFor(Function(model) model.moneda, CType(ViewBag.monedas, IEnumerable(Of SelectListItem)), New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(model) model.moneda)
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-6">
                <label for="porteTroq">@h.traducir("Portes") (@h.traducir("Troqueleria"))</label>
                @Html.DropDownListFor(Function(model) model.porteTroq, CType(ViewBag.porteTroq, IEnumerable(Of SelectListItem)), New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(model) model.porteTroq)
            </div>
            <div class="col-sm-6">
                <label for="tipoProveedorSis">@h.traducir("Tipo de proveedor") (@h.traducir("Sistemas"))</label>
                @Html.DropDownListFor(Function(model) model.tipoProveedorSis, CType(ViewBag.tipoProveedorSis, IEnumerable(Of SelectListItem)), h.traducir("[Seleccionar]"), New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(model) model.tipoProveedorSis)
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-6">
                <label for="comentarios">@h.traducir("Comentarios")</label>
                @Html.TextAreaFor(Function(model) model.comentarios, New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(model) model.comentarios)
            </div>
            <div class="col-sm-6">
                <label for="fechaBaja">@h.traducir("fechaBaja")</label>
                @Html.TextBoxFor(Function(model) model.fechaBaja, New With {.class = "form-control calendar"})
                @Html.ValidationMessageFor(Function(model) model.fechaBaja)
            </div>
        </div>
        <div class="row">
            <div class="col-sm-3">
                <input type="submit" id="btnCrear" value="@h.Traducir("Crear")" class="btn btn-primary btn-block" />
            </div>
            <div class="col-sm-4">
                @Html.ActionLink(h.traducir("Volver al listado"), "search")
            </div>
        </div>


    </form>

    
</div>
