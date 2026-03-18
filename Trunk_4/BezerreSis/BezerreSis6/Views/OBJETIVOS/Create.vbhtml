@ModelType BezerreSis.OBJETIVOS
@Code
    ViewData("Title") = "OBJETIVOS"
End Code

<br />
<h2>Crear objetivo para @ViewBag.Cliente</h2>
<br />
@Using (Html.BeginForm())
    @Html.AntiForgeryToken()

    @Html.HiddenFor(Function(model) model.IDCLIENTE)

    @<div class="form-horizontal">
    <div class="form-group">
        @Html.Label("Año", htmlAttributes:=New With {.class = "control-label col-md-2 col-md-offset-4"})
        <div class="col-md-6">
            @Html.EditorFor(Function(model) model.Año, Nothing, New With {.htmlAttributes = New With {.class = "form-control"}})
            @Html.ValidationMessageFor(Function(model) model.Año, Nothing, New With {.class = "text-danger"})
        </div>
    </div>
    <div class="form-group">
        @Html.LabelFor(Function(model) model.PPM_MENSUAL, htmlAttributes:=New With {.class = "control-label col-md-2 col-md-offset-4"})
        <div class="col-md-6">
            @Html.EditorFor(Function(model) model.PPM_MENSUAL, New With {.htmlAttributes = New With {.class = "form-control"}})
            @Html.ValidationMessageFor(Function(model) model.PPM_MENSUAL, "", New With {.class = "text-danger"})
        </div>
    </div>
    <div class="form-group">
        @Html.LabelFor(Function(model) model.PPM_ANUAL, htmlAttributes:=New With {.class = "control-label col-md-2 col-md-offset-4"})
        <div class="col-md-6">
            @Html.EditorFor(Function(model) model.PPM_ANUAL, New With {.htmlAttributes = New With {.class = "form-control"}})
            @Html.ValidationMessageFor(Function(model) model.PPM_ANUAL, Nothing, New With {.class = "text-danger"})
        </div>
    </div>
    <div class="form-group">
        @Html.LabelFor(Function(model) model.IPB_SEMESTRAL, htmlAttributes:=New With {.class = "control-label col-md-2 col-md-offset-4"})
        <div class="col-md-6">
            @Html.EditorFor(Function(model) model.IPB_SEMESTRAL, New With {.htmlAttributes = New With {.class = "form-control"}})
            @Html.ValidationMessageFor(Function(model) model.IPB_SEMESTRAL, "", New With {.class = "text-danger"})
        </div>
    </div>
    <div class="form-group">
        <div style="text-align:center">
            <input type="submit" value="Create" class="btn btn-default" />
        </div>
    </div>
</div>
End Using

@Section Scripts 
    @Scripts.Render("~/bundles/jqueryval")
End Section
