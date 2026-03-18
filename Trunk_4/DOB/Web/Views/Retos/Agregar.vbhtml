@Imports DOBLib

@code
    Dim seccion As Web.Configuration.HttpRuntimeSection = CType(ConfigurationManager.GetSection("system.web/httpRuntime"), Web.Configuration.HttpRuntimeSection)
    Dim planta As ELL.Planta = CType(ViewData("Planta"), ELL.Planta)
End Code

<script type="text/javascript">
    $(function () {
        $("form").submit(function () {            
            @*if ($("#descripcion").val() == "") {
                alert("@Html.Raw(Utils.Traducir("Descripción campo obligatorio"))");
                return false;
            }*@

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
    });

</script>

<h3>@String.Format("{0} - {1}", Utils.Traducir("Retos"), Utils.Traducir("Nuevo"))</h3>
<hr />

@Using Html.BeginForm("Agregar", "Retos", FormMethod.Post, New With {.enctype = "multipart/form-data", .class = "form-horizontal"})
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Planta")</label>
        <label class="col-sm-3 control-label" style="text-align:left;">@planta.Planta</label>
    </div>
        @<div Class="form-group">
            <label class="col-sm-2 control-label">@Utils.Traducir("Código")</label>
            <div class="col-sm-3">
                @Html.TextBox("codigo", String.Empty, New With {.maxlength = "3", .style = "text-transform:uppercase;", .required = "required", .class = "form-control"})
            </div>
        </div>
        @<div Class="form-group">
            <label class="col-sm-2 control-label">@Utils.Traducir("Título")</label>
            <div class="col-sm-3">
                @Html.TextArea("titulo", String.Empty, New With {.maxlength = "200", .rows = "2", .required = "required", .class = "form-control"})
            </div>
        </div>
        @<div Class="form-group">
            <label class="col-sm-2 control-label">@Utils.Traducir("Descripcion")</label>
            <div class="col-sm-5">
                @Html.TextArea("descripcion", New With {.maxlength = "2000", .required = "required", .class = "form-control ckeditor"})
            </div>
        </div>
        @<div Class="form-group">
            <label class="col-sm-2 control-label">@Utils.Traducir("Documento")</label>
            <div class="col-sm-5">
                @Html.TextBox("fuDocumento", Nothing, New With {.type = "file", .class = "form-control"})
            </div>
        </div>
        @<div Class="form-group">
            <div class="col-sm-offset-2 col-sm-5">
                <label class="label label-info">@String.Format("{0}: {1} MB", Utils.Traducir("Tamaño máximo del fichero"), seccion.MaxRequestLength / 1024)</label>
            </div>
        </div>
        @<div Class="form-group">
            <div class="col-sm-offset-2 col-sm-5">
                <input type="submit" id="submit" value="@Utils.Traducir("Añadir")" class="btn btn-primary input-block-level form-control" />
            </div>
        </div>
End Using
@*<br />
@Html.ActionLink(Utils.Traducir("Volver al listado"), "Index")*@