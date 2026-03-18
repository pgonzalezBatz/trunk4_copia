@ModelType BezerreSis.OBJETIVOS
@Code
    ViewData("Title") = "Edit"
End Code

<br />
<h2>Editar objetivo</h2>
<br />
@Using (Html.BeginForm())
    @Html.AntiForgeryToken()
    
    @<div class="form-horizontal">
    @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
    @Html.HiddenFor(Function(model) model.ID)

    <div class="form-group">
        @Html.Label("Año", htmlAttributes:=New With {.class = "control-label col-md-6"})
        <div class="col-md-6">
            @Html.EditorFor(Function(model) model.Año, Nothing, New With {.htmlAttributes = New With {.class = "form-control", .readonly = "readonly"}})
            @Html.ValidationMessageFor(Function(model) model.Año, Nothing, New With {.class = "text-danger"})
        </div>
    </div>

    <div class="form-group">
        @Html.LabelFor(Function(model) model.IDCLIENTE, "IDCLIENTE", htmlAttributes:=New With {.class = "control-label col-md-6"})
        <div class="col-md-6">
            @Html.DropDownList("IDCLIENTE", Nothing, htmlAttributes:=New With {.class = "form-control", .readonly = "readonly"})
            @Html.ValidationMessageFor(Function(model) model.IDCLIENTE, "", New With {.class = "text-danger"})
        </div>
    </div>

    <div class="form-group">
        @Html.LabelFor(Function(model) model.PPM_MENSUAL, htmlAttributes:=New With {.class = "control-label col-md-6"})
        <div class="col-md-6">
            @Html.EditorFor(Function(model) model.PPM_MENSUAL, New With {.htmlAttributes = New With {.class = "form-control"}})
            @Html.ValidationMessageFor(Function(model) model.PPM_MENSUAL, "", New With {.class = "text-danger"})
        </div>
    </div>

    <div class="form-group">
        @Html.LabelFor(Function(model) model.PPM_ANUAL, htmlAttributes:=New With {.class = "control-label col-md-6"})
        <div class="col-md-6">
            @Html.EditorFor(Function(model) model.PPM_ANUAL, New With {.htmlAttributes = New With {.class = "form-control"}})
            @Html.ValidationMessageFor(Function(model) model.PPM_ANUAL, "", New With {.class = "text-danger"})
        </div>
    </div>

    <div class="form-group">
        @Html.LabelFor(Function(model) model.IPB_SEMESTRAL, htmlAttributes:=New With {.class = "control-label col-md-6"})
        <div class="col-md-6">
            @Html.EditorFor(Function(model) model.IPB_SEMESTRAL, New With {.htmlAttributes = New With {.class = "form-control"}})
            @Html.ValidationMessageFor(Function(model) model.IPB_SEMESTRAL, "", New With {.class = "text-danger"})
        </div>
    </div>
</div>

    @<div style="text-align:center">
        <input type="submit" value="Guardar" Class="btn btn-info" style="font-size:18px;" />
    </div>
End Using
@Section Scripts 
    @Scripts.Render("~/bundles/jqueryval")

        <script type="text/javascript">
            
                    $('#IDCLIENTE > option:not(:selected)').attr('disabled', true);
        </script>
End Section
