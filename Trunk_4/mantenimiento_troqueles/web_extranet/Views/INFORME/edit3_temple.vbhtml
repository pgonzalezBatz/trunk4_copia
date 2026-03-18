@ModelType web_extranet.INFORMES
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

<h4>@h.traducir("Datos del informe")</h4>
<form action="@Url.Action("createEditSoldadura", h.ToRouteValues(Request.QueryString, Nothing))" method="post" class="form for">
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
        <div Class="row">
            <div Class="col-sm-4">
                <div Class="form-group">
                    <Label>@h.traducir("Notas")</label>
                    @Html.TextAreaFor(Function(m) m.NOTAS, New With {.class = "form-control"})
                </div>
            </div>
            <div class="col-sm-4">
                
            </div>
        </div>
        <h4>@h.traducir("Temple")</h4>
        <div class="row">
            <div class="col-sm-4">
                <div class="form-group">
                    <label>@h.traducir("Dureza requerida")</label>
                    @Html.Hidden("DUREZAREQUERIDATEMPLE", ViewData("durezarequerida"))
                    @Html.TextBox("DUREZAREQUERIDATEMPLE_txt", ViewData("durezarequerida"), New With {.class = "form-control", .disabled = "true"})
                    </div>
            </div>
            <div class="col-sm-4">
                <div class="form-group">
                    <label>@h.traducir("Dureza real HRC Max")</label>
                    @Html.TextBoxFor(Function(m) m.DUREZAREALTEMPLEMAX, New With {.class = "form-control"})
                </div>
            </div>
            <div class="col-sm-4">
                <div class="form-group">
                    <label>@h.traducir("Dureza real HRC Min")</label>
                    @Html.TextBoxFor(Function(m) m.DUREZAREALTEMPLEMIN, New With {.class = "form-control"})
                </div>
            </div>
            </div>
    <div class="row">
        <div class="col-sm-4">
            <div class="form-group">
                <label>@h.traducir("Numero de medidas")</label>
                @Html.TextBoxFor(Function(m) m.NUMEROMEDIDASTEMPLE, New With {.class = "form-control"})
            </div>
        </div>
        <div class="col-sm-4">
            <div class="form-group">
                <label>@h.traducir("Metros tratados")</label>
                @Html.TextBoxFor(Function(m) m.METROS, New With {.class = "form-control"})
            </div>
        </div>
        <div class="col-sm-4">
          
        </div>
    </div>
    <h4>@h.traducir("Parametros")</h4>
    <div class="row">
        <div class="col-sm-4">
            <div class="form-group">
                <label>@h.traducir("Temperatura C")</label>
                @Html.TextBoxFor(Function(m) m.TEMPERATURATEMPLE, New With {.class = "form-control"})
            </div>
        </div>
        <div class="col-sm-4">
            <div class="form-group">
                <label>@h.traducir("Optica/Paso")</label>
                @Html.TextBoxFor(Function(m) m.OPTICAPASO, New With {.class = "form-control"})
            </div>
        </div>
        <div class="col-sm-4">
            <div class="form-group">
                <label>@h.traducir("F/Avance (mm/min)")</label>
                @Html.TextBoxFor(Function(m) m.F, New With {.class = "form-control"})
            </div>
        </div>
    </div>
    <h4>@h.traducir("Control de deformaciones")</h4>
    <div class="row">
        <div class="col-sm-4">
            <div class="form-group">
                <label>@h.traducir("Antes de tratamiento")</label>
                @Html.TextBoxFor(Function(m) m.ANTESTRATAM, New With {.class = "form-control"})
            </div>
        </div>
        <div class="col-sm-4">
            <div class="form-group">
                <label>@h.traducir("Despues de tratamiento")</label>
                @Html.TextBoxFor(Function(m) m.DESPUESTRATAM, New With {.class = "form-control"})
            </div>
        </div>
        <div class="col-sm-4">
            <div class="form-group">
                <label>@h.traducir("Magnitud")</label>
                @Html.TextBoxFor(Function(m) m.CALZO, New With {.class = "form-control"})
            </div>
        </div>
    </div>
        <div class="row">
            <div class="col-sm-2">
                <div class="form-group">
                    @If Model.IDINFORME = 0 Then
                        @<input type="submit" value="@h.traducir("Crear informe")" Class="btn btn-primary" />
                    Else
                        @<input type="submit" value="@h.traducir("Guardar cambios")" Class="btn btn-primary" />
                    End If
                    
                </div>
            </div>
        </div>
</form>
