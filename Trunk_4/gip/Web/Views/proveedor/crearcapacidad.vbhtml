@imports web
@*@ModelType Web.proveedor*@

@section title
End section
@section header
@*<link href="//intranet2.batz.es/baliabideorokorrak/jquery-ui-1.11.4.min.css" rel="Stylesheet" type="text/css" />*@
End Section
@section scripts
    @*<script src='//intranet2.batz.es/baliabideorokorrak/jquery-ui-1.11.4.min.js' type="text/javascript"></script>
    <script src='//intranet2.batz.es/baliabideorokorrak/jquery.ui.datepicker-@(h.GetCulture().Split("-")(0)).js' type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('.calendar').datepicker($.datepicker.regional["@h.GetCulture().Split(" - ")(0)"]);
        });
    </script>*@
End Section
<div class="container-fluid">
    <h3>@h.Traducir("Creación de capacidad")</h3>
    <form action="" method="post" class="form-horizontal">
        @Html.AntiForgeryToken()

        @Html.ValidationSummary()
        <div class="form-group">
            <div class="col-sm-2">
                <label>@h.Traducir("Nombre")</label>
                @Html.TextBox("nombre", New With {.maxlength = 15, .class = "form-control"})
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
