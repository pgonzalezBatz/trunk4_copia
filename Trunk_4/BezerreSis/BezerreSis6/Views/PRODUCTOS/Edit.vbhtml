@ModelType BezerreSis.PRODUCTOS
@Code
    ViewData("Title") = "Edit"
End Code

<br />
<h2>Editar producto</h2>
<br />
@Using (Html.BeginForm())
    @Html.AntiForgeryToken()
    @<div class="form-horizontal">
    @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
    @Html.HiddenFor(Function(model) model.ID)
    @Html.HiddenFor(Function(model) model.IDPLANTA)
    <div class="form-group">
        @Html.LabelFor(Function(model) model.NOMBRE, htmlAttributes:=New With {.class = "control-label col-md-6"})
        <div class="col-md-6">
            @Html.EditorFor(Function(model) model.NOMBRE, New With {.htmlAttributes = New With {.class = "form-control"}})
            @Html.ValidationMessageFor(Function(model) model.NOMBRE, "", New With {.class = "text-danger"})
        </div>
    </div>
    <div style="text-align:center">
        <input type="submit" value="Guardar" Class="btn btn-info" style="font-size:18px;" />
    </div>
</div>
End Using
@Section Scripts 
    @Scripts.Render("~/bundles/jqueryval")
End Section
