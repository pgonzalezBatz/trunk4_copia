@Imports DOBLib

@code
    Dim seccion As Web.Configuration.HttpRuntimeSection = CType(ConfigurationManager.GetSection("system.web/httpRuntime"), Web.Configuration.HttpRuntimeSection)
    Dim revision As ELL.Revision = CType(ViewData("Revision"), ELL.Revision)
    Dim listaDocumentos As List(Of ELL.Documento) = CType(ViewData("Documentos"), List(Of ELL.Documento))
End Code

<script type="text/javascript">
    $(function () {
        if ("@revision.Revision" == "@CInt(ELL.Revision.Tipo.Enero)") {
            $("#grupoPAAñoSiguiente").show();
            $("#grupoPAComentario").show();
        }
        else {
            $("#grupoPAAñoSiguiente").hide();
            $("#grupoPAComentario").hide();
        }
    });

</script>

<h3>@String.Format("{0} - {1}", Utils.Traducir("Revisiones"), Utils.Traducir("Editar"))</h3>
<hr />

@Using Html.BeginForm("Editar", "Revisiones", New With {.idObjetivo = revision.IdObjetivo, .revisionTipo = revision.Revision}, FormMethod.Post, New With {.class = "form-horizontal"})
    @<div Class="form-group">
        <label class="col-sm-3 control-label">@Utils.Traducir("Objetivo")</label>
        <div class="col-sm-7">
            @*@Html.Hidden("hfObjetivos", revision.IdObjetivo)*@
            @Html.DropDownList("Objetivos", Nothing, New With {.required = "required", .class = "form-control", .disabled = "disabled"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-3 control-label">@Utils.Traducir("Revisión")</label>
        <div class="col-sm-2">
            @*@Html.Hidden("hfRevisionesTipo", revision.Revision)*@
            @Html.DropDownList("RevisionesTipo", Nothing, New With {.required = "required", .class = "form-control", .disabled = "disabled"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-3 control-label">@Utils.Traducir("Comentario")</label>
        <div class="col-sm-7">
            @Html.TextArea("comentario", revision.Comentario, New With {.maxlength = "500", .class = "form-control"})
        </div>
    </div>
    @<div id="grupoPAAñoSiguiente" Class="form-group" style="display:none;">
        <label class="col-sm-3 control-label">@Utils.Traducir("¿Plan acción año siguiente?")</label>
        <div class="col-sm-7">
            @Html.CheckBox("paAñoSiguiente", revision.PAAñoSiguiente)
        </div>
    </div>
    @<div id="grupoPAComentario" Class="form-group" style="display:none;">
        <label class="col-sm-3 control-label">@Utils.Traducir("Comentario plan acción")</label>
        <div class="col-sm-7">
            @Html.TextArea("paComentario", revision.PAComentario, New With {.maxlength = "500", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <div class="col-sm-offset-3 col-sm-7">
            <input type="submit" id="submit" value="@Utils.Traducir("Guardar")" class="btn btn-primary input-block-level form-control" />
        </div>
    </div>
End Using

<h4>@Utils.Traducir("Documentos")</h4>
@If (listaDocumentos.Count = 0) Then
    @Html.Label(String.Empty, Utils.Traducir("noExisteNingunRegistro"))
Else
    @<div class="row">
        <div class="col-sm-10">
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

@Using Html.BeginForm("Agregar", "Documentos", New With {.idPadre = revision.IdObjetivo, .idTipoDocumento = CInt(ELL.TipoDocumento.Tipo.Revision_cierre), .revision = revision.Revision}, FormMethod.Post, New With {.enctype = "multipart/form-data", .id = "AgregarDocumentos", .class = "form-horizontal"})
    @<h4>@Utils.Traducir("Agregar documentos")</h4>
    @<div Class="form-group">
        <label class="col-sm-3 control-label">@Utils.Traducir("Seleccione un documento")</label>
        <div class="col-sm-7">
            @Html.TextBox("fuDocumento", Nothing, New With {.type = "file", .style = "width:100%;", .required = "required", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <div class="col-sm-offset-3 col-sm-7">
            <label class="label label-info">@String.Format("{0}: {1} MB", Utils.Traducir("Tamaño máximo del fichero"), seccion.MaxRequestLength / 1024)</label>
        </div>
    </div>
    @<div Class="form-group">
        <div class="col-sm-offset-3 col-sm-7">
            <input type="submit" id="submit" value="@Utils.Traducir("Agregar")" class="btn btn-primary input-block-level form-control" />
        </div>
    </div>End Using