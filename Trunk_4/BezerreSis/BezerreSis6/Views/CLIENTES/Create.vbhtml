@ModelType BezerreSis.CLIENTES
@Code
    ViewData("Title") = "Create"
End Code

<br />
<h2>Crear cliente</h2>
<br />
@Using (Html.BeginForm()) 
    @Html.AntiForgeryToken()

    @<div class="form-horizontal">
    @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
    <div class="form-group col-md-12">
        @Html.LabelFor(Function(model) model.NOMBRE, htmlAttributes:=New With {.class = "control-label col-sm-6"})
        <div class="col-sm-6">
            @Html.EditorFor(Function(model) model.NOMBRE, New With {.htmlAttributes = New With {.class = "form-control"}})
            @Html.ValidationMessageFor(Function(model) model.NOMBRE, "", New With {.class = "text-danger"})
        </div>
    </div>
    <div class="form-group col-md-12" style="text-align:center">
        <span style="display:block;margin-bottom:10px;"><i>Este cliente debe crearse con el mismo nombre con el que existe en Xpert (término SUBGRUPO CLIENTE)</i></span>
        <input type="submit" value="Crear" Class="btn btn-info" style="font-size:18px;" />
    </div>
</div>

End Using
@Section Scripts 
    @Scripts.Render("~/bundles/jqueryval")
End Section
