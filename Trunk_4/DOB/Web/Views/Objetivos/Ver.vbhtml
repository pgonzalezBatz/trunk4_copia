@Imports DOBLib

<h3>@String.Format("{0} - {1}", Utils.Traducir("Objetivos"), Utils.Traducir("Ver"))</h3>
<hr />

@code
    Dim objetivo As ELL.Objetivo = CType(ViewData("Objetivo"), ELL.Objetivo)
    Dim listaDocumentos As List(Of ELL.Documento) = CType(ViewData("Documentos"), List(Of ELL.Documento))
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

<div class="form-horizontal">    
    <div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Descripción")</label>
        <div class="col-sm-6">
            @Html.TextArea("descripcion", objetivo.Descripcion, New With {.class = "form-control", .disabled = "disabled"})
        </div>
    </div>
    <div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Reto")</label>
        <div class="col-sm-6">
            @Html.TextBox("reto", objetivo.TituloReto, New With {.class = "form-control", .disabled = "disabled"})
        </div>
    </div>
    <div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Proceso")</label>
        <div class="col-sm-6">
            @Html.TextBox("proceso", objetivo.CodigoProceso, New With {.class = "form-control", .disabled = "disabled"})
        </div>
    </div>
    <div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Responsable")</label>
        <div class="col-sm-6">
            @Html.TextBox("indicador", objetivo.Responsable, New With {.class = "form-control", .disabled = "disabled"})
        </div>
    </div>
    <div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Fecha objetivo")</label>
        <div class="col-sm-6">
            <div class="input-group date" id="fecha">
                @Html.TextBox("fechaObjetivo", objetivo.FechaObjetivo.ToShortDateString(), New With {.class = "form-control", .disabled = "disabled"})
                <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
            </div>
        </div>
    </div>
    <div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Indicador")</label>
        <div class="col-sm-6">
            @Html.TextBox("indicador", objetivo.NombreIndicador, New With {.class = "form-control", .disabled = "disabled"})
        </div>
    </div>
    <div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Descripción indicador")</label>
        <div class="col-sm-6">
            @Html.TextArea("descripcionIndicador", objetivo.DescripcionIndicador, New With {.rows = "2", .class = "form-control", .disabled = "disabled"})
        </div>
    </div>
    <div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Tipo indicador")</label>
        <div class="col-sm-6">
            @Html.TextBox("tipoIndicador", objetivo.TipoIndicador, New With {.class = "form-control", .disabled = "disabled"})
        </div>
    </div>
    <div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Valor inicial")</label>
        <div class="col-sm-6">
            @Html.TextBox("valorInicial", objetivo.ValorInicial, New With {.class = "form-control", .disabled = "disabled"})
        </div>
    </div>
    <div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Valor objetivo")</label>
        <div class="col-sm-6">
            @Html.TextBox("valorObjetivo", objetivo.ValorObjetivo, New With {.class = "form-control", .disabled = "disabled"})
        </div>
    </div>    
    <div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Valor actual")</label>
        <div class="col-sm-6">
            @Html.TextBox("valorActual", objetivo.ValorActual, New With {.class = "form-control", .disabled = "disabled"})
        </div>
    </div>
    <div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Sentido")</label>
        <div class="col-sm-4">
            @Html.RadioButton("sentido", 1, objetivo.Sentido, New With {.disabled = "disabled"})&nbsp;<label>@Utils.Traducir("Ascendente")</label>
            @Html.RadioButton("sentido", 0, Not objetivo.Sentido, New With {.disabled = "disabled"})&nbsp;<label>@Utils.Traducir("Descendente")</label>
        </div>
    </div>
</div>

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

@Using Html.BeginForm("Agregar", "Documentos", New With {.idPadre = objetivo.Id, .idTipoDocumento = CInt(ELL.TipoDocumento.Tipo.Objetivo)}, FormMethod.Post, New With {.enctype = "multipart/form-data", .id = "AgregarDocumentos", .class = "form-horizontal"})
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
            </div>  End Using


