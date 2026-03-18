@ModelType coberturaPuesto
@imports web
@Code
    ViewBag.title = "Crear - Editar solicitud"
End Code

@section  header
    <link href="//intranet2.batz.es/baliabideorokorrak/jquery-ui-1.11.4.min.css" type="text/css" rel="stylesheet" />
End Section
<script type="text/javascript" src="@Url.Content("~/Scripts/select_dependency.js")"></script>
<script src="//intranet2.batz.es/baliabideorokorrak/jquery-1.11.0.min.js" type="text/javascript"></script>
<script src='//intranet2.batz.es/baliabideorokorrak/jquery-ui-1.11.4.min.js' type="text/javascript"></script>
<script src='//intranet2.batz.es/baliabideorokorrak/jquery.ui.datepicker-@(H.GetCulture().Split("-")(0)).js' type="text/javascript"></script>
<h3 class="my-3">@H.Traducir("Datos de la solicitud")</h3>
@BatzHelpers.ValidationSummaryBootstrap(ViewData.ModelState, Me.Html)
<form action="" method="post">
    <div class="row">
        <div class="col">
            <h4>
                @H.Traducir("Definición del puesto de trabajo")
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
                @Html.DropDownList("departamento", Nothing, New With {.class = "form-control"})
            </div>
            <div class="form-group">
                @code
                    Html.RenderPartial("userSearch")
                End Code
            </div>
            <div class="form-group">
                <label>
                    @H.Traducir("Profesión-Puesto")

                </label>
                @Html.TextBox("puesto", Nothing, New With {.class = "form-control"})
            </div>
            <div class="form-group">
                <label>
                    @H.Traducir("Nº de personas")
                </label>
                @Html.TextBox("npersonas", If(Model Is Nothing OrElse Model.nPersonas = 0, "", Model.nPersonas), New With {.class = "form-control"})
            </div>
            <div class="form-group">
                <label>
                    @H.Traducir("Plan de gestión")
                </label>

                @If Model.pgestion Is Nothing Then
                    @H.Traducir("Si")
                    @<input type="radio" name="pgestion" value="true" />
                    @H.Traducir("No")
                    @<input type="radio" name="pgestion" value="false" />
                Else
                    @If Model.pgestion Then
                        @H.Traducir("Si")
                        @<input type="radio" name="pgestion" value="true" checked="checked" />
                        @H.Traducir("No")
                        @<input type="radio" name="pgestion" value="false" />
                    Else
                        @H.Traducir("Si")
                        @<input type="radio" name="pgestion" value="true" />
                        @H.Traducir("No")
                        @<input type="radio" name="pgestion" value="false" checked="checked" />
                    End If
                End If
            </div>
            <div class="form-group">
                <label>
                    @H.Traducir("Puesto estructural")
                </label>
                <br />
                @If Model.pestructural Is Nothing Then
                    @H.Traducir("Si")
                    @<input type="radio" name="pestructural" value="true" />
                    @H.Traducir("No")
                    @<input type="radio" name="pestructural" value="false" />
                Else
                    If Model.pestructural Then
                        @H.Traducir("Si")
                        @<input type="radio" name="pestructural" value="true" checked="checked" />
                        @H.Traducir("No")
                        @<input type="radio" name="pestructural" value="false" />
                    Else
                        @H.Traducir("Si")
                        @<input type="radio" name="pestructural" value="true" />
                        @H.Traducir("No")
                        @<input type="radio" name="pestructural" value="false" checked="checked" />
                    End If
                End If
            </div>
            <div class="form-group">
                <label>
                    @H.Traducir("Descripción de funciones y responsabilidades")
                </label>
                @Html.TextArea("descripcion", Nothing, New With {.class = "form-control"})
            </div>

            <h4>
                @H.Traducir("Condiciones de contratación")
            </h4>
            <div class="form-group">
                <label>
                    @H.Traducir("Fecha prevista incorporación")
                </label>
                @Html.TextBox(name:="fecha", value:=Nothing, format:="{0:" + System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern + "}", htmlAttributes:=New With {.class = "calendar form-control"})
            </div>
            <div class="form-group">
                <label>
                    @H.Traducir("Duración del contrato")
                </label>
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
                @H.Traducir("Requisitos para el puesto")
            </h4>

            <div class="form-group">
                <label>
                    @H.Traducir("Formación")
                </label>
                @Html.TextBox("formacion", Nothing, New With {.class = "form-control"})
            </div>
            <div class="form-group">
                <label>
                    @h.traducir("Especialidad")
                </label>
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
                        @H.Traducir("Idiomas")
                    </label>
                    <br />
                    @Html.TextBox("idiomas", Nothing, New With {.class = "form-control"})
                </div>
                <div class="form-group">
                    <label>
                        @H.Traducir("Experiencia")
                    </label>
                    <br />
                    @Html.TextBox("experiencia", Nothing, New With {.class = "form-control"})
                </div>


                <h4>
                    @H.Traducir("Se valorará")
                </h4>
                <div class="form-group">
                    <label>
                        @H.Traducir("Formación complementaria en")
                    </label>
                    @Html.TextBox("formacion2", Nothing, New With {.class = "form-control"})
                </div>
                <div class="form-group">
                    <label>
                        @H.Traducir("Conocimientos específicos")
                    </label>
                    @Html.TextBox("conocimientos2", Nothing, New With {.class = "form-control"})
                </div>
                <div class="form-group">
                    <label>
                        @H.Traducir("Idiomas")
                    </label>
                    @Html.TextBox("idiomas2", Nothing, New With {.class = "form-control"})
                </div>
                <div class="form-group">
                    <label>
                        @H.Traducir("Experiencia")
                    </label>
                    <br />
                    @Html.TextBox("experiencia2", Nothing, New With {.class = "form-control"})
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
