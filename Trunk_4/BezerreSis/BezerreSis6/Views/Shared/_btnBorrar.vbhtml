@Using (Html.BeginForm("Delete", "OBJETIVOS_PRODUCTO", FormMethod.Post))
    @Html.AntiForgeryToken()
    @Html.Hidden("ID", 1)
    @<button name="btnBorrar_OP" type="submit" Class="btn btn-Default btn-xs" title="Borrar" onclick="return confirm('¿Borrar?');"><span Class="glyphicon glyphicon-remove"></span></button>
End Using
