@ModelType BezerreSis.RECLAMACIONES_CIERRE
@Code
    ' Título de la página
    ViewData("Title") = "Cerrar Reclamación"

    ' Obtener la reclamación de cierre del modelo
    Dim reclamacion = Model
    Dim myDB As New myDb
    Dim myLocaleForDate = myDB.getLocaleForDatePicker()

    ' Determinar la visibilidad de los campos de fechas basado en ViewBag.Classification
    Dim fechasVisible As String
    If ViewBag.Classification = 2 Then
        fechasVisible = "none"
    Else
        fechasVisible = ""
    End If

    ' Variables para almacenar el HTML de los archivos y mensajes
    Dim archivosHtml As String = ""
    Dim mensajeArchivos As String = ""

    ' Verificar si la reclamación ha sido guardada (ID <> 0)
    If reclamacion.ID <> 0 Then
        ' Verificar si hay archivos
        If ViewBag.Archivos IsNot Nothing AndAlso CType(ViewBag.Archivos, List(Of String)).Any() Then
            ' Construir la tabla de archivos con un margen a la izquierda para desplazarla a la derecha
            archivosHtml &= "<table class=""table table-striped table-condensed"" style=""width: auto; margin-left: 100px;"">" & vbCrLf
            archivosHtml &= "    <thead>" & vbCrLf
            archivosHtml &= "        <tr>" & vbCrLf
            archivosHtml &= "            <th>Archivo</th>" & vbCrLf
            archivosHtml &= "            <th style='text-align:right; width: 500px;'>Acciones</th>" & vbCrLf
            archivosHtml &= "        </tr>" & vbCrLf
            archivosHtml &= "    </thead>" & vbCrLf
            archivosHtml &= "    <tbody>" & vbCrLf

            For Each archivo In CType(ViewBag.Archivos, List(Of String))
                ' Generar URLs utilizando el helper Url.Action
                Dim urlVisualizar As String = Url.Action("DownloadArchivo", "Reclamaciones", New With {.reclamacionId = reclamacion.ID, .nombreArchivo = archivo})
                Dim urlEliminar As String = Url.Action("DeleteArchivo", "Reclamaciones")
                Dim antiforgeryToken As String = Html.AntiForgeryToken().ToString()

                ' Construir cada fila de la tabla con los botones a la derecha
                archivosHtml &= "<tr>" & vbCrLf
                archivosHtml &= $"    <td>{archivo}</td>" & vbCrLf
                archivosHtml &= "    <td style='text-align:right'>" & vbCrLf
                archivosHtml &= $"        <a href=""{urlVisualizar}"" class=""btn btn-success btn-sm mr-2"" target=""_blank"">Visualizar</a>" & vbCrLf
                archivosHtml &= $"        <form action=""{urlEliminar}"" method=""post"" style=""display:inline;"">" & vbCrLf
                archivosHtml &= $"            {antiforgeryToken}" & vbCrLf
                archivosHtml &= $"            <input type=""hidden"" name=""reclamacionId"" value=""{reclamacion.ID}"" />" & vbCrLf
                archivosHtml &= $"            <input type=""hidden"" name=""nombreArchivo"" value=""{archivo}"" />" & vbCrLf
                archivosHtml &= "            <button type=""submit"" class=""btn btn-danger btn-sm"" onclick=""return confirm('¿Estás seguro de que deseas eliminar este archivo?');"">Eliminar</button>" & vbCrLf
                archivosHtml &= "        </form>" & vbCrLf
                archivosHtml &= "    </td>" & vbCrLf
                archivosHtml &= "</tr>" & vbCrLf
            Next

            archivosHtml &= "    </tbody>" & vbCrLf
            archivosHtml &= "</table>" & vbCrLf

        Else
            ' No hay archivos subidos; no mostrar nada
            archivosHtml = ""
        End If
    Else
        ' Reclamación no guardada aún; no mostrar nada
        archivosHtml = ""
    End If
End Code

