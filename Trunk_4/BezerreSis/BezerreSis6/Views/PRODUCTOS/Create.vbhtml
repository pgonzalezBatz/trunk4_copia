@ModelType BezerreSis.PRODUCTOS
@Code
    ViewData("Title") = "Create"
End Code

<br />
<h2>Crear producto</h2>
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
            <span style="display:block;margin-bottom:10px;"><i>A día 01/03/2020, los productos (término NEGOCIO) definidos en Xpert son 'AGS','ELEVACION','HOIST','LIGHTWEIGHT','PALANCAS','PEDALES','SHIFTER' y 'TERMOSOLAR'. El sistema no reconocerá productos distintos a estos</i></span>
            <input id="btn-submit" type="submit" value="Crear" Class="btn btn-info" style="font-size:18px;" />
        </div>
    </div>
End Using

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section
