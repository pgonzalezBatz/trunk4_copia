@imports web
@Code
    ViewBag.title = "Crear - Editar solicitud becari@"
End Code
@section  header
    <link href="//intranet2.batz.es/baliabideorokorrak/jquery-ui-1.11.4.min.css" type="text/css" rel="stylesheet" />
End Section
<script src="//intranet2.batz.es/baliabideorokorrak/jquery-1.11.0.min.js" type="text/javascript"></script>
<script src='//intranet2.batz.es/baliabideorokorrak/jquery-ui-1.11.4.min.js' type="text/javascript"></script>
<script src='//intranet2.batz.es/baliabideorokorrak/jquery.ui.datepicker-@(H.GetCulture().Split("-")(0)).js' type="text/javascript"></script>
<script type="text/javascript" src="@Url.Content("~/Scripts/select_dependency.js")"></script>
<script src='http://intranet2.batz.es/baliabideorokorrak/ui.datepicker.@h.GetCulture()@Html.Encode(".js")' type="text/javascript"></script>

@BatzHelpers.ValidationSummaryBootstrap(ViewData.ModelState, Me.Html)
<form action="" method="post">
    <div class="row">
        <div class="col">
            <h4>
                @H.Traducir("Definición de las prácticas o proyecto fin de carrera")
            </h4>
            <div class="form-group">
                <label>
                    @H.Traducir("Negocio")

                </label>
                @Html.DropDownList("negocio", Nothing, New With {.class = "form-control"})
            </div>
            <div class="form-group">
                <label>
                    @H.Traducir("Departamento")
                </label>
                <br />
                @Html.DropDownList("departamento", Nothing, New With {.class = "form-control"})
            </div>
            <div class="form-group">
                @code
                    Html.RenderPartial("userSearch")
                End Code
            </div>
            <div class="form-group">
                <label>
                    @H.Traducir("Nº de personas")
                </label>
                <br />
                @Html.TextBox("npersonas", If(Model.npersonas = 0, "", Model.npersonas), New With {.class = "form-control"})
            </div>
            <div class="form-group">
                <label>
                    @H.Traducir("Descripción de las prácticas y las funciones a desempeñar")
                </label>
                <br />
                @Html.TextBox("descripcion", Nothing, New With {.class = "form-control"})
            </div>
            <h4>
                @H.Traducir("Condiciones del convenio")
            </h4>
            <div class="form-group">
                <label>
                    @H.Traducir("Fecha prevista incorporación")
                </label>
                <br />
                @Html.TextBox(name:="fecha", value:=Nothing, format:="{0:" + System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern + "}", htmlAttributes:=New With {.class = "calendar form-control"})
            </div>
            <div class="form-group">
                <label>
                    @H.Traducir("Duración del convenio")
                </label>
                <br />
                @Html.TextBox("duracion", Nothing, New With {.class = "form-control"})
            </div>
            <div class="form-group">
                <label>
                    @H.Traducir("Horario")
                </label>
                <br />
                @Html.DropDownList("horario", Nothing, H.Traducir("seleccionar horario"), New With {.class = "form-control"})
            </div>
           

        </div>
        <div class="col">
            <h4>
                @H.Traducir("Requisitos del becario")
            </h4>
            <div class="form-group">
                <label>
                    @H.Traducir("Titulación")
                </label>
                <br />
                @Html.TextBox("titulacion", Nothing, New With {.class = "form-control"})
            </div>
            <div class="form-group">
                <label>
                    @H.Traducir("Especialidad")
                </label>
                <br />
                @Html.TextBox("especialidad", Nothing, New With {.class = "form-control"})
            </div>
            <div class="form-group">
                <label>
                    @H.Traducir("Conocimientos específicos")
                </label>
                <br />
                @Html.TextBox("conocimientos", Nothing, New With {.class = "form-control"})
            </div>
            <div class="form-group">
                <label>
                    @H.Traducir("Universidad")
                </label>
                <br />
                @Html.TextBox("universidad", Nothing, New With {.class = "form-control"})
            </div>
            <div class="form-group">
                <label>
                    @H.Traducir("Idiomas")
                </label>
                @Html.TextBox("idiomas", Nothing, New With {.class = "form-control"})
            </div>
            <div class="form-group">
                <label>
                    @H.Traducir("Experiencia")
                </label>
                <br />
                @Html.TextBox("experiencia", Nothing, New With {.class = "form-control"})
            </div>
        </div>
    </div>





    <input type="submit" class="btn btn-primary" value="@H.Traducir("Guardar")" />
</form>

<script type="text/javascript">
        $(document).ready(function () {
            var f = function (e) { return "@Url.Action("getListOfDepartamento", "ajax")" + "?idNegocio=" + e };
            cascade_select($("#negocio"), $("#departamento"), f);
            $('.calendar').datepicker($.datepicker.regional["@H.GetCulture().Split(" - ")(0)"]);
        });
</script>