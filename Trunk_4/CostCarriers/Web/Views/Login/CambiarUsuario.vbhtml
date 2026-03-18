@Code
    Dim ticket As SabLib.ELL.Ticket = CType(Session("Ticket"), SabLib.ELL.Ticket)
End Code

<script src="~/Scripts/usuarios.js"></script>

<script type="text/javascript">

    $(function () {
        initBusquedaUsuarios("txtUsuario", "hfUsuario", "helperUsuario", "@Url.Action("BuscarUsuarios", "Metadata")");
    })

</script>

<h3><label>@Utils.Traducir("Cambiar usuario")</label></h3>
<hr />

@Using Html.BeginForm("CambiarUsuario", "Login", FormMethod.Post, New With {.class = "form-horizontal"})
    @<div Class="form-group row">
        <label Class="col-sm-12 label label-danger">@Utils.Traducir("This is a super secret menu. If you are not authorized, don't look!!. Your memory will be erased!!.")</label>
    </div> 
        @<div Class="form-group row">
            <label class="col-sm-1 col-form-label">@Utils.Traducir("Nuevo usuario")</label>
            <div class="col-sm-5">
                @Html.TextBox("txtUsuario", String.Empty, New With {.class = "form-control auto-no-seleccionado", .style = "width:100%;"})
                @Html.Hidden("hfUsuario", String.Empty)
                <div id="helperUsuario" style="margin-top: -1px;">
                </div>
            </div>
            <div class="col-sm-2">
                <input type="submit" id="submit" value="@Utils.Traducir("Cambiar")" Class="btn btn-primary" />&nbsp;
            </div>
        </div>
End Using