<br />
<h2>Cerrar Reclamación</h2>
<br />
@Using (Html.BeginForm("Close", "Reclamaciones", FormMethod.Post, New With {.enctype = "multipart/form-data"}))

    @Html.AntiForgeryToken()

    @<div class="form-horizontal">
        @Html.ValidationSummary(True, "", New With {.class = "text-danger"})

        <div class="form-group col-md-6" style="display:@fechasVisible">
            @Html.LabelFor(Function(model) model.FECHA_RESP_CONTENCION, htmlAttributes:=New With {.class = "control-label col-sm-6"})
            <div class="col-sm-6">
                <div class="col-md-12">
                    <div class="form-group">
                        <div class='input-group myDatePicker' id='datetimepicker1'>
                            @*@Html.TextBox("FECHA_RESP_CONTENCION", If(reclamacion Is Nothing OrElse reclamacion.FECHA_RESP_CONTENCION Is Nothing, "", reclamacion.FECHA_RESP_CONTENCION.Value.ToString("yyyy/MM/dd")), New With {.Class = "form-control"})*@
                            @Html.TextBox("FECHA_RESP_CONTENCION", If(reclamacion Is Nothing OrElse reclamacion.FECHA_RESP_CONTENCION Is Nothing, "", Date.ParseExact(reclamacion.FECHA_RESP_CONTENCION, "dd/MM/yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo).ToString("yyyy/MM/dd")), New With {.Class = "form-control"})

                            @*<input id="FECHA_RESP_CONTENCION" class="form-control" />*@
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>
                @Html.ValidationMessageFor(Function(model) model.FECHA_RESP_CONTENCION, "", New With {.class = "text-danger"})
            </div>
        </div>

        @*<div class='input-group date myDatePicker' id='datetimepicker_1'>
                @Html.TextBox("FECHA_DESDE", If(ViewBag.FECHA_DESDE Is Nothing, Date.ParseExact("01/01/2019", "dd/MM/yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo).ToString("yyyy/MM/dd"), Date.ParseExact(ViewBag.FECHA_DESDE, "dd/MM/yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo).ToString("yyyy/MM/dd")), New With {.class = "form-control"})
                <span class="input-group-addon">
                    <span class="glyphicon glyphicon-calendar"></span>
                </span>
            </div>*@

        <div class="form-group col-md-6" style="display:@fechasVisible">
            @Html.LabelFor(Function(model) model.FECHA_RESP_CORRECTIVAS, htmlAttributes:=New With {.class = "control-label col-sm-6"})
            <div class="col-sm-6">
                <div class="col-md-12">
                    <div class="form-group">
                        <div class='input-group myDatePicker' id='datetimepicker2'>
                            @Html.TextBox("FECHA_RESP_CORRECTIVAS", If(reclamacion Is Nothing OrElse reclamacion.FECHA_RESP_CORRECTIVAS Is Nothing, "", Date.ParseExact(reclamacion.FECHA_RESP_CORRECTIVAS, "dd/MM/yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo).ToString("yyyy/MM/dd")), New With {.class = "form-control"})
                            @*<input id="FECHA_RESP_CORRECTIVAS" class="form-control" />*@
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>
                @Html.ValidationMessageFor(Function(model) model.FECHA_RESP_CORRECTIVAS, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group col-md-6">
            @Html.LabelFor(Function(model) model.FECHA_CIERRECLIENTE, New With {.class = "control-label col-sm-6"})
            <div class="col-sm-6">
                <div class="col-md-12">
                    <div class="form-group">
                        <div class='input-group myDatePicker' id='datetimepicker3'>
                            @Html.TextBox("FECHA_CIERRECLIENTE", If(reclamacion Is Nothing OrElse reclamacion.FECHA_CIERRECLIENTE Is Nothing, "", Date.ParseExact(reclamacion.FECHA_CIERRECLIENTE, "dd/MM/yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo).ToString("yyyy/MM/dd")), New With {.class = "form-control"})

                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>
                @Html.ValidationMessageFor(Function(model) model.FECHA_CIERRECLIENTE, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group col-md-12">
            <div>
                <h3 style="text-align:center">COSTES:</h3>
            </div>
        </div>
        <div class="form-group col-md-6">
            @Html.LabelFor(Function(model) model.COSTE_REVISIONCLIENTE, New With {.class = "control-label col-sm-6"})
            <div class="col-sm-6">
                <div class="col-md-12">
                    <div class="form-group">
                        <div class='input-group euroTextbox'>
                            @Html.EditorFor(Function(model) model.COSTE_REVISIONCLIENTE, New With {.htmlAttributes = New With {.class = "form-control SeparadorDecimal"}})
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-euro"></span>
                            </span>
                        </div>
                    </div>
                </div>
                @Html.ValidationMessageFor(Function(model) model.COSTE_REVISIONCLIENTE, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group col-md-6">
            @Html.LabelFor(Function(model) model.COSTE_REVISIONINTERNA, New With {.class = "control-label col-sm-6"})
            <div class="col-sm-6">
                <div class="col-md-12">
                    <div class="form-group">
                        <div class='input-group euroTextbox'>
                            @Html.EditorFor(Function(model) model.COSTE_CARGOSCLIENTE, New With {.htmlAttributes = New With {.class = "form-control"}})
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-euro"></span>
                            </span>
                        </div>
                    </div>
                </div>
                @Html.ValidationMessageFor(Function(model) model.COSTE_CARGOSCLIENTE, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div Class="form-group col-md-6">
            @Html.LabelFor(Function(model) model.COSTE_REVISIONINTERNA, htmlAttributes:=New With {.class = "control-label col-sm-6"})
            <div Class="col-sm-6">
                <div class="col-md-12">
                    <div class="form-group">
                        <div class='input-group euroTextbox'>
                            @Html.EditorFor(Function(model) model.COSTE_REVISIONINTERNA, New With {.htmlAttributes = New With {.class = "form-control"}})
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-euro"></span>
                            </span>
                        </div>
                    </div>
                </div>
                @Html.ValidationMessageFor(Function(model) model.COSTE_REVISIONINTERNA, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div Class="form-group col-md-6">
            @Html.LabelFor(Function(model) model.COSTE_MATERIALESCHATARRA, New With {.class = "control-label col-sm-6"})
            <div Class="col-sm-6">
                <div class="col-md-12">
                    <div class="form-group">
                        <div class='input-group euroTextbox'>
                            @Html.EditorFor(Function(model) model.COSTE_MATERIALESCHATARRA, New With {.htmlAttributes = New With {.class = "form-control"}})
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-euro"></span>
                            </span>
                        </div>
                    </div>
                </div>
                @Html.ValidationMessageFor(Function(model) model.COSTE_MATERIALESCHATARRA, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div Class="form-group col-md-6">
            @Html.LabelFor(Function(model) model.COSTE_OTROS, htmlAttributes:=New With {.class = "control-label col-sm-6"})
            <div Class="col-sm-6">
                <div class="col-md-12">
                    <div class="form-group">
                        <div class='input-group euroTextbox'>
                            @Html.EditorFor(Function(model) model.COSTE_OTROS, New With {.htmlAttributes = New With {.class = "form-control"}})
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-euro"></span>
                            </span>
                        </div>
                    </div>
                </div>
                @Html.ValidationMessageFor(Function(model) model.COSTE_OTROS, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div Class="form-group col-md-6">
            <span class="glyphicon glyphicon-resize-horizontal" style="line-height: 1.4285; position: absolute; transform: translate(-15px, 8px);"></span>
            @Html.LabelFor(Function(model) model.COSTE_OTROS_DESCRIPCION, htmlAttributes:=New With {.class = "control-label col-sm-6"})
            <div Class="col-sm-6">
                @Html.EditorFor(Function(model) model.COSTE_OTROS_DESCRIPCION, New With {.htmlAttributes = New With {.class = "form-control"}})
                @Html.ValidationMessageFor(Function(model) model.COSTE_OTROS_DESCRIPCION, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group col-md-12">
            <div>
                <h3 style="text-align:center">FICHEROS:</h3>
            </div>
        </div>

        <!-- Campo para subir archivos -->
        <div class="form-group col-md-6">
            @Html.Label("Subir Archivos", New With {.class = "control-label col-sm-6 text-uppercase"})
            <div class="col-sm-6">
                <input type="file" id="uploadedFiles" name="uploadedFiles" multiple />
            </div>
        </div>

        <!-- Botón para enviar el formulario -->
        <div class="col-md-12" style="text-align:center">
            <input type="submit" value="Guardar" Class="btn btn-info" style="font-size:18px;" />
        </div>
    </div>
End Using

<!--  Sección: Mostrar Archivos Subidos -->
<div class="form-group col-md-12">
    <h4 style="margin-left: 100px; font-weight: bold;">Archivos Subidos:</h4> <!-- Margen y negrita aplicados al título -->
    @Html.Raw(archivosHtml)
    @Html.Raw(mensajeArchivos)
</div>



@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")

    <script type="text/javascript">
        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        });

        $('.myDatePicker').datetimepicker({
            format: 'YYYY/MM/DD',
            maxDate: moment()
        });

    </script>
End Section