@ModelType IEnumerable(Of BezerreSis.PLANTAS)
@Code
    ViewData("Title") = "Reclamaciones de Cliente"
End Code

<br />
<h2>
    @Html.Label("Plantas", htmlAttributes:=New With {.class = "control-label"})
</h2>
<br />
@Using Html.BeginForm("SeleccionPlanta", "Home", method:=FormMethod.Post, htmlAttributes:=New With {.id = "Formulario", .name = "Formulario"})
    @Html.AntiForgeryToken()
    @<div Class="row">
        <div Class="col-md-12">
            <p>
                @Html.DropDownList("ddlPlanta", Nothing, "Seleccionar uno", htmlAttributes:=New With {.Class = "form-control myDropDown", .onchange = "$(this).closest('form').trigger('submit');"})
            </p>
        </div>
    </div>
End Using
