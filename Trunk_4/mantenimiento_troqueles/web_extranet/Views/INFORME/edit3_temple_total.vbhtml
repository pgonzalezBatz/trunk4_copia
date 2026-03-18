@ModelType web_extranet.INFORMES
@section scripts
    <script type="text/javascript">
        $(document).on('change', ':file', function () {
            var input = $(this),
                numFiles = input.get(0).files ? input.get(0).files.length : 1,
                label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
            $("#file_feedback").html(label);
        });
    </script>
End Section


<h3>@h.traducir("Creación de informe")</h3>
<hr />
@Html.ValidationSummary(False, "", New With {.class = "alert alert-danger"})

    <div class="row">
        <div class="col-sm-6">
            <h4>@h.traducir("Datos seleccionados")</h4>
            </div>
        </div>
    <div class="row">
        <div class="col-sm-4">
            <label>@h.traducir("Tipo informe seleccionado")</label>
            <p>@Model.TIPOINFORME</p>
        </div>
        <div class="col-sm-4">
            <label>@h.traducir("Cliente")</label>
            <p>@Model.CLIENTE</p>
        </div>
        <div class="col-sm-4">
            <label>@h.traducir("Proyecto")</label>
            <p>@Model.proyecto</p>
        </div>
        </div>

    <div class="row">
        <div class="col-sm-4">
            <label>@h.traducir("OF - OP seleccionada")</label>
            <p>@Model.valorof - @Model.VALOROP</p>
        </div>
        <div Class="col-sm-4">
            <label>@h.traducir("Nº pieza")</label>
            <p>@Model.NPIEZA</p>
        </div>   
        <div Class="col-sm-4">
            <label>@h.traducir("Descripción pieza")</label>
            <p>@Model.DESCPIEZA</p>
        </div>
       </div>
    <div class="row">
        <div class="col-sm-4">
            <label>@h.traducir("Marcas seleccionada")</label>
            <p>
                @Html.Encode(" | ")
                @For Each m In ViewData("marca")
                    @m.marca @Html.Encode(" | ")
                Next
            </p>
        </div>
        <div Class="col-sm-4">
            <label>@h.traducir("Nº de troquel")</label>
            <p>@Model.NTROQUEL</p>
        </div>
        <div Class="col-sm-4">
            <label>@h.traducir("Material")</label>
            <p>@ViewData("comunesMarca").material</p>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-4">
          
        </div>
        <div Class="col-sm-4">
            <label>@h.traducir("Tratamiento Requerido")</label>
            <p>@ViewData("comunesMarca").tratamiento</p>
        </div>
        <div Class="col-sm-4">
            <label>@h.traducir("Dureza Material") <span class="glyphicon glyphicon-info-sign text-info" title="@h.traducir("Es la dureza del material suministrado")"></span></label>
            <p>@ViewData("comunesMarca").dureza</p>
            </div>
    </div>
    <div class="row">
        <div class="col-sm-4">

        </div>
        <div Class="col-sm-4">
            <label>@h.traducir("Tratamiento Secundario")</label>
            <p>@ViewData("comunesMarca").tratamientosecundario</p>
        </div>
        <div Class="col-sm-4">
        </div>
    </div>

<h4>@h.traducir("Adjuntar informe")</h4>
<form action="@Url.Action("AttachTempletotal", h.ToRouteValues(Request.QueryString, Nothing))" method="post" enctype="multipart/form-data">
    @Html.HiddenFor(Function(m) m.IDINFORME)
    @Html.HiddenFor(Function(m) m.TIPOINFORME)
    @Html.HiddenFor(Function(m) m.VALOROF)
    @Html.HiddenFor(Function(m) m.VALOROP)
    @Html.Hidden("marca", String.Join("|", CType(ViewData("marca"), IEnumerable(Of marca)).Select(Function(m) m.marca)))
    @Html.HiddenFor(Function(m) m.CLIENTE)
    @Html.HiddenFor(Function(m) m.PROYECTO)
    @Html.HiddenFor(Function(m) m.NPIEZA)
    @Html.HiddenFor(Function(m) m.DESCPIEZA)
    @Html.HiddenFor(Function(m) m.NTROQUEL)
    @Html.HiddenFor(Function(m) m.CREADOPOR)
    @Html.Hidden("material", ViewData("comunesMarca").material)
    @Html.Hidden("tratamsec", ViewData("comunesMarca").tratamientosecundario)
    @Html.Hidden("tratamiento", ViewData("comunesMarca").tratamiento)
    @Html.Hidden("dureza", ViewData("comunesMarca").dureza)
    @Html.Hidden("material", ViewData("comunesMarca").material)
    @Html.Hidden("numpedlin")
    <div class="input-group">
        <div id="file_feedback" class="input-group-addon"></div>
        <label for="file" class="btn btn-default btn-file">
            @h.traducir("Elegir archivo")
            <input type="file" name="file" id="file" style="display: none;"  accept="application/pdf" />
        </label>
    </div>
    <div class="input-group">
        <input type="submit" class="btn btn-primary" value="@h.traducir("Subir")" />
    </div>
</form>
<br />
@If ViewData("lstInformes") IsNot Nothing Then
    @<table class="table table-responsive">
        @for each r As CONTIENE In ViewData("lstInformes")
            @<tr>
                <td>
                    <a href="@Url.Action("displayAttachmentTempleTotal", h.ToRouteValues(Request.QueryString, New With {.idcertificado = r.IDCERTIFICADO}))">@h.traducir("Ver informe")</a>
                </td>
    <td><a href="@Url.Action("deleteAttachmentTempleTotal", h.ToRouteValues(Request.QueryString, New With {.idcertificado = r.IDCERTIFICADO}))">@h.traducir("Eliminar informe")</a></td>
            </tr>
        Next
    </table>
End If


