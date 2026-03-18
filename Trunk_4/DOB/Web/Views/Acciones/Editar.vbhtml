@Imports DOBLib

@code
    Dim seccion As Web.Configuration.HttpRuntimeSection = CType(ConfigurationManager.GetSection("system.web/httpRuntime"), Web.Configuration.HttpRuntimeSection)
End Code

<script type="text/javascript">
    $(function () {
        $("#AgregarDocumentos").submit(function () {
            var mensajeError = "";
            if($('#fuDocumento').val() != ''){
                var fileSize = -1;
                try {
                    fileSize = $('#fuDocumento')[0].files[0].size; // Para navegadores que soporten HTML5
                } catch (ex) {
                    var strFileName = $('#fuDocumento').val();
                    var objFSO
                    try{
                        objFSO = new ActiveXObject("Scripting.FileSystemObject");
                    } catch (ex) {
                    }

                    if(objFSO)
                    {
                        var e = objFSO.getFile(strFileName);
                        fileSize = e.size;
                    }
                    else
                    {
                        return false;
                    }
                }

                mensajeError = "@Html.Raw(String.Format("{0}: {1}MB", Utils.Traducir("Tamaño máximo del fichero"), seccion.MaxRequestLength / 1024))";
                var maxRequestLength = @seccion.MaxRequestLength;

                if (fileSize / 1024 > maxRequestLength) {
                    alert(mensajeError);
                    return false;
                }

                return true;
            }
        });

        $(function () {
            $(".boton-eliminar").click(function () {
                return confirm("@Html.Raw(Utils.Traducir("¿Desea eliminar el elemento seleccionado?"))");
            });
        });
    });

</script>

<h3>@String.Format("{0} - {1}", Utils.Traducir("Acciones"), Utils.Traducir("Editar"))</h3>
<hr />

@code
    Dim accion As ELL.Accion = CType(ViewData("Accion"), ELL.Accion)
    Dim listaDocumentos As List(Of ELL.Documento) = CType(ViewData("Documentos"), List(Of ELL.Documento))
    Dim ticket As SabLib.ELL.Ticket = CType(Session("Ticket"), SabLib.ELL.Ticket)
End Code

<script type="text/javascript">
    $(function () {
        $("#fecha").datetimepicker({
            showClear:true,
            locale: '@ticket.Culture',
            format: '@Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper'
        });

        $("form").submit(function () {
            if ($("#fechaObjetivo").val() == "") {
                alert("@Html.Raw(Utils.Traducir("Plazo obligatorio"))");
                return false;
            }
        });
    });
</script>

@Using Html.BeginForm("Editar", "Acciones", New With {.idAccion = accion.Id, .idObjetivo = accion.IdObjetivo}, FormMethod.Post, New With {.class = "form-horizontal"})
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Objetivo")</label>
        <label class="col-sm-3 control-label" style="text-align:left;">@accion.DescripcionObjetivo</label>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Descripción")</label>
        <div class="col-sm-3">
            @Html.TextArea("descripcion", accion.Descripcion, New With {.maxlength = "200", .rows = "2", .required = "required", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Plazo")</label>
        <div class="col-sm-3">
            <div class="input-group date" id="fecha">
                @Html.TextBox("fechaObjetivo", accion.FechaObjetivo.ToShortDateString(), New With {.class = "form-control"})
                <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
            </div>
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Grado importancia")</label>
        <div class="col-sm-3">
            @Html.TextBox("gradoImportancia", accion.GradoImportancia, New With {.required = "required", .step = "any", .type = "number", .min = "0", .max = "100", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <div class="col-sm-offset-2 col-sm-3">
            <input type="submit" id="submit" value="@Utils.Traducir("Guardar")" class="btn btn-primary input-block-level form-control" />
        </div>
    </div>
End Using

<h4>@Utils.Traducir("Documentos")</h4>
@If (listaDocumentos.Count = 0) Then
    @Html.Label(String.Empty, Utils.Traducir("noExisteNingunRegistro"))
Else
    @<div class="row">
        <div class="col-sm-6">
            <table class="table table-striped table-hover table-condensed">
                <thead>
                    <tr>
                        <th>@Utils.Traducir("Documento")</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    @code
                        For Each documento In listaDocumentos
                            @<tr>
                                <td>
                                    <a href='@Url.Action("Mostrar", "Documentos", New With {.idDocumento = documento.Id})'>
                                        @documento.NombreFichero
                                    </a>
                                </td>
                                <td class="text-center">
                                    <a href='@Url.Action("Eliminar", "Documentos", New With {.idDocumento = documento.Id})'>
                                        <span class="glyphicon glyphicon-remove boton-eliminar" aria-hidden="true" title="@Utils.Traducir("Eliminar")"></span>
                                    </a>
                                </td>
                            </tr>
                        Next
                    End Code

                </tbody>
            </table>
        </div>
    </div>

                        End If

@Using Html.BeginForm("Agregar", "Documentos", New With {.idPadre = accion.Id, .idTipoDocumento = CInt(ELL.TipoDocumento.Tipo.Accion)}, FormMethod.Post, New With {.enctype = "multipart/form-data", .id = "AgregarDocumentos", .class = "form-horizontal"})
    @<h4>@Utils.Traducir("Agregar documentos")</h4>
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Seleccione un documento")</label>
        <div class="col-sm-4">
            @Html.TextBox("fuDocumento", Nothing, New With {.type = "file", .style = "width:100%;", .required = "required", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <div class="col-sm-offset-2 col-sm-4">
            <label class="label label-info">@String.Format("{0}: {1} MB", Utils.Traducir("Tamaño máximo del fichero"), seccion.MaxRequestLength / 1024)</label>
        </div>
    </div>
    @<div Class="form-group">
        <div class="col-sm-offset-2 col-sm-4">
            <input type="submit" id="submit" value="@Utils.Traducir("Agregar")" class="btn btn-primary input-block-level form-control" />
        </div>
    </div>
End Using
@*<br />
    @Html.ActionLink(Utils.Traducir("Volver al listado"), "MisAcciones")*@
