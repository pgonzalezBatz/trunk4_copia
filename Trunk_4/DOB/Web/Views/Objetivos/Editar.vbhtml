@Imports DOBLib

@code
    Dim seccion As Web.Configuration.HttpRuntimeSection = CType(ConfigurationManager.GetSection("system.web/httpRuntime"), Web.Configuration.HttpRuntimeSection)
    Dim ticket As SabLib.ELL.Ticket = CType(Session("Ticket"), SabLib.ELL.Ticket)
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

        $(".boton-eliminar").click(function () {
            return confirm("@Html.Raw(Utils.Traducir("¿Desea eliminar el elemento seleccionado?"))");
        });

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

        $('[data-toggle="popover"]').popover({
            title: '@Html.Raw(Utils.Traducir("Información"))',
            content: '@Html.Raw(Utils.Traducir("Ascendente: cuanto más valor mejor. Si nos acercamos o quedamos por encima del valor objetivo es bueno. Descendente: cuanto menos mejor, Si nos acercamos o quedamos por debajo del valor objetivo es bueno."))',
            trigger: 'focus',
            placement: 'bottom'
        });
    });

</script>

<h3>@String.Format("{0} - {1}", Utils.Traducir("Objetivos"), Utils.Traducir("Editar"))</h3>
<hr />

@code
    Dim objetivo As ELL.Objetivo = CType(ViewData("Objetivo"), ELL.Objetivo)
    Dim listaDocumentos As List(Of ELL.Documento) = CType(ViewData("Documentos"), List(Of ELL.Documento))
End Code

@Using Html.BeginForm("Editar", "Objetivos", New With {.idObjetivo = objetivo.Id}, FormMethod.Post, New With {.class = "form-horizontal"})
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Descripción")</label>
        <div class="col-sm-6">
            @Html.TextArea("descripcion", objetivo.Descripcion, New With {.maxlength = "200", .rows = "5", .required = "required", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Reto")</label>
        <div class="col-sm-4">
            @Html.DropDownList("Retos", Nothing, New With {.required = "required", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Proceso")</label>
        <div class="col-sm-4">
            @Html.DropDownList("Procesos", Nothing, New With {.required = "required", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Responsable")</label>
        <div class="col-sm-4">
            @Html.DropDownList("Responsables", Nothing, New With {.required = "required", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
         <label class="col-sm-2 control-label">@Utils.Traducir("Fecha objetivo")</label>
         <div class="col-sm-4">
             <div class="input-group date" id="fecha">
                 @Html.TextBox("fechaObjetivo", objetivo.FechaObjetivo.ToShortDateString(), New With {.class = "form-control"})
                 <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
             </div>
         </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Indicador")</label>
        <div class="col-sm-4">
            @Html.TextBox("indicador", objetivo.NombreIndicador, New With {.maxlength = "100", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Sentido")</label>
        <div class="col-sm-4">
            @Html.RadioButton("sentido", 1, objetivo.Sentido)&nbsp;<label>@Utils.Traducir("Ascendente")</label>
            @Html.RadioButton("sentido", 0, Not objetivo.Sentido)&nbsp;<label>@Utils.Traducir("Descendente")</label>
            <a href="#" data-toggle="popover">
                <span class="glyphicon glyphicon glyphicon-info-sign" aria-hidden="true"></span>
            </a>
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Descripción indicador")</label>
        <div class="col-sm-4">
            @Html.TextArea("descripcionIndicador", objetivo.DescripcionIndicador, New With {.maxlength = "200", .rows = "2", .class = "form-control"})
        </div>
    </div>
        @*@<div Class="form-group">
            <label class="col-sm-2 control-label">@Utils.Traducir("Ascendente")</label>
            <div class="col-sm-4">
                @Html.CheckBox("ascendente", objetivo.Ascendente)
            </div>
        </div>*@
        @<div Class="form-group">
            <label class="col-sm-2 control-label">@Utils.Traducir("Tipo indicador")</label>
            <div class="col-sm-4">
                @Html.DropDownList("TiposIndicadores", Nothing, New With {.required = "required", .class = "form-control"})
            </div>
        </div>
        @<div Class="form-group">
            <label class="col-sm-2 control-label">@Utils.Traducir("Valor inicial")</label>
            <div class="col-sm-4">
                @Html.TextBox("valorInicial", objetivo.ValorInicial, New With {.required = "required", .type = "number", .step = "any", .class = "form-control text-right"})
            </div>
        </div>
        @<div Class="form-group">
            <label class="col-sm-2 control-label">@Utils.Traducir("Valor objetivo")</label>
            <div class="col-sm-4">
                @Html.TextBox("valorObjetivo", objetivo.ValorObjetivo, New With {.required = "required", .type = "number", .step = "any", .class = "form-control text-right"})
            </div>
        </div>
        @<div Class="form-group hide">
            <label class="col-sm-2 control-label">@Utils.Traducir("Periodicidad")</label>
            <div class="col-sm-4">
                @Html.DropDownList("Periodicidad", Nothing, New With {.required = "required", .class = "form-control"})
            </div>
        </div>
        @<div Class="form-group">
            <div class="col-sm-offset-2 col-sm-4">
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
            </div></div>
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
            @*<br />
            @Html.ActionLink(Utils.Traducir("Volver al listado"), "Index")*@
