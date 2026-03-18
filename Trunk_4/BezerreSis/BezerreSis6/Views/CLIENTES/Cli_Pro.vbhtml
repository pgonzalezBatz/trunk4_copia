@ModelType BezerreSis.CLIENTES
@Code
    ViewData("Title") = ""
End Code

<h2>Detalles cliente</h2>
@Using (Html.BeginForm("Guardar", "CLIENTES"))
    @Html.AntiForgeryToken()
    @Html.HiddenFor(Function(model) model.ID)
    @<div>
        <hr />
        <dl Class="dl-horizontal">
            <dt>
                @Html.DisplayNameFor(Function(model) model.NOMBRE)
            </dt>

            <dd>
                @Html.DisplayFor(Function(model) model.NOMBRE)
            </dd>
            <dt>Productos </dt>
            <dd>@Html.DropDownList("lProductos", "Seleccionar uno")</dd>
        </dl>

        <div style="text-align:center">
            <input type="submit" value="Guardar" Class="btn btn-info" style="font-size:18px;"/>
        </div>
    </div>
End Using