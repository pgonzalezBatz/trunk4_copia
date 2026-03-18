@ModelType IEnumerable(Of BezerreSis.OBJETIVOS)

@Imports GridMvc.Html

@Code
    ViewData("Title") = "Objetivos"
End Code

<br />
<h2>Objetivos</h2>
<br />

@Using (Html.BeginForm("Index", "OBJETIVOS", FormMethod.Get))
    @<div Class="row">
        <div Class="col-md-12">
            <h4 class="col-md-6">
                @Html.Label("Cliente", htmlAttributes:=New With {.class = "control-label"})
            </h4>
            <p class="col-md-6">
                @Html.DropDownList("lClientes_obj", Nothing, "Seleccionar uno", htmlAttributes:=New With {.Class = "form-control", .onchange = "$(this).closest('form').trigger('submit');"})
            </p>
        </div>
    </div>
End Using

<div class="container">
    <div class="panel-group" id="accordion">

        <div class="panel panel-default">
            <div class="panel-heading" style="background-color:#337ab7;color:white">
                <h4 class="panel-title">
                    <a data-toggle="collapse" data-parent="#accordion" href="#ObjGenerales">Generales</a>
                </h4>
            </div>
            <div id="ObjGenerales" class="panel-collapse collapse in">
                <div class="panel-body">
                    @If Not String.IsNullOrWhiteSpace(ViewBag.lClientes_Selected_obj) Then
                        @<button type = "button" Class="btn btn-info" onclick="window.location.href ='@Url.Action("Create", New With {.lClientes = ViewBag.lClientes_Selected_obj})'" style="font-size:18px">
                            <i Class="glyphicon glyphicon-plus" style="margin-right: 10px;"></i>Nuevo objetivo
                        </button>
                    End If

                    @Using (Html.BeginForm("Index", "Html.Grid", FormMethod.Get))
                        @Html.Grid(Model).Columns(
                         Function(columns) columns.Add(
                         Function(o As BezerreSis.OBJETIVOS) o.FECHA
                             ).Titled("Año").Format("{0:yyyy}").Sortable(True).SortInitialDirection(GridMvc.Sorting.GridSortDirection.Descending)
                         ).Columns(
                         Function(columns) columns.Add(
                         Function(o As BezerreSis.OBJETIVOS) o.PPM_MENSUAL
                             ).Titled("PPM MENSUAL")
                         ).Columns(
                         Function(columns) columns.Add(
                         Function(o As BezerreSis.OBJETIVOS) o.PPM_ANUAL
                             ).Titled("PPM ANUAL")
                         ).Columns(
                         Function(columns) columns.Add(
                         Function(o As BezerreSis.OBJETIVOS) o.IPB_SEMESTRAL
                             ).Titled("IPB SEMESTRAL")
                         ).Columns(
                         Function(columns) columns.Add().RenderValueAs(
                         Function(o) "<div class=""btn-group""><a class=""btn btn-default"" href = """ & Url.Action("Edit", New With {.id = o.ID}) & """ title=""Editar"" data-toggle=""tooltip""><span class=""glyphicon glyphicon-pencil"" style=""color:royalblue"" aria-hidden=""true""></span></a><a class=""btn btn-default"" href = """ & Url.Action("DeleteObjetivo", New With {.id = o.ID, .lClientes = ViewBag.lClientes_Selected_obj}) & """ onclick = ""return confirm('¿Estás seguro de que quieres eliminar este objetivo?');"" title=""Borrar"" data-toggle=""tooltip""><span class=""glyphicon glyphicon-remove"" style=""color:red"" aria-hidden=""true""></span></a></div>"
                             ).Encoded(False).Sanitized(False)
                         ).Sortable()
                    End Using
                </div>
            </div>
        </div>
    </div>
</div>
