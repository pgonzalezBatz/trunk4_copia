@imports web
@Code
'ViewData("Title") = "nuevacapacidad"
'Layout = "~/Views/Shared/sitelayout.vbhtml"
End Code
@section menu1
    @Html.Partial("menu")
End Section

<div class="container-fluid">
    <h3>@h.Traducir("Creación de capacidad")</h3>
    <form action="" method="post" class="form-horizontal">
        @Html.AntiForgeryToken()

        @Html.ValidationSummary()
        <div class="form-group">
            <div class="row">
                <label>@h.Traducir("Nombre")</label>
                @Html.TextBox("nombre", Nothing, New With {.maxlength = 40, .class = "form-control"})
            </div>

            <div class="row">
                <div class="col-sm-3">
                    <input type="submit" value="@h.Traducir("Crear")" class="btn btn-primary btn-block" />
                </div>
                <div class="col-sm-4">
                    @*@Html.ActionLink(h.Traducir("Volver al listado"), "search")*@
                    <a href="@Url.Action("listcapacidades", h.ToRouteValues(Request.QueryString, Nothing))">@h.Traducir("Volver a la lista del proveedor")</a>
                </div>
            </div>
        </div>

    </form>


</div>
